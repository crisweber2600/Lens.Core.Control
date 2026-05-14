---
feature: nextlens-src-topdownlens
story_id: TL-3
doc_type: story
status: in-progress
title: Bottom-Up Compatibility Rules
depends_on: [TL-1]
implementation_kind: docs-only
epic: 3
spine: false
updated_at: 2026-05-14T04:00:00Z
---

# TL-3 - Bottom-Up Compatibility Rules

## Goal

Ensure TopDownLens does not break the existing feature-first mental model. Encode rules that let a feature exist standalone forever and only get promoted when evidence supports it.

## Scope

- Standalone feature creation rules.
- Adjacency detection concept (weak by default).
- Repeated pressure categories.
- Promotion threshold guidance.
- Bottom-up promotion path: feature -> capability -> product area -> domain/system, gated by evidence.

## Acceptance

- A feature can remain independent forever.
- Adjacency is weak by default; no auto-grouping.
- Promotion is advisory; requires recorded evidence.
- The rule "no growth without pressure" is documented and referenced from `business-plan.md`.
- At least one worked example shows a feature staying standalone, and one shows a promotion path with evidence.

## Files To Produce

- `docs/nextlens/src/nextlens-src-topdownlens/guides/bottom-up-compatibility.md`.
- `docs/nextlens/src/nextlens-src-topdownlens/examples/promotion-example.md`.
- `docs/nextlens/src/nextlens-src-topdownlens/examples/standalone-example.md`.

## Notes For Dev

- Docs-only. No code or schemas authored here.
- Cross-link to TL-2 walkthrough so the two flows coexist.

## Dev Agent Record
