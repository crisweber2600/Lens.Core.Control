# When You Need Configuration

Most modules should not need configuration at all. Before adding configurable values, consider whether a simpler alternative exists.

| Approach              | When to Use                                                                                                                                               |
| --------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Sensible defaults** | The variable has one clearly correct answer for most users that could be overridden or updated by the specific skill that needs it the first time it runs |
| **Agent memory**      | Your module follows the agent pattern and the agent can learn preferences through conversation                                                            |
| **Configuration**     | The value genuinely varies across projects and cannot be inferred at runtime                                                                              |

:::tip[Standalone Skills]
If you are building a single standalone agent or workflow, you do not need a separate setup skill. The Module Builder can package it as a **standalone self-registering module** where the registration logic is embedded directly in the skill via an `assets/module-setup.md` reference file, and runs on first activation or when the user passes `setup`/`configure`.
:::
