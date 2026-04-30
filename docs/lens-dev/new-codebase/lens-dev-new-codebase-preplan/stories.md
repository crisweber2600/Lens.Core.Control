---
feature: lens-dev-new-codebase-preplan
doc_type: stories
status: draft
goal: "Full story list for the preplan clean-room rewrite with acceptance criteria and estimates"
key_decisions:
  - Stories are sized S (≤1 day), M (2-3 days) against new-codebase implementation time
  - All stories referencing shared utilities carry an explicit prerequisite gate on the relevant baseline story
  - No story may introduce an owned implementation script (tests-only scripts/tests/ subdir permitted per ADR 1)
depends_on:
  - lens-dev-new-codebase-baseline
blocks: []
updated_at: 2026-04-28T00:00:00Z
---

# Stories — Preplan Command

## Story List

| Story | Epic | Title | Estimate | Status |
|---|---|---|---:|---|
| PP-1.1 | PP-E1 | Add command prompt surfaces | S | not-started |
| PP-1.2 | PP-E1 | Write SKILL.md thin conductor contract | M | not-started |
| PP-1.3 | PP-E1 | Add parity test skeletons | S | not-started |
| PP-2.1 | PP-E2 | Implement conductor activation and constitution load | M | not-started |
| PP-2.2 | PP-E2 | Implement analyst activation and brainstorm mode selection | M | not-started |
| PP-2.3 | PP-E2 | Implement research and product-brief delegation | S | not-started |
| PP-3.1 | PP-E3 | Wire validate-phase-artifacts.py for review-ready | M | not-started |
| PP-3.2 | PP-E3 | Wire bmad-lens-batch for batch mode | M | not-started |
| PP-3.3 | PP-E3 | Wire bmad-lens-adversarial-review for phase completion | M | not-started |
| PP-4.1 | PP-E4 | Implement phase completion and feature.yaml update | M | not-started |
| PP-4.2 | PP-E4 | Align help surfaces and resolve target-repo scope | S | not-started |

---

## PP-1.1 — Add command prompt surfaces

**Epic:** PP-E1 | **Estimate:** S | **Sprint:** 1

**As a** Lens user starting a new feature,  
**I want** the `preplan` command to be available and discoverable in the new codebase,  
**so that** I can invoke `/preplan` through the prompt surface without manual path configuration.

**Acceptance Criteria:**
- `.github/prompts/lens-preplan.prompt.md` exists in `TargetProjects/lens-dev/new-codebase/lens.core.src`; it runs `light-preflight.py` and delegates to the release prompt.
- `_bmad/lens-work/prompts/lens-preplan.prompt.md` exists; it loads `bmad-lens-preplan/SKILL.md` with no additional logic.
- The stub redirect path stays relative to `lens.core/` (not hardcoded absolute paths).
- No old-codebase file patterns appear in the diff.

**Dependencies:** None.

---

## PP-1.2 — Write SKILL.md thin conductor contract

**Epic:** PP-E1 | **Estimate:** M | **Sprint:** 1

**As a** developer implementing the preplan conductor,  
**I want** a complete SKILL.md behavioral contract that describes every integration point and sequencing rule,  
**so that** implementation decisions can be made against a written specification rather than guesswork.

**Acceptance Criteria:**
- `_bmad/lens-work/skills/bmad-lens-preplan/SKILL.md` exists in `lens.core.src`.
- SKILL.md documents: analyst activation before brainstorm mode selection; user choice between `bmad-brainstorming` and `bmad-cis`; brainstorm-first ordering; batch delegation to `bmad-lens-batch`; review-ready delegation to `validate-phase-artifacts.py`; no-governance-write invariant; phase completion gate (party-mode adversarial review); phase state update via `bmad-lens-feature-yaml`; all integration points in a table.
- SKILL.md documents the `/next` pre-confirmed handoff: when activated via `/next` delegation, no launch confirmation prompt is shown — preplan begins immediately.
- SKILL.md does NOT contain an owned implementation script reference (no `preplan-ops.py`); a `scripts/tests/` subpath is permitted per ADR 1.
- Contract matches the release `lens.core/_bmad/lens-work/skills/bmad-lens-preplan/SKILL.md` behavioral specification without verbatim copying.

**Dependencies:** PP-1.1 (prompt surfaces must exist before SKILL.md is meaningful).

---

## PP-1.3 — Add parity test skeletons

**Epic:** PP-E1 | **Estimate:** S | **Sprint:** 1

**As a** developer implementing the preplan conductor,  
**I want** parity test skeletons that fail red before any implementation code is merged,  
**so that** the TDD discipline is enforced and no behavior is shipped without a corresponding green test.

**Acceptance Criteria:**
- Test file `_bmad/lens-work/skills/bmad-lens-preplan/scripts/tests/test-preplan-ops.py` exists in `lens.core.src` following the `test-{skill}-ops.py` naming convention established by peer skills (`bmad-lens-switch`, `bmad-lens-init-feature`).
- Tests fail red for each of the following parity categories: analyst activation ordering (before brainstorm mode selection), brainstorm mode choice — `bmad-brainstorming` path, brainstorm mode choice — `bmad-cis` path, brainstorm-first ordering (no research/product-brief wrapper callable before `brainstorm.md` exists), batch pass 1 stop (no lifecycle artifacts written), batch pass 2 resume (context loaded), review-ready delegation (calls `validate-phase-artifacts.py`, no inline checks), phase gate on fail verdict (blocks `feature.yaml` update), phase gate on pass verdict (allows transition), no-governance-write invariant (`publish-to-governance` not invoked at any step), `/next` pre-confirmed handoff (no confirmation prompt on `/next` → `/preplan` activation), `fetch-context` availability (confirms `bmad-lens-init-feature fetch-context` is callable in new codebase).
- Existing create-domain, init-feature, and create-service parity tests remain unchanged and pass.

**Dependencies:** PP-1.2 (SKILL.md contract is the authoritative specification for what tests to write).

---

## PP-2.1 — Implement conductor activation and constitution load

**Epic:** PP-E2 | **Estimate:** M | **Sprint:** 2

**As a** Lens user activating `/preplan`,  
**I want** the conductor to resolve feature context and load the domain constitution without error,  
**so that** the preplan session starts from a consistent, governed state regardless of missing org-level constitution entries.

**Acceptance Criteria:**
- Conductor resolves: feature context (via `bmad-lens-feature-yaml`), docs path, governance mirror path.
- Loads domain constitution via `bmad-lens-constitution`; handles partial hierarchy without panic (relies on baseline story 3-1).
- When activated via `/next` delegation, no launch confirmation prompt is presented — preplan begins immediately (pre-confirmed handoff invariant).
- `fetch-context` availability parity test turns green.
- Analyst-activation-ordering parity test still fails red (analyst not yet wired in this story).
- Baseline story 3-1 confirmed green before this story is closed.

**Prerequisites:** Baseline story 3-1 (`bmad-lens-constitution` partial-hierarchy fix) confirmed green.
**Dependencies:** PP-1.3 (parity skeletons must exist).

---

## PP-2.2 — Implement analyst activation and brainstorm mode selection

**Epic:** PP-E2 | **Estimate:** M | **Sprint:** 2

**As a** Lens user running preplan interactively,  
**I want** the conductor to activate `bmad-agent-analyst` before presenting brainstorm mode choices,  
**so that** requirements context is grounded before any brainstorming session begins.

**Acceptance Criteria:**
- In interactive mode, `bmad-agent-analyst` is activated before any brainstorm wrapper is invoked; analyst framing (goals, constraints, known assumptions) completes before mode selection.
- After analyst framing, the conductor presents the user with a choice: `bmad-brainstorming` (divergent ideation) or `bmad-cis` (structured innovation).
- Both modes route through `bmad-lens-bmad-skill`; the selected mode runs to completion before research wrappers are offered.
- `brainstorm.md` must exist in the docs path before research or product-brief wrappers are callable, regardless of which mode was selected.
- The following parity tests turn green: analyst activation ordering, brainstorm mode choice — `bmad-brainstorming` path, brainstorm mode choice — `bmad-cis` path, brainstorm-first ordering.
- `/next` pre-confirmed handoff parity test remains green (from PP-2.1).

**Dependencies:** PP-2.1 (conductor activation must be in place before this wiring is added).

---

## PP-2.3 — Implement research and product-brief delegation

**Epic:** PP-E2 | **Estimate:** S | **Sprint:** 2

**As a** Lens user after brainstorming completes,  
**I want** the conductor to offer research and product-brief authoring through canonical BMAD wrappers,  
**so that** all authoring goes through Lens-governed wrappers with no conductor-owned file generation.

**Acceptance Criteria:**
- After `brainstorm.md` exists, the conductor offers research authoring; routes through the narrowest applicable canonical wrapper identifier: `bmad-domain-research`, `bmad-market-research`, or `bmad-technical-research` (exact names — no shorthand aliases).
- Product brief routes through `bmad-product-brief` via `bmad-lens-bmad-skill`.
- Conductor does not author artifacts directly (no file writes in SKILL.md logic).
- Research mode inference does not skip user clarification when the research scope is ambiguous.
- No parity tests regress.

**Dependencies:** PP-2.2 (brainstorm-first ordering must be wired before research delegation is added).

---

## PP-3.1 — Wire validate-phase-artifacts.py for review-ready

**Epic:** PP-E3 | **Estimate:** M | **Sprint:** 3

**As a** Lens user re-activating preplan when all artifacts already exist,  
**I want** the conductor to detect the review-ready state via the shared validation script and skip to the adversarial review gate,  
**so that** I don't re-run authoring flows unnecessarily.

**Acceptance Criteria:**
- On activation, conductor calls `uv run ... validate-phase-artifacts.py --phase preplan --contract review-ready --lifecycle-path ... --docs-root ... --json`.
- `status: pass` triggers the review-ready fast path (advance to adversarial review gate; skip authoring).
- `status: fail` triggers normal authoring flow.
- No inline artifact presence checks (no direct `brainstorm.md`, `research.md`, `product-brief.md` checks in SKILL.md logic).
- Review-ready delegation parity test turns green.
- Baseline story 1-2 (`validate-phase-artifacts.py`) confirmed callable before this story is closed.

**Prerequisites:** Baseline story 1-2 (`validate-phase-artifacts.py` callable with `--phase preplan --contract review-ready`).
**Sprint 3 Gate:** Sprint 3 gate verification command passes before this story begins.
**Dependencies:** PP-2.2 (authoring flow must exist for the fast-path bypass to be meaningful).

---

## PP-3.2 — Wire bmad-lens-batch for batch mode

**Epic:** PP-E3 | **Estimate:** M | **Sprint:** 3

**As a** Lens user running preplan in batch mode,  
**I want** the conductor to delegate entirely to `bmad-lens-batch` for the 2-pass contract,  
**so that** batch intake and resume are governed by the shared contract and not re-implemented inline.

**Acceptance Criteria:**
- On batch pass 1: conductor calls `bmad-lens-batch --target preplan`, stops without writing lifecycle artifacts; `preplan-batch-input.md` is written and nothing else.
- On batch pass 2: `batch_resume_context` is loaded as pre-approved context; authoring resumes from where pass 1 stopped.
- No inline `if mode == batch and batch_resume_context absent` logic in the conductor.
- Batch pass 1 stop and batch pass 2 resume parity tests turn green.
- Baseline story 1-3 (`bmad-lens-batch`) confirmed callable before this story is closed.

**Prerequisites:** Baseline story 1-3 (`bmad-lens-batch --target preplan` callable).
**Sprint 3 Gate:** Sprint 3 gate verification command passes before this story begins.
**Dependencies:** PP-2.1 (conductor activation must be in place).

---

## PP-3.3 — Wire bmad-lens-adversarial-review for phase completion

**Epic:** PP-E3 | **Estimate:** M | **Sprint:** 3

**As a** Lens user completing preplan,  
**I want** the phase completion adversarial review to run through the shared `bmad-lens-adversarial-review` skill in party mode,  
**so that** the phase gate is consistently enforced and the review report is produced before any feature.yaml update.

**Acceptance Criteria:**
- At phase completion, conductor calls `bmad-lens-adversarial-review --phase preplan --source phase-complete`.
- `fail` verdict blocks `feature.yaml` update; `pass` or `pass-with-warnings` allows phase transition.
- Review runs in party mode as specified by `lifecycle.yaml`.
- **No-governance-write invariant test still passes after adversarial review wiring** — the adversarial review wiring does not introduce any governance write path (confirmed by the no-governance-write parity test).
- Phase gate on fail and phase gate on pass parity tests turn green.
- Baseline story 3-1 (constitution must be resolved before review can run) confirmed green.

**Prerequisites:** Baseline story 3-1 (`bmad-lens-constitution` partial-hierarchy fix) confirmed green.
**Sprint 3 Gate:** Sprint 3 gate verification command passes before this story begins.
**Dependencies:** PP-3.1, PP-3.2 (all shared utility wiring in same sprint; no-governance-write invariant must not regress across any PP-3.x story).

---

## PP-4.1 — Implement phase completion and feature.yaml update

**Epic:** PP-E4 | **Estimate:** M | **Sprint:** 4

**As a** Lens user finishing the preplan phase,  
**I want** the conductor to update `feature.yaml` to `preplan-complete` and emit a clear completion message naming `/businessplan`,  
**so that** the feature state is governed and the next action is obvious without creating a runtime dependency on businessplan availability.

**Acceptance Criteria:**
- After adversarial review passes (`pass` or `pass-with-warnings`), conductor updates `feature.yaml` phase to `preplan-complete` via `bmad-lens-feature-yaml`.
- Conductor emits a completion message naming `/businessplan` as the expected next command; this is a message only — no live routing call is made; actual routing is the user's action.
- **No `publish-to-governance` call occurs** at any point during the preplan phase (including phase completion).
- No-governance-write parity test passes.
- An independent end-to-end run of the full preplan flow (by a reviewer other than the implementing developer) is recorded in the story closure notes before the story is closed.

**Dependencies:** PP-3.1, PP-3.2, PP-3.3 (all shared utility wiring must be green).

---

## PP-4.2 — Align help surfaces and resolve target-repo scope

**Epic:** PP-E4 | **Estimate:** S | **Sprint:** 4

**As a** Lens user discovering available commands,  
**I want** `preplan` to appear consistently in help surfaces and module manifests,  
**so that** the command is discoverable and its position in the 17-command surface is official.

**Acceptance Criteria:**
- `module-help.csv` in `lens.core.src` includes `preplan` in the 17-command surface (or confirms this is owned by a separate alignment sweep feature, with a reference recorded).
- `lens.agent.md` in `lens.core.src` includes `preplan` in its command reference (or documents the same ownership delegation as above).
- A documented scope decision exists for `bmad-lens-target-repo` interruption-and-resume within preplan: either (a) a PP-4.3 story is added to this feature's backlog with the implementation scope, or (b) a written deferral note is added to the feature's out-of-scope section with a reference to the follow-up feature or issue.
- No existing parity tests regress.

**Dependencies:** PP-4.1 (phase completion must be in place before help alignment is the last remaining item).
