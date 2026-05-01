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
  - lens-dev-new-codebase-baseline/prd.md
  - lens-dev-new-codebase-baseline/research.md
blocks: []
updated_at: 2026-04-30T00:00:00Z
---

# Technical Plan — Rewrite split-feature Command

**Feature:** lens-dev-new-codebase-split-feature  
**Author:** crisweber2600  
**Date:** 2026-04-30  
**References:** [Business Plan](./business-plan.md), [Baseline PRD](../../lens-dev-new-codebase-baseline/prd.md), [Baseline Research](../../lens-dev-new-codebase-baseline/research.md)

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

1. **Sprint plan file** — parses the file using the following recognized format variants, tried in order:
   - **Canonical YAML** — a top-level `stories:` map where each key is a story ID and the value contains a `status:` field
   - **Fenced YAML block** — a ` ```yaml ` … ` ``` ` block anywhere in a markdown file that, when parsed, yields a `stories:` map with per-story status fields
   - **Inline key-value pairs** — lines matching `{story-id}: {status}` or `status: {status}` immediately preceded by a story-id context line
   
   If none of the above patterns match, the sprint plan file is treated as **no-data** for that story ID, and the fallback to source 2 applies. Degradation rules:
   - **Unrecognized format** — the file does not match any recognized variant; treated as no-data for all story IDs; no hard-stop.
   - **Structural YAML parse error** — the file exists but cannot be parsed (malformed YAML); treated as no-data for all story IDs; degrades silently.
   - **File I/O error** — file not found or permission denied; causes hard-stop exit code 1.
   
2. **Story file frontmatter** — if a story ID is not found in the sprint plan (or the sprint plan format is unrecognized), falls back to reading the story file's YAML frontmatter or inline `status:` pattern

**Status normalization:** Before the `in-progress` check, all status values are normalized: `in_progress`, `in-progress`, `IN_PROGRESS`, and `in progress` are treated as equivalent. This covers old-codebase sprint files that used underscore-delimited status values.

`validate-split` determines a single effective status per story using the precedence order above: if the sprint plan provides a status for the story, that status is authoritative; otherwise it uses the story file status; if both sources are absent, status is treated as unknown.

A story is blocked only if its effective status is `in-progress`. A story is eligible if its effective status is `pending`, `done`, `blocked`, `backlog`, `ready-for-dev`, or `review`, or if status is absent from both sources (eligible by default — unknown status is not blocked).

If any story in the requested split set has effective `in-progress` status, `validate-split` exits non-zero and the skill hard-stops. No create or move operations run. The skill lists the blocked story IDs explicitly.

**Exit codes for `validate-split`:**
- `0` — all stories eligible; result JSON includes `"status": "pass"`
- `1` — runtime error (file not found, I/O permission error)
- `2` — one or more in-progress stories found; result JSON includes `"status": "fail"`

### 2.4 Atomic Governance Write Ordering

**Pre-condition check (BS-3):** Before writing any artifact, `create-split-feature` checks that `new-feature-id` does not already exist in `feature-index.yaml`. If a matching entry is found, the script exits 1 with a clear duplicate-feature error and performs no writes. This prevents partial-state corruption if a prior interrupted run left a stale entry.

`create-split-feature` creates governance artifacts in this order before any modification of the source feature:

1. Verify `new-feature-id` is absent from `feature-index.yaml` (duplicate guard — exit 1 on conflict)
2. Create `features/{domain}/{service}/{new_feature_id}/` directory
3. Write `feature.yaml` for the new feature (atomic via temp-file rename)
4. Update `feature-index.yaml` entry for the new feature (atomic via temp-file rename)
5. Write `summary.md` stub

Only after all five artifacts are committed does `move-stories` run (if requested). The source feature is modified last. This ordering ensures the source feature is not mutated until creation of the new feature artifacts has been attempted. Temp-file rename makes the individual `feature.yaml` and `feature-index.yaml` writes atomic. However, because this sequence spans multiple filesystem operations (`mkdir`, multiple file writes), a failure during steps 2–5 can still leave partially created new-feature artifacts unless explicit cleanup/rollback is implemented.

**Failure handling / idempotency requirement:**
- If any step in `create-split-feature` fails before all four new-feature artifacts are present (directory, `feature.yaml`, `feature-index.yaml` entry, `summary.md`), `move-stories` must not run.
- The partially created feature must be treated as `incomplete`, not as a valid completed feature.
- `feature-index.yaml` is the publication point: a new feature is considered discoverable/complete only when `feature.yaml`, `summary.md`, and the feature directory exist and the `feature-index.yaml` entry has been written successfully.
- Rerunning `create-split-feature` with the same `new-feature-id` must be deterministic: the duplicate guard (step 1) exits 1 if the feature-index entry already exists, preventing double-write but also preventing transparent resume. Partial cleanup of incomplete artifacts must be performed manually or via a dedicated cleanup flag before retrying.

---

## 3. Script Surface (Preserved API)

`split-feature-ops.py` exposes three subcommands. All three are part of the retained published API and must be preserved unchanged at the call level.

**Argument validation (L2):** All `--feature-id`, `--source-feature-id`, `--new-feature-id`, `--source-domain`, `--source-service`, `--target-domain`, and `--target-service` arguments are validated against `SAFE_ID_PATTERN` (`[a-z0-9][a-z0-9._-]{0,63}`) at argument-parse time, before any subcommand logic runs. Invalid values cause exit code 1 with a descriptive error message. This check fires before any file system access.

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
| `lens.core/_bmad/lens-work/skills/bmad-lens-split-feature/SKILL.md` | Rewrite — thin conductor (must include post-move scan note per L3) | bmad-module-builder (BMB-first) |
| `lens.core/_bmad/lens-work/prompts/lens-split-feature.prompt.md` | Rewrite — release prompt (stub redirect) | bmad-workflow-builder |
| `.github/prompts/lens-split-feature.prompt.md` | Verify stub chain integrity | Direct (stub only) |
| `lens.core/_bmad/lens-work/skills/bmad-lens-split-feature/scripts/split-feature-ops.py` | Clean-room rewrite | bmad-module-builder / direct |
| `lens.core/_bmad/lens-work/skills/bmad-lens-split-feature/scripts/tests/test-split-feature-ops.py` | Create — regression test suite | direct |

### 4.2 References (owned by this skill — retain or update as needed)

| File | Action |
|------|--------|
| `lens.core/_bmad/lens-work/skills/bmad-lens-split-feature/references/validate-split.md` | Retain — behavior contract unchanged |
| `lens.core/_bmad/lens-work/skills/bmad-lens-split-feature/references/split-scope.md` | Retain — process steps unchanged |
| `lens.core/_bmad/lens-work/skills/bmad-lens-split-feature/references/split-stories.md` | Retain — story-move process unchanged |

### 4.3 Shared Dependencies (read-only — not modified in this feature)

| Dependency | Why Referenced |
|-----------|----------------|
| `lens.core/_bmad/lens-work/skills/bmad-lens-feature-yaml/` | Reads source feature.yaml; creates new feature.yaml schema |
| `lens.core/_bmad/lens-work/skills/bmad-lens-git-state/` | Cross-feature context on activation |
| `lens.core/_bmad/lens-work/scripts/light-preflight.py` | Frozen prompt-start gate |
| `lens.core/_bmad/lens-work/feature-index.yaml` | Updated by create-split-feature subcommand |

---

## 5. Clean-Room Implementation Notes

The old-codebase `split-feature-ops.py` is consulted as a behavioral reference only. The new implementation is authored from scratch and must satisfy the following contracts derived from the baseline PRD and architecture:

- `SAFE_ID_PATTERN` validation on all feature IDs passed as CLI arguments — path-constructing identifiers must match `[a-z0-9][a-z0-9._-]{0,63}`
- All YAML writes use atomic temp-file + rename to prevent partial-write corruption
- Status resolution order: sprint-plan file first, story-file frontmatter second; unrecognized sprint plan formats fall back to story-file frontmatter (never hard-stop)
- Status values are normalized before comparison — `in_progress`, `in-progress`, `IN_PROGRESS`, and `in progress` are equivalent
- Story files are treated as compatibility boundaries: the script moves files as-is without modifying content

**Behavioral reference path (BS-2):** The old-codebase implementation is located at `TargetProjects/lens-dev/old-codebase/lens.core.src/` in the control repo workspace. The governance-registered source for old-codebase discovery artifacts is the `lens-dev-old-codebase-discovery` feature (expressplan phase, archived). Implementing agents must locate the old-codebase `split-feature-ops.py` and `SKILL.md` at that path for output parity testing.

**SKILL.md post-move scan requirement (L3):** The rewritten `SKILL.md` must include an explicit note directing the Lens agent to scan all moved story files after `move-stories` completes and report any files whose `feature:` frontmatter field still references the source feature ID. No automatic rewrite — the scan result is surfaced to the user for manual resolution.

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
| Status delimiter normalization | `in_progress` (underscore), `IN_PROGRESS` (uppercase), `in progress` (space) all trigger the in-progress hard-stop identically to `in-progress` (hyphen) |
| Sprint plan format fallback | Sprint plan file with unrecognized format (non-YAML, non-fenced-block, non-inline-kv) falls back to story-file frontmatter without error; no stories are incorrectly marked blocked |
| Duplicate feature-index detection | `create-split-feature` with a `new-feature-id` already present in `feature-index.yaml` exits 1 with a duplicate-feature error before writing any file |

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

**Concurrent execution (L4):** Concurrent execution of `split-feature-ops.py` from two sessions targeting the same governance repo is unsupported. Feature authors must ensure only one split-feature session is active at a time. No file-lock mechanism is implemented; the atomic temp-file + rename pattern protects against single-process partial writes only.
