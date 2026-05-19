---
feature: nextlens-src-bottomup
doc_type: product-brief
status: complete
phase: preplan
track: full
goal: "Define the MVP product scope for bottom-up feature packet creation."
key_decisions: []
open_questions: []
depends_on:
  - docs/nextlens/src/nextlens-src-bottomup/brainstorm.md
  - docs/nextlens/src/nextlens-src-bottomup/research.md
blocks: []
title: "Bottom-Up LENS Feature Packet Creator"
inputDocuments:
  - docs/nextlens/src/nextlens-src-bottomup/brainstorm.md
  - docs/nextlens/src/nextlens-src-bottomup/research.md
updated_at: 2026-05-19T00:00:00Z
---

# Product Brief — Bottom-Up LENS Feature Packet Creator

## Executive Summary

Bottom-Up LENS is a planned future NextLens lane that lets users begin with one independently useful feature before a system, domain, capability, roadmap, or architecture is known. The first deliverable should not attempt to build the entire bottom-up ecosystem. The right first slice is a dedicated feature packet creator that captures one locally valuable feature safely, proves it did not create larger structure, and prepares a clean handoff path for later BMAD execution.

The packet creator should write one stable archived feature packet as historical work evidence, not as Living Landscape truth. It must select exactly one feature candidate, validate local sufficiency and scope boundaries, require human preview confirmation, emit a non-effects receipt, preserve stable identity/provenance for future derived-map reconstruction, and defer BMAD readiness, adjacency, pressure, Salmon, promotion, Landscape, and Graph behavior until later evidence-bearing phases.

## Problem

Current Lens planning is primarily top-down: users define or inherit a believed system shape before work flows into features. That works when the domain, service, and feature hierarchy is already known. It breaks down when the user only knows one small valuable thing worth building.

Without a bottom-up entry path, users face two bad options:

1. invent a premature system/domain/capability structure just to make the feature feel valid; or
2. keep useful feature work outside governed Lens/BMAD flow until enough structure appears.

Both options are harmful. Premature structure creates false truth. Ungoverned work loses the traceability, BMAD handoff, artifact archive, and future evidence benefits that Lens is designed to provide.

## Target Users

- Individual builders who know one useful feature but not the surrounding system yet.
- Product/engineering teams exploring early product slices without mature architecture.
- Lens/BMAD operators who need governed packet capture without accidentally promoting topology.
- Future reporting consumers, such as Auspex users, who need read-only visibility into feature-first work status.

## Product Goal

Create the first safe Bottom-Up LENS support mechanism: a feature packet creator that lets a user capture one independently useful feature without requiring pre-existing hierarchy and without emitting downstream structure too early.

## Core Value Proposition

Bottom-Up LENS packet creation gives users a safe way to say:

> “We do not know the system yet. We know one useful feature. Capture it honestly, bound it tightly, and do not pretend it is a system.”

The feature packet creator turns that statement into a governed artifact BMAD can later use, while preserving the anti-expansion rules that make bottom-up trustworthy.

## MVP Scope

### In Scope

- Dedicated bottom-up packet creation entry surface.
- Candidate-selection gate that selects exactly one feature candidate.
- Context-sufficiency questions for local value, problem, acceptance criteria, constraints, and assumptions.
- Scope-safety gate requiring included scope and explicit out-of-scope.
- Non-inference rules prohibiting system/domain/capability/roadmap/architecture invention.
- Human preview confirmation before writing the packet.
- Machine-readable non-effects receipt proving no adjacency, pressure, promotion, Salmon, Landscape, or Graph updates were emitted.
- Stable identity and provenance fields compatible with future derived-map rebuilds.
- Separate BMAD readiness gate concept.

### Out of Scope

- Full bottom-up lifecycle implementation.
- Automatic adjacency detection.
- Repeated pressure detection.
- Promotion candidate creation.
- Salmon workflow execution.
- Living Landscape updates.
- Derived Graph writes.
- Work Archive migration for existing Lens artifacts.
- Auspex reporting UI changes.

## Required User Flow

1. User invokes the future bottom-up packet creator.
2. NextLens accepts raw context and identifies possible feature candidates.
3. User selects exactly one candidate.
4. NextLens asks sufficiency questions until the selected candidate has local value, problem, acceptance criteria, known constraints, and assumptions.
5. NextLens requires included scope and explicit out-of-scope.
6. NextLens shows a preview containing feature goal, scope, out-of-scope, assumptions, acceptance criteria, constraints, provenance, and non-effects checklist.
7. User confirms the preview.
8. NextLens writes one feature packet.
9. NextLens returns a validator receipt proving no downstream topology or evidence artifacts were emitted.

## Packet Concept

The packet should be small, explicit, and schema-validatable.

Minimum fields:

- `featureId`
- `kind: feature_packet`
- `sourceMode: bottom_up`
- `status`
- `selectedFeature`
- `scope.includedScope`
- `scope.explicitOutOfScope`
- `scope.deferredCandidates`
- `constraints.knownConstraints`
- `constraints.nonInferenceRules`
- `assumptions.unpromoted`
- `provenance`
- `nonEffectsReceipt`
- `topology.belongs_to` with null service/domain/program values unless later promoted

## Success Criteria

- A user can create a bottom-up feature packet without naming a system, domain, capability, roadmap, or architecture.
- The packet captures exactly one independently useful feature.
- The packet includes explicit included scope and out-of-scope.
- The command fails closed when hard constraints are violated.
- The command writes no adjacency, pressure, promotion, Salmon, Landscape, or Graph outputs.
- The packet preserves identity and provenance for future derived-map rebuilds.
- BMAD execution readiness is evaluated separately from packet validity.

## Key Constraints

- Feature validity is local first: it solves a real problem.
- Structure only emerges later from repeated evidence.
- Relationships come before structure.
- Archive evidence is not Living Landscape truth.
- The derived map is a cache, not source truth.
- Salmon is deferred until implementation reveals evidence.
- Reporting must remain read-only and must not imply promotion.

## Strategic Fit

This feature fits the broader NextLens direction by supporting organic, feature-first growth while preserving governance. It aligns with the Two-Tree Model:

- bottom-up packets become Feature Archive evidence;
- Living Landscape truth remains promoted and human-gated;
- derived maps remain rebuildable projections;
- Salmon remains a later consistency workflow.

The feature also prepares for Auspex-style reporting by making packet status and provenance machine-readable without granting reporting tools mutation authority.

## Risks

| Risk | Impact | Mitigation |
|---|---|---|
| Packet schema becomes too large | Recreates top-down planning | Keep MVP fields minimal and enforce non-inference rules |
| Users try to packetize a system | Premature structure | Candidate-selection gate and fail-closed validation |
| BMAD invents hierarchy | Downstream artifacts become false truth | Separate BMAD readiness validator with non-inference instructions |
| Deferred candidates become roadmap | Scope creep | Store deferred candidates as unranked notes only |
| Reporting implies promotion | Stakeholder confusion | Explicit statuses and read-only reporting language |

## Recommendation

Proceed to BusinessPlan with the product direction: **Bottom-Up LENS Feature Packet Creator**.

The BusinessPlan phase should convert this brief into PRD and UX/design artifacts for a constrained MVP that creates one safe bottom-up feature packet and deliberately defers all evidence-driven topology behavior.
