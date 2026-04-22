# The module-help.csv File

The CSV registers the module's capabilities with the help system. Each row describes one capability that users can discover and invoke. The file has 13 columns:

```csv
module,skill,display-name,menu-code,description,action,args,phase,after,before,required,output-location,outputs
```

## Column Guide

| Column              | Purpose                                                                                                                                      |
| ------------------- | -------------------------------------------------------------------------------------------------------------------------------------------- |
| **module**          | Module display name. Groups entries in help output                                                                                          |
| **skill**           | Skill folder name (e.g., `bmad-agent-builder`); must match the actual directory name                                                        |
| **display-name**    | User-facing label shown in help menus (e.g., "Build an Agent")                                                                               |
| **menu-code**       | 1-3 letter shortcode displayed as `[CODE]` in help, unique across the module, intuitive mnemonic                                            |
| **description**     | What this capability does. Concise, action-oriented, specific enough for `bmad-help` to route correctly                                     |
| **action**          | Action name within the skill. Distinguishes capabilities when one skill exposes multiple (e.g., `build-process`, `quality-optimizer`)        |
| **args**            | Arguments the capability accepts (e.g., `[-H] [path]`), shown in help output                                                               |
| **phase**           | When the capability is available: `anytime` or a workflow phase like `1-analysis`, `2-planning`                                             |
| **after**           | Capabilities that should complete before this one: format `skill-name:action`, comma-separated for multiple                                  |
| **before**          | Capabilities that should run after this one, same format as `after`                                                                         |
| **required**        | `true` if this is a blocking gate for phase progression, `false` otherwise                                                                   |
| **output-location** | Config variable name (e.g., `output_folder`, `bmad_builder_reports`); `bmad-help` resolves from config to scan for completion artifacts     |
| **outputs**         | File patterns `bmad-help` looks for in the output location to detect completion (e.g., "quality report", "agent skill")                      |

## How bmad-help Uses These Entries

The `after`/`before` columns create a **dependency graph** that `bmad-help` walks to recommend next steps. `required=true` entries are blocking gates; `bmad-help` will not suggest later-phase capabilities until required gates pass. The `output-location` and `outputs` columns enable **completion detection**: `bmad-help` scans those paths for matching artifacts to determine what's been done.

## Example Entry

```csv
module,skill,display-name,menu-code,description,action,args,phase,after,before,required,output-location,outputs
BMad Builder,bmad-agent-builder,Build an Agent,BA,"Create, edit, convert, or fix an agent skill.",build-process,"[-H] [description | path]",anytime,,bmad-agent-builder:quality-optimizer,false,output_folder,agent skill
```

During registration, these rows are merged into the project-wide `_bmad/module-help.csv`, replacing any existing rows for this module (anti-zombie pattern).
