---
feature: nextlens-src-topdownlens
doc_type: guide
story_id: TL-3
title: Bottom-Up Compatibility Rules
updated_at: 2026-05-14T04:55:00Z
---

# Bottom-Up Compatibility Rules

TopDownLens keeps the feature-first path valid. A feature can stand alone forever. Promotion to capability, product area, domain, service, or system is advisory and requires evidence.

## Core Rules

- A bottom-up feature starts from one useful need, not from a platform assumption.
- Adjacency is weak by default; shared artifacts, users, risks, or workflows do not automatically create a capability.
- No growth without pressure: promotion requires repeated evidence.
- Promotion changes interpretation, not the identity of the original feature.
- Feature IDs remain stable when paths, parents, or promoted context change.

## Repeated Pressure Categories

Promotion may be suggested when two or more features show repeated pressure through:

- Artifact reuse.
- Repeated workflow steps.
- Shared dependency or integration risk.
- Common stakeholder ownership.
- Repeated review findings.
- Repeated Salmon signals against the same boundary.

## Promotion Path

The usual advisory path is:

```text
feature -> capability -> product area -> domain/service/system context
```

Promotion can skip levels only when evidence explains why. Services remain implementation consequences; they are not required parents for every feature.

## Coexistence With Top-Down Discovery

Top-down discovery is described in `guides/top-down-discovery.md` and `walkthroughs/top-down-example-1.md`. Bottom-up work can later connect to those same entity types, but it does not need to invent a system, product area, or capability on day one.