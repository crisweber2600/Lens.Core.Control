# Graceful Degradation

Skills run in multiple environments: CLI terminals, desktop apps, IDE extensions, and web interfaces like claude.ai. Not all environments can execute Python scripts.

The principle: **scripts are the fast, reliable path, but the skill must still deliver its outcome when execution is unavailable.**

When a script cannot run, the LLM performs the equivalent work directly. This is slower and less deterministic, but the user still gets a result. The script's `--help` output documents what it checks, making the fallback natural. The LLM reads the help to understand the script's purpose and replicates the logic.

Frame script steps as outcomes in the SKILL.md, not just commands:

| Approach    | Example                                                                      |
| ----------- | ---------------------------------------------------------------------------- |
| **Good**    | "Validate path conventions (run `scripts/scan-paths.py --help` for details)" |
| **Fragile** | "Execute `python3 scripts/scan-paths.py`" with no context                    |

The good version tells the LLM both what to accomplish and where to find the details, enabling graceful degradation without additional instructions.
