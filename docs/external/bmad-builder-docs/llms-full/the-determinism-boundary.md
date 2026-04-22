# The Determinism Boundary

The design principle is **intelligence placement**: put each operation where it belongs.

| Scripts Handle                     | LLM Handles                                      |
| ---------------------------------- | ------------------------------------------------ |
| Validate structure, format, schema | Interpret meaning, evaluate quality              |
| Count, parse, extract, transform   | Classify ambiguous input, make judgment calls    |
| Compare, diff, check consistency   | Synthesize insights, generate creative output    |
| Pre-process data into compact form | Analyze pre-processed data with domain reasoning |

**The test:** Given identical input, will this operation always produce identical output? If yes, it belongs in a script. Could you write a unit test with expected output? Definitely a script. Requires interpreting meaning, tone, or context? Keep it as an LLM prompt.

:::tip[The Pre-Processing Pattern]
One of the highest-value script uses is pre-processing. A script extracts compact metrics from large files into a small JSON summary. The LLM then reasons over the summary instead of reading raw files, dramatically reducing token usage while improving analysis quality because the data is clean and structured.
:::
