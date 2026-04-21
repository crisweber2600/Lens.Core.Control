# lens-bmad-domain-research Prompt Audit

## Purpose Summary
- `.github/prompts/lens-bmad-domain-research.prompt.md` is a control-repo stub that runs lightweight preflight, then delegates to the release prompt under `lens.core`.
- The release prompt (`lens.core/_bmad/lens-work/prompts/lens-bmad-domain-research.prompt.md`) is also a stub and delegates execution to `bmad-lens-bmad-skill` with `--skill bmad-domain-research`.
- Effective behavior therefore lives in the Lens wrapper contract (`bmad-lens-bmad-skill`) plus the downstream BMAD domain-research workflow (`bmad-domain-research/workflow.md`), not in prompt body logic.

## BMAD Skill Mapping
- Canonical BMAD identity is explicit in `skill-manifest.csv`: `bmad-domain-research` maps to `_bmad/bmm/1-analysis/research/bmad-domain-research/SKILL.md`.
- BMAD help metadata maps this command to `1-analysis` (`Domain Research`, code `DR`) with research document output.
- Lens module wiring includes the domain-research prompt in the prompt list and exposes the Lens BMAD wrapper skill (`bmad-lens-bmad-skill`) that delegates to registered BMAD skills.
- Lens operational help maps menu code `BDR` to `bmad-lens-bmad-skill` action `domain-research` in phase `preplan`, outputting to `feature.yaml.docs.path`.
- Lens skill-registry and wrapper metadata align on `contextMode=feature-required`, `outputMode=planning-docs`, and `phaseHints=[preplan]` for `bmad-domain-research`.
- Downstream skill is a thin SKILL stub that delegates to `workflow.md`, where core behavior is defined (web-search prerequisite + research initialization flow).

## Lifecycle Fit
- Lifecycle contract places `research` in `preplan` artifacts and starts canonical `phase_order` at `preplan`; this matches the wrapper/registry mapping for `bmad-domain-research`.
- `module-help.csv` anchors the Lens command (`BDR`) to `preplan`, so lifecycle position is internally consistent across help and contract surfaces.
- `module.yaml` positions `bmad-lens-preplan` as the phase conductor and separately registers both `lens-bmad-domain-research.prompt.md` and `bmad-lens-bmad-skill`, matching the delegated routing model used by this prompt.
- Wrapper write-boundary behavior (`planning-docs` to feature docs path, blocked governance direct authoring) is consistent with Lens staged-artifact discipline in preplan.

## Evidence Refs
- Control prompt stub + preflight + delegation:
  - `.github/prompts/lens-bmad-domain-research.prompt.md:7`
  - `.github/prompts/lens-bmad-domain-research.prompt.md:14`
  - `.github/prompts/lens-bmad-domain-research.prompt.md:18`
- Control skill stub delegation:
  - `.github/skills/bmad-domain-research/SKILL.md:8`
  - `.github/skills/bmad-domain-research/SKILL.md:13`
- Release prompt delegation to Lens wrapper:
  - `lens.core/_bmad/lens-work/prompts/lens-bmad-domain-research.prompt.md:10`
  - `lens.core/_bmad/lens-work/prompts/lens-bmad-domain-research.prompt.md:11`
- Required registry/help/module/lifecycle surfaces:
  - `lens.core/_bmad/_config/skill-manifest.csv:18`
  - `lens.core/_bmad/_config/bmad-help.csv:19`
  - `lens.core/_bmad/lens-work/module.yaml:105`
  - `lens.core/_bmad/lens-work/module.yaml:130`
  - `lens.core/_bmad/lens-work/module.yaml:242`
  - `lens.core/_bmad/lens-work/module.yaml:307`
  - `lens.core/_bmad/lens-work/module-help.csv:57`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:109`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:115`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:117`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:238`
- Relevant prompt/skill contracts:
  - `lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json:29`
  - `lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json:35`
  - `lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json:36`
  - `lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json:37`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:128`
  - `lens.core/_bmad/bmm/1-analysis/research/bmad-domain-research/SKILL.md:6`
  - `lens.core/_bmad/bmm/1-analysis/research/bmad-domain-research/workflow.md:1`
  - `lens.core/_bmad/bmm/1-analysis/research/bmad-domain-research/workflow.md:9`
  - `lens.core/_bmad/bmm/1-analysis/research/bmad-domain-research/workflow.md:14`
  - `lens.core/_bmad/bmm/1-analysis/research/bmad-domain-research/workflow.md:41`

## Confidence
- Overall: **High** for command routing, registry alignment, and lifecycle placement.
- Rationale: control prompt, release prompt, Lens wrapper, registry/help/module/lifecycle contract, and downstream workflow all converge on the same delegated `preplan` behavior.
- Residual uncertainty: **Medium-Low** on interactive UX specifics because both prompt layers and both SKILL entry files are stubs; runtime detail is delegated into workflow and wrapper execution.

## Gaps
- Double-stub layering (control prompt and release prompt) makes prompt-local intent opaque; acceptance criteria and runtime guarantees are visible only in downstream wrapper/workflow files.
- The downstream workflow hard-requires web search, but this requirement is not surfaced in the release prompt stub itself; it appears only in control prompt preflight notes and workflow internals.
- Discoverability is distributed across multiple surfaces (`module.yaml`, `module-help.csv`, `_config` manifests, and skill registry), increasing drift risk if one mapping changes without synchronized updates.
- `_config/bmad-help.csv` exposes the base BMAD `DR` command, while Lens-specific `BDR` routing lives only in `module-help.csv`; operators auditing only global help can miss Lens wrapper semantics.