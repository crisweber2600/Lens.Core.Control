# Skill Type Taxonomy

| Type                 | Description                                                                                                                   | Structure                                             |
| -------------------- | ----------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------- |
| **Simple Utility**   | Input/output building block. Headless, composable, often script-driven. May opt out of config loading for true standalone use | SKILL.md + `scripts/`                                 |
| **Simple Workflow**  | Multi-step process contained in a single SKILL.md. Loads config directly from module config.yaml. Minimal or no `prompts/`    | SKILL.md + optional `resources/`                      |
| **Complex Workflow** | Multi-stage with progressive disclosure, stage prompts in `prompts/`, config integration. May support headless mode           | SKILL.md (routing) + `prompts/` stages + `resources/` |
