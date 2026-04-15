# Lens-Work Investigation Report

Date: 2026-04-15
Scope: `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work`
Mode: Investigation only
Result: No implementation fixes applied in this pass

## Executive Summary

This investigation audited the current `lens-work` module as a builder-style consistency review, with special attention to prompt entrypoints, installer wiring, module registration, and lifecycle/documentation alignment. The current v4 core is only partially coherent.

The good news is that most of the active v4 phase skills and operational skills exist and resolve correctly. The main problems are concentrated in migration drift between older v3 surfaces and the current v4 lifecycle: dead prompt stubs, one missing current prompt, one malformed source prompt, mismatched installer inventories, incomplete prompt filtering metadata, and multiple documentation sets that still describe the old `devproposal` and `sprintplan` model even though the authoritative lifecycle contract has moved to `finalizeplan`.

The net conclusion is that `lens-work` is not uniformly broken, but it is not internally consistent. The most urgent issues are user-visible prompt entrypoints and installer drift. The next tier is documentation and registry cleanup.

## Method

The audit was performed in two stages:

1. Prompt-family trace passes using `runSubagent` over:
   - lifecycle prompts
   - operational prompts
   - BMAD wrapper prompts
   - manifest / installer / adapter wiring
2. Deterministic verification with direct file inspection and inventory scripts to confirm only the findings that were supported by source state.

The review intentionally distinguished:

- true breakage
- inconsistent or incomplete wiring
- legacy documentation or residual artifacts

## Inventory Baseline

Verified prompt and installer counts:

| Surface | Count | Notes |
|---|---:|---|
| `module.yaml` declared prompts | 59 | Source-of-truth command inventory |
| Source prompt files in `_bmad/lens-work/prompts` | 58 | Missing `lens-quickplan.prompt.md` |
| Checked-in GitHub prompt stubs in `.github/prompts` | 28 | Includes stale legacy names |
| JS installer stub prompts in `_module-installer/installer.js` | 58 | Missing `lens-discover.prompt.md` |
| Python standalone installer prompt stubs in `scripts/install.py` | 17 | Far behind the manifest |

Key note about release behavior:

- The release workflow overlays source `.github/prompts` on top of installer output and preserves installer-generated prompts that have no source twin. Missing source `.github` stubs are therefore inconsistent but not automatically a release blocker.
- Broken checked-in stubs are still real issues because the workflow preserves and validates them as authored prompt artifacts.

Evidence:

- `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/module.yaml`
- `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/_module-installer/installer.js`
- `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/scripts/install.py`
- `TargetProjects/lens.core/src/Lens.Core.Src/.github/workflows/promote-to-release.yml:269-286`
- `TargetProjects/lens.core/src/Lens.Core.Src/.github/workflows/promote-to-release.yml:569-602`

## Verified Findings

### 1. Critical: `lens-quickplan` is declared and exposed, but its source prompt does not exist

The current module registers `bmad-lens-quickplan` and declares `lens-quickplan.prompt.md` as a first-class prompt, but the source prompt file is missing from `_bmad/lens-work/prompts`.

Impact:

- the prompt surface is incomplete
- the checked-in GitHub prompt stub redirects to a missing source file
- the installer and module contract disagree about what is actually present

Evidence:

- `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/module.yaml:55-57`
- `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/module.yaml:216-274`
- `TargetProjects/lens.core/src/Lens.Core.Src/.github/prompts/lens-quickplan.prompt.md:1-12`
- no corresponding file exists at `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/prompts/lens-quickplan.prompt.md`

Assessment:

- this is true breakage, not just stale documentation

### 2. High: Two legacy GitHub prompt stubs are still checked in and both point to missing targets

The checked-in GitHub prompt layer still contains `lens-devproposal.prompt.md` and `lens-sprintplan.prompt.md`. Both redirect to source prompt files that do not exist.

Impact:

- users can still encounter stale legacy entrypoints
- the adapter layer advertises prompt names that no longer resolve
- this makes the GitHub prompt surface simultaneously v3 and v4

Evidence:

- `TargetProjects/lens.core/src/Lens.Core.Src/.github/prompts/lens-devproposal.prompt.md:1-12`
- `TargetProjects/lens.core/src/Lens.Core.Src/.github/prompts/lens-sprintplan.prompt.md:1-12`
- no corresponding files exist at:
  - `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/prompts/lens-devproposal.prompt.md`
  - `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/prompts/lens-sprintplan.prompt.md`

This is reinforced by documentation drift in:

- `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/docs/copilot-adapter-reference.md:13-24`

Assessment:

- these are dead, user-visible artifacts and should not remain published in `.github/prompts`

### 3. High: The source `lens-onboard` prompt is malformed

The source prompt file contains a second frontmatter block concatenated directly to the closing code fence. The checked-in GitHub stub routes into this malformed file.

Impact:

- prompt parsing is unreliable
- deprecation signaling is split between manifest metadata and malformed prompt content
- this raises the risk of prompt rendering or downstream tooling issues

Evidence:

- malformed source prompt: `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/prompts/lens-onboard.prompt.md:1-22`
- checked-in GitHub stub: `TargetProjects/lens.core/src/Lens.Core.Src/.github/prompts/lens-onboard.prompt.md:1-12`
- deprecated skill registration: `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/module.yaml:94-97`

Assessment:

- this is a concrete prompt-authoring defect, not just deprecation drift

### 4. High: Installer paths are materially out of sync with the module contract

The module presents `scripts/install.py` as a valid standalone installer alternative, but its GitHub prompt inventory is far behind the declared module prompt list. The JS installer is closer, but still misses `lens-discover.prompt.md`.

Impact:

- prompt availability varies based on installer path
- GitHub adapter state is not deterministic across install methods
- support and debugging become harder because the installed prompt set is path-dependent

Evidence:

- installer is advertised in `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/module.yaml:163-179`
- Python installer GitHub prompt list: `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/scripts/install.py:169-236`
- Common command list in Python installer: `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/scripts/install.py:120-141`
- JS installer stub list: `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/_module-installer/installer.js:360-369`
- JS installer write loop: `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/_module-installer/installer.js:453-460`
- declared prompt inventory: `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/module.yaml:216-340`

Specific verified drift:

- `scripts/install.py` writes 17 GitHub prompt stubs
- `_module-installer/installer.js` writes 58 GitHub prompt stubs
- `module.yaml` declares 59 prompts
- the JS installer omits `lens-discover.prompt.md`

Assessment:

- this is one of the highest-value cleanup targets because it affects installation, testing, and reproducibility

### 5. Medium: `lens-discover` is active but only partially wired

`lens-discover` exists as a prompt and as a skill, but its registration and packaging are incomplete.

Impact:

- the prompt exists and is usable in some contexts
- the skill is not listed in the module skill registry
- the JS installer does not emit the prompt stub even though the module manifest declares it

Evidence:

- prompt exists: `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/prompts/lens-discover.prompt.md:1-5`
- skill exists: `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/skills/bmad-lens-discover/SKILL.md:1-16`
- prompt declared in manifest: `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/module.yaml:267-274`
- module skill registry block does not include `bmad-lens-discover`: `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/module.yaml:32-157`
- JS installer stub inventory ends at `lens-quickplan` and omits discover: `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/_module-installer/installer.js:360-369`

Assessment:

- this is not dead code, but it is not fully integrated

### 6. Medium: Prompt filtering metadata in `preflight.py` is no longer synchronized with the actual catalog

`preflight.py` documents `_PROMPT_METADATA` as synchronized metadata for role and experience filtering, but many current prompt stems are missing from that table. Unknown stems are always included.

Impact:

- profile-aware prompt filtering is inconsistent
- newly added or migrated prompts silently bypass role / experience gating
- prompt visibility depends on stale metadata rather than the actual manifest

Evidence:

- metadata and fallback behavior: `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/scripts/preflight.py:158-206`
- declared prompts: `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/module.yaml:216-274`

Examples of current prompts missing from `_PROMPT_METADATA`:

- `lens-init-feature`
- `lens-target-repo`
- `lens-status`
- `lens-retrospective`
- `lens-promote`
- `lens-setup`
- `lens-module-management`
- `lens-profile`
- `lens-rollback`
- `lens-sensing`
- `lens-feature-yaml`
- `lens-git-state`
- `lens-git-orchestration`
- `lens-migrate`

Assessment:

- this is a consistency defect that will keep producing confusing prompt sets until the metadata is regenerated or moved closer to the manifest

### 7. Medium: `lens-preflight` bypasses the normal skill-wrapper pattern

Unlike the other current Lens prompts, `lens-preflight` dispatches directly to a script rather than a registered skill.

Impact:

- inconsistent prompt contract
- no skill registration for the flow
- harder to keep behavior aligned with the rest of the module

Evidence:

- prompt dispatches directly to script: `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/prompts/lens-preflight.prompt.md:1-17`
- prompt helper entries still live under deprecated onboard actions: `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/module-help.csv:39-40`
- no `bmad-lens-preflight` skill is present in `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/module.yaml:32-157`

Assessment:

- I did not verify a runtime failure here, but the surface is inconsistent with the rest of the v4 module design

### 8. High: Core docs still describe the older `devproposal` / `sprintplan` lifecycle rather than the current v4 contract

The authoritative lifecycle contract is v4 and explicitly says `FinalizePlan` collapsed the old `DevProposal` and `SprintPlan` chain. Multiple core docs still present the old lifecycle as current behavior.

Impact:

- maintainers and users are given conflicting lifecycle models
- prompt cleanup becomes harder because docs continue to legitimize removed phase names
- architecture and lifecycle docs are no longer safe as first-reference material

Evidence for the real contract:

- lifecycle contract version and migration note: `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/lifecycle.yaml:1-20`
- milestone and phase definitions showing `finalizeplan`: `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/lifecycle.yaml:76-99`
- current `FinalizePlan` phase definition: `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/lifecycle.yaml:169-203`

Evidence for stale documentation:

- `architecture.md` still claims `lifecycle.yaml` is schema v3.2 and lists `devproposal` / `sprintplan` milestones and phases: `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/docs/architecture.md:31-42`
- `lifecycle-reference.md` still lists tracks with `devproposal` / `sprintplan`: `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/docs/lifecycle-reference.md:61-76`
- `lifecycle-visual-guide.md` still presents phase tables for `/devproposal` and `/sprintplan`: `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/docs/lifecycle-visual-guide.md:523-546`
- `component-inventory.md` still lists `/devproposal`, `/sprintplan`, and `/close` as active workflows: `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/docs/component-inventory.md:143-151`
- `copilot-adapter-reference.md` still shows `lens-devproposal.prompt.md` and `lens-sprintplan.prompt.md` in the prompt tree: `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/docs/copilot-adapter-reference.md:13-24`

Assessment:

- this is broad documentation drift, not a single-file typo

### 9. Low to Medium: Unregistered legacy skill directories remain in the tree

The skill directory includes unregistered folders for older concepts such as `bmad-lens-devproposal`, `bmad-lens-sprintplan`, and `bmad-lens-lessons`.

Impact:

- reinforces uncertainty about which lifecycle is authoritative
- increases the chance of future accidental reuse or mis-registration
- makes inventory-based tooling less reliable

Evidence:

- present on disk in `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/skills/`
- not listed in `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/module.yaml:32-157`

Assessment:

- not necessarily broken by themselves, but they are strong evidence of incomplete migration cleanup

## Validated Good Areas

The audit also confirmed a set of areas that are currently coherent and should not be treated as suspect by default.

### Current v4 phase skills are present and registered

These active phase / lifecycle skills exist and are declared in `module.yaml`:

- `bmad-lens-preplan`
- `bmad-lens-businessplan`
- `bmad-lens-techplan`
- `bmad-lens-finalizeplan`
- `bmad-lens-expressplan`
- `bmad-lens-adversarial-review`
- `bmad-lens-dev`
- `bmad-lens-complete`
- `bmad-lens-retrospective`

Evidence:

- `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/module.yaml:108-157`

### Most active operational prompts resolve to real skills

The following prompt families were traced and did not show hard breakage in this pass:

- `preplan`
- `businessplan`
- `techplan`
- `adversarial-review`
- `finalizeplan`
- `dev`
- `complete`
- `retrospective`
- `status`
- `next`
- `batch`
- `switch`
- `constitution`
- `audit`
- `approval-status`
- `help`
- `module-management`
- `rollback`
- `pause-resume`
- `profile`
- `dashboard`
- `feature-yaml`
- `git-state`
- `git-orchestration`
- `migrate`
- `theme`

### BMAD wrapper prompts are mostly structurally sound

The BMAD wrapper prompt family generally resolves either through `bmad-lens-bmad-skill` or through a dedicated wrapper such as `bmad-lens-document-project`. The problems here are mainly adapter / packaging drift rather than broken downstream skill mappings.

Evidence:

- `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/skills/bmad-lens-bmad-skill/SKILL.md`
- `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/assets/lens-bmad-skill-registry.json`
- `TargetProjects/lens.core/src/Lens.Core.Src/_bmad/lens-work/skills/bmad-lens-document-project/SKILL.md`

## Root Cause Pattern

The findings cluster into one dominant pattern:

### Incomplete migration from the older staged planning model to the v4 `FinalizePlan` model

That single migration gap explains most of the observed defects:

- legacy GitHub prompt stubs still published
- current prompt names partially introduced but not fully created (`quickplan`)
- docs still teaching the older lifecycle
- legacy skill directories still present
- adapter and installer inventories diverged instead of being regenerated from one authoritative source

This looks less like random breakage and more like a partially completed migration that touched the lifecycle contract and some manifests but did not finish the prompt, adapter, installer, and documentation surfaces.

## Recommended Remediation Sequence

### Priority 0: Fix the dead prompt surfaces

1. Resolve `lens-quickplan` decisively.
   - Either create `_bmad/lens-work/prompts/lens-quickplan.prompt.md`
   - Or remove `lens-quickplan` from current prompt and adapter inventories
2. Remove or replace dead `.github` prompt stubs:
   - `lens-devproposal.prompt.md`
   - `lens-sprintplan.prompt.md`
3. Repair `lens-onboard.prompt.md` so it is syntactically valid and clearly deprecated.

### Priority 1: Unify prompt inventory generation

1. Make one surface authoritative for prompt inventory.
2. Regenerate or refactor:
   - `module.yaml` prompt list
   - `_module-installer/installer.js` stub list
   - `scripts/install.py` stub list
   - `.github/prompts` checked-in layer
3. Decide whether `scripts/install.py` remains supported. If yes, bring it to full parity.

### Priority 2: Finish registration and filtering cleanup

1. Register `bmad-lens-discover` in `module.yaml` if it is still active.
2. Reconcile `_PROMPT_METADATA` with the live prompt catalog.
3. Decide whether `lens-preflight` should gain a proper skill wrapper.

### Priority 3: Remove or archive legacy v3 lifecycle surfaces

1. Audit and either delete or archive:
   - `bmad-lens-devproposal`
   - `bmad-lens-sprintplan`
   - `bmad-lens-lessons`
2. Remove stale docs and prompt examples that present those phases as current.

### Priority 4: Rewrite lifecycle-facing documentation against `lifecycle.yaml`

At minimum, refresh:

- `architecture.md`
- `lifecycle-reference.md`
- `lifecycle-visual-guide.md`
- `component-inventory.md`
- `copilot-adapter-reference.md`

The rule should be simple: those docs must reflect `lifecycle.yaml` v4, not historical v3 behavior.

## Suggested Follow-Up Workstream

The cleanest follow-up is a two-pass repair:

1. **Prompt and installer repair pass**
   - restore or remove dead entrypoints
   - unify inventories
   - revalidate `.github/prompts`
2. **Documentation and legacy cleanup pass**
   - rewrite v3 lifecycle references
   - archive or delete unregistered legacy skill surfaces
   - rerun a shorter prompt audit after cleanup

## Closing Assessment

`lens-work` currently has a functioning v4 execution core, but its surrounding prompt, adapter, installer, and documentation surfaces still expose a mixed v3/v4 state. The fastest path to stability is to treat this as a migration-completion problem, not a collection of unrelated defects.

If the dead prompt surfaces and installer drift are fixed first, the remaining work becomes much easier to reason about and validate.