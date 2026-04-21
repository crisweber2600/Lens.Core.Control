# lens-upgrade Prompt Audit

## Purpose Summary
- .github/prompts/lens-upgrade.prompt.md is a control-layer stub that runs shared lightweight preflight and then delegates to the release prompt.
- lens.core/_bmad/lens-work/prompts/lens-upgrade.prompt.md is also a stub and delegates directly to bmad-lens-upgrade.
- The operational contract is in lens.core/_bmad/lens-work/skills/bmad-lens-upgrade/SKILL.md: detect source/target schema, load lifecycle migration descriptors, show a migration plan, require confirmation for writes, support dry-run, and separate schema migration from legacy branch migration.
- The skill explicitly routes schema-current but topology-legacy states to /lens-migrate scan rather than claiming migration is complete.

## BMAD Skill Mapping
- Global BMAD registries do not expose Lens upgrade as a canonical BMAD command surface:
  - lens.core/_bmad/_config/skill-manifest.csv has no bmad-lens-upgrade or bmad-lens-migrate entry.
  - lens.core/_bmad/_config/bmad-help.csv has no lens-upgrade or lens-migrate command row.
- Lens-local module surfaces do map upgrade explicitly:
  - lens.core/_bmad/lens-work/module.yaml registers bmad-lens-upgrade and bmad-lens-migrate as Lens skills.
  - lens.core/_bmad/lens-work/module.yaml ships lens-upgrade.prompt.md and .github/prompts/lens-upgrade.prompt.md adapter stubs.
  - lens.core/_bmad/lens-work/module-help.csv exposes UG (skill bmad-lens-upgrade, action upgrade, args [--dry-run], outputs migration results).
- Mapping inconsistency:
  - module-help.csv exposes only [--dry-run] for UG.
  - bmad-lens-upgrade/SKILL.md documents --from and --to in addition to --dry-run, and requires legacy-branch checks via /lens-migrate scan before declaring migration complete.

## Lifecycle Fit
- Lifecycle alignment is strong for schema migration:
  - lifecycle.yaml sets schema_version: 4 and defines a migrations section with explicit migration_command entries that route through /lens-upgrade.
  - Migration descriptors include transform-level changes (rename fields/keys, add/remove sections, path/value rewrites), matching the upgrade skill's plan/apply framing.
- Upgrade is correctly modeled as anytime operational infrastructure, not a phase conductor:
  - module-help.csv marks upgrade as anytime.
  - lifecycle phase_order remains preplan -> businessplan -> techplan -> finalizeplan; upgrade is not part of phase progression.
- Important lifecycle boundary captured by the skill:
  - bmad-lens-upgrade treats schema parity and branch-topology parity as different checks and hands legacy topology work to /lens-migrate.
- Fit risk:
  - lifecycle.yaml migration descriptors route to /lens-upgrade, but lifecycle.yaml does not itself encode the required follow-on /lens-migrate check for schema-current legacy branches; that behavior exists only in the upgrade skill contract.

## Evidence Refs
- Prompt chain
  - .github/prompts/lens-upgrade.prompt.md:2
  - .github/prompts/lens-upgrade.prompt.md:5
  - .github/prompts/lens-upgrade.prompt.md:18
  - lens.core/_bmad/lens-work/prompts/lens-upgrade.prompt.md:2
  - lens.core/_bmad/lens-work/prompts/lens-upgrade.prompt.md:4
- Upgrade skill contract
  - lens.core/_bmad/lens-work/skills/bmad-lens-upgrade/SKILL.md:2
  - lens.core/_bmad/lens-work/skills/bmad-lens-upgrade/SKILL.md:10
  - lens.core/_bmad/lens-work/skills/bmad-lens-upgrade/SKILL.md:15
  - lens.core/_bmad/lens-work/skills/bmad-lens-upgrade/SKILL.md:42
  - lens.core/_bmad/lens-work/skills/bmad-lens-upgrade/SKILL.md:51
  - lens.core/_bmad/lens-work/skills/bmad-lens-upgrade/SKILL.md:52
  - lens.core/_bmad/lens-work/skills/bmad-lens-upgrade/SKILL.md:83
- Required registry/module/lifecycle sources
  - lens.core/_bmad/_config/skill-manifest.csv (searched: no bmad-lens-upgrade or bmad-lens-migrate entry)
  - lens.core/_bmad/_config/bmad-help.csv (searched: no lens-upgrade or lens-migrate row)
  - lens.core/_bmad/lens-work/module.yaml:22
  - lens.core/_bmad/lens-work/module.yaml:97
  - lens.core/_bmad/lens-work/module.yaml:98
  - lens.core/_bmad/lens-work/module.yaml:148
  - lens.core/_bmad/lens-work/module.yaml:149
  - lens.core/_bmad/lens-work/module.yaml:256
  - lens.core/_bmad/lens-work/module.yaml:321
  - lens.core/_bmad/lens-work/module-help.csv:75
  - lens.core/_bmad/lens-work/lifecycle.yaml:20
  - lens.core/_bmad/lens-work/lifecycle.yaml:850
  - lens.core/_bmad/lens-work/lifecycle.yaml:854
  - lens.core/_bmad/lens-work/lifecycle.yaml:897
  - lens.core/_bmad/lens-work/lifecycle.yaml:940
  - lens.core/_bmad/lens-work/lifecycle.yaml:954
  - lens.core/_bmad/lens-work/lifecycle.yaml:984
  - lens.core/_bmad/lens-work/lifecycle.yaml:1044

## Confidence
- Overall: High.
- Rationale: the prompt chain, module registration, module-help exposure, and lifecycle migration contract are explicit and consistent about upgrade intent and routing.
- Residual uncertainty: Medium for executable behavior validation in this payload because lens.core/_bmad/lens-work/skills/bmad-lens-upgrade currently contains SKILL.md only (no colocated scripts directory to inspect in this tree).

## Gaps
- Double-stub indirection reduces prompt-local transparency. Both control and release prompt files are wrappers, so core behavior is only visible in the skill.
- Discoverability is split across registries. Upgrade is present in Lens-local module surfaces but absent in global _bmad/_config registries, which can mislead global catalog users.
- Help-to-skill argument drift exists:
  - module-help.csv advertises UG as upgrade [--dry-run].
  - bmad-lens-upgrade/SKILL.md documents --dry-run, --from, and --to.
- Lifecycle contract incompleteness for migration parity checks:
  - lifecycle.yaml encodes schema migrations via /lens-upgrade.
  - mandatory legacy-branch follow-up when schema is already current is only stated in bmad-lens-upgrade/SKILL.md.
- Runtime auditability gap in this distribution snapshot: bmad-lens-upgrade is skill-contract only (no colocated script artifacts under the skill path), reducing direct verification of implementation-vs-contract fidelity.