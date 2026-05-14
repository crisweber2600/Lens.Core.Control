---
feature: nextlens-src-implement
doc_type: ux-design
status: draft
goal: "Define the operator UX for a deterministic v1 NextLens flow that produces one selected packet, clear validation output, and actionable correction routing."
key_decisions:
  - Use a single guided command spine with explicit stage progression.
  - Preserve one-selection behavior with a required human confirmation checkpoint.
  - Present doctor output in a readable summary with linked JSONL evidence.
  - Route correction actions through structured classes and deduplicated event handling.
open_questions:
  - Final confidence threshold and ranking labels for packet selection candidates.
  - Preferred operator default after doctor reports informational versus blocking findings.
depends_on: [prd]
blocks: []
updated_at: 2026-05-14T00:00:00Z
---

# UX Design - nextlens-src-implement

## 1. UX Scope

This design defines the operator-facing experience for v1 NextLens in a command-driven environment. The UX is not a graphical application; it is a guided, deterministic interaction model that must reduce ambiguity, prevent accidental side effects, and produce auditable outputs.

The design covers:

- flow structure from context ingestion to packet emission,
- interaction states and system responses,
- confirmation and safety checkpoints,
- correction routing decisions and operator feedback.

## 2. Primary User Journey

### Journey A - Deterministic Packet Creation

1. Operator starts NextLens command.
2. System requests or loads context inputs.
3. System computes candidate features and ranking evidence.
4. System displays top candidate details and why it was selected.
5. Operator confirms selection.
6. System writes authoritative state, rebuilds derived projection, emits one packet.
7. System runs doctor checks and outputs summary plus JSONL artifact path.
8. If findings exist, system offers correction routing choices.

Expected result: one packet and one evidence bundle produced with clear status.

### Journey B - Correction Handling

1. Doctor or reviewer reports an issue.
2. System classifies issue into correction class.
3. System deduplicates against existing events.
4. System shows action recommendation and target route.
5. Operator confirms route or defers.

Expected result: correction is tracked and routed without duplicate side effects.

## 3. Information Architecture

The interaction model is stage-based. Each stage has a clear input, output, and completion signal.

1. Context Intake
2. Candidate Selection
3. Confirmation Gate
4. Authoritative Write
5. Projection Rebuild
6. Packet Emission
7. Doctor Validation
8. Correction Routing

Each stage must print a deterministic status line and an outcome line.

## 4. Interaction Design Requirements

### 4.1 Stage Framing

- Every stage starts with a short status header in the format [stage:name].
- Every stage ends with pass, warning, or fail.
- On fail, the system must print the blocking reason and the next valid action.

### 4.2 Candidate Presentation

- Show exactly one selected candidate and up to two alternates.
- For each shown candidate, display score rationale fields in a fixed order.
- Selection rationale must be textual and machine-traceable in evidence output.

### 4.3 Confirmation Gate

- Require explicit operator confirmation before packet emission.
- Provide a safe cancel path that performs no writes.
- If canceled, preserve diagnostic context for resume.

### 4.4 Doctor Results

- Display a concise summary table by severity: blocking, advisory, informational.
- Always provide JSONL path for full machine-readable report.
- Distinguish non-mutating checks from mutating actions in output labels.

### 4.5 Correction Routing

- Map each finding to one correction class with deterministic routing logic.
- Deduplicate using fingerprint identity before creating a new event.
- Surface whether a correction was created, merged, or ignored as duplicate.

## 5. UX States and Messages

### Success State

- Message confirms packet ID, evidence bundle path, and doctor status.
- Message includes reminder that governance publish is phase-owned elsewhere.

### Partial Success State

- Packet generated successfully but advisory findings exist.
- Message includes recommended correction route and confidence level.

### Failure State

- No packet emitted when blocking conditions fail before confirmation.
- Any write attempt failure must show rollback or idempotency replay result.

## 6. Accessibility and Clarity Rules

- Use consistent command vocabulary across all stages.
- Avoid overloaded terms for feature, packet, and correction event.
- Keep lines scannable and avoid long prose in live command output.
- Ensure all critical results are readable in plain text without color reliance.

## 7. UX Acceptance Criteria

1. Operator can complete the full flow without hidden branching.
2. System never emits more than one packet per run in v1 flow.
3. Confirmation gate prevents accidental packet emission.
4. Doctor output is understandable in summary and traceable via JSONL file.
5. Correction routing feedback clearly states created versus deduplicated behavior.
6. Non-mutating and mutating steps are visually and textually distinguishable.

## 8. Deferred UX Enhancements

- Rich comparative candidate views beyond top three.
- Guided correction playbooks by finding category.
- Session memory aids for repeated correction patterns.