# What You Get

When you opt in, your built skill folder includes:

```text
{skill-name}/
├── SKILL.md            # references {workflow.X} or {agent.X} for customized values
├── customize.toml      # your defaults, the canonical schema
├── references/
├── scripts/
└── assets/
```

Users get:

- A documented override surface via `customize.toml`
- Team-scoped overrides via `_bmad/custom/{skill-name}.toml`
- Personal-scoped overrides via `_bmad/custom/{skill-name}.user.toml`
- Automatic precedence handling from the resolver (user beats team beats defaults)
- A conversational authoring path: the `bmad-customize` core skill scans which skills are customizable, helps the user pick agent vs workflow scope, writes the override file, and verifies the merge. Users who prefer to hand-write TOML still can.
