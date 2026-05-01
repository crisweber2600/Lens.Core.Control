---
feature: lens-dev-new-codebase-constitution
doc_type: adversarial-review
phase: expressplan
source: phase-complete
verdict: pass-with-warnings
status: responses-recorded
critical_count: 0
high_count: 2
medium_count: 2
low_count: 0
carry_forward_blockers: []
updated_at: 2026-05-01T00:00:00Z
review_format: abc-choice-v1
---

# ExpressPlan Adversarial Review — lens-dev-new-codebase-constitution

**Source:** phase-complete  
**Artifacts reviewed:** business-plan.md, tech-plan.md, sprint-plan.md  
**Verdict:** `pass-with-warnings`

## Summary

The packet is coherent enough to complete ExpressPlan for the constitution rewrite. It correctly centers the shared partial-hierarchy fix, keeps the command read-only, and defines clean-room parity around the three retained subcommands. The remaining risks are implementation-shaping rather than packet-failing: the current technical contract needs explicit express-track parity, the feature state switch from the original full path must be recorded through the sanctioned feature-yaml flow, and the regression plan must prove the negative safety cases that matter for a shared governance primitive.

Those issues are now visible in the packet and carried into the sprint plan. None require this planning packet to fail.

## Response Record

| Option | Meaning |
| --- | --- |
| A / B / C | Accept the proposed resolution with its trade-offs |
| D | Provide a custom response after `D:` |
| E | Explicitly accept the finding with no action |

## Finding Summary

| ID | Severity | Title | Recorded Response |
| --- | --- | --- | --- |
| H1 | High | Express-track parity is missing from the current technical contract | A |
| H2 | High | The feature state still needs the sanctioned switch from full to express | A |
| M1 | Medium | `sensing_gate_mode` parity is underspecified | A |
| M2 | Medium | Read-only and path-safety guarantees need negative regressions, not just prose | A |

## High Findings

### H1 — Express-track parity is missing from the current technical contract

**Dimension:** Cross-feature dependencies  
**Gate:** Before automated express validation is declared usable

The packet is being authored for the express path, but the inherited technical contract still mirrors the old valid-track set that excluded `express`. If the rewrite preserves that allow-list and default set unchanged, `check-compliance` and any caller that trusts constitution output can make an express feature look invalid even when the lifecycle and constitutions allow it.

**Recorded response:** A  
**Applied adjustment:** The business plan, tech plan, and sprint plan now require express-track parity explicitly: `express` must be treated as a supported track when present in the resolved hierarchy, and the parity suite must cover express-specific resolve, compliance, and display cases.

**Choose one:**

- **A.** Add express-track support to the rewritten constitution contract and regressions.  
  **Why pick this:** Fixes the shared source of truth once and avoids downstream workarounds.  
  **Why not:** Slightly expands the parity surface compared with the older implementation.
- **B.** Keep the old track allow-list and force each express feature to bypass constitution checks locally.  
  **Why pick this:** Narrower scope for the constitution rewrite.  
  **Why not:** Duplicates governance logic and undermines the shared primitive.
- **C.** Defer express-track handling to a follow-up feature and treat current automation as informational-only.  
  **Why pick this:** Keeps this feature tightly scoped to partial hierarchies.  
  **Why not:** Leaves the express path unusable for the very command family it is supposed to unblock.
- **D.** Write your own response.
- **E.** Keep as-is.

### H2 — The feature state still needs the sanctioned switch from full to express

**Dimension:** Workflow integrity  
**Gate:** Before this packet is used by expressplan automation

The planning packet is now written for the express path, but the feature was initialized on the original `track: full` / `phase: preplan` route. If that state change is not recorded through the sanctioned feature-yaml flow, the packet can look correct in docs while automation still routes from the old path.

**Recorded response:** A  
**Applied adjustment:** The sprint plan now makes the sanctioned feature-yaml transition a Slice 1 gate and an explicit critical-path item for automation use.

**Choose one:**

- **A.** Require the sanctioned feature-yaml switch before relying on the packet.  
  **Why pick this:** Keeps the docs path and governance state aligned.  
  **Why not:** Adds a coordination step outside the planning docs themselves.
- **B.** Treat the express packet as documentation-only and leave feature state on the full path for now.  
  **Why pick this:** Avoids touching governance state during planning.  
  **Why not:** Makes the packet unusable by the intended express workflow.
- **C.** Update governance state manually outside the sanctioned tooling.  
  **Why pick this:** Fastest path to visible alignment.  
  **Why not:** Violates the repo's governance write discipline.
- **D.** Write your own response.
- **E.** Keep as-is.

## Medium Findings

### M1 — `sensing_gate_mode` parity is underspecified

**Dimension:** Coverage gaps  
**Gate:** Before the rewrite is called parity-complete

The current technical plan already references `sensing_gate_mode` in merge behavior, but it is missing from the known-key contract and defaults description. That leaves a drift risk for callers that read constitution output for sensing strictness.

**Recorded response:** A  
**Applied adjustment:** The tech plan and sprint plan now carry `sensing_gate_mode` through the known-key, defaults, and regression surfaces so the rewritten script does not silently drop it.

**Choose one:**

- **A.** Make `sensing_gate_mode` explicit in the technical contract and parity suite.  
  **Why pick this:** Keeps the shared governance output stable for existing callers.  
  **Why not:** Adds one more merge field to the regression matrix.
- **B.** Leave it implicit because the current packet focuses on planning conductors, not sensing.  
  **Why pick this:** Slightly reduces planning complexity.  
  **Why not:** Risks a silent regression in a shared runtime primitive.
- **C.** Drop `sensing_gate_mode` from the rewrite and ask sensing to work around it locally.  
  **Why pick this:** Narrowest short-term scope.  
  **Why not:** Pushes shared-governance drift onto downstream callers.
- **D.** Write your own response.
- **E.** Keep as-is.

### M2 — Read-only and path-safety guarantees need negative regressions, not just prose

**Dimension:** Assumptions and blind spots  
**Gate:** Before implementation is called safe to reuse

The packet states that `constitution` is read-only and that slug validation prevents path traversal, but the current plan did not force negative-path tests for malformed frontmatter, invalid slugs, or traversal attempts. For a shared governance primitive, those guarantees need observable proof.

**Recorded response:** A  
**Applied adjustment:** The sprint plan and tech plan now require negative regressions for malformed frontmatter, invalid slugs, path traversal, and no-write/read-only behavior.

**Choose one:**

- **A.** Make the negative safety cases part of the required parity suite.  
  **Why pick this:** Proves the shared command is safe under hostile or malformed inputs.  
  **Why not:** Slightly increases test authoring work.
- **B.** Keep safety guarantees as documented conventions and test only happy-path parity.  
  **Why pick this:** Smaller initial regression surface.  
  **Why not:** Leaves the highest-value guarantees unverified.
- **C.** Defer safety coverage to a follow-up hardening pass after the rewrite ships.  
  **Why pick this:** Keeps the first implementation pass moving.  
  **Why not:** Lets a shared governance primitive ship without proving its defensive behavior.
- **D.** Write your own response.
- **E.** Keep as-is.

## Accepted Risks

- The express packet is only fully automatable once the sanctioned feature-yaml switch is recorded.
- `sensing_gate_mode` parity and negative safety cases are now planned, but they still need implementation before the feature can claim full output parity.

## Party-Mode Challenge

Winston (Architect): A shared governance primitive cannot claim parity if one retained track (`express`) still looks unknown at the script layer.

John (PM): If the packet says express but the feature state still says full, users will assume automation is broken rather than incomplete.

Quinn (QA): Read-only promises are cheap in prose. The parity suite has to prove that malformed inputs and traversal attempts do not produce writes or unexpected reads.

## Open Questions Surfaced

- Should repo-level fixtures ship in the first parity wave or after org/domain/service coverage is stable?
- Do any current callers depend on the accidental omission of `sensing_gate_mode` from resolved output?
- Which sanctioned feature-yaml operation should be used to record the full-to-express switch for this feature?