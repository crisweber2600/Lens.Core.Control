# lens-rollback Prompt Audit

## Purpose Summary
- .github/prompts/lens-rollback.prompt.md is a control-layer stub. It requires the shared lightweight preflight command and then delegates execution to the release prompt in lens.core.
- lens.core/_bmad/lens-work/prompts/lens-rollback.prompt.md is also a stub and delegates directly to the rollback skill contract.
- Effective behavior therefore lives in lens.core/_bmad/lens-work/skills/bmad-lens-rollback/SKILL.md: validate rollback eligibility, compute valid backward targets, require explicit confirmation, update state with audit history, and report next-step guidance.
- The rollback skill is explicitly non-destructive in intent: no branch, artifact, or PR deletion.

## BMAD Skill Mapping
- Global BMAD registry surfaces do not expose rollback as a canonical BMAD skill:
  - lens.core/_bmad/_config/skill-manifest.csv includes no bmad-lens-rollback entry.
  - lens.core/_bmad/_config/bmad-help.csv includes no rollback command/topic row.
- Rollback is Lens-local and is registered in Lens module metadata:
  - lens.core/_bmad/lens-work/module.yaml registers bmad-lens-rollback with description Safe phase rollback with confirmation gates and audit trail.
  - lens.core/_bmad/lens-work/module.yaml includes lens-rollback.prompt.md in both prompt distribution and GitHub adapter stub prompts.
  - lens.core/_bmad/lens-work/module-help.csv exposes command RB with skill bmad-lens-rollback, action rollback, phase anytime, and outputs rollback results.
- Integration declared by the rollback skill is through Lens-local services, not global BMAD workflows:
  - bmad-lens-git-state for current initiative/phase resolution.
  - bmad-lens-git-orchestration for open-PR checks and commit operations.

## Lifecycle Fit
- Lifecycle alignment is partial:
  - lifecycle.yaml defines canonical phase order as preplan -> businessplan -> techplan -> finalizeplan.
  - lifecycle.yaml explicitly treats dev as delegation (not a lifecycle phase).
  - lifecycle.yaml has no first-class rollback phase or rollback transition contract.
- This means rollback behaves as an operational recovery action outside canonical phase progression rather than a modeled lifecycle phase.
- Positive fit:
  - The skill's backward-only target logic and explicit confirmation are consistent with controlled lifecycle discipline.
  - The stated open-PR blocking rule is consistent with lifecycle emphasis on PR-gated promotion.
- Fit risks:
  - Because rollback is marked anytime in module-help but not modeled in lifecycle transitions, policy boundaries depend on skill discipline rather than lifecycle contract enforcement.
  - The skill text references initiative-state.yaml and initiative terminology, while most Lens prompt surfaces and module-help rows are feature-centric; this mixed vocabulary can increase operator ambiguity.

## Evidence Refs
- Required core evidence sources
  - lens.core/_bmad/_config/skill-manifest.csv
  - lens.core/_bmad/_config/bmad-help.csv
  - lens.core/_bmad/lens-work/module.yaml
  - lens.core/_bmad/lens-work/module-help.csv
  - lens.core/_bmad/lens-work/lifecycle.yaml
- Rollback prompt chain
  - .github/prompts/lens-rollback.prompt.md
  - lens.core/_bmad/lens-work/prompts/lens-rollback.prompt.md
- Rollback behavior contract
  - lens.core/_bmad/lens-work/skills/bmad-lens-rollback/SKILL.md
- Related supporting prompt/skill files consulted for context parity with existing audits
  - docs/reports/lens-work/prompt-audit/lens-finalizeplan.audit.md
  - docs/reports/lens-work/prompt-audit/lens-expressplan.audit.md

## Confidence
- Overall: Medium-High.
- Rationale: Prompt routing and Lens registration are explicit and unambiguous across control prompt, release prompt, module.yaml, and module-help.csv.
- Residual uncertainty: Medium, because the rollback capability currently presents as a contract-only skill surface in this path (SKILL.md present, no colocated scripts/references directory under bmad-lens-rollback), so execution details are less inspectable than other Lens skills with script assets.

## Gaps
- Double-stub indirection reduces prompt-local observability: both control and release prompt files are wrappers.
- Lifecycle contract gap: rollback is not represented as a lifecycle transition primitive in lifecycle.yaml, so governance relies on skill-level policy text.
- Command contract drift risk:
  - module-help advertises rollback <featureId> [--target-phase] [--dry-run].
  - rollback SKILL args document --target <phase> (optional) and describe initiative-centric flow.
  - This mismatch can confuse callers and wrappers.
- Discoverability split: rollback appears in Lens-local module/help surfaces but not in global BMAD manifests/help, so tooling that inspects only global BMAD catalogs can miss it.
- Preflight policy split: the control prompt enforces light-preflight; the release prompt is pure delegation and does not restate that guardrail.
