---
feature: nextlens-src-topdownlens
doc_type: guide
story_id: TL-9
title: Constitution Layering For TopDownLens
updated_at: 2026-05-14T04:45:00Z
---

# Constitution Layering For TopDownLens

TopDownLens inherits Lens governance through the 4-level constitution model: organization, domain, service, and optional repository or feature-specific additions. The first TopDownLens increment uses informational gates only, so constitution findings are recorded and passed to implementation work without blocking downstream stories.

## Active Resolution

Command used during dev:

```bash
uv run --script lens.core/_bmad/lens-work/skills/lens-constitution/scripts/constitution-ops.py resolve \
  --governance-repo TargetProjects/lens/Lens.Core.Governance \
  --domain nextlens \
  --service src
```

Resolved levels:

| Level | Path | Scope | Gate Mode |
| --- | --- | --- | --- |
| Organization | `constitutions/org/constitution.md` | All Lens-governed domains and services. | informational |
| Domain | `constitutions/nextlens/constitution.md` | All features under `nextlens`. | informational |
| Service | `constitutions/nextlens/src/constitution.md` | All repositories and features under `nextlens/src`. | informational |
| Feature placeholder | `features/nextlens/src/nextlens-src-topdownlens/` | No direct constitution file is created in this dev story. Feature-level additions must be introduced through an approved governance operation. | inherits informational |

Resolved structured view:

```yaml
permitted_tracks:
  - express
  - full
required_artifacts:
  planning:
    - business-plan
    - tech-plan
  dev:
    - stories
gate_mode: informational
sensing_gate_mode: informational
additional_review_participants: []
enforce_stories: true
enforce_review: true
```

## Inheritance Rules

- Lower levels add constraints; they do not remove inherited requirements.
- Track permissions are intersected across levels.
- Required artifacts are unioned across levels.
- Hard gate mode would override informational mode, but no active TopDownLens level sets a hard gate for this increment.
- Story and review enforcement remain enabled because org, domain, and service levels all set them to true.

## First-Increment Rationale

This increment is contract-building work. It records schemas, examples, topology, Salmon format, and dogfooding evidence before the dedicated `nextlens-governance` and `nextlens-release` repos exist. Informational gates are appropriate because they surface governance context while avoiding deadlocks caused by future repo surfaces that are not available yet.

## Per-Story Application

Every story in this dev session receives the following applicable articles:

- Dev must operate from story artifacts.
- Review is enforced by default, with findings informational for this increment.
- Planning context must retain `business-plan` and `tech-plan` as required inputs.
- Direct governance and release writes are not allowed outside approved orchestration boundaries.

Stories that appear to require governance writes must either use an approved Lens operation or record a control-side mirror/placeholder and defer governance mutation until publication. This applies to feature-level constitution placeholders and any future `nextlens-governance` bootstrap files.