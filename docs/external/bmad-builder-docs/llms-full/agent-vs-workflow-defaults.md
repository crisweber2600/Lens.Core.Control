# Agent vs Workflow Defaults

Agents and workflows enter the customize.toml question from different starting points.

| Surface | Metadata block | Override surface | Notes |
| --- | --- | --- | --- |
| Agent | Always required | Opt-in | Metadata feeds `module.yaml:agents[]` and the central agent roster. |
| Workflow | Not required | Fully opt-in | No roster. If you don't opt in, no `customize.toml` is emitted at all. |

For agents, you always ship `customize.toml` (the roster depends on it). The real question is whether it carries an override surface beyond metadata. For workflows, the choice is binary: ship one or don't.
