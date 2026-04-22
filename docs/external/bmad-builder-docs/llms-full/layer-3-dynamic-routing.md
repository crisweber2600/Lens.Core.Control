# Layer 3: Dynamic Routing

The skill body acts as a **router** that dispatches to entirely different prompt flows, scripts, or external skills based on what the user is asking for.

```markdown
# What Are You Trying To Do?

## "Build a new workflow"

→ Read `prompts/create-flow.md` and follow its instructions

## "Review an existing workflow"

→ Read `prompts/review-flow.md` and follow its instructions

## "Run analysis"

→ Run `scripts/analyze.py --target <path>` and present results
```

The key difference from Layer 2: Layer 2 loads supplementary resources alongside the skill body. Layer 3 **branches the entire execution path**: different prompts, different scripts, different skills. The skill body becomes a dispatcher, not an instruction set.
