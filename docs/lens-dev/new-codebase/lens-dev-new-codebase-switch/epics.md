---
feature: lens-dev-new-codebase-switch
doc_type: epics
status: approved
goal: "Break the switch command clean-room rewrite into implementation epics"
key_decisions:
  - Three epics map directly to the three sprint slices in the sprint plan
  - All epics share a single dependency on the baseline 17-command surface being in place
  - No epic touches governance lifecycle state; governance read-only is a cross-cutting constraint
open_questions: []
depends_on: [lens-dev-new-codebase-baseline]
blocks: []
updated_at: 2026-04-27T00:00:00Z
---

# Epics — Switch Command

## Epic Overview

| Epic | Name | Sprint | Stories | Status |
|---|---|---|---|---|
| EP-1 | Contract Lock And Prompt Parity | Sprint 1 | SW-1, SW-2, SW-3, SW-4 | not-started |
| EP-2 | Switch Operation Parity | Sprint 2 | SW-5, SW-6, SW-7, SW-8, SW-9 | not-started |
| EP-3 | Release Surface And Documentation Parity | Sprint 3 | SW-10, SW-11, SW-12 | not-started |

---

## EP-1 — Contract Lock And Prompt Parity

**Goal:** Preserve the user-visible switch entrypoint, the prompt-start gate, and the numbered selection model.

**Scope:**
- Published stub (`lens-switch.prompt.md`): runs `light-preflight.py` then loads the release-module prompt. No other logic.
- Release prompt path anchoring under `lens.core/_bmad/lens-work/`.
- Config resolution from `bmadconfig.yaml` plus `.lens/governance-setup.yaml` override.
- Numbered menu: `domains` mode stops, `features` mode shows numbered list, invalid input rerenders and stops, `q` cancels cleanly.
- Remove all deprecated `init-feature` references from switch user-facing text.

**Acceptance Definition:**
- Prompt stub runs preflight and immediately loads release module prompt.
- Governance override wins when present; defaults apply when absent.
- List flow produces no branch checkouts, no file writes.
- Menu loop never infers feature from git, editor, or conversation state.
- Zero deprecated command references in switch-visible output text.

**Dependencies:** Baseline 17-command surface (published stub contract).

**Out of Scope:** Switch execution logic, governance writes, target repo state.

---

## EP-2 — Switch Operation Parity

**Goal:** Preserve deterministic feature switching behavior and bounded side effects.

**Scope:**
- Input validation: unsafe ids fail before path construction; feature id must exist in `feature-index.yaml`.
- Switch response: feature id, phase, track, priority, owner, stale flag, context paths, target repo state, branch result.
- Local context write: `.lens/personal/context.yaml` records domain/service/timestamp/updated-by; no governance mutation.
- Branch checkout: report `branch_switched: true/false`; report `branch_error` on failure; no fallback guessing.
- Dependency context: `related` → summary paths; `depends_on`/`blocks` → tech-plan paths; missing files skipped with warning.

**Acceptance Definition:**
- All SW-B1 through SW-B9 requirements pass.
- Switch with a valid id returns structured JSON with all required fields.
- Governance files are unchanged after any switch operation.
- `test-switch-ops.py` passes all fixtures including invalid id, missing index, stale feature, and target repo state.

**Dependencies:** EP-1 (prompt gate and config resolution must be complete).

**Out of Scope:** Feature creation, lifecycle phase changes, governance doc authoring.

---

## EP-3 — Release Surface And Documentation Parity

**Goal:** Ensure switch is coherent in the 17-command surface and fully documented.

**Scope:**
- Command discovery: module help CSV, prompt manifests, agent menu, and docs all list `switch` consistently.
- JSON contract documentation: skill references describe all response shapes including stale, domain fallback, and target repo state.
- Focused regression command: one documented `uv run` command validates switch script behavior end-to-end.

**Acceptance Definition:**
- Running the module help discovery returns `switch` as an available command with correct description.
- All four discovery surfaces (help CSV, prompt manifest, agent menu, docs) are consistent.
- Skill reference documents list and switch contracts in full.
- The focused regression command runs without error and outputs structured pass/fail JSON.
- SW-12 is unblocked only after SW-9/SW-10 test infrastructure exists.

**Dependencies:** EP-1, EP-2 (command behavior must be stable before documentation finalizes).

**Out of Scope:** New command additions, changes to non-switch command surfaces.
