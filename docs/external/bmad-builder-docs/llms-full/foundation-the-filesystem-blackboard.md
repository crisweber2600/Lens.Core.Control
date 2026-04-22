# Foundation: The Filesystem Blackboard

Every pattern below builds on this infrastructure. The filesystem acts as a shared database so the parent never bloats its context.

```
/output/
├── status.json        ← task states, completion flags
├── knowledge.md       ← accumulated findings (append-only)
└── task-queue.json    ← pending work items

/tasks/{id}/
├── input.md           ← instructions for this subagent
└── output/
    ├── result.json    ← structured output (strict schema)
    └── summary.md     ← compact summary (≤200 tokens)

/artifacts/            ← final deliverables
```

One technique is to have every subagent prompt ends the same way: _"You are stateless. Read ONLY the files listed. Write ONLY result.json + summary.md. Do not echo data back."_
