---
feature: hermes-lens-plugin
doc_type: product-brief
status: draft
goal: "Define the product vision, user value, and initial scope for a Hermes plugin that lets users select Lens projects and run prompt-backed Lens actions safely."
key_decisions:
  - "The product should feel like a friendly Lens command surface, not a raw prompt browser."
  - "The active Lens project is session-scoped and explicit."
  - "Execution must always be previewed, with confirmation required only for risky actions."
open_questions:
  - "What minimum prompt metadata should Lens repos adopt to improve action catalog quality?"
  - "Should the first release prioritize a CLI command browser, a chat-first experience, or both at equal depth?"
  - "Which Lens actions are in-scope for v1 versus later releases?"
depends_on:
  - brainstorm.md
  - research.md
blocks: []
updated_at: 2026-04-13T22:53:36Z
---

# Product Brief: Hermes Lens Project Command Plugin

## Executive Summary

Lens users who work across multiple local repositories have a coordination problem: they know their Lens workflows live in repo-specific prompt inventories, but the path from "I want to run a Lens command" to "I am operating safely in the right project" is still too manual. They have to remember which repo is active, inspect prompt folders directly, and reconstruct how to execute the right workflow without losing context.

The Hermes Lens Project Command Plugin solves that by turning Lens repos into a navigable, session-aware command surface inside Hermes. A user selects a project from local Lens repositories under `~/github/*.lens`, sees the available Lens actions as a friendly menu rather than a raw prompt directory, and runs those actions from the correct repository context with transparent previews and confirmation only when the action is risky.

This matters because Lens workflows are valuable only when they are easy to discover, safe to run, and hard to misapply. The plugin's job is not to invent new Lens capabilities. Its job is to remove the friction and ambiguity between a user's intent and the correct repo-scoped Lens action.

## The Problem

Today, repo-scoped Lens actions are too easy to mis-navigate and too hard to operationalize from memory. A user may have multiple Lens projects locally, each with its own prompt inventory and command vocabulary. Without an explicit project-selection step and a normalized action surface, the user is forced to:

- remember which project they intended to target
- browse prompt files directly
- infer what each prompt does
- decide how to run it in the right working directory
- mentally assess whether the action is read-only or risky

That creates cognitive overhead and a real trust problem. If the wrong repo is active or the execution plan is opaque, the user has to slow down and verify everything manually, which defeats the value of a command assistant.

## The Solution

Build a Hermes general plugin that gives Lens users a project-aware command layer.

The plugin should:

- discover candidate Lens projects under `~/github/*.lens`
- let the user explicitly choose the active project for the current Hermes session
- scan `.github/prompts` in the selected repo and build a normalized action catalog
- present actions as a friendly menu grouped by intent
- collect missing inputs interactively when needed
- preview the repo, action, inputs, execution plan, and risk level before running
- require confirmation only when the action is risky

The user experience should feel more like "work in this Lens project" than "open and inspect these prompt files." The plugin becomes the translator between repository prompt inventories and a safe, ergonomic Hermes workflow.

## What Makes This Different

The differentiation is not just that the plugin can run Lens commands. It is that it makes repo-scoped Lens work explicit, legible, and safe.

Key differentiators:

- **Session-scoped repo context:** the user always knows which Lens project is active.
- **Prompt catalog instead of raw files:** Lens prompts become understandable actions.
- **Hybrid control plane:** deterministic setup through CLI, guided execution through chat and tools.
- **Preview-first execution:** the plugin explains what will happen before anything runs.
- **Graceful metadata model:** prompt metadata improves UX when present without blocking adoption when absent.

## Who This Serves

### Primary users

- Developers and operators who work across multiple local Lens projects and want faster, safer access to repo-specific Lens workflows.
- Power users of Hermes who prefer an operational command surface over manual prompt browsing.

### Secondary users

- Teams standardizing Lens workflows across repositories who want a consistent operator experience.
- Maintainers of Lens repositories who want their prompt inventories to be discoverable and easier for others to run correctly.

## Success Criteria

The first release is successful if:

- a user can select a Lens repo once and reliably run subsequent actions in that project context
- the plugin can build a usable action catalog from `.github/prompts` in a real Lens repo
- users can understand what an action will do before execution
- risky actions are visibly distinguished and gated by confirmation
- the plugin reduces the need to browse prompt folders manually

Potential measurable signals:

- time from session start to first successful Lens action
- percentage of actions that can be cataloged without manual fallback
- rate of successful repo selection and context retention in-session
- low incidence of wrong-repo execution attempts

## Scope

### In scope for v1

- direct discovery of Lens repos under `~/github/*.lens`
- explicit session-scoped project selection
- normalized action catalog from `.github/prompts`
- friendly action listing grouped by intent
- interactive clarification for missing inputs
- mandatory preview for every action
- confirmation gate for risky actions only
- documented CLI entry points such as `hermes lens select` and `hermes lens commands`

### Out of scope for v1

- cross-session persistence of selected project state by default
- dependence on undocumented CLI slash-command extensions for general plugins
- automatic prompt-file rewriting or metadata generation
- scanning arbitrary filesystem roots without explicit user intent
- complex caching or background indexing beyond direct local discovery

## Vision

If this succeeds, Hermes becomes the best operational interface for Lens projects rather than just a generic agent shell that happens to run commands. The plugin evolves from a project selector into a Lens-native workflow surface: it knows how to orient a user in the current repo, present the right actions in the right language, explain risk clearly, and turn a prompt inventory into an approachable working environment.

Longer term, the plugin could support richer prompt metadata, better repo onboarding, reusable action taxonomies across Lens projects, and deeper integration between Lens workflow design and Hermes session behavior. But the first milestone is narrower and more important: make local Lens actions easy to find, safe to preview, and reliable to run in the correct project.
