# lens.core.src — Component Inventory

**Feature:** lens-dev-old-codebase-discovery
**Generated:** 2026-04-21

> **Source note:** The component inventory for `lens.core.src` is identical to the release distribution in terms of skill commands and capabilities. This is the authoritative source — the canonical `SKILL.md` files live here. The release bundles the same 41 skills with the same 57 prompts; they originate from here.

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
| `bmad-lens-document-project` | `/lens-bmad-document-project` | Project documentation |
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

## Operational Scripts — `_bmad/lens-work/scripts/` (19 total)

| Script | Purpose |
| --- | --- |
| `install.py` | IDE adapter generator — installs adapters into consumer control repos |
| `setup-control-repo.py` | Bootstrap new control repo — clone governance, install adapters, scaffold workspace |
| `preflight.py` | System and workspace health checks — git version, Python version, schema version, governance connectivity |
| `run-preflight-cached.py` | Cached preflight — skips redundant checks within a session |
| `validate-lens-bmad-registry.py` | Registry consistency: verifies module.yaml skills/prompts match actual files on disk |
| `validate-phase-artifacts.py` | Phase artifact structure and content validator — required sections, word count, cross-references |
| `create-pr.py` | GitHub REST API pull request creation with fallback manual URL |
| `complete-ops.py` | Feature completion and archival — atomic governance commit |
| `quickplan-ops.py` | Quickplan pipeline conductor — runs businessplan + techplan + finalizeplan sequentially |
| `switch-ops.py` | Feature context switch — loads feature.yaml + governance context |
| `pause-resume-ops.py` | Feature pause and resume — stores and restores paused_from_phase |
| `log-problem-ops.py` | Problem capture — appends to problems.md + increments open_problems counter |
| `dashboard-ops.py` | Dashboard generation — produces self-contained HTML from feature-index.yaml |
| `install-dep.py` | Optional BMAD dependency installation helper |
| `update-manifest.py` | Module manifest update utility |
| `validate-lens-core-parity.sh` | CI parity check — verifies source and release are in sync |

---

## Prompts — `_bmad/lens-work/prompts/` (57 total)

All prompts follow the two-step pattern:
1. Run `light-preflight.py` (workspace health check)
2. Load `SKILL.md` and follow instructions

| Prompt | Skill |
| --- | --- |
| `lens-adversarial-review.prompt.md` | `bmad-lens-adversarial-review` |
| `lens-approval-status.prompt.md` | `bmad-lens-approval-status` |
| `lens-audit.prompt.md` | `bmad-lens-audit` |
| `lens-batch.prompt.md` | `bmad-lens-batch` |
| `lens-bmad-brainstorming.prompt.md` | `bmad-lens-bmad-skill` → bmad-brainstorming |
| `lens-bmad-check-implementation-readiness.prompt.md` | `bmad-lens-bmad-skill` → bmad-check-implementation-readiness |
| `lens-bmad-code-review.prompt.md` | `bmad-lens-bmad-skill` → bmad-code-review |
| `lens-bmad-create-architecture.prompt.md` | `bmad-lens-bmad-skill` → bmad-create-architecture |
| `lens-bmad-create-epics-and-stories.prompt.md` | `bmad-lens-bmad-skill` → bmad-create-epics-and-stories |
| `lens-bmad-create-prd.prompt.md` | `bmad-lens-bmad-skill` → bmad-create-prd |
| `lens-bmad-create-story.prompt.md` | `bmad-lens-bmad-skill` → bmad-create-story |
| `lens-bmad-create-ux-design.prompt.md` | `bmad-lens-bmad-skill` → bmad-create-ux-design |
| `lens-bmad-document-project.prompt.md` | `bmad-lens-document-project` |
| `lens-bmad-domain-research.prompt.md` | `bmad-lens-bmad-skill` → bmad-domain-research |
| `lens-bmad-market-research.prompt.md` | `bmad-lens-bmad-skill` → bmad-market-research |
| `lens-bmad-product-brief.prompt.md` | `bmad-lens-bmad-skill` → bmad-product-brief |
| `lens-bmad-quick-dev.prompt.md` | `bmad-lens-bmad-skill` → bmad-quick-dev |
| `lens-bmad-sprint-planning.prompt.md` | `bmad-lens-bmad-skill` → bmad-sprint-planning |
| `lens-bmad-technical-research.prompt.md` | `bmad-lens-bmad-skill` → bmad-technical-research |
| `lens-businessplan.prompt.md` | `bmad-lens-businessplan` |
| `lens-checklist.prompt.md` | `bmad-lens-checklist` |
| `lens-complete.prompt.md` | `bmad-lens-complete` |
| `lens-constitution.prompt.md` | `bmad-lens-constitution` |
| `lens-dashboard.prompt.md` | `bmad-lens-dashboard` |
| `lens-dev.prompt.md` | `bmad-lens-dev` |
| `lens-discover.prompt.md` | `bmad-lens-discover` |
| `lens-expressplan.prompt.md` | `bmad-lens-expressplan` |
| `lens-feature-yaml.prompt.md` | `bmad-lens-feature-yaml` |
| `lens-finalizeplan.prompt.md` | `bmad-lens-finalizeplan` |
| `lens-git-orchestration.prompt.md` | `bmad-lens-git-orchestration` |
| `lens-git-state.prompt.md` | `bmad-lens-git-state` |
| `lens-help.prompt.md` | `bmad-lens-help` |
| `lens-init-feature.prompt.md` | `bmad-lens-init-feature` |
| `lens-log-problem.prompt.md` | `bmad-lens-log-problem` |
| `lens-migrate.prompt.md` | `bmad-lens-migrate` |
| `lens-module-management.prompt.md` | `bmad-lens-module-management` |
| `lens-move-feature.prompt.md` | `bmad-lens-move-feature` |
| `lens-new-domain.prompt.md` | `bmad-lens-init-feature` (domain alias) |
| `lens-new-feature.prompt.md` | `bmad-lens-init-feature` (feature alias) |
| `lens-new-project.prompt.md` | `bmad-lens-init-feature` (project alias) |
| `lens-new-service.prompt.md` | `bmad-lens-init-feature` (service alias) |
| `lens-next.prompt.md` | `bmad-lens-next` |
| `lens-onboard.prompt.md` | `bmad-lens-onboard` |
| `lens-pause-resume.prompt.md` | `bmad-lens-pause-resume` |
| `lens-preflight.prompt.md` | `preflight.py` |
| `lens-preplan.prompt.md` | `bmad-lens-preplan` |
| `lens-quickplan.prompt.md` | `bmad-lens-quickplan` |
| `lens-retrospective.prompt.md` | `bmad-lens-retrospective` |
| `lens-rollback.prompt.md` | `bmad-lens-rollback` |
| `lens-rollback-dev.prompt.md` | `bmad-lens-rollback` (dev alias) |
| `lens-sensing.prompt.md` | `bmad-lens-sensing` |
| `lens-setup-control-repo.prompt.md` | `setup-control-repo.py` |
| `lens-sprintplan.prompt.md` | `bmad-lens-sprintplan` |
| `lens-switch.prompt.md` | `bmad-lens-switch` |
| `lens-target-repo.prompt.md` | `bmad-lens-target-repo` |
| `lens-techplan.prompt.md` | `bmad-lens-techplan` |
| `lens-theme.prompt.md` | `bmad-lens-theme` |
| `lens-upgrade.prompt.md` | `bmad-lens-upgrade` |

---

## Authoring Templates — `_bmad/lens-work/assets/templates/` (11 total)

| Template | Used By |
| --- | --- |
| `feature-yaml-template.yaml` | `init-feature-ops.py` |
| `product-brief-template.md` | `bmad-lens-preplan`, `bmad-lens-quickplan` |
| `prd-template.md` | `bmad-lens-businessplan`, `bmad-lens-quickplan` |
| `ux-design-template.md` | `bmad-lens-businessplan` |
| `architecture-template.md` | `bmad-lens-techplan`, `bmad-lens-quickplan` |
| `epics-template.md` | `bmad-lens-finalizeplan` |
| `stories-template.md` | `bmad-lens-finalizeplan` |
| `implementation-readiness-template.md` | `bmad-lens-finalizeplan` |
| `sprint-status-template.yaml` | `bmad-lens-finalizeplan` |
| `problems-template.md` | `bmad-lens-log-problem` |
| `user-profile-template.md` | `bmad-lens-onboard` |

---

_Generated by lens-bmad-document-project for feature lens-dev-old-codebase-discovery._
