---
feature_id: lens-dev-new-codebase-new-domain
story_key: "1-4-parity-tests"
epic: 1
story: 4
title: "Schema parity tests"
type: test
estimate: M
priority: P0
status: not-started
assigned: crisweber2600
sprint: 1
depends_on:
  - "1-2-core-flow"
blocks: []
created_at: 2026-04-26T00:00:00Z
updated_at: 2026-04-26T00:00:00Z
---

# Story 1.4 — Schema parity tests

## Why This Story Exists

The clean-room constraint requires that the new implementation produces schema-equivalent output to the old codebase. Parity tests are the automated signal that proves this constraint is met without importing or copying old-codebase source.

**F10 requirement (finalizeplan-review.md Winston finding):** The constitution body test fixture must be defined from the spec, not from old-codebase source. Do NOT copy the function body from old-codebase `init-feature-ops.py`. Define the expected output as an inline Python string constant in the test file.

---

## File Locations

| File | Action |
|---|---|
| `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-init-feature/scripts/tests/test-init-feature-ops.py` | Add parity test cases |

---

## Constitution Body Authoritative Test Fixture

The `make_domain_constitution_md(domain)` function must produce output matching this template. Define this as an inline constant in the test file:

```python
EXPECTED_CONSTITUTION_BODY = """\
---
permitted_tracks: [quickplan, full, hotfix, tech-change]
required_artifacts:
  planning:
    - business-plan
  dev:
    - stories
gate_mode: informational
sensing_gate_mode: informational
additional_review_participants: []
enforce_stories: true
enforce_review: true
---

# {domain} Domain Constitution

## Scope

This constitution governs all features under the `{domain}` domain.

## Tracks

All tracks listed in `permitted_tracks` are available for features in this domain.

## Artifacts

Planning artifacts and development artifacts listed in `required_artifacts` are required for features in this domain.

## Review

Reviews are `{gate_mode}`. Sensing is `{sensing_gate_mode}`.

## Notes

This is an auto-generated default constitution. Edit this file to add domain-specific governance rules.
"""
```

In the test, interpolate `{domain}` before comparing. `{gate_mode}` and `{sensing_gate_mode}` should use the values from the frontmatter (`informational`).

**The fixture must NOT be loaded from any file. It must be a Python string literal defined in the test file.**

---

## Acceptance Criteria

- [ ] `test_domain_yaml_schema_parity`:
  - Runs `create-domain` with a temp governance repo
  - Loads the resulting `domain.yaml` as a dict
  - Asserts all required fields present: `kind`, `id`, `name`, `domain`, `status`, `owner`, `created`, `updated`
  - Asserts `kind == "domain"`, `id == domain_slug`, `status == "active"`
  - Asserts no unexpected fields present
  - Asserts `created` and `updated` are valid ISO-8601 timestamps

- [ ] `test_constitution_content_parity`:
  - Runs `create-domain` with a temp governance repo
  - Reads the resulting `constitution.md` content
  - Compares to `EXPECTED_CONSTITUTION_BODY` (inline constant, not file-loaded)
  - Test fixture is defined as a Python string literal in the test file

- [ ] `test_context_yaml_schema_parity`:
  - Runs `create-domain` with `--personal-folder` pointing to a temp dir
  - Loads the resulting `context.yaml`
  - Asserts exactly 4 fields: `domain`, `service`, `updated_at`, `updated_by`
  - Asserts `service` is Python `None` (YAML `null`)
  - Asserts `updated_by == "new-domain"`
  - Asserts no unexpected fields present

- [ ] `test_create_domain_name_defaults_to_slug`:
  - Runs `create-domain` without `--name`
  - Asserts `domain.yaml.name == domain_slug`

## Review Requirement

Story 1.4 review must verify the constitution fixture is NOT loaded from a file path. Reviewer checks `EXPECTED_CONSTITUTION_BODY` is defined as an inline constant.
