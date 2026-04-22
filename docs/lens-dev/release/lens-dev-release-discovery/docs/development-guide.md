# lens.core.release — Development Guide

**Feature:** lens-dev-release-discovery
**Generated:** 2026-05-20

---

> **Note:** This guide covers working with the release artifact as a consumer. For authoring changes to the module itself, see the source repo (`lens.core.src`) and its development guide. The release artifact is a promoted distribution — it is not where skills, prompts, or scripts are edited.

---

## Prerequisites

| Tool | Purpose | Required |
| --- | --- | --- |
| `git` | Version control | Yes |
| `uv` | Python script execution (fast, dependency-isolated) | Yes |
| `python 3.11+` | Runtime for scripts | Yes (via uv) |
| GitHub account | For PR operations and governance repo | Yes |
| GitHub PAT | For `create-pr.py` and `store-github-pat.py` | Yes for PR features |

`uv` resolves Python dependencies inline from script headers — no `pip install` or virtual environment setup needed.

---

## Setting Up a New Control Repo

The standard consumer workflow is:

### Step 1: Create and clone your control repo

```bash
# Create a new repo on GitHub (e.g., myproject.src), then:
git clone https://github.com/your-username/myproject.src.git
cd myproject.src
```

### Step 2: Clone the release module

```bash
# macOS / Linux / Git Bash
git clone --branch beta https://github.com/crisweber2600/lens.core.git

# The module will be at ./lens.core/
```

### Step 3: Run the setup script

Interactive wizard (no arguments = guided setup):
```bash
uv run lens.core/_bmad/lens-work/scripts/setup-control-repo.py
```

The setup script:
- Reads the existing `lens.core/` clone
- Prompts for your GitHub username and target repo info
- Generates `bmadconfig.yaml` with your configuration
- Sets up governance repo (auto-creates via `gh` if missing)
- Creates `.lens/` personal state directory
- Copies IDE adapter stubs to `.github/`, `.claude/`, `.codex/`, `.cursor/`

### Step 4: Store your GitHub PAT

**Run this outside the AI chat** (it writes to environment):
```bash
uv run lens.core/_bmad/lens-work/scripts/store-github-pat.py
```

PAT resolution order: `GITHUB_PAT` env var → `GH_TOKEN` env var → `profile.yaml` → URL-only fallback

---

## Working With the Release Directly

If you've cloned `lens.core.release` itself (as in this workspace), key operations:

### Dry-run install check
```bash
uv run _bmad/lens-work/scripts/install.py --dry-run
```

### Validate the skill/prompt registry
```bash
uv run _bmad/lens-work/scripts/validate-lens-bmad-registry.py
```

### Run all script tests
```bash
uv run --with pytest pytest _bmad/lens-work/scripts/tests -q
```

### Run a specific test
```bash
uv run --with pytest pytest _bmad/lens-work/scripts/tests/test-install.py -q
uv run --with pytest --with pyyaml pytest _bmad/lens-work/scripts/tests/test-setup-control-repo.py -q
```

### Full preflight check
```bash
uv run _bmad/lens-work/scripts/preflight.py
```

---

## Using Lens in an AI Chat Session

Once installed in a control repo, users invoke commands via their AI tool:

### GitHub Copilot (VS Code)
Use prompt files from `.github/prompts/`:
```
/lens-next
/lens-init-feature
/lens-businessplan
```
Or use the agent: `@lens-work-lens /help`

### Claude
Reference the module in your project settings or load the agent directly:
```
Read and follow: lens.core/_bmad/lens-work/agents/lens.agent.md
```

### Codex
`AGENTS.md` at the repo root activates automatically on Codex open.

### Cursor
Cursor rules reference the Lens agent and lifecycle contract.

---

## Key Development Patterns

### Preflight Pattern
Every prompt runs `light-preflight.py` first. It checks:
- Is there a valid `lens.core/` module present?
- Is there an active feature context?
- Is the module version compatible?

If preflight fails, it surfaces a clear error before loading the skill.

### Feature Context
Active feature context is stored in `.lens/active-feature.yaml` (git-ignored personal state). Commands use this to know which feature branch, governance path, and phase to operate on.

### Two-Branch Topology
Every feature has two branches:
- `{featureId}-plan` — Planning artifacts (businessplan, techplan, etc.)
- `{featureId}` — Working branch in target repos

Branch state + PR status = all lifecycle state. No external state stores.

### Script-Isolated Execution
Scripts use `uv run` with inline dependencies:
```python
# /// script
# dependencies = ["pyyaml"]
# ///
```
This means no global environment setup — each script runs in its own isolated environment.

---

## Troubleshooting

### "Module not found" in preflight
The `light-preflight.py` looks for `lens.core/` relative to project root. Ensure `lens.core/` is cloned at the expected path.

### Test failures for setup-control-repo
Run from `TargetProjects/lens.core/src/Lens.Core.Src` in the control repo. The source preflight resolves from the nearest ancestor containing `lens.core/`. See repo memory notes on source preflight invocation.

### PAT not found
Ensure `GITHUB_PAT` or `GH_TOKEN` environment variable is set, or run `store-github-pat.py` to configure it.

### "Feature context not found"
Run `/lens-switch` to select or create a feature context, or `/lens-init-feature` for a new feature.

---

_Generated by lens-bmad-document-project for feature lens-dev-release-discovery._
