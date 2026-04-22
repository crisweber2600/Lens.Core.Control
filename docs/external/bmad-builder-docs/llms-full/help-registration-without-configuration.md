# Help Registration Without Configuration

You may not need any configurable values but still want to register your module with the help system. Registration is still worthwhile when:

- The skill description in SKILL.md frontmatter cannot fully convey what the module offers while staying concise
- You want to express capability sequencing, phase constraints, or other metadata the CSV supports
- An agent has many internal capabilities that users should be able to discover
- Your module has more than about three distinct things it can do

For simpler cases, these alternatives are often sufficient:

| Alternative                   | What It Provides                                                                                                                         |
| ----------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------- |
| **SKILL.md overview section** | A concise summary at the top of the skill body; the `--help` system scans this section to present user-facing help, so keep it succinct |
| **Script header comments**    | Describe purpose, usage, and flags at the top of each script                                                                             |

If these cover your discoverability needs, you can skip the setup skill entirely.
