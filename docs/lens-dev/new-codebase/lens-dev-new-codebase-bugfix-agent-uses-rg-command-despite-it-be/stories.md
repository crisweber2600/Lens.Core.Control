---
feature: lens-dev-new-codebase-bugfix-agent-uses-rg-command-despite-it-be
doc_type: stories
status: draft
updated_at: "2026-05-04T00:40:00Z"
---

# Stories — Environment, Orchestration and Tooling Fixes

This document lists all stories for this feature. Full story files are in `stories/`.

## Epic 1: Agent Environment and Documentation Hardening

### Story 1.1 — Document Unavailable Commands and Unsafe Patterns in AGENTS.md

**Story file:** `stories/1-1-document-unavailable-commands-and-unsafe-patterns-in-agents-md.md`  
**Status:** ready-for-dev  
**Target repo:** control repo (`AGENTS.md`)  
**FRs covered:** FR1 (rg), FR2 (PowerShell)

As a Lens agent, I want AGENTS.md to clearly document `rg` unavailability, the PowerShell heredoc prohibition, and the `create-pr` base-branch requirement, so that I do not waste turns retrying unavailable commands or corrupt prompt files.

### Story 1.2 — Add Merge-Base History Check to git-orchestration-ops.py create-pr

**Story file:** `stories/1-2-add-merge-base-history-check-to-git-orchestration-ops-create-pr.md`  
**Status:** ready-for-dev  
**Target repo:** source repo (`git-orchestration-ops.py`)  
**FRs covered:** FR5 (merge-base check)

As a Lens agent, I want `git-orchestration-ops.py create-pr` to automatically select the correct base branch by comparing merge-base timestamps, so that PRs never target a base branch that shares no history with the head.

---

## Epic 2: Orchestration Script Correctness and CLI Completeness

### Story 2.1 — Fix branch_for_phase_write Step3 Routing and Add Branch-Mismatch Hard Error

**Story file:** `stories/2-1-fix-branch-for-phase-write-step3-routing-and-add-branch-mismatch-hard-error.md`  
**Status:** ready-for-dev  
**Target repo:** source repo (`git-orchestration-ops.py`)  
**FRs covered:** FR3 (step3 routing), FR6 (branch mismatch hard error)

As a Lens agent, I want `commit-artifacts --phase finalizeplan --phase-step step3` to route to the `{featureId}` branch and emit a structured hard error on any branch mismatch, so that downstream bundles are committed to the correct branch.

### Story 2.2 — Add --pull-request Flag to feature-yaml-ops.py Update

**Story file:** `stories/2-2-add-pull-request-flag-to-feature-yaml-ops-update.md`  
**Status:** ready-for-dev  
**Target repo:** source repo (`feature-yaml-ops.py`)  
**FRs covered:** FR4 (--pull-request flag)

As a Lens agent, I want `feature-yaml-ops.py update --pull-request <url>` to set `links.pull_request` in feature.yaml, so that the PR link is stored in the governance record without a CLI error.
