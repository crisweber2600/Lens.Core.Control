#!/usr/bin/env -S uv run --script
# /// script
# requires-python = ">=3.10"
# dependencies = ["pyyaml"]
# ///
"""Lessons-learned operations — format, filter, and list task-level micro-lessons.

Lessons are stored as machine-readable YAML at /memories/lessons-learned.yaml via
the agent memory tool. This script handles deterministic data operations only;
the agent handles all I/O via the memory tool.

Subcommands:
  format   Format a new lesson entry as YAML and print to stdout
  filter   Read lessons YAML from stdin; print matching entries as YAML
  list     Read lessons YAML from stdin; print a compact summary table
  validate Read lessons YAML from stdin; check structure and print issues

Usage:
  lessons.py format  --task-type TYPE --tags tag1,tag2 --context TEXT
                     --lesson TEXT [--source-skill NAME] [--severity LEVEL]
  lessons.py filter  [--task-type TYPE] [--tags tag1,tag2] [--keywords word1,word2]
                     [--severity LEVEL] [--limit N]
  lessons.py list    [--task-type TYPE] [--limit N]
  lessons.py validate
"""

import argparse
import json
import re
import sys
from datetime import datetime, timezone
from typing import Optional

import yaml

VALID_SEVERITIES = ("tip", "warning", "critical")
# Task-type examples for documentation; not an exhaustive enum.
SEVERITY_DEFAULT = "tip"

ID_PATTERN = re.compile(r"^lesson-\d{8}T\d{6}\d+Z$")


# --------------------------------------------------------------------------- #
# Helpers
# --------------------------------------------------------------------------- #


def _now_id() -> str:
    """Generate a unique lesson ID using microsecond-precision UTC timestamp."""
    return "lesson-" + datetime.now(timezone.utc).strftime("%Y%m%dT%H%M%S%fZ")


def _parse_tags(tag_str: str) -> list[str]:
    """Split a comma-separated tag string into a cleaned list."""
    return [t.strip() for t in tag_str.split(",") if t.strip()]


def _read_lessons_yaml(stream=None) -> dict:
    """Parse YAML from stream (or stdin). Return dict with 'lessons' list."""
    raw = (stream or sys.stdin).read()
    if not raw.strip():
        return {"lessons": []}
    data = yaml.safe_load(raw)
    if not isinstance(data, dict):
        return {"lessons": []}
    if "lessons" not in data:
        data["lessons"] = []
    return data


def _matches(entry: dict, task_type: Optional[str], tags: list[str], keywords: list[str], severity: Optional[str]) -> bool:
    """Return True if entry matches all supplied filter criteria."""
    if task_type and entry.get("task_type", "").lower() != task_type.lower():
        return False
    if severity and entry.get("severity", "").lower() != severity.lower():
        return False
    if tags:
        entry_tags = [t.lower() for t in entry.get("tags", [])]
        if not all(t.lower() in entry_tags for t in tags):
            return False
    if keywords:
        haystack = " ".join([
            entry.get("task_type", ""),
            entry.get("context", ""),
            entry.get("lesson", ""),
            " ".join(entry.get("tags", [])),
        ]).lower()
        if not all(kw.lower() in haystack for kw in keywords):
            return False
    return True


# --------------------------------------------------------------------------- #
# Subcommand: format
# --------------------------------------------------------------------------- #


def cmd_format(args: argparse.Namespace) -> int:
    """Format a new lesson entry as YAML. Use --json for structured JSON output."""
    if "/" in args.task_type or ".." in args.task_type:
        print("ERROR: task-type must not contain path separators.", file=sys.stderr)
        return 1

    tags = _parse_tags(args.tags) if args.tags else []
    severity = args.severity.lower() if args.severity else SEVERITY_DEFAULT
    if severity not in VALID_SEVERITIES:
        print(
            f"ERROR: severity must be one of {VALID_SEVERITIES}",
            file=sys.stderr,
        )
        return 1

    entry = {
        "id": _now_id(),
        "date": datetime.now(timezone.utc).strftime("%Y-%m-%dT%H:%M:%SZ"),
        "task_type": args.task_type,
        "tags": tags,
        "context": args.context,
        "lesson": args.lesson,
        "source_skill": args.source_skill or "",
        "severity": severity,
    }

    # Emit only the entry block; the agent merges it into lessons-learned.yaml
    if getattr(args, "json", False):
        print(json.dumps(entry, ensure_ascii=False, indent=2))
    else:
        print(yaml.dump(entry, sort_keys=False, allow_unicode=True, default_flow_style=False), end="")
    return 0


# --------------------------------------------------------------------------- #
# Subcommand: filter
# --------------------------------------------------------------------------- #


def cmd_filter(args: argparse.Namespace) -> int:
    """Filter lessons from stdin. Use --json for structured JSON output."""
    data = _read_lessons_yaml()
    tags = _parse_tags(args.tags) if args.tags else []
    keywords = _parse_tags(args.keywords) if args.keywords else []
    severity = args.severity.lower() if args.severity else None
    task_type = args.task_type or None

    matched = [
        e for e in data.get("lessons", [])
        if _matches(e, task_type, tags, keywords, severity)
    ]

    if args.limit and args.limit > 0:
        matched = matched[-args.limit:]  # most recent N

    result = {"lessons": matched}
    if not matched:
        if getattr(args, "json", False):
            print(json.dumps(result, ensure_ascii=False))
        else:
            print("lessons: []")
        return 0

    if getattr(args, "json", False):
        print(json.dumps(result, ensure_ascii=False, indent=2))
    else:
        print(yaml.dump(result, sort_keys=False, allow_unicode=True, default_flow_style=False), end="")
    return 0


# --------------------------------------------------------------------------- #
# Subcommand: list
# --------------------------------------------------------------------------- #


def cmd_list(args: argparse.Namespace) -> int:
    """Print a compact summary table of stored lessons."""
    data = _read_lessons_yaml()
    lessons = data.get("lessons", [])

    task_type = args.task_type or None
    if task_type:
        lessons = [e for e in lessons if e.get("task_type", "").lower() == task_type.lower()]

    if args.limit and args.limit > 0:
        lessons = lessons[-args.limit:]

    if not lessons:
        print("No lessons found.")
        return 0

    col_id = 30
    col_type = 22
    col_sev = 10
    col_lesson = 55

    header = (
        f"{'ID':<{col_id}}  {'TASK_TYPE':<{col_type}}  {'SEV':<{col_sev}}  LESSON"
    )
    sep = "-" * len(header)
    print(header)
    print(sep)
    for e in lessons:
        lesson_preview = (e.get("lesson", "") or "")[:col_lesson]
        print(
            f"{e.get('id',''):<{col_id}}  "
            f"{e.get('task_type',''):<{col_type}}  "
            f"{e.get('severity',''):<{col_sev}}  "
            f"{lesson_preview}"
        )
    print(f"\n{len(lessons)} lesson(s)")
    return 0


# --------------------------------------------------------------------------- #
# Subcommand: validate
# --------------------------------------------------------------------------- #


def cmd_validate(args: argparse.Namespace) -> int:
    """Validate lessons YAML structure from stdin. Print issues found."""
    data = _read_lessons_yaml()
    lessons = data.get("lessons", [])
    required_fields = ("id", "date", "task_type", "context", "lesson", "severity")
    issues: list[str] = []

    for i, entry in enumerate(lessons):
        prefix = f"lessons[{i}] id={entry.get('id', '(missing)')}"
        for field in required_fields:
            if not entry.get(field):
                issues.append(f"{prefix}: missing required field '{field}'")
        sev = entry.get("severity", "")
        if sev and sev not in VALID_SEVERITIES:
            issues.append(f"{prefix}: invalid severity '{sev}', must be one of {VALID_SEVERITIES}")
        if "tags" in entry and not isinstance(entry["tags"], list):
            issues.append(f"{prefix}: 'tags' must be a list")

    if issues:
        print(f"VALIDATION FAILED — {len(issues)} issue(s):")
        for issue in issues:
            print(f"  - {issue}")
        return 1

    print(f"OK — {len(lessons)} lesson(s) validated, no issues.")
    return 0


# --------------------------------------------------------------------------- #
# CLI
# --------------------------------------------------------------------------- #


def build_parser() -> argparse.ArgumentParser:
    parser = argparse.ArgumentParser(
        description="Lessons-learned YAML operations for BMad lens-work agents.",
        formatter_class=argparse.RawDescriptionHelpFormatter,
        epilog=__doc__,
    )
    sub = parser.add_subparsers(dest="command", required=True)

    # format
    p_fmt = sub.add_parser("format", help="Format a new lesson entry as YAML.")
    p_fmt.add_argument("--task-type", required=True, help="Short type identifier, e.g. git-merge")
    p_fmt.add_argument("--tags", default="", help="Comma-separated tags, e.g. git,conflict")
    p_fmt.add_argument("--context", required=True, help="1-2 sentence description of when this applies.")
    p_fmt.add_argument("--lesson", required=True, help="The core actionable insight.")
    p_fmt.add_argument("--source-skill", default="", help="Skill that invoked the log.")
    p_fmt.add_argument("--severity", default=SEVERITY_DEFAULT, choices=VALID_SEVERITIES, help="Lesson severity.")
    p_fmt.add_argument("--json", action="store_true", help="Output as JSON instead of YAML.")

    # filter
    p_flt = sub.add_parser("filter", help="Filter lessons from stdin. Input: lessons YAML.")
    p_flt.add_argument("--task-type", default="", help="Filter by task type (exact, case-insensitive).")
    p_flt.add_argument("--tags", default="", help="Comma-separated tags; ALL must match.")
    p_flt.add_argument("--keywords", default="", help="Comma-separated keywords; ALL must appear in text fields.")
    p_flt.add_argument("--severity", default="", help="Filter by severity level.")
    p_flt.add_argument("--limit", type=int, default=0, help="Return at most N most-recent matches.")
    p_flt.add_argument("--json", action="store_true", help="Output as JSON instead of YAML.")

    # list
    p_lst = sub.add_parser("list", help="Compact summary table. Input: lessons YAML via stdin.")
    p_lst.add_argument("--task-type", default="", help="Filter by task type.")
    p_lst.add_argument("--limit", type=int, default=0, help="Limit number of rows.")

    # validate
    sub.add_parser("validate", help="Validate structure of lessons YAML from stdin.")

    return parser


def main() -> int:
    parser = build_parser()
    args = parser.parse_args()

    dispatch = {
        "format": cmd_format,
        "filter": cmd_filter,
        "list": cmd_list,
        "validate": cmd_validate,
    }
    return dispatch[args.command](args)


if __name__ == "__main__":
    sys.exit(main())
