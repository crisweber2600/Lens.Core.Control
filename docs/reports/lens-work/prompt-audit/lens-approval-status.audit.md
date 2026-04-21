# lens-approval-status Prompt Audit

## Purpose Summary
- `.github/prompts/lens-approval-status.prompt.md` is a control-layer stub that enforces a shared preflight step, then delegates to the release prompt.
- `lens.core/_bmad/lens-work/prompts/lens-approval-status.prompt.md` is also a stub; it delegates to `bmad-lens-approval-status`.
- Effective behavior is defined in `bmad-lens-approval-status` skill text: read-only aggregation of promotion PR approval state, blocking classification, and actionable next-step reporting.

## BMAD Skill Mapping
- Prompt chain: `.github/prompts/lens-approval-status.prompt.md` -> `lens.core/_bmad/lens-work/prompts/lens-approval-status.prompt.md` -> `lens.core/_bmad/lens-work/skills/bmad-lens-approval-status/SKILL.md`.
- Module registration exists in `lens.core/_bmad/lens-work/module.yaml` with description "Promotion PR approval status aggregation".
- Command-surface mapping exists in `lens.core/_bmad/lens-work/module-help.csv`: skill `bmad-lens-approval-status`, display `approval-status`, menu code `AS`, action `check`, phase `anytime`.
- Global BMAD registries in `lens.core/_bmad/_config/skill-manifest.csv` and `lens.core/_bmad/_config/bmad-help.csv` do not list `bmad-lens-approval-status`; discovery authority is module-local rather than global.

## Lifecycle Fit
- Fit is conceptually strong for governance checkpoints: the skill centers on promotion PR state, and lifecycle axioms require PR-gated progression (`A2: PRs are the only gating mechanism`).
- `module-help.csv` marks the command as `anytime`, which is useful operationally for status inspection across phases, but weaker as an explicit gate because no phase-bound enforcement is defined for this command.
- Lifecycle milestones and phase order are explicit (`techplan`, `finalizeplan`, `dev-ready`; `preplan -> businessplan -> techplan -> finalizeplan`), and the approval-status surface aligns as an observability overlay across those milestones.

## Evidence Refs
- Control prompt stub + preflight + delegation: `.github/prompts/lens-approval-status.prompt.md:5`, `.github/prompts/lens-approval-status.prompt.md:14`, `.github/prompts/lens-approval-status.prompt.md:18`
- Release prompt stub + skill delegation: `lens.core/_bmad/lens-work/prompts/lens-approval-status.prompt.md:5`, `lens.core/_bmad/lens-work/prompts/lens-approval-status.prompt.md:10`
- Skill contract (purpose/scope/readonly/integrations): `lens.core/_bmad/lens-work/skills/bmad-lens-approval-status/SKILL.md:3`, `lens.core/_bmad/lens-work/skills/bmad-lens-approval-status/SKILL.md:12`, `lens.core/_bmad/lens-work/skills/bmad-lens-approval-status/SKILL.md:14`, `lens.core/_bmad/lens-work/skills/bmad-lens-approval-status/SKILL.md:38`, `lens.core/_bmad/lens-work/skills/bmad-lens-approval-status/SKILL.md:68`, `lens.core/_bmad/lens-work/skills/bmad-lens-approval-status/SKILL.md:69`
- Module registration + prompt registration: `lens.core/_bmad/lens-work/module.yaml:139`, `lens.core/_bmad/lens-work/module.yaml:140`, `lens.core/_bmad/lens-work/module.yaml:238`, `lens.core/_bmad/lens-work/module.yaml:303`
- Command mapping row: `lens.core/_bmad/lens-work/module-help.csv:72`
- Lifecycle PR-gate axiom + milestone/phase scaffolding: `lens.core/_bmad/lens-work/lifecycle.yaml:61`, `lens.core/_bmad/lens-work/lifecycle.yaml:78`, `lens.core/_bmad/lens-work/lifecycle.yaml:238`, `lens.core/_bmad/lens-work/lifecycle.yaml:649`
- Global registry absence check (no `bmad-lens-approval-status` entry): `lens.core/_bmad/_config/skill-manifest.csv`, `lens.core/_bmad/_config/bmad-help.csv`

## Confidence
- Overall: **Medium**.
- High confidence in routing/mapping and lifecycle alignment (direct text evidence from prompt chain, module registry, and lifecycle contract).
- Confidence is reduced because implementation appears specification-only at this layer (skill doc present, but no colocated executable ops script under the skill directory), so runtime behavior cannot be fully validated from prompt/skill text alone.

## Gaps
- Double-stub prompt chain means prompt files themselves contain no executable logic; assurance depends on downstream skill contract fidelity.
- `bmad-lens-approval-status` is absent from global BMAD manifests in `_bmad/_config`, creating discoverability inconsistency between global vs Lens module-local registries.
- The skill references integration/query behavior but does not expose concrete command examples, response schema versioning, or implementation file linkage in the same folder, limiting auditability.
- `anytime` lifecycle placement is useful for observability but does not express explicit gate semantics (for example, "must pass before milestone promotion"), leaving enforcement implicit rather than contractually explicit.
