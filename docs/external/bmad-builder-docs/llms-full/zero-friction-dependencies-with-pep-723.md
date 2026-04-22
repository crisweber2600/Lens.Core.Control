# Zero-Friction Dependencies with PEP 723

Python scripts in skills use [PEP 723](https://peps.python.org/pep-0723/) inline metadata to declare their dependencies directly in the file. Combined with `uv run`, this gives you `npx`-like behavior: dependencies are silently cached in an isolated environment, no global installs, no user prompts.

```python
#!/usr/bin/env -S uv run --script
# /// script
# requires-python = ">=3.10"
# dependencies = ["pyyaml>=6.0"]
# ///

import yaml
# script logic here
```

When a skill invokes this script with `uv run scripts/analyze.py`, the dependency (`pyyaml` in this example) is automatically resolved. The user never sees an install prompt, never needs to manage a virtual environment, and never pollutes their global Python installation.

Without PEP 723, skills that need libraries like `pyyaml` or `tiktoken` would force users to run `pip install`, a jarring experience that makes people hesitate to adopt the skill.
