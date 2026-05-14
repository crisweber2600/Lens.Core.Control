---
feature: nextlens-src-implement
doc_type: ux-design
status: draft
goal: "Define the operator UX for a deterministic v1 NextLens top-down bridge that produces one selected Feature packet, clear validation output, and actionable correction routing."
key_decisions:
  - Use a single guided command spine with explicit top-down context sufficiency before ranking.
  - Preserve one-selection behavior with a required human confirmation checkpoint.
  - Present doctor output in a readable summary with linked JSONL evidence.
  - Route correction actions through structured top-down impact classes and deduplicated event handling.
open_questions:
  - Final confidence threshold and ranking labels for packet selection candidates.
  - Preferred operator default after doctor reports informational versus blocking findings.
depends_on: [prd]
blocks: []
updated_at: 2026-05-14T00:00:00Z
---

# UX Design - nextlens-src-implement

## 1. UX Scope

This design defines the operator-facing experience for v1 NextLens in a command-driven environment. The UX is not a graphical application; it is a guided, deterministic top-down bridge from discovered system context to one selected Feature packet.

The design covers:

- flow structure from top-down context intake to packet emission,
- context sufficiency reporting before candidate ranking,
- interaction states and system responses,
- confirmation and safety checkpoints,
- correction routing decisions and operator feedback.

## 2. Primary User Journey

### Journey A - Deterministic Top-Down Packet Creation

1. Operator starts NextLens command.
2. System requests or loads top-down context from discovery output.
3. System reports context sufficiency as ready, ready_with_warnings, or blocked.
4. System ranks candidate Features by outcome alignment, journey criticality, role value, risk reduction, dependency readiness, implementation boundedness, BMAD readiness, evidence clarity, and open-question severity.
5. System displays top candidate details and why it was selected.
6. Operator confirms selection.
7. System writes authoritative state and rebuilds derived projection.
8. System runs pre-flight doctor checks; blocking findings stop packet emission, while non-blocking findings are summarized with a JSONL artifact path.
9. If validation passes, system emits one packet and then offers correction routing choices when findings remain.

Expected result: one packet and one evidence bundle produced with clear status.

### Journey B - Correction Handling

1. Doctor or reviewer reports an issue.
2. System classifies issue into top-down impact level.
3. System deduplicates against existing events.
4. System shows action recommendation and target route.
5. Operator confirms route or defers.

Expected result: correction is tracked and routed without duplicate side effects.

## 3. Information Architecture

The interaction model is stage-based. Each stage has a clear input, output, and completion signal.

1. Top-Down Context Intake
2. Context Sufficiency
3. Candidate Feature Selection
4. Confirmation Gate
5. Authoritative Write
6. Projection Rebuild
7. Doctor Validation
8. Packet Emission
9. Correction Routing

Each stage must print a deterministic status line and an outcome line.

## 4. Interaction Design Requirements

### 4.1 Stage Framing

- Every stage starts with a short status header in the format [stage:name].
- Every stage ends with pass, warning, or fail.
- On fail, the system must print the blocking reason and the next valid action.

### 4.2 Context Sufficiency Presentation

- Show system thesis, role, outcome, journey or journey hypothesis, candidate Feature trace, risks/open questions, and BMAD consumer context checks in a fixed order.
- If status is `blocked`, do not show a confirmation prompt or emit a packet; recommend returning to discovery.
- If status is `ready_with_warnings`, require explicit operator confirmation before ranking continues.

### 4.3 Candidate Presentation

- Show exactly one selected candidate and up to two alternates.
- For each shown candidate, display score rationale fields in a fixed order.
- Selection rationale must be textual and machine-traceable in evidence output.
- Explain that candidate Features are ranked by their ability to prove part of the discovered system, not as arbitrary backlog items.

### 4.4 Confirmation Gate

- Require explicit operator confirmation before packet emission.
- Provide a safe cancel path that performs no writes.
- If canceled, preserve diagnostic context for resume.

### 4.5 Doctor Results

- Display a concise summary table by severity: blocking, advisory, informational.
- Always provide JSONL path for full machine-readable report.
- Distinguish non-mutating checks from mutating actions in output labels.
- Flag selected Feature scope that spills into adjacent journeys, future Features, full platform architecture, or unrelated outcomes.

### 4.6 Correction Routing

- Map each finding to one Salmon impact level with deterministic routing logic.
- Deduplicate using fingerprint identity before creating a new event.
- Surface whether a correction was created, merged, or ignored as duplicate.

## 5. UX States and Messages

### Success State

- Message confirms packet ID, evidence bundle path, and doctor status.
- Message includes reminder that governance publish is phase-owned elsewhere.

### Partial Success State

- Packet generated successfully but context or Doctor advisory findings exist.
- Message includes recommended correction route and confidence level.

### Failure State

- No packet emitted when blocking conditions fail before confirmation.
- Any write attempt failure must show rollback or idempotency replay result.

## 6. Accessibility and Clarity Rules

- Use consistent command vocabulary across all stages.
- Avoid overloaded terms for feature, packet, and correction event.
- Use Feature as the official public operational unit; only mention legacy "slice" terminology when referencing historical discussion.
- Keep lines scannable and avoid long prose in live command output.
- Ensure all critical results are readable in plain text without color reliance.

## 7. UX Acceptance Criteria

1. Operator can complete the full flow without hidden branching.
2. System never emits more than one packet per run in v1 flow.
3. Complete top-down context ranks candidates, requires confirmation, emits exactly one Feature packet, and preserves traceability to system -> Role -> Outcome -> Journey -> Feature.
4. Ambiguous context with no outcome or journey blocks packet emission, produces a context sufficiency report, and recommends returning to discovery.
5. Candidate Feature scope that includes adjacent journeys or future Features is flagged by Doctor and blocked or marked ready_with_warnings by severity.
6. Implementation-discovered journey assumption errors route through Salmon to impacted Feature, Journey, Outcome, and BMAD correct-course recommendation.
7. Confirmation gate prevents accidental packet emission.
8. Doctor output is understandable in summary and traceable via JSONL file.
9. Correction routing feedback clearly states created versus deduplicated behavior.
10. Non-mutating and mutating steps are visually and textually distinguishable.

## 8. Deferred UX Enhancements

- Rich comparative candidate views beyond top three.
- Guided correction playbooks by finding category.
- Session memory aids for repeated correction patterns.
