---
feature: nextlens-src-implement
doc_type: research
status: draft
updated_at: 2026-05-14
---

# Technical Research - nextlens-src-implement

## Research Goal

Validate the smallest useful top-down implementation path from discovered system context to one selected Feature packet, with deterministic writes, non-mutating doctor checks, and upstream correction signaling.

## Executive Findings

1. The command/write path should treat landscape updates as commands and derived graph output as query projections.
2. Idempotency must be explicit for mutating operations, with stable request tokens and replayed responses for retries.
3. Doctor output should use newline-delimited JSON for streaming, machine-readable checks, and CI compatibility.
4. Workflow assets must be installed in executable workflow locations, not only documented as templates.
5. Multi-source correction signaling should deduplicate by fingerprint to avoid noisy retries and duplicate remediation loops.
6. Context sufficiency must run before ranking so incomplete discovery output cannot silently produce a packet.

## Technical Analysis

### 1. Command-Query Separation Fits the Landscape + Derived Graph Design

The current design intent already separates authoritative mutation surfaces (Living Landscape and packet selection state) from disposable read surfaces (Derived Graph). This aligns with CQRS guidance: write models enforce business consistency; read models optimize query behavior and can lag safely under eventual consistency.

Applied to nextlens:

- Treat Work Archive and Living Landscape files plus packet selection updates as command-side writes.
- Treat Derived Graph files as query-side projections that can be rebuilt eagerly on successful writes.
- Keep query projection rebuild deterministic and disposable, never authoritative.

This supports the existing invariant: stable IDs are source-of-truth, graph is projection.

### 2. Mutating Operations Need First-Class Idempotency

For retry safety, each mutating request should carry a deterministic idempotency token. Best-practice guidance emphasizes:

- generate unique tokens (UUID/KSUID class),
- persist token + result state atomically,
- return prior response for duplicate token reuse,
- expire old tokens with TTL,
- avoid timestamp-only keys and full-payload storage.

Applied to nextlens command spine:

- Each write command (`new-system`, packet write, salmon write) receives a `request_id` token.
- Token record stores status (`pending|completed|failed`) and normalized response envelope.
- Duplicate token returns original envelope, preventing duplicate side effects in retried runs.

This directly supports merge-safe reruns and one-command demo stability.

### 3. Doctor Report Format: JSONL is Correct for CI and Pipelines

JSON Lines format requirements support the doctor contract:

- UTF-8,
- one valid JSON value per line,
- newline terminator,
- stream-friendly processing.

Applied to doctor checks:

- one check result per line,
- stable `check_id`, severity, target path, remediation hint,
- final summary line for aggregate pass/warn/fail.

JSONL makes it trivial to parse with shell tools and test harnesses while preserving strict machine readability.

### 4. Workflow Installability Must Be Checked Against Runtime Location

Executable GitHub workflow files must reside in `.github/workflows`. Template YAML in docs is useful documentation but is not runnable automation.

Applied to identified risk:

- add a doctor rule that reports "template-only workflow" when YAML exists only under docs/reference paths,
- require explicit installed-path presence before treating automation as active.

This closes a known false-confidence gap in readiness checks.

### 5. Retry/Signal Design Requires Deduplication at Failure Fingerprint Level

Because failures can be reported by humans, doctor, and reviews, naive emission causes duplicate correction events. A canonical failure fingerprint should collapse equivalent findings while preserving provenance.

Recommended fingerprint fields:

- normalized failure class,
- target stable ID,
- canonical path or entity reference,
- normalized message hash.

Then attach all sources as evidence on one Salmon event record.

### 6. Top-Down Sufficiency Is Required Before Ranking

Candidate Features should be ranked only after the command verifies that discovery output contains a system thesis, role, outcome, journey or journey hypothesis, traceable candidate Features, captured risks/open questions, and BMAD consumer context. Missing outcome or journey context should block packet emission and recommend return to discovery rather than creating a bottom-up packet.

## Recommended Technical Shape for V1

1. Keep top-down context ingestion in the command spine.
2. Run context sufficiency before ranking candidate Features.
3. Implement request-level idempotency for all write commands.
4. Persist authoritative state by stable ID; rebuild graph projection eagerly after successful writes.
5. Emit doctor as JSONL with one line per check.
6. Enforce write allowlist and block governance/release direct mutation.
7. Emit deduplicated Salmon correction events keyed by failure fingerprint and top-down impacted nodes.

## Risks and Mitigations

- Risk: Eventual consistency confusion between writes and projections.
  Mitigation: Explicit metadata labeling (`authoritative` vs `projection`) and deterministic rebuild timestamp.

- Risk: Duplicate side effects under retries.
  Mitigation: Idempotency token store with atomic write + response replay.

- Risk: Workflow readiness false positives.
  Mitigation: Installed-location validation (`.github/workflows`) in doctor.

- Risk: Correction stream noise.
  Mitigation: Fingerprint-based dedup with multi-source evidence attachment.

- Risk: Ranking arbitrary or under-specified Features.
  Mitigation: Block or warn through context sufficiency before ranking.

## Decision Implications for Implementation

- Confirms RC direction: one command, one selected Feature packet, one report schema, one correction object per failure domain.
- Confirms FA direction: vocabulary/status drift and workflow-installability checks are essential early tests.
- Supports preplan objective: a constrained, auditable first implementation path before broader automation expansion.

## Sources

1. Microsoft Learn - CQRS pattern
   https://learn.microsoft.com/en-us/azure/architecture/patterns/cqrs

2. AWS Well-Architected REL04-BP04 - Make mutating operations idempotent
   https://docs.aws.amazon.com/wellarchitected/latest/framework/rel_prevent_interaction_failure_idempotent.html

3. JSON Lines format specification
   https://jsonlines.org/

4. GitHub Actions workflow syntax (workflow file location requirement)
   https://docs.github.com/en/actions/using-workflows/workflow-syntax-for-github-actions

5. Stripe API idempotent requests (practical replay semantics)
   https://docs.stripe.com/api/idempotent_requests
