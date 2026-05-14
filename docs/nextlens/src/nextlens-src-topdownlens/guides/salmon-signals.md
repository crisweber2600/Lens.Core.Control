---
feature: nextlens-src-topdownlens
doc_type: guide
story_id: TL-6
title: Salmon Signal Lifecycle And Routing
updated_at: 2026-05-14T05:00:00Z
---

# Salmon Signal Lifecycle And Routing

A Salmon signal records downstream evidence that may need to update upstream truth. Signals can target feature, journey, outcome, product area, or the broader landscape.

## Lifecycle

```text
open -> accepted -> resolved
open -> rejected
open -> accepted -> superseded
```

Signals start as `open`. A human or conductor either accepts the finding, rejects it with rationale, resolves it by recording evidence, or supersedes it when a later signal replaces it.

## Severity Routing

| Severity | Default Action | Routing |
| --- | --- | --- |
| low | `local_note` | Record on the current feature or story. |
| medium | `landscape_update` | Add to the next planning/update pass. |
| high | `bmad_correct_course` | Route to correct-course or immediate triage before more dependent work lands. |
| blocking | `block_promotion` | Stop promotion/release until resolved or explicitly superseded. |

`split_feature` may be used for medium, high, or blocking findings when the evidence shows scope is crossing durable boundaries.

## Evidence And Provenance

Each signal preserves:

- `source.type` and `source.id` for where the finding came from.
- `upstream_targets` for the level that may need correction.
- `finding` for the discovered issue.
- `evidence_refs` for durable evidence records.
- `recommended_action` for the next governance or planning move.

High and blocking severities are consumed by `guides/bugfix-flow.md` once TL-10 is complete.