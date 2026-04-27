---
feature: lens-dev-new-codebase-new-service
doc_type: expressplan-review
phase: expressplan-complete
source: phase-complete
verdict: pass-with-warnings
updated_at: 2026-04-27T14:13:14Z
reviewed_at: 2026-04-27T14:13:14Z
track: express
reviewer: Copilot Code Review
---

# ExpressPlan Review: lens-dev-new-codebase-new-service

## Summary

The express planning set now covers the business rationale, technical design, and sprint sequencing needed to restore `/new-service` as a clean-room retained command. The plan is coherent and implementation-ready enough to advance, but it carries explicit warnings: this feature began as `track: full`, the auto-advance requires converting it to `track: express`, and the implementation must continue to prove parity through observable tests rather than source reuse.

## Verdict

`pass-with-warnings`

## Critical

No critical blockers remain after adding `sprint-plan.md`. Continue with documented warnings.

## Findings

### H1 — Logic Flaws

**Finding:** The feature was registered as `track: full`, while this completion path is ExpressPlan. Advancing without recording the track conversion would leave lifecycle state inconsistent.

**Recommendation:** Convert the feature to `track: express`, move it through `expressplan`, then mark `expressplan-complete` using feature-yaml tooling.

**Response menu:**  
- **A. Accept** — Perform the recommended track conversion and lifecycle transition before advance.  
- B. Mitigate later — Proceed now and correct lifecycle state in a follow-up.  
- C. Reject — Keep the feature on `track: full` and do not use the express-plan completion path.  
- D. Need more evidence — Confirm the intended lifecycle state with feature-yaml tooling output.  
- E. Escalate — Request maintainer review if the track cannot be safely converted.

### H2 — Coverage Gaps

**Finding:** The plan depends on command-surface metadata being aligned, but implementation can still pass script tests while omitting prompt/help exposure.

**Recommendation:** Keep prompt, skill, module-help, and packaging checks in the implementation definition of done.

**Response menu:**  
- **A. Accept** — Keep these command-discovery and packaging checks in scope for implementation completion.  
- B. Mitigate later — Defer prompt/help/package validation until after core behavior lands.  
- C. Reject — Treat script-level tests as sufficient.  
- D. Need more evidence — Add an explicit checklist for every command-surface entry point.  
- E. Escalate — Ask maintainers to define the authoritative command-surface validation set.

### M1 — Complexity and Risk

**Finding:** Governance git automation and workspace scaffold follow-up are easy to conflate.

**Recommendation:** Preserve separate `governance_git_commands`, `workspace_git_commands`, and `remaining_git_commands` fields.

**Response menu:**  
- **A. Accept** — Keep these git command groups separate in planning and implementation artifacts.  
- B. Mitigate later — Allow temporary overlap, then normalize the fields before handoff.  
- C. Reject — Collapse all git automation into a single field.  
- D. Need more evidence — Validate field usage against downstream tooling expectations.  
- E. Escalate — Request schema clarification if tooling semantics are unclear.

### M2 — Assumptions and Blind Spots

**Finding:** Clean-room parity could be interpreted as source-level similarity instead of behavior-level equivalence.

**Recommendation:** Keep tests focused on outputs, JSON payloads, and no-write boundaries rather than function names or source text.

**Response menu:**  
- **A. Accept** — Define parity strictly in behavioral terms and test observable outputs.  
- B. Mitigate later — Start with broader parity checks, then tighten them before release.  
- C. Reject — Permit source-shape comparisons as evidence of parity.  
- D. Need more evidence — Add explicit examples of allowed and disallowed parity assertions.  
- E. Escalate — Ask for architecture review if clean-room boundaries are still ambiguous.

### L1 — Cross-Feature Dependencies

**Finding:** The baseline rewrite feature remains the source of retained-command policy and may evolve while this feature is implemented.

**Recommendation:** Re-check baseline retained-command inventory before final implementation handoff.

**Response menu:**  
- **A. Accept** — Re-validate retained-command inventory against the baseline rewrite before handoff.  
- B. Mitigate later — Reconcile only if drift is observed during implementation.  
- C. Reject — Treat the current retained-command inventory as fixed.  
- D. Need more evidence — Compare current baseline artifacts before starting Dev.  
- E. Escalate — Request cross-feature owner signoff if the retained-command policy changes.

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
