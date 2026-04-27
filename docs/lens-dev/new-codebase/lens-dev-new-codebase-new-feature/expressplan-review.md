---
feature: lens-dev-new-codebase-new-feature
doc_type: adversarial-review
phase: expressplan
source: phase-complete
verdict: pass-with-warnings
status: approved
critical_count: 0
high_count: 2
medium_count: 3
low_count: 1
updated_at: 2026-04-27T14:15:58Z
review_format: abc-choice-v1
---

# ExpressPlan Adversarial Review — lens-dev-new-codebase-new-feature

**Source:** phase-complete  
**Artifacts reviewed:** business-plan.md, tech-plan.md  
**Format:** Each finding presents options A/B/C (proposed solutions), D (write own), E (keep as-is). Respond by letter.

---

## How to Respond

For each finding below, reply with the letter of your chosen option. You may add a clarifying note after the letter. Your response is recorded and drives the carry-forward action for that finding.

| Option | Meaning |
|---|---|
| A / B / C | Accept the proposed resolution (with its stated trade-offs) |
| D | You will provide a custom resolution — write it after "D:" |
| E | Explicitly no action — this finding is accepted as-is |

---

## Verdict: `pass-with-warnings`

The business and technical plans are coherent enough to proceed into implementation planning: they identify the missing `new-feature` surface, preserve the old command's observable outputs, and draw a clear clean-room implementation boundary. The main risks are scope definition and lifecycle bookkeeping. No finding is a hard stop today. All other findings are resolved by option selections below.

---

## Finding Summary

| ID | Severity | Title | Your Response |
|---|---|---|---|
| H1 | High | Phase metadata mismatch — full/preplan with expressflow artifacts | |
| H2 | High | `fetch-context` parity open despite old initializer exposure | |
| M1 | Medium | Extending shared initializer risks regressions in new-domain behavior | |
| M2 | Medium | Cross-feature dependency on git-orchestration and switch | |
| M3 | Medium | Help/manifests ownership unclear | |
| L1 | Low | Legacy `quickplan` alias behavior assumption unconfirmed | |

---

## High Findings

---

### H1 — Phase metadata mismatch: full/preplan with expressflow artifacts

**Location:** feature.yaml (`track: full`, `phase: preplan`)  
**Gate:** Before lifecycle promotion

The planning artifacts use ExpressPlan structure, but the feature metadata still says `track: full` and `phase: preplan`. The expressflow was user-authorized for this planning pass, but the lifecycle state has not been updated to reflect the exception.

**Choose one:**

- **A.** Update metadata through the proper Lens workflow — advance `phase` to `expressplan` and record the expressflow exception in feature.yaml before any further lifecycle promotion.  
  **Why pick this:** Keeps lifecycle state consistent with actual artifacts; the exception is documented in the source-of-truth file; future commands that read feature.yaml will behave correctly.  
  **Why not:** Requires running a lifecycle command that may itself require the feature to be on the express track; may trigger downstream validation gates not intended for this planning-only exception.

- **B.** Record the expressflow exception in a separate implementation note — leave feature.yaml at `track: full` / `phase: preplan` and document the exception in implementation-notes.md.  
  **Why pick this:** Non-invasive; does not trigger any lifecycle tooling side-effects; the expressflow scope is self-contained to this planning pass.  
  **Why not:** Leaves a persistent mismatch between artifacts and lifecycle state; any tool that automates phase promotion will be confused by the inconsistency.

- **C.** Move feature to express track explicitly — update `track: express` and `phase: expressplan` to make metadata accurate, then proceed normally on the express lifecycle.  
  **Why pick this:** Most honest representation of what actually happened; eliminates the mismatch permanently; express track is the correct track for compressed planning.  
  **Why not:** Changing the track retroactively may affect reporting, sprint planning dashboards, or governance documents that already reference `track: full`.

- **D.** Write your own response.
- **E.** Keep as-is — accept the metadata mismatch; treat the expressflow artifacts as a planning-only exception on a full-track feature with no metadata update.

---

### H2 — `fetch-context` parity open despite old initializer exposure

**Location:** tech-plan.md (scope section), sprint-plan.md (NF-4.3)  
**Gate:** Definition of Done must be updated before dev starts

The old initializer exposes `fetch-context` and downstream planners may rely on it. The technical plan defers the scope decision to NF-4.3 without updating the Definition of Done, leaving it undefined at dev-start.

**Choose one:**

- **A.** Explicitly defer `fetch-context` — update the Definition of Done to say "fetch-context is out of scope in this delivery slice"; create an explicit follow-up feature entry or action item before dev starts.  
  **Why pick this:** Closes the ambiguity cleanly; any downstream caller will get a clear "not implemented" signal and can file a follow-up; the sprint plan's NF-4.3 becomes a spike or backlog item.  
  **Why not:** If fetch-context is a silent dependency for `/businessplan` callers, a downstream planner may invoke it and get unexpected behavior without a clear error.

- **B.** Include `fetch-context` in scope — add NF-4.3 acceptance criteria explicitly and confirm full initializer parity as the delivery target.  
  **Why pick this:** Eliminates any downstream surprise; the feature delivers true command parity; no follow-up feature required.  
  **Why not:** Sprint 4 is already the heaviest sprint; adding fetch-context implementation there creates a risk of slip that blocks the entire delivery.

- **C.** Spike `fetch-context` in Sprint 1 — add a spike story to NF-1.x to determine effort; decide in/out after spike results before Sprint 2 begins.  
  **Why pick this:** Data-driven decision; avoids committing to scope without knowing effort; keeps the option open until the cost is understood.  
  **Why not:** Adds a sprint 1 story that delays other NF-1.x work; spike results may not arrive in time to influence sprint 2 planning.

- **D.** Write your own response.
- **E.** Keep as-is — leave fetch-context scope undefined in the Definition of Done; address it when NF-4.3 is reached.

---

## Medium / Low Findings

---

### M1 — Extending shared initializer risks regressions in new-domain behavior

**Location:** tech-plan.md (implementation approach)  
**Context:** `bmad-lens-init-feature` is also used by the completed `new-domain` feature.

Extending the shared `init-feature-ops.py` file to add new-feature behavior could accidentally change create-domain paths that are already working and tested.

**Choose one:**

- **A.** Add parity test skeletons for new-domain behavior before any modifications — NF-1.3 must include tests that assert existing create-domain behavior is unchanged; these must be green before any new-feature implementation is merged.  
  **Why pick this:** Makes regressions visible immediately; the existing behavior is explicitly guarded by a test that runs alongside all new tests.  
  **Why not:** Writing parity test skeletons for behavior you did not implement adds Sprint 1 cost; if the existing tests already cover create-domain paths, this may be redundant.

- **B.** Scope changes to additive-only — implement new-feature functionality in a new function or class that does not modify any existing create-domain code paths.  
  **Why pick this:** Structural isolation prevents regression by design; no test required for the existing paths because the code that drives them is not touched.  
  **Why not:** If the new-feature and new-domain paths share initialization logic, additive-only may require duplicating that logic, creating future maintainability debt.

- **C.** Run existing create-domain tests as a blocking gate in CI — add the existing test suite to the CI check for this PR; a red existing test blocks merge.  
  **Why pick this:** Zero extra test authoring; leverages tests that already exist; CI enforces the gate automatically.  
  **Why not:** If existing tests are not yet automated or not in CI, this option requires CI scaffolding before it provides any protection.

- **D.** Write your own response.
- **E.** Keep as-is — accept the regression risk; rely on code review to catch unintended create-domain changes.

---

### M2 — Cross-feature dependency on git-orchestration and switch

**Location:** tech-plan.md (ADR 4), sprint-plan.md (NF-3.x integration tests)

The new-feature command depends on git-orchestration (branch creation) and switch command parity. Integration tests will fail if those commands are not operational when NF-3.x executes.

**Choose one:**

- **A.** Test returned command strings without execution — validate that the feature produces the correct `git-orchestration` and `switch` command strings; do not execute them in tests. Mark live integration as a Sprint 4 optional.  
  **Why pick this:** ADR 4 already returns commands rather than executing them; this option keeps tests in that design and avoids any dependency on external commands being operational.  
  **Why not:** String-level tests may miss behavioral incompatibilities that only surface when the commands are executed end-to-end.

- **B.** Gate NF-3.x on confirmed availability of git-orchestration and switch — add a pre-story dependency check: confirm those features are at `dev-complete` before starting NF-3.x.  
  **Why pick this:** Prevents writing integration tests that cannot pass; forces the dependency to be resolved before integration work begins.  
  **Why not:** May block NF-3.x indefinitely if the dependent features are delayed; creates a hard cross-feature dependency that could stall the sprint.

- **C.** Create mock command stubs for git-orchestration and switch — implement minimal test doubles that satisfy the integration test surface; replace with real implementations when available.  
  **Why pick this:** Unblocks Sprint 3 regardless of dependent feature status; the mocks define the expected interface and can be replaced transparently.  
  **Why not:** Maintaining mocks adds overhead; if the real implementations differ from the mocks, tests may pass with mocks but fail with real commands.

- **D.** Write your own response.
- **E.** Keep as-is — accept the cross-feature dependency; proceed with integration tests when the dependent features are ready.

---

### M3 — Help/manifests ownership unclear

**Location:** tech-plan.md (retained command surface)

Command-surface registration for help/manifests is not assigned to this feature or to a dedicated retained-command surface sweep. If no feature owns this, the 17-command surface will be incomplete at delivery.

**Choose one:**

- **A.** Assign help/manifests registration explicitly to this feature — add a story in Sprint 4 (NF-4.2 or a new story) that claims ownership of command-surface registration for the new-feature command.  
  **Why pick this:** Closes the ownership gap; this feature is the natural owner since it is implementing the new-feature command surface.  
  **Why not:** If help/manifests registration requires knowledge of all 17 commands, this feature should not own the sweep; it would be taking on scope beyond its charter.

- **B.** Defer help/manifests to a dedicated surface-sweep feature — explicitly record that this feature does NOT own help/manifests; create a follow-up action item for a sweep feature to claim it.  
  **Why pick this:** Keeps this feature's scope clean; a sweep feature can handle all 17-command registrations in one coordinated pass.  
  **Why not:** If the sweep feature is never created, the registration gap persists indefinitely; the action item may not be tracked.

- **C.** Document help/manifests as a known gap in the Definition of Done — add an explicit "out of scope: help/manifests registration" note to the Definition of Done; the feature is complete without it.  
  **Why pick this:** Honest about scope; downstream reviewers will not assume registration is included; the gap is visible in the DoD.  
  **Why not:** A gap documented in the DoD is still a gap; if help/manifests are required for the 17-command surface to be complete, this is a delivery risk, not just a documentation note.

- **D.** Write your own response.
- **E.** Keep as-is — leave ownership undefined; assign at dev-start when the full retained-command sweep picture is clearer.

---

### L1 — Legacy `quickplan` alias behavior assumption unconfirmed

**Location:** tech-plan.md (ADR 3 — track alias resolution)  
**Note:** ADR 3 resolves track aliases from lifecycle.yaml; quickplan is listed as an alias.

The plan assumes the `quickplan` alias remains useful in the new codebase, but it is unconfirmed whether the new codebase retains the alias or only exposes `feature`/`full`/`express` track names.

**Choose one:**

- **A.** Confirm alias retention in lifecycle.yaml — verify whether `quickplan` is listed in lifecycle.yaml as an alias for `express`; if present, confirm ADR 3 covers it; if absent, mark the alias as removed.  
  **Why pick this:** Low-cost fact-check; a single lifecycle.yaml read closes the question; the ADR 3 coverage claim is validated.  
  **Why not:** If lifecycle.yaml has not yet been updated for the new codebase, the confirmation may be provisional.

- **B.** Add a quickplan-alias parity test to NF-1.3 — regardless of confirmation, add a test that verifies the alias routes correctly; the test documents the expected behavior.  
  **Why pick this:** The test doubles as documentation; if the alias is retained, the test passes; if removed, the test documents the behavior delta explicitly.  
  **Why not:** Writing a test for behavior that may be deliberately removed adds test maintenance burden if the alias is not retained.

- **C.** Explicitly remove the quickplan alias in the Definition of Done — document that the new codebase does not retain `quickplan` as an alias; users are expected to use `express` directly.  
  **Why pick this:** Deliberate behavior delta is better than an undocumented removal; makes the change visible in the DoD.  
  **Why not:** If any existing features or users rely on the quickplan alias, this commits to removing it without a migration notice.

- **D.** Write your own response.
- **E.** Keep as-is — accept the unconfirmed assumption; resolve at implementation time if the alias is missing.

---

## Accepted Risks

- Expressflow was explicitly authorized for this planning pass even though the feature is currently recorded as a full-track feature. This review accepts the planning artifact shape, but not an implicit governance phase transition.
- Clean-room parity is based on observable contracts and tests, not direct file copying. That may require more test effort than a direct port, but it keeps the implementation boundary clean.

---

## Open Questions Surfaced

- Is `fetch-context` required for this feature's completion?
- Which feature owns help/manifests registration for the retained 17-command surface?
- Should the current feature metadata be moved to express track, or should these artifacts remain a user-authorized expressflow exception on a full-track feature?
