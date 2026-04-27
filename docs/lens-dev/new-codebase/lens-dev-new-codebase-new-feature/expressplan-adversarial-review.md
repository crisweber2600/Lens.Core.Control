---
feature: lens-dev-new-codebase-new-feature
doc_type: adversarial-review
phase: expressplan
source: phase-complete
verdict: pass-with-warnings
status: draft
critical_count: 0
high_count: 2
medium_count: 3
low_count: 1
updated_at: 2026-04-27T14:15:58Z
---

# Adversarial Review: lens-dev-new-codebase-new-feature / expressplan

**Reviewed:** 2026-04-27T14:15:58Z  
**Source:** phase-complete  
**Overall Rating:** pass-with-warnings

## Summary

The business and technical plans are coherent enough to proceed into implementation planning: they identify the missing `new-feature` surface, preserve the old command's observable outputs, and draw a clear clean-room implementation boundary. The main risks are scope definition and lifecycle bookkeeping. The current feature is registered as `full`/`preplan`, while this planning pass used the user-authorized expressflow to create compressed business and technical planning artifacts; that should be treated as a documented exception unless the feature metadata is intentionally updated by the proper Lens lifecycle command.

## Findings

### Critical

| # | Dimension | Finding | Recommendation |
|---|---|---|---|
| None | - | No critical blocker found in the artifact set. | Continue with the warnings below recorded. |

### High

| # | Dimension | Finding | Recommendation |
|---|---|---|---|
| H1 | Logic Flaws | The planning artifacts use ExpressPlan structure, but feature metadata still says `track: full` and `phase: preplan`. | Before lifecycle promotion, either update metadata through the proper Lens workflow or record this as a planning-only expressflow exception. |
| H2 | Coverage Gaps | `fetch-context` parity is open even though the old initializer exposes it and downstream planners may rely on it. | Decide before implementation whether `fetch-context` is in scope; if deferred, create an explicit follow-up and avoid claiming full initializer parity. |

### Medium / Low

| # | Dimension | Finding | Recommendation |
|---|---|---|---|
| M1 | Complexity and Risk | Extending the shared initializer could regress the already-started `new-domain` behavior. | Keep existing create-domain tests unchanged and run them with all new tests. |
| M2 | Cross-Feature Dependencies | The new-feature command depends on git-orchestration and switch command parity, which may be owned by separate new-codebase features. | Verify those commands exist before implementing integration tests; otherwise test returned command strings without executing them. |
| M3 | Coverage Gaps | Help/manifests ownership is unclear. | Assign command-surface registration explicitly to this feature or to a retained-command surface sweep. |
| L1 | Assumptions and Blind Spots | The plan assumes legacy `quickplan` alias behavior remains useful. | Confirm whether the new codebase keeps the alias or only exposes `feature`/`full`/`express` names. |

## Accepted Risks

- Expressflow was explicitly authorized for this planning pass even though the feature is currently recorded as a full-track feature. This review accepts the planning artifact shape, but not an implicit governance phase transition.
- Clean-room parity is based on observable contracts and tests, not direct file copying. That may require more test effort than a direct port, but it keeps the implementation boundary clean.

## Party-Mode Challenge

John (Product): The business outcome is clear, but the acceptance line must avoid the phrase "full parity" unless `fetch-context`, help/manifests, and service marker dependencies are either implemented or explicitly carved out.

Winston (Architecture): The biggest technical trap is hidden coupling to git-orchestration and switch. The plan wisely returns those commands instead of reimplementing them, but tests need to prove command shape without requiring live GitHub access.

Amelia (Developer): The script already has create-domain helpers in the new codebase. The implementation should extend those helpers carefully rather than replace the file wholesale; otherwise the clean-room promise can accidentally become a risky rewrite of working behavior.

## Gaps You May Not Have Considered

1. Should `feature-index.yaml` duplicate detection check both `id` and `featureId` for compatibility with mixed old/new entries?
2. Should `summary.md` format be byte-for-byte equivalent from a user-output perspective, or only semantically equivalent?
3. Should dirty governance preflight also reject untracked files in nested ignored directories, or is `git status --short` sufficient?
4. Should the test suite include a fixture for an existing domain/service so parent marker creation is not always tested from an empty repo?
5. Should planning PR command generation be tested with repository names containing spaces or shell-sensitive characters?

## Open Questions Surfaced

- Is `fetch-context` required for this feature's completion?
- Which feature owns help/manifests registration for the retained 17-command surface?
- Should the current feature metadata be moved to express track, or should these artifacts remain a user-authorized expressflow exception on a full-track feature?
