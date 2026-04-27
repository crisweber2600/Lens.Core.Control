---
feature: lens-dev-new-codebase-switch
story_id: SW-9
epic: EP-2
sprint: 2
title: Normalize Dependency Context Paths
estimate: S
status: not-started
blocked_by: []
assignee: crisweber2600
doc_type: story
updated_at: 2026-04-27T00:00:00Z
---

# SW-9 — Normalize Dependency Context Paths

## Context

Callers of switch need to know which related documents exist and where to find them, without having to scan governance themselves. Switch must compute context paths for related features, dependencies, and blockers, and report whether each file exists.

**Requirement:** SW-B7

## Task

In `switch-ops.py` switch operation, after loading `feature.yaml`:

1. For each id in `feature.yaml.dependencies.related` (if any): produce a path to `features/{domain}/{service}/{id}/summary.md` in the governance repo.
2. For each id in `feature.yaml.dependencies.depends_on`: produce a path to `docs/{domain}/{service}/{id}/tech-plan.md` in the control repo docs path.
3. For each id in `feature.yaml.dependencies.blocks`: produce a path to `docs/{domain}/{service}/{id}/tech-plan.md`.
4. For each path, check if the file exists: include `exists: true` or `exists: false`.
5. Missing files are not errors — include them with `exists: false` and let callers skip them.
6. Populate `context_paths` in the switch response.

Also in this story: **create `test-switch-ops.py`** at the path referenced in the tech-plan validation section. This file is a prerequisite for SW-12.

## Expected context_paths Shape

```json
{
  "context_paths": {
    "related": [
      {"id": "lens-dev-new-codebase-baseline", "path": "TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-baseline/summary.md", "exists": true}
    ],
    "depends_on": [],
    "blocks": []
  }
}
```

## Files

- `lens.core/_bmad/lens-work/skills/bmad-lens-switch/scripts/switch-ops.py`
- `lens.core/_bmad/lens-work/skills/bmad-lens-switch/tests/test-switch-ops.py` (CREATE)

## Acceptance Criteria

- [ ] `related` ids produce summary.md paths with `exists` flag.
- [ ] `depends_on` ids produce tech-plan.md paths with `exists` flag.
- [ ] `blocks` ids produce tech-plan.md paths with `exists` flag.
- [ ] Missing files included with `exists: false`, not omitted.
- [ ] `test-switch-ops.py` exists at the verified path.
- [ ] Test fixtures: no dependencies, one `depends_on`, one `blocks`, mixed missing files.

## Dev Notes

- Resolve domain and service for dependency ids by looking them up in `feature-index.yaml`. If not found, include `exists: false` and skip path resolution.
- This story unblocks SW-12. Do not mark SW-9 complete until `test-switch-ops.py` exists and the fixture tests pass.
