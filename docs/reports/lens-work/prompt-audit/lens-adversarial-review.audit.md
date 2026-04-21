# lens-adversarial-review Prompt Audit

## Purpose Summary
- .github/prompts/lens-adversarial-review.prompt.md is an adapter stub that enforces a lightweight preflight, then delegates to the module prompt.
- lens.core/_bmad/lens-work/prompts/lens-adversarial-review.prompt.md is also a stub; effective behavior is implemented by bmad-lens-adversarial-review.
- The underlying Lens skill defines this as a lifecycle gate for planning checkpoints with adversarial findings plus a required party-mode blind-spot challenge and a verdict artifact written to staged docs.

## BMAD Skill Mapping
- Prompt chain:
  - .github/prompts/lens-adversarial-review.prompt.md -> lens.core/_bmad/lens-work/prompts/lens-adversarial-review.prompt.md -> lens.core/_bmad/lens-work/skills/bmad-lens-adversarial-review/SKILL.md.
- Lens module registration:
  - module.yaml registers bmad-lens-adversarial-review with description "Lifecycle adversarial review gate with party-mode blind-spot challenge".
  - module-help.csv exposes command surface as display adversarial-review, menu code RV, action review, phase anytime, and lifecycle review outputs.
- Core BMAD dependency mapping:
  - The Lens skill explicitly integrates bmad-review-adversarial-general and bmad-party-mode.
  - _config/skill-manifest.csv and _config/bmad-help.csv include bmad-review-adversarial-general and bmad-party-mode as core skills.
  - _config/skill-manifest.csv and _config/bmad-help.csv do not register bmad-lens-adversarial-review (Lens-local mapping authority is module.yaml plus module-help.csv).

## Lifecycle Fit
- Fit is strong and explicit for lifecycle gate semantics:
  - lifecycle.yaml declares adversarial-review as milestone/phase gate behavior, including party mode for planning-phase completion reviews.
  - preplan, businessplan, techplan, finalizeplan, and expressplan all define completion_review contracts and report outputs that align with the Lens skill output table.
- Command-surface fit is operationally broad:
  - module-help.csv marks the command phase as anytime, which is compatible with manual reruns and phase-complete calls.
- Notable contract divergence to track:
  - The skill scope includes finalizeplan and expressplan, but references/review-contract.md states the contract applies to preplan, businessplan, and techplan only. This creates ambiguity about which rules are normative for finalizeplan/expressplan reviews.

## Evidence Refs
- Prompt adapter and delegation chain
  - .github/prompts/lens-adversarial-review.prompt.md:5
  - .github/prompts/lens-adversarial-review.prompt.md:14
  - .github/prompts/lens-adversarial-review.prompt.md:18
  - lens.core/_bmad/lens-work/prompts/lens-adversarial-review.prompt.md:5
  - lens.core/_bmad/lens-work/prompts/lens-adversarial-review.prompt.md:10
- Module and command registration
  - lens.core/_bmad/lens-work/module.yaml:122
  - lens.core/_bmad/lens-work/module.yaml:124
  - lens.core/_bmad/lens-work/module.yaml:224
  - lens.core/_bmad/lens-work/module-help.csv:51
- Lifecycle gate and phase contracts
  - lens.core/_bmad/lens-work/lifecycle.yaml:87
  - lens.core/_bmad/lens-work/lifecycle.yaml:109
  - lens.core/_bmad/lens-work/lifecycle.yaml:129
  - lens.core/_bmad/lens-work/lifecycle.yaml:149
  - lens.core/_bmad/lens-work/lifecycle.yaml:169
  - lens.core/_bmad/lens-work/lifecycle.yaml:201
  - lens.core/_bmad/lens-work/lifecycle.yaml:238
- Core BMAD registries (requested evidence set)
  - lens.core/_bmad/_config/skill-manifest.csv:10
  - lens.core/_bmad/_config/skill-manifest.csv:11
  - lens.core/_bmad/_config/bmad-help.csv:43
  - lens.core/_bmad/_config/bmad-help.csv:44
- Relevant skill contracts
  - lens.core/_bmad/lens-work/skills/bmad-lens-adversarial-review/SKILL.md:12
  - lens.core/_bmad/lens-work/skills/bmad-lens-adversarial-review/SKILL.md:37
  - lens.core/_bmad/lens-work/skills/bmad-lens-adversarial-review/SKILL.md:53
  - lens.core/_bmad/lens-work/skills/bmad-lens-adversarial-review/SKILL.md:95
  - lens.core/_bmad/lens-work/skills/bmad-lens-adversarial-review/references/review-contract.md:7
  - lens.core/_bmad/core/bmad-review-adversarial-general/SKILL.md:1
  - lens.core/_bmad/core/bmad-party-mode/SKILL.md:1

## Confidence
- Overall: High.
- High confidence on prompt chain, module registration, and lifecycle gate fit because all are directly evidenced in prompt/module/help/lifecycle sources.
- Moderate residual uncertainty is limited to runtime implementation internals beyond SKILL.md contract text.

## Gaps
- Double-stub indirection means prompt text itself does not carry executable behavior; assurance depends on skill contracts and command registries.
- Discoverability split: global BMAD registries expose only core adversarial/party skills, while Lens-local adversarial review routing is module-local. Tooling that reads only global manifests can miss this prompt capability.
- Contract coverage mismatch: references/review-contract.md currently scopes itself to preplan/businessplan/techplan while the Lens skill and lifecycle include finalizeplan and expressplan. Clarifying this contract boundary would reduce review ambiguity.
- The .github adapter includes light-preflight, while the module prompt stub does not; direct module-prompt invocation may bypass the adapter-level preflight expectation unless enforced elsewhere.
