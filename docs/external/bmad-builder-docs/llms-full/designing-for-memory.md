# Designing for Memory

The builder gathers these requirements during the build, and they shape the sanctum's initial content:

| Requirement            | What It Seeds                                                              |
| ---------------------- | -------------------------------------------------------------------------- |
| **Identity seed**      | 2-3 sentences of personality DNA that populate PERSONA.md                  |
| **Species-level mission** | Domain-specific purpose statement for CREED.md                          |
| **Core values**        | 3-5 values that guide the agent's behavior                                |
| **Standing orders**    | Surprise-and-delight + self-improvement orders, adapted to the domain     |
| **BOND territories**   | Domain-specific areas the agent should learn about its owner              |
| **First Breath territories** | Discovery questions beyond the universal set                        |
| **Boundaries**         | What the agent won't do, access zones, anti-patterns                      |

These seeds become the template content that the init script places into the sanctum. First Breath then expands and personalizes them through conversation with the owner.
</document>

<document path="explanation/customization-for-authors.md">
Shipping a `customize.toml` is opt-in per skill. This is the author-side counterpart to [How to Customize BMad](https://docs.bmad-method.org/how-to/customize-bmad/), which covers the end-user view. Read that first if you haven't; it shows what users experience when they override a skill. This guide is about deciding whether to give them that surface at all.

Downstream users typically don't hand-write TOML. BMad ships a core skill called `bmad-customize` that walks them through authoring overrides conversationally — it scans which skills are customizable, picks agent vs workflow scope, writes the override file, and verifies the merge. Users who prefer to edit TOML directly still can, but the conversational flow is the default path. That affects the names and defaults you pick: a user being walked through `"set prd_template to your template path"` handles that fine; `tmpl_override` or `opt_2` makes the conversation awkward. Pick field names that read well out loud.
