# Freedom Levels

Match specificity to task fragility.

| Freedom                           | When to Use                                  | Example                                                       |
| --------------------------------- | -------------------------------------------- | ------------------------------------------------------------- |
| **High** (text instructions)      | Multiple valid approaches, context-dependent | "Analyze structure, check for issues, suggest improvements"   |
| **Medium** (pseudocode/templates) | Preferred pattern exists, some variation OK  | `def generate_report(data, format="markdown"):`               |
| **Low** (exact scripts)           | Fragile operations, consistency critical     | `python scripts/migrate.py --verify --backup` (do not modify) |

**Analogy:** Narrow bridge with cliffs = low freedom. Open field = high freedom.
