---
feature: lens-dev-new-codebase-complete
doc_type: tech-plan
status: draft
goal: "Implement clean-room /complete parity through a retained prompt, the bmad-lens-complete conductor, and a focused archive script without changing lifecycle schemas"
key_decisions:
  - Keep complete as a dedicated closure skill rather than folding archival behavior into dev or finalizeplan.
  - Preserve a split responsibility model: the skill orchestrates confirmation, retrospective, and document-project; the script owns precondition checks and archive writes.
  - Canonicalize the archive summary artifact as summary.md to match the current script, tests, and governance feature folders.
  - Validate parity through focused command-level tests instead of source-level similarity.
open_questions: []
depends_on:
  - lens-dev-new-codebase-baseline
blocks: []
updated_at: 2026-04-27T21:28:18Z
---

# Technical Plan - Complete Command

## Technical Summary

Implement `complete` as a clean-room retained command that preserves the existing closure architecture: prompt stub -> release prompt -> `bmad-lens-complete` skill -> `complete-ops.py` subcommands, with retrospective and document-project delegated as prerequisite workflows. The design goal is to preserve observable closure semantics, not to invent a broader archive system.

The implementation must keep four invariants intact:

1. `complete` only archives features already in a completable phase.
2. The user is warned or blocked before irreversible execution.
3. Final project documentation exists before governance state is finalized.
4. The terminal archive record remains recognizable through `feature.yaml`, `feature-index.yaml`, and `summary.md`.

## Architecture Overview

The retained command chain should remain:

```text
published prompt stub
  -> release prompt: _bmad/lens-work/prompts/lens-complete.prompt.md
  -> skill: _bmad/lens-work/skills/bmad-lens-complete/SKILL.md
  -> script: _bmad/lens-work/skills/bmad-lens-complete/scripts/complete-ops.py
  -> delegated prerequisites: bmad-lens-retrospective, bmad-lens-document-project
  -> outputs: retrospective.md, control-repo docs, governance docs mirror, feature.yaml, feature-index.yaml, summary.md
```

The skill remains the workflow conductor. It resolves configuration and feature context, enforces the irreversible confirmation gate, and ensures the retrospective and document-project steps happen before final archival. The script remains a narrow execution surface that exposes three explicit operations:

- `check-preconditions`
- `finalize`
- `archive-status`

This separation matters because it keeps the irreversible business workflow readable while leaving archive mutations in a deterministic, testable script.

## Design Decisions (ADRs)

### ADR 1 - Keep `complete` as a Dedicated Closure Skill

**Decision:** Preserve `bmad-lens-complete` as its own retained command owner.

**Rationale:** The baseline traceability matrix treats `complete` as a first-class retained command with its own sequence and regression obligations. Folding it into `dev` or `finalizeplan` would erase a published lifecycle boundary and blur when archival becomes irreversible.

**Alternatives Rejected:**

- Absorb archival into `dev`: rejected because development and closure have different preconditions, risks, and user expectations.
- Absorb archival into `finalizeplan`: rejected because finalizeplan closes planning, not delivered feature work.

### ADR 2 - Separate Orchestration from Archive Mutation

**Decision:** Keep confirmation, retrospective triggering, and document-project delegation in the skill, while `complete-ops.py` owns precondition evaluation and archive writes.

**Rationale:** The current script is intentionally small and testable. The skill is where conversational confirmation and dependency orchestration belong. Preserving that split gives the rewrite a clean-room implementation surface without overloading either layer.

**Alternatives Rejected:**

- Move retrospective and document-project into the script: rejected because those are workflow delegations, not atomic archive mutations.
- Move archive writes into the skill: rejected because it weakens direct CLI testability and duplicates file-write logic.

### ADR 3 - Canonicalize the Archive Summary as `summary.md`

**Decision:** Treat `summary.md` as the canonical archive summary filename.

**Rationale:** The current script writes `summary.md`, the tests assert `summary.md`, and existing feature folders already use `summary.md`. Some older discovery prose mentions `final-summary.md`, but that name does not match the current executable behavior. Preserving `summary.md` avoids drift between the new skill, the governance archive, and test expectations.

**Alternatives Rejected:**

- Introduce `final-summary.md`: rejected because it changes observable outputs and would require downstream reader changes.
- Write both files: rejected because it creates avoidable duplication and uncertainty about the source of truth.

### ADR 4 - Preserve Three Explicit Script Operations

**Decision:** Keep `check-preconditions`, `finalize`, and `archive-status` as separate subcommands.

**Rationale:** The three-command split maps directly to user needs: determine readiness, preview or execute archive, and query terminal state. It also matches current tests and module-help command discovery.

**Alternatives Rejected:**

- Collapse into one `complete` subcommand with flags: rejected because it hides the archive-status surface and makes testing less clear.
- Remove `archive-status`: rejected because other readers and operators benefit from a read-only terminal-state query.

### ADR 5 - Preserve Warning-vs-Blocker Semantics in Preconditions

**Decision:** Missing retrospective remains a warning, while invalid phase or missing feature remains a failure.

**Rationale:** Current behavior allows an explicit retrospective skip to be acknowledged without rewriting the lifecycle model. That keeps the workflow strict but practical.

**Alternatives Rejected:**

- Make missing retrospective an unconditional hard failure: rejected because it removes the explicit skip path already present in the skill references.
- Downgrade invalid phase to a warning: rejected because archiving from planning states would break lifecycle semantics.

## API Contracts

### Release Prompt Contract

The retained prompt remains a thin stub that runs light preflight and loads the complete skill. It should keep the published command stable without embedding archive logic directly in prompt prose.

Expected path:

```text
.github/prompts/lens-complete.prompt.md
  -> lens.core/_bmad/lens-work/prompts/lens-complete.prompt.md
  -> lens.core/_bmad/lens-work/skills/bmad-lens-complete/SKILL.md
```

Breaking change: false.

### Skill Contract

The skill should continue to support these user-visible operations and responsibilities:

1. Resolve governance repo and feature context.
2. Run `check-preconditions` before irreversible execution.
3. Trigger or confirm retrospective handling.
4. Delegate final project documentation to `bmad-lens-document-project`.
5. Require explicit confirmation before finalize.
6. Run finalize and report archive outputs clearly.

Breaking change: false.

### Script CLI Contract

Retain this CLI shape:

```bash
uv run _bmad/lens-work/skills/bmad-lens-complete/scripts/complete-ops.py check-preconditions \
  --governance-repo "{governance_repo}" \
  --feature-id "{featureId}" \
  --domain "{domain}" \
  --service "{service}"

uv run _bmad/lens-work/skills/bmad-lens-complete/scripts/complete-ops.py finalize \
  --governance-repo "{governance_repo}" \
  --feature-id "{featureId}" \
  --domain "{domain}" \
  --service "{service}" \
  [--dry-run]

uv run _bmad/lens-work/skills/bmad-lens-complete/scripts/complete-ops.py archive-status \
  --governance-repo "{governance_repo}" \
  --feature-id "{featureId}"
```

Expected result semantics:

- `check-preconditions`: JSON with `status`, `phase`, `retrospective_exists`, `issues`, and `blockers`
- `finalize`: JSON with `status`, `feature_id`, `archived_at`, `feature_yaml_path`, and `index_updated`; dry-run includes planned changes without writes
- `archive-status`: JSON with `status`, `archived`, `phase`, and `completed_at`

Breaking change: false.

## Data Model Changes

No new lifecycle or governance schemas are required.

The existing archive mutation model remains:

```yaml
feature.yaml:
  phase: complete
  completed_at: {ISO timestamp}

feature-index.yaml entry:
  status: archived
  updated_at: {ISO timestamp}
```

The archive summary remains a Markdown document at `{feature-dir}/summary.md`.

## Dependencies

- `pyyaml` for YAML read and write operations in `complete-ops.py`
- `bmad-lens-retrospective` for pre-archive retrospective capture
- `bmad-lens-document-project` for final project documentation generation and mirroring
- Existing feature directory and `feature-index.yaml` governance structures
- Light preflight and standard Lens prompt bootstrap behavior

## Rollout Strategy

Deliver the feature in focused stages:

1. Preserve or restore the retained prompt and skill metadata.
2. Implement or refine `complete-ops.py` to match the retained contract.
3. Ensure the skill references and confirmation flow align with the script behavior.
4. Verify retrospective and document-project delegation points.
5. Run focused `complete-ops` regressions.
6. Run a narrow command-surface check to confirm `/complete` remains available.

Rollback is straightforward because no schema changes are introduced. Reverting the prompt, skill, script, and focused tests removes the feature without migration work.

## Testing Strategy

Focused regression coverage should prove:

- `check-preconditions` passes for `dev` and `complete` phases when retrospective exists
- `check-preconditions` fails for planning phases such as `preplan`
- missing retrospective returns `warn`, not `fail`
- `finalize` updates `feature.yaml` to `phase: complete`
- `finalize` updates the matching `feature-index.yaml` entry to `status: archived`
- `finalize --dry-run` reports changes without writing files
- `finalize` writes `summary.md`
- `archive-status` reports terminal and in-progress states correctly
- not-found cases fail cleanly with actionable errors

Likely verification commands from the source tree root:

```bash
uv run --script _bmad/lens-work/skills/bmad-lens-complete/scripts/tests/test-complete-ops.py
uv run _bmad/lens-work/skills/bmad-lens-complete/scripts/complete-ops.py finalize --help
```

If wrapper-level tests do not yet exist, add a narrow regression that proves the orchestration order remains retrospective -> document-project -> finalize, with user confirmation required before irreversible execution.

## Observability

The command is operationally observable through deterministic JSON output and the final archive artifacts. The key signals are:

- clear precondition blockers
- explicit warning on skipped retrospective paths
- dry-run change previews before irreversible execution
- archive-state query via `archive-status`

No new telemetry is required. Stable CLI output and focused regression coverage are the primary observability mechanisms for this feature.

## Clean-Room Implementation Notes

This feature should be implemented from the behavioral contract above, using old prompt and discovery materials only as evidence of expected outputs and sequencing. Source code, tests, and prose in the new codebase should be newly written and aligned to the current release-module boundaries rather than copied from the old implementation.

## ExpressPlan Compatibility Note

This feature remains registered as `track: full` and `phase: preplan`. These artifacts are staged under the control-repo docs path for planning purposes and follow the ExpressPlan two-document/frontmatter contract requested for this task, but they do not by themselves change feature lifecycle state or auto-advance the feature through the express track.