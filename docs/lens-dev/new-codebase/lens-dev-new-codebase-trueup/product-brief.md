---
feature: lens-dev-new-codebase-trueup
doc_type: product-brief
status: draft
goal: "Close delivery gaps in the 5 non-preplan new-codebase features: audit parity against old-codebase baseline, fix missing prompts and SKILL.md packages, and resolve the complete prerequisite design decision"
key_decisions:
  - Scope is limited to features NOT in preplan: new-domain, new-service, switch (complete); new-feature, complete (finalizeplan-complete)
  - Shared infrastructure (adversarial-review, publish-to-governance, feature-yaml updates) is explicitly out of scope — those belong to individual preplan feature planning cycles
  - Success is defined by measurable parity gates for the 5 in-scope features only
  - True Up does not activate or plan any of the 12 preplan-phase features
open_questions:
  - Should the complete prerequisite decision (graceful degradation vs. hard prerequisite) be an ADR artifact owned by True Up or by the complete feature itself?
  - Is there an existing test harness that can be extended for behavioral parity validation?
depends_on: [brainstorm, research]
blocks: []
updated_at: 2026-04-28T01:00:00Z
---

# Product Brief — True Up (lens-dev-new-codebase-trueup)

---

## 1. Vision

The **True Up** initiative brings the 5 non-preplan new-codebase features to genuine parity with the old-codebase baseline. These are the features where governance phase tracking has outpaced delivery verification — commands that are marked complete or dev-ready but have observable gaps in their prompt publishing, SKILL.md packages, test coverage, and behavioral contracts.

The 5 in-scope features are:

| Feature | Governance Phase | Gap Summary |
|---------|-----------------|------------|
| `new-domain` | complete | Structural parity unverified against old-codebase schema outputs |
| `new-service` | complete | Structural parity unverified against old-codebase schema outputs |
| `switch` | complete | `lens-switch.prompt.md` not mirrored to `.github/prompts/` |
| `new-feature` | finalizeplan-complete | No prompt stub; governance phase label may lag actual implementation state; fetch-context parity open |
| `complete` | finalizeplan-complete | No SKILL.md, no script, no tests; prerequisite delegation design unresolved |

True Up does not plan or unblock the 12 preplan-phase features. Shared infrastructure (adversarial-review, publish-to-governance, feature-yaml updates) is explicitly deferred — those components will be scoped into the individual preplan feature planning cycles as each activates.

---

## 2. Problem Statement

Governance phase labels track **planning status**, not **implementation completeness**. The 5 non-preplan features have all advanced through planning, but their actual delivery state has not been verified against the old-codebase parity baseline. Research has confirmed the following gaps:

### 2.1 Behavioral Parity Unverified (new-domain, new-service)

`new-domain` and `new-service` are marked `complete` with tech plans that specify frozen schemas (domain.yaml, service.yaml, constitution.md). However, no parity validation has been run to confirm that the new-codebase outputs match the old-codebase outputs field-for-field. The schemas are declared frozen, but whether the new implementation actually produces them correctly is an open assumption.

### 2.2 Prompt Publishing Gap (switch, new-feature)

`lens-switch.prompt.md` exists in `_bmad/lens-work/prompts/` but is **not mirrored to `.github/prompts/`** — the location IDE agents use to discover prompts. `lens-new-feature.prompt.md` does not exist in either location. Both commands are invisible to IDE agent sessions until their stubs are correctly published.

### 2.3 Governance Phase Label vs. Implementation State (new-feature)

`new-feature` is labeled `finalizeplan-complete` (planning done, dev not started), but `init-feature-ops.py create` — its core script — ran successfully in this session to create the trueup feature. Either the governance label is stale (implementation outpaced governance tracking), or the `create` subcommand is partially implemented with untested gaps (specifically `fetch-context`). Neither state has been confirmed.

### 2.4 Incomplete Delivery Package (complete)

`complete` is labeled `finalizeplan-complete` but the new-codebase source has only `references/finalize-feature.md` — no SKILL.md, no `complete-ops.py`, no tests. Additionally, `complete` delegates to `bmad-lens-retrospective` and `bmad-lens-document-project`, neither of which exists in the new-codebase source. The prerequisite handling strategy (graceful degradation vs. hard prerequisite) has not been decided, which blocks dev from starting.

---

## 3. Target Audience

**Primary:** Governance and compliance reviewers.  
True Up produces the evidence base — parity audit report, corrected governance phase labels, and verified SKILL.md packages — that justifies calling the 5 non-preplan commands actually complete.

**Secondary:** The agent team implementing new-codebase Lens features.  
The `complete` prerequisite design decision and the parity checklist define the acceptance criteria they need before `complete` dev can start.

**Tertiary:** Any Lens user invoking `switch` or `new-feature` from an IDE agent session.  
They are currently invisible to IDE prompt discovery due to missing `.github/prompts/` stubs.

---

## 4. Scope

### 4.1 In Scope

**Tier 1 — Parity audit (all 5 non-preplan features):**
- [ ] Behavioral parity validation for `new-domain`: verify `init-feature-ops.py create-domain` outputs match old-codebase domain.yaml and constitution.md schemas field-for-field
- [ ] Behavioral parity validation for `new-service`: verify `init-feature-ops.py create-service` outputs match old-codebase service.yaml and service constitution schemas
- [ ] Governance phase label audit for `new-feature`: confirm whether `init-feature-ops.py create` is fully implemented or partially implemented; update governance phase label to match actual state
- [ ] `fetch-context` parity investigation for `new-feature`: determine whether the subcommand is implemented, stubbed, or absent
- [ ] Parity audit report — a single artifact documenting findings for all 5 features with actionable issues

**Tier 2 — Prompt publishing gap closure:**
- [ ] `lens-switch.prompt.md` mirrored to `.github/prompts/`
- [ ] `lens-new-feature.prompt.md` published to both `_bmad/lens-work/prompts/` and `.github/prompts/`
- [ ] `lens-complete.prompt.md` stub published (even if backing SKILL.md has no implementation yet)

**Tier 3 — SKILL.md and test gap closure for partially-delivered commands:**
- [ ] `bmad-lens-complete` — SKILL.md authored to capture the command contract; `references/finalize-feature.md` reviewed and incorporated
- [ ] `bmad-lens-complete` — test stubs scaffolded covering the happy path and prerequisite-missing degradation path

**Tier 4 — `complete` prerequisite design decision:**
- [ ] ADR: graceful-degradation vs. hard-prerequisite for `bmad-lens-retrospective` and `bmad-lens-document-project` delegation
- [ ] ADR outcome published to `complete` feature governance docs as a blocker annotation before its dev activates

**Tier 5 — Parity checklist:**
- [ ] Published gate specification: what constitutes "fully migrated" for a retained command (SKILL.md present, script present, tests present, prompt published, `.github` mirror present, governance phase label correct, behavioral parity validated)

### 4.2 Out of Scope

- All 12 preplan-phase features — they are not in scope; they activate on their own cadence after their own planning cycles
- Shared infrastructure (adversarial-review, publish-to-governance, feature-yaml updates) — deferred to individual preplan feature planning
- Full `complete-ops.py` implementation — belongs to the complete feature's dev phase
- `bmad-lens-preflight` SKILL.md and tests — preflight is in preplan and out of scope for this feature
- Any source-code behavioral analysis beyond schema/output contract verification

---

## 5. Success Criteria

True Up is complete when ALL of the following are verifiable:

### SC-1: Parity Audit Published

```
[ ] Parity audit report artifact committed to docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/
[ ] Report covers all 5 non-preplan features with per-feature findings
[ ] Governance phase label for new-feature corrected to reflect actual implementation state
[ ] All parity gaps logged as actionable issues in the affected feature governance docs
```

### SC-2: Prompt Stubs Correct

```
[ ] lens-switch.prompt.md present in .github/prompts/
[ ] lens-new-feature.prompt.md present in both _bmad/lens-work/prompts/ and .github/prompts/
[ ] lens-complete.prompt.md stub present in both locations
```

### SC-3: complete Delivery Package Scaffolded

```
[ ] bmad-lens-complete/SKILL.md authored and committed to new-codebase source
[ ] bmad-lens-complete test stubs scaffolded (happy path + prerequisite-missing path)
[ ] ADR for prerequisite handling committed and referenced in complete feature governance docs
```

### SC-4: Parity Checklist Published

```
[ ] Parity checklist artifact committed to docs/lens-dev/new-codebase/lens-dev-new-codebase-trueup/
[ ] Checklist covers: SKILL.md present, script present, tests present, prompt published,
    .github mirror present, governance phase label correct, behavioral parity validated
```

---

## 6. Risks

| Risk | Likelihood | Impact | Mitigation |
|------|-----------|--------|-----------|
| Behavioral parity validation for new-domain/new-service requires inspecting script output, which may surface unexpected schema mismatches | Medium | MEDIUM | Scope to field presence and type validation against the frozen schema spec in the tech plans; do not chase edge-case value equality |
| `fetch-context` investigation for new-feature may reveal the subcommand is absent, triggering a scope question about whether True Up should fix it | Medium | MEDIUM | True Up documents the gap as an issue; it does not implement fetch-context — that belongs to the new-feature dev phase |
| Governance phase label for new-feature may be correct (create has untested gaps), requiring no label change and a different audit finding | Low | LOW | Either outcome (label correct or stale) is a valid audit result; document the finding, not the expected answer |
| `complete` prerequisite design decision takes multiple review rounds to resolve | Medium | MEDIUM | Time-box at two rounds; default to graceful-degradation with a structured warning log if no consensus |
| `bmad-lens-complete` SKILL.md authorship requires reading `references/finalize-feature.md` plus the old-codebase SKILL.md — scope may expand | Low | LOW | Timebox SKILL.md authorship; it is a contract capture document, not a full implementation spec |

---

## 7. Dependencies

### 7.1 What True Up Needs

- Governance tech plans for all 5 non-preplan features (already loaded; available via governance fetch-context)
- Old-codebase SKILL.md for `bmad-lens-complete` (available in RELEASE module `lens.core/`) to inform SKILL.md authorship
- Filesystem access to new-codebase source for behavioral parity spot-checks (non-invasive reads only)

### 7.2 What Depends on True Up

| True Up Output | Who Needs It |
|----------------|-------------|
| Corrected governance phase label for new-feature | The new-feature dev team — they need an accurate planning state before activating dev |
| ADR for complete prerequisite handling | The complete dev phase — must not start dev until this decision is published |
| Parity checklist | All future command features — it defines the acceptance gate for declaring a command fully migrated |
| Prompt stubs for switch, new-feature, complete | Any IDE agent user invoking these commands |

True Up has no dependency on — and does not produce — any shared infrastructure (adversarial-review, publish-to-governance, feature-yaml updates). Those are preplan-feature concerns.

---

## 8. Relationship to the Rewrite Roadmap

True Up is a **retrospective verification pass** over the features that have already moved through planning. It does not sequence the remaining 12 preplan features or provide infrastructure for them.

```
NON-PREPLAN FEATURES (True Up scope)
   new-domain ──── complete? ──── parity audit ──┐
   new-service ─── complete? ──── parity audit ──┤
   switch ───────── complete? ──── prompt fix ────┤──► Parity Checklist (SC-4)
   new-feature ─── label correct? + prompt fix ──┤
   complete ─────── SKILL.md + ADR ───────────────┘

PREPLAN FEATURES (out of scope — activate on their own cadence)
   preflight, next, constitution, preplan, businessplan, techplan,
   finalizeplan, expressplan, dev, split-feature, discover, upgrade
```

After True Up, the 5 non-preplan features are verifiably in the state their governance labels claim. The 12 preplan features proceed independently without dependency on True Up outputs.
