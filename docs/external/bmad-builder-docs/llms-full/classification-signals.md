# Classification Signals

## Simple Utility

- Clear input → processing → output pattern
- No user interaction needed during execution
- Other skills and workflows call it
- Deterministic or near-deterministic behavior
- Could be a script but needs LLM judgment
- Examples: JSON validator, format converter, file structure checker

## Simple Workflow

- 3-8 numbered steps
- User interaction at specific points
- Uses standard tools (gh, git, npm, etc.)
- Produces a single output artifact
- No need to track state across compactions
- Examples: PR creator, deployment checklist, code review

## Complex Workflow

- Multiple distinct phases or stages
- Long-running (likely to hit context compaction)
- Progressive disclosure needed (too much for one file)
- Routing logic in SKILL.md dispatches to stage prompts
- Produces multiple artifacts across stages
- May support headless/autonomous mode
- Examples: agent builder, module builder, project scaffolder
