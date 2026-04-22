# Quick Start

## 1. Register the Module

On first use, run `bmad-bmb-setup` to register BMad Builder in your project. This collects your preferences (name, language, output paths) and registers the builder's capabilities with the help system so `bmad-help` can guide you.

:::tip[Single-Skill Modules]
If you install a module that contains only one skill, that skill handles its own registration on first run. No separate setup step needed.
:::

## 2. Build Something

Invoke the **Agent Builder** or **Workflow Builder** and describe what you want to create. Both walk you through a series of questions and produce a ready-to-use skill folder.

| Goal                      | Builder          | Menu Code |
| ------------------------- | ---------------- | --------- |
| AI companion with memory  | Agent Builder    | BA        |
| Structured process / tool | Workflow Builder | BW        |
| Package skills as module  | Module Builder   | CM        |

## 3. Use Your Skill

The builders produce a complete skill folder. Copy it into your AI tool's skills directory (`.claude/skills/`, `.codex/skills/`, `.agents/skills/`, or wherever your tool looks) and it's immediately usable.

:::tip[Custom Module Installation]
The BMad Method installer supports installing custom modules from any Git host (GitHub, GitLab, Bitbucket, self-hosted) or local paths. See the [BMad Method install guide](https://docs.bmad-method.org/how-to/install-custom-modules/) for details.
:::

:::tip[No Module Required]
If you're building something for personal use, you don't need to package it as a module. Copy the skill folder and use it directly. Module packaging (with `bmad-help` registration and configuration) is for sharing or richer discoverability.
:::

## 4. Learn More

See the [Builder Commands Reference](/reference/builder-commands.md) for all capabilities, modes, and phases.
