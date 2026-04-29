---
feature: lens-dev-new-codebase-techplan
phase: dev
review_format: abc-choice-v1
status: responses-recorded
reviewed_by: bmad-lens-dev conductor
reviewed_at: 2026-04-29
---

# Dev-Closeout Adversarial Review — `lens-dev-new-codebase-techplan`

**Branch reviewed:** `feature/lens-dev-new-codebase-techplan` → `develop`
**Scope:** Full working branch diff — 10 files, 643 insertions

---

## Finding 1 — Shared utility SKILL.md files lack executable scripts

**Dimension:** Coverage gaps

The four shared utility skills (`bmad-lens-git-orchestration`, `bmad-lens-bmad-skill`, `bmad-lens-adversarial-review`, `bmad-lens-constitution`) deliver only SKILL.md prompt files. Peer skills like `bmad-lens-switch` and `bmad-lens-feature-yaml` include executable Python scripts for machine-verifiable operations. If downstream callers (e.g., CI gates or test runners) attempt to invoke these utilities programmatically, they will find no runnable entry point.

**Options:**

- A) Accept as-is: the story acceptance criteria asked for "callable" skill surfaces, not Python scripts. SKILL.md defines the interface for prompt-driven invocation. Defer executable scripts to a follow-on feature.
- B) Add minimal stub Python scripts to each shared utility skill to establish the entry point contract, even if they only print "not yet implemented."
- C) Add a note in each shared utility SKILL.md documenting the expected script path for future implementers.
- D) Reject the PR until executable scripts are present in all four shared utilities.
- E) No action — the absence of scripts is a known property of this skill type and does not warrant any artifact change.

**Response recorded:** A — The story AC explicitly required "a publish hook (or equivalent mechanism) is available to the conductor skill" and defined delivery in terms of SKILL.md callable surfaces. All four skills have SKILL.md files defining the interface. Executable scripts are a future concern addressed by follow-on features. This is not a blocking gap for dev-complete.

---

## Finding 2 — Test path anchoring is non-obvious and underdocumented

**Dimension:** Complexity and risk

The `parents[2]` anchoring in `test-techplan-ops.py` resolves correctly but the original implementation used `parents[3]` (wrong), causing all 7 initial tests to fail. The fix was applied, but the failure revealed the path calculation is easy to get wrong. Future tests added to this harness may make the same mistake.

**Options:**

- A) Add a `# VERIFIED: parents[2] = bmad-lens-techplan/` comment to the path section (already done — comments added to the path block).
- B) Add a `test_path_anchoring_sanity()` test that explicitly asserts the resolved paths exist, providing fast feedback on path drift.
- C) No action — the comments already in the file are sufficient documentation.
- D) Extract a `_resolve_paths()` helper with a docstring explaining the directory depth.
- E) Add a `conftest.py` that validates the paths at session startup.

**Response recorded:** A — The detailed directory-level comments are already present in the file (added during TK-2.5). These comments explain each level of `parents[]`. This is sufficient for this implementation scope.

---

## Finding 3 — Conductor SKILL.md's phase prerequisite check only validates `businessplan-complete`

**Dimension:** Logic flaws

Step 5 of the conductor validates `feature.yaml.phase is businessplan-complete or later`. The phrase "or later" is ambiguous when read by an AI agent — it doesn't enumerate which later phases are valid. If the feature is in `paused` or `abandoned` state (which are valid phase values), "or later" could be misinterpreted.

**Options:**

- A) Clarify the conductor step to enumerate explicitly: "businessplan-complete, techplan-complete, or finalizeplan-complete".
- B) Add `paused` handling: if phase is `paused`, surface a warning before proceeding.
- C) Accept as-is — the phrase is standard in Lens governance SKILL.md files and is not ambiguous in practice.
- D) Rewrite step 5 as a blocklist: "if phase is `preplan`, `expressplan`, `businessplan`, or `paused`, stop."
- E) Add a note documenting the interpretation: "paused features must be unpaused before proceeding."

**Response recorded:** C — The phrase "or later" follows the established Lens SKILL.md convention (see `bmad-lens-switch/SKILL.md` and `bmad-lens-businessplan/SKILL.md`). Phase ordering is defined in the governance constitution. The convention is accepted as-is.

---

## Finding 4 — The `module-help.csv` row uses em-dash in description

**Dimension:** Complexity and risk (compatibility)

The added row in `module-help.csv` uses an em-dash (`—`) character in the description field. Other rows in the CSV use hyphens. If any CSV parser assumes ASCII-only content, the em-dash may cause parsing errors.

**Options:**

- A) Replace the em-dash with a hyphen in the CSV description.
- B) Accept as-is — Python's `csv` module handles Unicode by default when using `encoding='utf-8'`.
- C) Check the existing CSV for other non-ASCII characters before deciding.
- D) Add a charset note to the module-help.csv header comment.
- E) Replace with an en-dash (`–`) which is slightly more compatible than em-dash.

**Response recorded:** C — Checking: the existing `module-help.csv` rows use `→` (U+2192 RIGHT ARROW) in `bmad-lens-quickplan`'s description. The file already contains non-ASCII characters and is consumed by UTF-8 aware tools. The em-dash is acceptable.

---

## Overall Verdict

**PASS** — No blocking findings. All four findings were resolved with non-blocking responses. The implementation is ready for PR creation.

- Stories TK-2.1 through TK-3.2: all complete
- Tests: 17 passed, 0 failed, 0 skipped
- Governance: `feature.yaml` advanced to `dev-complete`
- Branch: `feature/lens-dev-new-codebase-techplan` pushed and ready for PR
