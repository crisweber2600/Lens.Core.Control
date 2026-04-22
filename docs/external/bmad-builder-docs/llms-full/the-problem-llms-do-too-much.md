# The Problem: LLMs Do Too Much

Without scripts, every operation in a skill runs through the LLM. That means:

- **Non-deterministic results.** Ask an LLM to count tokens in a file three times and you may get three different numbers. Ask a script and you get the same answer every time.
- **Wasted tokens and time.** Parsing a JSON file, checking if a directory exists, or comparing two strings are mechanical operations. Running them through the LLM burns context window and adds latency for no gain.
- **Harder to test.** You can write unit tests for a script. You cannot write unit tests for an LLM prompt.

The pattern shows up everywhere: skills that try to LLM their way through structural validation are slower, less reliable, and more expensive than skills that offload those checks to scripts.
