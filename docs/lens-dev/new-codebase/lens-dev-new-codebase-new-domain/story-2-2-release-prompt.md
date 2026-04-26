---
feature_id: lens-dev-new-codebase-new-domain
story_key: "2-2-release-prompt"
epic: 2
story: 2
title: "Release prompt: lens-new-domain.prompt.md"
type: implementation
estimate: S
priority: P1
status: not-started
assigned: crisweber2600
sprint: 2
depends_on:
  - "2-1-stub"
blocks:
  - "2-3-skill-ux"
created_at: 2026-04-26T00:00:00Z
updated_at: 2026-04-26T00:00:00Z
---

# Story 2.2 — Release prompt: lens-new-domain.prompt.md

## What To Build

The release prompt at `lens.core/_bmad/lens-work/prompts/lens-new-domain.prompt.md`. This is the thin delegation layer that loads the `bmad-lens-init-feature` SKILL.md and declares `create-domain` as the intent.

---

## File Locations

| File | Action |
|---|---|
| `lens.core/_bmad/lens-work/prompts/lens-new-domain.prompt.md` | Create |

---

## Pattern to Follow

Read existing release prompts in `lens.core/_bmad/lens-work/prompts/` for the exact structure. Check if `lens-new-feature.prompt.md` or `lens-new-service.prompt.md` exist and use those as pattern references.

The release prompt must:
1. Load `bmad-lens-init-feature` SKILL.md
2. Declare the intent as `create-domain`
3. Resolve config keys: `governance_repo`, `target_projects_path`, `personal_output_folder` from `bmadconfig.yaml`
4. Pass the resolved config values to the skill
5. Contain no domain creation logic

---

## Acceptance Criteria

- [ ] File created at `lens.core/_bmad/lens-work/prompts/lens-new-domain.prompt.md`
- [ ] Prompt loads `bmad-lens-init-feature` SKILL.md
- [ ] Prompt declares `intent: create-domain`
- [ ] Prompt resolves `governance_repo` from bmadconfig.yaml
- [ ] Prompt resolves `target_projects_path` from bmadconfig.yaml
- [ ] Prompt resolves `personal_output_folder` from bmadconfig.yaml (optional key — gracefully absent)
- [ ] No domain creation logic in the release prompt
- [ ] Prompt structure matches the pattern of at least one existing release prompt in `lens.core/_bmad/lens-work/prompts/`
