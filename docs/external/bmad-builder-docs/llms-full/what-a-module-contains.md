# What a Module Contains

| Component           | Multi-Skill Module                                      | Standalone Module                                          |
| ------------------- | ------------------------------------------------------- | ---------------------------------------------------------- |
| **Skills**          | Two or more agents/workflows                            | A single agent or workflow                                 |
| **Registration**    | Dedicated `{code}-setup` skill                          | Built into the skill itself (`assets/module-setup.md`)     |
| **module.yaml**     | In the setup skill's `assets/`                          | In the skill's own `assets/`                               |
| **module-help.csv** | In the setup skill's `assets/`                          | In the skill's own `assets/`                               |
| **Distribution**    | Plugin with multiple skill folders                      | Plugin with single skill folder + `marketplace.json`       |

For multi-skill modules, the setup skill is the glue; it registers all capabilities in one step. For standalone modules, the skill handles its own registration on first run or when the user passes `setup`/`configure`.
