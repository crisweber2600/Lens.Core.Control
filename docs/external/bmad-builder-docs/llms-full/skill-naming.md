# Skill Naming

| Context        | Agent Pattern              | Workflow Pattern       |
| -------------- | -------------------------- | ---------------------- |
| **Standalone** | `agent-{name}`             | `{name}`               |
| **Module**     | `{modulecode}-agent-{name}`| `{modulecode}-{name}`  |

Names must be kebab-case and match the folder name. Agents should include `agent` in the name. For module-based skills, the user chooses the module code prefix during the build.

:::caution[Reserved Prefix]
The `bmad-` prefix is reserved for official BMad creations. User-built skills should not include it. If converting a skill that already has a `bmad-` prefix, retain it unless the user requests a rename.
:::
