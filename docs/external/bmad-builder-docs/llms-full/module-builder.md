# Module Builder

The Module Builder (`bmad-module-builder`) handles module-level planning, scaffolding, and validation. It operates at a higher level than the Agent and Workflow Builders; it orchestrates what those builders produce into a cohesive, installable module.

## Capabilities Overview

| Capability          | Menu Code | What It Does                                                                                                    |
| ------------------- | --------- | --------------------------------------------------------------------------------------------------------------- |
| **Ideate Module**   | IM        | Brainstorm and plan a module through creative facilitation                                                      |
| **Create Module**   | CM        | Package skills as an installable module: setup skill for multi-skill, self-registration for standalone           |
| **Validate Module** | VM        | Check structural integrity and entry quality for both multi-skill and standalone modules                        |

## Ideate Module (IM)

A brainstorming session that helps you plan your module from scratch. The builder acts as a creative collaborator, drawing out ideas, exploring possibilities, and guiding you toward the right architecture.

| Aspect          | Detail                                          |
| --------------- | ----------------------------------------------- |
| **Interaction** | Interactive only; no headless mode              |
| **Input**       | An idea or rough description                    |
| **Output**      | Plan document saved to `{bmad_builder_reports}` |

**What it covers:**

- Problem space exploration and creative brainstorming
- Architecture decision: single agent with capabilities vs. multiple skills vs. hybrid
- Standalone module or expansion of an existing module
- External dependencies (CLI tools, MCP servers)
- UI and visualization opportunities
- Setup skill extensions beyond configuration
- Per-skill capability definitions with help CSV metadata
- Configuration variables and sensible defaults

The plan document uses a resumable template with YAML frontmatter, so long brainstorming sessions survive context compaction.

**After ideation:** Build each planned skill using the Agent Builder (BA) or Workflow Builder (BW), then return to Create Module (CM) to scaffold the module.

## Create Module (CM)

Packages built skills as an installable BMad module. Auto-detects single-skill vs. multi-skill input and recommends the appropriate approach. Supports `--headless` / `-H`.

| Aspect          | Detail                                                                                      |
| --------------- | ------------------------------------------------------------------------------------------- |
| **Interaction** | Guided or headless                                                                          |
| **Input**       | Path to a skills folder or single skill (or SKILL.md file), optional plan document          |
| **Output**      | Setup skill for multi-skill modules, or self-registration files for standalone modules      |

**What it does:**

1. Reads the SKILL.md files to understand each skill
2. Detects single vs. multi-skill and confirms the packaging approach with the user
3. Collects module identity (name, code, description, version, greeting)
4. Defines help CSV entries: capabilities, menu codes, ordering, relationships
5. Captures configuration variables and external dependencies
6. Scaffolds the module infrastructure

**Multi-skill output:** A dedicated `{code}-setup/` folder with merge scripts, cleanup scripts, and a generic SKILL.md.

**Standalone output:** `assets/module-setup.md`, `assets/module.yaml`, and `assets/module-help.csv` embedded in the skill, plus merge scripts in `scripts/` and a `.claude-plugin/marketplace.json` for distribution. The skill's SKILL.md is updated to check for registration on activation.

## Validate Module (VM)

Verifies that a module's structure is complete and accurate. Auto-detects multi-skill modules (with setup skill) and standalone modules (with self-registration). Combines a deterministic validation script with LLM-based quality assessment.

| Aspect          | Detail                                                 |
| --------------- | ------------------------------------------------------ |
| **Interaction** | Interactive                                            |
| **Input**       | Path to the module's skills folder or single skill     |
| **Output**      | Validation report                                      |

**Structural checks** (script-driven):

| Check                  | What It Catches                                                                             |
| ---------------------- | ------------------------------------------------------------------------------------------- |
| Module structure       | Missing setup skill or standalone files (`module-setup.md`, merge scripts)                  |
| Coverage               | Skills without CSV entries, orphan entries for nonexistent skills                           |
| Menu codes             | Duplicate codes across the module                                                           |
| References             | Before/after fields pointing to nonexistent capabilities                                    |
| Required fields        | Missing skill name, display name, menu code, or description in CSV rows                     |
| module.yaml            | Missing code, name, or description                                                          |

**Quality assessment** (LLM-driven):

- Description accuracy: does each entry match what the skill actually does?
- Description quality: concise, action-oriented, specific, not overly verbose
- Completeness: are all distinct capabilities registered as separate rows?
- Ordering: do before/after relationships make sense?
- Menu codes: are they intuitive and memorable?
