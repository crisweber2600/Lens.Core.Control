---
name: log-lesson
description: Extract and persist a task-level lesson after completing a task.
---

# Log a Lesson

## Step 1 — Extract the Lesson

From the task narrative, conversation, or explicit input, extract the following fields:

- **task_type** — what kind of task was this? Use a short kebab identifier like `git-merge`, `code-review`, `schema-migration`, `ci-debug`, `pr-creation`, `dependency-update`, `deploy`. If the task doesn't fit a known type, invent a clear kebab name.
- **tags** — 1-4 searchable keywords. Think: technology name, domain, pattern type, error class (e.g. `git`, `conflict`, `yaml`, `postgres`, `security`, `rebase`).
- **context** — 1-2 sentences: when does this lesson apply? What situation triggers it?
- **lesson** — the actionable core: what should the agent do (or avoid)? Specific. Imperative. No hedging.
- **severity** — `tip` (optional improvement), `warning` (avoid this mistake), `critical` (must follow).
- **source_skill** — which skill is logging this? Use the active skill name, or leave blank.

**Extraction guidance:**
- If the user says "the lesson is X" — use that directly.
- If the user describes what happened — infer the lesson: what would a clean repeat of this task do differently?
- Reject vague lessons: "be careful with git" → "always run `git fetch --prune` before resolving remote-tracking conflicts".

## Step 2 — Format the Entry

Run the format script to produce a validated YAML entry:

```bash
python3 scripts/lessons.py format \
  --task-type "{task_type}" \
  --tags "{tag1},{tag2}" \
  --context "{context}" \
  --lesson "{lesson}" \
  --source-skill "{source_skill}" \
  --severity {severity}
```

The script outputs one YAML entry block to stdout.

## Step 3 — Persist

Read the current lessons store using the `memory` tool:
```
memory view /memories/lessons-learned.yaml
```

If the file does not exist yet, create it:
```yaml
lessons: []
```

Append the new entry under the `lessons` list. The result should look like:
```yaml
lessons:
  - id: lesson-...
    date: "..."
    task_type: ...
    tags: [...]
    context: ...
    lesson: ...
    source_skill: ...
    severity: ...
  # ... existing entries
```

Write back using:
```
memory str_replace /memories/lessons-learned.yaml
```
If this is the first entry, use `memory create` instead.

## Step 4 — Confirm

Emit exactly one line:
```
Logged: {id} [{task_type}/{severity}]
```

In headless mode: emit only the lesson YAML entry (no confirmation line).

---

## Edge Cases

- **Duplicate lesson:** If an existing entry is nearly identical (same lesson text + same task_type), skip and output: `Skipped: duplicate of {existing_id}`.
- **Missing required field:** If you cannot determine task_type, context, or lesson from the input, ask once: "What was the task type / key context / core takeaway?" — then proceed.
- **Parsing failure in script:** Check stderr and correct the argument format before retrying.
