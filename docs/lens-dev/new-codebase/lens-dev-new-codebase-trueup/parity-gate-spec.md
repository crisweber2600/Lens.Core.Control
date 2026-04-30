---
feature: lens-dev-new-codebase-trueup
doc_type: parity-gate-spec
status: accepted
goal: Define the reusable fully migrated gate for retained Lens command migrations
depends_on:
  - architecture.md
  - adr-constitution-tracks.md
blocks: []
updated_at: 2026-04-30T00:00:00Z
---

# Parity Gate Specification

## Purpose

This specification defines what `fully migrated` means for a retained Lens command in the new-codebase source. It is a reusable gate for future command migrations and for reviewing partial migration work.

A command is fully migrated only when all three layers pass: prompt stub, skill contract, and script plus tests.

## Gate Verdicts

| Verdict | Meaning |
| --- | --- |
| `fully-migrated` | All three layers pass and required design standards are satisfied. |
| `partial` | At least one layer is complete, but one or more required layers remain tracked work. |
| `gap` | A required structural layer is absent and no tracking story exists. |
| `regression` | A behavioral capability present in the old-codebase command is absent or incompatible in the new-codebase command. |

## Layer 1: Prompt Stub

Layer 1 is the command entry controller.

Required checks:

- `_bmad/lens-work/prompts/lens-{command}.prompt.md` exists.
- The prompt has valid frontmatter and a clear command title.
- The prompt runs `uv run ./lens.core/_bmad/lens-work/scripts/light-preflight.py` before reading config or executing command scripts.
- The prompt delegates to the backing `SKILL.md` or script and does not implement business logic inline.
- The prompt declares missing runtime implementations as `not_yet_implemented` rather than guessing behavior.
- `.github/prompts/` mirroring is treated as an adapter publication step. If the current feature scope excludes `.github/`, the mirror must be tracked as a human-owned or release-owned follow-up rather than silently omitted.

Layer 1 fails when the prompt is absent, bypasses preflight, writes governance directly, or embeds behavior that belongs in a skill or script.

## Layer 2: SKILL.md

Layer 2 is the agent-facing command contract.

Required checks:

- `_bmad/lens-work/skills/{skill-name}/SKILL.md` exists.
- The skill was authored or materially revised through the BMB implementation channel when it changes a Lens Workbench skill.
- Frontmatter contains `name` and a conservative `description` trigger.
- The Overview states what the skill does, how it works, and the outcome it protects.
- Command contracts document inputs, guards, return shape, dry-run behavior when applicable, and error handling.
- Cross-skill prerequisites and graceful-degradation rules are explicit.
- The skill references any relevant `references/` documents without nesting multi-hop instructions.

Layer 2 fails when the file is a placeholder, omits return contracts, hides irreversible operations, or asks the agent to patch authority-domain files directly.

## Layer 3: Script and Tests

Layer 3 is the deterministic runtime implementation.

Required checks:

- `_bmad/lens-work/skills/{skill-name}/scripts/{command}-ops.py` exists for commands with runtime behavior.
- The script supports `--help` and returns structured JSON for agent consumption.
- The script validates path-constructing inputs before writing files.
- Dry-run mode exists where the old-codebase command supported preview behavior or where writes are irreversible.
- `scripts/tests/conftest.py` exists when fixtures are shared.
- `scripts/tests/test-{command}-ops.py` exists with at least six scenario tests or stubs during red-phase development.
- Tests cover pass, warning, failure, dry-run, write, and read/status paths where applicable.

Layer 3 fails when a script is absent for implemented behavior, writes without dry-run for irreversible operations, returns prose-only output, or lacks test coverage for known edge cases.

## Migration Standards

Layer 3 scripts must follow the Python version decision from ADR-3 in `architecture.md`: Python 3.12 is the reviewed new-codebase baseline for Lens Workbench scripts. New scripts should not quietly lower or raise the runtime requirement without an explicit ADR.

Path-constructing identifiers must follow ADR-4 in `architecture.md`: future feature IDs and slugs should be hyphen-normalized and compatible with the tightened safe-id rule. Any command that accepts feature IDs must validate inputs before filesystem or git operations.

The ADR-2 constitution-track decision also applies to generated governance containers: `express` and `expressplan` are canonical permitted tracks for Lens Dev new-codebase constitutions.

## How to Apply

Evaluate retained command migrations in order: Layer 1, then Layer 2, then Layer 3. A command with a valid prompt and skill but no runtime script is `partial`, not fully migrated. A partial migration may be acceptable only when the missing layer has an explicit story, owner, and blocker or deferral note.

Use the old-codebase command as the behavioral reference and the new-codebase architecture docs as the design reference. If the new implementation intentionally differs from old behavior, record that as an ADR-backed reviewed decision. If it differs without a decision, classify it as a parity finding and decide whether it is a gap or a regression.

Do not let adapter publication hide source readiness. A command can have a valid source prompt but still need a human-owned `.github/prompts/` mirror after merge. Conversely, an IDE adapter prompt without a backing source prompt or skill contract does not count as fully migrated.

## Post-Merge Governance Action

After this feature lands, add a reference to this document from `TargetProjects/lens/lens-governance/constitutions/lens-dev/new-codebase/constitution.md` under a `Migration Standards` section. That is a governance commit and is intentionally deferred from this source/control artifact commit.
