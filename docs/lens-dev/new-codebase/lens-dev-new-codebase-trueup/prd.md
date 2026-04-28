---
feature: lens-dev-new-codebase-trueup
doc_type: prd
status: draft
goal: "Close verified delivery gaps in the 5 non-preplan new-codebase features: behavioral parity audit, prompt publishing, complete skill scaffolding, and constitution track ADR"
key_decisions:
  - Scope is limited to the 5 non-preplan features: new-domain, new-service, switch, new-feature, complete
  - Shared infrastructure (adversarial-review, publish-to-governance, feature-yaml) is explicitly out of scope — belongs to individual preplan feature planning cycles
  - Behavioral parity target is the old-codebase implementation verified by code analysis (NOT the RELEASE module binary)
  - The complete prerequisite decision (graceful-degradation vs. hard-prerequisite) is a required ADR owned by this feature
  - Constitution permitted_tracks divergence (express/expressplan in new vs absent in old) requires an explicit ADR
  - Python 3.12 requirement in new init-feature-ops.py must be explicitly documented or reverted
  - SAFE_ID_PATTERN tightening in switch-ops.py (no dots/underscores) requires backward-compatibility validation
stepsCompleted: [step-01-init, step-02-discovery, step-02b-vision, step-02c-executive-summary, step-03-success, step-04-journeys, step-05-domain, step-06-innovation, step-07-project-type, step-08-scoping, step-09-functional, step-10-nonfunctional, step-11-polish]
open_questions:
  - Do any existing feature IDs in governance contain dots or underscores that would break under the new SAFE_ID_PATTERN?
  - Is the Python 3.12 bump intentional (new language feature used) or accidental drift?
  - Should lc-agent-core-repo be investigated as part of this feature or deferred to a separate feature?
  - Should SC-3b behavioral parity require automated tests or is a manual audit report sufficient?
depends_on: [brainstorm, research, product-brief]
blocks: []
updated_at: 2026-04-27T00:00:00Z
---

# Product Requirements Document — True Up (lens-dev-new-codebase-trueup)

---

## 1. Executive Summary

True Up closes the verified delivery gaps in the 5 non-preplan new-codebase features. These features have advanced through planning phases but their implementation state has not been validated against the old-codebase baseline. Deep code analysis (April 2026) plus the old-codebase discovery artifact set now produce a complete gap map: two features have critical script gaps (new-feature and complete are missing their primary commands), one of those gaps also removes the old init-feature context-loading contract (`fetch-context` and `read-context`), one feature has a prompt publishing gap visible to all IDE users (switch), two features have behavioral parity assumptions that remain unverified (new-domain and new-service), and all five features share cross-cutting schema drift (constitution permitted_tracks, Python version, identifier validation rules).

True Up produces four categories of output: (1) a behavioral parity audit report with evidence for all 5 features, (2) closed prompt publishing gaps so switch, new-feature, and complete are discoverable by IDE agents, (3) a SKILL.md package for `bmad-lens-complete` with prerequisite ADR, and (4) design decisions that unblock subsequent dev phases.

---

## 2. Background and Problem Statement

### 2.1 Governance Phase ≠ Implementation Completeness

Governance phase labels track **planning status**, not verified implementation. The 5 non-preplan features have all passed through planning:

| Feature | Governance Phase | Implementation Reality |
|---------|-----------------|----------------------|
| `new-domain` | `complete` | Script present; behavioral parity unverified |
| `new-service` | `complete` | Script present; behavioral parity unverified |
| `switch` | `complete` | Script present; prompt not published to `.github/prompts/` |
| `new-feature` | `finalizeplan-complete` | Primary `create` command **absent** from new-codebase script |
| `complete` | `finalizeplan-complete` | **Entire skill absent** (no script, no SKILL.md, no tests) |

### 2.2 Critical Gaps Found by Code Analysis

**new-feature (init-feature-ops.py):** The old-codebase discovery docs and source both describe `bmad-lens-init-feature` as a first-class planning surface with `create`, `fetch-context`, `read-context`, published prompts, and the `references/auto-context-pull.md` and `references/init-feature.md` guidance files. The new-codebase script (798 lines) exposes only `create-domain` and `create-service`. The `create` subcommand — which performs canonical featureId construction, lifecycle track resolution, feature.yaml authoring, feature-index.yaml registration, control-repo 2-branch creation, and planning PR creation — is entirely absent. The following are also absent: `fetch-context`, `read-context`, and all supporting functions (`resolve_feature_identity`, `make_feature_yaml`, `load_lifecycle`, `resolve_start_phase`, etc.). Because BusinessPlan and other planning flows rely on Auto-Context Pull, absence of `fetch-context` and `read-context` is a functional regression, not just a missing convenience command.

**complete (complete-ops.py):** The new-codebase `bmad-lens-complete` skill folder contains only `references/finalize-feature.md`. There is no `complete-ops.py`, no SKILL.md, no tests. The old-codebase implementation is 450 lines covering `check-preconditions`, `finalize`, and `archive-status`, all with dry-run support.

**switch (switch-ops.py):** The new-codebase implementation is more capable than the old (820 vs 732 lines, adds governance repo auto-discovery, richer output shape), but `lens-switch.prompt.md` is not mirrored to `.github/prompts/`. IDE agents cannot discover the switch command.

**new-domain / new-service (init-feature-ops.py):** Both `create-domain` and `create-service` are present and functional. However: (a) new-codebase constitutions include `express` and `expressplan` in `permitted_tracks`, which old-codebase constitutions do not; (b) `requires-python` was bumped to `>=3.12` from `>=3.10`; (c) reference documents (`references/auto-context-pull.md`, `references/init-feature.md`) are absent from new-codebase. No parity validation has been run to confirm field-for-field schema equivalence.

### 2.3 Cross-Cutting Behavioral Divergences

| Divergence | Old Behavior | New Behavior | Risk |
|-----------|-------------|-------------|------|
| `SAFE_ID_PATTERN` (switch-ops.py) | `^[a-z0-9][a-z0-9._-]{0,63}$` | `^[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$` | Feature IDs with dots/underscores break |
| `permitted_tracks` in constitutions | `[quickplan, full, hotfix, tech-change]` | Adds `express, expressplan` | New constitutions diverge from old |
| `build_context_paths` return type (switch) | `(summaries[], full_docs[])` tuple | `{related:[], depends_on:[], blocks:[]}` dict | Downstream callers may break |
| `requires-python` (init-feature-ops.py) | `>=3.10` | `>=3.12` | Environments with Python 3.10/3.11 fail |

---

## 3. Goals and Non-Goals

### 3.1 Goals

1. Produce a verified behavioral parity audit report for all 5 in-scope features
2. Close prompt publishing gaps so switch, new-feature, and complete are IDE-discoverable
3. Author `bmad-lens-complete` SKILL.md with full command contract
4. Scaffold `bmad-lens-complete` test stubs covering key paths
5. Publish ADR for `complete` prerequisite handling (graceful-degradation vs. hard-prerequisite)
6. Publish ADR for constitution `permitted_tracks` divergence
7. Document Python 3.12 and `SAFE_ID_PATTERN` design decisions
8. Produce a "fully migrated" gate specification for all future retained command migrations
9. Explicitly classify the missing init-feature context-loading surface as a blocker for `lens-dev-new-codebase-new-feature` until restored or formally deferred

### 3.2 Non-Goals

- Implementing `complete-ops.py` — belongs to `lens-dev-new-codebase-complete` dev phase
- Implementing `init-feature-ops.py create` — belongs to `lens-dev-new-codebase-new-feature` dev phase
- Any of the 12 preplan-phase features
- Shared infrastructure (adversarial-review, publish-to-governance, feature-yaml ops)
- Full behavioral test automation for parity (a manual audit report is sufficient for this phase)

---

## 4. User Stories

### US-1 — Governance Reviewer

**As** a governance reviewer,  
**I want** a parity audit report with per-feature evidence of implementation state,  
**So that** I can confidently confirm or correct the governance phase labels for all 5 non-preplan features.

**Acceptance Criteria:**
- Parity audit report covers all 5 features with a pass/fail/gap finding per feature
- Each gap finding includes the specific file, function, or schema field that differs
- The report distinguishes structural gaps (file missing) from behavioral gaps (schema diverges) from governance gaps (label inconsistent with implementation)
- Report is committed to `docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/`

---

### US-2 — IDE Agent User (switch)

**As** an IDE agent user invoking `/switch`,  
**I want** the switch prompt to appear in IDE prompt discovery,  
**So that** I can activate the feature context switcher without knowing the underlying script path.

**Acceptance Criteria:**
- `lens-switch.prompt.md` exists at `.github/prompts/lens-switch.prompt.md`
- Content matches the source at `_bmad/lens-work/prompts/lens-switch.prompt.md`
- No functional behavior change to `switch-ops.py`

---

### US-3 — IDE Agent User (new-feature and complete)

**As** an IDE agent user,  
**I want** prompt stubs for `/new-feature` and `/complete` to appear in IDE prompt discovery,  
**So that** I can invoke these commands even if their full implementation is pending.

**Acceptance Criteria:**
- `lens-new-feature.prompt.md` exists at `.github/prompts/lens-new-feature.prompt.md` and at `_bmad/lens-work/prompts/lens-new-feature.prompt.md`
- `lens-complete.prompt.md` exists at `.github/prompts/lens-complete.prompt.md` and at `_bmad/lens-work/prompts/lens-complete.prompt.md`
- Both stubs correctly route to their backing SKILL.md (or to a "not yet implemented" message if SKILL.md is absent)

---

### US-4 — Dev Team (complete feature)

**As** a developer about to implement `lens-dev-new-codebase-complete`,  
**I want** a documented design decision on how `complete` handles missing `bmad-lens-retrospective` and `bmad-lens-document-project`,  
**So that** I can start dev without a mid-sprint design conflict.

**Acceptance Criteria:**
- ADR artifact published: either "graceful degradation" (complete proceeds with a warning when prerequisites are absent) or "hard prerequisite" (complete blocks until prerequisites are available)
- ADR references the old-codebase `complete-ops.py` behavior and the `finalize-feature.md` reference doc
- ADR is committed to `docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/adr-complete-prerequisite.md`
- `lens-dev-new-codebase-complete` feature.yaml contains a blocker annotation referencing this ADR until dev activates

---

### US-5 — Dev Team (constitution track list)

**As** a developer implementing future domain or service setup,  
**I want** a documented decision on whether new constitutions should include `express` and `expressplan` tracks,  
**So that** I know which constitution template is canonical going forward.

**Acceptance Criteria:**
- ADR published: either (a) new-codebase template is canonical (express/expressplan included), or (b) old-codebase template is canonical (express/expressplan excluded), or (c) constitution template is parameterized by lifecycle
- ADR committed to `docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/adr-constitution-tracks.md`

---

### US-6 — Dev Team (complete SKILL.md)

**As** a developer implementing `complete-ops.py`,  
**I want** a SKILL.md that specifies the full command contract, invocation protocol, and failure modes,  
**So that** I have a clear specification to implement against.

**Acceptance Criteria:**
- `bmad-lens-complete/SKILL.md` authored and committed to new-codebase source
- SKILL.md covers all 3 commands: `check-preconditions`, `finalize`, `archive-status`
- SKILL.md specifies the prerequisite handling per the ADR from US-4
- SKILL.md references the existing `references/finalize-feature.md` and incorporates its contract content
- Test stubs scaffolded at `bmad-lens-complete/scripts/tests/test-complete-ops.py` (empty functions with docstrings)

---

## 5. Functional Requirements

### FR Group 1 — Prompt Publishing (switch, new-feature, complete)

**FR-1:** Mirror `lens-switch.prompt.md` to `.github/prompts/`

- Source: `_bmad/lens-work/prompts/lens-switch.prompt.md` (confirmed present in new-codebase)
- Target: `.github/prompts/lens-switch.prompt.md` (confirmed absent)
- Operation: copy or symlink; content must be identical
- Priority: **High** — switch is `complete` in governance; this is the only remaining gap

**FR-2:** Publish `lens-new-feature.prompt.md` stub

- Create at: `_bmad/lens-work/prompts/lens-new-feature.prompt.md` AND `.github/prompts/lens-new-feature.prompt.md`
- Content: stub that delegates to `bmad-lens-init-feature` with intent `new-feature`; must note if `create` subcommand is not yet implemented
- Priority: **High** — new-feature is finalizeplan-complete; prompt stub enables discoverability before full implementation

**FR-3:** Publish `lens-complete.prompt.md` stub

- Create at: `_bmad/lens-work/prompts/lens-complete.prompt.md` AND `.github/prompts/lens-complete.prompt.md`
- Content: stub that delegates to `bmad-lens-complete`; must note prerequisite design decision is pending until ADR resolves
- Priority: **High** — complete is finalizeplan-complete; stub enables discoverability

---

### FR Group 2 — `bmad-lens-complete` Package (SKILL.md + tests)

**FR-4:** Author `bmad-lens-complete/SKILL.md`

- Must specify the full command contract for 3 operations: `check-preconditions`, `finalize`, `archive-status`
- Must incorporate content from `references/finalize-feature.md` (confirmation gate, pre-conditions, process steps)
- Must specify prerequisite handling per the ADR outcome (FR-8)
- Must specify the script invocation pattern: `uv run ./scripts/complete-ops.py <command> --governance-repo <path> --feature-id <id>`
- Must specify that `finalize` is irreversible and requires explicit user confirmation
- Priority: **High**

**FR-5:** Scaffold `bmad-lens-complete` test stubs

- Create `bmad-lens-complete/scripts/` and `bmad-lens-complete/scripts/tests/` if absent
- Create `bmad-lens-complete/scripts/tests/test-complete-ops.py` with stub functions (not implemented)
- Required test function signatures:
  - `test_check_preconditions_pass()` — feature in dev phase, retrospective.md present
  - `test_check_preconditions_warn_no_retrospective()` — feature in dev phase, retrospective.md absent
  - `test_check_preconditions_fail_wrong_phase()` — feature in non-completable phase
  - `test_finalize_dry_run()` — verify dry_run returns correct change preview without writing
  - `test_finalize_archives_feature()` — end-to-end: phase→complete, index→archived, summary.md written
  - `test_archive_status_archived()` — feature is complete, returns archived=True
  - `test_archive_status_not_archived()` — feature is dev, returns archived=False
  - `test_prerequisite_missing_degradation()` OR `test_prerequisite_missing_hard_block()` — per ADR outcome
- Priority: **Medium** (stubs only; implementation deferred to complete dev phase)

---

### FR Group 3 — Design Decisions (ADRs)

**FR-6:** ADR — `complete` prerequisite handling

- Decision: graceful-degradation vs. hard-prerequisite for `bmad-lens-retrospective` and `bmad-lens-document-project`
- Context: old-codebase `complete-ops.py` has no delegation logic (no subprocess calls to retrospective or document-project); the `check-preconditions` command checks for `retrospective.md` file existence but does not invoke the skill. The graceful path already exists in the script: `retrospective_skipped = not (feature_dir / "retrospective.md").exists()` with a warning flag in the output.
- Evidence: `complete-ops.py check-preconditions` returns `status: warn` (not fail) when `retrospective.md` is absent; finalize proceeds with `retrospective_skipped=True`. This implies the old-codebase already chose graceful-degradation.
- Recommendation: Document "graceful-degradation is the canonical behavior" based on old-codebase evidence.
- Output: `adr-complete-prerequisite.md` committed to trueup docs path
- Companion governance action: update `TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-complete/feature.yaml` with a blocker note referencing this ADR until complete dev activates
- Priority: **High** — blocks complete dev activation

**FR-7:** ADR — constitution `permitted_tracks` divergence

- Decision: which `permitted_tracks` template is canonical for new domains/services
- Options: (a) keep new-codebase template (`express, expressplan` included), (b) revert to old-codebase template, (c) parameterize from lifecycle.yaml
- Evidence: new-codebase added tracks in commit history; old-codebase did not have them; whether `express` and `expressplan` are active phases in `lifecycle.yaml` should determine whether they belong in constitutions
- Required validation step: read `lifecycle.yaml` track and phase definitions before choosing the ADR outcome
- Output: `adr-constitution-tracks.md` committed to trueup docs path
- Priority: **Medium**

**FR-8:** Decision note — Python 3.12 requirement

- Document whether `requires-python = ">=3.12"` in `init-feature-ops.py` is intentional (uses 3.12+ language features) or accidental drift from `>=3.10`
- If intentional: document the feature(s) used
- If accidental: revert to `>=3.10` and commit the fix
- Output: section in parity audit report
- Priority: **Low**

**FR-9:** Decision note — `SAFE_ID_PATTERN` tightening in `switch-ops.py`

- Old pattern: `^[a-z0-9][a-z0-9._-]{0,63}$` (dots, underscores allowed)
- New pattern: `^[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$` (hyphens only, must not start/end with hyphen)
- Required action: scan all feature IDs in `TargetProjects/lens/lens-governance/feature-index.yaml` and all `feature.yaml` files under `TargetProjects/lens/lens-governance/features/` for IDs containing dots or underscores
- If any found: document as a breaking change requiring migration; add to parity report
- If none found: document as safe tightening
- Output: section in parity audit report
- Priority: **High** — potential silent breakage on switch for existing features

---

### FR Group 4 — Behavioral Parity Audit

**FR-10:** Parity audit — `new-domain` (`create-domain`)

Verify new-codebase `init-feature-ops.py create-domain` produces outputs matching old-codebase contract:

| Schema | Required Fields | Verification Method |
|--------|----------------|-------------------|
| `domain.yaml` | `id`, `name`, `domain`, `status`, `owner`, `permitted_tracks` | Side-by-side dry-run output comparison |
| `constitution.md` | Header format, `permitted_tracks` block, governance section | Template diff against old-codebase generated output |
| `context.yaml` | `domain`, `service: null`, `updated_at`, `updated_by` | Dry-run output field check |
| `.gitkeep` scaffolds | `TargetProjects/{domain}/` and `docs/{domain}/` | File path check in dry-run output |

Special: Document the `permitted_tracks` divergence per FR-7 ADR. Record whether the `context_path` field is always present in new-codebase (it is) vs conditional in old-codebase.

Priority: **High**

**FR-11:** Parity audit — `new-service` (`create-service`)

Verify new-codebase `init-feature-ops.py create-service` produces outputs matching old-codebase contract:

| Schema | Required Fields | Verification Method |
|--------|----------------|-------------------|
| `service.yaml` | `id`, `name`, `domain`, `service`, `status`, `owner`, `permitted_tracks` | Dry-run output comparison |
| Service constitution | Header, `permitted_tracks`, inheritance section | Template diff |
| `context.yaml` | `domain`, `service`, `updated_at`, `updated_by` | Dry-run field check |
| Auto-domain creation | `created_domain_marker` flag, domain.yaml created on demand | Dry-run output flag check |

Special: Confirm `domain_must_exist` guard behavior matches old-codebase; document `created_domain_marker` as a new-codebase addition.

Priority: **High**

**FR-12:** Governance phase and init-feature contract audit — `new-feature` and `complete`

- For `new-feature`: verify the old-codebase canonical init-feature contract using both discovery docs and source. Minimum expected surface: `create`, `fetch-context`, `read-context`, `references/auto-context-pull.md`, and `references/init-feature.md`.
- For `new-feature`: compare that contract to the new-codebase source and classify each missing item as structural gap, behavioral gap, or functional regression.
- Treat missing `fetch-context` and `read-context` as a required finding in the parity audit report. They are part of the old Auto-Context Pull planning flow and therefore count as a functional regression, not merely missing helper commands.
- Output must include a blocker recommendation for `TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-new-feature/feature.yaml` until the init-feature context-loading contract is restored or explicitly deferred.
- For `complete`: verify governance phase label against implementation state. `finalizeplan-complete` is consistent with "planning done, dev not started" given the absence of `complete-ops.py`.
- Output: governance label verification table in parity audit report; the report must formalize the current label verdicts rather than assume them implicitly
- Priority: **Medium**

---

### FR Group 5 — Missing Reference Documents

**FR-13:** Restore `init-feature` reference documents

Both reference documents present in old-codebase are absent from new-codebase `bmad-lens-init-feature`:

- `references/auto-context-pull.md` — describes automatic context.yaml population from active session
- `references/init-feature.md` — describes the new-feature creation workflow for agent sessions

Action: author new versions of both reference documents for new-codebase, incorporating new behaviors (governance repo auto-discovery, `--workspace-root` support, `created_domain_marker` output).

Priority: **Low** (does not block any existing functionality, but needed for complete SKILL.md package)

---

### FR Group 6 — Parity Gate Specification

**FR-14:** Author "fully migrated" gate specification

Define the required checklist for a retained command to be declared fully migrated. Based on code analysis findings, the gate must cover:

**Structural (verifiable by file existence):**
- [ ] SKILL.md present in `_bmad/lens-work/skills/{skill}/SKILL.md`
- [ ] Primary script present in `_bmad/lens-work/skills/{skill}/scripts/{script}.py`
- [ ] Test file present in `_bmad/lens-work/skills/{skill}/scripts/tests/`
- [ ] Prompt stub present in `_bmad/lens-work/prompts/lens-{command}.prompt.md`
- [ ] Prompt mirrored to `.github/prompts/lens-{command}.prompt.md`
- [ ] Required review artifact published for the phase gate (`businessplan-adversarial-review.md`, etc.)
- [ ] Governance phase label consistent with implementation state

**Behavioral (verifiable by dry-run comparison):**
- [ ] All required output fields present and correctly typed
- [ ] `--dry-run` flag supported where the old-codebase had it
- [ ] Failure modes return structured `{status: fail, error: "...", message: "..."}` payloads
- [ ] Backward-compatible with any `--governance-repo`-required call sites

**Design decisions resolved:**
- [ ] Breaking changes (schema, identifier rules, Python version) documented with explicit decision
- [ ] Cross-skill prerequisites documented in SKILL.md

Output: `parity-gate-spec.md` committed to trueup docs path

Priority: **High** — this is the reusable artifact that makes the entire True Up initiative coherent

---

### FR Group 7 — Parity Audit Report

**FR-15:** Publish parity audit report

Consolidate all FR Group 4 findings into a single `parity-audit-report.md`:

Required sections:
- **Executive verdict table** — per-feature: structural pass/fail, behavioral pass/fail, governance label correct/incorrect, open items
- **new-domain findings** — schema comparison results, constitution track divergence, Python version note
- **new-service findings** — schema comparison results, auto-domain behavior note
- **switch findings** — prompt publishing gap (closed by FR-1), SAFE_ID_PATTERN note, context_paths shape change note
- **new-feature findings** — create command absent (confirmed), governance label formally verified, `fetch-context`/`read-context` absence treated as functional regression, blocker recommendation, list of missing functions and references
- **complete findings** — skill entirely absent (confirmed), governance label correct, prerequisite decision (from ADR)
- **Cross-cutting findings** — Python 3.12, SAFE_ID_PATTERN, constitution tracks, build_context_paths return shape

Output: `parity-audit-report.md` committed to trueup docs path

Priority: **High** — primary deliverable of this feature

---

## 6. Non-Functional Requirements

### NFR-1 — Audit Evidence Collection Is Read-Only

All parity audit evidence collection is read-only. No `feature.yaml`, `feature-index.yaml`, or governance file may be mutated while gathering parity findings. Any blocker annotations or governance phase label corrections required by this PRD must be executed as separate intentional follow-up commits after the audit artifacts land.

### NFR-2 — Prompt Stubs Are Delegation-Only

New prompt files (`lens-new-feature.prompt.md`, `lens-complete.prompt.md`) must not implement behavioral logic. They are invocation stubs that route to SKILL.md. If the backing SKILL.md has no implementation, the stub must surface a clear "not yet implemented" message rather than attempting to run a missing script.

### NFR-3 — Atomic Commits on Plan Branch

All new files must be committed to the `lens-dev-new-codebase-trueup-plan` branch in the control repo before any governance publication step. Do not mix parity audit artifacts with governance publication in the same commit.

### NFR-4 — SKILL.md Package Completeness

Every SKILL.md authored by this feature must be a complete, usable agent-discovery document — not a placeholder. Agents load SKILL.md at runtime. A partial SKILL.md is worse than no SKILL.md because it provides false confidence. For `bmad-lens-complete`, SKILL.md completeness is contingent on FR-6 resolving the prerequisite ADR first; do not commit a partial contract before the ADR outcome is written.

### NFR-5 — ADR Permanence

ADR artifacts authored by this feature must be treated as permanent governance documents. They may be superseded by later ADRs but never silently deleted.

### NFR-6 — Context_paths Shape Change Documentation

The breaking change in `build_context_paths` return type between old and new `switch-ops.py` must be documented in the parity audit report. Any downstream callers (prompts, SKILL.md invocation patterns) that reference the old `{summaries, full_docs}` shape must be updated to use `context_to_load` (the backward-compat adapter field). If no current callers exist in new-codebase, the parity audit report must state that explicitly rather than imply follow-up work exists.

---

## 7. Success Metrics

### SC-1 — Parity Audit Report Published

```
[ ] parity-audit-report.md committed to docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/
[ ] All 5 features have an explicit verdict (pass / gap / confirmed-absent)
[ ] All cross-cutting divergences documented with explicit decision notes
```

### SC-2 — Governance Phase Labels Verified

```
[ ] new-domain: formally documented as correctly labeled complete (script present, parity verified)
[ ] new-service: formally documented as correctly labeled complete (script present, parity verified)
[ ] switch: formally documented as correctly labeled complete (script present, prompt gap closed)
[ ] new-feature: formally documented as correctly labeled finalizeplan-complete, with init-feature context-loading blocker recorded
[ ] complete: formally documented as correctly labeled finalizeplan-complete, with ADR blocker annotation recorded
```

### SC-3 — Structural Parity Gate Closed (all 3 prompt gaps)

```
[ ] .github/prompts/lens-switch.prompt.md exists (mirrored)
[ ] .github/prompts/lens-new-feature.prompt.md exists (new stub)
[ ] .github/prompts/lens-complete.prompt.md exists (new stub)
[ ] _bmad/lens-work/prompts/lens-new-feature.prompt.md exists
[ ] _bmad/lens-work/prompts/lens-complete.prompt.md exists
```

### SC-4 — `bmad-lens-complete` SKILL.md Authored

```
[ ] bmad-lens-complete/SKILL.md exists in new-codebase source
[ ] SKILL.md covers check-preconditions, finalize, archive-status
[ ] SKILL.md references prerequisite ADR decision
[ ] Test stubs scaffolded at bmad-lens-complete/scripts/tests/test-complete-ops.py
```

### SC-5 — Design Decisions Published

```
[ ] adr-complete-prerequisite.md committed (graceful-degradation vs. hard-prerequisite)
[ ] adr-constitution-tracks.md committed (express/expressplan permitted_tracks decision)
[ ] Python 3.12 decision documented in parity audit report
[ ] SAFE_ID_PATTERN backward-compatibility validated and documented
```

### SC-6 — Parity Gate Specification Published

```
[ ] parity-gate-spec.md committed with structural and behavioral checklists
[ ] Gate spec usable by TechPlan and future feature migration work
```

---

## 8. Out of Scope

- Implementing `complete-ops.py` — belongs to `lens-dev-new-codebase-complete` dev phase
- Implementing `init-feature-ops.py create` — belongs to `lens-dev-new-codebase-new-feature` dev phase
- Implementing `fetch-context` or `read-context` — same; only documenting the regression and blocker state is in scope here
- All 12 preplan-phase features (adversarial-review, publish-to-governance, feature-yaml, next, preflight, etc.)
- Full behavioral test automation — manual parity audit report is sufficient
- Operational monitoring for governance publish failures (deferred to individual skills' own dev phases)
- `lc-agent-core-repo` skill investigation — deferred unless investigation proves it is a retained Lens command
- `bmad-lens-lessons` source file investigation — deferred (`.pyc`-only; separate scope)

---

## 9. Dependencies and Risks

### Dependencies

| Dependency | Type | Notes |
|-----------|------|-------|
| Old-codebase `init-feature-ops.py` (1832 lines) | Context only | Already read in full; available for parity comparison |
| Old-codebase discovery docs (`source-tree-analysis.md`, `deep-dive-lens-work-module.md`) | Context only | Establish canonical old prompt surface and Auto-Context Pull contract |
| Old-codebase `switch-ops.py` (732 lines) | Context only | Already read in full |
| Old-codebase `complete-ops.py` (450 lines) | Context only | Already read in full; source of truth for complete contract |
| `lens-dev-new-codebase-complete` feature.yaml | Governance write | ADR blocker annotation must be written before complete dev activates |
| `lens-dev-new-codebase-new-feature` feature.yaml | Governance write | Init-feature context-loading blocker must be written until `fetch-context` / `read-context` are restored or deferred |
| `_bmad/lens-work/lifecycle.yaml` | Read only | Needed to validate `express`/`expressplan` phase existence for constitution ADR |

### Risks

| Risk | Likelihood | Impact | Mitigation |
|------|-----------|--------|-----------|
| Existing feature IDs use dots/underscores — break on new `SAFE_ID_PATTERN` | Low | High | Scan governance `feature-index.yaml` before any switch rollout |
| `context_paths` shape change breaks downstream callers relying on `{summaries, full_docs}` | Medium | Medium | `context_to_load` backward-compat adapter is present in new switch-ops.py; callers must be updated to use it |
| Missing `fetch-context` / `read-context` blocks old Auto-Context Pull behavior in planning sessions | Medium | High | Treat as functional regression in parity audit report and record blocker on `lens-dev-new-codebase-new-feature` |
| `complete` dev activates before ADR is published | Low | High | Add explicit `blocks` annotation to `lens-dev-new-codebase-complete` feature.yaml |
| Python 3.12 requirement silently breaks users on 3.10/3.11 | Medium | Medium | Document explicitly; revert if no 3.12-specific features are actually used |
| Constitution template divergence creates governance state where old and new domains have different tracks | Medium | Low | ADR pins canonical template; migration guide for existing domains if needed |

---

## 10. Open Questions

| # | Question | Owner | Target |
|---|---------|-------|--------|
| OQ-1 | Do any existing feature IDs in governance contain dots or underscores that would break under the new `SAFE_ID_PATTERN`? | True Up dev | Before SC-3 gate review |
| OQ-2 | Is the Python 3.12 bump intentional (new language feature used) or accidental drift? | True Up dev | FR-8 decision note |
| OQ-3 | Should `lc-agent-core-repo` be investigated as part of this feature or deferred? | Feature owner | Businessplan review |
| OQ-4 | Should `bmad-lens-lessons` `.pyc`-only state be fixed (gitignore issue) or explicitly deferred? | Feature owner | Businessplan review |
| OQ-5 | Does `lifecycle.yaml` include `express` and `expressplan` as active phases? | True Up dev | FR-7 ADR input |

---

## 11. Glossary

| Term | Definition |
|------|-----------|
| **Parity audit** | Read-only verification that a new-codebase implementation produces the same output schema as the old-codebase baseline |
| **Structural gap** | A missing file (SKILL.md, script, test, prompt) that was present in old-codebase |
| **Behavioral gap** | An output field mismatch or schema divergence between new and old implementations |
| **Governance label** | The `phase` field in `feature.yaml`; tracks planning status, not implementation status |
| **Prompt stub** | A `.prompt.md` file in `.github/prompts/` that enables IDE agent discovery of a command |
| **ADR** | Architecture Decision Record — a permanent document capturing a significant design decision with context and rationale |
| **Graceful degradation** | A prerequisite is recommended but not required; the skill proceeds with a warning when the prerequisite is absent |
| **Hard prerequisite** | A prerequisite is enforced; the skill blocks until the prerequisite is satisfied |
