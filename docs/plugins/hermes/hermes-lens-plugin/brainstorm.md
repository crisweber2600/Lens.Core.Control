---
feature: hermes-lens-plugin
doc_type: brainstorm
status: draft
goal: "Explore how a Hermes plugin should let a user select a Lens project and run prompt-backed Lens commands inside that project."
key_decisions:
  - "Use a hybrid CLI and chat control plane with `hermes lens select` plus a friendly command browser in chat."
  - "Build the action catalog from `.github/prompts` using hybrid inference by default and metadata when present."
  - "Keep repo selection in session-scoped shared context with no default cross-session persistence."
  - "Always show a preview and execution plan; require confirmation only for risky actions based on command effect."
open_questions:
  - "How should the plugin identify candidate Lens projects under ~/github/*.lens?"
  - "Should prompt-backed actions be surfaced as Hermes tools, a CLI subcommand, or both?"
  - "How should command execution safety, cwd switching, and prompt argument collection work?"
depends_on: []
blocks: []
updated_at: 2026-04-13T22:53:36Z
stepsCompleted: [1, 2, 3, 4]
inputDocuments:
  - TargetProjects/lens/lens-governance/features/plugins/hermes/hermes-lens-plugin/feature.yaml
  - TargetProjects/lens/lens-governance/features/plugins/hermes/hermes-lens-plugin/summary.md
  - TargetProjects/lens/lens-governance/constitutions/plugins/constitution.md
  - TargetProjects/lens/lens-governance/constitutions/plugins/hermes/constitution.md
session_topic: "Hermes plugin for selecting a Lens project from ~/github/*.lens and running Lens prompt-backed commands inside that project."
session_goals: "Define plugin behaviors, interaction patterns, command exposure, and implementation options for a Hermes-native Lens project selector and command runner."
selected_approach: 'ai-recommended'
techniques_used:
  - First Principles Thinking
  - Morphological Analysis
  - Constraint Mapping
ideas_generated:
  - Session-Scoped Repo Handle
  - Prompt Catalog as Action Index
  - Two-Step Execution Gate
  - Hybrid CLI and Chat Control Plane
  - Friendly Intent Menu
  - Metadata-Assisted Prompt Forms
  - Single Source Session Context
  - Execution Preview Contract
context_file: 'https://hermes-agent.nousresearch.com/docs/guides/build-a-hermes-plugin'
technique_execution_complete: true
session_active: false
workflow_completed: true
facilitation_notes:
  - "The user consistently optimized for explicit repo scope, graceful UX, and execution safety."
  - "The strongest differentiator was the hybrid CLI-plus-chat interaction model."
---

# Brainstorming Session Results

**Facilitator:** GitHub Copilot
**Date:** 2026-04-13T22:29:49Z

## Session Overview

**Topic:** Hermes plugin for selecting a Lens project from `~/github/*.lens` and running Lens prompt-backed commands inside that project.

**Goals:** Define plugin behaviors, interaction patterns, command exposure, and implementation options for a Hermes-native Lens project selector and command runner.

### Context Guidance

- Governance scope: `plugins/hermes` full-track preplan for feature `hermes-lens-plugin`.
- Plugin domain and service constitutions require standard planning progression and peer review, with no additional Hermes-specific constraints yet.
- Hermes plugin guidance confirms the core extension surface: plugin manifest, schemas, handlers returning JSON strings, registration, optional CLI commands, hooks, and bundled skills.

### Session Setup

- Confirmed the plugin should discover candidate Lens projects under `~/github/*.lens`.
- Confirmed the user should be able to select an active project, inspect available Lens prompts, and run Lens commands in that project context.
- Brainstorming remains in governance and design space for now; no implementation code or target-project source inspection is needed for this phase.

## Technique Selection

**Approach:** AI-Recommended Techniques
**Analysis Context:** Hermes plugin for selecting a Lens project from `~/github/*.lens` and running Lens prompt-backed commands inside that project.

**Recommended Techniques:**

- **First Principles Thinking:** Establish the irreducible truths, assumptions, and non-negotiables behind Lens project discovery, command exposure, and safe execution.
- **Morphological Analysis:** Explore the plugin design space systematically across dimensions like discovery, selection UX, action surfacing, argument collection, and execution strategy.
- **Constraint Mapping:** Separate real platform and product constraints from assumed ones so the viable plugin shapes are explicit.

**AI Rationale:** The session needs a structured solutioning flow: first challenge assumptions, then generate concrete architectural options, then narrow them using operational and safety constraints.

## Technique Execution Results

**First Principles Thinking:**

- **Interactive Focus:** Establish the minimum truths for project discovery, prompt inventory, execution scope, and safety.
- **Key Breakthroughs:** The plugin must be session-scoped, repo-bounded, prompt-driven, and confirmation-aware for risky actions.
- **User Creative Strengths:** Clear prioritization of must-haves versus forbidden behaviors.
- **Energy Level:** Focused and decisive.

### First Principles Checkpoint

- Must discover candidate Lens projects under `~/github/*.lens`.
- Must let the user explicitly choose the active project.
- Must read available actions from `.github/prompts`.
- Must execute the chosen action inside the selected repo cwd.
- Must remember the selected project for the current Hermes session.
- Must require confirmation for risky or mutating actions.
- Must never run outside the selected repo.
- Must never modify prompt files automatically.
- Must never scan outside `~/github` without being asked.
- Must never persist project state across sessions by default.
- Must never invent missing arguments silently.

### Initial Idea Set

**[Category #1]**: Session-Scoped Repo Handle
_Concept_: The plugin maintains an in-memory active project for the current Hermes session only. All discovery, listing, and execution tools resolve against that handle unless the user explicitly switches projects.
_Novelty_: This treats project choice as conversational context rather than persistent configuration, which matches the user's safety boundary.

**[Category #2]**: Prompt Catalog as Action Index
_Concept_: The plugin reads `.github/prompts` and turns prompt files into a normalized action catalog with display name, source file, likely inputs, and risk hints. Hermes then exposes the catalog instead of raw filesystem content.
_Novelty_: This separates prompt discovery from execution, making the plugin an indexer and runner rather than a blind shell wrapper.

**[Category #3]**: Two-Step Execution Gate
_Concept_: Every action goes through preview first: selected repo, chosen prompt/action, inferred arguments, and whether confirmation is required. Only after preview does the plugin execute.
_Novelty_: This makes safety and transparency part of the UX instead of bolted-on validation.

**[Category #4]**: Hybrid CLI and Chat Control Plane
_Concept_: Use a small CLI surface for deterministic tasks like selecting a project or listing actions, while keeping chat-native tools for running prompt-backed flows and collecting missing inputs interactively.
_Novelty_: This avoids forcing one interface to do both setup and high-context execution equally well.

### Direction Selection Checkpoint

- Strongest directions selected for deeper design:
  - Prompt catalog as action index
  - Two-step execution gate
  - Hybrid CLI and chat control plane
  - Session-scoped repo handle
- Confirmation model chosen: always preview, but only require an execution confirmation for risky actions.
- Preferred user experience: a friendly command menu grouped by intent rather than a raw file list.

### Morphological Analysis Checkpoint

- **Project discovery:** direct scan of `~/github/*.lens`.
- **Prompt catalog construction:** hybrid approach that infers from prompt files by default and uses metadata when present.
- **Primary entry point:** `hermes lens select` followed by a friendly command browser in chat.
- **Input handling:** interactive follow-up questions before execution, with structured fields when prompt metadata makes that possible.

**[Category #5]**: Friendly Intent Menu
_Concept_: Present prompt-backed actions grouped by user intent such as plan, review, switch, or init, instead of exposing a raw prompt directory. Raw lookup can remain a fallback, but the default surface is semantic and task-oriented.
_Novelty_: This turns a prompt folder into a product surface the user can navigate quickly, rather than requiring them to understand repository internals.

**[Category #6]**: Metadata-Assisted Prompt Forms
_Concept_: When prompt metadata exists, the plugin asks targeted follow-up questions for known parameters; otherwise it falls back to freeform interactive clarification. The same catalog entry can support both paths.
_Novelty_: This creates a graceful maturity model where prompts become more ergonomic over time without making metadata mandatory on day one.

### Constraint Mapping Checkpoint

- **Biggest usability risk:** CLI and chat state drift.
- **Primary risk signal:** the underlying command type or effect.
- **Preview must show:** risk level and why, plus the underlying Lens command or execution plan.
- **Fallback behavior:** degrade gracefully to generic prompt execution with clarification when metadata is missing or ambiguous.

**[Category #7]**: Single Source Session Context
_Concept_: Keep the selected repo and current action context in one shared session object that both CLI helpers and chat tools resolve through, instead of duplicating selection state per interface.
_Novelty_: This treats state drift as a first-class architectural problem, not an incidental bug to patch later.

**[Category #8]**: Execution Preview Contract
_Concept_: Define a standard preview payload for every action that explains what will run, why it is risky or safe, and what repo context it will execute against before any confirmation decision is made.
_Novelty_: This makes preview a product contract and audit surface, not just a courtesy message.

## Idea Organization and Prioritization

**Thematic Organization:**

**Theme 1: State and Session Context**
_Focus: keeping project selection coherent across chat and CLI surfaces_

- **Session-Scoped Repo Handle:** explicit active project within the Hermes session.
- **Hybrid CLI and Chat Control Plane:** deterministic setup via CLI, guided execution via chat.
- **Single Source Session Context:** one shared state model to prevent context drift.

**Pattern Insight:** the plugin succeeds only if repo selection behaves like one shared source of truth instead of two loosely related interfaces.

**Theme 2: Action Discovery and UX**
_Focus: turning prompt files into a usable product surface_

- **Prompt Catalog as Action Index:** prompts become structured actions instead of raw files.
- **Friendly Intent Menu:** actions are grouped by user intent such as plan, review, switch, and init.
- **Metadata-Assisted Prompt Forms:** metadata improves input collection without becoming mandatory.

**Pattern Insight:** the user wants the plugin to feel like a curated command surface, not a filesystem browser.

**Theme 3: Safe Execution Contract**
_Focus: making execution transparent and bounded before anything runs_

- **Two-Step Execution Gate:** every action is previewed before execution.
- **Execution Preview Contract:** the preview explains the real plan, context, and risk reasoning.

**Pattern Insight:** trust comes from making command effects legible before execution rather than from hiding complexity.

**Breakthrough Concepts:**

- **Hybrid CLI and Chat Control Plane:** the most strategically differentiated concept because it uses each Hermes surface for the job it does best.
- **Execution Preview Contract:** a strong trust and safety differentiator that can become part of the plugin's product identity.

**Prioritization Results:**

- **Top Priority Themes:** state and session context, action discovery and UX, and safe execution contract all remain critical.
- **Top Quick Win:** Prompt Catalog as Action Index.
- **Most Innovative Approach:** Hybrid CLI and Chat Control Plane.

**Action Planning:**

**Priority 1: Prompt Catalog as Action Index**
**Why This Matters:** It is the enabling quick win that turns `.github/prompts` into something Hermes can reliably expose.
**Next Steps:**

1. Define the normalized action catalog shape: display name, source prompt, intent group, confidence, inferred inputs, and risk hints.
2. Decide the inference rules for prompts with no metadata and the confidence labels for ambiguous entries.
3. Design the fallback path for low-confidence actions so they remain usable with clarification instead of disappearing.

**Resources Needed:** prompt parsing rules, prompt naming conventions, and sample Lens prompt inventories.
**Timeline:** short design spike followed by prototype implementation.
**Success Indicators:** a selected repo can produce a stable, human-friendly action list from `.github/prompts`.

**Priority 2: Single Source Session Context**
**Why This Matters:** The user identified CLI/chat state drift as the biggest usability risk.
**Next Steps:**

1. Define a shared session context object for active repo, last action, and preview state.
2. Map how `hermes lens select` updates that state and how chat tools read it.
3. Decide session lifetime behavior and reset semantics without cross-session persistence by default.

**Resources Needed:** Hermes session model assumptions and plugin state-handling conventions.
**Timeline:** medium design effort before implementation.
**Success Indicators:** switching repos once changes behavior consistently across both CLI and chat.

**Priority 3: Execution Preview Contract**
**Why This Matters:** Preview is the main trust mechanism for risky actions.
**Next Steps:**

1. Define the minimum preview payload: repo path, action name, collected inputs, execution plan, risk level, and risk rationale.
2. Specify what makes an action risky based on underlying command effect.
3. Establish the confirmation rule: always preview, confirm only when the action crosses the risk threshold.

**Resources Needed:** Lens command taxonomy and examples of safe versus mutating actions.
**Timeline:** medium design effort that should happen before execution code exists.
**Success Indicators:** users can tell what will happen before run and understand why confirmation is or is not required.

## Session Summary and Insights

**Key Achievements:**

- Organized **8 core ideas** across **3 major themes**.
- Identified the strongest product differentiator: a hybrid CLI-plus-chat control plane.
- Identified the biggest delivery risk: state drift between CLI and chat.
- Converted the brainstorm into concrete next-step design work for cataloging, state handling, and safe execution.

**Session Reflections:**

- The session consistently favored explicit scope, graceful degradation, and operational clarity over hidden magic.
- The most important non-functional property is user trust: repo scope, action meaning, and command effect must all stay visible.
- This brainstorming leg is complete and ready to feed both `research.md` and `product-brief.md` as follow-on preplan artifacts.