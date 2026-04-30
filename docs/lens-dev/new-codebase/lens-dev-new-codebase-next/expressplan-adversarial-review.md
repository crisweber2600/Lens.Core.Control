---
feature: lens-dev-new-codebase-next
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
updated_at: 2026-04-30T20:47:21Z
review_format: abc-choice-v1
---

# ExpressPlan Adversarial Review - lens-dev-new-codebase-next

**Source:** phase-complete  
**Artifacts reviewed:** business-plan.md, tech-plan.md, sprint-plan.md  
**Verdict:** `pass-with-warnings`

## Summary

The packet is coherent enough to complete ExpressPlan. It correctly switches the feature from the full path to the express path, defines Next as a read-only routing conductor, and sets output parity as fixture-backed observable behavior. The remaining risks are implementation risks: paused-state routing could point to a removed public surface, and the constitution resolver currently filters express tracks even though raw constitutions and lifecycle.yaml allow them. Those risks are documented as high findings, but they do not require this planning packet to fail.

## Response Record

| Option | Meaning |
| --- | --- |
| A / B / C | Accept the proposed resolution with its trade-offs |
| D | Provide a custom response after `D:` |
| E | Explicitly accept the finding with no action |

## Finding Summary

| ID | Severity | Title | Recorded Response |
| --- | --- | --- | --- |
| H1 | High | Paused-state routing can delegate to a removed public surface | A |
| H2 | High | Constitution resolver filters express tracks | A |
| M1 | Medium | Output parity needs fixture-backed cases, not prose review | A |
| M2 | Medium | Pre-confirmed delegation needs a concrete handoff mechanism | A |

## High Findings

### H1 - Paused-state routing can delegate to a removed public surface

**Dimension:** Logic flaws  
**Gate:** Before implementation release readiness

The current Next behavior contract includes a paused-state route, but the retained 17-command baseline does not preserve `pause-resume` as a public command. If the new skill returns `/pause-resume` without an installed public command or intentional internal skill route, users will hit a dead end.

**Recorded response:** A  
**Applied adjustment:** The business, technical, and sprint plans now require an explicit paused-state decision before release readiness.

**Choose one:**

- **A.** Treat paused-state routing as an implementation decision gate and require a regression for the selected behavior.  
  **Why pick this:** Keeps the rewrite honest about the retained public surface.  
  **Why not:** Adds a decision before the feature can be called release-ready.
- **B.** Keep routing to an internal pause-resume skill while leaving the public stub removed.  
  **Why pick this:** Preserves recovery behavior without expanding the public surface.  
  **Why not:** Requires the skill route to be explicit and tested.
- **C.** Report paused features as blocked and direct users to the retained recovery process.  
  **Why pick this:** Avoids routing to missing commands.  
  **Why not:** Reduces automation compared with the old behavior.
- **D.** Write your own response.
- **E.** Keep as-is.

### H2 - Constitution resolver filters express tracks

**Dimension:** Cross-feature dependencies  
**Gate:** Before automated expressplan validation is declared usable

The raw org, domain, and service constitutions list `express`, and lifecycle.yaml plus feature-yaml support it. The constitution resolver currently treats `express` and `expressplan` as unknown tracks and returns a resolved constitution that omits them. That can make an express feature look invalid to automation even after the governance file is aligned.

**Recorded response:** A  
**Applied adjustment:** The technical and sprint plans now name resolver express-track support as a dependency or scoped fix, with no local constitution fork inside Next.

**Choose one:**

- **A.** Require the resolver allow-list to include express tracks before automated express validation is claimed.  
  **Why pick this:** Fixes the shared source of truth and benefits every express feature.  
  **Why not:** May pull constitution work into the critical path.
- **B.** Keep this as a prerequisite owned by the constitution feature and block only automated express validation, not the Next planning packet.  
  **Why pick this:** Keeps feature scope narrow.  
  **Why not:** Leaves a known automation gap until the prerequisite lands.
- **C.** Add a Next-local workaround that ignores constitution resolver output for express.  
  **Why pick this:** Unblocks Next quickly.  
  **Why not:** Duplicates governance logic and risks drift.
- **D.** Write your own response.
- **E.** Keep as-is.

## Medium Findings

### M1 - Output parity needs fixture-backed cases, not prose review

**Dimension:** Coverage gaps  
**Gate:** Before implementation is called parity-complete

The plan defines parity well, but a future implementation could still pass a broad prose review while missing route-specific cases such as `expressplan-complete`, stale context, blockers, or unknown phase handling.

**Recorded response:** A  
**Applied adjustment:** The sprint plan now places full, express, complete, missing-phase, blocker, warning, paused, and unknown-state fixtures in the routing-engine slice.

**Choose one:**

- **A.** Require route fixtures as the parity gate.  
  **Why pick this:** Makes parity observable and repeatable.  
  **Why not:** Requires more setup than a single happy-path test.
- **B.** Require only full-track and express-track happy paths.  
  **Why pick this:** Faster first slice.  
  **Why not:** Misses the blocker-first behavior that makes Next valuable.
- **C.** Treat parity as manual review only.  
  **Why pick this:** Lowest implementation overhead.  
  **Why not:** Conflicts with the user's output-parity requirement.
- **D.** Write your own response.
- **E.** Keep as-is.

### M2 - Pre-confirmed delegation needs a concrete handoff mechanism

**Dimension:** Assumptions and blind spots  
**Gate:** Before the skill is considered user-flow complete

The baseline requires `/next` to delegate without asking the user a second launch question. The packet states the behavior but must force the implementation to carry a concrete pre-confirmed context flag, call convention, or equivalent mechanism into the delegated skill.

**Recorded response:** A  
**Applied adjustment:** The technical and sprint plans now include pre-confirmed handoff propagation and a no-redundant-confirmation test.

**Choose one:**

- **A.** Add a handoff context mechanism and regression test.  
  **Why pick this:** Directly validates the user-facing efficiency requirement.  
  **Why not:** Requires coordination with downstream skill entry contracts.
- **B.** Document the expectation only in the Next skill.  
  **Why pick this:** Keeps implementation simple.  
  **Why not:** Downstream skills may still re-ask.
- **C.** Allow redundant confirmation when downstream skills are interactive.  
  **Why pick this:** Avoids cross-skill coordination.  
  **Why not:** Violates the baseline acceptance criterion.
- **D.** Write your own response.
- **E.** Keep as-is.

## Accepted Risks

- Express-track constitution resolver support is not solved by these planning documents. It remains an implementation prerequisite or scoped fix before automated express validation can be considered fully usable.
- Paused-state recovery remains open, but it is now visible and must be resolved before release readiness.

## Party-Mode Challenge

Winston (Architect): The risky part is not routing happy paths. It is making sure Next never points to something the reduced command surface no longer installs.

John (PM): Users invoke Next because they want motion, not advice. Any blocker copy should be short, specific, and clearly tied to what must change before delegation can happen.

Mary (Analyst): The express switch is useful only if tests prove the express route. Otherwise the feature can claim express planning while the implementation quietly keeps full-track assumptions.

## Gaps You May Not Have Considered

1. What should `next` do when feature.yaml and feature-index disagree about phase or track?
2. How will a delegated skill know that `/next` already supplied launch consent?
3. Should stale context ever become a blocker, or is it always warning-only?
4. Where should paused-state recovery live after prompt-surface reduction?

## Open Questions Surfaced

- Decide the retained paused-state behavior before implementation release readiness.
- Add express-track support to the constitution resolver or explicitly depend on the feature that does.
- Select the target test file for route parity fixtures before code work starts.