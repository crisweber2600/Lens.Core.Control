# Design Guidance

Configuration is for **basic, project-level settings**: output folders, language preferences, feature toggles. Keep the number of configurable values small.

| Pattern                | Configuration Role                                                                                              |
| ---------------------- | --------------------------------------------------------------------------------------------------------------- |
| **Agent pattern**      | Prefer agent memory for per-user preferences. Use config only for values that must be shared across the project |
| **Workflow pattern**   | Use config for output locations and behavior switches that vary across projects                                 |
| **Skill-only pattern** | Use config sparingly. If the skill works with sensible defaults, skip config entirely                           |

Extensive workflow customization (step overrides, conditional branching, template selection) is a separate concern and will be covered in a dedicated document.
