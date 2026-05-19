---
feature: nextlens-src-bottomup
doc_type: brainstorm
status: in-progress
stepsCompleted: [1, 2]
inputDocuments:
  - user-provided Bottom-Up LENS description
session_topic: "Bottom-Up LENS feature-first track support for NextLens"
session_goals: "Clarify the future bottom-up lane, including feature packet creation, BMAD handoff, archive capture, adjacency detection, pressure detection, Salmon correction routing, human-gated promotion, and separation of Archive, Landscape, and Graph."
selected_approach: "ai-recommended"
techniques_used:
  - Question Storming
  - Constraint Mapping
  - Morphological Analysis
ideas_generated: []
question_storm_count: 108
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

The BMAD brainstorming workflow setup is complete. The selected technique approach is AI-recommended techniques.

## Technique Selection

**Approach:** AI-Recommended Techniques

**Analysis Context:** Bottom-Up LENS feature-first track support for NextLens, with focus on defining a future bottom-up lane that is useful without premature system, domain, capability, roadmap, or architecture assumptions.

**Recommended Techniques:**

- **Question Storming:** Recommended first because the feature has a high risk of inventing structure too early. This technique keeps the work honest by generating the questions a safe bottom-up packet creator must answer before any schema, command, or workflow is locked in.
- **Constraint Mapping:** Recommended second because Bottom-Up LENS is defined by anti-expansion constraints: no forced growth, explicit out-of-scope, relationships before structure, advisory promotion, and no accidental adjacency, Salmon, Landscape, or Graph side effects during initial packet creation.
- **Morphological Analysis:** Recommended third because the future capability has several interacting dimensions: candidate selection, context sufficiency, packet fields, confirmation gates, BMAD handoff, archive capture, adjacency detection, pressure detection, Salmon routing, promotion governance, and topology boundaries.

**AI Rationale:** The sequence moves from question discovery to guardrail mapping to structured option design. That keeps ideation generative while preserving the central Bottom-Up LENS rule: build one useful feature, observe evidence, and let larger structure emerge only when reality earns it.

**Total Estimated Time:** About 60 minutes.

## Technique Execution Results

### Question Storming

**Interactive Focus:** Identify the questions NextLens must answer before it can safely write a bottom-up feature packet.

**User Prompt:** What questions must NextLens answer before it writes a bottom-up feature packet?

**User Response:** Help me answer this.

**Facilitator Seed Questions:**

#### Candidate Selection

1. What is the smallest independently useful feature in the user's raw context?
2. What candidate slices are present, and which one is most bounded?
3. What evidence suggests this is a real problem rather than a speculative system idea?

#### Context Sufficiency

4. Who needs this feature, even if the larger system is unknown?
5. What concrete problem does the feature solve right now?
6. What acceptance criteria would prove this feature is useful locally?
7. What constraints are known enough for BMAD to begin without inventing a domain or architecture?

#### Scope Safety

8. What is explicitly included in this packet?
9. What is explicitly out of scope, especially tempting adjacent work?
10. What assumptions must remain unpromoted until implementation produces evidence?

#### BMAD Handoff

11. What does BMAD need in order to produce PRD, architecture, stories, and implementation artifacts from this feature-first input?
12. Which normal BMAD expectations must be relaxed because no system, domain, capability, or roadmap exists yet?

#### Non-Effects

13. What must packet creation not emit: adjacency, pressure, promotion, Salmon, Landscape, or Graph updates?
14. How does the command prove that it only wrote the packet and did not accidentally create downstream structure?

#### Evidence And Promotion

15. What artifacts might later provide adjacency evidence?
16. What repeated pressure signals would be strong enough to suggest promotion?
17. What must a human see before accepting or rejecting a promotion candidate?

#### Correction Routing

18. If implementation reveals the packet was wrong, what Salmon signal should be created and where should it route?

**Emerging Pattern:** The packet command is less about schema generation than restraint. Its core job is to identify one local feature, gather enough context for BMAD, and preserve evidence without turning early guesses into durable structure.

#### Deepened Cluster: Candidate Selection

19. What signal tells NextLens that the user's raw context contains more than one possible bottom-up feature?
20. How should NextLens split candidates without ranking them as a roadmap?
21. What makes one candidate independently useful rather than merely a setup task for another candidate?
22. What candidate is small enough to execute through BMAD without requiring a named capability?
23. What should NextLens do when the user describes a system but only one feature is actionable?
24. What evidence should be shown to the user before asking them to choose one candidate?

#### Deepened Cluster: Context Sufficiency

25. What minimum user/problem/success context is required before a packet can be written?
26. Which missing answers are blockers, and which can be recorded as assumptions?
27. How should NextLens avoid filling unknowns with invented domain or architecture context?
28. What does BMAD need to proceed when the only known object is the selected feature?
29. What should the command do if the selected feature is real but acceptance criteria are weak?
30. How does the sufficiency gate distinguish useful ambiguity from unsafe vagueness?

#### Deepened Cluster: Scope Safety

31. What included scope items are necessary for the selected feature to be independently useful?
32. What out-of-scope items are tempting enough that they must be named explicitly?
33. What should happen when an out-of-scope item appears necessary during packet creation?
34. How should assumptions be labeled so they do not become promoted truth?
35. What wording prevents the packet from implying future system ownership?
36. What is the smallest anti-expansion checklist every packet must pass?

#### Deepened Cluster: BMAD Handoff

37. What fields must be present for BMAD to create PRD, architecture, stories, and implementation work from a feature-first packet?
38. What should BMAD be told not to infer from the packet?
39. How should packet provenance be preserved in downstream BMAD artifacts?
40. What handoff language tells BMAD this is a local feature, not a system-derived feature?
41. How should BMAD capture later evidence without writing to the Living Landscape directly?
42. What validator should fail the handoff if BMAD tries to require a domain, capability, roadmap, or architecture before the feature is valid?

**Deepened Pattern:** The likely MVP is a gated packet creator, not a full bottom-up lifecycle. It should select one candidate, check sufficiency, force explicit scope boundaries, and hand BMAD a feature-first packet with clear non-inference instructions.

#### Domain Pivot: Operator Experience

43. How does the user know they are starting a bottom-up packet and not a top-down planning flow?
44. What language should the command use to keep the user focused on one useful feature?
45. How should candidate choices be presented without implying priority, roadmap order, or architecture hierarchy?
46. What preview should the user see before the packet is written?
47. What explicit confirmation should be required before creating `feature-packet.json`?
48. How should the command explain that no system/domain/capability is needed yet?

#### Domain Pivot: Governance And Auditability

49. What metadata proves the packet came from a bottom-up source mode?
50. What fields distinguish a packet assumption from accepted landscape truth?
51. What audit trail records why a candidate was selected and why other candidates were deferred?
52. What lifecycle state should exist before BMAD receives the packet?
53. Should the packet have a status such as `draft`, `confirmed`, `executed`, or `archived`?
54. What reviewer or gate should confirm that no forbidden downstream side effects were emitted?

#### Domain Pivot: Runtime Evidence

55. What implementation outputs count as artifacts for later adjacency analysis?
56. How should BMAD or Dev record artifact evidence without promoting it?
57. What artifact identifiers are stable enough for graph links but not authoritative enough for landscape truth?
58. When should artifact reuse become an adjacency candidate?
59. What evidence threshold separates one-off reuse from repeated pressure?
60. How should conflicting evidence be recorded when two features appear related but serve different user problems?

#### Domain Pivot: Failure Modes

61. How could the command accidentally create a fake system?
62. How could BMAD accidentally infer architecture from a local feature packet?
63. What happens if the user chooses a feature that is too large for bottom-up?
64. What happens if the user chooses a feature that is too small to be independently useful?
65. How should the command respond when explicit out-of-scope items are actually required for acceptance?
66. What should fail closed if the command cannot prove non-effects?

#### Domain Pivot: Operator Trust

67. What should NextLens say to reassure the user that bottom-up will not trap them in premature architecture?
68. What should NextLens say when it detects adjacency but refuses to promote structure yet?
69. How should promotion suggestions expose uncertainty and confidence?
70. What should a human acceptance screen contain for promotion candidates?
71. How can the user reject a promotion without losing the underlying relationship evidence?
72. How does the system preserve optionality after each bottom-up run?

**Second-Wave Pattern:** The bottom-up packet creator needs two trust surfaces: a user-facing restraint surface that proves it will not overgrow the idea, and a machine-facing evidence surface that preserves artifacts for later analysis without promoting them.

#### Domain Pivot: Packet Schema And Validation

73. What is the minimum packet schema that can express source mode, selected feature, goal, included scope, explicit out-of-scope, acceptance criteria, constraints, and assumptions?
74. Which packet fields are required at creation time, and which are allowed to remain unknown?
75. What packet fields must explicitly prohibit system/domain/capability/roadmap inference?
76. How should schema validation distinguish an empty field from an intentionally unknown field?
77. Should the packet store raw user wording alongside normalized fields for auditability?
78. How should packet versioning work if bottom-up evolves after early features exist?

#### Domain Pivot: Storage Topology

79. Where should the packet live in the Work Archive before BMAD execution begins?
80. What files should exist after packet creation and before BMAD execution?
81. What files should not exist until after implementation creates evidence?
82. How should the archive path avoid implying a promoted domain or system?
83. Should archived bottom-up features live under `docs/features/`, active Lens feature docs, or a dedicated bottom-up namespace?
84. How does the storage model make history durable without turning history into current truth?

#### Domain Pivot: Command Behavior

85. What command name or mode should signal bottom-up packet creation?
86. Should bottom-up packet creation be a new command, a mode of `new-feature`, or a future NextLens track?
87. What happens if the command is run twice for the same candidate?
88. How should the command resume an incomplete candidate-selection or sufficiency gate?
89. What should dry-run output show without writing any packet?
90. What automated tests prove the command has no accidental downstream side effects?

#### Domain Pivot: Human Control

91. What exact confirmation phrase should be required before writing the packet?
92. What does the user need to reject or edit before packet creation?
93. How can the user say, “this is too big,” and return to candidate slicing?
94. How can the user say, “this is too vague,” and return to sufficiency questions?
95. What should the UI do when the user wants to preserve multiple candidate slices but execute only one?
96. What is the simplest review screen that shows selected feature, included scope, out-of-scope, assumptions, and non-effects?

#### Domain Pivot: Salmon And Corrections

97. What packet assumptions should be monitored during BMAD execution?
98. What implementation discoveries become Salmon signals instead of simple implementation notes?
99. How does Salmon route a correction when it affects only the local feature packet?
100. How does Salmon route a correction when it affects multiple bottom-up features through shared artifacts?
101. What is the difference between a scope correction, an adjacency signal, and a promotion signal?
102. How should Salmon avoid turning every local implementation surprise into a landscape change?

#### Domain Pivot: Metrics And Success

103. What counts as successful packet creation before any code is written?
104. What counts as successful BMAD handoff from a bottom-up packet?
105. What count or quality of explicit out-of-scope items indicates the packet is safely bounded?
106. What signal shows that bottom-up preserved optionality rather than creating hidden architecture?
107. What post-run evidence should be inspected before allowing adjacency detection?
108. What later evidence would prove the feature-first approach was better than forcing a top-down model first?

**Third-Wave Pattern:** Bottom-up needs a “minimum viable restraint loop”: slice one candidate, prove sufficient local value, require explicit human confirmation, write only the packet, and preserve evidence for later without creating present-tense structure.

**Question Storming Completion:** The first technique reached 108 questions and surfaced the main candidate MVP: a gated packet creator with a minimum viable restraint loop. This is sufficient to move into constraint mapping without prematurely organizing the entire session.

### Constraint Mapping

**Interactive Focus:** Identify the guardrails, blockers, and non-effects that must constrain future Bottom-Up LENS packet creation.

**Transition Rationale:** Question Storming revealed that safety is the product. The next step is to map the constraints that preserve bottom-up’s core promise: a feature can be valid locally without forcing premature structure.

#### Hard Constraint Map

| Constraint | Meaning | Packet-Creation Consequence |
|---|---|---|
| One candidate only | Packet creation must select exactly one bottom-up feature candidate. | If multiple candidates remain unresolved, stop at candidate selection and ask the user to choose or split. |
| Local value required | The candidate must solve a real problem independently. | If the feature is only a setup task, dependency stub, or abstract system idea, do not write a packet. |
| Explicit out-of-scope required | Tempting adjacent work must be named as out of scope. | If adjacent work is not bounded, stop and require out-of-scope clarification. |
| No structure inference | Do not infer system, domain, capability, roadmap, or architecture. | Any implied hierarchy must be removed or recorded as an unpromoted assumption. |
| No downstream side effects | Packet creation must not emit adjacency, pressure, promotion, Salmon, Landscape, or Graph updates. | The command must fail closed if it cannot prove only the packet was written. |
| Human confirmation required | The user must see and approve a final preview before writing. | No packet file is created before explicit confirmation. |
| Assumptions unpromoted | Assumptions must be labeled and must not become accepted truth. | Packet assumptions remain local packet metadata until evidence and promotion justify elevation. |

**Not Yet Hard-Selected:** BMAD handoff sufficiency is important, but the current selection treats it as a readiness quality rather than a hard packet-creation blocker. This may become a separate handoff validator after packet creation.

#### Failure Response Map

| Failure Response | Applies When | Result |
|---|---|---|
| Stop with blocker reason | Any hard constraint fails. | Name the violated constraint, explain what would satisfy it, and do not write the packet. |
| Return to candidate slicing | Multiple candidates remain, the candidate is oversized, or the user describes a system rather than one feature. | Re-enter candidate selection instead of creating a vague packet. |
| Ask sufficiency questions | Local value, user need, success criteria, constraints, or acceptance criteria are weak. | Continue discovery until the packet is locally useful or explicitly blocked. |
| Require out-of-scope edit | Adjacent or tempting work is not bounded. | Force explicit out-of-scope statements before preview. |
| Show non-effects checklist | Before final confirmation. | Prove the command will not emit adjacency, pressure, promotion, Salmon, Landscape, or Graph updates. |

**Rejected Response:** Do not create a draft anyway when hard constraints fail. Draft-with-warnings would weaken bottom-up’s anti-expansion promise and invite invented structure.

**Constraint Mapping Breakthrough:** The future command should fail closed. It is better to create no packet than to create a packet that smuggles in hierarchy, hides adjacent scope, or triggers downstream evidence mechanisms too early.