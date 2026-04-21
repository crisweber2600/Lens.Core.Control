# lens-migrate Prompt Audit

## Purpose Summary
- `.github/prompts/lens-migrate.prompt.md` is a control-repo stub that only delegates into the release-layer prompt.
- The release prompt at `lens.core/_bmad/lens-work/prompts/lens-migrate.prompt.md` is also a stub and delegates directly to `bmad-lens-migrate/SKILL.md`.
- The substantive contract therefore lives in `bmad-lens-migrate/SKILL.md`, which defines `lens-migrate` as a governance-first bridge for moving legacy LENS v3 branch families into the Lens Next two-branch model without losing in-progress work.
- The skill expands the user-facing purpose beyond simple branch renaming: it requires dry-run before writes, mirrors discovered legacy documents into the control repo as migration proof, verifies migrated artifacts before cleanup, and preserves old branches until an explicit cleanup step.

## BMAD Skill Mapping
- There is no canonical entry for `bmad-lens-migrate` in the global BMAD registries searched in `lens.core/_bmad/_config/skill-manifest.csv` and `lens.core/_bmad/_config/bmad-help.csv`; the mapping is Lens-module-specific rather than part of the shared BMAD catalog.
- `lens.core/_bmad/lens-work/module.yaml` registers `bmad-lens-migrate` as a Lens skill and ships both the release prompt and the `.github/` stub prompt.
- `lens.core/_bmad/lens-work/module-help.csv` exposes three public migrate operations as anytime commands:
  - `SC` for scan
  - `MI` for migrate-feature
  - `CC` for check-conflicts
- `lens.core/_bmad/lens-work/skills/bmad-lens-upgrade/SKILL.md` treats `bmad-lens-migrate` as the legacy-branch migration route when schema version is already current but old branches still exist.
- The main mapping gap is interface drift between discovery surfaces and the executable contract:
  - `module-help.csv` advertises `--governance-dir` and a simplified `migrate-feature <featureId> [--governance-dir] [--dry-run]` shape.
  - The skill and `migrate-ops.py` require `--governance-repo`, plus `--old-id`, `--feature-id`, `--domain`, and `--service`, with optional `--username`, `--source-repo`, and `--control-repo`.
- Additional executable operations exist below the help surface:
  - `verify`
  - `cleanup`
  - `normalize-feature-ids`
  - `fix-feature-dirs`
  These are present in the script, but only `verify` and `cleanup` are surfaced in the skill text, and none of them appear in `module-help.csv`.

## Lifecycle Fit
- `lens-migrate` is not a lifecycle phase conductor. `lifecycle.yaml` keeps `phase_order` to `preplan`, `businessplan`, `techplan`, and `finalizeplan`, with schema migrations routed through `/lens-upgrade` rather than `/lens-migrate`.
- The fit is therefore adjacent to the lifecycle engine, not inside it:
  - `/lens-upgrade` owns schema-version migration descriptors from `lifecycle.yaml`.
  - `/lens-migrate` owns legacy branch topology migration when the schema may already be current.
- This split is made explicit by `bmad-lens-upgrade/SKILL.md`, which says version parity is not migration parity and requires `/lens-migrate scan` before declaring a workspace fully migrated.
- Operationally, `lens-migrate` behaves like an anytime remediation and normalization utility that supports lifecycle adoption, module upgrades, and post-upgrade cleanup rather than a step in the normal feature planning/execution chain.
- The lifecycle-fit risk is discoverability: the core lifecycle contract does not mention `lens-migrate` directly, so a user reading only `lifecycle.yaml` would learn about schema upgrades but not necessarily about the separate legacy-branch migration path.

## Evidence Refs
- Control prompt stub:
  - `.github/prompts/lens-migrate.prompt.md:2`
  - `.github/prompts/lens-migrate.prompt.md:5`
  - `.github/prompts/lens-migrate.prompt.md:10`
- Release prompt stub:
  - `lens.core/_bmad/lens-work/prompts/lens-migrate.prompt.md:2`
  - `lens.core/_bmad/lens-work/prompts/lens-migrate.prompt.md:5`
  - `lens.core/_bmad/lens-work/prompts/lens-migrate.prompt.md:10`
- Lens module registration and shipped prompt surfaces:
  - `lens.core/_bmad/lens-work/module.yaml:97`
  - `lens.core/_bmad/lens-work/module.yaml:98`
  - `lens.core/_bmad/lens-work/module.yaml:148`
  - `lens.core/_bmad/lens-work/module.yaml:149`
  - `lens.core/_bmad/lens-work/module.yaml:265`
  - `lens.core/_bmad/lens-work/module.yaml:330`
- Lens module-help command mapping:
  - `lens.core/_bmad/lens-work/module-help.csv:41`
  - `lens.core/_bmad/lens-work/module-help.csv:42`
  - `lens.core/_bmad/lens-work/module-help.csv:43`
  - `lens.core/_bmad/lens-work/module-help.csv:73`
  - `lens.core/_bmad/lens-work/module-help.csv:75`
- Lifecycle and upgrade routing context:
  - `lens.core/_bmad/lens-work/lifecycle.yaml:238`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:810`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:854`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:897`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:940`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:954`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:984`
  - `lens.core/_bmad/lens-work/lifecycle.yaml:1044`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-upgrade/SKILL.md:42`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-upgrade/SKILL.md:51`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-upgrade/SKILL.md:52`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-upgrade/SKILL.md:83`
- Migrate skill contract:
  - `lens.core/_bmad/lens-work/skills/bmad-lens-migrate/SKILL.md:3`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-migrate/SKILL.md:12`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-migrate/SKILL.md:14`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-migrate/SKILL.md:16`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-migrate/SKILL.md:86`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-migrate/SKILL.md:91`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-migrate/SKILL.md:94`
- Executable CLI contract:
  - `lens.core/_bmad/lens-work/skills/bmad-lens-migrate/scripts/migrate-ops.py:2652`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-migrate/scripts/migrate-ops.py:2653`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-migrate/scripts/migrate-ops.py:2658`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-migrate/scripts/migrate-ops.py:2659`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-migrate/scripts/migrate-ops.py:2669`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-migrate/scripts/migrate-ops.py:2675`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-migrate/scripts/migrate-ops.py:2683`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-migrate/scripts/migrate-ops.py:2697`
  - `lens.core/_bmad/lens-work/skills/bmad-lens-migrate/scripts/migrate-ops.py:2705`
- Global BMAD registry context checked for canonical mapping:
  - `lens.core/_bmad/_config/skill-manifest.csv` searched: no `bmad-lens-migrate` entry found
  - `lens.core/_bmad/_config/bmad-help.csv` searched: no `bmad-lens-migrate` or `lens-migrate` entry found

## Confidence
- Overall: **High** for purpose, module mapping, and lifecycle positioning.
- Rationale: the prompt chain is simple, and the module-help, skill, upgrade, lifecycle, and script surfaces consistently show that `lens-migrate` is an anytime legacy-topology migration utility rather than a normal phase workflow.
- Residual uncertainty: **Medium** for operator-facing invocation shape because prompt/help surfaces and the underlying script do not fully agree on arguments and exposed subcommands.

## Gaps
- Prompt transparency is low. Both prompt layers are stubs, so governance-first dossier behavior, verification requirements, and cleanup proof rules are invisible unless the user drills into the skill.
- Global BMAD discoverability is missing. Because `bmad-lens-migrate` does not appear in `skill-manifest.csv` or `bmad-help.csv`, users relying on the shared BMAD registry will not find it.
- Help-to-CLI drift is material:
  - `module-help.csv` uses `--governance-dir`, but the script requires `--governance-repo`.
  - `module-help.csv` implies `migrate-feature` can run from a lone `<featureId>`, but the script requires legacy identity and scope inputs.
- Capability exposure is incomplete. The skill surfaces `scan`, dry-run behavior, migration, verification, and cleanup, but `module-help.csv` omits `verify` and `cleanup`, and neither prompt nor skill explains the script-only `normalize-feature-ids` and `fix-feature-dirs` maintenance commands.
- Lifecycle adjacency is implicit rather than explicit. `lifecycle.yaml` only encodes schema migrations through `/lens-upgrade`, so the separate need for `/lens-migrate` must be learned from `bmad-lens-upgrade/SKILL.md` or module help, not from the lifecycle contract itself.