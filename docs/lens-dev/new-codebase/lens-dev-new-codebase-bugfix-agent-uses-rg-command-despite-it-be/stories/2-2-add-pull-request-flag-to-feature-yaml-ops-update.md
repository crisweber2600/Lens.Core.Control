# Story 2.2: Add --pull-request Flag to feature-yaml-ops.py Update

Status: ready-for-dev

## Story

As a **Lens agent**,  
I want `feature-yaml-ops.py update --pull-request <url>` to set `links.pull_request` in feature.yaml,  
so that the PR link is stored in the governance record without a CLI error.

## Acceptance Criteria

1. **Given** I call `feature-yaml-ops.py update --feature-id X --pull-request https://github.com/org/repo/pull/42 --governance-repo Y`, **when** the command executes, **then** it exits 0 and sets `links.pull_request: "https://github.com/org/repo/pull/42"` in the feature.yaml.

2. **And** `feature-yaml-ops.py update --help` lists `--pull-request` in the `update` subcommand options.

3. **And** no regression on existing `--phase`, `--target-repos`, `--docs-path`, and other existing flags.

4. **And** a unit test verifies `--pull-request` sets the correct field and existing flags are unaffected.

## Tasks / Subtasks

- [ ] Task 1: Add `--pull-request` to `update_parser` (AC: #1, #2)
  - [ ] Locate the `update_parser.add_argument(...)` block (around line 557)
  - [ ] Add: `update_parser.add_argument("--pull-request", required=False, help="PR URL to set in feature.yaml links.pull_request")`
- [ ] Task 2: Wire `--pull-request` into the `update` command handler (AC: #1)
  - [ ] In the `update` command body, add: `if args.pull_request: feature_data["links"]["pull_request"] = args.pull_request`
  - [ ] Ensure `links` key is initialized if it doesn't exist yet (`feature_data.setdefault("links", {})`)
- [ ] Task 3: Write unit test (AC: #4)
  - [ ] Test: `--pull-request https://...` results in `feature_data["links"]["pull_request"] == "https://..."`
  - [ ] Test: existing flags (`--phase`, `--target-repos`) still function without `--pull-request`

## Dev Notes

**Target file:** `TargetProjects/lens-dev/new-codebase/lens.core.src/skills/lens-feature-yaml/scripts/feature-yaml-ops.py`

**Argument parser addition (from tech-plan B4):**
```python
update_parser.add_argument(
    "--pull-request",
    required=False,
    help="PR URL to set in feature.yaml links.pull_request"
)
```

**Handler logic (from tech-plan B4):**
```python
if args.pull_request:
    feature_data.setdefault("links", {})
    feature_data["links"]["pull_request"] = args.pull_request
```

**Key constraint:** `links.pull_request` field already exists in the `feature.yaml` schema — no schema migration needed. The fix is purely additive CLI plumbing.

**Locate existing code:** Search for `update_parser` and `add_argument` in `feature-yaml-ops.py` to find the block where the new argument should be added.

### Project Structure Notes

- Source repo path: `TargetProjects/lens-dev/new-codebase/lens.core.src/`
- Script path: `skills/lens-feature-yaml/scripts/feature-yaml-ops.py`
- Purely additive change — no existing arguments touched

### References

- [Source: docs/...tech-plan.md#B4] B4 — exact argument name, `required=False`, handler pattern
- [Source: docs/...epics.md#Story 2.2] AC specifications

## Dev Agent Record

### Agent Model Used

_to be filled by dev agent_

### Debug Log References

### Completion Notes List

### File List

- `TargetProjects/lens-dev/new-codebase/lens.core.src/skills/lens-feature-yaml/scripts/feature-yaml-ops.py`
