"""Unit tests for lessons.py — covers format, filter, list, and validate subcommands."""

import io
import sys
import unittest
import yaml
from pathlib import Path
from unittest.mock import patch

# Add script directory to path so we can import from it
sys.path.insert(0, str(Path(__file__).parent.parent))

from lessons import (
    _now_id,
    _parse_tags,
    _matches,
    _read_lessons_yaml,
    cmd_format,
    cmd_filter,
    cmd_list,
    cmd_validate,
    build_parser,
)

# --------------------------------------------------------------------------- #
# Helpers
# --------------------------------------------------------------------------- #

SAMPLE_YAML = """
lessons:
  - id: lesson-20260101T120000000000Z
    date: "2026-01-01T12:00:00Z"
    task_type: git-merge
    tags: [git, conflict]
    context: When merging long-lived feature branches into main.
    lesson: Always rebase onto main before opening a PR to minimize merge conflicts.
    source_skill: bmad-lens-dev
    severity: warning
  - id: lesson-20260202T130000000000Z
    date: "2026-02-02T13:00:00Z"
    task_type: code-review
    tags: [review, security]
    context: When reviewing code that handles user input.
    lesson: Check for input validation at system boundaries before approving.
    source_skill: bmad-lens-dev
    severity: critical
  - id: lesson-20260303T140000000000Z
    date: "2026-03-03T14:00:00Z"
    task_type: git-merge
    tags: [git, migration]
    context: When migrating database schema in a PR.
    lesson: Run migration dry-run in staging before merging schema changes.
    source_skill: bmad-lens-techplan
    severity: warning
"""


def _make_stdin(content: str):
    return io.StringIO(content)


def _run_cmd(argv: list[str], stdin_content: str = "") -> tuple[int, str, str]:
    """Run a CLI command and capture stdout/stderr."""
    parser = build_parser()
    args = parser.parse_args(argv)

    captured_out = io.StringIO()
    captured_err = io.StringIO()
    stdin = io.StringIO(stdin_content)

    with patch("sys.stdout", captured_out), patch("sys.stderr", captured_err):
        dispatch = {
            "format": cmd_format,
            "filter": cmd_filter,
            "list": cmd_list,
            "validate": cmd_validate,
        }
        rc = dispatch[args.command](args)

    return rc, captured_out.getvalue(), captured_err.getvalue()


# --------------------------------------------------------------------------- #
# Tests: _helpers
# --------------------------------------------------------------------------- #


class TestHelpers(unittest.TestCase):
    def test_now_id_format(self):
        id_ = _now_id()
        self.assertTrue(id_.startswith("lesson-"), id_)
        self.assertIn("T", id_)

    def test_parse_tags_basic(self):
        self.assertEqual(_parse_tags("a,b,c"), ["a", "b", "c"])

    def test_parse_tags_strips_whitespace(self):
        self.assertEqual(_parse_tags("git , conflict , "), ["git", "conflict"])

    def test_parse_tags_empty(self):
        self.assertEqual(_parse_tags(""), [])

    def test_matches_all_criteria(self):
        entry = {
            "task_type": "git-merge",
            "tags": ["git", "conflict"],
            "context": "When merging branches.",
            "lesson": "Always rebase first.",
            "severity": "warning",
        }
        self.assertTrue(_matches(entry, "git-merge", ["git"], [], "warning"))

    def test_matches_no_criteria(self):
        entry = {"task_type": "git-merge", "tags": ["git"], "context": "", "lesson": "", "severity": "tip"}
        self.assertTrue(_matches(entry, None, [], [], None))

    def test_matches_task_type_mismatch(self):
        entry = {"task_type": "code-review", "tags": [], "context": "", "lesson": "", "severity": "tip"}
        self.assertFalse(_matches(entry, "git-merge", [], [], None))

    def test_matches_tag_partial_fail(self):
        entry = {"task_type": "x", "tags": ["git"], "context": "", "lesson": "", "severity": "tip"}
        self.assertFalse(_matches(entry, None, ["git", "security"], [], None))

    def test_matches_keyword_in_lesson(self):
        entry = {"task_type": "x", "tags": [], "context": "", "lesson": "Always rebase first", "severity": "tip"}
        self.assertTrue(_matches(entry, None, [], ["rebase"], None))

    def test_matches_keyword_not_found(self):
        entry = {"task_type": "x", "tags": [], "context": "normal context", "lesson": "normal lesson", "severity": "tip"}
        self.assertFalse(_matches(entry, None, [], ["nonexistent"], None))

    def test_read_lessons_yaml_empty(self):
        result = _read_lessons_yaml(_make_stdin(""))
        self.assertEqual(result, {"lessons": []})

    def test_read_lessons_yaml_valid(self):
        result = _read_lessons_yaml(_make_stdin(SAMPLE_YAML))
        self.assertEqual(len(result["lessons"]), 3)


# --------------------------------------------------------------------------- #
# Tests: format
# --------------------------------------------------------------------------- #


class TestFormat(unittest.TestCase):
    def test_format_basic_output(self):
        rc, out, err = _run_cmd([
            "format",
            "--task-type", "git-merge",
            "--context", "When merging branches.",
            "--lesson", "Rebase before PR.",
        ])
        self.assertEqual(rc, 0, err)
        entry = yaml.safe_load(out)
        self.assertEqual(entry["task_type"], "git-merge")
        self.assertEqual(entry["lesson"], "Rebase before PR.")
        self.assertTrue(entry["id"].startswith("lesson-"))
        self.assertEqual(entry["severity"], "tip")

    def test_format_with_tags(self):
        rc, out, _ = _run_cmd([
            "format",
            "--task-type", "code-review",
            "--tags", "git,conflict",
            "--context", "ctx",
            "--lesson", "check input",
        ])
        entry = yaml.safe_load(out)
        self.assertEqual(entry["tags"], ["git", "conflict"])

    def test_format_severity_critical(self):
        rc, out, _ = _run_cmd([
            "format",
            "--task-type", "deploy",
            "--context", "ctx",
            "--lesson", "test in staging",
            "--severity", "critical",
        ])
        entry = yaml.safe_load(out)
        self.assertEqual(entry["severity"], "critical")

    def test_format_source_skill(self):
        rc, out, _ = _run_cmd([
            "format",
            "--task-type", "deploy",
            "--context", "ctx",
            "--lesson", "test in staging",
            "--source-skill", "bmad-lens-dev",
        ])
        entry = yaml.safe_load(out)
        self.assertEqual(entry["source_skill"], "bmad-lens-dev")

    def test_format_rejects_path_traversal_task_type(self):
        rc, out, err = _run_cmd([
            "format",
            "--task-type", "../../../etc/passwd",
            "--context", "ctx",
            "--lesson", "should fail",
        ])
        self.assertEqual(rc, 1)
        self.assertIn("ERROR", err)

    def test_format_required_fields(self):
        with self.assertRaises(SystemExit):
            build_parser().parse_args(["format", "--task-type", "x"])


# --------------------------------------------------------------------------- #
# Tests: filter
# --------------------------------------------------------------------------- #


class TestFilter(unittest.TestCase):
    def _filter(self, argv, stdin=SAMPLE_YAML):
        """Run filter command with sample YAML injected via stdin mock."""
        parser = build_parser()
        args = parser.parse_args(["filter"] + argv)

        captured_out = io.StringIO()
        captured_err = io.StringIO()

        with patch("sys.stdout", captured_out), patch("sys.stderr", captured_err), \
             patch("sys.stdin", io.StringIO(stdin)):
            rc = cmd_filter(args)

        return rc, captured_out.getvalue(), captured_err.getvalue()

    def test_filter_by_task_type(self):
        rc, out, _ = self._filter(["--task-type", "git-merge"])
        data = yaml.safe_load(out)
        self.assertEqual(len(data["lessons"]), 2)
        for e in data["lessons"]:
            self.assertEqual(e["task_type"], "git-merge")

    def test_filter_by_tag(self):
        rc, out, _ = self._filter(["--tags", "security"])
        data = yaml.safe_load(out)
        self.assertEqual(len(data["lessons"]), 1)
        self.assertIn("security", data["lessons"][0]["tags"])

    def test_filter_by_keyword(self):
        rc, out, _ = self._filter(["--keywords", "rebase"])
        data = yaml.safe_load(out)
        self.assertGreaterEqual(len(data["lessons"]), 1)
        # At least one should contain "rebase"
        texts = [e.get("lesson", "") + e.get("context", "") for e in data["lessons"]]
        self.assertTrue(any("rebase" in t.lower() for t in texts))

    def test_filter_by_severity(self):
        rc, out, _ = self._filter(["--severity", "critical"])
        data = yaml.safe_load(out)
        self.assertEqual(len(data["lessons"]), 1)
        self.assertEqual(data["lessons"][0]["severity"], "critical")

    def test_filter_no_match_returns_empty(self):
        rc, out, _ = self._filter(["--task-type", "nonexistent-task"])
        self.assertIn("lessons: []", out)

    def test_filter_limit(self):
        rc, out, _ = self._filter(["--task-type", "git-merge", "--limit", "1"])
        data = yaml.safe_load(out)
        self.assertEqual(len(data["lessons"]), 1)

    def test_filter_empty_input(self):
        rc, out, _ = self._filter([], stdin="")
        self.assertIn("lessons: []", out)


# --------------------------------------------------------------------------- #
# Tests: list
# --------------------------------------------------------------------------- #


class TestList(unittest.TestCase):
    def _list(self, argv, stdin=SAMPLE_YAML):
        parser = build_parser()
        args = parser.parse_args(["list"] + argv)

        captured_out = io.StringIO()
        with patch("sys.stdout", captured_out), patch("sys.stdin", io.StringIO(stdin)):
            rc = cmd_list(args)

        return rc, captured_out.getvalue()

    def test_list_all(self):
        rc, out = self._list([])
        self.assertEqual(rc, 0)
        self.assertIn("3 lesson(s)", out)
        self.assertIn("ID", out)  # header row

    def test_list_filter_task_type(self):
        rc, out = self._list(["--task-type", "code-review"])
        self.assertIn("1 lesson(s)", out)

    def test_list_limit(self):
        rc, out = self._list(["--limit", "2"])
        self.assertIn("2 lesson(s)", out)

    def test_list_empty(self):
        rc, out = self._list([])
        # No crash with valid data
        self.assertEqual(rc, 0)

    def test_list_no_match(self):
        rc, out = self._list(["--task-type", "nope"])
        self.assertIn("No lessons found", out)


# --------------------------------------------------------------------------- #
# Tests: validate
# --------------------------------------------------------------------------- #


class TestValidate(unittest.TestCase):
    def _validate(self, stdin):
        parser = build_parser()
        args = parser.parse_args(["validate"])

        captured_out = io.StringIO()
        with patch("sys.stdout", captured_out), patch("sys.stdin", io.StringIO(stdin)):
            rc = cmd_validate(args)

        return rc, captured_out.getvalue()

    def test_validate_valid(self):
        rc, out = self._validate(SAMPLE_YAML)
        self.assertEqual(rc, 0)
        self.assertIn("OK", out)

    def test_validate_missing_field(self):
        bad_yaml = """
lessons:
  - id: lesson-20260101T120000000000Z
    date: "2026-01-01T12:00:00Z"
    task_type: git-merge
    tags: [git]
    severity: tip
"""
        # Missing 'context' and 'lesson'
        rc, out = self._validate(bad_yaml)
        self.assertEqual(rc, 1)
        self.assertIn("VALIDATION FAILED", out)

    def test_validate_invalid_severity(self):
        bad_yaml = """
lessons:
  - id: lesson-20260101T120000000000Z
    date: "2026-01-01T12:00:00Z"
    task_type: x
    tags: []
    context: ctx
    lesson: test
    severity: extreme
"""
        rc, out = self._validate(bad_yaml)
        self.assertEqual(rc, 1)
        self.assertIn("invalid severity", out)

    def test_validate_empty(self):
        rc, out = self._validate("")
        self.assertEqual(rc, 0)
        self.assertIn("OK", out)

    def test_validate_tags_not_list(self):
        bad_yaml = """
lessons:
  - id: lesson-20260101T120000000000Z
    date: "2026-01-01T12:00:00Z"
    task_type: x
    tags: "not-a-list"
    context: ctx
    lesson: test
    severity: tip
"""
        rc, out = self._validate(bad_yaml)
        self.assertEqual(rc, 1)
        self.assertIn("tags", out)


if __name__ == "__main__":
    unittest.main()
