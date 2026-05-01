---
feature: lens-dev-new-codebase-constitution
doc_type: epics
status: approved
goal: "Decompose the constitution rewrite into implementation epics that preserve read-only governance behavior, fix partial-hierarchy resolution, and close parity gaps for express-track callers."
key_decisions:
  - Keep the four-slice execution model from sprint-plan and convert each slice into a reviewable delivery epic.
  - Treat express-track support and sensing_gate_mode as part of the shared resolver contract, not downstream workarounds.
  - Reserve negative safety regressions and reference-doc alignment for the final release-hardening epic.
open_questions:
  - Should repo-level fixture coverage land inside the first parity wave or only after org/domain/service coverage is green?
depends_on:
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-constitution/finalizeplan-review.md
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-constitution/sprint-plan.md
blocks: []
updated_at: 2026-05-01T16:30:00Z
---

# Epics - Constitution Command Rewrite

## Epic 1 - Express Alignment and Command Surface

**Goal:** Lock the feature onto the express path in a way that automation can trust, while
preserving the thin public command surface and explicit read-only boundaries.

**Scope:**
- Confirm the sanctioned feature-yaml transition remains the source of truth for
  `track: express` and `phase: expressplan-complete` before code work starts.
- Verify the public stub, release prompt, and SKILL.md remain a thin chain with no inlined
  resolution logic.
- Document the read-only authority boundary so implementation cannot drift into governance
  writes or control-doc mutations.

**Exit Criteria:**
- Governance state and planning docs agree on the express path.
- The 3-hop command chain is verified and any drift is called out in story notes.
- The implementation packet states clearly that only `bmad-lens-feature-yaml` may mutate
  feature state.

---

## Epic 2 - Resolver Core and Merge Contract

**Goal:** Rewrite the resolver core in `constitution-ops.py` so missing hierarchy levels
warn and continue, while preserving the merged governance contract for all supported fields.

**Scope:**
- Replace the org-level hard-fail with partial-hierarchy tolerance.
- Preserve merge semantics for permitted tracks, required artifacts, gate modes, review
  participants, enforce flags, and `sensing_gate_mode`.
- Keep express-track support inside the shared resolver contract.
- Harden constitution file loading for unknown keys and malformed frontmatter reporting.

**Exit Criteria:**
- `resolve` returns exit code `0` for valid partial hierarchies.
- Merge outputs preserve the existing additive contract and explicit warnings.
- `express` can remain in `permitted_tracks` when the hierarchy allows it.

---

## Epic 3 - Compliance and Progressive Display Parity

**Goal:** Bring `check-compliance` and `progressive-display` up to parity with the rewritten
resolver so downstream callers receive stable output shapes and gate behavior.

**Scope:**
- Implement hard-vs-informational gate handling in `check-compliance`.
- Preserve phase-filtered and track-filtered display output, including
  `full_constitution_available` and warning propagation.
- Keep the SKILL.md and release prompt as thin delegators to the script.
- Align reference docs with the implemented script contract.

**Exit Criteria:**
- `check-compliance` returns exit code `2` only for hard-gate failures.
- `progressive-display --track express` reports the correct permission state.
- The prompt and skill surfaces contain orchestration only.

---

## Epic 4 - Regression and Release Hardening

**Goal:** Prove the rewritten resolver is safe to reuse by closing fixture-backed parity,
negative safety coverage, and release-readiness checks.

**Scope:**
- Build temp-directory governance fixtures for full and partial hierarchies.
- Add negative tests for malformed frontmatter, invalid slugs, path traversal, and no-write
  guarantees.
- Verify references, prompts, tests, and release notes describe the same contract.
- Capture any remaining caller-audit follow-ups before opening the final feature PR.

**Exit Criteria:**
- Parity fixtures cover resolve, compliance, and progressive-display.
- Negative-path tests prove read-only and path-safe behavior.
- Release-readiness notes identify any remaining downstream follow-up without blocking merge.