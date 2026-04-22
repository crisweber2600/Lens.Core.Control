# Quality Dimensions

Six dimensions to keep in mind during the build phase. The quality scanners check these automatically during optimization.

| Dimension                  | What It Means                                                                                                                                                                                        |
| -------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Informed Autonomy**      | Overview establishes domain framing, theory of mind, and design rationale, enough for judgment calls                                                                                                |
| **Intelligence Placement** | Scripts handle plumbing (fetch, transform, validate). Prompts handle judgment (interpret, classify, decide). If a script contains an `if` that decides what content _means_, intelligence has leaked |
| **Progressive Disclosure** | SKILL.md stays focused; stage instructions go in `prompts/`, reference data in `resources/`                                                                                                          |
| **Description Format**     | Two parts: `[5-8 word summary]. [Use when user says 'X' or 'Y'.]`. Default to conservative triggering                                                                                               |
| **Path Construction**      | Use `{project-root}` for any project-scope path and `./` for same-folder references inside a skill. Cross-directory skill-internal paths are bare (e.g. `references/foo.md`). Config variables already contain `{project-root}`, so never double-prefix them                              |
| **Token Efficiency**       | Remove genuine waste (repetition, defensive padding). Preserve context that enables judgment (domain framing, rationale)                                                                             |
