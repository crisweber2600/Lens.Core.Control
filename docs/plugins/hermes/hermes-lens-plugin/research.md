---
feature: hermes-lens-plugin
doc_type: research
status: draft
goal: "Verify the current Hermes plugin capabilities and identify the safest architecture for a Lens project selector and command runner plugin."
key_decisions:
  - "Treat this as a general Hermes plugin that combines tools, hooks, and plugin-specific CLI commands."
  - "Do not depend on a CLI `/commands` feature that is not documented for general plugins."
  - "Prefer direct local project discovery plus session-scoped repo state over persistent global state."
open_questions:
  - "What prompt metadata schema should Lens prompts expose for intent grouping, arguments, and risk hints?"
  - "How should risky Lens actions be classified when metadata is absent?"
  - "How should CLI and chat share active-project state in a way that survives one session but not all sessions?"
depends_on:
  - brainstorm.md
blocks: []
updated_at: 2026-04-13T22:51:44Z
---

# Technical Research: Hermes Plugin Architecture for Lens Project Commands

## Research Scope

This research focused on the technical feasibility of a Hermes plugin that:

- discovers candidate Lens projects under `~/github/*.lens`
- lets the user select an active project
- turns `.github/prompts` into a usable command surface
- runs Lens actions inside the selected repository
- previews execution and confirms only when the action is risky

## Methodology

- Reviewed current public Hermes documentation for plugin construction, CLI behavior, plugin hooks, general plugin capabilities, and platform command surfaces.
- Cross-checked plugin architecture assumptions against the official plugin guide, architecture overview, plugin user guide, hook reference, CLI reference, and slash-command reference.
- Compared those findings against the brainstorm outcomes captured in `brainstorm.md`.

## Verified Findings

### 1. General Hermes plugins can support the core extension surface needed here

Hermes general plugins can add:

- tools via `ctx.register_tool(...)`
- hooks via `ctx.register_hook(...)`
- CLI commands via `ctx.register_cli_command(...)`
- bundled skills via an installed `skill.md`
- optional message injection in CLI mode via `ctx.inject_message(...)`

This means the Lens plugin can be implemented as a standard general plugin rather than needing a memory-provider or context-engine plugin shape.

### 2. Plugin discovery supports both personal and project-local installation

Hermes discovers plugins from three sources:

- `~/.hermes/plugins/` for user plugins
- `.hermes/plugins/` for project plugins
- Python entry points for pip-distributed plugins

Project-local plugins are disabled by default and require `HERMES_ENABLE_PROJECT_PLUGINS=true`. That matters if the Lens plugin is expected to travel with a repo rather than live in the user's Hermes home.

### 3. The safest documented command surface is plugin CLI commands plus tools

General plugins can register CLI subcommands under `hermes <plugin> <subcommand>`. The current docs clearly describe that surface.

The slash-command docs show that:

- built-in CLI slash commands come from a central command registry
- installed skills become dynamic slash commands
- `/commands` is documented as messaging-only, not CLI-wide

That means a CLI design that assumes a generic `/commands` browser for general plugin actions is not strongly supported by the current public docs. If a command browser is desired in the CLI, the safer options are:

- a plugin-specific CLI subcommand such as `hermes lens commands`
- a bundled skill that exposes a slash command
- a normal tool-driven chat workflow that lists available actions conversationally

### 4. Hooks are viable for shared session context and guardrails

Plugin hooks run in both CLI and gateway sessions. The most relevant ones here are:

- `on_session_start` for initializing session state
- `pre_llm_call` for injecting active-project context into the current turn
- `pre_tool_call` and `post_tool_call` for audit, guardrails, or metrics
- `on_session_end` for cleanup

Hook failures are isolated and do not crash the agent. All callbacks should accept `**kwargs` for forward compatibility.

This makes a session-scoped active-project model technically aligned with Hermes.

### 5. Tool and handler contracts are strict enough to shape plugin design

The build guide is explicit that plugin handlers should:

- accept `args: dict` and `**kwargs`
- return a JSON string on success and failure
- catch exceptions and return error JSON instead of raising

This encourages a design where the Lens plugin returns structured previews, catalog entries, and execution results instead of ad hoc text blobs.

### 6. Hermes architecture reinforces a visible, interruptible execution model

The architecture docs emphasize:

- observable execution
- interruptibility
- loose coupling through registries and optional subsystems
- one platform-agnostic core agent across CLI and gateway

These principles match the brainstorm's preference for a visible preview contract and a hybrid CLI-plus-chat experience.

## Architecture Implications for the Lens Plugin

### Recommended baseline architecture

The most technically coherent design is:

1. A **general Hermes plugin** installed under `~/.hermes/plugins/` during early development.
2. A **plugin CLI surface** for deterministic actions such as:
   - `hermes lens select`
   - `hermes lens commands`
   - `hermes lens current`
3. A **tool surface** for catalog retrieval, action preview, input clarification, and execution.
4. An optional **bundled skill** if the final UX should expose a dedicated slash command in the CLI.
5. A **shared session context object** that stores the active Lens project for the current Hermes session only.

### Recommended project discovery model

Use a direct scan of `~/github/*.lens` as the default discovery path. This matches the user's intent and minimizes configuration.

Potential refinement:

- support configurable roots later, but keep `~/github/*.lens` as the opinionated default
- verify that each candidate repo contains the expected Lens markers before surfacing it as selectable

### Recommended prompt-catalog model

Build a normalized action catalog from `.github/prompts` using a hybrid approach:

- infer action name and group from prompt filename/path when no metadata exists
- use metadata when present for:
  - display label
  - intent group
  - parameter definitions
  - risk hints
  - recommended execution mode

When confidence is low, the plugin should still expose the action, but make the lower confidence visible and fall back to interactive clarification.

### Recommended safety model

The preview contract should always show at least:

- selected repo path
- friendly action name
- collected or inferred inputs
- underlying execution plan or Lens command
- risk level and why

Confirmation should be required only when the action is risky, with risk inferred primarily from the underlying command effect rather than from filenames alone.

## Key Technical Risks

### CLI and chat state drift

This is the largest usability risk. If the CLI thinks one project is active while tool calls resolve another, the plugin becomes untrustworthy.

Mitigation:

- define a single session-context source of truth
- make both CLI helpers and tool handlers resolve through the same state object
- surface the active repo visibly in preview and status outputs

### Prompt ambiguity

Prompt files may not encode enough structure for reliable action inference.

Mitigation:

- define a minimal optional metadata schema
- support low-confidence fallback behavior rather than hiding the action entirely
- keep inference simple and transparent

### Underdocumented slash-command assumptions

The docs do not show general plugins registering arbitrary slash commands directly in the CLI command registry. Skills do become slash commands, and plugin CLI commands are documented, but `/commands` is messaging-only.

Mitigation:

- do not make the initial product dependent on undocumented CLI slash-command behavior
- use documented plugin CLI commands and tool-driven chat UX first

## Recommendations

### Immediate recommendations

1. Implement the plugin as a **general plugin** with tools, hooks, and `hermes lens ...` CLI commands.
2. Treat the **prompt catalog** as the first implementation milestone because it unlocks both listing and execution UX.
3. Define a **single source session-context model** before any multi-surface UX work.
4. Formalize a **preview contract** before enabling risky action execution.

### Defer until later

1. Cross-session persistence of selected project state.
2. Dependence on undocumented CLI slash-command extensions for general plugins.
3. Complex caching or indexing beyond direct local discovery.

## Research Conclusion

The Hermes platform is technically capable of supporting the intended Lens plugin. The documented primitives are sufficient for a robust v1, provided the design avoids relying on undocumented CLI slash-command behavior and instead uses the supported combination of:

- plugin CLI commands
- plugin tools
- session hooks
- optional bundled skills for richer command UX

The architecture that best fits the current docs is a session-scoped Lens project selector with a normalized prompt catalog and a mandatory preview contract.

## Sources

- [Build a Hermes Plugin](https://hermes-agent.nousresearch.com/docs/guides/build-a-hermes-plugin)
- [Plugins](https://hermes-agent.nousresearch.com/docs/user-guide/features/plugins)
- [Event Hooks](https://hermes-agent.nousresearch.com/docs/user-guide/features/hooks)
- [CLI Commands Reference](https://hermes-agent.nousresearch.com/docs/reference/cli-commands)
- [Slash Commands Reference](https://hermes-agent.nousresearch.com/docs/reference/slash-commands)
- [CLI Interface](https://hermes-agent.nousresearch.com/docs/user-guide/cli)
- [Architecture](https://hermes-agent.nousresearch.com/docs/developer-guide/architecture)
