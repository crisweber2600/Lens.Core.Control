# Layer 2: On-Demand Resources

SKILL.md points to resources loaded only when relevant. This includes both **reference files** (context for the LLM) and **scripts** (offload work from the LLM entirely).

```markdown
# Which Guide to Read

- Python project → Read `resources/python.md`
- TypeScript project → Read `resources/typescript.md`
- Need validation → Run `scripts/validate.py` (don't read the script, just run it)
```

Scripts are particularly powerful here: the LLM does not process the logic, it just calls the script and receives structured output. This offloads deterministic work and saves tokens.
