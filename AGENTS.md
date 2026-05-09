# AGENTS.md - Lens Core Source Enforcement
## MUST HAVE RULES:
- Use `vscode_askQuestions` #askquestions for all follow-up questions instead of freeform chat prompts.
- Always run `bmad-lens-postflight` immediately after every Lens command execution before ending the session.
## Purpose

This file defines non-negotiable operating constructs required for Lens and BMAD skills to work reliably in a control-repo lifecycle model.

Scope covered by this policy:
- Control-repo planning orchestration
- Governance publication boundaries
- Feature lifecycle gates
- Artifact naming contracts
- Delegation boundaries between conductors and BMAD skills

## Repository Roles and Source of Truth

- Control repo is the planning workspace and branch topology owner.
- Governance repo is the metadata and published-artifact authority on main only.
- Target repos are implementation-only surfaces for code and tests.
- Release clone surfaces are read-only in local control workspaces.
- All code changes must be made in `TargetProjects/`; do not place implementation edits in the control repo root or release clone surfaces.
- All output documents and planning artifacts must be written under `docs/`.

Canonical authorities:
- Feature lifecycle state: feature.yaml in governance repo.
- Planning staging path: docs path resolved from feature.yaml.docs.path.
- Governance mirror updates: publish-to-governance flow only.

## Write Scope Boundaries

### Allowed writes by class

- Planning conductors: write only to resolved control-repo docs path.
- Dev conductors and implementation delegates: write only to resolved target repo path.
- Governance updates: only through approved orchestration boundaries.

### Prohibited writes

The following are hard prohibited unless explicitly required by a dedicated governance operation:
- Direct file patching under governance repo feature folders.
- Direct authoring under governance repo docs mirror paths.
- Direct writes to release clone paths in local control workspaces.
- Direct writes to generated control-root .github artifacts when those artifacts are setup-generated mirrors.

Hard stop behavior:
- On write-scope violation risk, stop before mutation.
- Do not auto-reroute to a guessed path.
- Report required approved path or operation.

## Governance Boundary Contract

Governance artifacts must be updated through approved boundaries only:
- publish-to-governance workflow
- lens-git-orchestration governed operations
- lens-feature-yaml lifecycle metadata operations

Rules:
- Governance remains on main as the authoritative state.
- No feature-branch governance topology is allowed.
- No hand-copy publication is allowed.

Hard stop behavior:
- If publication cannot proceed via approved boundary, stop with blocking reason.
- Never suggest direct manual file patching in governance as a fallback.

## Feature Context Resolution Contract

Before reading or writing phase artifacts, resolve context in this order:
1. Explicit feature input
2. Session feature context
3. feature.yaml authority

Required resolved values:
- domain
- service
- feature_id
- track
- phase
- docs.path
- target_repos for implementation operations

Rules:
- Never infer feature identity from open files, cwd, or branch name.
- Never infer phase from branch topology or artifact presence.
- Never silently apply track defaults.

Hard stop behavior:
- Missing required context for requested operation is blocking.
- Do not continue with placeholder or guessed values.

## Phase Gate Contract

Phase progression requires explicit gate success. Review failure blocks advancement.

Rules:
- A failed adversarial review blocks all downstream phase actions.
- On failed review: no phase update, no PR creation, no publish, no continuation.
- Phase transitions must follow lifecycle ordering for the active track.

Express track progression:
- expressplan -> finalizeplan -> dev -> complete

Full track progression:
- preplan -> businessplan -> techplan -> finalizeplan -> dev -> complete

Hard stop behavior:
- If predecessor gate is incomplete or invalid, stop with expected predecessor state and artifact contract.

## Artifact Naming and Metadata Contract

Canonical filenames are strict and required.

### Review artifacts

- preplan-adversarial-review.md
- businessplan-adversarial-review.md
- techplan-adversarial-review.md
- expressplan-adversarial-review.md
- finalizeplan-review.md

Strict naming rule:
- Legacy aliases are not accepted as compliant outputs.
- Do not create, recommend, or validate legacy alias filenames.

### Finalize bundle core outputs

- epics.md
- stories.md
- implementation-readiness.md
- sprint-status.yaml
- story files

### Story file frontmatter minimum

Each story file must include:
- feature
- story_id
- doc_type set to story
- status
- title
- depends_on
- updated_at

### Sprint status format

For new outputs, sprint-status.yaml must be a single YAML document.

Hard stop behavior:
- Missing required artifact names or required frontmatter fields blocks phase completion.

## Track-Aware Input Contract

Downstream BMAD generation must use track-approved input artifacts from lifecycle validation.

Rules:
- Use approved input document list supplied by lifecycle gate.
- Do not block on generic BMAD file expectations that are not required by active track.

Express-track required planning inputs for FinalizePlan downstream generation:
- business-plan.md
- tech-plan.md
- sprint-plan.md

Hard stop behavior:
- If lifecycle input-ready validation fails, do not invoke downstream bundle generation.

## Delegation Contract

Conductors orchestrate. Delegates author.

Rules:
- Conductors must not synthesize delegated artifacts inline.
- BMAD wrapper resolves context, enforces write scope, delegates, then stops.
- Wrapper does not continue conductor menus or discovery after handoff.

Hard stop behavior:
- If delegate skill is unavailable or unregistered, stop with actionable resolution path.

## Batch Mode Contract

Batch flows follow strict two-pass behavior.

Pass 1:
- Generate batch intake artifact only.
- Do not publish, do not phase-transition, do not generate lifecycle outputs.

Pass 2:
- Resume owning conductor with approved batch answers.
- Only ask genuinely unresolved questions.

Hard stop behavior:
- Placeholder or incomplete required answer blocks are blocking.

## Dev Readiness and Implementation Contract

Dev operations require explicit readiness.

Rules:
- Allowed entry phases for quick dev must be explicitly dev-ready per lifecycle policy.
- target_repos must be resolvable from feature metadata before implementation writes.
- If control-repo dev branch activation is required by conductor, branch gate must pass before reading sprint or story state.

Hard stop behavior:
- Missing target repo mapping or failed dev branch activation blocks implementation flow.

## Operational Guardrails

- Prefer durable repo scripts over ad hoc one-off shell parsing for lifecycle state.
- Do not bypass validators for phase or metadata contracts.
- Do not use destructive git operations unless explicitly requested and approved.
- Keep governance operations auditable and boundary-preserving.

## Quick Compliance Checklist

Before any phase action:
- Context resolved from authoritative sources
- Operation write scope resolved and logged
- Predecessor phase gate passed
- Required artifact contract known for active track

Before any publication:
- Review gate passed
- Publication route is approved orchestration boundary
- No direct governance patching attempted

Before phase completion:
- Required artifacts present with strict names
- Required metadata validated
- Lifecycle state update performed through approved feature metadata operation

## Enforcement Priority

When rules conflict, apply this precedence:
1. Write boundary safety
2. Governance immutability and publication boundary
3. Lifecycle phase gate integrity
4. Canonical artifact and metadata contracts
5. Delegation boundary correctness

If unresolved after precedence resolution, stop and escalate with explicit blocker details.
