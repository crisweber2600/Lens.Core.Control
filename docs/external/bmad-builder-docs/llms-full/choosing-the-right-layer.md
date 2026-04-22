# Choosing the Right Layer

| Situation                                      | Recommended Layer             |
| ---------------------------------------------- | ----------------------------- |
| Single-purpose utility with one path           | Layer 1-2                     |
| Skill with conditional reference data          | Layer 2                       |
| Skill that does multiple distinct things       | Layer 3                       |
| Skill with stages that depend on each other    | Layer 3 + compaction survival |
| Strict sequential process, no skipping allowed | Layer 4                       |
| Long-running workflow producing a document     | Layer 3 + document-as-cache   |
</document>

<document path="explanation/scripts-in-skills.md">
Scripts handle work that has clear right-and-wrong answers (validation, transformation, extraction, counting) so the LLM can focus on judgment, synthesis, and creative reasoning.
