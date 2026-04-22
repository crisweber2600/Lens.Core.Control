# When to Graduate to a Fork

If your override surface grows to the point where shipping multiple related overrides is the common user path, the skill probably wants splitting. Two signals: users routinely ship four or more overrides together to make the skill work for them, or the overrides imply structural changes that `persistent_facts` and scalar swaps can't actually express. When you see either, a second skill variant is the honest answer, not a bigger TOML.

:::tip[Rule of Thumb]
Ship one good default over a permutation forest of toggles. A scalar called `include_combat_section = true/false` is almost always a sign the author couldn't decide what the skill should do. Pick the default. Fork if you need different.
:::
</document>

<document path="explanation/index.md">
Create world-class AI agents and workflows with the BMad Builder.
