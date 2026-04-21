# lens-bmad-product-brief Prompt Audit

## Purpose Summary
- `.github/prompts/lens-bmad-product-brief.prompt.md` is a control-repo stub that runs lightweight preflight, then delegates to the release prompt under `lens.core`.
- The release prompt (`lens.core/_bmad/lens-work/prompts/lens-bmad-product-brief.prompt.md`) is also a stub and delegates to `bmad-lens-bmad-skill` with `--skill bmad-product-brief`.
- Effective runtime behavior is therefore defined by the Lens wrapper (`bmad-lens-bmad-skill`) and the downstream BMAD skill (`bmad-product-brief`) plus its stage prompts.

## BMAD Skill Mapping
- Canonical BMAD mapping is explicit in `skill-manifest.csv`: `bmad-product-brief` resolves to `_bmad/bmm/1-analysis/bmad-product-brief/SKILL.md`.
- Global BMAD help maps this to `Create Brief` (`CB`) in `1-analysis`, indicating analysis-phase planning output.
- Lens module wiring includes both the product-brief prompt and the generic BMAD wrapper (`bmad-lens-bmad-skill`) in the Lens command surface.
- Lens operational help maps wrapper action `BPF` (`bmad-product-brief`) to phase `preplan` with output rooted at `feature.yaml.docs.path`.
- Lens registry and wrapper agree on `contextMode=feature-required`, `outputMode=planning-docs`, and `phaseHints=[preplan]` for `bmad-product-brief`.
- The downstream BMAD skill defines the staged flow (intent, contextual discovery, elicitation, draft/review, finalize) and output naming contract under `{planning_artifacts}`.

## Lifecycle Fit
- `lifecycle.yaml` defines `preplan` as the analysis phase and explicitly requires `product-brief` as one of the completion artifacts (`ready_when_artifacts: [product-brief, research, brainstorm]`).
- `module-help.csv` aligns this by routing `bmad-lens-preplan` to "brainstorm research and product brief" and exposing `bmad-product-brief` (`BPF`) in `preplan`.
- `module.yaml` registers both the preplan conductor and the product-brief prompt, consistent with the delegated wrapper model.
- Wrapper write-boundary rules for `planning-docs` (feature docs path authoritative, governance/release roots blocked) fit Lens control-repo staging expectations for preplan artifacts.

## Evidence Refs
- Control prompt stub + preflight + delegation:
  - `.github/prompts/lens-bmad-product-brief.prompt.md:7`
  - `.github/prompts/lens-bmad-product-brief.prompt.md:14`
  - `.github/prompts/lens-bmad-product-brief.prompt.md:18`
- Release prompt delegation to Lens wrapper:
  - `lens.core/_bmad/lens-work/prompts/lens-bmad-product-brief.prompt.md:10`
  - `lens.core/_bmad/lens-work/prompts/lens-bmad-product-brief.prompt.md:11`
- Required mapping/lifecycle surfaces:
  - `lens.core/_bmad/_config/skill-manifest.csv:17`
  - `lens.core/_bmad/_config/bmad-help.csv:23`
  - `lens.core/_bmad/lens-work/module.yaml:105`
  - `lens.core/_bmad/lens-work/module.yaml:130`
  - `lens.core/_bmad/lens-work/module.yaml:241`
  - `lens.core/_bmad/lens-work/module-help.csv:48`
  - `lens.core/_bmad/lens-work/module-help.csv:56`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:109`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:116`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:125`
- Relevant prompt/skill contracts:
  - `lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json:17`
  - `lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json:22`
  - `lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json:23`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:33`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:126`
  - `lens.core/_bmad/bmm/1-analysis/bmad-product-brief/SKILL.md:6`
  - `lens.core/_bmad/bmm/1-analysis/bmad-product-brief/SKILL.md:22`
  - `lens.core/_bmad/bmm/1-analysis/bmad-product-brief/SKILL.md:82`
  - `lens.core/_bmad/bmm/1-analysis/bmad-product-brief/prompts/contextual-discovery.md:17`
  - `lens.core/_bmad/bmm/1-analysis/bmad-product-brief/prompts/contextual-discovery.md:23`
  - `lens.core/_bmad/bmm/1-analysis/bmad-product-brief/prompts/draft-and-review.md:20`
  - `lens.core/_bmad/bmm/1-analysis/bmad-product-brief/prompts/finalize.md:3`
  - `lens.core/_bmad/bmm/1-analysis/bmad-product-brief/prompts/finalize.md:11`
  - `lens.core/_bmad/bmm/1-analysis/bmad-product-brief/prompts/finalize.md:24`

## Confidence
- Overall: **High** for routing/mapping/lifecycle alignment.
- Rationale: prompt chain, registry, help surfaces, module wiring, and lifecycle contract consistently place `bmad-product-brief` in preplan as a planning-docs BMAD delegation.
- Residual uncertainty: **Medium-Low** for exact runtime UX because both prompt layers are stubs and behavior is distributed into wrapper + downstream stage prompts.

## Gaps
- Double-stub prompt layering reduces local observability: prompt-local acceptance criteria are thin, while runtime logic lives in wrapper/skill assets.
- The control prompt enforces lightweight preflight, but the release prompt does not restate this operational requirement; behavior depends on preserving the control-layer entrypoint.
- Discovery and governance mapping are spread across multiple surfaces (`module.yaml`, `module-help.csv`, `_config` manifests, and skill registry), increasing drift risk if one changes without synchronized updates.
- Downstream product-brief stage prompts require web research coverage (`Web Researcher` / inline fallback), but this expectation is not visible in the release prompt stub itself.