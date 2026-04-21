# lens-git-orchestration Prompt Audit

## Purpose Summary
- `.github/prompts/lens-git-orchestration.prompt.md` is a control-repo stub, not the behavioral source of truth.
- The control stub delegates to `lens.core/_bmad/lens-work/prompts/lens-git-orchestration.prompt.md`, which is itself a second stub.
- Effective behavior is defined in `lens.core/_bmad/lens-work/skills/bmad-lens-git-orchestration/SKILL.md` and its backing script `scripts/git-orchestration-ops.py`.
- That downstream skill owns the actual purpose: write-side git orchestration for Lens' 2-branch control-repo model, target-repo dev branch preparation, PR creation/reuse, merge operations, artifact commits, pushes, and governance publication.

## BMAD Skill Mapping
- Prompt chain:
  - `.github/prompts/lens-git-orchestration.prompt.md`
  - `lens.core/_bmad/lens-work/prompts/lens-git-orchestration.prompt.md`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-git-orchestration/SKILL.md`
- Lens module registration is explicit:
  - `module.yaml` registers `bmad-lens-git-orchestration` as a Lens skill and registers `.github/prompts/lens-git-orchestration.prompt.md` as a shipped prompt surface.
- Lens operational help is explicit:
  - `module-help.csv` maps the skill to display name `orchestrate-git`, code `GO`, action `create-feature-branches`, phase `anytime`, and output `git operations log`.
- Global BMAD registries do not own this mapping:
  - `lens.core/_bmad/_config/skill-manifest.csv` does not contain `bmad-lens-git-orchestration`.
  - `lens.core/_bmad/_config/bmad-help.csv` does not contain `bmad-lens-git-orchestration`.
  - That absence is consistent with this being a Lens module-local skill rather than a canonical BMAD core/bmm skill.

## Lifecycle Fit
- Fit is strong at the architectural level:
  - The skill explicitly describes itself as the write counterpart to `bmad-lens-git-state` and enforces the Lens 2-branch invariant in the control repo.
  - The lifecycle contract states that `finalizeplan` auto-advances to `/dev`, that `dev-ready` is constitution-gated, and that `dev` is a delegation command rather than a lifecycle phase.
  - The skill's `prepare-dev-branch` capability matches that contract by preparing the target repo working branch used by Dev without changing the control-repo topology.
  - The skill's `publish-to-governance` capability also fits the control-repo artifact-authority model by mirroring staged docs into governance through the CLI instead of ad hoc patching.
- Fit is weaker at the prompt surface:
  - Neither prompt stub explains when git writes are appropriate in the lifecycle, which repo is authoritative for which artifact class, or which operations are phase-sensitive.
  - Those constraints only become visible after following the stub chain into the downstream skill.
- Operational nuance:
  - `module-help.csv` labels the command `anytime`, but several capabilities are lifecycle-adjacent in practice: `create-feature-branches` aligns with feature init, `merge-plan` and `publish-to-governance` align with planning promotion, and `prepare-dev-branch` aligns with post-`finalizeplan` delegation into Dev.
  - That `anytime` label is serviceable for discoverability, but it is broader than the lifecycle contract implied by `lifecycle.yaml`.

## Evidence Refs
- Control prompt stub:
  - `.github/prompts/lens-git-orchestration.prompt.md:5`
  - `.github/prompts/lens-git-orchestration.prompt.md:7`
- Release prompt stub:
  - `lens.core/_bmad/lens-work/prompts/lens-git-orchestration.prompt.md:5`
  - `lens.core/_bmad/lens-work/prompts/lens-git-orchestration.prompt.md:7`
- Lens skill contract:
  - `lens.core/_bmad/lens-work/skills/bmad-lens-git-orchestration/SKILL.md:8`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-git-orchestration/SKILL.md:12`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-git-orchestration/SKILL.md:25`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-git-orchestration/SKILL.md:34`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-git-orchestration/SKILL.md:84`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-git-orchestration/SKILL.md:138`
- Scripted implementation:
  - `lens.core/_bmad/lens-work/skills/bmad-lens-git-orchestration/scripts/git-orchestration-ops.py:7`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-git-orchestration/scripts/git-orchestration-ops.py:981`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-git-orchestration/scripts/git-orchestration-ops.py:1019`
- Required registry/module/help/lifecycle surfaces:
  - `lens.core/_bmad/lens-work/module.yaml:39`
  - `lens.core/_bmad/lens-work/module.yaml:40`
  - `lens.core/_bmad/lens-work/module.yaml:328`
  - `lens.core/_bmad/lens-work/module-help.csv:4`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:89`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:91`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:181`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:233`
- Global BMAD registry context reviewed for absence of Lens-local entry:
  - `lens.core/_bmad/_config/skill-manifest.csv`
  - `lens.core/_bmad/_config/bmad-help.csv`

## Confidence
- Overall: **High**.
- High confidence on prompt routing, module registration, command mapping, and lifecycle alignment because those are directly declared in the stubs, `module.yaml`, `module-help.csv`, `lifecycle.yaml`, and the git orchestration skill contract.
- Medium confidence on full operator behavior under every branch/PR edge case because this audit inspected contracts and implementation surfaces but did not execute the orchestration commands end to end.

## Gaps
- Double-stub indirection:
  - Both prompt layers are wrappers, so prompt-local content does not expose the actual write contract.
- Registry discoverability split:
  - The skill is Lens-local and absent from global BMAD registries, which can mislead tooling or reviewers who expect `skill-manifest.csv` or `bmad-help.csv` to be authoritative for every callable skill.
- Phase discoverability is weaker than phase reality:
  - `module-help.csv` marks the command `anytime`, but several operations are meaningfully tied to feature init, plan promotion, or Dev handoff.
- Prompt-level guardrails are thin:
  - Critical constraints such as not modifying `feature.yaml`, using the publish CLI for governance mirroring, and preserving the 2-branch invariant exist only in the downstream skill/script, not in the prompt text itself.