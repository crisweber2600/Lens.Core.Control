# Quality Optimize (QO)

Validation and optimization for existing skills. Runs deterministic lint scripts for instant structural checks and LLM scanner subagents for judgment-based analysis, all in parallel.

## Pre-Scan Checks

In interactive mode, the optimizer:

1. Checks for uncommitted changes and recommends committing first
2. Asks if the skill is currently working as expected

In autonomous mode, both checks are skipped and noted as warnings in the report.

## Scan Pipeline

The optimizer runs three tiers of analysis.

**Tier 1: Lint scripts** (deterministic, zero tokens, instant):

| Script                   | Focus                            |
| ------------------------ | -------------------------------- |
| `scan-path-standards.py` | Path convention violations       |
| `scan-scripts.py`        | Script portability and standards |

**Tier 2: Pre-pass scripts** (extract metrics for LLM scanners):

| Script                        | Agent Builder                       | Workflow Builder                |
| ----------------------------- | ----------------------------------- | ------------------------------- |
| Structure/integrity pre-pass  | `prepass-structure-capabilities.py` | `prepass-workflow-integrity.py` |
| Prompt metrics pre-pass       | `prepass-prompt-metrics.py`         | `prepass-prompt-metrics.py`     |
| Execution dependency pre-pass | `prepass-execution-deps.py`         | `prepass-execution-deps.py`     |

**Tier 3: LLM scanners** (judgment-based, run as parallel subagents):

| Scanner                       | Agent Builder Focus                                                        | Workflow Builder Focus                                                                       |
| ----------------------------- | -------------------------------------------------------------------------- | -------------------------------------------------------------------------------------------- |
| **Structure / Integrity**     | Structure, capabilities, identity, memory setup, consistency               | Logical consistency, description quality, progression conditions, type-appropriate structure |
| **Prompt Craft**              | Token efficiency, anti-patterns, persona voice, overview quality           | Token efficiency, anti-patterns, overview quality, progressive disclosure                    |
| **Execution Efficiency**      | Parallelization, subagent delegation, memory loading, context optimization | Parallelization, subagent delegation, read avoidance, context optimization                   |
| **Cohesion**                  | Persona-capability alignment, gaps, redundancies                           | Stage flow coherence, purpose alignment, complexity appropriateness                          |
| **Enhancement Opportunities** | Script automation, autonomous potential, edge cases, delight               | Creative edge-case discovery, experience gaps, assumption auditing                           |

## Report Synthesis

After all scanners complete, the optimizer synthesizes results into a unified report saved to `{bmad_builder_reports}/{skill-name}/quality-scan/{timestamp}/`.

In interactive mode, it presents a summary with severity counts and offers next steps:

- Apply fixes directly
- Export checklist for manual fixes
- Discuss specific findings

In autonomous mode, it outputs structured JSON with severity counts and the report file path.

## Optimization Guidance

Not every suggestion should be applied. The optimizer communicates these decision rules:

- **Keep phrasing** that captures the intended voice. Leaner is not always better for persona-driven skills
- **Keep content** that adds clarity for the AI even if a human finds it obvious
- **Prefer scripting** for deterministic operations; **prefer prompting** for creative or judgment-based tasks
- **Reject changes** that flatten personality unless a neutral tone is explicitly wanted
