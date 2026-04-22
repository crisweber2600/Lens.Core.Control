# Understanding Modules

A BMad module bundles skills so they're discoverable and configurable. The Module Builder offers two approaches depending on what you're building:

| Approach              | When to Use                                  | What Gets Generated                                             |
| --------------------- | -------------------------------------------- | --------------------------------------------------------------- |
| **Setup skill**       | Folder of 2+ skills                          | Dedicated `{code}-setup` skill with config and help assets      |
| **Self-registration** | Single standalone skill                      | Registration embedded in the skill's own `assets/` folder       |

Both produce the same registration artifacts: `module.yaml` (identity and config variables) and `module-help.csv` (capability entries), which register with `bmad-help`.

See **[What Are Modules](/explanation/what-are-modules.md)** for the architecture behind these choices.
