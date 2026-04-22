# Why Python, Not Bash

Skills must work across macOS, Linux, and Windows. Bash is not portable.

| Factor               | Bash                                          | Python                   |
| -------------------- | --------------------------------------------- | ------------------------ |
| **macOS / Linux**    | Works                                         | Works                    |
| **Windows (native)** | Fails or behaves inconsistently               | Works identically        |
| **Windows (WSL)**    | Works, but can conflict with Git Bash on PATH | Works identically        |
| **Error handling**   | Limited, fragile                              | Rich exception handling  |
| **Testing**          | Difficult                                     | Standard unittest/pytest |
| **Complex logic**    | Quickly becomes unreadable                    | Clean, maintainable      |

Even basic commands like `sed -i` behave differently on macOS vs Linux. Piping, `jq`, `grep`, `awk`. All of these have cross-platform pitfalls that Python's standard library avoids entirely.

**Safe bash commands** that work everywhere and remain fine to use directly:

| Command              | Purpose                        |
| -------------------- | ------------------------------ |
| `git`, `gh`          | Version control and GitHub CLI |
| `uv run`             | Python script execution        |
| `npm`, `npx`, `pnpm` | Node.js ecosystem              |
| `mkdir -p`           | Directory creation             |

Everything beyond that list should be a Python script.
