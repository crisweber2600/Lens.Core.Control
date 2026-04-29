---
feature: lens-dev-new-codebase-trueup
doc_type: stories
status: approved
goal: "11 dev-ready stories covering all 14 FRs across 5 epics"
key_decisions:
  - Story IDs use TU prefix (True Up); format TU-{epic}.{story}
  - Estimates in story points (1=trivial, 2=small, 3=medium, 5=large, 8=extra-large)
  - CF annotations reference consolidated carry-forward constraints from finalizeplan-review.md
  - Story files are the canonical spec; this document is the summary index
open_questions: []
depends_on:
  - epics.md
  - architecture.md
  - finalizeplan-review.md
blocks: []
updated_at: 2026-04-29T00:00:00Z
---

# Stories — lens-dev-new-codebase-trueup (True Up)

---

## EP-1: Prompt Publishing Closure

### TU-1.1 — Verify lens-switch.prompt.md Prompt Stub

**Type:** Verification + publish  
**Estimate:** 1 point  
**FR:** FR-1  
**Goal:** Confirm `lens-switch.prompt.md` exists at `_bmad/lens-work/prompts/` in the new-codebase source and is correctly formatted with the standard stub template.

**Acceptance Criteria:**
- [ ] `lens.core.src/_bmad/lens-work/prompts/lens-switch.prompt.md` exists
- [ ] File follows the established stub format (stub header, `light-preflight.py` invocation, pointer to full prompt)
- [ ] File is committed to the target repo on the plan branch
- [ ] `.github/prompts/lens-switch.prompt.md` mirroring is **NOT** an acceptance criterion for this story — that is a post-dev human action (CF-6)

**Notes:** If the file already exists and is correctly formatted, this story is a quick pass — document the verification result in a commit message and close.

---

### TU-1.2 — Author lens-new-feature.prompt.md Prompt Stub

**Type:** Author  
**Estimate:** 2 points  
**FR:** FR-2  
**Goal:** Create `lens-new-feature.prompt.md` in `_bmad/lens-work/prompts/` following the standard stub format.

**Acceptance Criteria:**
- [ ] `lens.core.src/_bmad/lens-work/prompts/lens-new-feature.prompt.md` created and committed
- [ ] Stub header identifies the command as `lens-new-feature`
- [ ] Stub invokes `light-preflight.py` before loading the full prompt
- [ ] Stub points to `lens.core/_bmad/lens-work/prompts/lens-new-feature.prompt.md` as the full prompt source
- [ ] Content matches the authority and scope of the `bmad-lens-init-feature create` command
- [ ] `.github/prompts/lens-new-feature.prompt.md` mirroring is **NOT** an acceptance criterion — post-dev human action (CF-6)

---

### TU-1.3 — Author lens-complete.prompt.md Prompt Stub

**Type:** Author  
**Estimate:** 2 points  
**FR:** FR-3  
**Goal:** Create `lens-complete.prompt.md` in `_bmad/lens-work/prompts/` following the standard stub format.

**Acceptance Criteria:**
- [ ] `lens.core.src/_bmad/lens-work/prompts/lens-complete.prompt.md` created and committed
- [ ] Stub header identifies the command as `lens-complete`
- [ ] Stub invokes `light-preflight.py` before loading the full prompt
- [ ] Stub points to `lens.core/_bmad/lens-work/prompts/lens-complete.prompt.md` as the full prompt source
- [ ] Content matches the authority and scope of the `bmad-lens-complete` skill
- [ ] `.github/prompts/lens-complete.prompt.md` mirroring is **NOT** an acceptance criterion — post-dev human action (CF-6)

---

## EP-2: bmad-lens-complete Package

### TU-2.1 — Author bmad-lens-complete SKILL.md via BMB Channel

**Type:** Author (BMB channel required)  
**Estimate:** 5 points  
**FR:** FR-4  
**Goal:** Produce a complete `SKILL.md` for the `bmad-lens-complete` skill with a full command contract for `check-preconditions`, `finalize`, and `archive-status`.

**Acceptance Criteria:**
- [ ] Load `lens.core/_bmad/bmb/bmadconfig.yaml` and invoke the `bmad-module-builder` skill before authoring begins (CF-1)
- [ ] Load the BMad Builder reference index at `TargetProjects/lens/lens-governance/externaldocs/bmad-builder-docs/llms-full/index.md` before authoring begins (CF-1)
- [ ] `SKILL.md` committed to `lens.core.src/_bmad/lens-work/skills/bmad-lens-complete/SKILL.md`
- [ ] `check-preconditions` command contract documented: inputs, guards, return shape, graceful-degradation behavior (ADR-1)
- [ ] `finalize` command contract documented: inputs, irreversible confirmation gate, dry-run mode, operations list
- [ ] `archive-status` command contract documented: read-only, return shape
- [ ] SKILL.md follows the established module SKILL.md format for the BMB module system

---

### TU-2.2 — Author test-complete-ops.py Scaffolded Stubs

**Type:** Author  
**Estimate:** 3 points  
**FR:** FR-5  
**Goal:** Create the scaffolded test file for `complete-ops.py` with 8 named test stubs and a `conftest.py` fixture scaffold for feature.yaml loading.

**Acceptance Criteria:**
- [ ] `lens.core.src/_bmad/lens-work/skills/bmad-lens-complete/scripts/tests/test-complete-ops.py` created and committed
- [ ] `conftest.py` fixture scaffold created at the same path: includes at least one fixture for feature.yaml fixture loading (CF-10)
- [ ] Eight test stubs present (no implementation, docstring-only bodies):
  - `test_check_preconditions_pass`
  - `test_check_preconditions_warn_no_retrospective`
  - `test_check_preconditions_fail_wrong_phase`
  - `test_finalize_dry_run`
  - `test_finalize_archives_feature`
  - `test_archive_status_archived`
  - `test_archive_status_not_archived`
  - `test_prerequisite_missing_degradation`
- [ ] Each stub has a docstring describing the scenario
- [ ] File imports pass without errors (no implementation required)

---

## EP-3: ADR Artifacts

### TU-3.1 — Author adr-complete-prerequisite.md

**Type:** Author  
**Estimate:** 2 points  
**FR:** FR-6  
**Goal:** Produce the binding ADR document for the `bmad-lens-complete` prerequisite handling strategy (graceful-degradation).

**Acceptance Criteria:**
- [ ] `docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/adr-complete-prerequisite.md` created and committed
- [ ] ADR includes: Context, Decision (graceful-degradation), Evidence from old-codebase `complete-ops.py`, Implications for implementation, Companion governance action (blocker annotation reference)
- [ ] ADR status: `Accepted` (accepted on techplan-adversarial-review pass, 2026-04-28T02:20:00Z)
- [ ] Decision content matches ADR-1 in `architecture.md`

---

### TU-3.2 — Author adr-constitution-tracks.md

**Type:** Author  
**Estimate:** 2 points  
**FR:** FR-7  
**Goal:** Produce the binding ADR document confirming the new-codebase constitution template (`express` + `expressplan` included) as canonical.

**Acceptance Criteria:**
- [ ] `docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/adr-constitution-tracks.md` created and committed
- [ ] ADR includes: Context, Decision (new-codebase template canonical), Evidence (lifecycle.yaml v4, domain constitution, expressflow usage), Implications for `create-domain` and `create-service`, Parity audit classification (resolved, not a gap)
- [ ] ADR status: `Accepted`
- [ ] Decision content matches ADR-2 in `architecture.md`

---

## EP-4: Parity Audit

### TU-4.1 — Author parity-audit-report.md

**Type:** Author (research-heavy)  
**Estimate:** 8 points  
**FR:** FR-8, FR-9, FR-10, FR-11, FR-12, FR-15  
**Goal:** Produce the consolidated parity audit report with per-feature gap analysis for all 5 impacted features plus design decision sections for Python 3.12 and SAFE_ID_PATTERN.

**Acceptance Criteria:**
- [ ] `docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/parity-audit-report.md` created and committed
- [ ] Per-feature sections for: `switch`, `new-domain`, `new-service`, `new-feature`, `complete`
- [ ] Each per-feature section includes: verdict (pass/gap/regression), specific gaps identified (file, function, or schema field), governance classification (structural/behavioral/governance gap)
- [ ] FR-8 section: Python 3.12 requirement documented as intentional (reasons: `tomllib`, structural pattern matching); classified as "reviewed decision, not a gap"
- [ ] FR-9 section: SAFE_ID_PATTERN tightening documented; scan scope stated (all `feature.yaml` and `feature-index.yaml` in `TargetProjects/lens/lens-governance/`); scan date stated; scan result stated (pass/fail) (CF-2)
- [ ] `new-feature` findings include: `create` subcommand absent (regression), `fetch-context` absent (regression), `read-context` absent (regression)
- [ ] `complete` findings include: entire skill absent (regression), SKILL.md missing, script missing, tests missing
- [ ] `switch` findings include: prompt publishing gap (classified)
- [ ] Parity audit review window: author commits this document before EP-5 stories run, giving stakeholders visibility (Mary's carry-forward, CF-9)

**Notes (CF-5):** This story does NOT write blocker annotations. The annotation is a post-audit governance write in EP-5 (TU-5.1). This timing gap is expected and correct.

---

### TU-4.2 — Author Reference Documents (auto-context-pull.md, init-feature.md)

**Type:** Author  
**Estimate:** 3 points  
**FR:** FR-13  
**Goal:** Author two reference documents for `bmad-lens-init-feature` using the clean-room model: `auto-context-pull.md` and `init-feature.md`.

**Acceptance Criteria:**
- [ ] `lens.core.src/_bmad/lens-work/skills/bmad-lens-init-feature/references/auto-context-pull.md` created and committed
- [ ] `lens.core.src/_bmad/lens-work/skills/bmad-lens-init-feature/references/init-feature.md` created and committed
- [ ] `auto-context-pull.md` documents the Auto-Context Pull flow: how `fetch-context` and `read-context` are invoked, expected output contracts (matching architecture Section 6)
- [ ] `init-feature.md` documents the `init-feature create` command flow: featureId construction, lifecycle track resolution, feature.yaml authoring, 2-branch creation
- [ ] Both files reflect the canonical permitted_tracks list (ADR-2: includes `express`, `expressplan`)
- [ ] Content derived from discovery docs and behavioral contracts — clean-room authoring (no old-codebase code)

---

### TU-4.3 — Author parity-gate-spec.md

**Type:** Author  
**Estimate:** 3 points  
**FR:** FR-14  
**Goal:** Produce the migration gate specification that defines what "fully migrated" means for any retained command migration in the new-codebase.

**Acceptance Criteria:**
- [ ] `docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/parity-gate-spec.md` created and committed
- [ ] Spec defines a "fully migrated" gate covering: Layer 1 (prompt stub), Layer 2 (SKILL.md), Layer 3 (script + tests)
- [ ] Spec includes a "How to Apply" section (2–3 paragraphs) explaining how to use the gate for a future retained command migration (CF-4)
- [ ] Spec references ADR-3 (Python 3.12) and ADR-4 (SAFE_ID_PATTERN) as migration standards
- [ ] Post-merge action: reference `parity-gate-spec.md` from the service constitution `constitutions/lens-dev/new-codebase/constitution.md` under a new "Migration Standards" section (CF-11) — this is a governance commit, not a source commit

---

## EP-5: Governance Companion Actions

### TU-5.1 — Write Blocker Annotations and Confirm 14-FR Completion Gate

**Type:** Governance write + completion verification  
**Estimate:** 2 points  
**FR:** FR-15 (governance companion)  
**Goal:** Write blocker annotations to the `feature.yaml` files of `lens-dev-new-codebase-new-feature` and `lens-dev-new-codebase-complete`, and verify the 14-FR completion gate before requesting dev-complete.

**Acceptance Criteria (Pre-Checks — CF-3):**
- [ ] Read `TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-new-feature/feature.yaml` before writing
- [ ] Read `TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-complete/feature.yaml` before writing

**Acceptance Criteria (Blocker Annotations):**
- [ ] `lens-dev-new-codebase-new-feature/feature.yaml` updated with blocker annotation: `"True Up parity audit (2026-04-29): create, fetch-context, read-context absent from new-codebase init-feature-ops.py — functional regression. Blocker on dev phase activation until restored per lens-dev-new-codebase-trueup architecture Section 6."`
- [ ] `lens-dev-new-codebase-complete/feature.yaml` updated with blocker annotation: `"True Up parity audit (2026-04-29): entire bmad-lens-complete skill absent (no SKILL.md, no complete-ops.py, no tests). Blocker on dev phase activation pending True Up dev-complete. See ADR-1 in lens-dev-new-codebase-trueup for prerequisite strategy."`
- [ ] Annotations committed and pushed to governance repo `main`
- [ ] Idempotent: re-running this story with annotations already present produces no duplicate entries (CF-3)

**Acceptance Criteria (14-FR Completion Gate — CF-7):**
- [ ] FR-1: `lens-switch.prompt.md` in `_bmad/lens-work/prompts/` ✓
- [ ] FR-2: `lens-new-feature.prompt.md` in `_bmad/lens-work/prompts/` ✓
- [ ] FR-3: `lens-complete.prompt.md` in `_bmad/lens-work/prompts/` ✓
- [ ] FR-4: `bmad-lens-complete/SKILL.md` committed via BMB channel ✓
- [ ] FR-5: `test-complete-ops.py` stubs with `conftest.py` scaffold committed ✓
- [ ] FR-6: `adr-complete-prerequisite.md` committed ✓
- [ ] FR-7: `adr-constitution-tracks.md` committed ✓
- [ ] FR-8: Python 3.12 section in `parity-audit-report.md` committed ✓
- [ ] FR-9: SAFE_ID_PATTERN scan evidence section in `parity-audit-report.md` committed ✓
- [ ] FR-10: switch parity audit section in `parity-audit-report.md` committed ✓
- [ ] FR-11: new-domain parity audit section in `parity-audit-report.md` committed ✓
- [ ] FR-12: new-service parity audit section in `parity-audit-report.md` committed ✓
- [ ] FR-13: `auto-context-pull.md` and `init-feature.md` reference docs committed ✓
- [ ] FR-14: `parity-gate-spec.md` with "How to Apply" section committed ✓
- [ ] Blocker annotations for new-feature and complete committed to governance ✓

**Notes (CF-5):** The blocker annotation is the governance-visible record of the `new-feature` regression verdict. Before this story runs, the regression finding exists only in the parity audit report — this is expected and correct.

---

## Stories Summary

| Story | Epic | Title | Points | FRs |
|-------|------|-------|--------|-----|
| TU-1.1 | EP-1 | Verify lens-switch.prompt.md | 1 | FR-1 |
| TU-1.2 | EP-1 | Author lens-new-feature.prompt.md | 2 | FR-2 |
| TU-1.3 | EP-1 | Author lens-complete.prompt.md | 2 | FR-3 |
| TU-2.1 | EP-2 | Author bmad-lens-complete SKILL.md (BMB) | 5 | FR-4 |
| TU-2.2 | EP-2 | Author test-complete-ops.py stubs | 3 | FR-5 |
| TU-3.1 | EP-3 | Author adr-complete-prerequisite.md | 2 | FR-6 |
| TU-3.2 | EP-3 | Author adr-constitution-tracks.md | 2 | FR-7 |
| TU-4.1 | EP-4 | Author parity-audit-report.md | 8 | FR-8 to FR-12, FR-15 |
| TU-4.2 | EP-4 | Author reference documents (FR-13) | 3 | FR-13 |
| TU-4.3 | EP-4 | Author parity-gate-spec.md | 3 | FR-14 |
| TU-5.1 | EP-5 | Blocker annotations + 14-FR completion gate | 2 | — |
| **Total** | | | **33 points** | |
