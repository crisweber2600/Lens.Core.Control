# Adversarial Review: lens-dev-new-codebase-discover / expressplan

**Reviewed:** 2026-04-28T00:00:00Z
**Source:** phase-complete
**Overall Rating:** pass-with-warnings

---

## Summary

The three expressplan artifacts — `business-plan.md`, `tech-plan.md`, and `sprint-plan.md` — are internally consistent and correctly bound the discover command's two unique behavioral contracts: bidirectional inventory sync and the governance-main auto-commit exception. The scope is appropriately tight for an express-track split feature. No critical findings were identified. One high-severity finding surfaces a scoping risk in Story 5.4.1: the plans assume the existing `SKILL.md` and `discover-ops.py` are substantially complete without a pre-assessment gate, which could cause Story 5.4.1 to expand beyond its sprint estimate. Five medium-severity findings address test coverage gaps (dry-run, no-op commit absence, no-remote edge case) and a logic conflict between SC-5 (hard success criterion) and Story 5.4.7 (audit-only stance). The phase may proceed to completion. Findings should be reflected back into sprint story acceptance criteria before Story 5.4.1 begins.

---

## Findings

### Critical

*None.*

---

### High

| # | Dimension | Finding | Recommendation |
|---|---|---|---|
| H1 | Coverage Gaps | Stories 5.4.1–5.4.3 assume the existing `bmad-lens-discover/SKILL.md` and `discover-ops.py` are substantially complete and require only review and minor patching. No pre-sprint assessment story or completeness gate exists to confirm this assumption. If the existing SKILL.md has large structural gaps (e.g., the auto-commit section is absent, headless mode is undocumented, config resolution is missing), Story 5.4.1 could expand significantly beyond the estimated 2–3 sessions. | Add a bounded pre-assessment step at the start of Story 5.4.1: read the full current `SKILL.md` and `discover-ops.py`, document any missing sections against the tech-plan spec, and explicitly scope the story to match findings. If gaps are large enough to require a new story, surface that before the sprint begins. |

---

### Medium

| # | Dimension | Finding | Recommendation |
|---|---|---|---|
| M1 | Logic Flaws | The tech-plan specifies the conditional auto-commit hash comparison as "inline in the skill," but a SKILL.md is a behavioral specification document, not executable code. The actual hash comparison must be implemented by the orchestrating agent or as a callable script expression. The tech-plan does not clarify who owns the hash computation at execution time, creating ambiguity for Story 5.4.2. | Add a clarifying sentence to tech-plan ADR-3 or Story 5.4.2: specify whether the hash comparison is expressed as Python inline in the skill's agent invocation sequence, or whether `discover-ops.py` exposes a `hash-inventory` utility subcommand. Either approach is valid, but the decision must be explicit before Story 5.4.2 begins. |
| M2 | Logic Flaws | Business-plan SC-5 states "no other command in the 17-command surface acquires or references the governance-main direct-commit path" as a hard success criterion. Story 5.4.7 (Architecture Isolation Audit) states that violations found in other commands are "surfaced as findings (not fixed here)." These are in direct conflict: if SC-5 is a hard criterion, finding violations in other commands would fail the criterion without remediation. | Resolve the tension by reframing SC-5: either narrow its scope to "no spread is introduced BY THIS FEATURE'S rewrite" (the audit confirms this feature didn't propagate the pattern), or split Story 5.4.7 into audit + conditional remediation for any incidental spread found. The current framing leaves SC-5 unverifiable within this feature's scope. |
| M3 | Coverage Gaps | The tech-plan's Story 5.4.4 and the T2 test case (`test_scan_reports_untracked_repo_with_targetprojects_prefix`) surface untracked repos for `add-entry`. ADR-2 states that `add-entry` requires `--remote-url`, and the skill must resolve this via `git remote get-url origin`. Neither the SKILL.md behavioral spec review (Story 5.4.1) nor the test suite explicitly handles the case where a local repo has no remote configured — a realistic state for locally-initialised repos. | Add an acceptance criterion to Story 5.4.1: document the no-remote case in SKILL.md (either skip the repo with a warning, or prompt the user for a URL in interactive mode). Add a test case (T2b) that verifies the script handles a local repo with no remote gracefully (does not crash; returns appropriate status in the untracked entry or a separate `untracked_no_remote` list). |
| M4 | Coverage Gaps | Business-plan SC-6 requires dry-run mode to produce no file mutations and no git commits. The sprint plan has no regression test for dry-run behavior. Story 5.4.6 mentions "a manual smoke test confirms the chain executes without errors" but this refers to the prompt chain, not dry-run. The T1–T8 test matrix has no dry-run test case. | Add T9 (`test_dry_run_produces_no_mutations`) to the test matrix in the tech-plan: run `scan` against a state that would trigger both clone and add-entry actions, but pass `--dry-run`; assert inventory file is byte-for-byte unchanged and that git commit count (by checking git log) is zero. Add Story 5.4.4b or extend Story 5.4.4 to include this test. |
| M5 | Coverage Gaps | T8 (no-op test) as designed validates that `scan` returns zero `missing_from_disk` and zero `untracked`, then infers no `add-entry` was triggered. However, it does not validate the absence of a git commit. Since the conditional auto-commit is inline in the skill (not in `discover-ops.py`), the script-level test cannot directly assert that no commit was made. SC-4 ("no commit on no-op") therefore cannot be fully validated by the T-series test suite alone. | Resolve by one of: (a) add a `hash-inventory` or `check-hash` subcommand to `discover-ops.py` so the test can compare pre/post file hashes programmatically, making the no-commit path unit-testable; or (b) document explicitly that SC-4 validation requires a skill-level integration test (outside T-series) and accept that the T-series test covers only the scan-level no-op. Either resolution is acceptable; the gap must be acknowledged in Story 5.4.5. |

---

### Low

| # | Dimension | Finding | Recommendation |
|---|---|---|---|
| L1 | Coverage Gaps | The feature.yaml milestones block has `businessplan`, `techplan`, `finalizeplan`, `dev-ready`, and `dev-complete` fields all set to null. On the express track, the expressplan phase collapses businessplan and techplan into one phase — it is unclear whether the expressplan conductor sets these milestone timestamps or leaves them null. | Add a note to business-plan.md or feature.yaml metadata: on the express track, `businessplan` and `techplan` milestones are not separately timestamped; only `dev-ready` and `dev-complete` are set during implementation. This keeps the governance record clean. |
| L2 | Complexity and Risk | The sprint plan references "EW/OW via BMB" for all SKILL.md edits but does not specify which BMB command applies when editing an existing SKILL.md that was authored by a prior baseline session. CW (Create Workflow) would be incorrect; EW or OW depends on whether the SKILL.md is considered a workflow or a skill asset. | Add a one-line process note to Story 5.4.1 specifying the correct BMB command for editing an existing skill file (likely OW or the skill's native edit path). Consult `lens.core/_bmad/bmb/` docs before starting. |
| L3 | Cross-Feature Dependencies | The `depends_on: [lens-dev-new-codebase-baseline]` reference is correct but the plans do not confirm that the baseline feature is in `dev-complete` state. If it is still in a mid-flight state, concurrent edits to shared lens-work assets could conflict. | Verify baseline feature.yaml `phase` is `dev-complete` or `complete` before Story 5.4.1 begins. No action needed if baseline is confirmed closed. |
| L4 | Assumptions and Blind Spots | The tech-plan and business-plan describe the `publish-to-governance` CLI as the alternative path that must not be used for discover. But neither document explains that the expressplan planning artifacts (business-plan.md, tech-plan.md) WILL be published via `publish-to-governance` after the expressplan phase completes — this is separate from the discover auto-commit. The distinction between "what discover does at runtime" vs. "what the expressplan conductor does to publish these planning docs" is implicit. | Minor — no change required to the plans. Add a note to the sprint plan's Definition of Done acknowledging that the expressplan conductor will use `publish-to-governance` to push the planning docs to governance after `expressplan-complete` is set. |
| L5 | Assumptions and Blind Spots | The Definition of Done in the sprint plan gates on `uv run --with pytest` passing in CI, but does not verify that the CI configuration for this repo includes the `--with pytest` inline dependency resolution for the discover test script. | Verify CI runs at least once against the discover test file before the dev-complete milestone. No structural change needed given the established baseline test pattern. |

---

## Accepted Risks

None pre-accepted at this time. All medium and high findings above should be reflected back into story acceptance criteria before Sprint 1 begins.

---

## Party-Mode Challenge

**Marta (Release Validator):** I keep reading SC-4 — "no commit on no-op" — and thinking: the test suite you've designed only validates that `scan` returns empty lists. It does NOT test that the git commit was skipped. If someone accidentally puts the commit call BEFORE the hash comparison in the skill, your T8 test still passes and SC-4 silently fails. The no-op contract is the thing most likely to be wrong in a "works on my machine" implementation, and you have no test that can catch it.

**Carlos (Governance Architect):** I want to talk about Story 5.4.7. You've framed the isolation audit as "find violations, don't fix them." But SC-5 in the business plan says the criterion is that no other command has the direct-commit path. If the audit finds one, you haven't passed SC-5. So either the success criterion is wrong (it should be "this feature didn't introduce any new spread"), or the sprint plan needs a remediation path for violations found in other commands. You can't have both a hard success criterion and an audit-only story.

**Priya (QA Lead):** The no-remote-repo edge case in `add-entry` worries me more than you've acknowledged. In real usage, a developer often initialises a local repo and hasn't pushed it anywhere yet. If `discover` tries to call `git remote get-url origin` on that repo, it fails. What does the user see? Does `discover` crash? Does it silently skip the repo? Does it prompt for a URL? There's no answer in any of these three documents. That's a user-facing behavior gap that will generate a bug report the day someone runs discover on a fresh workspace.

---

## Gaps You May Not Have Considered

1. **What happens when `git remote get-url origin` fails for an untracked local repo?** The current plans have no answer. This is the single most likely real-world failure mode for the `discover` command.

2. **Can the T8 no-op test validate commit absence without a real git context?** If not, you either need a git integration test layer or you need to expose a testable unit (a hash-check subcommand) that lets T-series tests indirectly verify the no-commit path.

3. **If Story 5.4.1 reveals the existing SKILL.md is substantially incomplete, what is the sprint's course-of-action?** The current plan has no "expand scope" path and no blocker escalation. Add a rule: if Story 5.4.1 uncovers more than 3 missing sections, create a Story 5.4.0 pre-sprint assessment artifact before proceeding.

4. **Is `headless` mode intended for CI/automation pipelines?** If so, what's the behavior when `git clone` fails mid-run in headless mode — does it continue with the next repo or abort? This resilience behavior is not documented in any of the three plans.

5. **Does the sprint plan account for the governance push required AFTER expressplan-complete is set?** The `publish-to-governance` step for the planning artifacts themselves (business-plan.md, tech-plan.md, sprint-plan.md) is not mentioned in any sprint story or Definition of Done. It is the conductor's responsibility, but it should be noted.

---

## Open Questions Surfaced

1. **Hash comparison ownership:** Is the pre/post hash comparison implemented inline by the orchestrating agent, or should `discover-ops.py` expose a `hash-inventory` subcommand? Resolve before Story 5.4.2.
2. **SC-5 scope clarification:** Should SC-5 be narrowed to "no spread introduced by THIS feature"? Or does it require a broader audit and remediation pass that is out of scope for this sprint? Resolve before Story 5.4.7.
3. **No-remote repo behavior:** What should `discover` do when an untracked local repo has no git remote? Skip with warning? Prompt for URL? Resolve before Story 5.4.1 acceptance criteria are finalized.
4. **Dry-run test coverage:** Is a regression test for `--dry-run` required as a hard gate, or is manual smoke testing acceptable? Resolve before Story 5.4.4 begins.
5. **Headless error resilience:** In headless mode, does a failed `git clone` for one repo abort the entire run or continue to the next repo? Resolve before Story 5.4.1 acceptance criteria are finalized.
