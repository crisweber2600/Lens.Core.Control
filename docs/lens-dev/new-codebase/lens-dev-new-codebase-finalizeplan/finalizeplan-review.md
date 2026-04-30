---
feature: lens-dev-new-codebase-finalizeplan
doc_type: finalizeplan-review
phase: finalizeplan
source: manual-rerun
verdict: pass-with-warnings
status: review-complete
critical_count: 0
high_count: 1
medium_count: 3
low_count: 1
governance_impact: low
carry_forward_blockers: []
updated_at: '2026-04-30T00:00:00Z'
review_format: abc-choice-v1
---

# FinalizePlan Review — lens-dev-new-codebase-finalizeplan

**Source:** manual-rerun  
**Artifacts reviewed:** business-plan.md, tech-plan.md, sprint-plan.md, expressplan-adversarial-review.md  
**Format:** abc-choice-v1. Response options A–E are presented per finding.

---

## Verdict: `pass-with-warnings`

The combined planning set is coherent, internally consistent, and ready for the downstream
planning bundle (epics, stories, readiness, sprint). No critical blockers found. One high
finding from the expressplan review (H2 — FinalizePlan predecessor gate inconsistency between
SKILL.md and tech plan) carries forward as a Sprint 1 acceptance item that must be resolved
before Sprint 1 is marked done, not before planning PR merge.

The governance cross-check identified a low-impact relationship with
`lens-dev-new-codebase-expressplan` (recently completed) — the expressplan feature delivered
the ExpressPlan command while this feature's planning docs retroactively document that same
work. The planning packet for this feature is therefore a documentation-after-implementation
cycle, which is expected for an express track feature where implementation preceded formal
planning. The governance docs path for the expressplan feature's SKILL.md and this feature's
SKILL.md both live in the same target repo (`lens.core.src`) and must not create duplicate
registrations in `module.yaml`. Sprint 1 story must confirm no duplicate entries.

Three medium findings address: the tech plan's sprint-plan split (Sprint 1 reduced from 8 to
5 stories per party-mode feedback but the sprint-plan.md still shows the original 8-story
count), the FinalizePlan step-1 publish sequence needing clarification for express-track
features, and the absence of a `docs.path` field in `feature.yaml` requiring fallback path
resolution. One low finding notes that `feature.yaml.target_repos` is empty — it should
reference `lens.core.src` to make cross-feature tooling aware of the target.

---

## Response Record

| Option | Meaning |
| --- | --- |
| A / B / C | Accept the proposed resolution with its stated trade-offs |
| D | Provide a custom resolution after `D:` |
| E | Explicitly accept the finding with no action |

---

## Finding Summary

| ID | Severity | Title | Response |
| --- | --- | --- | --- |
| H1 | High | Carry-forward from ExpressPlan H2 — predecessor gate inconsistency must be resolved in Sprint 1 | **A** |
| M1 | Medium | sprint-plan.md still shows 8-story Sprint 1 despite party-mode consolidation | **A** |
| M2 | Medium | FinalizePlan step-1 publish path needs express-track clarification | **B** |
| M3 | Medium | `feature.yaml.docs.path` absent — relies on fallback resolution | **E** |
| L1 | Low | `feature.yaml.target_repos` empty | **D: Add lens.core.src to target_repos in this session** |

---

## Governance Cross-Check

### Related Features Reviewed

| Feature | Relationship | Impact |
|---------|-------------|--------|
| `lens-dev-new-codebase-expressplan` | Implementation overlap — both features delivered ExpressPlan/FinalizePlan skills | Low: both features reference the same SKILL.md files; no conflict as long as module.yaml has no duplicate entries |
| `lens-dev-new-codebase-baseline` | Source split — this feature uses baseline product-brief, research, brainstorm, PRD as planning inputs | None: read-only dependency |
| `lens-dev-new-codebase-techplan` | Sibling feature — delivered the TechPlan command; FinalizePlan's activation validates a compatible predecessor | None: clean separation |

### Impacted Services

No other services in the `lens-dev` domain are impacted. The new conductor skills are
internal to `lens.core.src` and do not change any published contracts visible to external
consumers.

### Governance Docs That Reference This Feature

- `lens-dev/new-codebase/service.yaml` — lists features; no change needed
- `feature-index.yaml` — will be updated when finalizeplan-complete is set

---

## Findings Detail

### H1 — Carry-forward: predecessor gate inconsistency (from ExpressPlan H2)

**Context:** The FinalizePlan SKILL.md activation validation checks `techplan` as the
predecessor phase. For express-track features the predecessor is `expressplan-complete`.
If the SKILL.md does not accept `expressplan-complete`, this feature's own FinalizePlan
activation would fail when the feature is in `expressplan-complete` phase.

**Resolution A (chosen):** Sprint 1 validation story S1.1 (conductor infrastructure
checklist) must include: "Confirm `bmad-lens-finalizeplan/SKILL.md` On Activation step 5
accepts both `techplan` and `expressplan-complete` as valid predecessors. If only
`techplan` is listed, add `expressplan-complete` as a remediation sub-story."

---

### M1 — sprint-plan.md shows 8-story Sprint 1

**Context:** The party-mode challenge round recommended consolidating Sprint 1's 7
read-and-validate stories into a single checklist story. The response was "accepted," but
the sprint-plan.md file still shows the original 8-story count. This creates a discrepancy
between the planning response log and the sprint-plan artifact.

**Resolution A (chosen):** The sprint-plan.md will be updated in the control repo commit
(this session). Sprint 1 will show 5 stories: one consolidated validation story, S1.7
(tests), S1.8 (commit), and two additions from party-mode (constitution permission check,
tech-plan.md naming check).

---

### M2 — FinalizePlan step-1 publish for express track

**Context:** Step 1 of FinalizePlan says "publish staged TechPlan artifacts via
`publish-to-governance --phase techplan`." For an express-track feature, the TechPlan
milestone docs are `tech-plan.md` and `business-plan.md`, not `techplan.md` or
`architecture.md`. The publish CLI must handle this naming difference or the publish
will silently produce an empty governance mirror.

**Resolution B (chosen):** Accept that the publish CLI maps artifact slugs to filenames.
Sprint 1 checklist must include confirming whether the publish CLI handles
hyphenated express-track artifact names. If it doesn't, the publish step for this feature
will be a no-op governance mirror update, which is acceptable given no external consumers
depend on it yet. Flag as a tracked item for the CLI team.

---

### M3 — `feature.yaml.docs.path` absent

**Context:** The feature's `feature.yaml` has no `docs.path` field. All resolution falls
back to the computed `docs/lens-dev/new-codebase/lens-dev-new-codebase-finalizeplan/`
path. This is correct behavior and the path exists. No functional issue.

**Resolution E (chosen):** Accepted without action. The fallback path is correct and
the directory exists.

---

### L1 — `feature.yaml.target_repos` empty

**Context:** The `target_repos` array is empty. The feature's implementation target is
`lens.core.src` in the `lens-dev/new-codebase` workspace. Populating this makes the
feature searchable by cross-feature tooling.

**Resolution D (chosen):** Update `feature.yaml.target_repos` to reference `lens.core.src`
in this session as part of the finalizeplan commit.

---

## Party-Mode Blind-Spot Challenge

> **John (PM):** This feature's planning packet was created after the implementation. The
> epics and stories we're about to generate will describe work that is already partially done.
> When an implementation agent reads "Sprint 1 — Foundation Validation," are they doing
> validation work or re-implementation work? The distinction matters for story point estimates
> and acceptance criteria. The planning docs need to be explicit about which stories are
> "confirm already done" vs "new work to do."

> **Winston (Architect):** The tech plan shows three separate SKILL.md files with no shared
> utilities. The product brief (from baseline) mentions extracting shared utilities for
> review-ready gating, batch contract, and publish-entry hook as G3 of the rewrite. But
> none of these shared utilities appear in the tech plan for this feature. Either this feature
> is delivering conductor stubs that intentionally defer shared utilities, or the tech plan is
> missing a dependency. Which is it?

> **Bob (QA):** The adversarial review from ExpressPlan generated party-mode feedback that
> recommended consolidating Sprint 1 stories. But the sprint-plan.md wasn't updated. The
> FinalizePlan review then flagged this as M1. The finding is marked "Resolution A: update
> in this session." But we're now IN FinalizePlan, generating epics and stories from the
> sprint plan. If the sprint plan is stale, the epics will inherit the stale structure. The
> sprint plan update must happen before we generate epics, not after.

> **Freya (UX/Product):** The business plan says the user-observable success signal is
> "Activating `/lens-finalizeplan` on a feature in `expressplan-complete` proceeds directly
> to Step 1 adversarial review without re-prompting." But who tests this end-to-end? The
> test files test structural properties of SKILL.md. There's no integration test that
> activates the command in a real Lens session. That's probably fine for this phase, but
> it means the acceptance criteria are documentation-level, not behavior-level.

**Agent responses to party-mode challenge:**

**On John's sequencing observation:** Epics and stories will explicitly label each story's
nature: `[confirm]` for stories that verify already-completed work, `[new]` for stories
requiring net-new implementation. Sprint 1 stories are predominantly `[confirm]` type with
one `[new]` (constitution permission check). This distinction will appear in the acceptance
criteria of each story.

**On Winston's shared utilities gap:** This feature intentionally delivers conductor stubs
without shared utilities. The baseline G3 (shared utility extraction) is a separate
workstream tracked under `lens-dev-new-codebase-baseline`'s dev phase. The tech plan
correctly omits it because the scope boundary is the three conductor SKILL.md files, not
the shared utility layer. Adding a note to the business plan out-of-scope section: "Shared
utility extraction (G3 from baseline) is a separate feature outside this feature's scope."

**On Bob's sprint plan order:** Confirmed. The sprint-plan.md will be updated to the
5-story Sprint 1 structure BEFORE epics are generated. The updated sprint-plan.md with
consolidated S1.1 and two new party-mode stories is the authoritative input for the
epics document.

**On Freya's integration test gap:** Documented as a known gap in the implementation
readiness report. The acceptance criteria are documentation-level (structural validation
of SKILL.md content). A behavior-level integration test is out of scope for this planning
cycle and is a tracked item for a future hardening sprint.
