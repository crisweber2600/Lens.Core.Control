---
feature: lens-dev-new-codebase-constitution
doc_type: tech-plan
status: draft
goal: "Define the technical design for rewriting bmad-lens-constitution as a thin script-backed conductor: 3-hop command chain, partial-hierarchy tolerance in constitution-ops.py, express-track parity, preserved merge rules for all three subcommands, and regression coverage for partial and full hierarchies."
key_decisions:
  - Constitution command follows the thin-conductor pattern; SKILL.md delegates all resolution logic to constitution-ops.py
  - Partial-hierarchy fix: remove org-level hard-fail; replace with informational warning + defaults continuation
  - Express-track parity is part of the shared contract: `express` must be a supported governed track when constitutions permit it
  - All merge rules preserved identically to old codebase (intersection, union, strongest-wins, etc.)
  - SKILL.md authored via BMB; release prompt authored via bmad-workflow-builder; stub chain verified only
  - Regression coverage required before merge: partial-hierarchy, additive-merge, all three subcommands
open_questions: []
depends_on:
  - business-plan.md (this feature)
  - lens-dev-new-codebase-baseline
blocks: []
updated_at: 2026-05-01T00:00:00Z
---

# Technical Plan — Rewrite constitution Command

**Feature:** lens-dev-new-codebase-constitution  
**Author:** crisweber2600  
**Date:** 2026-05-01  
**References:** [Business Plan](./business-plan.md), [Baseline Research](../../lens-dev-new-codebase-baseline/research.md)

---

## 1. System Design

The `constitution` command follows the invariant 3-hop command resolution chain:

```
.github/prompts/lens-constitution.prompt.md          (stub — user entry point)
  → lens.core/_bmad/lens-work/prompts/
        lens-constitution.prompt.md                  (release prompt — loads SKILL.md)
    → skills/bmad-lens-constitution/SKILL.md         (conductor — delegates to script)
      → scripts/constitution-ops.py {subcommand}     (script backend — all resolution logic)
```

The SKILL.md is a **thin conductor**. It accepts the user's subcommand intent, marshals the argument set, calls `constitution-ops.py`, and formats the result for the agent session. It contains no resolution logic inline.

### 1.1 Authority Domain

`bmad-lens-constitution` is a **read-only shared runtime primitive**. Its authority domain is:

| Operation | Permitted | Notes |
|-----------|-----------|-------|
| Read governance repo constitution files | YES | Normal operation |
| Read feature.yaml for compliance check | YES | check-compliance subcommand |
| Read artifacts path for artifact existence | YES | check-compliance artifact gate |
| Write any governance artifact | NO | Hard constraint — never relaxed |
| Write feature state | NO | Hard constraint — never relaxed |
| Write control repo | NO | Hard constraint — never relaxed |

---

## 2. constitution-ops.py Design

### 2.1 Script Header

```python
#!/usr/bin/env -S uv run --script
# /// script
# requires-python = ">=3.11"
# dependencies = ["pyyaml"]
# ///
"""
constitution-ops.py — Governance constitution resolution for Lens.
All subcommands write JSON to stdout. Exit codes: 0=success, 1=error, 2=compliance failure.
"""
```

### 2.2 DEFAULTS

The following defaults apply when a constitution level is absent or when no levels are loaded:

```python
DEFAULTS = {
    "permitted_tracks": ["quickplan", "full", "express", "hotfix", "tech-change"],
    "required_artifacts": {
        "planning": ["business-plan", "tech-plan"],
        "dev": ["stories"],
    },
    "gate_mode": "informational",
    "sensing_gate_mode": "informational",
    "additional_review_participants": [],
    "enforce_stories": False,
    "enforce_review": False,
}
```

The rewritten script's supported-track allow-list must also include `express`. This is a clean-room correction to old drift, not a downstream workaround: lifecycle.yaml and the baseline expressplan story both treat express as a retained route, so constitution output must be able to validate it.

### 2.3 Constitution File Format

Constitution files are Markdown files with YAML frontmatter:

```
{governance_repo}/constitutions/org/constitution.md
{governance_repo}/constitutions/{domain}/constitution.md
{governance_repo}/constitutions/{domain}/{service}/constitution.md
{governance_repo}/constitutions/{domain}/{service}/{repo}/constitution.md
```

The YAML frontmatter contains zero or more of the following known keys:
- `permitted_tracks` — list of allowed track names
- `required_artifacts` — dict mapping phase names to artifact name lists
- `gate_mode` — one of: `informational`, `hard`
- `sensing_gate_mode` — one of: `informational`, `hard`
- `additional_review_participants` — list of reviewer identifiers
- `enforce_stories` — boolean
- `enforce_review` — boolean

Unknown keys are flagged in the return payload with `_unknown_keys` but do not cause errors.

### 2.4 load_constitution(path) → dict | None

Reads a constitution file and returns its frontmatter as a dict. Returns `None` if the file does not exist (allowing callers to distinguish a missing file from a file with empty frontmatter). Returns `{"_parse_error": str}` if the YAML frontmatter is malformed. Records `_unknown_keys` for any key not in the known set.

Parsing rule: the frontmatter delimiter is the first `---` pair only. A `---` sequence inside a YAML value is not treated as a closing delimiter.

### 2.5 merge_constitutions(levels: list[dict]) → tuple[dict, list[dict]]

Merges a list of constitution dicts in order from highest (org) to lowest (repo). Returns `(merged_constitution, warnings)`, where `warnings` is a list of structured warning objects (dicts with keys such as `type` and `detail`).

Merge rules applied in order:

| Field | Rule | Rationale |
|-------|------|-----------|
| `permitted_tracks` | **Intersection** of all levels that specify this field | Lower levels can only restrict; they cannot expand permitted tracks |
| `required_artifacts` | **Union** per phase bucket, deduplicated | Any level can add requirements; none can remove them |
| `gate_mode` | **Strongest wins** — `hard` overrides `informational` | Most restrictive gate at any level applies |
| `sensing_gate_mode` | **Strongest wins** (if present) | Same rationale as gate_mode |
| `additional_review_participants` | **Union**, deduplicated | Any level can add reviewers |
| `enforce_stories` | **True wins** — `true` overrides `false` | Once enforcement is required at any level, it applies everywhere |
| `enforce_review` | **True wins** — `true` overrides `false` | Same rationale as enforce_stories |

When `permitted_tracks` intersection resolves to an empty list, append a warning:
```json
{"type": "empty_permitted_tracks", "detail": "No tracks permitted after intersection"}
```

When `levels` is empty, return DEFAULTS and append a warning:
```json
{"type": "no_levels_loaded", "detail": "No constitution levels found; using defaults"}
```

### 2.6 cmd_resolve — Partial-Hierarchy Fix

The `resolve` subcommand builds the constitution hierarchy path list and loads each level in order. The **critical behavioral change** from the old codebase is that a missing level is skipped with a warning — not returned as an error.

**Old behavior (to eliminate):**
```python
if "org" not in levels_loaded:
    return {"error": "org_constitution_missing", ...}, 1  # HARD FAIL — removed
```

**New behavior:**
```python
for level_name, path in level_paths:
    data = load_constitution(path)
    if data is None:
        warnings.append({"type": "level_absent", "level": level_name,
                          "path": str(path)})
        continue  # skip missing levels gracefully
    levels.append(data)
    levels_loaded.append(level_name)
```

Resolution proceeds with whatever levels were successfully loaded. If `levels_loaded` is empty, `merge_constitutions([])` returns DEFAULTS with a `no_levels_loaded` warning.

**Output structure:**
```json
{
  "domain": "...",
  "service": "...",
  "levels_loaded": ["org", "domain"],
  "resolved_constitution": { ...merged fields... },
  "warnings": [
    {"type": "level_absent", "level": "service", "path": "..."}
  ]
}
```

Exit code is always `0` for a valid resolve invocation (including partial hierarchy). Exit code `1` is reserved for argument errors and unreadable paths.

### 2.7 cmd_check_compliance

The `check-compliance` subcommand:

1. Calls `cmd_resolve` internally to get the effective constitution for the feature's domain/service/repo scope
2. Reads `feature.yaml` from the provided local path
3. Checks all enabled compliance gates and records each as a pass/fail entry

Compliance checks performed:

| Check | Condition | Gate |
|-------|-----------|------|
| Track permitted | `feature.yaml.track` in `permitted_tracks` | gate_mode value |
| Required artifacts present | Each artifact in `required_artifacts[phase]` exists on disk | gate_mode value |
| Reviewers configured | If `enforce_review=true`, `additional_review_participants` is non-empty | gate_mode value |
| Stories enforced | If `enforce_stories=true`, dev-phase stories file exists | gate_mode value |

**Output structure:**
```json
{
  "feature_id": "...",
  "phase": "...",
  "constitution_scope": {"domain": "...", "service": "...", "levels_loaded": [...]},
  "compliance_summary": "PASS" | "FAIL",
  "checks": [
    {"requirement": "...", "status": "PASS" | "FAIL", "gate": "informational" | "hard", "detail": "..."}
  ],
  "hard_failures": [...],
  "informational_failures": [...]
}
```

Exit code: `0` = all pass or informational-only failures; `2` = any hard gate failure; `1` = script error.

### 2.8 cmd_progressive_display

The `progressive-display` subcommand returns a context-filtered view of the resolved constitution for a given domain/service/repo scope, optionally filtered by phase and track:

1. Calls `cmd_resolve` internally
2. Extracts the display-relevant fields from the resolved constitution
3. If `--phase` provided: includes `required_artifacts_for_phase` for that phase bucket
4. If `--track` provided: includes `track_permitted` and `permitted_tracks` 
5. Propagates any warnings from resolve

**Output structure:**
```json
{
  "domain": "...",
  "service": "...",
  "levels_loaded": [...],
  "gate_mode": "informational" | "hard",
  "additional_review_participants": [...],
  "enforce_stories": false,
  "enforce_review": false,
  "full_constitution_available": true | false,
  "required_artifacts_for_phase": [...],
  "track_permitted": true | false,
  "permitted_tracks": [...],
  "warnings": [...]
}
```

`full_constitution_available` is `true` only when `org` is in `levels_loaded`. This preserves the semantic value of the flag used by callers that distinguish full-hierarchy from partial-hierarchy deployments.

---

## 3. Files to Create / Modify

| File | Action | Authoring Channel |
|------|--------|-------------------|
| `lens.core/_bmad/lens-work/skills/bmad-lens-constitution/SKILL.md` | Rewrite | BMB (`bmad-module-builder`) |
| `lens.core/_bmad/lens-work/prompts/lens-constitution.prompt.md` | Rewrite | `bmad-workflow-builder` |
| `.github/prompts/lens-constitution.prompt.md` | Verify stub chain (no content change expected) | Manual verify only |
| `lens.core/_bmad/lens-work/skills/bmad-lens-constitution/scripts/constitution-ops.py` | Rewrite | Direct implementation |
| `lens.core/_bmad/lens-work/skills/bmad-lens-constitution/scripts/tests/test-constitution-ops.py` | Rewrite + extend | Direct implementation |
| `lens.core/_bmad/lens-work/skills/bmad-lens-constitution/references/resolve-rules.md` | Verify/update | Review against new design |
| `lens.core/_bmad/lens-work/skills/bmad-lens-constitution/references/validate-compliance.md` | Verify/update | Review against new design |
| `lens.core/_bmad/lens-work/skills/bmad-lens-constitution/references/progressive-display.md` | Verify/update | Review against new design |

### 3.1 SKILL.md Conductor Structure

The new SKILL.md contains:
- **Capability declarations**: resolve, check-compliance, progressive-display
- **Invocation contract**: argument shapes for each subcommand
- **Delegation instruction**: explicit call to `constitution-ops.py {subcommand} {args}`
- **Output formatting guidance**: how to present JSON results to the user
- **Authority domain declaration**: read-only, no writes permitted
- **Integration notes**: which callers use which subcommand and when

No merge logic, no YAML parsing, no file I/O inline.

---

## 4. Regression Strategy

### 4.1 Regression Classes Required

| Class | Coverage Goal | Example Tests |
|-------|---------------|---------------|
| `partial-hierarchy` | Missing any combination of levels resolves without error | org-missing, domain-missing, both missing, all missing |
| `additive-merge` | Lower levels add constraints on top of higher levels | service adds required artifact; domain restricts tracks |
| `express-track-parity` | Express is preserved as a valid governed track when the hierarchy permits it | express allowed by domain/service passes resolve, compliance, and display |
| `merge-rules` | Each merge rule applied correctly | intersection, union, strongest-wins for each field |
| `check-compliance-gate` | Hard vs informational gate produces correct exit code | hard-fail exits 2, informational-fail exits 0 |
| `progressive-display-filter` | Phase and track filters applied correctly | phase=planning returns planning bucket only |
| `full-constitution-available-flag` | Flag correct for full and partial hierarchies | org-present → true; org-absent → false |
| `path-safety-read-only` | Invalid scopes are rejected and read paths never escape constitutions/ | invalid slug, repo traversal, read-only no-write assertions |
| `load-constitution-edge-cases` | Malformed frontmatter, unknown keys, empty file | parse error recorded, unknown keys flagged |

### 4.2 Test Fixture Pattern

All tests use real temp-directory governance repo structures (not mocks). The `tmp_path` pytest fixture creates an isolated governance repo for each test. The `write_constitution(path, data, prose)` helper writes YAML frontmatter constitution files. This pattern is preserved from the old-codebase test suite design.

### 4.3 New Regression Tests Required (not in old codebase)

| Test | Validates |
|------|-----------|
| `test_org_missing_returns_warning_not_error` | Core partial-hierarchy fix |
| `test_domain_missing_skipped_gracefully` | Partial-hierarchy fix for non-org levels |
| `test_all_levels_missing_returns_defaults` | Empty-hierarchy coverage |
| `test_express_track_allowed_when_hierarchy_permits` | express-track parity in resolve/check-compliance |
| `test_progressive_display_express_track_filter` | express-track parity in progressive-display |
| `test_sensing_gate_mode_strongest_wins` | sensing strictness preserved in merged output |
| `test_org_missing_full_constitution_flag_false` | progressive-display flag accuracy |
| `test_partial_hierarchy_check_compliance` | Compliance works on partial-hierarchy resolve |
| `test_partial_hierarchy_additive_merge` | Merge applies only to present levels |
| `test_invalid_slug_or_traversal_rejected` | Path traversal and malformed scope inputs fail safely |

---

## 5. Acceptance Test Matrix

| Scenario | Subcommand | Expected Outcome | Exit Code |
|----------|------------|-----------------|-----------|
| Full 4-level hierarchy, all permits match | resolve | Merged constitution, no warnings | 0 |
| Org-level missing, domain+service present | resolve | Merged domain+service, warning for org | 0 |
| All levels missing | resolve | DEFAULTS, warning for all levels | 0 |
| Track in permitted_tracks | check-compliance | PASS for track check | 0 |
| Express track permitted in active hierarchy | check-compliance | PASS for express track check | 0 |
| Track not in permitted_tracks, informational gate | check-compliance | FAIL in payload, no hard_failures | 0 |
| Track not in permitted_tracks, hard gate | check-compliance | hard_failures non-empty | 2 |
| Required artifact missing, hard gate | check-compliance | hard_failures non-empty | 2 |
| Progressive-display with phase filter | progressive-display | required_artifacts_for_phase populated | 0 |
| Progressive-display with `track=express` | progressive-display | `track_permitted=true` when express is allowed | 0 |
| Progressive-display, org absent | progressive-display | full_constitution_available=false, warnings present | 0 |
| Invalid slug / traversal attempt | any | error in JSON payload, no path escape | 1 |
| Invalid argument | any | error in JSON payload | 1 |
