# lens-split-feature Prompt Audit

## Purpose Summary
- `.github/prompts/lens-split-feature.prompt.md` is a control-surface stub that delegates behavior to the release module.
- The release prompt at `lens.core/_bmad/lens-work/prompts/lens-split-feature.prompt.md` is also a stub; operational behavior lives in `bmad-lens-split-feature` skill assets.
- The effective purpose is safe feature partitioning: validate split eligibility first, create a first-class new feature (feature.yaml + feature-index entry + summary stub), and optionally move eligible stories.

## BMAD Skill Mapping
- Prompt chain:
  - `.github/prompts/lens-split-feature.prompt.md`
  - `lens.core/_bmad/lens-work/prompts/lens-split-feature.prompt.md`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-split-feature/SKILL.md`
- Lens module registration is explicit:
  - `module.yaml` includes `bmad-lens-split-feature` in the skill inventory and `lens-split-feature.prompt.md` in prompt inventory.
- Command-surface mapping is explicit in Lens help registry:
  - `module-help.csv` maps `bmad-lens-split-feature` to `validate-split` (`VS`), `split-feature` (`SL`), and `move-stories` (`MS`).
  - `module-help.csv` also maps phase conductors `preplan`, `businessplan`, `techplan`, `finalizeplan`, and `expressplan`, showing split-feature is an operational utility alongside lifecycle conductors rather than a conductor itself.
- Skill/script contract alignment:
  - `SKILL.md` defines validate-first and hard-block rules for `in-progress` stories, interactive confirmation requirements, and first-class governance artifact creation.
  - `scripts/split-feature-ops.py` implements matching subcommands (`validate-split`, `create-split-feature`, `move-stories`) and enforces blocker behavior.
- Global BMAD registries do not expose Lens-local mapping:
  - No `bmad-lens-split-feature` entry in `_config/skill-manifest.csv` or `_config/bmad-help.csv`.

## Lifecycle Fit
- Fit with lifecycle contract is good for a non-phase utility:
  - `module-help.csv` marks split-feature operations as `anytime`, consistent with a cross-cutting maintenance/orchestration action.
  - `lifecycle.yaml` defines canonical phase progression as `preplan -> businessplan -> techplan -> finalizeplan` with `expressplan` as standalone; split-feature does not alter phase ordering.
- Governance safety fit is strong:
  - Skill principles and references require `validate-split` before execution and block any split involving `status: in-progress` stories.
  - Script-level enforcement returns hard fail for blocked stories and prevents move execution in those cases.
- Fit caveat:
  - Because both prompt layers are wrappers, lifecycle safety is carried by downstream skill/script logic, not prompt-local logic.

## Evidence Refs
- Control prompt stub delegation:
  - `.github/prompts/lens-split-feature.prompt.md:5`
  - `.github/prompts/lens-split-feature.prompt.md:7`
  - `.github/prompts/lens-split-feature.prompt.md:10`
- Release prompt stub delegation:
  - `lens.core/_bmad/lens-work/prompts/lens-split-feature.prompt.md:5`
  - `lens.core/_bmad/lens-work/prompts/lens-split-feature.prompt.md:7`
  - `lens.core/_bmad/lens-work/prompts/lens-split-feature.prompt.md:10`
- Module registration (`module.yaml`):
  - `lens.core/_bmad/lens-work/module.yaml` (skills list includes `bmad-lens-split-feature`; prompts list includes `lens-split-feature.prompt.md`; phase conductors include `bmad-lens-preplan`, `bmad-lens-businessplan`, `bmad-lens-techplan`, `bmad-lens-finalizeplan`, `bmad-lens-expressplan`)
- Command/help mapping (`module-help.csv`):
  - `lens.core/_bmad/lens-work/module-help.csv` (`validate-split`, `split-feature`, `move-stories` rows for `bmad-lens-split-feature`; phase conductor rows for preplan/businessplan/techplan/finalizeplan/expressplan)
- Lifecycle contract alignment:
  - `lens.core/_bmad/lens-work/lifecycle.yaml` (`phases` definitions for preplan/businessplan/techplan/finalizeplan/expressplan and canonical `phase_order: [preplan, businessplan, techplan, finalizeplan]`)
- Skill behavior and workflow references:
  - `lens.core/_bmad/lens-work/skills/bmad-lens-split-feature/SKILL.md`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-split-feature/references/validate-split.md`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-split-feature/references/split-scope.md`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-split-feature/references/split-stories.md`
- Scripted enforcement:
  - `lens.core/_bmad/lens-work/skills/bmad-lens-split-feature/scripts/split-feature-ops.py` (`cmd_validate_split`, `cmd_create_split_feature`, `cmd_move_stories`)
- Global registry absence check:
  - `lens.core/_bmad/_config/skill-manifest.csv` (no `bmad-lens-split-feature` match)
  - `lens.core/_bmad/_config/bmad-help.csv` (no `bmad-lens-split-feature` or `/split-feature` match)

## Confidence
- Overall: **High**.
- High confidence on routing and mapping because prompt stubs, module registration, help command mapping, and lifecycle contract are explicit.
- Medium-high confidence on runtime behavior because script logic was inspected directly, but not executed in this audit.

## Gaps
- Stub indirection gap:
  - Both prompt layers are wrappers; operational guardrails are not visible in prompt-local text.
- Discoverability gap across global manifests:
  - Lens-local commands/skills are not represented in `_config/skill-manifest.csv` or `_config/bmad-help.csv`, so audits that only inspect global registries will miss split-feature capabilities.
- Lifecycle placement ambiguity:
  - `anytime` classification is operationally flexible, but no explicit lifecycle guidance states best timing for split operations (for example, preferred before active dev stories exist).
- Validation scope gap:
  - `validate-split` primarily relies on sprint-plan and status parsing; if status metadata is stale or distributed inconsistently, eligibility can be misclassified unless users reconcile plan/story status first.
