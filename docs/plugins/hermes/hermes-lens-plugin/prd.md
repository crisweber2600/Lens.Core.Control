---
feature: hermes-lens-plugin
doc_type: prd
status: complete
goal: "Define the product requirements for the Hermes Lens Plugin business plan."
key_decisions: []
open_questions: []
depends_on:
  - product-brief.md
  - research.md
  - brainstorm.md
  - preplan-adversarial-review.md
blocks: []
updated_at: 2026-04-14T03:00:00Z
stepsCompleted:
  - step-01-init
  - step-02-discovery
  - step-02b-vision
  - step-02c-executive-summary
  - step-03-success
  - step-04-journeys
  - step-05-domain
  - step-06-innovation
  - step-07-project-type
  - step-08-scoping
  - step-09-functional
  - step-10-nonfunctional
  - step-11-polish
  - step-12-complete
inputDocuments:
  - docs/plugins/hermes/hermes-lens-plugin/product-brief.md
  - docs/plugins/hermes/hermes-lens-plugin/research.md
  - docs/plugins/hermes/hermes-lens-plugin/brainstorm.md
  - docs/plugins/hermes/hermes-lens-plugin/preplan-adversarial-review.md
workflowType: prd
documentCounts:
  briefCount: 1
  researchCount: 1
  brainstormingCount: 1
  projectDocsCount: 0
classification:
  projectType: cli_tool
  projectTypeNotes: developer_tool signals also apply (SDK-style session-context contract, adapter interfaces)
  domain: developer_tool
  domainLabel: developer tooling / workflow automation
  complexity: medium
  projectContext: greenfield
---

# Product Requirements Document - Hermes Lens Plugin

**Author:** CrisWeber
**Date:** 2026-04-13T23:31:44Z

---

## Executive Summary

Lens users who operate across multiple local repositories face a recurring coordination problem: Lens workflows live in repo-specific prompt inventories, but the path from "I want to run a Lens command" to "I am operating safely in the right project" is still fully manual. They must remember which repo is active, browse prompt directories, reconstruct execution context, and assess risk without any tooling support.

The Hermes Lens Plugin resolves this by turning Lens repositories into a session-aware, navigable command surface inside Hermes. A user selects their active project from local Lens repositories discovered under `~/github/*.lens`, sees the available Lens actions as a normalized action catalog rather than raw prompt files, and runs those actions through a transparent preview-first execution model that requires confirmation only for risky operations.

This is a greenfield `cli_tool` and `developer_tool` product in the developer tooling / workflow automation domain, with medium complexity driven by three design contracts that must be resolved in this phase: the single session-context contract governing active-project state across CLI and tool surfaces, the optional prompt metadata schema that powers the action catalog, and the risk taxonomy that drives the preview and confirmation model.

**Vision:** The plugin does not invent new Lens capabilities. It makes existing prompt inventories legible and safe to execute. Session-scoped repo context plus preview-first execution creates trust. That trust is the product.

**Key differentiators:**
- Session-scoped repo context makes it unambiguous which project is active at all times
- The action catalog abstracts `.github/prompts` into understandable, grouped actions
- The hybrid CLI-plus-chat control plane gives users both deterministic setup and guided execution
- Preview-first execution shows the full execution plan and risk level before anything runs
- The graceful metadata model delivers UX improvement when prompt metadata is present without blocking adoption when it is absent

---

## Product Vision

Hermes Lens Plugin turns Lens repositories from a collection of prompt files into a safe, navigable command surface. A developer selects their active Lens project once per session, sees its prompt inventory as a friendly action catalog grouped by intent, and runs Lens workflows with a transparent preview payload and a targeted confirmation gate — without needing to remember which repo is active, browse directories manually, or independently assess execution risk.

The product's core insight is that Lens repos are already full of workflow value that is invisible and inaccessible from the outside. The plugin does not add capabilities; it makes existing prompt inventories legible and safe to execute. The infrastructure was built ahead of its own UX layer. This plugin is that UX layer.

The delight moment is not "look at all these commands." It is: "I know exactly what will run, where it will run, and I am one confirmation away." That shift — from opaque prompt execution to transparent, session-scoped command dispatch — is the product's full value proposition.

---

## Success Criteria

### User Success

- A user can discover all Lens repos under `~/github/*.lens` and select an active project in a single interaction with no prior configuration
- The action catalog derived from `.github/prompts` is comprehensive enough to surface all available Lens actions in a named, grouped form — including actions with no metadata
- The user understands what an action will do before it runs: the preview payload is legible and complete
- Risky actions are visibly distinguished from safe actions, and the confirmation gate fires predictably
- A user who switches projects mid-session can trust that all subsequent actions resolve against the new active project, with no residual state from the previous selection
- The plugin eliminates the need to browse prompt folders manually for teams using standard Lens repo layouts

### Business Success

- At least one real Lens repo's full prompt inventory is catalogable by the plugin on first release
- The plugin ships as a standard Hermes general plugin installed under `~/.hermes/plugins/` without requiring project-local configuration
- The session-context contract is stable enough to serve as the foundation for future multi-session or project-context persistence features
- The metadata schema is documented and adoptable by Lens repo maintainers without tooling changes on their side

### Technical Success

- Active-project state is held in a single shared session-context object; both `hermes lens` CLI commands and plugin tools resolve against the same state — no drift between surfaces
- Catalog build time for a Lens repo with up to 50 prompt files is under two seconds
- Plugin load and session-init do not block or crash Hermes when no active project is set
- All tool handler returns are structured JSON; no tool raises unhandled exceptions to the agent

### Measurable Signals (v1 proxy metrics)

- Time from `hermes` session start to first successful Lens action execution (target: under three minutes for a first-time user)
- Percentage of `.github/prompts` files converted to catalog entries without manual fallback (target: ≥90% for repos following standard Lens naming)
- Zero wrong-repo execution incidents attributable to session-state drift in structured testing

---

## User Journeys

### Journey 1 — Alex: The Multi-Project Developer

**Situation:** Alex maintains three active Lens repos: a platform services repo, a data pipeline repo, and a documentation generator. Each has its own Lens workflows. Every time Alex wants to run a Lens command, they have to remember which project they intended to target and manually navigate to the right directory before the action means anything.

**Goal:** Run the right Lens workflow in the right repo without switching mental contexts or second-guessing which project is active.

**Obstacle:** Today there is no Hermes-native way to select a project. Alex either has to open a terminal and manually navigate to the repo directory, or they run a Lens prompt and quietly hope the cwd is correct.

**Journey with the plugin:**
1. Alex opens a Hermes session and types `hermes lens select`. The plugin lists the three local Lens projects discovered under `~/github/*.lens`.
2. Alex picks the data pipeline repo. The plugin confirms the active project and stores it in session context.
3. Alex asks "what Lens commands are available here?" The plugin returns the action catalog for that repo — six actions, grouped by intent: two for pipeline validation, two for documentation, two for release prep.
4. Alex picks the release-prep action. The plugin presents the preview payload: repo path, action name, inferred inputs, execution plan, risk level (read-only). Alex confirms.
5. The action runs in the correct repo directory. Alex did not have to think about paths, prompts, or execution context.

**Next chapter:** Alex uses the plugin as the standard entry point for Lens work across all three repos.

---

### Journey 2 — Sam: The Lens Workflow Maintainer

**Situation:** Sam authors and maintains Lens workflows for their team. They know the workflows work when run manually, but they have no way to verify that someone else using Hermes can discover and execute those workflows correctly without a walkthrough.

**Goal:** Confirm that the Lens plugin can surface their `.github/prompts` catalog correctly and that the preview represents the actual execution plan.

**Obstacle:** Today Sam can only tell teammates "look at the prompts folder." There is no shared, queryable action surface.

**Journey with the plugin:**
1. Sam selects their team repo via `hermes lens select`. The plugin builds the catalog.
2. Sam runs `hermes lens commands` and compares the output to the raw prompts directory. The plugin has cataloged all standard prompts and correctly inferred intent groups from filename conventions.
3. Sam notices one prompt is categorized with lower confidence because it has an unusual filename. The plugin surfaces the action but marks it with a lower-confidence indicator.
4. Sam adds minimal metadata (a `display_label` and an `intent_group` field) to that prompt file. On next catalog build, the action appears with full confidence.

**Next chapter:** Sam adopts the minimal metadata schema across all team repos to improve catalog quality.

---

### Journey 3 — First-Time Lens User

**Situation:** A developer has just been added to a team that uses Lens workflows. They know the repo exists but have no idea what Lens commands are available or how to run them safely.

**Goal:** Discover what Lens can do in this repo and run at least one action without needing help from a teammate.

**Obstacle:** Without context, browsing `.github/prompts` is confusing. Filenames are not always descriptive. The user does not know which actions are safe to run.

**Journey with the plugin:**
1. The user runs `hermes lens select`, picks the team repo from the discovered list.
2. The user asks "what can I do here?" The plugin returns the catalog grouped by intent — they can immediately see the broad categories of available workflows.
3. The user picks a documentation action. The preview shows it is read-only, no confirmation required. It runs and succeeds.
4. The user picks a deployment-prep action. The preview marks it as risky (file mutation). The user reads the execution plan and confirms explicitly before running.

**Next chapter:** The user understands the Lens command surface for this repo without any onboarding session.

---

## Domain Constraints

**Domain:** Developer tooling / workflow automation. **Complexity:** Medium.

The medium complexity rating is driven by three cross-cutting design contracts rather than domain-specific compliance requirements. There are no regulatory or external compliance dependencies for v1.

### Session-Context Contract (M1)

The plugin must define a single source of truth for active-project state. This contract governs:

- **State owner:** the plugin session-context object, initialized on `on_session_start` hook, held in memory for the current Hermes session only
- **Lifecycle:** initialized as `null` (no project selected); set explicitly by the user via `hermes lens select` or the `lens_select_project` tool; cleared on `on_session_end` or `/new`
- **CLI↔tool handoff:** both `hermes lens ...` CLI command handlers and all plugin tool handlers resolve active-project state through the same session-context object; there is no separate CLI-side state store
- **State visibility:** the active project must be surfaced in every preview payload and in `hermes lens current`
- **Per-turn context injection:** the `pre_llm_call` hook surfaces the active project into every LLM turn by returning `{"context": "Active Lens project: <name> (<path>)"}` — the Hermes agent loop prepends this to the user message; returns `None` when no project is set to avoid polluting turns with null state
- **Session boundaries:** session context does not persist across Hermes sessions by default in v1; `on_session_end` fires at the end of every `run_conversation` call and on CLI exit, which also covers `/new` and `/reset`; cross-session persistence is a v2 capability

### Prompt Metadata Schema (M2)

The plugin must support a minimal optional metadata schema for prompt files. Fields and behavior:

| Field | Type | Required | Fallback if absent |
|-------|------|----------|-------------------|
| `display_label` | string | No | Inferred from filename (title-case, hyphens → spaces) |
| `intent_group` | string | No | Inferred from directory name or filename prefix |
| `parameters` | list of `{name, description, required}` | No | Inferred from prompt body patterns; interactive clarification used |
| `risk_hints` | list of risk class names | No | Risk inferred from command effect heuristics |
| `execution_mode` | `read` or `mutating` | No | Inferred from risk classification |

Metadata is embedded in prompt files using a YAML frontmatter block delimited by `---`. The plugin reads metadata when present and falls back to inference when absent. Catalog entries derived purely from inference are marked with a lower-confidence indicator visible to the user.

### Risk Taxonomy (M3)

The plugin must classify every action into one of three risk classes before the preview is shown:

| Risk Class | Definition | Confirmation Required |
|------------|-----------|----------------------|
| `read` | Action reads, analyzes, or reports; no file writes, no git mutations, no external calls | No |
| `mutating` | Action writes or modifies files, commits, or performs other state-changing git operations within the selected repo | Yes |
| `elevated` | Action pushes, opens PRs, deletes branches, or performs operations with external or cross-repo effects | Yes — with explicit confirmation prompt naming the effect |

**Risk determination precedence:**
1. `risk_hints` from prompt metadata (highest authority)
2. `execution_mode` from prompt metadata
3. Heuristic analysis of prompt body: presence of `git push`, `git commit`, `rm`, `git branch -d`, file-write patterns
4. Default to `mutating` when classification is uncertain

**Preview payload schema:** Every action preview must include:
- `active_repo`: absolute path to selected Lens repo
- `action_name`: display label from catalog
- `action_source`: relative path of the prompt file
- `collected_inputs`: key-value map of resolved parameter values
- `inferred_inputs`: key-value map of values the plugin inferred without explicit user input
- `execution_plan`: human-readable description of what will run (e.g., "Run lens-businessplan prompt in CrisWeber/my-lens-repo with feature=hermes-lens-plugin")
- `risk_class`: one of `read`, `mutating`, or `elevated`
- `risk_basis`: brief explanation of why this risk class was assigned

---

## Innovation Patterns

The Hermes Lens Plugin applies two genuine innovations in the Hermes plugin space:

**1. Hybrid CLI-plus-chat control plane for Hermes general plugins**

Most Hermes plugins are either chat-native (tool-only) or CLI-native (subcommand-only). This plugin deliberately uses both surfaces in a coordinated way: deterministic project selection and catalog inspection via `hermes lens` CLI commands, exploratory discovery and guided execution via chat and tools, with a single shared session-context object ensuring the two surfaces never diverge. This is a new pattern for Hermes general plugins.

This plugin also deliberately complements Hermes's existing static Context Files system (`AGENTS.md` and project context files that shape every conversation) with *dynamic* per-turn active-project injection via `pre_llm_call`. Static context files describe a project's conventions globally; the plugin's `pre_llm_call` injection tells the agent which Lens project is active *right now*, in this session, on this turn. The two mechanisms are additive: a repo can carry an `AGENTS.md` describing its Lens conventions while the plugin supplies the live project-selection signal each turn.

**2. The prompt-catalog-as-action-index abstraction**

Treating `.github/prompts` files as a queryable, grouped action catalog with confidence scoring and graceful inference is a novel abstraction layer that does not exist in the current Hermes ecosystem. It converts a filesystem convention into a typed, navigable structure. The metadata schema creates a lightweight but extensible convention that Lens repo maintainers can adopt incrementally.

---

## Project-Type Requirements (CLI Tool + Developer Tool)

### CLI Command Surface

The plugin registers the following documented CLI subcommands via `ctx.register_cli_command(name, help, setup_fn, handler_fn)`. Each registration adds a `hermes lens <subcommand>` entry to the Hermes CLI:

| Command | Description |
|---------|-------------|
| `hermes lens select` | Present discovered Lens projects; prompt user to choose one; set active project in session context |
| `hermes lens current` | Display the currently active Lens project path and name |
| `hermes lens commands` | List available actions in the active Lens project's catalog |
| `hermes lens run <action>` | Run a named action with preview-first flow; prompt for missing inputs |
| `hermes lens refresh` | Invalidate and rebuild catalog for the active Lens project |

All CLI commands must be usable without a prior Hermes session. Commands that require an active project must print a clear error and the `hermes lens select` hint if no project is set.

### Tool Surface

The plugin registers the following tools via `ctx.register_tool(...)` for chat-native invocation:

| Tool | Description |
|------|-------------|
| `lens_list_projects` | Return JSON list of discovered Lens repos under `~/github/*.lens` |
| `lens_select_project` | Set active project; return confirmation JSON |
| `lens_get_current_project` | Return current active project metadata |
| `lens_list_actions` | Return JSON action catalog for active project |
| `lens_preview_action` | Return full preview payload JSON for a named action with resolved inputs |
| `lens_run_action` | Execute a previewed action after confirmation; return result JSON |
| `lens_clarify_input` | Interactively collect a missing parameter value; return resolved value JSON |

All tool handlers accept `args: dict` and `**kwargs`. All tools return a JSON string. Exceptions must be caught and returned as error JSON — no tool raises to the agent.

### Plugin Hooks

| Hook | Purpose |
|------|---------|
| `on_session_start` | Initialize `LensSessionContext` with `active_project: null`; fires on new session creation (first turn only) |
| `pre_llm_call` | Return `{"context": "Active Lens project: <name> (<path>)"}` when a project is selected, causing Hermes to prepend active-project state to the user message; return `None` when no project is set |
| `pre_tool_call` | Log tool invocation for audit; enforce that `lens_run_action` requires a prior `lens_preview_action` call in the same turn |
| `post_tool_call` | Record audit log entry after `lens_run_action` completes; emit result summary for session-level audit trail |
| `on_session_end` | Clear session-context; fires at end of every `run_conversation` call and on CLI exit (covers `/new` and `/reset`); no persistence by default |

### Installation

- v1 targets user-level installation under `~/.hermes/plugins/hermes-lens/`
- Project-local installation (`.hermes/plugins/`) is out of scope for v1
- Plugin must include a `plugin.yaml` manifest with `name`, `version`, and `description`; optional `requires_env` field lists environment variable names the user is prompted to configure during `hermes plugins install`
- Tools, hooks, and CLI commands are registered in the `register(ctx)` function in `__init__.py`, not in the manifest; the manifest is metadata only
- Plugin is installable and manageable via `hermes plugins install / update / remove / enable / disable`

### Inter-Surface State Contract

- All tools and CLI command handlers import and mutate a single `LensSessionContext` object that is initialized per Hermes session
- `LensSessionContext` stores: `active_project_path`, `active_project_name`, `catalog_cache` (nullable), `catalog_built_at` (nullable)
- The CLI and tool surfaces share one import path to this context; no local copies, no separate state sinks

---

## Scope

### v1 — In Scope

- Lens project discovery under `~/github/*.lens`
- Explicit session-scoped project selection with single shared session-context model
- Normalized action catalog built from `.github/prompts` with confidence scoring
- Intent-based grouping using directory name and filename prefix inference
- Minimal optional prompt metadata schema (display_label, intent_group, parameters, risk_hints, execution_mode)
- Catalog entries for actions with no metadata using inference-only fallback with lower-confidence indicator
- Three-tier risk taxonomy: `read`, `mutating`, `elevated`
- Full preview payload for every action before execution
- Confirmation gate for `mutating` and `elevated` actions only; `read` actions execute without confirmation
- Interactive input clarification for missing or ambiguous parameters
- `hermes lens select / current / commands / run / refresh` CLI commands
- All five tools as documented in Project-Type Requirements
- Plugin hooks: `on_session_start`, `pre_llm_call`, `pre_tool_call`, `post_tool_call`, `on_session_end`
- User-level plugin installation under `~/.hermes/plugins/`
- Catalog refresh via explicit `hermes lens refresh` command (no background indexing)
- Action-confidence indicator surfaced in catalog listings and in preview payloads
- Standard `plugin.yaml` manifest with `name`, `version`, `description`, and optional `requires_env` for environment variable declaration

### v2 and Beyond — Out of Scope for v1

- Cross-session persistence of active project selection
- Project-local plugin installation (`.hermes/plugins/`)
- Configurable discovery roots beyond `~/github/*.lens`
- Background catalog indexing or file-watcher-based auto-refresh
- Automatic prompt metadata generation or rewriting
- Bundled Hermes skill for slash-command exposure
- Cross-repo action execution
- Prompt convention linting or recommendations
- pip/PyPI distribution of the plugin via the `hermes_agent.plugins` entry_points mechanism
- Project-local plugin installation under `.hermes/plugins/` (requires `HERMES_ENABLE_PROJECT_PLUGINS=true`)
- Programmatic / batch Lens action dispatch via `execute_code` (`execute_code` is a Hermes agent-level code execution tool intercepted before `handle_function_call()` — it bypasses the plugin hook chain entirely, making safe Lens action orchestration through it impossible in v1)

---

## Functional Requirements

### FR-A: Project Discovery

- FR1: The plugin can discover all directories matching `~/github/*.lens` on the local filesystem
- FR2: The plugin validates that each discovered directory is a Lens project by checking for the presence of `.github/prompts`
- FR3: The plugin returns a list of discovered projects including their display name (directory basename) and absolute path
- FR4: The plugin returns an empty list without error when no Lens repos are found

### FR-B: Project Selection and Session Context

- FR5: A user can select an active Lens project from the discovered list in a single interactions via `hermes lens select` or the `lens_select_project` tool
- FR6: The active project is stored in a single `LensSessionContext` object that is shared by all CLI command handlers and tool handlers for the duration of the session
- FR7: Any CLI command or tool that requires an active project returns a clear error and instructs the user to run `hermes lens select` when no project is set
- FR8: A user can view the currently active project path and name at any time via `hermes lens current` or `lens_get_current_project`
- FR9: A user can change the active project at any time by re-running `hermes lens select`; all subsequent actions resolve against the newly selected project immediately
- FR10: Session context is cleared at `on_session_end`; the active project does not persist across sessions by default in v1

### FR-C: Action Catalog

- FR11: The plugin builds a normalized action catalog from `.md` files in `.github/prompts` of the active Lens project; files matching `README*`, `_*`, or containing no prompt body content are excluded; non-`.md` files are silently skipped; subdirectory structure within `.github/prompts/` is used for intent group inference
- FR12: Each catalog entry includes: display name, source file path, intent group, confidence level (`high` or `inferred`), and resolved risk class
- FR13: Display name is derived from `display_label` metadata when present, otherwise from the prompt filename (title-case, hyphens and underscores replaced with spaces)
- FR14: Intent group is derived from `intent_group` metadata when present, otherwise from the prompt's directory name within `.github/prompts` or from filename prefix conventions
- FR15: Catalog entries derived entirely from inference are marked with a `confidence: inferred` indicator visible in listings and previews
- FR16: All entries appear in the catalog regardless of metadata presence; no prompt file is silently excluded
- FR17: A user can rebuild the action catalog on demand via `hermes lens refresh` or `lens_list_actions` with a `refresh` flag
- FR18: The catalog is cached for the session after first build; subsequent calls return the cached version unless a refresh is requested

### FR-D: Input Collection

- FR19: The plugin infers required parameter values from prompt file body patterns when no explicit parameter definitions are present in metadata
- FR20: When `parameters` metadata is present, the plugin uses those definitions to determine required and optional inputs
- FR21: The plugin interactively requests any missing required input values before presenting the preview payload
- FR22: The plugin shows inferred input values separately from explicitly collected values in the preview payload

### FR-E: Preview and Execution

- FR23: The plugin generates a full preview payload for every action before execution; no action executes without a preview being generated first
- FR24: The preview payload includes: `active_repo`, `action_name`, `action_source`, `collected_inputs`, `inferred_inputs`, `execution_plan`, `risk_class`, `risk_basis`
- FR25: The plugin classifies every action into one of three risk classes: `read`, `mutating`, or `elevated`, using the precedence order: metadata risk_hints → metadata execution_mode → heuristic analysis → default `mutating`
- FR26: Actions classified as `read` execute after preview without a confirmation gate
- FR27: Actions classified as `mutating` require explicit user confirmation before execution
- FR28: Actions classified as `elevated` require explicit user confirmation with a confirmation prompt that names the specific external or cross-repo effect
- FR29: The plugin executes the selected action in the active project's directory; no action runs against a different repo or the cwd from which Hermes was launched
- FR30: The plugin returns a structured result JSON after execution including exit code, output, and active project context

### FR-F: CLI Surface

- FR31: `hermes lens select` discovers all Lens projects, presents them to the user, and sets the selected project as active in session context
- FR32: `hermes lens current` displays the active project path and name, or a clear "no active project" message
- FR33: `hermes lens commands` lists the full action catalog for the active project grouped by intent
- FR34: `hermes lens run <action>` runs a named action through the full preview-then-confirm-then-execute flow; for `mutating` and `elevated` actions in CLI mode, the plugin presents an interactive `y/N` confirmation prompt displaying the risk class and effect before execution; a `--confirm` flag may be passed to pre-approve execution without the interactive prompt
- FR35: `hermes lens refresh` invalidates the catalog cache and rebuilds it from the active project's `.github/prompts`

### FR-G: Session Lifecycle

- FR36: The `on_session_start` hook initializes a `LensSessionContext` with `active_project: null` and `catalog_cache: null`; when Hermes resumes an existing session via `--resume`, `on_session_start` does not fire — the restored session carries no active project (v1 does not persist session context), so the user must re-select a project after resume
- FR37: The `pre_llm_call` hook returns `{"context": "Active Lens project: <name> (<path>)"}` when a project is selected, which Hermes prepends to the user message for that turn; returns `None` with no injection when no project is set; the hook fires once per top-level `run_conversation` call (at turn start, before the tool-call loop) — context injection targets the initial user message, not intermediate tool-result messages within the same turn
- FR38: The `pre_tool_call` hook enforces that `lens_run_action` cannot be called unless `lens_preview_action` was called and completed for the same action in a prior turn or earlier in the current turn's sequential flow, for `mutating` and `elevated` risk class actions only; if the model batches `lens_run_action` and `lens_preview_action` in the same concurrent tool response for these risk classes, `lens_run_action` must be rejected; `read` actions are exempt from this constraint and may have `lens_preview_action` and `lens_run_action` dispatched in the same concurrent batch
- FR39: The `post_tool_call` hook records an audit log entry after `lens_run_action` completes, including tool name, active project, and result status
- FR40: The `on_session_end` hook clears session context without persisting state; fires at end of `run_conversation` and on CLI exit, covering `/new` and `/reset`
- FR41: The `lens_clarify_input` tool accepts a `parameter_name` and a `prompt_text` argument, presents the prompt to the user interactively, and returns a JSON object `{"parameter": "<name>", "value": "<user-provided-value>"}` ; when the user provides no value, it returns `{"parameter": "<name>", "value": null}` and the caller must treat the parameter as unresolved

---

## Non-Functional Requirements

### Performance

- NFR1: Catalog build for a Lens repo with up to 50 prompt files completes in under 2 seconds on a standard developer laptop with local filesystem access
- NFR2: `hermes lens select`, `hermes lens current`, and `lens_get_current_project` respond in under 500 milliseconds including project discovery scan
- NFR3: Preview payload generation for any action completes in under 1 second

### Security and Execution Safety

- NFR4: The plugin does not execute any filesystem operations outside the path of the selected active Lens project without explicit user authorization
- NFR5: No action is executed before a preview payload has been generated and the appropriate confirmation gate has completed
- NFR6: The plugin does not store, log, or transmit user credentials, authentication tokens, or API keys as part of any preview or execution flow
- NFR7: Heuristic risk classification defaults to `mutating` when classification is uncertain, never to `read`
- NFR8: All input values collected by the input clarification flow are scoped to the current turn and not persisted beyond it

### Reliability

- NFR9: Plugin load failure must not crash or prevent Hermes from starting; a clear plugin-load error message is emitted and Hermes continues normally
- NFR10: Hook execution failures are isolated and do not surface as unhandled exceptions to the Hermes agent or user
- NFR11: All tool handler functions return a valid JSON string in both success and error cases; no tool raises an exception to the caller
- NFR12: The plugin remains functional and returns clear error messages when the active project's `.github/prompts` directory is empty or contains no readable files

### Integration

- NFR13: The plugin uses only the documented Hermes general plugin API: `ctx.register_tool`, `ctx.register_hook` (with hooks `on_session_start`, `pre_llm_call`, `pre_tool_call`, `post_tool_call`, `on_session_end`), and `ctx.register_cli_command(name, help, setup_fn, handler_fn)`; `ctx.inject_message` is CLI-only and is not used in v1; `pre_tool_call` fires after Hermes resolves the handler from `tools/registry.py` but before Hermes's own danger/approval check — the plugin's preview-enforcement gate is the first interceptor in the per-tool execution chain; Hermes agent-level tools (`todo`, `memory`, `session_search`, `delegate_task`) are intercepted before `handle_function_call()` and bypass `pre_tool_call`/`post_tool_call` entirely — this does not affect plugin-registered tools, which all go through the registry and are fully covered by the hook chain
- NFR14: The plugin does not depend on any undocumented CLI slash-command surfaces for general plugins
- NFR15: The plugin is installable as a standard user-level Hermes plugin under `~/.hermes/plugins/` without any global configuration changes

### Ecosystem Assumption

- NFR16: The plugin adapts to variance in Lens repo prompt naming conventions; it does not require repos to adopt the optional metadata schema before being usable. Repos that adopt the metadata schema receive higher catalog confidence and better UX.
