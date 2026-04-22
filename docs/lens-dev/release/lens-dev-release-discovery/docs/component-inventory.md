# lens.core.release — Component Inventory

**Feature:** lens-dev-release-discovery
**Generated:** 2026-05-20

---

## Skills — `_bmad/lens-work/skills/` (41 total)

### Planning Conductors

| Skill | Command | Description |
| --- | --- | --- |
| `bmad-lens-preplan` | `/lens-preplan` | Pre-planning phase — feature scoping, stakeholder alignment, problem statement |
| `bmad-lens-businessplan` | `/lens-businessplan` | Business planning — objectives, user journeys, success metrics |
| `bmad-lens-techplan` | `/lens-techplan` | Technical planning — architecture decisions, implementation approach |
| `bmad-lens-adversarial-review` | `/lens-adversarial-review` | Red-team review of planning artifacts before finalization |
| `bmad-lens-finalizeplan` | `/lens-finalizeplan` | FinalizePlan phase — collapse DevProposal + SprintPlan into final handoff |
| `bmad-lens-expressplan` | `/lens-expressplan` | Express planning track — rapid business + tech plan in one session |
| `bmad-lens-quickplan` | `/lens-quickplan` | End-to-end planning pipeline conductor |
| `bmad-lens-devproposal` | `/lens-devproposal` | Dev proposal generation for a finalized plan |

### Execution Skills

| Skill | Command | Description |
| --- | --- | --- |
| `bmad-lens-dev` | `/lens-dev` | Development phase conductor — story execution handoff |
| `bmad-lens-complete` | `/lens-complete` | Feature completion gating — validate artifacts, PR to main |

### Lifecycle Utilities

| Skill | Command | Description |
| --- | --- | --- |
| `bmad-lens-init-feature` | `/lens-init-feature` / `/lens-new-feature` | Feature initialization: 2-branch topology, feature.yaml, governance PR |
| `bmad-lens-target-repo` | `/lens-target-repo` | Provision and register feature target repos |
| `bmad-lens-next` | `/lens-next` | Context-aware next-action routing |
| `bmad-lens-batch` | `/lens-batch` | Batch multi-feature operations |
| `bmad-lens-switch` | `/lens-switch` | Switch active feature context |
| `bmad-lens-pause-resume` | `/lens-pause-resume` | Pause or resume a feature |
| `bmad-lens-retrospective` | `/lens-retrospective` | Feature retrospective |
| `bmad-lens-move-feature` | `/lens-move-feature` | Move a feature to a different domain/service |
| `bmad-lens-split-feature` | `/lens-split-feature` | Split one feature into multiple |
| `bmad-lens-lessons` | `/lens-lessons` | Capture and surface lessons learned |
| `bmad-lens-discover` | `/lens-discover` | Repo inventory discovery and sync |

### Governance and Reporting

| Skill | Command | Description |
| --- | --- | --- |
| `bmad-lens-constitution` | `/lens-constitution` | Governance rules resolution — 4-level additive hierarchy |
| `bmad-lens-sensing` | `/lens-sensing` | Cross-initiative sensing — detect conflicts and dependencies |
| `bmad-lens-audit` | `/lens-audit` | Feature and initiative audit |
| `bmad-lens-dashboard` | `/lens-dashboard` | Initiative dashboard overview |
| `bmad-lens-approval-status` | `/lens-approval-status` | PR/approval gate status |
| `bmad-lens-rollback` | `/lens-rollback` | Phase rollback |
| `bmad-lens-profile` | `/lens-profile` | User profile and PAT management |
| `bmad-lens-theme` | `/lens-theme` | Theme and persona overlay system |
| `bmad-lens-log-problem` | `/lens-log-problem` | Problem capture and logging |

### Setup and Migration

| Skill | Command | Description |
| --- | --- | --- |
| `bmad-lens-migrate` | `/lens-migrate` | Schema migration (dry-run/apply/verify) |
| `bmad-lens-upgrade` | `/lens-upgrade` | Lifecycle schema upgrade |
| `bmad-lens-document-project` | `/lens-bmad-document-project` | Project documentation (this skill!) |
| `bmad-lens-module-management` | `/lens-module-management` | Module installation management |
| `bmad-lens-onboard` | `/lens-onboard` | Deprecated onboard bridge (retained for compatibility) |

### Foundation / Low-Level

| Skill | Command | Description |
| --- | --- | --- |
| `bmad-lens-git-state` | `/lens-git-state` | Read-only git queries for 2-branch feature model |
| `bmad-lens-git-orchestration` | `/lens-git-orchestration` | Git write operations for 2-branch topology |
| `bmad-lens-feature-yaml` | `/lens-feature-yaml` | Feature YAML lifecycle operations and validation |
| `bmad-lens-help` | `/lens-help` | Command help and discovery |
| `bmad-lens-bmad-skill` | `/lens-bmad-skill` | BMAD skill invocation wrapper |
| `bmad-lens-sprintplan` | `/lens-sprintplan` | Sprint planning |

---

## Prompts — `_bmad/lens-work/prompts/` (57 total)

### Native Lens Commands (41 prompt files)
One-to-one mapping with the 41 lens-work skills:
`lens-adversarial-review.prompt.md`, `lens-approval-status.prompt.md`, `lens-audit.prompt.md`, `lens-batch.prompt.md`, `lens-businessplan.prompt.md`, `lens-checklist.prompt.md`, `lens-complete.prompt.md`, `lens-constitution.prompt.md`, `lens-dashboard.prompt.md`, `lens-dev.prompt.md`, `lens-discover.prompt.md`, `lens-expressplan.prompt.md`, `lens-feature-yaml.prompt.md`, `lens-finalizeplan.prompt.md`, `lens-git-orchestration.prompt.md`, `lens-git-state.prompt.md`, `lens-help.prompt.md`, `lens-init-feature.prompt.md`, `lens-log-problem.prompt.md`, `lens-migrate.prompt.md`, `lens-module-management.prompt.md`, `lens-move-feature.prompt.md`, `lens-next.prompt.md`, `lens-onboard.prompt.md`, `lens-pause-resume.prompt.md`, `lens-preflight.prompt.md`, `lens-preplan.prompt.md`, `lens-quickplan.prompt.md`, `lens-retrospective.prompt.md`, `lens-rollback.prompt.md`, `lens-sensing.prompt.md`, `lens-setup.prompt.md`, `lens-split-feature.prompt.md`, `lens-sprintplan.prompt.md`, `lens-switch.prompt.md`, `lens-target-repo.prompt.md`, `lens-techplan.prompt.md`, `lens-theme.prompt.md`, `lens-upgrade.prompt.md`, `lens-devproposal.prompt.md`, `lens-profile.prompt.md`

### Convenience Aliases (3 prompt files)
`lens-new-domain.prompt.md`, `lens-new-feature.prompt.md`, `lens-new-project.prompt.md`, `lens-new-service.prompt.md`

### BMAD Bridge Prompts (13 prompt files)
Lens-wrapped invocations of BMAD methodology skills:
`lens-bmad-brainstorming.prompt.md`, `lens-bmad-check-implementation-readiness.prompt.md`, `lens-bmad-code-review.prompt.md`, `lens-bmad-create-architecture.prompt.md`, `lens-bmad-create-epics-and-stories.prompt.md`, `lens-bmad-create-prd.prompt.md`, `lens-bmad-create-story.prompt.md`, `lens-bmad-create-ux-design.prompt.md`, `lens-bmad-document-project.prompt.md`, `lens-bmad-domain-research.prompt.md`, `lens-bmad-market-research.prompt.md`, `lens-bmad-product-brief.prompt.md`, `lens-bmad-quick-dev.prompt.md`, `lens-bmad-sprint-planning.prompt.md`, `lens-bmad-technical-research.prompt.md`

---

## Scripts — `_bmad/lens-work/scripts/` (19 items)

### Install and Bootstrap

| Script | Purpose | Usage |
| --- | --- | --- |
| `install.py` | Copy module files into a control repo; generate IDE adapters | `uv run _bmad/lens-work/scripts/install.py [--dry-run]` |
| `setup-control-repo.py` | Bootstrap new control repos — clone module, config, governance | `uv run _bmad/lens-work/scripts/setup-control-repo.py` |
| `bootstrap-target-projects.py` | Scaffold TargetProjects/ for new repos | `uv run _bmad/lens-work/scripts/bootstrap-target-projects.py` |
| `store-github-pat.py` | Setup GitHub PAT in environment (run outside AI chat) | `uv run _bmad/lens-work/scripts/store-github-pat.py` |

### Preflight and Validation

| Script | Purpose | Usage |
| --- | --- | --- |
| `preflight.py` | Full preflight: lifecycle.yaml, feature state, registry | `uv run _bmad/lens-work/scripts/preflight.py` |
| `light-preflight.py` | Fast preflight: environment + feature context check | Called by prompts automatically |
| `run-preflight-cached.py` | Preflight with caching to avoid redundant checks | Called internally |
| `validate-lens-bmad-registry.py` | Validate skill/prompt registry consistency | `uv run _bmad/lens-work/scripts/validate-lens-bmad-registry.py` |
| `validate-lens-core-parity.sh` | Check source/release parity | `bash _bmad/lens-work/scripts/validate-lens-core-parity.sh` |
| `validate-phase-artifacts.py` | Validate phase transition artifacts are present | `uv run _bmad/lens-work/scripts/validate-phase-artifacts.py` |
| `validate-feature-move.py` | Validate feature move/rename operations | `uv run _bmad/lens-work/scripts/validate-feature-move.py` |

### State and Discovery

| Script | Purpose |
| --- | --- |
| `derive-next-action.py` | Compute next recommended action from git state |
| `derive-initiative-status.py` | Compute initiative status from git + artifacts |
| `scan-active-initiatives.py` | Cross-repo active initiative scanning |
| `load-command-registry.py` | Load module-help.csv into structured form |
| `plan-lifecycle-renames.py` | Compute rename plan for lifecycle schema migrations |

### PR Helpers

| Script | Purpose |
| --- | --- |
| `create-pr.py` | GitHub REST API PR creation (no `gh` CLI required) |

---

## Script Tests — `_bmad/lens-work/scripts/tests/` (10 Python + README)

| Test File | What It Tests |
| --- | --- |
| `test-install.py` | Adapter generation and install flow |
| `test-preflight.py` | Full preflight validation |
| `test-light-preflight.py` | Fast preflight check |
| `test-setup-control-repo.py` | Control repo bootstrap and governance repo naming |
| `test-create-pr.py` | PR creation via REST API |
| `test-store-github-pat.py` | PAT storage |
| `test-validate-phase-artifacts.py` | Phase artifact presence validation |
| `test-adversarial-review-contracts.py` | Adversarial review behavioral contracts |
| `test-batch-mode-contracts.py` | Batch mode behavioral contracts |
| `test-phase-conductor-contracts.py` | Phase conductor behavioral contracts |

---

## Contract Tests — `_bmad/lens-work/tests/contracts/` (8 files)

Declarative behavioral specification files that document expected invariants:

| Contract File | Domain |
| --- | --- |
| `branch-parsing.md` | Branch name parsing rules |
| `governance.md` | Constitutional governance invariants |
| `sensing.md` | Cross-initiative sensing behavior |
| `two-branch-topology.md` | 2-branch (feature + plan) topology rules |
| + 4 more | (additional invariants) |

---

## Assets — `_bmad/lens-work/assets/`

| Asset | Purpose |
| --- | --- |
| `lens-bmad-skill-registry.json` | Machine-readable registry of all BMAD skills with metadata |
| `templates/` | Reusable templates for skill/agent/prompt scaffolding |

---

## IDE Adapters

### `.github/` (GitHub Copilot)

| File/Dir | Purpose |
| --- | --- |
| `agents/bmad-agent-lens-work-lens.agent.md` | Copilot agent definition |
| `instructions/lens-control-repo.instructions.md` | Always-on context instructions |
| `lens-work-instructions.md` | Module-level rules |
| `prompts/` | 57 prompt stubs (thin wrappers over `_bmad/lens-work/prompts/`) |
| `skills/` | BMAD skill stubs |

### AGENTS.md (Codex)
Root-level Codex activation file. Points to module path, agent definition, lifecycle contract, and command reference.

---

## Bundled BMAD Modules Summary

| Module | Path | Contents |
| --- | --- | --- |
| bmb | `_bmad/bmb/` | bmad-agent-builder (agent+workflow builder), bmad-workflow-builder |
| bmm | `_bmad/bmm/` | 4 phases: 1-analysis, 2-plan-workflows, 3-solutioning, 4-implementation |
| cis | `_bmad/cis/` | Creative Innovation Studio skills |
| core | `_bmad/core/` | Foundation BMAD skills |
| gds | `_bmad/gds/` | Game Design System agents and workflows |
| tea | `_bmad/tea/` | Test Engineering Agent workflows |
| wds | `_bmad/wds/` | Web Design System skills and workflows |

---

_Generated by lens-bmad-document-project for feature lens-dev-release-discovery._
