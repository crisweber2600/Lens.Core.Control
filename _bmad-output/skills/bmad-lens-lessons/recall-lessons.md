---
name: recall-lessons
description: Retrieve relevant task-level lessons before starting a task or list all stored lessons.
---

# Recall Lessons

## Step 1 — Determine Mode

- **recall** — find lessons relevant to an upcoming task (default)
- **list** — show all lessons as a summary table (when user says "show all" or "list lessons")

## Step 2 — Read the Lessons Store

Use the `memory` tool:
```
memory view /memories/lessons-learned.yaml
```

If the file does not exist or is empty, output:
```
No lessons stored yet. Use 'log a lesson' after completing a task to begin building the knowledge base.
```

## Step 3a — Recall Mode: Map Intent to Filter Criteria

From the user's task description or context, identify:

- **task_type** — what kind of task will be performed? (e.g. `git-merge`, `code-review`)
- **tags** — relevant technology or domain keywords
- **keywords** — any free-text terms to search across context and lesson fields

Pass the lessons YAML to the filter script via stdin:

```bash
echo "{lessons_yaml}" | python3 scripts/lessons.py filter \
  [--task-type "{task_type}"] \
  [--tags "{tag1},{tag2}"] \
  [--keywords "{keyword1},{keyword2}"] \
  [--severity {severity}] \
  [--limit 10]
```

Omit any flag where you have no relevant criteria. Start broad (task_type only), then narrow if too many results return.

**Result handling:**
- If 0 matches: try again with only keywords, then broaden to no filter. If still empty, output "No relevant lessons found." 
- If 1-5 matches: output the full YAML block — agents consume this directly.
- If 6+ matches: re-run with `--limit 5` and note the total count.

## Step 3b — List Mode

Pass the lessons YAML to the list script via stdin:

```bash
echo "{lessons_yaml}" | python3 scripts/lessons.py list \
  [--task-type "{task_type}"] \
  [--limit {n}]
```

Output the table as-is.

## Step 4 — Output Format

**Recall mode (agent-consumable):**

```
## Relevant Lessons for: {task_type}

{filtered_yaml_block}
```

No prose commentary. The agent reads the YAML directly.

**Recall mode (human-requested):**

Output the filtered YAML followed by a one-line summary:
```
{N} lesson(s) found for task_type={task_type}.
```

**List mode:** Output the table as printed by the script.

---

## Filter Strategy Reference

| User says | Filter approach |
|-----------|----------------|
| "before I do a PR merge" | `--task-type git-merge` |
| "reviewing Python security" | `--tags security,python` |
| "anything about postgres migrations" | `--keywords postgres --task-type schema-migration` |
| "all critical lessons" | `--severity critical` |
| "show all lessons" | list mode (no filter) |
| "lessons from last sprint" | `--limit 20` (most recent 20) |
