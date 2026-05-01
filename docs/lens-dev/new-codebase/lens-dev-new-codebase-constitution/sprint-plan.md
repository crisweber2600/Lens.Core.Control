---
feature: lens-dev-new-codebase-constitution
doc_type: sprint-plan
status: draft
goal: "Sequence the constitution rewrite into implementation-ready slices that remove the org-level hard-fail, preserve read-only subcommand parity, and keep express-track governance usable."
key_decisions:
  - Use four slices: express alignment, resolver core, compliance and display parity, and regression hardening.
  - Treat express-track support as shared constitution parity, not a downstream command workaround.
  - Keep the conductor thin: the public stub and release prompt delegate, while constitution-ops.py owns all resolution logic.
  - Verify parity with temp-directory governance fixtures and negative-path regressions instead of copied outputs.
open_questions:
  - Should the repo-level constitution fixture land in the first regression pass or after org/domain/service parity is green?
  - Can `sensing_gate_mode` parity land in the first rewrite pass, or does it need a follow-up caller audit?
depends_on:
  - business-plan.md
  - tech-plan.md
blocks:
  - expressplan-adversarial-review.md
updated_at: 2026-05-01T00:00:00Z
---

# Sprint Plan — Constitution Command

## Sprint Objective

Deliver a clean-room `constitution` rewrite that can move through the express planning path, removes the org-level hard-fail, preserves read-only behavior across all three subcommands, and proves parity with focused regressions rather than prose claims.

## Current Packet Status

- The packet is authored for the express planning route rather than the original full multi-phase path.
- Business and technical plans now carry the express-track parity requirement in addition to the partial-hierarchy fix.
- The sanctioned feature-yaml switch from `track: full` / `phase: preplan` to the express path is a Slice 1 gate before automation should rely on this packet.

## Delivery Slices

| Slice | Objective | Exit Criteria |
| --- | --- | --- |
| Slice 1 | Express alignment and surface validation | Feature state is switched through feature-yaml; prompt chain and read-only boundaries are confirmed |
| Slice 2 | Resolver core rewrite | Missing levels warn instead of error; express is treated as a supported governed track; conductor remains thin |
| Slice 3 | Compliance and progressive-display parity | All three subcommands preserve output shape, gate behavior, and warning propagation |
| Slice 4 | Regression and release hardening | Temp-dir parity suite, negative safety tests, and reference docs are complete |

## Slice 1 — Express Alignment And Surface Validation

### Scope

- Record the sanctioned feature-yaml transition from the original full/preplan path to the express/expressplan path.
- Verify the retained 3-hop chain: public stub → release prompt → SKILL.md → `constitution-ops.py`.
- Confirm the authority domain remains read-only for governance, control docs, and feature state writes outside the sanctioned feature-yaml operation.

### Deliverables

- Express-plan packet aligned with the intended feature track.
- Verified prompt-chain entry contract.
- Explicit read-only boundary notes for implementation.

### Exit Criteria

- The feature no longer depends on the full multi-phase planning route.
- The prompt chain is present and no layer contains inlined resolution logic.
- No unresolved prompt-chain or write-boundary gaps remain.

## Slice 2 — Resolver Core Rewrite

### Scope

- Rewrite `constitution-ops.py` to skip missing hierarchy levels with warnings.
- Preserve additive merge order and existing subcommand surfaces.
- Extend track validation and defaults so `express` is supported when constitutions permit it.
- Keep `sensing_gate_mode` and other governance fields in the same merged result contract.

### Acceptance Criteria

- `resolve` returns exit code `0` for partial hierarchies that previously failed on missing `org/constitution.md`.
- Empty hierarchies return defaults plus warnings instead of a hard error.
- `express` is handled as a valid governed track; no downstream command needs a local workaround.
- SKILL.md and the release prompt remain thin conductors that delegate all logic to the script.

## Slice 3 — Compliance And Progressive-Display Parity

### Scope

- Implement `check-compliance` parity for hard vs informational gates.
- Implement `progressive-display` parity for phase and track filters.
- Propagate warnings, `full_constitution_available`, and express-track results consistently across outputs.

### Acceptance Criteria

- `check-compliance` returns exit code `2` only for hard-gate failures and `0` for informational-only failures.
- `progressive-display --track express` reports `track_permitted=true` when the resolved hierarchy allows express.
- `full_constitution_available` remains false when org is absent, even when domain/service data is sufficient.
- Missing-level warnings from `resolve` are preserved in downstream outputs.

## Slice 4 — Regression And Release Hardening

### Scope

- Build the parity suite with temp-directory governance fixtures.
- Add negative tests for malformed frontmatter, invalid slugs, path traversal, and no-write/read-only guarantees.
- Verify the prompt stub, release prompt, references, and tests all describe the same contract.

### Acceptance Criteria

- Regression coverage includes partial hierarchy, additive merge, express-track parity, compliance gate outcomes, and progressive-display filters.
- Negative-path tests prove invalid scope inputs are rejected and no governance/control writes occur during reads.
- Reference docs and test names align with the rewritten script behavior.

## Critical Path

1. Record the feature-yaml switch so the packet can actually run on the express path.
2. Land the resolver core fix for partial hierarchies and express-track support.
3. Land compliance and progressive-display parity on top of the same merged contract.
4. Close the parity suite with negative safety tests before implementation begins.

## Definition Of Ready For Implementation

The feature is ready for code work when the express path is recorded through the sanctioned feature-yaml flow, the planning packet is accepted, and the initial parity test targets are named.

## Definition Of Done

The feature is done when `constitution` remains read-only, resolves partial hierarchies without hard failure, supports express-track governance where permitted, and ships with fixture-backed regressions for all three subcommands.