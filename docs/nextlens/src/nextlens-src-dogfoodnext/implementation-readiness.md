---
feature: nextlens-src-dogfoodnext
doc_type: implementation-readiness
status: ready
track: express
phase: finalizeplan
depends_on: [business-plan, tech-plan, sprint-plan, epics, stories]
blocks: []
updated_at: 2026-05-15T20:00:00Z
inputDocuments:
  - business-plan.md
  - tech-plan.md
  - sprint-plan.md
  - expressplan-adversarial-review.md
  - finalizeplan-review.md
---

# Implementation Readiness - NextLens Bugfix Skill

## Verdict

**READY for dev** after bundle generation. The plan is scoped, story-backed, and carries the review findings into concrete acceptance criteria. Dev must preserve the boundary that the skill source is authored in `lens.core.src`, while runtime NextLens fixes are constrained to `TargetProjects/nextlens/src/NextLens`.

## Readiness Checklist

| Check | Status | Evidence |
|-------|--------|----------|
| Business goal documented | pass | `business-plan.md` |
| Technical plan documented | pass | `tech-plan.md` |
| Sprint slices defined | pass | `sprint-plan.md` |
| ExpressPlan review reconciled | pass-with-warnings | `expressplan-adversarial-review.md`, `finalizeplan-review.md` |
| FinalizePlan review produced | pass-with-warnings | `finalizeplan-review.md` |
| Epics defined | pass | `epics.md` |
| Story index and story files defined | pass | `stories.md`, `stories/NLB-*.md` |
| Constitution resolved | pass | `nextlens/src`, informational gate mode, stories and review enforced |
| Bug namespace specified | pass | `bugs/nextlens/{New|QuickDev|Inprogress|Fixed}/{slug}.md` |
| Source and runtime target boundaries specified | pass | `lens.core.src`, `TargetProjects/nextlens/src/NextLens` |
| Target repo metadata | pass | `feature.yaml` target_repos includes `lens.core.src` and `NextLens` local paths. |

## Dependency Graph

```text
NLB-1
  `-- NLB-2
        `-- NLB-3

NLB-1
  `-- NLB-4

NLB-2 + NLB-3 + NLB-4
  `-- NLB-5
        `-- NLB-6
              `-- NLB-7

NLB-1..NLB-7
  `-- NLB-8
```

No cycles were identified.

## Constraints Carried Into Dev

- Canonical skill naming must be decided once and validated across skill folder, prompt alias, help output, manifest metadata, and setup or release-sync checks.
- Bug artifact creation, duplicate detection, PR recording, and closeout must work under `bugs/nextlens/...` without changing existing `bugs/QuickDev` Lens core behavior.
- Branch identity must derive from the stable bug slug, for example `feature/nextlens-bugfix-{bug_slug}`.
- The conductor owns push, PR creation or reuse, PR recording, validation evidence, and closeout.
- NextLens Doctor owns health validation. The bugfix flow records Doctor evidence instead of reimplementing Doctor checks.
- Durable artifacts use transcript summaries and evidence references by default. Raw transcript persistence requires an approved evidence artifact.

## Risks Carried Forward

- Namespace support changes a one-level bug folder assumption and must be regression-tested carefully.
- Target resolution may vary by invocation context; tests must run from a non-root working directory.
- Secret-like transcript input can only be detected conservatively; the safe default is minimization plus evidence references.

## Hand-Off

Start with NLB-1 and NLB-2. Do not implement delegation or closeout until the canonical skill surface, intake schema, namespace behavior, and resolver contract have failing tests and accepted behavior.