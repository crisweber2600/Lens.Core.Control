# The Three Questions

For each candidate knob, ask:

1. **Does it vary naturally across the actual user population?** If every user wants roughly the same value, don't make it configurable. Pick the right default and move on.
2. **Is it the skill's identity, or something the skill consumes?** Identity stays baked. Consumed context (templates, facts, output paths, tone) is the right surface.
3. **Would hiding it force a fork, or just a sentence?** If the alternative is forking the whole skill, expose it. If the alternative is a one-line sentence the user can drop into `persistent_facts`, hide it.

Candidates that pass all three earn a place in `customize.toml`. Everything else stays baked, or gets folded into `persistent_facts` where sentence-shaped variance belongs.
