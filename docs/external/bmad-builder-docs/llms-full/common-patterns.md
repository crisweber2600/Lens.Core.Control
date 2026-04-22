# Common Patterns

## Soft Gate Elicitation

For guided workflows, use "anything else?" soft gates at natural transition points instead of hard menus.

```markdown
Present what you've captured so far, then:
"Anything else you'd like to add, or shall we move on?"
```

Users almost always remember one more thing when given a graceful exit ramp rather than a hard stop. This consistently produces richer artifacts than rigid section-by-section questioning. Use at every natural transition in collaborative discovery workflows. Skip in autonomous/headless execution.

## Intent-Before-Ingestion

Never scan artifacts or project context until you understand WHY the user is here. Without knowing intent, you cannot judge what is relevant in a 100-page document.

```markdown
1. Greet and understand intent
2. Accept whatever inputs the user offers
3. Ask if they have additional context
4. ONLY THEN scan artifacts, scoped to relevance
```

## Capture-Don't-Interrupt

When users provide information beyond the current scope (dropping requirements during a product brief, mentioning platforms during vision discovery), capture it silently for later use rather than redirecting them.

Users in creative flow share their best insights unprompted. Interrupting to say "we'll cover that later" kills momentum and may lose the insight entirely.

## Dual-Output: Human Artifact + LLM Distillate

Any artifact-producing workflow can output two complementary documents: a polished human-facing artifact AND a token-conscious, structured distillate optimized for downstream LLM consumption.

| Output         | Purpose                                                                                                                                                                                |
| -------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Primary**    | Human-facing document: concise, well-structured                                                                                                                                       |
| **Distillate** | Dense, structured summary for downstream LLM workflows: captures overflow, rejected ideas (so downstream does not re-propose them), detail bullets with enough context to stand alone |

The distillate bridges the gap between what belongs in the human document and what downstream workflows need. Always offered to the user, never forced.

## Three-Mode Architecture

Interactive workflows can offer three execution modes matching different user contexts.

| Mode                      | Trigger                     | Behavior                                                                                 |
| ------------------------- | --------------------------- | ---------------------------------------------------------------------------------------- |
| **Guided**                | Default                     | Section-by-section with soft gates; drafts from what it knows, questions what it doesn't |
| **YOLO**                  | `--yolo` or "just draft it" | Ingests everything, drafts complete artifact upfront, then walks user through refinement |
| **Headless (Autonomous)** | `--headless` / `-H`         | Headless; takes inputs, produces artifact, no interaction                                |

Not every workflow needs all three, but considering them during design prevents painting yourself into a single interaction model.

## Parallel Review Lenses

Before finalizing any significant artifact, fan out multiple reviewers with different perspectives.

| Reviewer                | Focus                                                                                                 |
| ----------------------- | ----------------------------------------------------------------------------------------------------- |
| **Skeptic**             | What is missing? What assumptions are untested?                                                       |
| **Opportunity Spotter** | What adjacent value? What angles?                                                                     |
| **Contextual**          | LLM picks the best third lens for the domain (regulatory risk for healthtech, DX critic for devtools) |

Graceful degradation: if subagents are unavailable, the main agent does a single critical self-review pass.

## Graceful Degradation

Every subagent-dependent feature should have a fallback path. Skills run across different platforms, models, and configurations. A skill that hard-fails without subagents is fragile. One that falls back to sequential processing works everywhere.

## Verifiable Intermediate Outputs

For complex tasks: plan, validate, execute, verify.

1. Analyze inputs
2. Create `changes.json` with planned updates
3. Validate with script before executing
4. Execute changes
5. Verify output

Catches errors early, is machine-verifiable, and makes planning reversible.
