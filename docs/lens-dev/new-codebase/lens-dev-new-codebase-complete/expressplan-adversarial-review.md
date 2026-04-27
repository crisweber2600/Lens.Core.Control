# Adversarial Review: lens-dev-new-codebase-complete / expressplan

**Reviewed:** 2026-04-27T21:36:55Z
**Source:** phase-complete
**Overall Rating:** pass-with-warnings

## Summary

The staged expressplan artifact set is coherent enough to move forward: the business plan preserves the closure-product intent, the tech plan keeps the script-versus-skill boundary explicit, and the sprint plan sequences the work into test-first slices. No critical blocker prevents the feature from advancing to finalizeplan. The remaining issues are implementation-shaping risks around archive summary naming, branch-cleanup ownership, and wrapper-level confirmation coverage, so the feature should proceed with those risks documented rather than treated as settled.

## Findings

### Critical

No critical findings.

### High

| # | Dimension | Finding | Recommendation |
|---|-----------|---------|----------------|
| H1 | Logic Flaws | The current technical plan intentionally standardizes on `summary.md`, but older discovery and reference material still mentions `final-summary.md`. If implementation starts without a deliberate audit, different readers may treat different files as canonical archive state. | Keep `summary.md` as the implementation target, but add an explicit audit task to identify and reconcile older `final-summary` references before code merges. |
| H2 | Complexity and Risk | The archive workflow boundary is still slightly ambiguous on branch cleanup. Current discovery prose emphasizes governance archival, while the finalize reference also discusses control-repo merge and branch deletion. That ambiguity can leak into implementation scope and test expectations. | Decide in implementation whether branch cleanup remains part of `/complete` orchestration or is documented as adjacent operational follow-up, then encode that boundary in tests and help text. |

### Medium / Low

| # | Dimension | Finding | Recommendation |
|---|-----------|---------|----------------|
| M1 | Coverage Gaps | The sprint plan covers focused script regressions, but wrapper-level confirmation behavior is only described, not yet tied to a concrete verification artifact. | Add a narrow orchestration regression or scripted walkthrough check that proves explicit confirmation is required before finalize runs. |
| M2 | Cross-Feature Dependencies | The plan assumes `bmad-lens-retrospective` and `bmad-lens-document-project` remain available and behaviorally stable, but it does not yet include a verification step for those dependency surfaces. | Add a handoff note or narrow smoke check that confirms those delegated skills still produce the required archive inputs. |
| L1 | Assumptions and Blind Spots | The plan assumes no schema changes are needed, which is likely correct, but the archive-state readers that consume `phase: complete` and `status: archived` are only indirectly referenced. | Include one adjacent reader check in Sprint 4 so terminal-state recognition is validated, not just asserted. |

## Accepted Risks

No accepted risks recorded yet.

## Party-Mode Challenge

Winston (Architect): The shape is good, but the archive boundary still has one unresolved seam: if one document says complete also cleans branches and another says it is governance-only, your implementation stories can drift into accidental scope.

John (Product Manager): The user promise is irreversible closure with a full historical record. If confirmation, retrospective skip handling, or summary naming feel inconsistent, users will see the command as unsafe even if the script technically works.

Paige (Technical Writer): The risky part is not the archive write itself, it is the narrative around it. Help text, references, and tests all need to tell the same story about what `/complete` does and what it does not do.

## Gaps You May Not Have Considered

1. Which artifact name do downstream readers, docs, and humans actually treat as the canonical archive summary today?
2. Is branch cleanup part of the retained `/complete` behavior, or just an operational follow-up described in one reference layer?
3. What is the smallest executable check that proves the confirmation gate is real, not just documented?
4. How will you detect if retrospective or document-project delegation regresses without breaking `complete-ops.py` itself?

## Open Questions Surfaced

- Should the implementation codify branch cleanup inside the `complete` wrapper, or document it as post-archive follow-up outside the archive script contract?
- Do any retained readers still expect `final-summary.md`, or can those references be cleaned up in favor of `summary.md` only?
- What is the narrowest wrapper-level regression that proves retrospective -> document-project -> finalize ordering in practice?