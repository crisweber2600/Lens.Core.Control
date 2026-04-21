# lens-git-state Prompt Audit

## Purpose Summary
- `.github/prompts/lens-git-state.prompt.md` is a routing stub, not an implementation prompt.
- It delegates to `lens.core/_bmad/lens-work/prompts/lens-git-state.prompt.md`, which is also a stub.
- Effective behavior is defined by `lens.core/_bmad/lens-work/skills/bmad-lens-git-state/SKILL.md` plus its reference docs and script. The resulting command surface is a read-only Lens utility for three classes of queries: `feature-state`, `branches`, and `active-features`.
- The skill is explicitly observational. It combines `feature.yaml` metadata with branch existence, surfaces discrepancies instead of reconciling them, and never performs git writes.

## BMAD Skill Mapping
- Prompt chain: `.github/prompts/lens-git-state.prompt.md` -> `lens.core/_bmad/lens-work/prompts/lens-git-state.prompt.md` -> `bmad-lens-git-state`.
- Module registration is Lens-local, not global. `lens.core/_bmad/lens-work/module.yaml` registers `bmad-lens-git-state` as a skill with description `Read-only git queries for the Lens 2-branch feature model` and includes `lens-git-state.prompt.md` in the module prompt catalog.
- Operational mapping is also Lens-local. `lens.core/_bmad/lens-work/module-help.csv` maps the skill to display name `read-git-state`, menu code `GS`, action `feature-state`, phase `anytime`, and output `git state JSON`.
- The global BMAD registries requested for this audit, `lens.core/_bmad/_config/skill-manifest.csv` and `lens.core/_bmad/_config/bmad-help.csv`, do not contain `bmad-lens-git-state`. That means generic BMAD discovery surfaces do not advertise this prompt or skill; the authority for mapping is the Lens module metadata, not the global BMAD catalogs.
- The skill is also a dependency surface for other Lens skills rather than a standalone lifecycle conductor. The surrounding Lens skill contracts cite it for initiative enumeration, branch-state loading, rollback context, sensing, quickplan cross-feature context, and approval/audit support.

## Lifecycle Fit
- Fit is structurally good but indirect. `bmad-lens-git-state` is not a lifecycle phase command; `module-help.csv` marks it `anytime`, which matches its role as a read-only inspection utility.
- The lifecycle contract in `lens.core/_bmad/lens-work/lifecycle.yaml` makes git and PR state foundational: `Git is the only source of truth for shared workflow state`, and lifecycle progression is gated through milestones and reviews. A read-only state inspector is therefore a necessary support skill for the phase conductors.
- The skill aligns well with the Lens v4 topology described in `lifecycle.yaml`: planning culminates at `finalizeplan`, then `/dev` is an auto-advance handoff rather than a canonical lifecycle phase. Git-state helps confirm whether branch reality and `feature.yaml` metadata agree before or after those transitions.
- The skill also fits the module model in `module.yaml`, where Lens is described as a guided lifecycle router with automated branch topology. `bmad-lens-git-state` supplies the read-side visibility that complements `bmad-lens-git-orchestration` on the write side.

## Evidence Refs
- Prompt wrapper entrypoint:
  - `.github/prompts/lens-git-state.prompt.md`
- Lens prompt wrapper:
  - `lens.core/_bmad/lens-work/prompts/lens-git-state.prompt.md`
- Canonical skill contract:
  - `lens.core/_bmad/lens-work/skills/bmad-lens-git-state/SKILL.md`
- Reference docs for subcommands:
  - `lens.core/_bmad/lens-work/skills/bmad-lens-git-state/references/feature-state.md`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-git-state/references/branch-queries.md`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-git-state/references/active-features.md`
- Script implementation and CLI contract:
  - `lens.core/_bmad/lens-work/skills/bmad-lens-git-state/scripts/git-state-ops.py`
- Focused behavioral confirmation:
  - `lens.core/_bmad/lens-work/skills/bmad-lens-git-state/scripts/tests/test-git-state-ops.py`
- Module registration and prompt catalog:
  - `lens.core/_bmad/lens-work/module.yaml`
- Lens-local command mapping:
  - `lens.core/_bmad/lens-work/module-help.csv`
- Global BMAD registry context and discoverability gap check:
  - `lens.core/_bmad/_config/skill-manifest.csv`
  - `lens.core/_bmad/_config/bmad-help.csv`
- Lifecycle contract used for fit and terminology checks:
  - `lens.core/_bmad/lens-work/lifecycle.yaml`

## Confidence
- Overall: **Medium-High**.
- High confidence that the prompt is only a delegating wrapper and that the real behavior is Lens-local and read-only; those points are explicit in the prompt stubs, module metadata, skill contract, and script interface.
- High confidence that the skill exposes more than one operation even though `module-help.csv` foregrounds `feature-state`; the skill contract and script both define `feature-state`, `branches`, and `active-features`.
- Slightly reduced confidence on full end-to-end runtime semantics because this audit did not execute the script in a live feature repo. Confidence is still supported by the focused tests for discrepancy exit codes, branch matching behavior, and active-feature filters.

## Gaps
- Double-stub indirection: both prompt files are wrappers, so prompt-level review alone cannot validate behavior. Real correctness lives in the skill, references, and script.
- Discoverability split: `bmad-lens-git-state` is absent from the global BMAD registries used in this audit scope and is only discoverable through Lens-local metadata in `module.yaml` and `module-help.csv`.
- Reference-doc drift versus implementation: `active-features.md` documents `--domain`, `--phase`, and `--track`, but the script and tests also support `--status` and `--limit`. Those user-visible filters are implemented but undocumented in the reference file.
- Internal topology inconsistency in git-state references: `feature-state.md` and `active-features.md` say the governance repo stays on `main` and has no feature branches, while `branch-queries.md` describes `{featureId}` and `{featureId}-plan` as governance-repo branches. The prompt itself does not resolve that contradiction.
- Lifecycle terminology drift: `active-features.md` uses `--phase dev` in its example output, but `lifecycle.yaml` explicitly states that `dev` is not a lifecycle phase and is instead a delegation command after `finalizeplan`. That makes the prompt family vulnerable to phase-language confusion unless the docs are normalized.