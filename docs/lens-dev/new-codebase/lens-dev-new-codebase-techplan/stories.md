---
feature: lens-dev-new-codebase-techplan
doc_type: stories
status: approved
goal: "Story list for the techplan rewrite — three delivery slices, seven stories."
key_decisions:
  - Story TK-1.1 is already complete (planning artifacts).
  - Stories TK-2.1 through TK-2.5 implement the command surface in the target project.
  - Stories TK-3.1 and TK-3.2 deliver shared utilities and parity hardening.
  - Clean-room rule applies to all code stories: no old-codebase skill prose reproduced.
depends_on:
  - epics.md
  - sprint-plan.md
blocks: []
updated_at: 2026-04-29T00:00:00Z
---

# Story List — Techplan Command

## Story Summary

| Story ID | Title | Points | Status | Depends On |
| --- | --- | --- | --- | --- |
| TK-1.1 | Express Path and Governance Alignment | 2 | ✅ Complete | — |
| TK-2.1 | Pre-Slice Assessment: Confirm Discovery File and Test Path | 1 | 🔵 Ready | TK-1.1 |
| TK-2.2 | Implement `lens-techplan` Public Stub | 2 | 🔵 Ready | TK-2.1 |
| TK-2.3 | Implement Release Prompt and Conductor Skill | 3 | 🔵 Ready | TK-2.2 |
| TK-2.4 | Wire Discovery Surface for `lens-techplan` | 1 | 🔵 Ready | TK-2.3 |
| TK-2.5 | Create Focused Test-Harness Foundation | 2 | 🔵 Ready | TK-2.4 |
| TK-3.1 | Land Shared Utility Surfaces | 5 | 🟡 Blocked (starts after TK-2.1 in-progress) | TK-2.1 |
| TK-3.2 | Parity Regressions, Handoff, and Dev-Complete Gate | 3 | 🟡 Blocked | TK-3.1 |

**Total story points:** 19 (excluding TK-1.1 already complete)

---

## Story TK-1.1 — Express Path and Governance Alignment

**Status:** ✅ Complete  
**Story Points:** 2  
**Epic:** Epic 1

**Story:**  
As a Lens governance reviewer,  
I need the techplan feature folder to contain a coherent expressplan-compatible artifact set,  
so that the downstream planning bundle and implementation agents have a contradiction-free plan to execute against.

**Acceptance Criteria:**
1. `business-plan.md`, `tech-plan.md`, `sprint-plan.md`, and `expressplan-adversarial-review.md` are staged and internally consistent.
2. `feature.yaml` carries `track: express` and `phase: expressplan-complete`.
3. `finalizeplan-review.md` verdict is `pass-with-warnings` with all findings responded to.

---

## Story TK-2.1 — Pre-Slice Assessment: Confirm Discovery File and Test Path

**Status:** 🔵 Ready  
**Story Points:** 1  
**Epic:** Epic 2  
**Depends on:** TK-1.1

**Story:**  
As an implementer beginning the techplan command surface,  
I need to identify and document the exact discovery file and focused test path before writing any code,  
so that TK-2.4 (discovery wiring) and TK-2.5 (test harness) have explicit targets.

**Acceptance Criteria:**
1. The retained-command discovery surface for `lens-techplan` is identified and documented in a brief assessment note (can be inline in TK-2.2 dev notes or a separate `assessment.md`).
2. The focused test file path for prompt-start and wrapper-equivalence regressions is identified and documented.
3. If the existing target-project structure deviates significantly from the tech plan's assumptions, a scope adjustment is documented before TK-2.2 begins.
4. No code changes are produced by this story — assessment only.

**Definition of Done:**
- [ ] Discovery file path identified and documented.
- [ ] Test file path identified and documented.
- [ ] No unresolved scope surprises blocking TK-2.2 through TK-2.5.

**Dev Notes:**
- Check `.github/skills/`, `_bmad/lens-work/bmadconfig.yaml`, module manifest, and any existing prompt file index for where retained commands are registered.
- The preflight and new-domain commands are already present (`lens-new-domain.prompt.md` per tech-plan current state) — match that pattern.

---

## Story TK-2.2 — Implement `lens-techplan` Public Stub

**Status:** 🔵 Ready  
**Story Points:** 2  
**Epic:** Epic 2  
**Depends on:** TK-2.1

**Story:**  
As a Lens user,  
I want a `lens-techplan` prompt stub that runs shared preflight before handing off to the release prompt,  
so that techplan fails fast on configuration errors and loads the correct release logic.

**Acceptance Criteria:**
1. File `TargetProjects/lens-dev/new-codebase/lens.core.src/.github/prompts/lens-techplan.prompt.md` exists.
2. The stub runs `uv run ./lens.core/_bmad/lens-work/scripts/light-preflight.py` (or equivalent shared preflight) and stops if it exits non-zero.
3. On success, the stub loads `lens.core/_bmad/lens-work/prompts/lens-techplan.prompt.md`.
4. The stub follows the same stub pattern as existing prompts in the target project (e.g., `lens-new-domain.prompt.md`).
5. No implementation logic resides in the stub — delegation only.

**Definition of Done:**
- [ ] Stub file exists at the correct path.
- [ ] Stub delegates to release prompt without implementing logic.
- [ ] Clean-room: no old-codebase stub prose reproduced; pattern derived from existing target-project stubs.

---

## Story TK-2.3 — Implement Release Prompt and Conductor Skill

**Status:** 🔵 Ready  
**Story Points:** 3  
**Epic:** Epic 2  
**Depends on:** TK-2.2

**Story:**  
As a Lens feature owner,  
I want the techplan release prompt and owning skill to enforce publish-before-author ordering and PRD reference rules,  
so that architecture authoring stays governed and traceable.

**Acceptance Criteria:**
1. File `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/prompts/lens-techplan.prompt.md` exists as a thin redirect to `bmad-lens-techplan/SKILL.md`.
2. File `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-techplan/SKILL.md` exists.
3. The skill is conductor-only: it resolves feature context and delegates architecture authoring through the Lens BMAD wrapper — no inline architecture generation.
4. The skill enforces publish-before-author: reviewed `businessplan` artifacts must be published before architecture authoring begins (delegates to `bmad-lens-git-orchestration` publish hook when available; documents the dependency clearly when the hook is not yet present in the target project).
5. The skill enforces the PRD reference rule: architecture generation fails or is blocked if the authoritative PRD cannot be located or referenced.
6. The skill does not write governance files directly.
7. Conductor-only rule is verifiable: skill content contains no architecture prose, only delegation instructions.

**Definition of Done:**
- [ ] Release prompt exists and redirects only.
- [ ] Conductor skill exists and is conductor-only.
- [ ] Publish-before-author dependency is documented in the skill or delegated to the publish hook.
- [ ] PRD reference rule is enforced or explicitly described in the skill's activation instructions.
- [ ] No direct governance writes.
- [ ] Clean-room: implementation derived from baseline `4-3-rewrite-techplan.md` and architecture docs only.

**Dev Notes:**
- The publish-before-author hook (part of Epic 3 / TK-3.1) may not be present yet. Document the dependency explicitly in the skill rather than creating a local workaround.
- The BMAD wrapper (`bmad-lens-bmad-skill`) may also not be present yet at TK-2.3 time. Document the delegation path in the skill for the wrapper to slot into when it lands in TK-3.1.

---

## Story TK-2.4 — Wire Discovery Surface for `lens-techplan`

**Status:** 🔵 Ready  
**Story Points:** 1  
**Epic:** Epic 2  
**Depends on:** TK-2.3

**Story:**  
As a Lens user,  
I want `lens-techplan` to appear on the retained command discovery surface,  
so that the command is visible and usable without knowing its exact path.

**Acceptance Criteria:**
1. The discovery file identified in TK-2.1 is updated to include `lens-techplan`.
2. The command is discoverable through the same mechanism as other retained commands already present.
3. The discovery registration follows the existing conventions — no custom discovery logic added.

**Definition of Done:**
- [ ] Discovery file updated.
- [ ] `lens-techplan` appears alongside existing retained commands.

---

## Story TK-2.5 — Create Focused Test-Harness Foundation

**Status:** 🔵 Ready  
**Story Points:** 2  
**Epic:** Epic 2  
**Depends on:** TK-2.4

**Story:**  
As a Lens developer,  
I want a focused test-harness foundation for `techplan` regressions,  
so that prompt-start and wrapper-equivalence behaviors can be validated before and after future changes.

**Acceptance Criteria:**
1. The test file path identified in TK-2.1 exists and contains at least one placeholder or passing test.
2. The test harness includes or documents expectations for:
   - **Prompt-start regression:** stub runs preflight and stops on non-zero exit.
   - **Wrapper-equivalence regression:** techplan delegates architecture authoring through the Lens BMAD wrapper (not inline).
3. Tests may be stubs or lightweight assertions if the shared utility surfaces (Epic 3) are not yet present — document which tests require TK-3.1 to pass.
4. Tests follow the existing test conventions in the target project.

**Definition of Done:**
- [ ] Test file exists at the identified path.
- [ ] At least one passing test is present.
- [ ] Tests that depend on Epic 3 are explicitly marked (e.g., `@pytest.mark.skip` with a note, or a documented prerequisite).
- [ ] Clean-room: no old-codebase test logic reproduced.

---

## Story TK-3.1 — Land Shared Utility Surfaces

**Status:** 🟡 Blocked (starts after TK-2.1 is in-progress)  
**Story Points:** 5  
**Epic:** Epic 3  
**Depends on:** TK-2.1

**Story:**  
As a Lens maintainer,  
I want the four absorbed shared utility surfaces to be present in the target project,  
so that `techplan` can execute end-to-end and sibling features (`expressplan`, `finalizeplan`) can consume them.

**Acceptance Criteria:**
1. **Publish hook** — `bmad-lens-git-orchestration` publish entry hook is present in the target project and callable from `bmad-lens-techplan`. Reviewed `businessplan` artifacts publish before architecture authoring.
2. **BMAD wrapper routing** — `bmad-lens-bmad-skill` wrapper routing is present and enables conductor-only delegation for architecture authoring.
3. **Adversarial review gate** — `bmad-lens-adversarial-review` integration is present and reachable from the `techplan` phase transition.
4. **Constitution loading** — Constitution resolution path is present and resolves correctly for the `lens-dev / new-codebase` domain/service context.
5. Each surface is landed as a shared canonical implementation — not as a `techplan`-local fork.
6. Sibling feature notes: `lens-dev-new-codebase-expressplan` and `lens-dev-new-codebase-finalizeplan` must reference these surfaces, not re-implement them. This story should document the expected import/invocation pattern for sibling consumers.

**Abandon condition:** If all four shared utility surfaces are delivered by another feature before this story begins, this story's scope reduces to: document the consumption pattern for `bmad-lens-techplan` and close the story with "absorbed by [feature X]." The command surface stories (TK-2.x) remain unaffected.

**Definition of Done:**
- [ ] All four shared utility surfaces are present in the target project.
- [ ] `bmad-lens-techplan` can invoke publish hook and BMAD wrapper through shared interfaces.
- [ ] Sibling consumer pattern is documented (at minimum, a one-line note in the skill or a README in each shared utility).
- [ ] No techplan-local forks of shared behavior.
- [ ] All six TK-2.5 regression tests that were marked as "requires Epic 3" now pass.

---

## Story TK-3.2 — Parity Regressions, Handoff, and Dev-Complete Gate

**Status:** 🟡 Blocked  
**Story Points:** 3  
**Epic:** Epic 3  
**Depends on:** TK-3.1

**Story:**  
As a Lens governance reviewer,  
I want all parity regression checks passing and a clean dev-complete gate,  
so that the techplan rewrite is confirmed as implementation-complete and sibling features are unblocked.

**Acceptance Criteria:**
1. **Prompt-start regression** — Stub runs preflight and stops on non-zero exit: ✅ passing.
2. **Publish-before-author regression** — Reviewed `businessplan` artifacts publish before architecture authoring: ✅ passing.
3. **PRD reference regression** — Architecture output explicitly references the authoritative PRD: ✅ passing.
4. **Wrapper-equivalence regression** — Delegation stays routed through the Lens BMAD wrapper; no inline architecture generation: ✅ passing.
5. **Discovery regression** — `lens-techplan` is discoverable through the chosen retained discovery surface: ✅ passing.
6. **Governance-write audit** — `techplan` does not write governance files directly: ✅ confirmed by code review.
7. **Clean-room validation** — No old-codebase skill prose reproduced; implementation derived from baseline PRD and architecture only: ✅ confirmed by code review.
8. Feature `dev-complete` milestone entry criteria satisfied per constitution gate (`gate_mode: informational`).

**Definition of Done:**
- [ ] All six regressions in ACs 1–6 are passing or explicitly acknowledged.
- [ ] Clean-room validation confirmed.
- [ ] `feature.yaml` updated to `dev-complete` via `bmad-lens-feature-yaml`.
- [ ] Final PR merged to `main` (or target branch per governance).
