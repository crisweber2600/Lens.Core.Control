# lens-bmad-market-research Prompt Audit

## Purpose Summary
- `.github/prompts/lens-bmad-market-research.prompt.md` is a control-repo stub that runs lightweight preflight, then delegates to the release prompt under `lens.core`.
- The release prompt (`lens.core/_bmad/lens-work/prompts/lens-bmad-market-research.prompt.md`) is also a stub and delegates execution to `bmad-lens-bmad-skill` with `--skill bmad-market-research`.
- Effective runtime behavior therefore lives in the Lens wrapper contract (`bmad-lens-bmad-skill`) plus the downstream BMAD market-research workflow (`bmad-market-research/workflow.md`), not in prompt body logic.

## BMAD Skill Mapping
- Canonical skill identity is explicit in `skill-manifest.csv`: `bmad-market-research` maps to `_bmad/bmm/1-analysis/research/bmad-market-research/SKILL.md`.
- BMAD help metadata maps this command to `1-analysis` (`Market Research`, code `MR`) with research document output.
- Lens module wiring includes the market-research prompt in the prompt list and exposes `bmad-lens-preplan` as the planning-phase conductor that owns early analysis.
- Lens operational help maps menu code `BMR` to `bmad-lens-bmad-skill` action `market-research` in phase `preplan`, outputting to `feature.yaml.docs.path`.
- Lens skill-registry and wrapper metadata align on `contextMode=feature-required`, `outputMode=planning-docs`, and `phaseHints=[preplan]` for `bmad-market-research`.
- Downstream skill is a thin SKILL stub that delegates to `workflow.md`, where core behavior is defined (web-search prerequisite + research initialization flow).

## Lifecycle Fit
- Lifecycle contract places research in `preplan` artifacts and starts canonical `phase_order` at `preplan`; this matches the wrapper/registry mapping for `bmad-market-research`.
- `module-help.csv` also anchors the Lens command (`BMR`) to `preplan`, so lifecycle position is internally consistent across help and contract surfaces.
- `bmad-lens-preplan` explicitly routes follow-on research through `bmad-lens-bmad-skill`, selecting `bmad-market-research` when market-focused synthesis is requested.
- Wrapper write-boundary behavior (`planning-docs` to feature docs path, blocked governance direct authoring) is consistent with Lens staged-artifact discipline during planning.

## Evidence Refs
- Control prompt stub + preflight + delegation:
  - `.github/prompts/lens-bmad-market-research.prompt.md:5`
  - `.github/prompts/lens-bmad-market-research.prompt.md:7`
  - `.github/prompts/lens-bmad-market-research.prompt.md:18`
- Control skill stub delegation:
  - `.github/skills/bmad-market-research/SKILL.md:6`
  - `.github/skills/bmad-market-research/SKILL.md:8`
  - `.github/skills/bmad-market-research/SKILL.md:13`
- Release prompt delegation to Lens wrapper:
  - `lens.core/_bmad/lens-work/prompts/lens-bmad-market-research.prompt.md:5`
  - `lens.core/_bmad/lens-work/prompts/lens-bmad-market-research.prompt.md:10`
  - `lens.core/_bmad/lens-work/prompts/lens-bmad-market-research.prompt.md:11`
- Required registry/help/module/lifecycle surfaces:
  - `lens.core/_bmad/_config/skill-manifest.csv:19`
  - `lens.core/_bmad/_config/bmad-help.csv:22`
  - `lens.core/_bmad/lens-work/module.yaml:105`
  - `lens.core/_bmad/lens-work/module.yaml:243`
  - `lens.core/_bmad/lens-work/module.yaml:308`
  - `lens.core/_bmad/lens-work/module-help.csv:48`
  - `lens.core/_bmad/lens-work/module-help.csv:58`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:109`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:238`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:248`
- Relevant prompt/skill contracts:
  - `lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json:41`
  - `lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json:43`
  - `lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json:49`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:128`
  - `lens.core/_bmad/bmm/1-analysis/research/bmad-market-research/SKILL.md:6`
  - `lens.core/_bmad/bmm/1-analysis/research/bmad-market-research/workflow.md:1`
  - `lens.core/_bmad/bmm/1-analysis/research/bmad-market-research/workflow.md:9`
  - `lens.core/_bmad/bmm/1-analysis/research/bmad-market-research/workflow.md:13`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-preplan/SKILL.md:8`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-preplan/SKILL.md:101`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-preplan/SKILL.md:117`

## Confidence
- Overall: **High** for command routing, registry alignment, and lifecycle placement.
- Rationale: control prompt, release prompt, Lens wrapper, registry/help surfaces, and lifecycle contract all point to the same `preplan` placement and delegated execution model.
- Residual uncertainty: **Medium-Low** on operator UX details because both prompt layers and both SKILL entry files are stubs; behavior depends on delegated workflow execution.

## Gaps
- Double-stub layering (control prompt and release prompt) makes prompt-local intent opaque; acceptance criteria and run-time guarantees are only visible in downstream workflow files.
- The downstream workflow hard-requires web search, but this constraint is not surfaced in either prompt stub. Users can invoke the command without knowing it may hard-stop on capability availability.
- Mapping/discoverability is distributed across many surfaces (`module.yaml`, `module-help.csv`, `bmad-help.csv`, skill manifest, skill registry), which increases drift risk if one entry changes without synchronized updates.
- Prompt-level text does not explicitly state expected artifact naming/output schema for market research; those expectations are inherited from wrapper output-mode rules and downstream workflow initialization.