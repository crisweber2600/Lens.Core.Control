# When to Reach for a Script

Look for these signal verbs in a skill's requirements; they indicate script opportunities:

| Signal                             | Script Type      |
| ---------------------------------- | ---------------- |
| "validate", "check", "verify"      | Validation       |
| "count", "tally", "aggregate"      | Metrics          |
| "extract", "parse", "pull from"    | Data extraction  |
| "convert", "transform", "format"   | Transformation   |
| "compare", "diff", "match against" | Comparison       |
| "scan for", "find all", "list all" | Pattern scanning |

The builders guide you through script opportunity discovery during the build process. If you find yourself writing detailed validation logic in a prompt, it almost certainly belongs in a script instead.
</document>

<document path="explanation/skill-authoring-best-practices.md">
Practical guidance for writing skills that work reliably and adapt gracefully. These patterns apply to agents, workflows, and utilities alike.
