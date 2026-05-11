---
stepsCompleted:
  - step-01-init
  - step-02-discovery
  - step-02b-vision
  - step-02c-executive-summary
inputDocuments:
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-flatten/product-brief.md
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-flatten/research.md
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-flatten/brainstorm.md
  - docs/lens-dev/new-codebase/lens-dev-new-codebase-flatten/ux-design-specification.md
documentCounts:
  briefCount: 1
  researchCount: 1
  brainstormingCount: 1
  projectDocsCount: 1
workflowType: prd
workflowStatus: in-progress
classification:
  projectType: developer_tool
  domain: general
  complexity: medium
  projectContext: brownfield
---

# Product Requirements Document - build-output

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
