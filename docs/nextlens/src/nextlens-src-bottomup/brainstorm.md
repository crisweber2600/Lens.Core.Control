---
feature: nextlens-src-bottomup
doc_type: brainstorm
status: complete
stepsCompleted: [1, 2, 3, 4]
inputDocuments:
  - user-provided Bottom-Up LENS description
  - user-provided Lens/BMAD topology and Auspex transcription from 2026-05-12
session_topic: "Bottom-Up LENS feature-first track support for NextLens"
session_goals: "Clarify the future bottom-up lane, including feature packet creation, BMAD handoff, archive capture, adjacency detection, pressure detection, Salmon correction routing, human-gated promotion, and separation of Archive, Landscape, and Graph."
selected_approach: "ai-recommended"
techniques_used:
  - Question Storming
  - Constraint Mapping
  - Morphological Analysis
ideas_generated: []
question_storm_count: 108
technique_execution_complete: true
session_active: false
workflow_completed: true
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

#### Soft Constraint Map

| Soft Constraint | Why It Matters | Possible Handling |
|---|---|---|
| BMAD handoff sufficiency | BMAD needs enough context to avoid inventing missing hierarchy. | Warn or route to additional sufficiency questions before handoff; may become hard at BMAD handoff time rather than packet creation time. |
| Packet schema minimalism | Over-large packet schemas can recreate top-down planning under a different name. | Keep only fields needed for local value, scope, acceptance, constraints, assumptions, and provenance. |
| Candidate preservation | Users may identify multiple valuable candidates but execute only one. | Store deferred candidates as session notes, not packets, roadmap, or adjacency. |
| Language neutrality | Labels can accidentally imply structure. | Prefer “selected feature,” “candidate,” “artifact evidence,” and “unpromoted assumption” over “capability,” “domain,” or “system.” |
| Resume safety | Multi-step intake may be interrupted. | Resume at the last unanswered gate without writing partial downstream artifacts. |
| Reviewability | Human operators need to trust the packet. | Provide compact preview with selected feature, included scope, out-of-scope, assumptions, acceptance criteria, constraints, and non-effects checklist. |

#### Validator Ideas

1. **Single Candidate Validator:** Fails if more than one candidate is marked selected.
2. **Local Value Validator:** Fails if goal/problem/success fields are absent or only describe future architecture.
3. **Explicit Out-of-Scope Validator:** Fails if out-of-scope is empty or generic, such as “everything else.”
4. **No Structure Validator:** Flags system/domain/capability/roadmap/architecture claims unless explicitly stored as unpromoted assumptions.
5. **Non-Effects Validator:** Confirms no adjacency, pressure, promotion, Salmon, Landscape, or Graph outputs are created by the packet command.
6. **Confirmation Validator:** Fails if packet write is attempted without a recorded preview approval.
7. **Provenance Validator:** Ensures source mode, raw input summary, selected candidate rationale, and deferred candidate notes are recorded.

#### Design Tensions

- **Enough context vs. invented structure:** BMAD needs actionable input, but bottom-up must not fabricate a larger model.
- **Candidate slicing vs. roadmap creation:** The command may discover multiple candidates, but it must not turn them into a prioritized roadmap.
- **Evidence preservation vs. premature graphing:** Artifacts should be available later, but initial packet creation cannot create graph truth.
- **Human confirmation vs. flow speed:** Confirmation protects safety but can make quick feature capture feel heavy.
- **Strict out-of-scope vs. useful discovery:** Out-of-scope protects focus, but some excluded items may later become Salmon corrections or separate candidates.

**Constraint Mapping Second Breakthrough:** BMAD handoff sufficiency is likely a boundary validator at the transition from packet creation to BMAD execution, not necessarily a blocker to saving a confirmed packet. This keeps packet capture small while still preventing BMAD from inventing missing context.

#### Working Tension Resolutions

| Tension | Working Resolution |
|---|---|
| BMAD context vs. invented structure | Split packet validity from BMAD handoff readiness. A packet can be saved with local feature truth and assumptions, but BMAD execution requires a handoff validator that confirms sufficient goal, scope, acceptance, constraints, and non-inference instructions. |
| Candidate slicing vs. roadmap creation | Candidate discovery can list deferred candidates only as unranked notes. It must not assign priority, sequence, roadmap position, dependency order, capability grouping, or ownership. |
| Evidence preservation vs. graphing | Packet creation records no graph edges. Later implementation artifacts may be archived as evidence, and separate post-run analysis may propose adjacency candidates without making them authoritative. |
| Out-of-scope vs. discovery | Out-of-scope items are excluded from the current packet but can later become separate candidates or Salmon corrections if implementation evidence proves they are necessary. They do not automatically expand the packet. |

**Constraint Mapping Third Breakthrough:** The future design likely needs two gates after candidate selection: a **packet validity gate** that allows safe archive capture, and a later **BMAD handoff readiness gate** that determines whether execution can begin without invented context.

### Morphological Analysis

**Interactive Focus:** Explore design dimensions and coherent option combinations for the future Bottom-Up LENS packet creator.

**Transition Rationale:** Question Storming generated the problem space, and Constraint Mapping identified safety rails. Morphological Analysis now turns those dimensions into possible implementation shapes without prematurely choosing architecture.

#### Design Dimensions Matrix

| Dimension | Option A | Option B | Option C |
|---|---|---|---|
| Entry Surface | New bottom-up command | Mode of `new-feature` | Track within NextLens planning |
| Candidate Handling | Single selected candidate only | Multiple candidates with one selected | Candidate backlog notes retained separately |
| Packet Validity Gate | Strict before write | Draft allowed with blockers | Save only after preview confirmation |
| BMAD Readiness Gate | Same as packet validity | Separate post-packet validator | Deferred until user invokes BMAD |
| Storage Location | Active feature docs path | `docs/features/<feature-id>/` archive path | Dedicated bottom-up archive namespace |
| Deferred Candidate Treatment | Discard after selection | Store as unranked notes | Convert to future feature candidates |
| Non-Effects Proof | Checklist in preview | Automated validator receipt | Both preview checklist and validator receipt |
| Evidence Handling | No evidence until implementation | Archive evidence only | Archive plus later adjacency proposal job |
| Salmon Handling | No Salmon at packet creation | Salmon only after BMAD execution | Salmon on sufficiency failures too |
| Promotion Handling | Not available in MVP | Advisory candidates after repeated pressure | Human-gated promotion workflow |

#### Coherent Combination Candidates

**Combination 1: Minimal Packet Creator**

- Entry Surface: New bottom-up command
- Candidate Handling: Single selected candidate only
- Packet Validity Gate: Save only after preview confirmation
- BMAD Readiness Gate: Separate post-packet validator
- Storage Location: Active feature docs path for this top-down delivery; future Work Archive path for real bottom-up runs
- Deferred Candidate Treatment: Store as unranked notes
- Non-Effects Proof: Both preview checklist and validator receipt
- Evidence Handling: No evidence until implementation
- Salmon Handling: No Salmon at packet creation
- Promotion Handling: Not available in MVP

**Combination 2: Track-Centric Bottom-Up Lane**

- Entry Surface: Track within NextLens planning
- Candidate Handling: Multiple candidates with one selected
- Packet Validity Gate: Strict before write
- BMAD Readiness Gate: Same as packet validity
- Storage Location: Dedicated bottom-up archive namespace
- Deferred Candidate Treatment: Convert to future feature candidates
- Non-Effects Proof: Automated validator receipt
- Evidence Handling: Archive plus later adjacency proposal job
- Salmon Handling: Salmon only after BMAD execution
- Promotion Handling: Advisory candidates after repeated pressure

**Combination 3: Discovery-Heavy Intake**

- Entry Surface: Mode of `new-feature`
- Candidate Handling: Candidate backlog notes retained separately
- Packet Validity Gate: Draft allowed with blockers
- BMAD Readiness Gate: Deferred until user invokes BMAD
- Storage Location: `docs/features/<feature-id>/` archive path
- Deferred Candidate Treatment: Store as unranked notes
- Non-Effects Proof: Checklist in preview
- Evidence Handling: Archive evidence only
- Salmon Handling: Salmon on sufficiency failures too
- Promotion Handling: Human-gated promotion workflow

**Morphological Insight:** Combination 1 best matches the current top-down feature’s likely MVP because it implements the first safe slice: packet creation with hard anti-expansion gates, no runtime evidence systems, and no promotion machinery.

#### Selected Hybrid MVP Shape

The selected direction is a hybrid: bottom-up should be visible as a future NextLens track/lane, but the current MVP slice should implement only safe packet creation.

**Selected Components:**

- **New bottom-up command:** A dedicated entry surface makes the mode obvious and avoids overloading top-down feature creation semantics.
- **Track within NextLens planning:** The command belongs to a future bottom-up lane/track in the product model, even if this top-down feature only designs the first mechanism.
- **Single selected candidate only:** Each packet run creates one feature packet for one independently useful feature.
- **Deferred candidates as unranked notes:** Other possible slices may be preserved as notes, but not as roadmap, priority, adjacency, dependency, or capability structure.
- **Preview confirmation before write:** The user sees selected feature, scope, out-of-scope, assumptions, acceptance criteria, and non-effects proof before file creation.
- **Separate BMAD readiness gate:** Packet validity and BMAD handoff readiness are separate. A packet can be valid for archive capture before it is ready for BMAD execution.
- **Non-effects checklist and validator receipt:** The command should provide both a human-readable checklist and a machine-verifiable receipt showing no adjacency, pressure, promotion, Salmon, Landscape, or Graph updates occurred.
- **No Salmon or promotion in MVP:** Correction and promotion machinery are intentionally deferred until post-implementation evidence exists.

**Selected Hybrid Rationale:** This shape preserves the strategic truth that bottom-up is a future NextLens lane while keeping this feature from trying to build the entire lane. It creates the first safe mechanism: one confirmed, locally valid feature packet with proof that no larger structure was created.

**Morphological Breakthrough:** The product concept is “bottom-up track support,” but the implementation slice should be “create a bottom-up feature packet with fail-closed restraint gates.”

## Idea Organization and Prioritization

### Session Achievement Summary

- **Total divergent prompts generated:** 108 Question Storming questions
- **Creative techniques used:** Question Storming, Constraint Mapping, Morphological Analysis
- **Primary breakthrough:** Bottom-Up LENS should be represented as a future NextLens lane, but this feature should focus on the first safe slice: a bottom-up feature packet creator with fail-closed gates.

### Thematic Organization

#### Theme 1: Minimum Viable Restraint Loop

_Focus: The smallest safe mechanism that creates one packet without creating larger structure._

**Ideas in this cluster:**

- Slice one candidate from raw context.
- Prove local value before writing anything.
- Require explicit included scope and out-of-scope.
- Show preview before write.
- Emit non-effects proof.

**Pattern Insight:** The MVP is not “build bottom-up.” The MVP is “make one bottom-up feature packet safely.”

#### Theme 2: Two-Gate Model

_Focus: Separating safe packet capture from readiness for BMAD execution._

**Ideas in this cluster:**

- Packet validity gate confirms the packet can be written and archived.
- BMAD handoff readiness gate confirms BMAD has enough context to proceed.
- A packet can be valid before it is executable.
- Handoff sufficiency should prevent BMAD from inventing hierarchy.

**Pattern Insight:** Separating these gates keeps capture lightweight while preserving downstream execution quality.

#### Theme 3: Anti-Expansion Governance

_Focus: Constraints that stop bottom-up from becoming premature top-down architecture._

**Ideas in this cluster:**

- No system, domain, capability, roadmap, or architecture inference.
- Deferred candidates remain unranked notes.
- Assumptions remain unpromoted.
- No adjacency, pressure, promotion, Salmon, Landscape, or Graph outputs during packet creation.

**Pattern Insight:** Bottom-up’s defining value is restraint. The command should fail closed when restraint cannot be proven.

#### Theme 4: Human Confirmation and Trust

_Focus: Making restraint visible and reviewable to the operator._

**Ideas in this cluster:**

- Preview selected feature, included scope, out-of-scope, assumptions, acceptance criteria, and constraints.
- Require confirmation before writing packet.
- Show non-effects checklist.
- Provide machine-verifiable validator receipt.

**Pattern Insight:** Users must be able to see that the command did not silently grow the idea.

#### Theme 5: Evidence Deferred Until Reality Exists

_Focus: Preserving future evidence paths without creating them too early._

**Ideas in this cluster:**

- No implementation evidence exists at packet creation time.
- No graph edges are created by packet creation.
- Later BMAD/Dev outputs can be archived as evidence.
- Post-run analysis may later propose adjacency candidates.
- Salmon and promotion are deferred until implementation reveals truth.

**Pattern Insight:** Packet creation prepares for learning but does not pretend learning has already happened.

### Breakthrough Concepts

1. **Fail-Closed Packet Creation:** If the command cannot prove a hard constraint, it writes nothing.
2. **Unranked Deferred Candidates:** Raw context can produce multiple possible features, but only one is selected; the rest are notes, not roadmap.
3. **Non-Effects Receipt:** Packet creation should produce proof that it did not create adjacency, pressure, promotion, Salmon, Landscape, or Graph updates.
4. **Packet Validity vs. BMAD Readiness:** Saving a locally valid packet and executing through BMAD are separate transitions.
5. **Future Lane, Present Slice:** The product concept is a future bottom-up lane, but the current feature slice is only the packet creator.

### Prioritization Results

#### Top Priority Ideas

1. **Hybrid MVP: Dedicated bottom-up command plus future track/lane semantics**
  - Highest strategic alignment: makes bottom-up explicit while preserving MVP boundaries.
2. **Minimum viable restraint loop**
  - Highest safety value: captures one useful feature without premature architecture.
3. **Two-gate model**
  - Highest downstream quality value: avoids conflating packet creation with BMAD execution readiness.

#### Quick Win Opportunities

- Define the packet preview fields.
- Define the non-effects checklist.
- Define the hard constraints and failure responses.
- Define deferred candidate note semantics.

#### Longer-Term Deferred Opportunities

- Post-implementation adjacency proposal jobs.
- Repeated pressure detection.
- Salmon routing for bottom-up assumptions.
- Human-gated promotion workflows.
- Work Archive, Living Landscape, and Derived Graph implementation details.

### Action Planning

#### Action 1: Define the Bottom-Up Feature Packet Contract

**Why This Matters:** The packet is the handoff object that lets BMAD execute one feature without requiring pre-existing structure.

**Next Steps:**

1. Specify required fields: `sourceMode`, `selectedFeature`, `goal`, `includedScope`, `explicitOutOfScope`, `acceptanceCriteria`, `knownConstraints`, `assumptions`, `nonInferenceRules`, and `provenance`.
2. Define which fields are required for packet validity.
3. Define which fields are required for BMAD handoff readiness.

**Success Indicators:** A future validator can distinguish valid packet, invalid packet, and valid-but-not-BMAD-ready packet.

#### Action 2: Define the Fail-Closed Gate Sequence

**Why This Matters:** The gate sequence preserves bottom-up’s anti-expansion promise.

**Next Steps:**

1. Candidate-selection gate: exactly one selected feature candidate.
2. Context-sufficiency gate: local value, problem, success, acceptance, and constraints are known enough.
3. Scope-safety gate: included and out-of-scope boundaries are explicit.
4. Non-inference gate: no implied system/domain/capability/roadmap/architecture.
5. Confirmation gate: final preview accepted before write.
6. Non-effects gate: no adjacency, pressure, promotion, Salmon, Landscape, or Graph outputs.

**Success Indicators:** Any hard failure returns a blocker and writes no packet.

#### Action 3: Define the MVP Boundary

**Why This Matters:** The current top-down feature must not accidentally plan the entire bottom-up ecosystem.

**Next Steps:**

1. Include packet creation, candidate selection, sufficiency questions, preview, and validator receipt.
2. Exclude adjacency detection, repeated pressure detection, Salmon routing, promotion, Landscape updates, and Graph updates.
3. Record deferred concepts as future capability candidates, not current acceptance criteria.

**Success Indicators:** The feature can be implemented as one coherent packet-creation mechanism.

## Session Summary and Insights

### Key Achievements

- Confirmed that Bottom-Up LENS should be framed as a future NextLens lane, not current bottom-up execution.
- Generated 108 questions across candidate selection, sufficiency, scope safety, BMAD handoff, non-effects, evidence, promotion, command behavior, Salmon, and success metrics.
- Identified hard constraints and fail-closed responses.
- Resolved key tensions around BMAD context, deferred candidates, evidence preservation, and out-of-scope discovery.
- Selected a hybrid MVP direction: dedicated future bottom-up entry surface, track/lane semantics, one feature packet, two gates, non-effects proof, and no Salmon/promotion in MVP.

### Final Brainstorm Recommendation

This PrePlan should carry forward the following recommendation:

> Build the first Bottom-Up LENS support slice as a dedicated bottom-up feature packet creator for a future NextLens bottom-up lane. The packet creator must select exactly one independently useful feature, validate local sufficiency and scope boundaries, require human preview confirmation, emit a non-effects receipt, and defer BMAD execution readiness, adjacency, pressure, Salmon, promotion, Landscape, and Graph behavior until later evidence-bearing phases.

### Session Reflection

The strongest creative discovery was that bottom-up’s first mechanism is not growth; it is restraint. The packet creator earns trust by proving what it did not do.

## Integrated Prior Context — Lens/BMAD Topology and Auspex

### Source Context

The user supplied a consolidated transcription from an earlier brainstorming and architecture session titled:

> Re-examining Lens/BMAD project artifact topology for organic, multi-feature, team-scale work

That session examined the knowledge-scatter and feature-pocket-universe problem in Lens/BMAD project artifacts. It used a progressive flow with First Principles Thinking, Morphological Analysis, Concept Blending, and Solution Matrix.

The supplied material also included Auspex context: a read-only reporting UI for making project delivery artifacts and status visible to stakeholders without requiring direct GitHub access.

### Key Findings To Carry Forward

#### 1. Feature Folders Are Work Archives, Not Durable Truth Homes

The prior session concluded that features are units of work, not the right long-term owners of service, domain, or program knowledge. Feature artifacts record what happened during a feature, but they do not provide a stable current-state view once work becomes cumulative.

**Bottom-Up LENS implication:** A bottom-up feature packet should be treated as historical work evidence in the Feature Archive. It should not automatically become Living Landscape truth.

#### 2. Humans Need Places, Not Search Queries

The prior session identified a human usability failure: machines can index scattered artifacts, but humans need stable homes for current truth. The current feature-pocket model makes related knowledge hard to find when projects become organic, iterative, or team-scale.

**Bottom-Up LENS implication:** Bottom-up can start from one feature, but promoted truth must eventually land in a stable human-facing landscape home. Until promotion happens, the packet remains archive evidence.

#### 3. Two Failure Modes Must Stay Separate

- **Knowledge Consolidation:** Durable design truth is scattered across feature folders and branches.
- **Cross-feature Dependency at Authoring Time:** New work needs sibling or predecessor feature context that may be inaccessible, incomplete, stale, or branch-isolated.

**Bottom-Up LENS implication:** The packet creator MVP should not try to solve both. Its job is safe packet capture. Later bottom-up capabilities may use archive evidence, derived maps, and landscape ledgers to solve consolidation and authoring-time dependency loading.

#### 4. Two-Tree Model With Derived Map

The prior session’s strongest topology recommendation was a Two-Tree Model with Derived Map:

- **Feature Archive:** Permanent, flat/stable, never reorganized; contains feature scratchpads, WIP notes, closed artifacts, and feature-level evidence.
- **Landscape:** Reorganizable service/domain/program ledgers; contains current human-readable truth.
- **Governance Map:** Derived projection rebuilt from frontmatter; machine-readable cache, never hand-authored source truth.
- **Salmon Workflow:** Upstream-impact signal that triggers recursive consistency checks across the topology.

Core prior statement:

> Features are immutable facts. The landscape is an interpretation. The map is a cache.

**Bottom-Up LENS implication:** This maps directly onto bottom-up safety:

- A bottom-up packet is an immutable archived fact about a selected feature attempt.
- Adjacency and promotion are interpretations made later from repeated evidence.
- The Graph/Map is a derived projection, not authoritative truth.

#### 5. Stable IDs Must Decouple Identity From Path

The prior session concluded Lens must stop treating path as identity. Features, services, domains, and programs need stable IDs, while landscape paths may reorganize as topology grows.

**Bottom-Up LENS implication:** Bottom-up packets should carry stable IDs and provenance early. Archive paths should be stable enough for historical traceability, while future landscape placement remains a promoted interpretation rather than packet-time identity.

#### 6. Planning Branch Dependence Is A Future Design Concern

The prior session argued that planning docs should no longer depend on branch isolation for legitimacy, and that draft/published state should be explicit metadata rather than implicit branch location.

**Bottom-Up LENS implication:** Future bottom-up packet state should be explicit: for example `draft`, `confirmed`, `valid`, `bmad-ready`, `executed`, or `archived`. This is a future topology design consideration; the current Lens control-repo branch model still governs this PrePlan session.

#### 7. Salmon Is Active Consistency Maintenance

The prior session defined Salmon as more than a notification. A feature-level upstream-impact signal is non-blocking by default, but it triggers recursive checks upward and downward. Blocks come from discovered material inconsistency, not from merely raising the signal.

**Bottom-Up LENS implication:** The current bottom-up MVP should still defer Salmon. However, packet assumptions and implementation discoveries should be designed so a later Salmon workflow can distinguish:

- local packet correction
- cross-feature adjacency signal
- repeated-pressure promotion signal
- landscape consistency issue

### Refined Brainstorm Recommendation

Original brainstorm recommendation:

> Build the first Bottom-Up LENS support slice as a dedicated bottom-up feature packet creator for a future NextLens bottom-up lane. The packet creator must select exactly one independently useful feature, validate local sufficiency and scope boundaries, require human preview confirmation, emit a non-effects receipt, and defer BMAD execution readiness, adjacency, pressure, Salmon, promotion, Landscape, and Graph behavior until later evidence-bearing phases.

Refined with topology context:

> Build the first Bottom-Up LENS support slice as a dedicated bottom-up feature packet creator for a future NextLens bottom-up lane. The packet creator should write one stable, archived feature packet as historical work evidence, not as Living Landscape truth. It must select exactly one independently useful feature, validate local sufficiency and scope boundaries, require human preview confirmation, emit a non-effects receipt, preserve stable identity and provenance for future derived-map reconstruction, and defer BMAD readiness, adjacency, pressure, Salmon, promotion, Landscape, and Graph behavior until later evidence-bearing phases.

### New Design Implications

#### Packet Location and Topology

The bottom-up packet should be designed with the Two-Tree Model in mind:

- **Short term for this feature:** write within the active Lens feature docs path, because this work is still a top-down planning feature.
- **Future bottom-up lane:** write bottom-up packets into the permanent Feature Archive, such as `docs/features/<feature-id>/feature-packet.json`, not directly into a service/domain/program landscape.

#### Packet Metadata

Packet metadata should include reconstructible identity and relationship hints without creating promoted structure:

```yaml
featureId: <stable-feature-id>
kind: feature_packet
sourceMode: bottom_up
status: confirmed
docs_path: <stable archive path>
belongs_to:
  service: null
  domain: null
  program: null
assumptions_promoted: false
landscape_promoted: false
graph_edges_emitted: false
```

#### Deferred Candidate Treatment

Deferred candidates may be retained as unranked notes in the feature archive, but they should not become roadmap, dependency, service, domain, program, or graph truth.

#### Derived Map Compatibility

The packet should be parseable by a future projection rebuild command. The derived map must remain a cache rebuilt from source files, not a hand-authored authority.

#### Future Auspex Reporting Fit

Auspex reinforces the need for read-only stakeholder views over Lens artifacts. Bottom-up packet metadata should be reportable without giving stakeholders direct repository access.

Potential Auspex reporting fields:

- bottom-up packet count
- packet status distribution
- packets awaiting BMAD readiness
- packets executed but not promoted
- promotion candidates awaiting human decision
- stale packets with no evidence update

These reporting views must remain read-only and must not mutate governance or landscape artifacts.

### Updated Breakthrough Statement

> Bottom-Up LENS should create feature-first historical evidence in the Feature Archive, not current topology in the Landscape. The Landscape may later interpret repeated evidence, and the derived map may later index relationships, but packet creation itself must remain a restrained archival act.

### Updated Action Additions For Downstream Work

1. Evaluate how bottom-up packet storage should align with the Two-Tree Model.
2. Define which packet metadata must be sufficient for future projection-map rebuilds.
3. Distinguish archive evidence from Living Landscape truth in all product language.
4. Treat Salmon as a deferred consistency workflow, not part of MVP packet creation.
5. Consider Auspex read-only reporting needs when choosing packet status fields.