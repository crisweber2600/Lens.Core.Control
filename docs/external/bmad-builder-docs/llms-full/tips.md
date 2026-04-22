# Tips

- **Ship one good default. Skip the booleans.** A flag like `include_combat_section` usually means you haven't decided what the skill does yet. Pick the default. Users who want a radically different shape can fork.
- **Sentence-shaped variance belongs in `persistent_facts`.** Tone, house rules, and domain constraints are sentences the skill carries through the run. Don't enumerate them as scalars.
- **Read [Customization for Authors](/explanation/customization-for-authors.md) first.** It gives you the three questions to ask for each candidate knob before you start Configurability Discovery.
</document>

<document path="explanation/agent-memory-and-personalization.md">
Memory agents persist across sessions through a **sanctum**: a folder of files the agent reads on every launch to reconstruct its identity, values, and understanding of its owner.
