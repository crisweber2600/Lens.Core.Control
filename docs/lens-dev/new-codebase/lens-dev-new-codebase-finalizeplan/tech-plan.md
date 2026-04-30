---
feature: lens-dev-new-codebase-finalizeplan
doc_type: tech-plan
status: draft
goal: "Define the technical implementation for the FinalizePlan, ExpressPlan, and QuickPlan conductor skills in the new codebase."
key_decisions:
  - All three skills are SKILL.md-only conductors — no Python scripts or CLI additions needed
  - FinalizePlan delegates governance publish to the existing publish-to-governance CLI
  - ExpressPlan delegates planning to bmad-lens-quickplan via bmad-lens-bmad-skill
  - QuickPlan is not published as a prompt stub; it is registered only as an internal bmad-lens-bmad-skill target
  - Prompt stubs follow the shared prompt-start preflight pattern with thin redirects
  - Tests use pytest + pyyaml; no new test infrastructure needed
open_questions: []
depends_on: [business-plan]
blocks: []
updated_at: '2026-04-30T00:00:00Z'
---

# Tech Plan — FinalizePlan, ExpressPlan, QuickPlan Conductors

## 1. Overview

Three conductor skills are delivered as part of this feature:

| Skill | Published | Role |
|-------|-----------|------|
| `bmad-lens-finalizeplan` | Yes (`.github/prompts/lens-finalizeplan.prompt.md`) | Three-step post-TechPlan consolidation conductor |
| `bmad-lens-expressplan` | Yes (`.github/prompts/lens-expressplan.prompt.md`) | Express-track planning conductor |
| `bmad-lens-quickplan` | No — internal only | Full planning pipeline (business→tech→sprint) delegated by ExpressPlan |

All three live under:
```
_bmad/lens-work/skills/{skill-name}/SKILL.md
```

## 2. System Design

### 2.1 FinalizePlan Conductor Architecture

```
/lens-finalizeplan
        │
        ▼
.github/prompts/lens-finalizeplan.prompt.md  (public stub)
        │  preflight gate + redirect
        ▼
_bmad/lens-work/prompts/lens-finalizeplan.prompt.md  (thin redirect)
        │
        ▼
bmad-lens-finalizeplan/SKILL.md  (conductor)
        │
        ├── Step 1: bmad-lens-adversarial-review --phase finalizeplan
        │           + publish-to-governance CLI (TechPlan artifacts → governance)
        │           + governance cross-check (confirm publish succeeded)
        │           + commit/push via bmad-lens-git-orchestration
        │
        ├── Step 2: bmad-lens-git-orchestration merge-plan --strategy pr
        │           (create/verify {featureId}-plan → {featureId} PR)
        │
        └── Step 3: bmad-lens-bmad-skill
                        ├── bmad-create-epics-and-stories
                        ├── bmad-check-implementation-readiness
                        ├── bmad-sprint-planning
                        └── bmad-create-story
                    + bmad-lens-git-orchestration create-pr
                      ({featureId} → main)
                    + bmad-lens-feature-yaml (phase → finalizeplan-complete)
```

### 2.2 ExpressPlan Conductor Architecture

```
/lens-expressplan
        │
        ▼
.github/prompts/lens-expressplan.prompt.md  (public stub)
        │  preflight gate + redirect
        ▼
_bmad/lens-work/prompts/lens-expressplan.prompt.md  (thin redirect)
        │
        ▼
bmad-lens-expressplan/SKILL.md  (conductor)
        │
        ├── Activation: validate track == express
        │               validate constitution permits express
        │               check review-ready fast path
        │
        ├── Step 1: bmad-lens-bmad-skill
        │               → bmad-lens-quickplan (full pipeline)
        │
        ├── Step 2: bmad-lens-adversarial-review
        │               --phase expressplan --source phase-complete
        │               + party-mode required
        │
        └── Step 3: bmad-lens-feature-yaml (phase → expressplan-complete)
                    bmad-lens-git-orchestration (feature-index.yaml update)
                    signal /finalizeplan
```

### 2.3 QuickPlan Internal Architecture

```
bmad-lens-quickplan/SKILL.md  (internal, no public stub)
        │
        ├── Phase A: business plan (John PM role)
        ├── Phase B: tech plan (Winston architect role)
        └── Phase C: sprint plan (Bob SM role)
            each with intra-phase adversarial review gate
```

## 3. Data Model

### 3.1 Feature Docs Path
Staged artifacts written to:
```
docs/{domain}/{service}/{featureId}/
├── business-plan.md      (QuickPlan)
├── tech-plan.md          (QuickPlan)
├── sprint-plan.md        (QuickPlan)
├── expressplan-adversarial-review.md  (ExpressPlan review)
├── finalizeplan-review.md (FinalizePlan review)
├── epics.md              (FinalizePlan bundle)
├── stories.md            (FinalizePlan bundle)
├── implementation-readiness.md  (FinalizePlan bundle)
└── sprint-status.yaml    (FinalizePlan bundle)
```

### 3.2 feature.yaml Phase Transitions

| Phase | Set By | Trigger |
|-------|--------|---------|
| `expressplan` | `bmad-lens-feature-yaml` | On ExpressPlan activation (from preplan) |
| `expressplan-complete` | `bmad-lens-feature-yaml` | On adversarial review pass in ExpressPlan |
| `finalizeplan-complete` | `bmad-lens-feature-yaml` | On final PR open in FinalizePlan step 3 |

### 3.3 Required Artifact Frontmatter

```yaml
---
feature: {featureId}
doc_type: business-plan | tech-plan | sprint-plan | expressplan-adversarial-review | ...
status: draft | in-review | approved
goal: "{one-line goal}"
key_decisions: []
open_questions: []
depends_on: []
blocks: []
updated_at: {ISO timestamp}
---
```

## 4. API Design

### 4.1 ExpressPlan Activation Gates

1. `feature.yaml.track == "express"` — hard block if false
2. Constitution `requires_constitution_permission` for express track
3. `validate-phase-artifacts.py --phase expressplan --contract review-ready` — fast path check

### 4.2 FinalizePlan Activation Gates

1. Predecessor `techplan` (or `expressplan-complete`) phase confirmed
2. `{featureId}` and `{featureId}-plan` branches exist
3. Governance publish CLI available (publish-to-governance)

### 4.3 bmad-lens-bmad-skill QuickPlan Registration

QuickPlan is registered in the `bmad-lens-bmad-skill` integration table:

```markdown
| `bmad-lens-quickplan` | Internal express planning pipeline |
```

## 5. Implementation Details

### 5.1 Directory Structure (new-codebase target repo)

```
_bmad/lens-work/
├── module.yaml  (add lens-finalizeplan.prompt.md, lens-expressplan.prompt.md)
├── prompts/
│   ├── lens-finalizeplan.prompt.md
│   └── lens-expressplan.prompt.md
└── skills/
    ├── bmad-lens-finalizeplan/
    │   ├── SKILL.md
    │   └── tests/
    │       └── test_finalizeplan_conductor.py
    ├── bmad-lens-expressplan/
    │   ├── SKILL.md
    │   └── tests/
    │       └── test_expressplan_conductor.py
    └── bmad-lens-quickplan/
        └── SKILL.md
```

### 5.2 Prompt Stub Pattern

`.github/prompts/lens-{command}.prompt.md`:
```markdown
---
mode: agent
description: "{command description}"
---
Run light-preflight, then redirect to
_bmad/lens-work/prompts/lens-{command}.prompt.md.
```

`_bmad/lens-work/prompts/lens-{command}.prompt.md` (thin redirect):
```markdown
Read and follow SKILL.md at _bmad/lens-work/skills/bmad-lens-{command}/SKILL.md
```

### 5.3 Governance Publish Sequence (FinalizePlan Step 1)

1. Confirm TechPlan artifacts present in staged docs path
2. Run: `uv run {module_path}/_bmad/lens-work/skills/bmad-lens-git-orchestration/scripts/git-orchestration-ops.py publish-to-governance --governance-repo {governance_repo} --control-repo {control_repo} --feature-id {feature_id} --phase techplan`
3. Push governance changes
4. Then proceed with FinalizePlan review

## 6. Testing Strategy

| Test File | Coverage |
|-----------|---------|
| `test_finalizeplan_conductor.py` | Three-step ordering, governance-write blocked, predecessor phase validation, PR creation triggers, bundle ordering |
| `test_expressplan_conductor.py` | Express-only gate, QuickPlan delegation route, adversarial review hard-stop, review-ready fast path, party-mode enforcement |

### Regression Expectations

1. `bmad-lens-finalizeplan/SKILL.md` is loadable and contains the three-step contract
2. Step 1 references adversarial review before governance publish
3. Step 2 references `merge-plan --strategy pr`
4. Step 3 delegates to `bmad-lens-bmad-skill`
5. No direct governance file creation in conductor
6. `bmad-lens-expressplan/SKILL.md` blocks non-express features
7. ExpressPlan step 1 delegates via `bmad-lens-bmad-skill --skill bmad-lens-quickplan`
8. ExpressPlan step 2 invokes `--phase expressplan --source phase-complete`
9. ExpressPlan step 3 sets `expressplan-complete` and signals `/finalizeplan`
10. Party mode is required (not optional) in ExpressPlan review

## 7. Rollout Strategy

### Phase 1 — Skill Files (DONE in prior session)
- All three SKILL.md files created in new-codebase `_bmad/lens-work/skills/`
- Prompt stubs and thin redirects created
- `module.yaml` updated with both prompt entries
- `bmad-lens-bmad-skill` registration updated
- 34 tests passing

### Phase 2 — Formal Planning Cycle (THIS SESSION)
- ExpressPlan: produce business-plan.md, tech-plan.md, sprint-plan.md, adversarial review
- FinalizePlan: produce epics.md, stories.md, implementation-readiness.md, sprint-status.yaml, story files
- Commit all planning artifacts to control repo `lens-dev-new-codebase-finalizeplan-plan` branch
- Create planning PR into `lens-dev-new-codebase-finalizeplan`

### Phase 3 — Dev Handoff
- Planning PR merged
- Final PR from `lens-dev-new-codebase-finalizeplan` → `main`
- `/dev` to wire the new-codebase skills to the actual module deliverable and run full regression
