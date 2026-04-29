---
feature: lens-dev-new-codebase-techplan
story_id: "TK-2.1"
doc_type: story
status: complete
title: "Pre-Slice Assessment: Confirm Discovery File and Test Path"
priority: P0
story_points: 1
epic: "Epic 2 — Target-Project Command Surface"
depends_on: ["TK-1.1"]
blocks: ["TK-2.2", "TK-2.4", "TK-2.5"]
updated_at: 2026-04-29T00:00:00Z
---

# Story TK-2.1 — Pre-Slice Assessment: Confirm Discovery File and Test Path

**Feature:** `lens-dev-new-codebase-techplan`  
**Epic:** 2 — Target-Project Command Surface  
**Priority:** P0 | **Points:** 1 | **Status:** not-started

---

## Goal

Before writing any code, identify and document the exact discovery file and focused test path in the target project. This assessment gates TK-2.2 through TK-2.5 and also unblocks TK-3.1 (shared utility delivery may start after this story is in-progress).

---

## Context

The tech-plan names two open questions that must be resolved at assessment time:
- **OQ-1:** Which exact discovery file registers `lens-techplan` in the target project?
- **OQ-2:** Which focused test file should own prompt-start and wrapper-equivalence regressions?

The target project already has `.github/prompts/lens-new-domain.prompt.md` and the preflight command. The pattern used for `lens-new-domain` is the reference for how `lens-techplan` should be wired.

**No code changes are produced by this story — assessment and documentation only.**

---

## Acceptance Criteria

- [ ] **OQ-1 resolved:** The discovery file (or files) that must be updated to register `lens-techplan` as a retained command is identified and documented. Acceptable outputs: file path + one-line description of the registration mechanism.
- [ ] **OQ-2 resolved:** The focused test file path for prompt-start and wrapper-equivalence regressions is identified and documented. Acceptable output: file path + one-line description of how tests in that file are run.
- [ ] **Scope check:** If the target project's structure deviates significantly from the tech-plan's assumptions (e.g., discovery mechanism is entirely different, preflight is not yet callable), document the deviation and adjust the scope of TK-2.2–TK-2.5 before marking this story complete.
- [ ] Assessment findings are committed as a brief note (inline in TK-2.2 dev notes OR as `assessment.md` in the stories folder — either is acceptable).

---

## Dev Notes

**Where to look for OQ-1 (discovery file):**
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/bmadconfig.yaml` — retained command list or skill manifest
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/_config/skill-manifest.csv` or `manifest.yaml`
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/module-help.csv`
- Existing prompt files in `.github/prompts/` may self-register via frontmatter or a separate manifest

**Where to look for OQ-2 (test file path):**
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/tests/` 
- Pattern from `bmad-lens-preflight` tests or `bmad-lens-init-feature` tests as reference

**Reference for stub pattern:**
- `TargetProjects/lens-dev/new-codebase/lens.core.src/.github/prompts/lens-new-domain.prompt.md` (confirmed present per tech-plan)
