---
feature: lens-dev-new-codebase-switch
story_id: SW-5
epic: EP-2
sprint: 2
title: Validate Target Feature Identity
estimate: S
status: not-started
blocked_by: []
assignee: crisweber2600
doc_type: story
updated_at: 2026-04-27T00:00:00Z
---

# SW-5 — Validate Target Feature Identity

## Context

Before any file I/O, switch must sanitize the feature id input and confirm the feature exists in `feature-index.yaml`. Unsafe ids and unknown ids must fail fast with structured error responses.

**Requirement:** SW-B5

## Task

In `switch-ops.py` switch operation:

1. Apply safe-identifier validation: lowercase alphanumeric + hyphens only, no slashes, spaces, or other characters. Reject immediately if invalid.
2. Load `feature-index.yaml` from the resolved governance repo path.
3. If the file is missing, return `status: fail`, `error: index_not_found`.
4. If the file is malformed (invalid YAML or missing required fields), return `status: fail`, `error: index_malformed`.
5. Check that the sanitized feature id exists in the index. If not, return `status: fail`, `error: feature_not_found`.
6. Only after index validation succeeds, proceed to `feature.yaml` path resolution.

## Files

- `lens.core/_bmad/lens-work/skills/bmad-lens-switch/scripts/switch-ops.py`
- `TargetProjects/lens/lens-governance/feature-index.yaml` (read)

## Acceptance Criteria

- [ ] Id with path separators (`/`, `\`), spaces, or uppercase letters is rejected before any file I/O with `error: invalid_feature_id`.
- [ ] Valid id not in index returns `error: feature_not_found`.
- [ ] Missing index file returns `error: index_not_found`.
- [ ] Malformed index returns `error: index_malformed`.
- [ ] Valid id in index proceeds to `feature.yaml` resolution.
- [ ] Test fixtures: invalid characters, unknown id, missing index, malformed index.

## Dev Notes

- Use the same validation pattern as `bmad-lens-init-feature` for id sanitization.
- Do not fall back to scanning `features/` directories if the index is missing — that is the `mode: domains` path reserved for `list`.
