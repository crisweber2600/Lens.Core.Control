# Standard Library First

Python's standard library covers most script needs without any external dependencies. Stdlib-only scripts run with plain `python3`, need no special tooling, and have zero supply-chain risk.

| Need               | Standard Library   |
| ------------------ | ------------------ |
| JSON parsing       | `json`             |
| Path handling      | `pathlib`          |
| Pattern matching   | `re`               |
| CLI interface      | `argparse`         |
| Text comparison    | `difflib`          |
| Counting, grouping | `collections`      |
| Source analysis    | `ast`              |
| Data formats       | `csv`, `xml.etree` |

Only reach for external dependencies when the stdlib genuinely cannot do the job: `tiktoken` for accurate token counting, `pyyaml` for YAML parsing, `jsonschema` for schema validation. Each external dependency adds install-time cost, requires `uv` to be available, and expands the supply-chain surface. The BMad builders require explicit user approval for any external dependency during the build process.
