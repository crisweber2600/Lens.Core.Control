---
feature: lens-dev-new-codebase-trueup
doc_type: parity-audit-report
status: accepted
goal: Consolidated parity audit for the five non-preplan new-codebase command features
scan_date: 2026-04-30
depends_on:
  - adr-complete-prerequisite.md
  - adr-constitution-tracks.md
  - parity-gate-spec.md
blocks: []
updated_at: 2026-04-30T00:00:00Z
---

# Parity Audit Report

## Executive Verdict

| Feature | Verdict | Classification | Rationale |
| --- | --- | --- | --- |
| `lens-dev-new-codebase-switch` | `pass-with-gap-closed` | `structural-gap` | Runtime script and skill are present. The prompt publication gap is structural and has a source prompt plus target IDE adapter in the current source tree; no behavioral regression found. |
| `lens-dev-new-codebase-new-domain` | `pass-with-reviewed-decision` | `reviewed-decision` | `create-domain` exists. Track-list divergence is intentional per ADR-2. |
| `lens-dev-new-codebase-new-service` | `pass-with-reviewed-decision` | `reviewed-decision` | `create-service` exists. Auto-domain marker behavior is an additive output, not a regression. |
| `lens-dev-new-codebase-new-feature` | `regression` | `behavioral-regression` | `create`, `fetch-context`, and `read-context` are present in the old-codebase contract and absent from the checked new-codebase source script. |
| `lens-dev-new-codebase-complete` | `partial-after-trueup` | `behavioral-regression` | Baseline new-codebase had only `references/finalize-feature.md`. True Up has now added `SKILL.md` and test stubs, but `complete-ops.py` remains deferred to the complete feature dev phase. |

## Scope

This audit covers five features only:

- `switch`
- `new-domain`
- `new-service`
- `new-feature`
- `complete`

Evidence was gathered from the new-codebase target repo at `TargetProjects/lens-dev/new-codebase/lens.core.src`, the old-codebase reference repo at `TargetProjects/lens-dev/old-codebase/lens.core.src`, and governance records under `TargetProjects/lens/lens-governance`.

## SAFE_ID_PATTERN Scan (FR-9)

Scan date: 2026-04-30.

Scan scope:

- `TargetProjects/lens/lens-governance/feature-index.yaml`
- All `feature.yaml` files under `TargetProjects/lens/lens-governance/features/`

Files scanned: 22 total (1 index file plus 21 `feature.yaml` files).

Scan rule: flag any `featureId` or `featureSlug` value containing a dot or underscore.

Result: pass. No scanned `featureId` or `featureSlug` values contain dots or underscores.

Classification: `reviewed-decision`. ADR-4 in `architecture.md` remains valid: the tightened safe-id rule is compatible with current governance data.

## Python 3.12 Decision (FR-8)

The new-codebase `init-feature-ops.py` declares Python 3.12 as the runtime baseline. This is classified as an intentional design decision, not a parity gap.

Rationale:

- The new-codebase scripts use modern standard-library assumptions such as `tomllib` availability.
- The implementation style assumes modern Python syntax and typing.
- A single Python 3.12 baseline reduces migration ambiguity for future Lens Workbench scripts.

Classification: `reviewed-decision`. See ADR-3 in `architecture.md` and the migration standards in `parity-gate-spec.md`.

## Feature Findings

### lens-dev-new-codebase-switch

Verdict: `pass-with-gap-closed`.

Classification: `structural-gap`.

Evidence:

- New-codebase source skill exists at `_bmad/lens-work/skills/bmad-lens-switch/SKILL.md`.
- New-codebase runtime script exists at `_bmad/lens-work/skills/bmad-lens-switch/scripts/switch-ops.py`.
- Source prompt exists at `_bmad/lens-work/prompts/lens-switch.prompt.md`.
- Target IDE adapter prompt exists at `.github/prompts/lens-switch.prompt.md` in the target repo and delegates through the preflight gate.
- `switch-ops.py` exposes a compatibility `context_to_load` shape with `summaries` and `full_docs` while also returning richer `context_paths`.

Specific gap:

- Prompt publishing was the original structural gap. In the current target source tree, both source prompt and target IDE adapter are present.

Rationale:

No behavioral regression was found for the audited switch surface. The known context path shape change is mitigated by the `context_to_load` compatibility adapter.

### lens-dev-new-codebase-new-domain

Verdict: `pass-with-reviewed-decision`.

Classification: `reviewed-decision`.

Evidence:

- New-codebase `init-feature-ops.py` exposes `create-domain`.
- `create-domain` creates `domain.yaml`, a domain constitution, optional TargetProjects scaffold, optional docs scaffold, and personal context when configured.
- Generated constitutions include `quickplan`, `full`, `hotfix`, `tech-change`, `express`, and `expressplan`.

Specific divergence:

- The track list is broader than older templates.

Rationale:

The broader track list is intentional and canonical per ADR-2. It is not a parity gap.

### lens-dev-new-codebase-new-service

Verdict: `pass-with-reviewed-decision`.

Classification: `reviewed-decision`.

Evidence:

- New-codebase `init-feature-ops.py` exposes `create-service`.
- `create-service` creates service markers and service constitution files and can create missing domain containers before service creation.
- The output contract includes additive scaffold and marker fields for workspace follow-up.

Specific divergence:

- `created_domain_marker` style output is additive compared with older behavior.
- The canonical constitution track list includes express planning tracks.

Rationale:

The additive output does not remove old behavior. Constitution track divergence is resolved by ADR-2.

### lens-dev-new-codebase-new-feature

Verdict: `regression`.

Classification: `behavioral-regression`.

Evidence:

- Old-codebase `bmad-lens-init-feature` includes the feature creation and context-loading command surface.
- New-codebase `init-feature-ops.py` currently exposes `create-domain` and `create-service` only.

Specific regressions:

- `create` subcommand absent from `init-feature-ops.py`.
- `fetch-context` subcommand/function absent from `init-feature-ops.py`.
- `read-context` subcommand/function absent from `init-feature-ops.py`.

True Up remediation status:

- `lens-new-feature.prompt.md` has been added as a source prompt controller.
- `references/auto-context-pull.md` has been added to document the expected context-loading contract.
- Runtime restoration for `create`, `fetch-context`, and `read-context` remains owned by `lens-dev-new-codebase-new-feature` dev work.

Governance recommendation:

Record a blocker annotation on `lens-dev-new-codebase-new-feature` until the context-loading contract is restored or explicitly deferred. TU-5.1 owns that governance write.

### lens-dev-new-codebase-complete

Verdict: `partial-after-trueup`.

Classification: `behavioral-regression`.

Evidence:

- Old-codebase `bmad-lens-complete` includes `complete-ops.py` with `check-preconditions`, `finalize`, and `archive-status`.
- Baseline new-codebase contained only `references/finalize-feature.md` under `bmad-lens-complete`.

Specific baseline regressions:

- `bmad-lens-complete/SKILL.md` absent.
- `complete-ops.py` absent.
- `scripts/tests/test-complete-ops.py` absent.
- Test fixture scaffold absent.

True Up remediation status:

- `bmad-lens-complete/SKILL.md` has been authored with command contracts for `check-preconditions`, `finalize`, and `archive-status`.
- `scripts/tests/test-complete-ops.py` and `scripts/tests/conftest.py` have been scaffolded.
- `complete-ops.py` remains intentionally out of scope and belongs to the `lens-dev-new-codebase-complete` dev phase.

Governance recommendation:

Record a blocker annotation on `lens-dev-new-codebase-complete` until runtime implementation work begins or the remaining script gap is explicitly tracked. TU-5.1 owns that governance write.

## Cross-Cutting Findings

| Finding | Classification | Resolution |
| --- | --- | --- |
| Python 3.12 baseline | `reviewed-decision` | Documented in architecture ADR-3 and this report. |
| SAFE_ID_PATTERN tightening | `reviewed-decision` | Narrowed governance YAML scan passed on 2026-04-30. |
| Constitution track list includes `express` and `expressplan` | `reviewed-decision` | ADR-2 makes new-codebase template canonical. |
| `switch` context path shape change | `reviewed-decision` | `context_to_load` compatibility adapter is present. |
| `.github/prompts/` adapter ownership | `authority-boundary` | Dev work updates source prompts; adapter mirroring is human/release-owned unless explicitly authorized. |

## Timing Note (CF-5)

This report does not write blocker annotations to governance. That timing gap is expected: audit evidence lands first, stakeholders can review it, and TU-5.1 performs the explicit governance companion writes afterward.

## Review Window

Stakeholders for `new-feature` and `complete` should review the regression findings before TU-5.1 writes governance-visible blocker annotations. The findings above are the decision record for those annotations.
