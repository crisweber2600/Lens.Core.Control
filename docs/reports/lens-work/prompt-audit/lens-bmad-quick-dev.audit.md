# lens-bmad-quick-dev Prompt Audit

## Purpose Summary
- `.github/prompts/lens-bmad-quick-dev.prompt.md` is a control-repo stub that enforces a lightweight preflight (`light-preflight.py`) before delegating to the release prompt in `lens.core`.
- The release prompt (`lens.core/_bmad/lens-work/prompts/lens-bmad-quick-dev.prompt.md`) is also a stub; it delegates to `bmad-lens-bmad-skill` with `--skill bmad-quick-dev`.
- Runtime behavior is therefore composed from the Lens wrapper (`bmad-lens-bmad-skill`), the Lens BMAD registry (`lens-bmad-skill-registry.json`), and the downstream BMAD implementation workflow (`bmad-quick-dev/workflow.md`).

## BMAD Skill Mapping
- Canonical BMAD mapping is explicit in `skill-manifest.csv`: `bmad-quick-dev` resolves to `_bmad/bmm/4-implementation/bmad-quick-dev/SKILL.md`.
- Global BMAD help maps `bmad-quick-dev` as `Quick Dev` (`QQ`), available `anytime`, with implementation artifact outputs.
- Lens module wiring includes both `lens-bmad-quick-dev.prompt.md` and the `bmad-lens-bmad-skill` wrapper skill.
- Lens operational help maps wrapper command `BQD` to `bmad-lens-bmad-skill` action `quick-dev` with phase `dev` and output location `docs/implementation-artifacts`.
- Lens registry and wrapper policy align on `bmad-quick-dev` as `feature-required` + `implementation-target` + `phaseHints=[dev]`, delegating to the downstream BMM quick-dev skill entry path.
- Downstream quick-dev skill is a SKILL stub that delegates to `workflow.md`; workflow intent is to convert user intent into hardened, reviewable implementation artifacts via a step-file sequence (clarify -> plan -> implement -> review -> present).

## Lifecycle Fit
- Placement is consistent with Lens lifecycle semantics: `dev` is described as a delegation command rather than a lifecycle phase, and `bmad-quick-dev` is routed as a delegated implementation action through the wrapper.
- `module-help.csv` places the command in `dev`, while `lifecycle.yaml` keeps canonical `phase_order` to planning phases (`preplan`, `businessplan`, `techplan`, `finalizeplan`), which matches delegated execution after planning.
- The `quickdev` track starts at `finalizeplan`, reinforcing that quick-dev execution should occur after planning consolidation/gating, not as a planning artifact authoring step.
- Wrapper write-boundary rules for `implementation-target` are consistent with this lifecycle intent, but the surfaced output location (`docs/implementation-artifacts`) can be interpreted as control-repo output unless implementation-target behavior is explicitly explained at runtime.

## Evidence Refs
- Control prompt preflight/delegation:
  - `.github/prompts/lens-bmad-quick-dev.prompt.md:5`
  - `.github/prompts/lens-bmad-quick-dev.prompt.md:8`
  - `.github/prompts/lens-bmad-quick-dev.prompt.md:14`
  - `.github/prompts/lens-bmad-quick-dev.prompt.md:18`
- Release prompt delegation:
  - `lens.core/_bmad/lens-work/prompts/lens-bmad-quick-dev.prompt.md:2`
  - `lens.core/_bmad/lens-work/prompts/lens-bmad-quick-dev.prompt.md:5`
  - `lens.core/_bmad/lens-work/prompts/lens-bmad-quick-dev.prompt.md:10`
  - `lens.core/_bmad/lens-work/prompts/lens-bmad-quick-dev.prompt.md:11`
- Required registry/help/module/lifecycle surfaces:
  - `lens.core/_bmad/_config/skill-manifest.csv:41`
  - `lens.core/_bmad/_config/bmad-help.csv:27`
  - `lens.core/_bmad/lens-work/module.yaml:130`
  - `lens.core/_bmad/lens-work/module.yaml:131`
  - `lens.core/_bmad/lens-work/module.yaml:252`
  - `lens.core/_bmad/lens-work/module.yaml:317`
  - `lens.core/_bmad/lens-work/module-help.csv:68`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:238`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:86`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:298`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:300`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:303`
- Relevant wrapper/skill contracts:
  - `lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json:149`
  - `lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json:151`
  - `lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json:154`
  - `lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json:156`
  - `lens.core/_bmad/lens-work/assets/lens-bmad-skill-registry.json:157`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:52`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:74`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:80`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:83`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md:137`
  - `lens.core/_bmad/bmm/4-implementation/bmad-quick-dev/SKILL.md:6`
  - `lens.core/_bmad/bmm/4-implementation/bmad-quick-dev/workflow.md:6`
  - `lens.core/_bmad/bmm/4-implementation/bmad-quick-dev/workflow.md:65`
  - `lens.core/_bmad/bmm/4-implementation/bmad-quick-dev/workflow.md:75`
  - `lens.core/_bmad/bmm/4-implementation/bmad-quick-dev/workflow.md:79`
  - `lens.core/_bmad/bmm/4-implementation/bmad-quick-dev/step-01-clarify-and-route.md:57`
  - `lens.core/_bmad/bmm/4-implementation/bmad-quick-dev/step-01-clarify-and-route.md:89`

## Confidence
- Overall: **High** for routing, skill mapping, and lifecycle placement.
- Rationale: all required surfaces (`skill-manifest`, `bmad-help`, `module.yaml`, `module-help`, and `lifecycle.yaml`) converge on the same delegated `dev` implementation behavior.
- Residual uncertainty: **Medium-Low** for operator-facing runtime specifics because both prompt layers are stubs and rely on downstream wrapper/workflow files for concrete behavior.

## Gaps
- Double-stub prompt layering means prompt-local text does not expose concrete acceptance criteria, output schema, or failure handling for quick-dev; operators must inspect wrapper/workflow artifacts.
- Output-surface ambiguity persists: `module-help.csv` lists `docs/implementation-artifacts`, while registry/wrapper enforce `implementation-target` write scope. This is likely intentional (reporting surface vs. execution surface) but not explicit in prompt text.
- The control prompt includes `light-preflight.py` but the release prompt does not; this split is useful yet drift-prone if preflight policy changes are not synchronized across layers.
- Lifecycle intent is distributed across multiple files (`module-help.csv`, `lifecycle.yaml`, wrapper, registry). Without synchronized updates, command-phase or write-scope semantics could diverge silently.