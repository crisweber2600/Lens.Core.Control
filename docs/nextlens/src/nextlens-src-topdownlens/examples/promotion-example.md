---
feature: nextlens-src-topdownlens
doc_type: example
story_id: TL-3
title: Bottom-Up Promotion Example
updated_at: 2026-05-14T04:55:00Z
---

# Bottom-Up Promotion Example

## Starting Features

- `feature.export_single_artifact_markdown`
- `feature.batch_export_artifact_bundle`
- `feature.publish_artifact_snapshot`

Each feature began as a standalone utility.

## Repeated Pressure

Evidence accumulated across three features:

- Artifact reuse: all three read the same artifact index format.
- Repeated workflow: all three resolve feature docs, normalize metadata, and emit a portable output.
- Shared risk: review findings repeatedly mention stale metadata and missing provenance.
- Stakeholder ownership: one operator owns the artifact export workflow.

## Promotion Recommendation

Promote candidate capability `capability.artifact_export`.

Do not promote a product area yet. The evidence proves a durable capability, but not a broader product boundary.

## Recorded Evidence

- `evidence.artifact_export_reuse_1`
- `evidence.artifact_export_review_findings_1`
- `salmon.20260514.metadata_staleness`

## Result

The three features keep their original IDs. The new capability record references them as promotion evidence and remains candidate until another feature validates the same ability.