# Where This Shows Up in Your Build

Both the Agent Builder and the Workflow Builder ask the opt-in question during requirements gathering. If you say yes, a follow-up phase called Configurability Discovery walks you through candidate knobs (templates, output paths, hooks) and emits them into your skill's `customize.toml`. If you say no, workflows get no `customize.toml` at all, and agents get a metadata-only block.

The builders default the opt-in to **no** in headless mode unless you pass `--customizable`. Customization should be a deliberate decision, not an automatic one.
