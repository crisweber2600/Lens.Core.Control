---
feature: lens-dev-new-codebase-trueup
doc_type: product-brief
status: draft
goal: "Define the True Up initiative as a product: what it is, who needs it, what success looks like, and how it fits the rewrite roadmap"
key_decisions:
  - True Up is a cross-cutting gap-closure initiative, not an implementation sprint for a single command
  - Its primary output is shared infrastructure that unblocks 10 of 12 remaining retained commands
  - Success is defined by measurable parity gates, not subjective completeness
  - True Up does not own individual command features — it owns the shared floor and the audit that verifies parity
open_questions:
  - Should True Up be tracked as a single feature or decomposed into sub-features per tier?
  - What is the target timeline or release milestone for Tier 1 infrastructure?
  - Is there an existing test harness that can be extended for parity validation, or does one need to be built?
depends_on: [brainstorm, research]
blocks: []
updated_at: 2026-04-28T01:00:00Z
---

# Product Brief — True Up (lens-dev-new-codebase-trueup)

---

## 1. Vision

The **True Up** initiative closes the gap between the old-codebase Lens Workbench (41 skills, 17 retained commands, 57 prompts) and the new-codebase rewrite (currently: 5 skill directories, 3 commands with prompt stubs, 0 planning-phase skills implemented).

True Up does not implement the 12 unimplemented retained commands. Instead, it:

1. **Builds the shared infrastructure** that all 10 planning-phase and session-management commands depend on
2. **Audits the 5 already-delivered commands** for parity gaps not caught by governance phase tracking
3. **Creates the parity checklist** — a runnable, automated verification suite that validates the new codebase against the old
4. **Documents the dependency order** so each of the 12 remaining features can be activated without blocking surprises

Without True Up, the next 10 features to be dev'd will each independently discover they need `bmad-lens-adversarial-review`, `publish-to-governance`, and `bmad-lens-feature-yaml` updates — and will be blocked. True Up runs first to prevent that compounding delay.

---

## 2. Problem Statement

### 2.1 The Blocking Gap

The three critical infrastructure components missing from the new codebase are:

| Component | Blocked Commands |
|-----------|----------------|
| `bmad-lens-adversarial-review` | preplan, businessplan, techplan, finalizeplan, expressplan, dev (all 6 planning phases) |
| `git-orchestration-ops.py publish-to-governance` | businessplan, techplan, finalizeplan, dev (4 commands) |
| `bmad-lens-feature-yaml` update operations | Every phase-transition workflow (all 12 unimplemented commands) |

If any one of these three components is missing when the next command feature begins dev, that feature will be blocked at the implementation gate.

### 2.2 The Audit Gap

The 5 delivered commands (new-domain, new-service, switch, new-feature, complete) were tracked by governance phase. But governance phase labels track **planning status**, not **implementation completeness**. Research has confirmed:

- `new-feature` is labeled `finalizeplan-complete` (planning done, dev not started) but its core script (`init-feature-ops.py create`) appears to already work in production
- `complete` is labeled `finalizeplan-complete` but has no SKILL.md, no tests, and a reference-only doc — it is not dev-ready
- `switch` prompt stub is not mirrored to `.github/prompts/` — IDE integrations cannot discover it
- `preflight` has its script but no SKILL.md or tests — it is not a fully delivered skill package

### 2.3 The Prompt Gap

14 of 17 retained commands have no published `.prompt.md` stub in the new codebase. Without prompt stubs, users cannot invoke these commands from IDE agents. Even when scripts are implemented, the commands are invisible until their stubs are published.

---

## 3. Target Audience

**Primary:** The agent team implementing new-codebase Lens features.  
They are blocked right now — not by lack of plans for individual commands, but by missing shared infrastructure that every command needs.

**Secondary:** Any Lens user who wants to run a planning phase in the new codebase.  
Currently, all planning phases fall back to the RELEASE module (old codebase). After True Up, the new codebase can host its first planning phase feature.

**Tertiary:** Governance and compliance reviewers.  
The parity audit and verification checklist created by True Up will be the evidence base for declaring each retained command fully migrated.

---

## 4. Scope

### 4.1 In Scope

**Tier 1 — Shared infrastructure (required for any planning phase to complete):**
- [ ] `bmad-lens-adversarial-review` — new-codebase skill package with SKILL.md and functional script
- [ ] `git-orchestration-ops.py publish-to-governance` — subcommand for publishing phase artifacts to governance
- [ ] `bmad-lens-feature-yaml` update operations — `phase-transition`, `update-field`, and `add-entry` subcommands for all phase skills

**Tier 2 — Audit and parity verification:**
- [ ] Parity audit of 5 delivered commands (new-domain, new-service, switch, new-feature, complete)
  - Verify governance phase labels reflect actual implementation state
  - Identify observable behavioral gaps vs. old-codebase baseline
  - Document findings as actionable issues for each affected feature
- [ ] Parity checklist — a documented gate specification for declaring a retained command fully migrated

**Tier 3 — Prompt publishing and SKILL.md gap closure for delivered commands:**
- [ ] `lens-switch.prompt.md` mirrored to `.github/prompts/`
- [ ] `lens-new-feature.prompt.md` published
- [ ] `bmad-lens-preflight` — SKILL.md and tests added
- [ ] `bmad-lens-complete` — SKILL.md and tests scaffolded (even if implementation is not started, the SKILL.md should capture the contract)

**Tier 4 — `complete` prerequisite design decision:**
- [ ] Document graceful-degradation vs. hard-prerequisite decision for `complete` command
- [ ] Define the acceptance criteria for `bmad-lens-retrospective` and `bmad-lens-document-project` as blockers for `complete` dev

### 4.2 Out of Scope

- Implementing any of the 12 unimplemented retained commands (they own their own features)
- Rewriting or replacing the RELEASE module (`lens.core/`) — True Up targets the new-codebase source
- Full `complete-ops.py` implementation (that belongs to the complete feature's dev phase)
- Any source-code parity analysis (all analysis is governance-docs and filesystem-structure only)

---

## 5. Success Criteria

True Up is complete when ALL of the following are verifiable:

### SC-1: Shared Infrastructure Live

```
[ ] bmad-lens-adversarial-review: SKILL.md + script + tests committed to new-codebase source
[ ] git-orchestration-ops.py: 'publish-to-governance' subcommand present and tested
[ ] bmad-lens-feature-yaml: update operations (phase-transition, update-field) present and tested
```

### SC-2: Parity Audit Complete

```
[ ] Parity audit report published for all 5 delivered commands
[ ] Governance phase labels for new-feature updated to reflect actual delivery state
[ ] All parity gaps (behavioral, prompt, SKILL.md) logged as actionable issues in feature governance docs
```

### SC-3: Parity Checklist Published

```
[ ] Parity checklist artifact committed to docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/
[ ] Checklist covers: SKILL.md present, script present, tests present, prompt published, .github mirror present, governance phase label correct
[ ] Checklist is runnable (validate-phase-artifacts.py can be extended to check against it or it can be verified manually)
```

### SC-4: Prompt Gaps Closed for Delivered Commands

```
[ ] lens-switch.prompt.md present in .github/prompts/
[ ] lens-new-feature.prompt.md published to both locations
[ ] lens-complete.prompt.md scaffold published (even if SKILL.md-only, no implementation)
```

### SC-5: Next Command Can Be Activated

```
[ ] At least one of the 12 unimplemented commands (ideally 'next' or 'preflight') can start dev phase
    without hitting an infrastructure blocker
[ ] The Tier 1 infrastructure passes integration smoke tests against a test feature
```

---

## 6. Risks

| Risk | Likelihood | Impact | Mitigation |
|------|-----------|--------|-----------|
| `publish-to-governance` is more complex than expected (requires governance-main commit + push semantics) | Medium | HIGH | Research RELEASE module's current implementation before writing new version |
| `bmad-lens-adversarial-review` requires significant BMAD skill-wrapping complexity | Medium | HIGH | Scope to the minimal review-gate contract; defer full adversarial features |
| `bmad-lens-feature-yaml` update operations touch governance-main — risky if not isolated | Low | HIGH | All feature-yaml updates must be wrapped in pull-then-commit-then-push sequence |
| Governance phase audit reveals `new-feature` phase label is actually correct and `create` has untested gaps | Medium | MEDIUM | Treat any fetch-context gap as a separate issue ticket, not a blocker for True Up |
| `complete`'s prerequisite design decision (graceful degradation vs. hard prerequisite) becomes a long debate | Medium | MEDIUM | Time-box the decision; default to graceful degradation with a clear warning log |

---

## 7. Dependencies

### 7.1 What True Up Needs

- Access to RELEASE module (`lens.core/`) to inspect `git-orchestration-ops.py` and validate `publish-to-governance` contract
- Governance access to the 5 delivered features' tech plans (already loaded in this session)
- Old-codebase reference implementations for adversarial-review, feature-yaml, and git-orchestration

### 7.2 What Depends on True Up

Every one of the 12 unimplemented retained-command features depends on Tier 1 outputs:

| Tier 1 Output | Features Unblocked |
|---------------|-------------------|
| `bmad-lens-adversarial-review` | preplan, businessplan, techplan, finalizeplan, expressplan, dev |
| `publish-to-governance` | businessplan, techplan, finalizeplan, dev |
| `bmad-lens-feature-yaml` updates | all 12 |

True Up must complete Tier 1 before ANY of those 12 features can advance beyond planning.

---

## 8. Relationship to the Rewrite Roadmap

The new-codebase rewrite is tracking individual retained commands as separate governance features. True Up is intentionally **orthogonal** to that structure — it is the shared foundation layer that makes the per-command feature sprint cadence possible.

The appropriate mental model:

```
OLD CODEBASE (reference)
         │
         ▼
True Up (Audit + Shared Infrastructure)
   ├── SC-1: Tier 1 infrastructure ─────────────────────────┐
   ├── SC-2: Parity audit                                   │
   ├── SC-3: Parity checklist                               │
   ├── SC-4: Prompt gaps (delivered commands)               │
   └── SC-5: First downstream command unblocked             │
                                                            ▼
NEW CODEBASE COMMAND FEATURES (in dependency order)
   preflight → next → constitution → preplan → businessplan → techplan → finalizeplan → expressplan → dev
   split-feature, discover, upgrade (independently deliverable after Tier 1)
```

True Up runs once, then the per-command features run in priority order. Without True Up, the per-command features will each independently re-discover the same infrastructure gaps and be blocked.
