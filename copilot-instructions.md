---
model: "Claude Sonnet 4.6 (copilot)"
---

# Project Guidelines & Commands

This workspace includes LENS Workbench for lifecycle management and AI-guided development.

## Workspace Commands

### LENS Workbench Slash Commands

Use these commands to manage features, planning, and governance. All commands support fuzzy matching.

#### Initiative & Feature Management

- `/init-feature` — Create a new feature with 2-branch topology
- `/new-domain` — Create a new domain container
- `/new-service` — Create a new service within a domain
- `/new-feature` — Alias for init-feature
- `/switch-feature` — Switch active feature context
- `/switch` — Alias for switch-feature
- `/feature-status` — Show current feature phase and activity
- `/status` — Alias for feature-status
- `/list-features` — List all features with status info

#### Planning & Workflow Routes

- `/preplan` — Start PrePlan phase (research, product brief, brainstorm)
- `/businessplan` — Start BusinessPlan phase (PRD, UX design)
- `/techplan` — Start TechPlan phase (architecture, technical planning)
- `/finalizeplan` — Start FinalizePlan phase (review, bundling, PR handoff)
- `/finalize` — Alias for finalizeplan
- `/expressplan` — Run all planning in one session (express track)
- `/express` — Alias for expressplan
- `/dev` — Delegate implementation to target project agents
- `/discover` — Inspect TargetProjects and prepare repo context

#### Navigation & Routing

- `/next` — Get recommended next action for current feature
- `/next-action` — Alias for next action
- `/help` — Show available LENS commands
- `/menu` — Redisplay the main menu
- `/chat` — Chat with the agent about anything

#### Lifecycle & Completion

- `/close` — Complete, abandon, or supersede a feature
- `/complete` — Alias for close
- `/abandon` — Alias for close
- `/retrospective` — Review what happened during a feature (lessons learned)
- `/retro` — Alias for retrospective
- `/profile` — View or update your onboarding profile

#### Problem Management

- `/log-problem` — Record an issue or blocker in problem log
- `/resolve-problem` — Mark a logged problem as resolved
- `/list-problems` — Show all problems for a feature
- `/analyze-problems` — Analyze problem patterns and frequency

#### Feature Structure & Relationships

- `/move-feature` — Relocate a feature to a different domain/service
- `/split-feature` — Create a new feature from a subset of stories
- `/validate-split` — Validate story IDs before splitting

#### Governance & Constitution

- `/constitution` — Load governance rules and workflow defaults
- `/load-constitution` — Alias for constitution
- `/compliance` — Run compliance validation against constitutional rules
- `/compliance-check` — Alias for compliance
- `/sense` — Detect cross-feature overlap and risks
- `/sensing` — Alias for sense
- `/audit-all` — Audit all active initiatives for compliance

#### Approval & Promotion

- `/promote` — Advance current feature to next audience tier
- `/approval-status` — Show pending PR approvals and review status
- `/rollback` — Revert to previous milestone
- `/rollback-phase` — Alias for rollback

#### Reporting & Dashboards

- `/dashboard` — Create HTML dashboard for all features
- `/generate-dashboard` — Alias for dashboard
- `/domain-status` — Show all features in a domain
- `/portfolio-status` — Show all active features across portfolio

#### Feature Pause & Resume

- `/pause-epic` — Suspend in-flight feature state
- `/resume-epic` — Resume a paused feature with re-sensing

#### Module & Setup

- `/upgrade` — Migrate control repo to latest schema version
- `/lens-upgrade` — Alias for upgrade
- `/migrate` — Alias for upgrade
- `/module-management` — Check module version and update guidance
- `/update` — Alias for module-management
- `/onboard` — Scaffold governance repo (legacy — use /new-domain instead)
- `/setup-lens` — Install/update LENS module configuration

#### Collaborative Modes

- `/party-mode` — Start collaborative multi-agent discussion
- `/exit` — Dismiss the agent
- `/quit` — Alias for exit

## Reference

- **Module:** `lens.core/_bmad/lens-work/`
- **Agent definition:** `lens.core/_bmad/lens-work/agents/lens.agent.md`
- **Config:** `lens.core/_bmad/lens-work/bmadconfig.yaml`
- **Lifecycle:** `lens.core/_bmad/lens-work/lifecycle.yaml`
- **Commands:** `lens.core/_bmad/lens-work/module-help.csv`
