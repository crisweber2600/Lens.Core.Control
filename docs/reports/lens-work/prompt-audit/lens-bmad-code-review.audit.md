# lens-bmad-code-review Prompt Audit

## Purpose Summary
- `.github/prompts/lens-bmad-code-review.prompt.md` is a control-repo stub that runs `light-preflight.py` first, then delegates to the release prompt under `lens.core`.
- The release prompt (`lens.core/_bmad/lens-work/prompts/lens-bmad-code-review.prompt.md`) is also a stub; it delegates to `bmad-lens-bmad-skill` with `--skill bmad-code-review`.
- Effective behavior is therefore determined by three layers: Lens wrapper policy (`bmad-lens-bmad-skill`), the registry contract (`lens-bmad-skill-registry.json`), and the downstream BMAD code-review workflow.

## BMAD Skill Mapping
- Global BMAD registry defines canonical `bmad-code-review` as a BMM implementation skill and separately lists `bmad-review-adversarial-general` and `bmad-review-edge-case-hunter` as core review primitives.
- Global help maps `bmad-code-review` into the 4-implementation story loop (`DS -> CR -> DS/CS/ER`) and describes adversarial + edge-case review as complementary review modes.
- Lens module wiring registers both the prompt (`lens-bmad-code-review.prompt.md`) and the wrapper skill (`bmad-lens-bmad-skill`).
- Lens operational help binds `bmad-code-review` to wrapper command `BCR`, phase `dev`, and output location `docs/implementation-artifacts`.
- Lens BMAD skill registry sets `bmad-code-review` to `contextMode=feature-required`, `outputMode=implementation-target`, and `phaseHints=[dev]`, with entry path to the downstream BMM code-review skill.
- Wrapper policy enforces feature-required context, implementation-target write scope, and delegation to `entryPath`; wrapper table explicitly includes both `bmad-quick-dev` and `bmad-code-review` as dev implementation-target skills.

## Lifecycle Fit
- Fit is strong with the Lens model: `dev` is explicitly a delegation command (not a lifecycle phase), and this prompt is routed as a delegated BMAD implementation action.
- Lifecycle contract makes `finalizeplan` the planning endpoint with adversarial-review gates before progression to dev-ready; this aligns with using code review as a dev-loop quality gate rather than a planning-phase authoring command.
- Track definitions (`quickdev`, `express`) and module help both keep code-review semantics on the implementation side while planning/review artifacts remain staged through earlier phases.
- Minor surface mismatch exists: module-help output location for `bmad-code-review` is `docs/implementation-artifacts` even though registry/wrapper classify it as `implementation-target` (target repo write scope). This looks intentional (report surface in control repo) but should remain explicitly documented to avoid confusion.

## Evidence Refs
- Control prompt stub and preflight/delegation:
  - `.github/prompts/lens-bmad-code-review.prompt.md:5`
  - `.github/prompts/lens-bmad-code-review.prompt.md:14`
  - `.github/prompts/lens-bmad-code-review.prompt.md:18`
- Release prompt delegation:
  - `lens.core/_bmad/lens-work/prompts/lens-bmad-code-review.prompt.md:2`
  - `lens.core/_bmad/lens-work/prompts/lens-bmad-code-review.prompt.md:5`
  - `lens.core/_bmad/lens-work/prompts/lens-bmad-code-review.prompt.md:10`
  - `lens.core/_bmad/lens-work/prompts/lens-bmad-code-review.prompt.md:11`
- Required registry/help/module/lifecycle sources:
  - `lens.core/_bmad/_config/skill-manifest.csv:11`
  - `lens.core/_bmad/_config/skill-manifest.csv:12`
  - `lens.core/_bmad/_config/skill-manifest.csv:36`
  - `lens.core/_bmad/_config/skill-manifest.csv:41`
  - `lens.core/_bmad/_config/bmad-help.csv:9`
  - `lens.core/_bmad/_config/bmad-help.csv:17`
  - `lens.core/_bmad/_config/bmad-help.csv:44`
  - `lens.core/_bmad/_config/bmad-help.csv:45`
  - `lens.core/_bmad/lens-work/module.yaml:130`
  - `lens.core/_bmad/lens-work/module.yaml:131`
  - `lens.core/_bmad/lens-work/module.yaml:211`
  - `lens.core/_bmad/lens-work/module.yaml:253`
  - `lens.core/_bmad/lens-work/module.yaml:318`
  - `lens.core/_bmad/lens-work/module-help.csv:54`
  - `lens.core/_bmad/lens-work/module-help.csv:68`
  - `lens.core/_bmad/lens-work/module-help.csv:69`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:83`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:87`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:166`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:181`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:233`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:238`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:298`
- Relevant wrapper/skill files:
  - `lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json:161`
  - `lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json:166`
  - `lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json:167`
  - `lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json:168`
  - `lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json:169`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:62`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:68`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:115`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:137`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:138`
  - `lens.core/_bmad/bmm/4-implementation/bmad-code-review/SKILL.md:2`
  - `lens.core/_bmad/bmm/4-implementation/bmad-code-review/workflow.md:6`
  - `lens.core/_bmad/core/bmad-review-adversarial-general/SKILL.md:2`
  - `lens.core/_bmad/core/bmad-review-edge-case-hunter/SKILL.md:2`

## Confidence
- Overall: **High** for routing, mapping, and lifecycle placement.
- Rationale: all required manifests/help/module/lifecycle surfaces are internally consistent on `bmad-code-review` being delegated through `bmad-lens-bmad-skill` into dev-oriented implementation flow.
- Residual uncertainty: prompt bodies are stubs, so behavioral guarantees depend on downstream registry/skill/workflow artifacts rather than prompt text itself.

## Gaps
- Dual-stub indirection means prompt text alone does not expose concrete output schema, acceptance criteria, or failure-mode handling for code review.
- Control prompt has `light-preflight.py`; release prompt does not. This split is useful, but susceptible to drift if onboarding/preflight policy changes are applied to only one layer.
- Module-help output location (`docs/implementation-artifacts`) can be read as control-repo output, while wrapper/registry indicate implementation-target execution. Clarifying whether findings are mirrored, summarized, or target-only would reduce operator ambiguity.
- The downstream `bmad-code-review` workflow describes architecture and initialization but does not surface, in this prompt chain, an explicit machine-readable review artifact contract (for example, standardized findings schema) at prompt level.
