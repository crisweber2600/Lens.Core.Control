---
feature: lens-dev-new-codebase-flatten
doc_type: prd
status: draft
goal: "Define the product requirements for a governance-controlled flat control-repo workflow mode that preserves lifecycle rigor while removing control-repo branch and PR enforcement"
key_decisions:
  - Structured mode remains the default; flat mode is an explicit governance-controlled opt-in.
  - Flat mode removes control-repo branch and PR enforcement only; review artifacts, publish-to-governance, and phase-completion gates remain required.
  - Workflow mode is resolved through shared governance policy and must drive every mode-sensitive command surface consistently.
  - v1 blocks workflow-mode changes while active features exist instead of attempting branch-topology migration.
  - feature_id remains the canonical session-context key across both workflow modes.
open_questions:
  - Should review artifacts remain fully mandatory in flat mode even when control-repo PRs are removed?
  - What is the exact compatibility rule if governance changes workflow mode after features already exist?
  - Should the effective workflow mode surface only in command output, or also persist in user/session context for diagnostics?
  - What is the acceptance boundary for calling flat mode complete if dev-phase behavior is the last major structured-only surface?
depends_on: [brainstorm, research, product-brief, business-plan, preplan-adversarial-review]
blocks: []
updated_at: 2026-05-12T00:00:00Z
---

# Product Requirements Document - Governance-Controlled Flat Mode (lens-dev-new-codebase-flatten)

**Author:** CrisWeber
**Date:** 2026-05-08

## Executive Summary

Lens needs a governance-controlled control-repo workflow mode that preserves lifecycle rigor while reducing process overhead for teams not ready for branch-heavy planning. The product introduces two modes: `structured` (default, existing behavior) and `flat` (no control-repo branch/PR enforcement). This feature targets new Lens adopters, small teams, and multi-service teams that need reliable feature context and phase flow without mandatory control-repo branch choreography. The implementation scope is limited to control-repo workflow behavior; governance publication boundaries, lifecycle gates, and target-repo branching strategy remain separate concerns.

### What Makes This Special

This is not a "lighter planning shortcut"; it is a policy-driven operating mode that keeps one lifecycle model while allowing different control-repo execution styles. The differentiator is centralized governance resolution of workflow mode, then deterministic behavior across commands (`new-feature`, `switch`, `preflight`, `git-state`, orchestration, and downstream phase behavior). Teams keep consistent feature identity and auditable phase progression regardless of mode, while structured-mode users experience zero regression.

## Project Classification

- **Project Type:** developer_tool
- **Domain:** general
- **Complexity:** medium
- **Project Context:** brownfield
