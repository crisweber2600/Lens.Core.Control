---
feature: lens-dev-new-codebase-new-service
doc_type: epics
status: approved
goal: "Restore /new-service as a retained Lens setup command with clean-room observable parity"
key_decisions:
  - Four epics map to the four sprint phases: test lock, script impl, surface parity, verification
  - Epic sequencing enforces test-first: NS-E1 (contract tests) must land before NS-E2 (script impl)
  - Each epic ends with a gated outcome that enables the next epic
open_questions: []
depends_on:
  - lens-dev-new-codebase-baseline
blocks:
  - lens-dev-new-codebase-new-feature
updated_at: 2026-04-27T16:00:00Z
---

# Epic List — New Service Command

## Summary

Four epics deliver the `/new-service` retained command. The sequencing is strict: observable parity tests must exist before implementation, and the full init-feature regression must pass before handoff.

**Total estimate:** 29 story points across 13 stories

| Epic | Title | Stories | Points | Gate |
|------|-------|---------|--------|------|
| NS-E1 | Contract and Test Lock | NS-1, NS-2, NS-3 | 7 | Failing tests cover all CLI contract scenarios |
| NS-E2 | Script Implementation | NS-4, NS-5, NS-6, NS-7 | 11 | All NS-E1 tests pass green |
| NS-E3 | Skill, Prompt, and Surface Parity | NS-8, NS-9, NS-10 | 6 | All NS-E2 tests pass green |
| NS-E4 | Verification and Handoff | NS-11, NS-12, NS-13 | 5 | Full init-feature regression passes; NS-13 handoff notes complete |

---

## NS-E1: Contract and Test Lock

**Goal:** Lock the observable CLI contract for `create-service` with failing tests before any implementation begins.

**Business Value:** Prevents clean-room drift — tests prove observable user-visible equivalence without requiring source-level duplication from the old codebase.

**Scope:**
- CLI argument acceptance, dry-run behavior, duplicate/invalid-ID rejection, output field coverage
- Service-not-feature boundary: tests prove no feature lifecycle artifacts are written
- Discovery surface expectations: prompt and module help expose the retained command

**Exit Gate:** NS-1, NS-2, NS-3 all in place with failing (red) tests that define the implementation target.

**Dependencies:** Requires `business-plan.md` and `tech-plan.md` to be read by the dev agent before writing tests.

**Stories:** NS-1, NS-2, NS-3

---

## NS-E2: Script Implementation

**Goal:** Implement the `create-service` script subcommand so all NS-E1 contract tests turn green.

**Business Value:** Delivers the governance-layer data model for service containers: service YAML, inherited service constitution, optional scaffolds, personal context, and governance git integration.

**Scope:**
- Service marker and constitution builder helpers
- `create-service` parser route and CLI argument handling
- Context writer extension (domain-only and domain-plus-service in one helper)
- Governance git integration with idempotency guarantee
- ADR-3 parent-domain delegation: `create-service` calls existing `create-domain` helpers — no parallel mutation path

**Exit Gate:** All NS-E1 tests pass green; idempotency test for governance git path passes.

**Dependencies:** NS-E1 exit gate must be reached before implementation begins.

**Stories:** NS-4, NS-5, NS-6, NS-7

---

## NS-E3: Skill, Prompt, and Surface Parity

**Goal:** Make `/new-service` reachable and discoverable through the retained command surface.

**Business Value:** Users can invoke `/new-service` from any of the three Lens command discovery surfaces: prompt stubs, release prompts, and module help.

**Scope:**
- `bmad-lens-init-feature` SKILL.md extended with the `new-service` intent flow (via `.github/skills/bmad-module-builder`)
- Release prompt `lens-new-service.prompt.md` added (via `.github/skills/bmad-workflow-builder`)
- Module help and packaging surfaces updated to include `new-service`

**Exit Gate:** Three discovery surfaces in sync: prompt stub, release prompt, module help. All NS-E2 tests remain green.

**Implementation Channel Note:** SKILL.md changes route through `.github/skills/bmad-module-builder`. Release prompt and workflow artifacts route through `.github/skills/bmad-workflow-builder`. Direct edits to `lens.core.src` for NS-4–NS-7 script stories are accepted deviations (per `gate_mode: informational`), recorded in NS-13.

**Stories:** NS-8, NS-9, NS-10

---

## NS-E4: Verification and Handoff

**Goal:** Prove end-to-end parity and hand off to the next development phase with complete documentation.

**Business Value:** Ensures no regression in the `new-domain` path and leaves a clear, self-contained handoff document so the `/dev` agent can begin implementation without re-reading the full planning set.

**Scope:**
- Focused service parity test suite (`-k create_service`) passes
- Full init-feature regression passes (no `new-domain` regression)
- NS-13 handoff notes document: files, test commands, implementation channel decisions, clean-room constraint, and accepted deviations

**Exit Gate:** NS-12 passes; NS-13 handoff notes exist and are validated by NS-12.

**Stories:** NS-11, NS-12, NS-13
