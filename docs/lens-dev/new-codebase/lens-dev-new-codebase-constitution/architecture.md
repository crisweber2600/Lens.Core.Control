---
feature: lens-dev-new-codebase-constitution
doc_type: architecture
status: draft
goal: "Define the technical architecture for rewriting bmad-lens-constitution as a thin, read-only governance resolver with partial-hierarchy tolerance and express-track parity"
key_decisions:
  - bmad-lens-constitution stays read-only and delegates all resolution logic to constitution-ops.py
  - Partial-hierarchy tolerance is the core behavioral fix: missing constitution levels warn and continue instead of hard-failing
  - Express-track parity is part of the shared contract: `express` must remain valid when the resolved constitution permits it
  - `sensing_gate_mode` remains an explicit governance field alongside `gate_mode`
  - Negative regressions are required for malformed frontmatter, invalid slugs, traversal attempts, and no-write guarantees
open_questions: []
depends_on:
  - business-plan.md
  - tech-plan.md
blocks: []
updated_at: 2026-05-01T15:20:00Z
---

# Architecture - Constitution Command Rewrite

**Feature:** lens-dev-new-codebase-constitution  
**Author:** Winston (Architect)  
**Date:** 2026-05-01

## Overview

The constitution command is a shared governance primitive, not a feature-specific planner. Its job is to resolve the applicable governance rules for a scope, report the effective constitution, and validate compliance without mutating state. The rewrite keeps the existing command shape but replaces the brittle resolution behavior with a clean-room implementation that tolerates missing hierarchy levels, preserves additive inheritance, and supports the express track when governance permits it.

The architecture is intentionally narrow:

1. Resolve governance rules from org, domain, service, and repo constitutions in order
2. Merge the hierarchy additively with explicit precedence rules
3. Expose compliance and display views for callers without writing to governance
4. Preserve read-only behavior across all code paths

## System Design

### 1. Command Topology

The command remains a three-hop Lens surface:

```
.github/prompts/lens-constitution.prompt.md
  -> lens.core/_bmad/lens-work/prompts/lens-constitution.prompt.md
    -> skills/bmad-lens-constitution/SKILL.md
      -> scripts/constitution-ops.py
```

The prompt layer only delegates. The skill layer only orchestrates. The script performs all resolution and validation logic.

### 2. Resolution Model

Constitution resolution is a layered merge, not a replacement chain.

| Field | Merge rule |
|---|---|
| `permitted_tracks` | intersection |
| `required_artifacts` | union by phase bucket |
| `gate_mode` | strongest wins |
| `sensing_gate_mode` | strongest wins |
| `additional_review_participants` | union |
| `enforce_stories` | true wins |
| `enforce_review` | true wins |

Missing levels are valid. A missing org, domain, or service file produces a warning and continues with the levels that do exist.

### 3. Runtime Boundaries

The command is read-only by design:

- It may read feature metadata and artifact presence for compliance checks
- It may read constitution files under governance
- It may not write governance artifacts
- It may not mutate feature state
- It may not escape the configured constitutions path

### 4. Express-Track Parity

The rewrite must preserve express-track behavior as a first-class governed path. That means the resolved constitution must be able to validate `express` when the active hierarchy allows it, and the compliance display must report that fact without requiring callers to bypass the shared resolver.

### 5. Safety Requirements

The architecture treats hostile or malformed inputs as testable cases, not edge-case trivia. The implementation must reject:

- malformed frontmatter
- invalid slugs
- traversal attempts
- out-of-scope filesystem reads

It must also prove that normal execution does not write to governance or feature state.

## Implications

- The rewrite is a shared dependency for the planning conductors and must stay stable before later planning work expands
- `sensing_gate_mode` must not drift out of the resolved output
- Tests must cover both happy-path resolution and negative safety cases
