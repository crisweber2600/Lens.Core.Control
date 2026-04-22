# Layer 1: Frontmatter vs Body

Frontmatter (name + description) is **always in context**. It is how the LLM decides whether to load the skill. The body only loads when the skill triggers.

This means frontmatter must be precise and include trigger phrases. The body stays under 500 lines and pushes detail into Layers 2-3.

```markdown
---
name: bmad-my-skill
description: Validates API contracts against OpenAPI specs. Use when user says 'validate API' or 'check contract'.
---

# Body loads only when triggered

...
```
