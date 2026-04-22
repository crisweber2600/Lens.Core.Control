# Reference

| Resource                                                 | Description                                           |
| -------------------------------------------------------- | ----------------------------------------------------- |
| **[Builder Commands](/reference/builder-commands.md)**   | All capabilities, modes, and phases for both builders |
| **[Workflow Patterns](/reference/workflow-patterns.md)** | Skill types, structure patterns, and execution models |
</document>

<document path="explanation/module-configuration.md">
BMad modules register their capabilities with the help system and optionally collect user preferences. Multi-skill modules use a dedicated **setup skill** for this. Single-skill standalone modules handle registration themselves on first run.

When you create your own module, you can either add a configuration skill or embed the feature in every skill following the standalone pattern. For modules with more than 1-2 skills, a setup skill is the better choice.
