# What Module Registration Does

Module registration serves two purposes:

| Purpose               | What Happens                                                                              |
| --------------------- | ----------------------------------------------------------------------------------------- |
| **Configuration**     | Collects user preferences and writes them to shared config files                          |
| **Help registration** | Adds the module's capabilities to the project-wide help system so users can discover them |

## Why Register with the Help System?

The `bmad-help` skill reads `module-help.csv` to understand what capabilities are available, detect which ones have been completed (by checking output locations for artifacts), and recommend next steps based on the dependency graph. Without registration, `bmad-help` cannot discover or recommend your module's capabilities beyond what it knows basically from skill headers. The help system provides richer detail: arguments, relationships to other skills, inputs and outputs, and any other authored metadata. If a skill has multiple capabilities, each one gets its own help entry.

## Two Registration Paths

| Path                  | When to Use                                               | How It Works                                                                    |
| --------------------- | --------------------------------------------------------- | ------------------------------------------------------------------------------- |
| **Setup skill**       | Multi-skill modules (2+ skills)                           | A dedicated `{code}-setup` skill handles registration for all skills            |
| **Self-registration** | Single-skill standalone modules                           | The skill itself registers on first run or when user passes `setup`/`configure` |

The Module Builder detects which path to use based on what you give it: a folder of skills triggers the setup skill approach, a single skill triggers the standalone approach.
