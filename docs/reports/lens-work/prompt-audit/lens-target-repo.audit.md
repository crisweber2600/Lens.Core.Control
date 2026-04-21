# lens-target-repo Prompt Audit

## Purpose Summary
- `.github/prompts/lens-target-repo.prompt.md` is a control-adapter stub that does two things: runs lightweight preflight and delegates execution to the release prompt.
- The release prompt at `lens.core/_bmad/lens-work/prompts/lens-target-repo.prompt.md` is intentionally thin and delegates behavior to the skill contract.
- The functional source of truth is `bmad-lens-target-repo` (`lens.core/_bmad/lens-work/skills/bmad-lens-target-repo/SKILL.md`) plus `target-repo-ops.py`, which define provisioning/registration scope: remote verify/create, canonical clone into TargetProjects, inventory persistence, and feature metadata updates.
- This prompt is therefore a routing surface, not a lifecycle phase conductor or planning authoring surface.

## BMAD Skill Mapping
- Primary mapped skill: `bmad-lens-target-repo`.
- Command mapping is explicit in Lens command registry (`module-help.csv`):
  - Skill: `bmad-lens-target-repo`
  - Display: `target-repo`
  - Menu code: `TR`
  - Action: `provision`
  - Description: provision/register target repo and persist to inventory + feature metadata.
- Module registration (`module.yaml`) confirms this skill and the `lens-target-repo.prompt.md` prompt are both first-class Lens module surfaces.
- Skill integration points indicate upstream/downstream coupling:
  - Upstream from `bmad-lens-init-feature`
  - Downstream to `bmad-lens-dev`
  - Data coupling with `bmad-lens-feature-yaml` and `bmad-lens-discover`
- Global BMAD catalogs (`skill-manifest.csv`, `bmad-help.csv`) do not enumerate Lens-local `bmad-lens-*` skills, so discoverability for this prompt depends on Lens-local module files rather than global registries.

## Lifecycle Fit
- Lifecycle fit is strong for an anytime orchestration utility:
  - `module-help.csv` marks `target-repo` as `phase=anytime` with sequencing `after=bmad-lens-init-feature:create` and `before=bmad-lens-dev:implement`.
- This positioning is compatible with `lifecycle.yaml` because lifecycle phase order governs planning milestones, while target-repo setup is a supporting operation required for implementation root resolution.
- The skill explicitly enforces planning boundary separation (repo orchestration only; no planning artifact authoring), which aligns with lifecycle axioms about disciplined phase responsibilities.
- The script behavior supports lifecycle continuity by persisting repo metadata to both governance inventory and `feature.yaml.target_repos`, ensuring later `/dev` runs can resolve target implementation roots and repo-scoped branch mode deterministically.

## Evidence Refs
- Stub adapter prompt: `.github/prompts/lens-target-repo.prompt.md`
- Release prompt delegate: `lens.core/_bmad/lens-work/prompts/lens-target-repo.prompt.md`
- Skill contract: `lens.core/_bmad/lens-work/skills/bmad-lens-target-repo/SKILL.md`
- Skill implementation script: `lens.core/_bmad/lens-work/skills/bmad-lens-target-repo/scripts/target-repo-ops.py`
- Lens module skill/prompt registration: `lens.core/_bmad/lens-work/module.yaml`
- Lens command mapping and lifecycle adjacency: `lens.core/_bmad/lens-work/module-help.csv`
- Lifecycle contract context: `lens.core/_bmad/lens-work/lifecycle.yaml`
- Global registry scope check: `lens.core/_bmad/_config/skill-manifest.csv`
- Global help scope check: `lens.core/_bmad/_config/bmad-help.csv`

## Confidence
- Overall: High.
- High confidence on purpose, mapping, and lifecycle placement because the prompt, skill contract, module registry, and command mapping are explicit and mutually consistent.
- Medium confidence on edge-case runtime behavior because this audit is static (no live provisioning run in this pass).

## Gaps
- Indirection complexity:
  - User-facing prompt is two-step delegation (stub -> release prompt -> skill), increasing risk of shallow audits missing real behavior.
- Discoverability split:
  - Lens-local command/skill surfaces document this flow, but required global catalogs do not, which can confuse tooling that assumes global `_config` files are exhaustive.
- Lifecycle clarity:
  - `phase=anytime` is correct operationally, but non-obvious to operators who expect every command to map to a lifecycle phase milestone.
- Provider constraints:
  - Auto-create logic is GitHub-host constrained in script behavior; non-GitHub flows depend on manual remote creation guidance.
- Pathing assumptions:
  - Script requires governance repo path ancestry resolvable under `TargetProjects/`; atypical workspace topologies can fail root derivation.
