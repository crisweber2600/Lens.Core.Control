---
feature: lens-dev-new-codebase-trueup
doc_type: epics
status: approved
goal: "Close verified delivery gaps across the 5 non-preplan new-codebase features"
key_decisions:
  - Five epics aligned to the four PRD delivery categories plus governance finalization
  - EP-4 Parity Audit is the pivot epic — its output gates EP-5 governance writes
  - EP-1 through EP-3 are parallelizable and may be assigned independently
  - All artifacts staged in the plan branch; `.github/prompts/` publishing is a post-dev human action (CF-6)
open_questions: []
depends_on:
  - prd.md
  - architecture.md
  - finalizeplan-review.md
blocks: []
updated_at: 2026-04-29T00:00:00Z
---

# Epics — lens-dev-new-codebase-trueup (True Up)

---

## EP-1: Prompt Publishing Closure

**Goal:** Ensure `switch`, `new-feature`, and `complete` commands are discoverable by IDE agents by publishing prompt stubs to the `_bmad/lens-work/prompts/` source location in the new-codebase.

**Scope:**
- Verify `lens-switch.prompt.md` exists and is correctly formatted (FR-1)
- Author `lens-new-feature.prompt.md` (FR-2)
- Author `lens-complete.prompt.md` (FR-3)
- Note: `.github/prompts/` mirroring is a **post-dev human action** (H1 resolution, CF-6); dev agent scope is `_bmad/lens-work/prompts/` only

**Stories:** TU-1.1, TU-1.2, TU-1.3

**Dependencies:** None — parallelizable with EP-2 and EP-3

**Acceptance Gate:** All three prompt stubs committed to `lens.core.src/_bmad/lens-work/prompts/` in the target repo

---

## EP-2: bmad-lens-complete Package

**Goal:** Produce the `bmad-lens-complete` SKILL.md with full command contract and scaffolded test stubs, using the BMB channel per constitution requirement.

**Scope:**
- Author `SKILL.md` for 3 operations: `check-preconditions`, `finalize`, `archive-status` (FR-4)
- Author `test-complete-ops.py` with 8 scaffolded test stubs and fixture scaffold (FR-5)

**Stories:** TU-2.1, TU-2.2

**Dependencies:** None — parallelizable with EP-1 and EP-3. **FR-4 must route through BMB channel (CF-1).**

**Acceptance Gate:** `SKILL.md` committed to `lens.core.src/_bmad/lens-work/skills/bmad-lens-complete/` via BMB invocation; test stubs committed with `conftest.py` fixture scaffold

---

## EP-3: ADR Artifacts

**Goal:** Commit the two binding ADR documents that formalize the complete-prerequisite strategy and the constitution permitted_tracks canonical template.

**Scope:**
- `adr-complete-prerequisite.md`: graceful-degradation decision, implications, companion governance action (FR-6)
- `adr-constitution-tracks.md`: new-codebase template canonical, evidence, implications (FR-7)

**Stories:** TU-3.1, TU-3.2

**Dependencies:** None — parallelizable with EP-1 and EP-2. ADRs are accepted as of the techplan-adversarial-review verdict (2026-04-28T02:20:00Z).

**Acceptance Gate:** Both ADR documents committed to `docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/`

---

## EP-4: Parity Audit

**Goal:** Produce the consolidated parity audit report, reference documents, and migration gate specification that close all verified evidence gaps across the 5 non-preplan features.

**Scope:**
- `parity-audit-report.md` covering per-feature parity (switch, new-domain, new-service, new-feature, complete) plus FR-8 (Python 3.12) and FR-9 (SAFE_ID_PATTERN scan evidence) design decision sections (FR-8, FR-9, FR-10 to FR-12, FR-15)
- `references/auto-context-pull.md` and `references/init-feature.md` reference documents (FR-13)
- `parity-gate-spec.md` migration gate spec including "How to Apply" section (FR-14, CF-4)

**Stories:** TU-4.1, TU-4.2, TU-4.3

**Dependencies:** Requires ADRs from EP-3 to be complete before authoring the parity-audit-report design-decision sections. EP-1–EP-3 parallelizable.

**Acceptance Gate:** All parity audit artifacts committed; FR-9 scan evidence section present; parity-gate-spec.md includes "How to Apply" section

**Parity Audit Review Window:** After TU-4.1 is committed, stakeholders for the 5 impacted features have visibility into parity findings before EP-5 blocker annotations are written.

---

## EP-5: Governance Companion Actions

**Goal:** Write blocker annotations to `feature.yaml` for `lens-dev-new-codebase-new-feature` and `lens-dev-new-codebase-complete` based on the parity audit results, and confirm the 14-FR master completion gate.

**Scope:**
- Read both target feature.yaml files before writing (CF-3)
- Write blocker annotations for new-feature (regression: `create`, `fetch-context`, `read-context` absent) and complete (regression: entire skill absent)
- Confirm idempotent execution: running twice produces same governance state (CF-3)
- Verify 14-FR completion checklist (CF-7) before marking dev-complete

**Stories:** TU-5.1

**Dependencies:** Must run after EP-4 (parity audit report committed). Blocker annotation is the governance-visible record of the regression verdict (CF-5); timing gap from parity audit completion to this story is expected and correct.

**Acceptance Gate:** Both blocker annotations committed to governance repo; True Up dev-complete confirmed via 14-FR checklist

---

## Epic Summary

| Epic | Title | Stories | Parallelizable | Gate |
|------|-------|---------|----------------|------|
| EP-1 | Prompt Publishing Closure | TU-1.1, TU-1.2, TU-1.3 | Yes | Prompts in `_bmad/lens-work/prompts/` |
| EP-2 | bmad-lens-complete Package | TU-2.1, TU-2.2 | Yes | SKILL.md + stubs via BMB |
| EP-3 | ADR Artifacts | TU-3.1, TU-3.2 | Yes | ADRs committed |
| EP-4 | Parity Audit | TU-4.1, TU-4.2, TU-4.3 | After EP-3 ADRs | All parity docs committed |
| EP-5 | Governance Companion Actions | TU-5.1 | After EP-4 | Blocker annotations + 14-FR gate |
