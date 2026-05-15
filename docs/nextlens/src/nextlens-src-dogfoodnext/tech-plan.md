---
feature: nextlens-src-dogfoodnext
doc_type: tech-plan
track: express
updated_at: 2026-05-15
inputDocuments:
  - docs/nextlens/src/nextlens-src-topdownlens/guides/bugfix-flow.md
  - docs/nextlens/src/nextlens-src-topdownlens/examples/bugfix-example.md
  - lens.core/_bmad/lens-work/skills/bmad-lens-core-bugfix/SKILL.md
  - lens.core/_bmad/lens-work/scripts/bug-reporter-ops.py
  - lens.core/_bmad/lens-work/scripts/bugbash_schema.py
  - lens.core/_bmad/lens-work/scripts/bugbash_scope_guard.py
---

# Tech Plan

## Planning Boundary

This document defines the implementation contract for a future Lens-owned NextLens bugfix skill. It does not implement code, modify registration, or write to the target repo.

There are two distinct boundaries:

- Skill source boundary: the new orchestration skill is authored in `lens.core.src`.
- Runtime fix boundary: when the skill performs or delegates a NextLens fix, implementation edits are allowed only under `TargetProjects/nextlens/src/NextLens`.

Governance repositories, governance docs mirrors, release clone surfaces, and unrelated control-root files remain out of scope for runtime implementation writes.

## Lens Core Bugfix Flow Analysis

The NextLens design should reuse the mechanics of `/lens-core-bugfix`, not only its name:

1. **Structured intake first.** The core flow requires title, description, repro steps, expected behavior, and actual behavior. It calls `bug-reporter-ops.py create-bug` with `--source lens-core-bugfix`, `--queue QuickDev`, and a chat-log marker so the bug artifact has durable provenance.
2. **Stable bug identity.** `bug-reporter-ops.py` derives `slug = {title-slug}-{sha256(title+description)[:8]}` and checks all status folders for duplicates. The slug becomes the source of branch identity.
3. **Fresh branch rule.** The conductor derives `feature_id = lens-core-bugfix-{bug_slug}` and creates a fresh target branch from the base branch through `git-orchestration-ops.py prepare-dev-branch`. It blocks branch reuse, branch scope mismatches, dirty target worktrees, and no-op completion.
4. **Delegation is bounded.** The conductor delegates implementation to `bmad-quick-dev`, but the delegate stops after implementation and local commit. The conductor owns push, PR creation or reuse, PR recording, bug closeout, and final output verification.
5. **Closeout is stateful.** The conductor records the PR URL through `record-quickdev-pr`, then closes the artifact through `close-quickdev-bug`. A successful run requires a non-empty working branch, commit hash, PR URL, and final bug artifact path under `bugs/Fixed/`.
6. **Operational bug artifacts are special.** Existing bugbash docs treat `bugs/` as operational state written by approved bug scripts, while feature docs mirrors under `features/` still require publication boundaries.

For NextLens, the adapted flow should preserve these mechanics with different configuration: source skill in `lens.core.src`, bug namespace `nextlens`, target repo `TargetProjects/nextlens/src/NextLens`, branch prefix such as `feature/nextlens-bugfix-{bug_slug}`, and design context from `docs/nextlens/src`.

## NextLens Bug Report Namespace

The bug report created by this skill must be stored in a NextLens-specific folder. The planned artifact layout is:

```text
bugs/nextlens/New/{slug}.md
bugs/nextlens/QuickDev/{slug}.md
bugs/nextlens/Inprogress/{slug}.md
bugs/nextlens/Fixed/{slug}.md
```

The status folders should mirror the existing bugbash state model (`New`, `QuickDev`, `Inprogress`, `Fixed`), but the `nextlens` namespace prevents NextLens defects from mixing with Lens core bug reports. The implementation may extend the existing bug reporter operation with a namespace argument or introduce a NextLens-specific wrapper, but it must retain slug idempotency, schema validation, scope guarding, PR recording, and closeout behavior.

## Expected Capability Additions

- Add a dedicated Lens skill such as `lens-nextlens-bugfix` or equivalent under the `lens.core.src` skill surface.
- Expose an operator-facing prompt or command alias according to Lens skill and prompt conventions.
- Update Lens skill registry, help, and release-sync metadata so the capability can be discovered from the Lens module.
- Add source-owned helpers in `lens.core.src` for chat-history intake, namespaced bug report creation, design-context loading, fix-spec generation, runtime boundary validation, PR recording, and closeout.
- Extend Lens validation checks so missing registration, missing helpers, invalid intake, inaccessible docs context, namespaced bug folder misconfiguration, or target-boundary misconfiguration are reported clearly.

## Intake Contract

The bugfix skill should require these inputs:

- `what_happened`: concise description of the observed behavior.
- `what_should_have_happened`: concise description of the expected behavior.
- `chat_history`: evidence text, transcript excerpt, or path to an approved evidence artifact.

Optional fields may include severity, originating Salmon signal ID, evidence references, suspected surface, requested validation, and operator notes. The parser should normalize these inputs into a structured intake object before any implementation handoff.

The normalized intake should map cleanly to the bug-reporting model: title, description, repro or observed transcript summary, expected behavior, actual behavior, chat evidence reference, source `nextlens-bugfix`, queue `QuickDev`, and namespace `nextlens`.

Before durable artifact creation, the intake layer must minimize transcript persistence: summarize noisy chat history, preserve approved evidence references, and reject or redact obvious secrets or credentials when feasible. Raw chat should not be copied into bug artifacts unless the operator supplied an approved evidence artifact intended for durable retention.

## Design Context Loader

The context loader should read from `docs/nextlens/src` and select relevant NextLens design guidance, with the TopDownLens bugfix flow as the initial pattern reference. It should treat governance and release surfaces as read-only unless an approved Lens operation handles publication or promotion outside this skill.

The loader should return a compact context bundle with source paths, extracted constraints, known workflow patterns, and any conflicts or missing context that require operator resolution.

Path resolution must be explicit and portable. The skill should resolve the control repo root, `docs/nextlens/src`, and `TargetProjects/nextlens/src/NextLens` from Lens configuration or feature metadata, support operator overrides only when validated inside the approved roots, and fail with actionable diagnostics when invoked from an unexpected current working directory.

## Fix Specification Contract

The fix-spec generator should produce an implementation-ready artifact or in-memory handoff containing:

- Feature ID and bugfix title.
- Namespaced bug artifact path under `bugs/nextlens/{status}/{slug}.md`.
- Actual behavior, expected behavior, and summarized evidence.
- Relevant design-context references.
- Suspected target files or capability surfaces when known.
- Explicit skill source root: `lens.core.src`.
- Explicit runtime allowed write root: `TargetProjects/nextlens/src/NextLens`.
- Explicit prohibited write roots: governance repos, governance docs mirrors, release clones, and unrelated control repo paths.
- Implementation approach, validation commands, and evidence expectations.
- Salmon linkage and closure notes when an originating signal exists.

## Salmon Integration

If a Salmon signal is present, the skill should preserve signal ID, severity, recommended action, and evidence references in the fix specification. High or blocking signals should follow the bugfix pattern: capture evidence, prepare a dedicated correction path, verify the target repo change, record PR evidence, and close or supersede the originating signal only through the approved route.

If no Salmon signal is present, the skill should still produce evidence fields that can later be attached to a signal or review artifact.

Doctor owns validation health checks. The bugfix flow should invoke or reference NextLens Doctor validation outputs rather than reimplementing Doctor logic, and closure evidence should identify whether Doctor validation passed, was not applicable, or is intentionally deferred with rationale.

## Delegation And Write Boundary

The bugfix skill should prepare implementation delegation, not perform broad discovery or unrestricted edits. Before delegation it must confirm:

- Target repo path resolves to `TargetProjects/nextlens/src/NextLens`.
- The proposed files are inside the allowed target root.
- The fix spec includes validation and evidence requirements.
- Dev work has an implementation story when lifecycle gates require one.
- The Lens skill source root is not treated as the NextLens fix target except while implementing this feature's own skill code.

The conductor should then mirror the core bugfix completion gate: prepare a fresh target branch from the configured base branch, delegate implementation, require a real implementation commit, push the branch, create or reuse a PR, record the PR URL on the namespaced bug artifact, close the bug artifact only after validation evidence exists, and block success if any required output is missing.

Any attempt to write outside the target root should stop the workflow with a boundary violation.

## Validation Strategy

- Unit tests for the intake parser with complete, missing, and noisy chat-history inputs.
- Unit tests for fix-spec generation and required field enforcement.
- Unit tests for namespaced bug artifact creation, duplicate detection, PR recording, and closeout transitions.
- Boundary tests proving prohibited paths are rejected.
- Integration-style fixture covering chat history plus docs context producing a deterministic bug artifact, fresh branch identity, and fix spec.
- Lens validation that checks skill registration, prompt/help metadata, helper availability, docs context access, target repo resolution, and schema health.

## Technical Risks

- Input transcripts can be large; scripts should summarize or reference durable evidence without storing excessive raw chat.
- Context loading can become too broad; selection should prefer explicit feature docs and known NextLens guidance.
- Boundary enforcement must be tested at path-normalization edges, especially on Windows paths.
- Boundary enforcement must cover traversal segments, path casing, resolved symlink or junction escapes where supported, and both Windows and POSIX separators.
- Lens prompt, skill, and help metadata must stay synchronized with the skill entrypoint.
- The namespace addition changes the existing one-level `bugs/{status}` lookup assumption; FinalizePlan must call this out so implementation does not accidentally close or duplicate the wrong bug artifact.