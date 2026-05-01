---
feature: lens-dev-new-codebase-split-feature
doc_type: tech-plan
status: draft
goal: "Define the technical design for rewriting split-feature as a thin conductor: validate-first enforcement, in-progress blocking, atomic create-then-move ordering, and dual story-file format support — all in a clean-room implementation."
key_decisions:
  - split-feature command follows the thin-conductor pattern — SKILL.md orchestrates by delegation; no inline governance-write logic
  - validate-split result is the single gate before any governance mutation; script must exit non-zero on blocked stories
  - create-split-feature runs before move-stories in all code paths — no exceptions
  - Story status is read from both sprint-plan file and story-file frontmatter; sprint-plan takes precedence on conflict
  - Both .md and .yaml story file formats are treated as first-class inputs to validate-split and move-stories
  - Atomic write pattern for governance artifacts: feature.yaml + feature-index entry committed together via temp-file rename
  - BMB-first implementation channel for SKILL.md changes; bmad-workflow-builder for release prompt
  - dry-run mode produces no file writes for create-split-feature and move-stories
open_questions: []
depends_on:
  - business-plan.md (this feature)
  - lens-dev-new-codebase-baseline architecture.md
blocks: []
updated_at: 2026-04-30T00:00:00Z
---

# Technical Plan — Rewrite split-feature Command

**Feature:** lens-dev-new-codebase-split-feature  
**Author:** crisweber2600  
**Date:** 2026-04-30  
**References:** [Business Plan](./business-plan.md), [Baseline Architecture](../../lens-dev-new-codebase-baseline/docs/architecture.md)

---

## 1. System Design

The split-feature command follows the invariant 3-hop command resolution chain identical to every other retained command:

```
.github/prompts/lens-split-feature.prompt.md          (stub — user entry point)
  → lens.core/_bmad/lens-work/prompts/lens-split-feature.prompt.md  (release prompt)
    → skills/bmad-lens-split-feature/SKILL.md          (owning skill — thin conductor)
      → scripts/split-feature-ops.py                   (script — three-subcommand surface)
```

`light-preflight.py` fires at the stub before the redirect, as it does for all 17 retained commands. Its exit-code interface is frozen.

Unlike phase-planning commands (businessplan, techplan, etc.), split-feature is a **utility conductor**: it does not produce planning artifacts and does not follow the publish-before-author lifecycle. Its governance mutations are feature-reshaping operations — creating a new feature, updating feature-index.yaml, and moving story files. These mutations are performed by the script, not by direct agent file writes.

---

## 2. Thin Conductor Design

### 2.1 Execution Flow

The split-feature skill is interactive only. There is no batch mode.

```
[entry]
  → Load config from bmadconfig.yaml + config.user.yaml
  → Resolve governance_repo and username
  → Load source feature.yaml via bmad-lens-feature-yaml
  → Load cross-feature context via bmad-lens-git-state
  → Prompt: split mode? [validate | scope | stories]
  → Run validate-split before any governance mutation
      ├── blocked stories → hard stop; list blocked IDs; offer no workaround
      └── pass → confirm split plan with user; show both sides
  → [scope path] split-feature-ops.py create-split-feature (dry-run first)
                 → user confirms → execute create-split-feature
                 → update original feature.yaml scope notes
  → [stories path] split-feature-ops.py create-split-feature (dry-run first)
                   → user confirms → execute create-split-feature
                   → split-feature-ops.py move-stories (dry-run first)
                   → user confirms → execute move-stories
  → Report: new feature path, modified files, story moves (if any)
[done]
```

### 2.2 Delegation Points

| Concern | Old codebase | New codebase |
|---------|-------------|--------------|
| Validate story eligibility | Inline status check in SKILL.md | `split-feature-ops.py validate-split` |
| Create new feature governance | Direct file writes in some paths | `split-feature-ops.py create-split-feature` |
| Move story files | Direct file operations in SKILL.md | `split-feature-ops.py move-stories` |
| Read feature context | Direct YAML read | `bmad-lens-feature-yaml` |
| Git state context | Direct git call | `bmad-lens-git-state` |
| Feature-index update | Inline write | atomic write via `create-split-feature` subcommand |

### 2.3 validate-split — Hard Gate

`validate-split` reads story status from two sources, in priority order:

1. **Sprint plan file** — parses `development_status:` section or `stories:` map for YAML; also handles markdown-embedded YAML code blocks and simple `key: value` line pairs
2. **Story file frontmatter** — if a story ID is not in the sprint plan, falls back to reading the story file's YAML frontmatter or inline `status:` pattern

A story is `in-progress` if either source reports `in-progress`. A story is eligible if status is `pending`, `done`, `blocked`, `backlog`, `ready-for-dev`, or `review` in both sources (or absent from both, which is treated as eligible by default — status unknown is not blocked).

If any story in the requested split set has `in-progress` status, `validate-split` exits non-zero and the skill hard-stops. No create or move operations run. The skill lists the blocked story IDs explicitly.

**Exit codes for `validate-split`:**
- `0` — all stories eligible; result JSON includes `"status": "pass"`
- `1` — runtime error (file not found, parse failure)
- `2` — one or more in-progress stories found; result JSON includes `"status": "fail"`

### 2.4 Atomic Governance Write Ordering

`create-split-feature` creates governance artifacts in this order before any modification of the source feature:

1. Create `features/{domain}/{service}/{new_feature_id}/` directory
2. Write `feature.yaml` for the new feature (atomic via temp-file rename)
3. Update `feature-index.yaml` entry for the new feature (atomic via temp-file rename)
4. Write `summary.md` stub

Only after all four artifacts are committed does `move-stories` run (if requested). The source feature is modified last. This ordering ensures that if any step fails, the governance state is either fully clean (no new feature) or fully complete (new feature present with all required artifacts).

---

## 3. Script Surface (Preserved API)

`split-feature-ops.py` exposes three subcommands. All three are part of the retained published API and must be preserved unchanged at the call level.

### 3.1 validate-split

```bash
uv run split-feature-ops.py validate-split \
  --sprint-plan-file {path} \
  --story-ids "{comma-separated or JSON array}"
```

Output (JSON to stdout):
```json
{
  "status": "pass|fail",
  "eligible": ["story-id", ...],
  "blocked": [{"id": "story-id", "reason": "in-progress"}, ...],
  "blockers": ["story-id", ...]
}
```

### 3.2 create-split-feature

```bash
uv run split-feature-ops.py create-split-feature \
  --governance-repo {path} \
  --source-feature-id {id} \
  --source-domain {domain} \
  --source-service {service} \
  --new-feature-id {id} \
  --new-name "{name}" \
  --track {track} \
  --username {username} \
  [--dry-run]
```

Dry-run prints a plan with no writes. Live execution creates: feature directory, feature.yaml, feature-index entry, summary stub.

### 3.3 move-stories

```bash
uv run split-feature-ops.py move-stories \
  --governance-repo {path} \
  --source-feature-id {id} \
  --source-domain {domain} \
  --source-service {service} \
  --target-feature-id {id} \
  --target-domain {domain} \
  --target-service {service} \
  --story-ids "{comma-separated or JSON array}" \
  [--dry-run]
```

Dry-run lists which files would be moved. Live execution moves `.md` and `.yaml` story files from the source `stories/` directory to the target `stories/` directory.

---

## 4. Files to Create / Modify

### 4.1 Core Deliverables

| File | Action | Channel |
|------|--------|---------|
| `lens.core/_bmad/lens-work/skills/bmad-lens-split-feature/SKILL.md` | Rewrite — thin conductor | bmad-module-builder (BMB-first) |
| `lens.core/_bmad/lens-work/prompts/lens-split-feature.prompt.md` | Rewrite — release prompt (stub redirect) | bmad-workflow-builder |
| `.github/prompts/lens-split-feature.prompt.md` | Verify stub chain integrity | Direct (stub only) |
| `lens.core/_bmad/lens-work/skills/bmad-lens-split-feature/scripts/split-feature-ops.py` | Clean-room rewrite | bmad-module-builder / direct |

### 4.2 References (owned by this skill — retain or update as needed)

| File | Action |
|------|--------|
| `skills/bmad-lens-split-feature/references/validate-split.md` | Retain — behavior contract unchanged |
| `skills/bmad-lens-split-feature/references/split-scope.md` | Retain — process steps unchanged |
| `skills/bmad-lens-split-feature/references/split-stories.md` | Retain — story-move process unchanged |

### 4.3 Shared Dependencies (read-only — not modified in this feature)

| Dependency | Why Referenced |
|-----------|----------------|
| `skills/bmad-lens-feature-yaml/` | Reads source feature.yaml; creates new feature.yaml schema |
| `skills/bmad-lens-git-state/` | Cross-feature context on activation |
| `scripts/light-preflight.py` | Frozen prompt-start gate |
| `feature-index.yaml` | Updated by create-split-feature subcommand |

---

## 5. Clean-Room Implementation Notes

The old-codebase `split-feature-ops.py` is consulted as a behavioral reference only. The new implementation is authored from scratch and must satisfy the following contracts derived from the baseline PRD and architecture:

- `SAFE_ID_PATTERN` validation on all feature IDs passed as CLI arguments — path-constructing identifiers must match `[a-z0-9][a-z0-9._-]{0,63}`
- All YAML writes use atomic temp-file + rename to prevent partial-write corruption
- Status resolution order: sprint-plan file first, story-file frontmatter second
- Story files are treated as compatibility boundaries: the script moves files as-is without modifying content

---

## 6. Testing Strategy

| Test Class | Coverage |
|-----------|----------|
| validate-first regression | Any create-split-feature or move-stories call without a prior passing validate-split is blocked |
| In-progress blocking | Story with `in-progress` in sprint-plan YAML, markdown-embedded YAML, and story-file frontmatter — all three formats must hard-stop |
| Atomic ordering regression | Process terminates after create-split-feature (before move-stories) — new feature governance artifacts must be present; source feature must be unmodified |
| Story-file format regression | validate-split and move-stories handle .md and .yaml story file variants without error |
| Dry-run regression | `--dry-run` flag on create-split-feature and move-stories produces no writes; non-zero exit only on argument or pre-condition errors |
| Governance completeness regression | feature.yaml, feature-index entry, and summary stub are all written for the new feature after create-split-feature |
| Identifier validation regression | Invalid feature IDs (uppercase, spaces, path traversal) are rejected with a clear error before any file operation |

Test file: `lens.core/_bmad/lens-work/skills/bmad-lens-split-feature/scripts/tests/test-split-feature-ops.py`

---

## 7. Rollout Strategy

- No feature flags required — this is an internal skill rewrite with no user-visible API change
- Deployed atomically as part of the Lens.Core.Release promotion cycle
- Rollback: revert to prior `split-feature-ops.py` commit; no governance schema migrations involved
- Backwards compatibility: `feature.yaml` and `feature-index.yaml` schema are unchanged (v4 frozen); all existing split features remain valid

---

## 8. Observability

No new metrics or alerts. The script writes structured JSON results to stdout for each subcommand. The SKILL.md surfaces validate-split results verbatim as the user-facing hard-stop message. No silent failures.
