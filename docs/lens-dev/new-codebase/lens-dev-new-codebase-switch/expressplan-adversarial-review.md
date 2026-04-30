---
feature: lens-dev-new-codebase-switch
doc_type: adversarial-review
phase: expressplan
source: phase-complete
verdict: pass-with-warnings
status: reviewed
critical_count: 0
high_count: 1
medium_count: 3
low_count: 1
carry_forward_blockers: []
updated_at: 2026-04-27T00:00:00Z
---

# ExpressPlan Adversarial Review - Switch Command

## Review Summary

**Verdict:** `pass-with-warnings`

The business plan, tech plan, and sprint plan are sufficient to advance. The feature is small, the command boundary is clear, and the required clean-room parity target is described in observable behavior rather than copied implementation text. No critical blocker remains.

The warnings below should be carried into implementation because `switch` is a trust command: if it chooses the wrong feature, references deprecated commands, or hides state drift, later phase work can start from the wrong context.

## Findings

| ID | Severity | Finding | Required Follow-Up |
|---|---|---|---|
| H1 | High | Switch references can still point users to deprecated `init-feature` naming in fallback/help text. | Replace user-facing fallback guidance with the retained command name used by the new release surface and add a text regression. |
| M1 | Medium | The phrase "switching context never modifies state" can be misread because the command writes local context and checks out a branch. | Clarify that governance state is read-only, while local context persistence and control-repo checkout are expected side effects. |
| M2 | Medium | Feature listing reports index `status`, while switching reports feature.yaml `phase`; if lifecycle updates only one source, users may see drift. | Add lifecycle follow-up to keep feature-index status and feature.yaml phase synchronized after phase completion. |
| M3 | Medium | Domain fallback is helpful for empty governance repos, but menu flow must stop in `domains` mode. | Keep the prompt-level hard stop and add a regression that no feature is inferred from domain inventory. |
| L1 | Low | The old stub only proves prompt-start behavior, not full switch semantics. | Maintain the explicit clean-room requirement map and avoid treating the old stub as a complete behavioral spec. |

## Party-Mode Blind-Spot Challenge

John (Product): The plan protects explicit selection, but the user-facing copy must be just as strict as the code. Any fallback instruction that names an old command weakens trust in the reduced surface.

Winston (Architecture): The script contract is clean, but status drift between `feature-index.yaml` and `feature.yaml` is the place future users will feel inconsistency. The switch command can surface both, but lifecycle commands must keep them aligned.

Sally (UX): The numbered menu is good because it slows down a dangerous action just enough. The design should resist convenience shortcuts that auto-select from the current branch.

## Blind-Spot Questions

1. Have all switch-facing help strings been checked against the 17-command retained surface?
2. Does every caller understand that `branch_switched: false` is a warning, not permission to continue on the old branch?
3. Is there a test proving `domains` mode stops instead of selecting the first service?
4. Do lifecycle phase-completion commands update feature-index status consistently enough that switch menus do not become stale?
5. Is stale-context detection still based on feature.yaml `updated`, and is 30 days the intended threshold for this release?

## Gate Decision

Advance with warnings. The documented follow-ups are implementation concerns, not planning blockers.
