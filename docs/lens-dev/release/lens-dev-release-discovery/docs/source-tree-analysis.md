# lens.core.release — Source Tree Analysis

**Feature:** lens-dev-release-discovery
**Generated:** 2026-05-20

---

## Repository Root

```
lens.core.release/
├── AGENTS.md                          # Codex agent activation entry point
├── _bmad/                             # All BMAD module content
│   ├── bmb/                           # BMAD Builder module
│   ├── bmm/                           # BMAD Methodologies module
│   ├── cis/                           # Creative Innovation Studio module
│   ├── core/                          # Core BMAD skills
│   ├── gds/                           # Game Design System module
│   ├── lens-work/                     # ★ LENS Workbench module (primary)
│   ├── tea/                           # Test Engineering Agent module
│   └── wds/                           # Web Design System module
├── .claude/                           # Claude IDE adapter
├── .codex/                            # Codex IDE adapter
├── .cursor/                           # Cursor IDE adapter
└── .github/                           # GitHub Copilot adapter
    ├── agents/                        # Copilot agent definition
    ├── instructions/                  # Always-on Copilot instructions
    ├── prompts/                       # 57 prompt stubs
    ├── skills/                        # Skill stubs (linked from .github)
    └── lens-work-instructions.md
```

---

## `_bmad/lens-work/` — Primary Module (Annotated)

```
_bmad/lens-work/
│
├── module.yaml               # ★ Module identity, skill/prompt registry, adapters
├── lifecycle.yaml            # ★ Lifecycle contract v4 — phases, gates, axioms
├── module-help.csv           # Command discovery and help registry
├── bmadconfig.yaml           # Default configuration for Lens installs
├── README.md                 # Human-readable module reference
├── TODO.md                   # Active module TODO items
│
├── agents/                   # Thin-shell agent definitions
│   ├── lens.agent.md         # ★ Primary Lens agent (compact menu + skill routing)
│   └── lens.agent.yaml       # YAML representation of agent
│
├── prompts/                  # 57 published lens-*.prompt.md entry points
│   ├── lens-adversarial-review.prompt.md
│   ├── lens-approval-status.prompt.md
│   ├── lens-audit.prompt.md
│   ├── lens-batch.prompt.md
│   ├── lens-bmad-brainstorming.prompt.md
│   ├── lens-bmad-check-implementation-readiness.prompt.md
│   ├── lens-bmad-code-review.prompt.md
│   ├── lens-bmad-create-architecture.prompt.md
│   ├── lens-bmad-create-epics-and-stories.prompt.md
│   ├── lens-bmad-create-prd.prompt.md
│   ├── lens-bmad-create-story.prompt.md
│   ├── lens-bmad-create-ux-design.prompt.md
│   ├── lens-bmad-document-project.prompt.md
│   ├── lens-bmad-domain-research.prompt.md
│   ├── lens-bmad-market-research.prompt.md
│   ├── lens-bmad-product-brief.prompt.md
│   ├── lens-bmad-quick-dev.prompt.md
│   ├── lens-bmad-sprint-planning.prompt.md
│   ├── lens-bmad-technical-research.prompt.md
│   ├── lens-businessplan.prompt.md
│   ├── lens-checklist.prompt.md
│   ├── lens-complete.prompt.md
│   ├── lens-constitution.prompt.md
│   ├── lens-dashboard.prompt.md
│   ├── lens-dev.prompt.md
│   ├── lens-discover.prompt.md
│   ├── lens-expressplan.prompt.md
│   ├── lens-feature-yaml.prompt.md
│   ├── lens-finalizeplan.prompt.md
│   ├── lens-git-orchestration.prompt.md
│   ├── lens-git-state.prompt.md
│   ├── lens-help.prompt.md
│   ├── lens-init-feature.prompt.md
│   ├── lens-log-problem.prompt.md
│   ├── lens-migrate.prompt.md
│   ├── lens-module-management.prompt.md
│   ├── lens-move-feature.prompt.md
│   ├── lens-new-domain.prompt.md
│   ├── lens-new-feature.prompt.md
│   ├── lens-new-project.prompt.md
│   ├── lens-new-service.prompt.md
│   ├── lens-next.prompt.md
│   ├── lens-onboard.prompt.md
│   ├── lens-pause-resume.prompt.md
│   ├── lens-preflight.prompt.md
│   ├── lens-preplan.prompt.md
│   ├── lens-quickplan.prompt.md
│   ├── lens-retrospective.prompt.md
│   ├── lens-rollback.prompt.md
│   ├── lens-sensing.prompt.md
│   ├── lens-setup.prompt.md
│   ├── lens-split-feature.prompt.md
│   ├── lens-sprintplan.prompt.md
│   ├── lens-switch.prompt.md
│   ├── lens-target-repo.prompt.md
│   ├── lens-techplan.prompt.md
│   ├── lens-theme.prompt.md
│   └── lens-upgrade.prompt.md
│
├── skills/                   # 41 bmad-lens-* skill directories
│   ├── bmad-lens-adversarial-review/   # Adversarial plan review
│   ├── bmad-lens-approval-status/      # Approval gate status
│   ├── bmad-lens-audit/                # Feature and initiative audit
│   ├── bmad-lens-batch/                # Batch multi-feature operations
│   ├── bmad-lens-bmad-skill/           # BMAD skill invocation wrapper
│   ├── bmad-lens-businessplan/         # Business planning conductor
│   ├── bmad-lens-complete/             # Feature completion gating
│   ├── bmad-lens-constitution/         # Governance rules resolution
│   ├── bmad-lens-dashboard/            # Initiative dashboard
│   ├── bmad-lens-dev/                  # Development phase conductor
│   ├── bmad-lens-devproposal/          # Dev proposal generation
│   ├── bmad-lens-discover/             # Repo inventory discovery
│   ├── bmad-lens-document-project/     # Project documentation
│   ├── bmad-lens-expressplan/          # Express planning track
│   ├── bmad-lens-feature-yaml/         # Feature YAML operations
│   ├── bmad-lens-finalizeplan/         # Finalize plan phase
│   ├── bmad-lens-git-orchestration/    # Git write operations
│   ├── bmad-lens-git-state/            # Git read-only queries
│   ├── bmad-lens-help/                 # Command help and discovery
│   ├── bmad-lens-init-feature/         # Feature initialization
│   ├── bmad-lens-lessons/              # Lessons learned capture
│   ├── bmad-lens-log-problem/          # Problem logging
│   ├── bmad-lens-migrate/              # Schema migration
│   ├── bmad-lens-module-management/    # Module installation management
│   ├── bmad-lens-move-feature/         # Feature domain/service move
│   ├── bmad-lens-next/                 # Next-action routing
│   ├── bmad-lens-onboard/              # Deprecated onboard bridge
│   ├── bmad-lens-pause-resume/         # Feature pause and resume
│   ├── bmad-lens-preplan/              # Pre-planning phase
│   ├── bmad-lens-profile/              # User profile and PAT management
│   ├── bmad-lens-quickplan/            # Quick planning pipeline
│   ├── bmad-lens-retrospective/        # Feature retrospective
│   ├── bmad-lens-rollback/             # Phase rollback
│   ├── bmad-lens-sensing/              # Cross-initiative sensing
│   ├── bmad-lens-split-feature/        # Feature split operations
│   ├── bmad-lens-sprintplan/           # Sprint planning
│   ├── bmad-lens-switch/               # Feature context switch
│   ├── bmad-lens-target-repo/          # Target repo provisioning
│   ├── bmad-lens-techplan/             # Technical planning conductor
│   ├── bmad-lens-theme/                # Theme and persona overlay
│   └── bmad-lens-upgrade/              # Schema upgrade
│
├── scripts/                  # 19 operational scripts (14 .py + 1 .sh + README + tests/)
│   ├── install.py                      # Install module into control repo
│   ├── setup-control-repo.py           # Bootstrap new control repos
│   ├── preflight.py                    # Full preflight validation
│   ├── light-preflight.py              # Fast preflight check
│   ├── run-preflight-cached.py         # Cached preflight
│   ├── create-pr.py                    # GitHub REST API PR creation
│   ├── store-github-pat.py             # PAT environment setup
│   ├── validate-lens-bmad-registry.py  # Registry consistency validation
│   ├── validate-lens-core-parity.sh    # Source/release parity check
│   ├── validate-phase-artifacts.py     # Phase artifact presence check
│   ├── validate-feature-move.py        # Feature move validation
│   ├── derive-next-action.py           # Next-action computation
│   ├── derive-initiative-status.py     # Initiative status computation
│   ├── scan-active-initiatives.py      # Cross-repo initiative scan
│   ├── load-command-registry.py        # Command registry loader
│   ├── bootstrap-target-projects.py    # TargetProjects scaffolding
│   ├── plan-lifecycle-renames.py       # Lifecycle rename planning
│   ├── README.md                       # Script documentation
│   └── tests/                          # 10 Python test files + README
│       ├── README.md
│       ├── test-adversarial-review-contracts.py
│       ├── test-batch-mode-contracts.py
│       ├── test-create-pr.py
│       ├── test-install.py
│       ├── test-light-preflight.py
│       ├── test-phase-conductor-contracts.py
│       ├── test-preflight.py
│       ├── test-setup-control-repo.py
│       ├── test-store-github-pat.py
│       └── test-validate-phase-artifacts.py
│
├── assets/                   # Static assets
│   ├── lens-bmad-skill-registry.json   # Machine-readable skill registry
│   └── templates/                      # Reusable templates
│
├── docs/                     # Embedded reference documentation
│   └── (24 embedded docs)
│
├── tests/                    # Contract tests
│   └── contracts/            # 8 behavioral contract files
│       ├── branch-parsing.md
│       ├── governance.md
│       ├── sensing.md
│       ├── two-branch-topology.md
│       └── (4 more contracts)
│
├── _module-installer/        # CI/CD adapter generator
│   └── (generates .github/, .claude/, .codex/, .cursor/ adapter stubs)
│
└── .claude-plugin/           # Distribution manifest for Claude
```

---

## `_bmad/bmb/` — BMAD Builder Module

```
_bmad/bmb/
├── bmadconfig.yaml
├── module-help.csv
└── skills/
    ├── bmad-agent-builder/   # Build/edit/validate AI agent skills
    │   ├── SKILL.md
    │   ├── assets/           # Templates: init, memory, quality report
    │   ├── references/       # Metadata, quality dimensions, scan schemas
    │   └── scripts/          # manifest.py, prepass scripts, scan scripts
    └── bmad-workflow-builder/ # Build/validate skill workflows
        ├── SKILL.md
        ├── assets/           # SKILL and quality report templates
        ├── references/       # Classification, patterns, quality dims
        └── scripts/          # manifest.py, prepass scripts
```

---

## `_bmad/bmm/` — BMAD Methodologies Module

```
_bmad/bmm/
├── 1-analysis/               # Discovery and analysis
│   ├── bmad-agent-analyst/
│   ├── bmad-agent-tech-writer/
│   ├── bmad-document-project/   # Full project documentation workflow
│   ├── bmad-product-brief/      # Product brief creation
│   └── research/
│       ├── bmad-domain-research/
│       ├── bmad-market-research/
│       └── bmad-technical-research/
├── 2-plan-workflows/          # Planning workflows
│   ├── bmad-agent-pm/
│   ├── bmad-agent-ux-designer/
│   ├── bmad-create-prd/          # 12-step PRD creation
│   ├── bmad-create-ux-design/    # UX design specification
│   ├── bmad-edit-prd/
│   └── bmad-validate-prd/
├── 3-solutioning/             # Architecture and technical design
└── 4-implementation/          # Dev stories, QA, retrospectives
```

---

## `_bmad/core/` — Core BMAD Skills

```
_bmad/core/
├── bmad-advanced-elicitation/
├── bmad-brainstorming/
├── bmad-distillator/
├── bmad-editorial-review-prose/
├── bmad-editorial-review-structure/
├── bmad-help/
├── bmad-index-docs/
├── bmad-init/
├── bmad-party-mode/
├── bmad-review-adversarial-general/
└── bmad-review-edge-case-hunter/
```

---

## `.github/` — GitHub Copilot Adapter

```
.github/
├── agents/
│   └── bmad-agent-lens-work-lens.agent.md  # Copilot agent definition
├── instructions/
│   └── lens-control-repo.instructions.md   # Control repo rules
├── prompts/                                 # 57 .prompt.md stubs
│   ├── lens-adversarial-review.prompt.md
│   ├── lens-businessplan.prompt.md
│   └── ... (55 more)
├── skills/                                  # Skill stubs
└── lens-work-instructions.md
```

---

## Notable Structural Patterns

### Skill Directory Convention
Every `bmad-lens-*` skill follows a consistent layout:
```
bmad-lens-{name}/
├── SKILL.md          # Primary skill instructions (required)
├── scripts/          # Focused Python scripts (optional)
│   └── {name}-ops.py
├── assets/           # Templates, schemas, data (optional)
├── tests/            # Skill-specific tests (optional)
└── *.md              # Supporting instruction files (optional)
```

### Prompt Stub Convention
Every `lens-*.prompt.md` follows the same two-step pattern:
1. Run `light-preflight.py` to validate environment and feature context
2. Load the corresponding `SKILL.md` and follow its instructions

### Script Test Convention
Tests use `uv run --with pytest` for isolated execution:
```bash
uv run --with pytest pytest _bmad/lens-work/scripts/tests/{test-file}.py -q
```

---

_Generated by lens-bmad-document-project for feature lens-dev-release-discovery._
