# Adversarial Review: lens-dev-new-codebase-new-service / expressplan

**Reviewed:** 2026-04-27T14:13:14Z  
**Source:** phase-complete  
**Overall Rating:** pass-with-warnings

## Summary

The express planning set now covers the business rationale, technical design, and sprint sequencing needed to restore `/new-service` as a clean-room retained command. The plan is coherent and implementation-ready enough to advance, but it carries explicit warnings: this feature began as `track: full`, the auto-advance requires converting it to `track: express`, and the implementation must continue to prove parity through observable tests rather than source reuse.

## Findings

### Critical

| # | Dimension | Finding | Recommendation |
|---|---|---|---|
| None | - | No critical blockers remain after adding `sprint-plan.md`. | Continue with documented warnings. |

### High

| # | Dimension | Finding | Recommendation |
|---|---|---|---|
| H1 | Logic Flaws | The feature was registered as `track: full`, while this completion path is ExpressPlan. Advancing without recording the track conversion would leave lifecycle state inconsistent. | Convert the feature to `track: express`, move it through `expressplan`, then mark `expressplan-complete` using feature-yaml tooling. |
| H2 | Coverage Gaps | The plan depends on command-surface metadata being aligned, but implementation can still pass script tests while omitting prompt/help exposure. | Keep prompt, skill, module-help, and packaging checks in the implementation definition of done. |

### Medium / Low

| # | Dimension | Finding | Recommendation |
|---|---|---|---|
| M1 | Complexity and Risk | Governance git automation and workspace scaffold follow-up are easy to conflate. | Preserve separate `governance_git_commands`, `workspace_git_commands`, and `remaining_git_commands` fields. |
| M2 | Assumptions and Blind Spots | Clean-room parity could be interpreted as source-level similarity instead of behavior-level equivalence. | Keep tests focused on outputs, JSON payloads, and no-write boundaries rather than function names or source text. |
| L1 | Cross-Feature Dependencies | The baseline rewrite feature remains the source of retained-command policy and may evolve while this feature is implemented. | Re-check baseline retained-command inventory before final implementation handoff. |

## Accepted Risks

- The lifecycle track conversion from `full` to `express` is accepted because the user explicitly authorized auto-advance for this planning set and the feature scope is narrow.
- `pass-with-warnings` is accepted because warnings are actionable implementation guardrails, not blockers to planning completion.

## Party-Mode Challenge

Mary (Analyst): The user promise is not just that a script exists. The retained command must be findable, invokable, and safe for a new user walking the setup path. I would keep asking whether every place that advertises the 17-command surface includes `new-service` consistently.

Winston (Architect): The clean-room constraint changes the implementation posture. The safest path is test-first behavioral parity: write assertions for artifacts and command output, then implement independently. That gives maintainers confidence without source copying.

Paige (Technical Writer): The handoff needs to explain the service-versus-feature boundary plainly. If future developers blur that boundary, `new-service` could start mutating lifecycle state and create confusing downstream failures.

## Gaps You May Not Have Considered

1. Does command discovery validation include both source module metadata and installed prompt stubs?
2. Should `new-service` preserve missing-parent-domain auto-creation exactly, or should docs call out that behavior as intentional compatibility?
3. How will reviewers detect accidental feature-index writes in service creation tests?
4. Does the new implementation need a read-context regression immediately after service creation to prove downstream commands see the active service?

## Open Questions Surfaced

None blocking. The implementation should treat the questions above as review prompts during Dev rather than unresolved planning inputs.