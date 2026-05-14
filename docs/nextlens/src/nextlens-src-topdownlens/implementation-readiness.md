---
feature: nextlens-src-topdownlens
doc_type: implementation-readiness
status: ready
track: express
phase: finalizeplan
depends_on: [business-plan, tech-plan, sprint-plan, epics, stories]
blocks: []
updated_at: 2026-05-14T04:00:00Z
---

# Implementation Readiness - TopDownLens Module

## Verdict

**READY for dev** on the constrained spine (TL-1, TL-4, TL-8, TL-9, TL-12) plus supporting stories (TL-2, TL-3, TL-6, TL-10, TL-11). TL-5 and TL-7 are explicitly deferrable.

## Readiness Checklist

| Check | Status | Evidence |
|-------|--------|----------|
| Business goals documented | pass | `business-plan.md` |
| Technical architecture documented | pass | `tech-plan.md` (includes Self-Hosting And Dogfooding Architecture) |
| Sprint plan with key decisions | pass | `sprint-plan.md` (key_decisions populated) |
| ExpressPlan adversarial review | pass-with-warnings, responses recorded | `expressplan-adversarial-review.md` |
| FinalizePlan review | pass-with-warnings, fixes applied | `finalizeplan-review.md` |
| Epics defined | pass | `epics.md` (3 epics) |
| Stories enumerated | pass | `stories.md` + `stories/TL-*.md` (12 files) |
| Target repo identified | pending | `feature.yaml` not yet committed; directive recorded: target_repos = [Lens.Core.Control] |
| Constitution resolved | pass (informational gates) | org -> nextlens -> nextlens/src |
| Dependencies graph acyclic | pass | see Story Dependency Graph below |
| First dev increment scoped | pass | spine + supporting, TL-5/TL-7 deferred |

## Story Dependency Graph

```
TL-1 (root)
  |-- TL-2
  |     `-- (uses TL-4 schema)
  |-- TL-3
  |-- TL-4
  |     `-- TL-7 (deferrable)
  |-- TL-5 (deferrable)
  |-- TL-6
  |     |-- TL-7 (deferrable)
  |     `-- TL-10
  `-- TL-8
        |-- TL-9
        |-- TL-10
        `-- TL-11

TL-12 depends on: TL-1, TL-2, TL-4, TL-6, TL-8, TL-9 (closure gate)
```

No cycles. TL-12 is the explicit closure gate and depends on spine stories plus TL-2 (walkthrough format) and TL-6 (Salmon signal schema).

## Constraints Carried Into Dev

- **Target repo:** Lens.Core.Control only for the first dev increment. `nextlens-control` remains a forward-looking concept.
- **TL-12 acceptance relaxed:** skip `nextlens-release` verification (repo does not yet exist).
- **No direct governance or release patches:** all publication via `publish-to-governance` / `promote-to-release`.
- **Output root:** durable module outputs under `docs/nextlens/src/nextlens-src-topdownlens/` (control repo). Governance mirror updated only via `publish-to-governance`.
- **Constitution gates:** informational (not hard) at org/domain/service levels. Authoring proceeds without hard-gate halts.

## Risks Carried Forward

- First build may need to choose between `docs/` and `_bmad-output/lens/` for durable outputs. Default: stay under `docs/`.
- Command names not yet fixed. Treat any CLI surface as provisional until TL-12 surfaces friction.
- Salmon severity rules need validation against real signals before automation depends on them.
- Bottom-up promotion thresholds need worked examples in TL-3.

## Definition Of Done For First Increment

- All spine stories merged through governed publication path.
- All supporting stories either merged or explicitly deferred with a recorded reason.
- TL-12 produces planning artifacts for the next TopDownLens feature using TopDownLens commands.
- At least one Salmon signal is generated and routed end-to-end.
- Doctor checks pass on the resulting artifacts (TL-7 if shipped; otherwise manual validation noted in TL-12 evidence).

## Hand-Off

Dev cycle entry point: `nextlens-src-topdownlens-dev` branch in `Lens.Core.Control`. Sprint status tracking: `sprint-status.yaml`.
