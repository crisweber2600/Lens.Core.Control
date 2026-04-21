# lens-log-problem Prompt Audit

## Purpose Summary
- `.github/prompts/lens-log-problem.prompt.md` is a routing stub, not an implementation prompt.
- It delegates to `lens.core/_bmad/lens-work/prompts/lens-log-problem.prompt.md`, which is also a stub.
- Effective behavior is defined by the native Lens skill `bmad-lens-log-problem`: a lightweight, anytime problem logger that records append-only `problems.md` entries with mandatory phase and category tags, plus `log`, `resolve`, and `list` operations.
- The command is designed to stay out of the main workflow path. The skill explicitly says it "runs in the background of any workflow," "never block[s] the workflow," and feeds `bmad-lens-retrospective` for later pattern analysis.

## BMAD Skill Mapping
- Prompt chain: `.github/prompts/lens-log-problem.prompt.md` -> `lens.core/_bmad/lens-work/prompts/lens-log-problem.prompt.md` -> `bmad-lens-log-problem`.
- Canonical module mapping is Lens-local, not BMAD-global. `module.yaml` registers `bmad-lens-log-problem` as "Problem capture and logging for Lens features," and `module-help.csv` exposes it as `log-problem` with menu code `LG` and phase `anytime`.
- There is no direct BMAD registry entry for `bmad-lens-log-problem` in `_config/skill-manifest.csv` or `_config/bmad-help.csv`. Within the audit scope, that means this prompt is not a wrapped BMAD Method skill like the `bmad-lens-bmad-skill` rows used for brainstorming, PRD, UX, architecture, epics/stories, sprint planning, story creation, quick dev, and code review.
- Functional BMAD adjacency is indirect rather than one-to-one. The closest downstream relationship is retrospective/learning, not a BMAD authoring workflow: `module-help.csv` wires `bmad-lens-retrospective:analyze-problems` after `bmad-lens-log-problem:log`, and the retrospective skill is explicitly phase-aware and consumes `problems.md`.

## Lifecycle Fit
- Fit is strong as a cross-cutting utility. `module-help.csv` marks all log/resolve/list commands as `anytime`, and Lens help topics classify `/log-problem` under `tracking` with `phases: [all]`.
- Fit is weak as a formal lifecycle contract artifact. `lifecycle.yaml` does not define `/log-problem` as a phase, milestone gate, auto-advance step, or execution contract step. It is auxiliary telemetry for the lifecycle rather than part of the promotion backbone.
- The skill's own vocabulary aligns with the major lifecycle span because it requires phase tags from `preplan`, `businessplan`, `techplan`, `finalizeplan`, `dev`, and `complete`, which mirrors the phases and terminal completion concepts Lens cares about.
- The strongest lifecycle linkage is retrospective feedback. The help surface, module-help dependency, template comment, and retrospective skill all describe `problems.md` as an input to later analysis rather than an approval or routing gate.

## Evidence Refs
- Control-repo prompt stub: `.github/prompts/lens-log-problem.prompt.md:5`, `.github/prompts/lens-log-problem.prompt.md:10`
- Release prompt stub: `lens.core/_bmad/lens-work/prompts/lens-log-problem.prompt.md:5`, `lens.core/_bmad/lens-work/prompts/lens-log-problem.prompt.md:10`
- Module skill registration: `lens.core/_bmad/lens-work/module.yaml:58`, `lens.core/_bmad/lens-work/module.yaml:59`
- Module prompt registration: `lens.core/_bmad/lens-work/module.yaml:229`
- Command surface: `lens.core/_bmad/lens-work/module-help.csv:12`, `lens.core/_bmad/lens-work/module-help.csv:44`, `lens.core/_bmad/lens-work/module-help.csv:45`
- Retrospective handoff: `lens.core/_bmad/lens-work/module-help.csv:23`
- BMAD bridge rows for comparison: `lens.core/_bmad/lens-work/module-help.csv:55`
- Lens help classification: `lens.core/_bmad/lens-work/skills/bmad-lens-help/assets/help-topics.yaml:18`, `lens.core/_bmad/lens-work/skills/bmad-lens-help/assets/help-topics.yaml:19`, `lens.core/_bmad/lens-work/skills/bmad-lens-help/assets/help-topics.yaml:21`
- Log-problem skill contract: `lens.core/_bmad/lens-work/skills/bmad-lens-log-problem/SKILL.md:6`, `lens.core/_bmad/lens-work/skills/bmad-lens-log-problem/SKILL.md:33`, `lens.core/_bmad/lens-work/skills/bmad-lens-log-problem/SKILL.md:42`, `lens.core/_bmad/lens-work/skills/bmad-lens-log-problem/SKILL.md:83`, `lens.core/_bmad/lens-work/skills/bmad-lens-log-problem/SKILL.md:125`
- Runtime path implementation: `lens.core/_bmad/lens-work/skills/bmad-lens-log-problem/scripts/log-problem-ops.py:60`
- Problems template consumption note: `lens.core/_bmad/lens-work/assets/templates/problems-template.md:6`
- Retrospective consumption contract: `lens.core/_bmad/lens-work/skills/bmad-lens-retrospective/SKILL.md:3`, `lens.core/_bmad/lens-work/skills/bmad-lens-retrospective/SKILL.md:39`, `lens.core/_bmad/lens-work/skills/bmad-lens-retrospective/SKILL.md:44`, `lens.core/_bmad/lens-work/skills/bmad-lens-retrospective/SKILL.md:62`
- Lifecycle problem-logging contract: `lens.core/_bmad/lens-work/lifecycle.yaml:833`, `lens.core/_bmad/lens-work/lifecycle.yaml:836`, `lens.core/_bmad/lens-work/lifecycle.yaml:839`
- Negative mapping check: `lens.core/_bmad/_config/skill-manifest.csv`, `lens.core/_bmad/_config/bmad-help.csv`

## Confidence
- Overall: **High**.
- High confidence on purpose, routing chain, and module-level classification because the prompt, module, help, skill, and script evidence all agree that this is a Lens-native anytime tracking utility.
- Slightly reduced confidence on lifecycle normalization because the lifecycle contract contains older path metadata that conflicts with the active skill and implementation.

## Gaps
- Double-stub indirection means the prompt itself carries almost no executable policy. Audit confidence comes from the delegated skill and script, not from prompt text.
- `lifecycle.yaml` still defines `problem_logging.file_pattern` as `docs/lens-work/initiatives/{domain}/{service}/problems.md`, while the active skill, script, template, and retrospective flow operate on `features/{domain}/{service}/{featureId}/problems.md`. That is contract drift in a file the user explicitly asked to treat as evidence.
- Category vocabularies are inconsistent between producer and consumer. `bmad-lens-log-problem` uses `dependency-issue`, `tech-debt`, and `process-breakdown`, while `bmad-lens-retrospective` expects `external-dependency`, `technical-debt`, `process-gap`, plus additional values like `communication-breakdown` and `unknown`. Unless normalization exists elsewhere, downstream analysis can be lossy or inconsistent.
- BMAD discoverability is weak in the global registries requested for this audit. Users looking only at `_config/skill-manifest.csv` or `_config/bmad-help.csv` will not find `log-problem`; they must rely on Lens module-local registries.
- Lifecycle placement is intentionally auxiliary, but the current prompt/skill text does not state clear escalation thresholds for when a logged problem should remain telemetry versus trigger `/correct-course`, `/retrospective`, or issue export. That leaves cross-workflow usage somewhat underspecified.