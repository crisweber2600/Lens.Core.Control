# Layer 4: Step Files

The most restrictive pattern. The agent reads **one step file at a time**, does not know what is next, and waits for user confirmation before proceeding.

```
prompts/
├── step-01.md  ← agent reads ONLY current step
├── step-02.md  ← loaded after user confirms step 1
├── step-03a.md ← branching path A
└── step-03b.md ← branching path B
```

**When to use:** Only when you need exact sequential progression with no skipping, compaction-resistance (each step is self-contained), or the agent deliberately constrained from looking ahead.

**Trade-off:** Very rigid. Limits the agent's ability to adapt, combine steps, or be creative. Do not use for exploratory or creative tasks. Do not use when Layer 3 routing would suffice. Try to follow level 1-3 first! The lowest level needed is best.

:::tip[Start at Layer 2]
Most skills only need Layers 1-2. Add Layer 3 when the skill genuinely handles multiple distinct operations. Add Layer 4 only for strict compliance or audit workflows where the agent must not skip ahead.
:::
