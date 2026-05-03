---
feature: lens-dev-new-codebase-bugfix-agent-uses-rg-command-despite-it-be
doc_type: expressplan-adversarial-review
status: responses-recorded
review_format: abc-choice-v1
phase: expressplan
source: phase-complete
verdict: pass-with-warnings
updated_at: "2026-05-03T23:46:00Z"
---

# ExpressPlan Adversarial Review

**Feature:** `lens-dev-new-codebase-bugfix-agent-uses-rg-command-despite-it-be`  
**Phase:** expressplan  
**Source:** phase-complete  
**Verdict:** `pass-with-warnings`

---

## Artifacts Reviewed

- `business-plan.md` ✓
- `tech-plan.md` ✓
- `sprint-plan.md` ✓

---

## Findings

### [HIGH] B3 open question should be marked resolved

The tech-plan `open_questions` list still contains "Confirm what the FinalizePlan SKILL.md step3 contract says is the correct target branch." This was verified inline during planning — FinalizePlan SKILL.md line 99 confirms step3 must commit on `{featureId}`. The open question is resolved and should be updated before FinalizePlan to avoid confusion.

**Action:** Mark open question resolved in tech-plan before FinalizePlan; annotate with SKILL.md citation.

### [MEDIUM] B6 --allow-branch-mismatch flag may become a silent bypass

The `--allow-branch-mismatch` flag design creates an escape hatch that agents could learn to pass by default. The tech and sprint plans do not specify when the flag is legitimately used vs when a mismatch must be treated as a hard error.

**Action:** Add a SKILL.md annotation (or inline code comment) defining the only permitted use of `--allow-branch-mismatch`: user-confirmed overrides when the target branch differs intentionally. Default behavior on mismatch should remain hard-error; `--allow-branch-mismatch` must only enable the warn+proceed path.

### [MEDIUM] B1 fix is documentation-only with no code-level guard

The rg fix lives entirely in AGENTS.md. No skill file explicitly states "use grep, not rg." If AGENTS.md is not loaded in a future session context, the bug recurs.

**Action:** Add a grep-vs-rg note to the `Common Constraints` or `Non-Negotiables` section of the bug-fixer SKILL.md and any other skills that perform file-search operations.

### [LOW] Sprint plan S4 is a phantom story

S4 is declared but immediately collapsed into S2. This creates false story count tracking.

**Action:** Remove S4 as a standalone story; note test coverage inline in S2 story spec.

### [LOW] No rollback plan for B3

No mitigation is documented if step3 routing change breaks an assumed `-dev` caller.

**Action:** Before coding, grep all skill SKILL.md files for callers that expect `-dev` behavior for step3. Document result in the dev story.

---

## Party-Mode Blind-Spot Challenge

**Mary (BA):** The business plan lists "transparent orchestration" as a goal, but B6's fix is a JSON warning output. Does any skill currently parse `status: warn` from commit-artifacts output? If not, the warning will be silently consumed and have zero effect on agent behavior.

**Winston (Architect):** For B3 — the routing function takes `feature_id`. Is this called with `feature/lens-dev-new-codebase-bugfix-agent-uses-rg-command-despite-it-be` (git prefix) or `lens-dev-new-codebase-bugfix-agent-uses-rg-command-despite-it-be` (bare id)? Verify before coding.

**Bob (SM):** AGENTS.md (S1) lives in the control repo; `git-orchestration-ops.py` and `feature-yaml-ops.py` (S2, S3) live in the source repo. The sprint plan does not call out that these require two separate commit+push operations in two different repos. The dev story must handle this coordination explicitly.

---

## Blind-Spot Challenge Questions

1. Does any skill currently read and branch on `status: warn` in `commit-artifacts` output? If not, the B6 warning will be consumed silently.
2. Is `feature_id` passed to `branch_for_phase_write` with or without the `feature/` git prefix?
3. For B4, which skill currently calls `feature-yaml-ops.py update --pull-request`? Does that skill's SKILL.md need to be updated too?
4. The PowerShell fix (B2) is in AGENTS.md. What prevents an agent in a future session from choosing PowerShell again if AGENTS.md isn't fully loaded?
5. The plan covers two repos (control + source). Is the express track appropriate for multi-repo scope, or does this warrant more planning?

---

## Responses

1. **Status: warn parsing** — No skill currently reads `status: warn` from commit-artifacts; the warning will need skill-level handler documentation or the design should be changed to a hard abort with a clear message. **Recommendation: change to hard error with a more descriptive message rather than a warn+proceed path.**

2. **feature_id prefix** — Confirmed bare id (no `feature/` prefix) is what the skill passes. The git branch name is `feature/{featureId}` but the routing function is called with the bare `featureId` value only.

3. **--pull-request caller** — The `lens-dev` skill calls `feature-yaml-ops.py update --pull-request` in its dev flow. The SKILL.md should be updated to document this as an explicit step after PR creation.

4. **PowerShell recurrence** — AGENTS.md terminal-errors section may not always be loaded. Mitigation: add a note in the bug-fixer SKILL.md non-negotiables to always use Python for file modification, never PowerShell heredocs.

5. **Multi-repo express track** — Express track is appropriate because all changes are small and independent. The multi-repo coordination is simple (two separate git add+commit+push sequences) and can be documented in the dev story.

---

## Verdict

**`pass-with-warnings`** — Planning artifacts are implementation-ready. Medium findings (B6 flag design, B1 AGENTS-only enforcement) are documented. Blind-spot questions have been answered. Proceed to `/finalizeplan`.
