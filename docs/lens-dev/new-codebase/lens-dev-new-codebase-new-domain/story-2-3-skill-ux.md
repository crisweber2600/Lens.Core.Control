---
feature_id: lens-dev-new-codebase-new-domain
story_key: "2-3-skill-ux"
epic: 2
story: 3
title: "Skill prompt UX: slug derivation + confirmation"
type: implementation
estimate: S
priority: P1
status: not-started
assigned: crisweber2600
sprint: 2
depends_on:
  - "2-2-release-prompt"
  - "1-1-safe-id-pattern"
blocks: []
created_at: 2026-04-26T00:00:00Z
updated_at: 2026-04-26T00:00:00Z
---

# Story 2.3 — Skill prompt UX: slug derivation + confirmation

## Why This Story Exists

The finalizeplan review (John-P finding, Medium severity) requires explicit user confirmation before any domain is written. The user provides a display name; the skill derives a slug from it and must show the slug to the user and wait for a Yes before invoking `create-domain`.

---

## What To Build

The interaction logic within the `bmad-lens-init-feature` SKILL.md for the `create-domain` intent path.

---

## File Locations

| File | Action |
|---|---|
| `lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/SKILL.md` | Add create-domain intent section |

---

## Interaction Flow

```
User: [runs /new-domain]

Skill: What is the display name for the new domain?
  (This will be recorded in domain.yaml as `name`)

User: My Platform Services

Skill: Domain slug will be: `my-platform-services`
       Proceed? [Y/n/edit]

User: Y

Skill: [invokes create-domain --domain my-platform-services --name "My Platform Services" ...]

  On success (no auto-git):
    Domain created.
    Run these commands to commit to governance:
      git -C {governance_repo} add ...
      git -C {governance_repo} commit -m "..."
      git -C {governance_repo} push

  On success (auto-git):
    Domain created and committed to governance.
    Commit SHA: {sha}

  On fail (duplicate):
    Error: Domain `my-platform-services` already exists in governance.
    No files were written.

  On fail (sync failure):
    Error: Could not sync governance repo before creating domain.
    Details: {error message from JSON}
```

---

## Slug Derivation Rules

1. Strip leading/trailing whitespace
2. Lowercase everything
3. Replace spaces and any non-alphanumeric character (except hyphen) with hyphen
4. Collapse consecutive hyphens to a single hyphen
5. Strip leading/trailing hyphens
6. Validate against `SAFE_ID_PATTERN` (resolved in Story 1.1)

**Examples:**
- `"My Platform Services"` → `"my-platform-services"`
- `"Platform.Core"` → `"platform-core"`
- `"LENS Dev"` → `"lens-dev"`
- `"A Domain!"` → `"a-domain"`

If the derived slug is invalid (empty, too long), prompt user to provide a valid slug manually.

---

## Acceptance Criteria

- [ ] Skill asks only for display name as minimum required input
- [ ] Slug derived from display name following the rules above
- [ ] Derived slug shown to user before any invocation: "Domain slug will be: `{slug}`. Proceed? [Y/n/edit]"
- [ ] On "Y": `create-domain` invoked with `--domain {slug} --name {display_name}` plus resolved config args
- [ ] On "n": operation cancelled, user informed, no script invoked
- [ ] On "edit": user prompted for a manual slug, manual slug validated against `SAFE_ID_PATTERN`, then confirmation shown again
- [ ] If derived slug fails `SAFE_ID_PATTERN`: user is asked to enter a valid slug directly (skip "Proceed?" since the derived value is invalid)
- [ ] On success without auto-git: `remaining_git_commands` from JSON response are displayed
- [ ] On success with auto-git: `governance_commit_sha` displayed
- [ ] On `status: fail`: `error` field from JSON response displayed verbatim

## Manual Walkthrough Verification

Story 2.3 requires a manual walkthrough verification:
1. Run `/new-domain` with a display name containing a space
2. Confirm slug is shown correctly
3. Choose "edit" and enter a custom slug
4. Confirm the custom slug confirmation is shown before invocation
5. Proceed and verify output
