---
feature: nextlens-src-bottomup
doc_type: brainstorm
status: in-progress
stepsCompleted: [1]
inputDocuments:
  - user-provided Bottom-Up LENS description
session_topic: "Bottom-Up LENS feature-first track support for NextLens"
session_goals: "Clarify the future bottom-up lane, including feature packet creation, BMAD handoff, archive capture, adjacency detection, pressure detection, Salmon correction routing, human-gated promotion, and separation of Archive, Landscape, and Graph."
selected_approach: ""
techniques_used: []
ideas_generated: []
context_file: ""
lens_context:
  domain: nextlens
  service: src
  feature_id: nextlens-src-bottomup
  phase: preplan
  track: full
  docs_path: docs/nextlens/src/nextlens-src-bottomup
constitutional_context:
  permitted_tracks: [express, full]
  planning_required_artifacts: [business-plan, tech-plan]
  dev_required_artifacts: [stories]
  gate_mode: informational
  enforce_stories: true
  enforce_review: true
updated_at: 2026-05-19T00:00:00Z
---

# Brainstorming Session Results

**Facilitator:** BMad  
**Date:** 2026-05-19

## Session Overview

**Topic:** Bottom-Up LENS as a future feature-first track in NextLens.

**Goals:** Use the supplied Bottom-Up LENS description to explore the shape of a future NextLens capability that lets users begin from one independently useful feature without requiring an existing system, domain, capability, roadmap, or architecture. The session should sharpen the packet, gates, evidence model, correction routing, and promotion boundaries before downstream PrePlan artifacts are created.

### Context Guidance

The provided context establishes that Bottom-Up LENS is planned future functionality, not current behavior. The active top-down planning feature should describe and design that future capability rather than pretending a real bottom-up run is already available.

The source material frames the future capability as:

- Start with one small, independently useful feature.
- Create a minimal feature packet for BMAD.
- Execute through the normal BMAD chain.
- Store the run in a Work Archive.
- Observe produced artifacts and relationships.
- Detect adjacency before structure.
- Watch repeated pressure before suggesting larger structure.
- Suggest promotion only through human acceptance.
- Keep Work Archive, Living Landscape, and Derived Graph separate.
- Route implementation truth and assumption changes through Salmon without turning every correction into a system-level change.

### Session Setup

This brainstorming session is grounded in the supplied Bottom-Up LENS brief and the active Lens feature context:

- **Feature:** nextlens-src-bottomup
- **Domain:** nextlens
- **Service:** src
- **Track:** full
- **Phase:** preplan
- **Write scope:** docs/nextlens/src/nextlens-src-bottomup

The primary brainstorming challenge is not whether bottom-up should exist. The challenge is to discover the smallest coherent planning slice that can define bottom-up packet creation safely while preserving the anti-expansion rules that make the lane useful.

### Initial Focus Areas

The session should keep these areas visible during ideation:

1. Candidate selection: how raw user context becomes one bounded bottom-up feature candidate.
2. Context sufficiency: what must be known before a feature packet can be written.
3. Confirmation: what preview the user must approve before any artifact is created.
4. Feature packet schema: the smallest durable structure BMAD can consume.
5. BMAD handoff: how the packet changes input without replacing BMAD.
6. Archive capture: what belongs in historical work records.
7. Adjacency detection: how related work is noticed without becoming structure.
8. Pressure detection: what repeated evidence is strong enough to suggest promotion.
9. Promotion governance: how suggestions stay advisory and human-gated.
10. Salmon routing: how implementation truth updates assumptions without uncontrolled scope growth.
11. Topology boundaries: how Archive, Landscape, and Graph remain distinct.
12. Non-effects: what initial packet creation must explicitly not emit.

### Constraints To Preserve

- A feature can be valid before any system, domain, capability, roadmap, or architecture is known.
- No larger structure is created without repeated real-world pressure.
- Relationships come before structure.
- Artifact evidence does not automatically imply growth.
- Promotion candidates are advisory, not automatic.
- Initial packet creation must not emit adjacency records, pressure detection, promotion candidates, Salmon signals, Landscape updates, or Graph updates.
- Explicit out-of-scope is mandatory for every bottom-up feature packet.

### Next Required Selection

The BMAD brainstorming workflow setup is complete. The next step is selecting the technique approach for ideation.