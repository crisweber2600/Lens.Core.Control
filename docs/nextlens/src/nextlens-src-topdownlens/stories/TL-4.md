---
feature: nextlens-src-topdownlens
story_id: TL-4
doc_type: story
status: in-progress
title: BMAD Bridge Packet
depends_on: [TL-1]
implementation_kind: schema
epic: 1
spine: true
updated_at: 2026-05-14T04:00:00Z
---

# TL-4 - BMAD Bridge Packet

## Goal

Define how LENS hands one selected feature to BMAD as a packet. BMAD remains responsible for PRD, architecture, epics, stories, implementation, and review.

## Scope

- Packet schema.
- Source context list (raw notes, walkthrough refs, capability candidates).
- Include/exclude guardrails (what BMAD must not pull in).
- Required BMAD artifacts (the packet declares what BMAD must produce).
- Traceability fields back to outcome, journey, capability, and evidence.

## Acceptance

- Packet targets exactly one feature (one selected vertical).
- Packet includes explicit scope boundaries and acceptance evidence.
- Packet includes outcome/journey traceability when available.
- BMAD remains responsible for PRD, architecture, epics, stories, implementation, and review; the packet does not pre-author them.
- Example packet committed under `examples/`.

## Files To Produce

- `docs/nextlens/src/nextlens-src-topdownlens/schemas/bmad-packet.schema.json`.
- `docs/nextlens/src/nextlens-src-topdownlens/examples/bmad-packet-example.json`.
- `docs/nextlens/src/nextlens-src-topdownlens/guides/bmad-bridge.md` (handshake notes).

## Notes For Dev

- Spine story. Cannot start until TL-1 is merged.
- TL-7 doctor checks (deferrable) will validate packet traceability fields.

## Dev Agent Record
