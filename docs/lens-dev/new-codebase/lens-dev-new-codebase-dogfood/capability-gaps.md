---
feature: lens-dev-new-codebase-dogfood
doc_type: capability-gaps
status: draft
updated_at: "2025-07-17"
---

# Capability Gaps — Discover and Document-Project

This report compares the old-codebase implementations of `bmad-lens-discover` and
`bmad-lens-document-project` against the new-codebase state, and classifies each
gap into one of three categories:

- **(a) Implemented** — feature present and working in new-codebase
- **(b) Deferred with known scope** — feature intentionally omitted; scope is understood
- **(c) Requires research** — gap exists but the correct implementation approach is unclear

---

## bmad-lens-discover

### Overview

| Aspect | Old-codebase | New-codebase |
|---|---|---|
| SKILL.md | Full implementation with interactive/headless/dry-run modes | **Full implementation** — parity achieved in new-codebase |
| `discover-ops.py scan` | Script-backed, `scan` subcommand | Script-backed; same subcommand shape |
| `discover-ops.py add-entry` | Idempotent by `remote_url` | Idempotent by `remote_url` — same contract |
| `discover-ops.py validate` | Required `name` + `remote_url` | Same contract |
| Headless mode (`-H`) | Supported | Supported |
| Dry-run mode | Supported | Supported |
| Auto-commit governance main | Inline in skill, hash-gated | Inline in skill, hash-gated |
| Config resolution | `.lens/governance-setup.yaml` → `bmadconfig.yaml` | Same resolution order |
| Legacy `repos:` key support | Via `scan` | `scan` accepts both `repositories:` and `repos:` |
| Clone path derivation | `TargetProjects/` prefix handling | Same rule |

### Gap Analysis

| Gap | Category | Notes |
|---|---|---|
| SKILL.md content parity | (a) Implemented | New-codebase SKILL.md covers all modes and subcommands from old-codebase |
| `discover-ops.py` script | (a) Implemented | Script present; all three subcommands covered in SKILL.md |
| No drift from old-codebase | — | No capability gaps identified for `bmad-lens-discover` |

**Summary:** `bmad-lens-discover` has full parity with the old-codebase implementation. No deferred items.

---

## bmad-lens-document-project

### Overview

| Aspect | Old-codebase | New-codebase |
|---|---|---|
| SKILL.md | Full implementation (feature-aware output paths, governance copy, delegation to `bmad-document-project`) | **Absent** — no `bmad-lens-document-project/SKILL.md` exists |
| Prompt stub | `lens-bmad-document-project.prompt.md` present | No corresponding prompt |
| `module.yaml` registration | Not explicitly registered in `module.yaml` | Not registered |
| Feature-aware docs path routing | Reads `feature.yaml` → `docs.path` and `docs.governance_docs_path` | Not implemented |
| Governance repo copy (index.md, project-overview.md) | Written to `{governance_repo_path}/{resolved_governance_docs_path}` | Not implemented |
| User fallback when no feature context | 3-option prompt (manual / flat / cancel) | Not implemented |
| Dependency on `bmad-document-project` core skill | Delegates to `.github/skills/bmad-document-project/SKILL.md` | Core skill available but lens wrapper absent |

### Gap Analysis

| Gap | Category | Notes |
|---|---|---|
| `bmad-lens-document-project` SKILL.md absent | **(b) Deferred with known scope** | Scope is fully documented in the old-codebase SKILL.md. Implementation requires: create `skills/bmad-lens-document-project/SKILL.md`, register in `module.yaml`, add `lens-document-project.prompt.md` prompt stub. Estimated size: M (medium). |
| Feature-aware output path routing | (b) Deferred — blocked by SKILL.md absence | Reads `feature.yaml.docs.path` and `feature.yaml.docs.governance_docs_path`. Deterministic once SKILL.md is created. |
| Governance repo documentation copy | (b) Deferred — blocked by SKILL.md absence | Copies `index.md` + `project-overview.md` to governance. Deterministic once SKILL.md is created. |
| Prompt stub `lens-document-project.prompt.md` | (b) Deferred | Simple stub once SKILL.md exists. |
| `module.yaml` registration | (b) Deferred | One-line addition to `prompts:` list and optional `skills:` entry. |

**Summary:** `bmad-lens-document-project` is entirely absent from the new-codebase. The gap is fully understood — the old-codebase implementation is the specification. No research needed.

---

## Remediation Plan

### Discover

No remediation required. `bmad-lens-discover` is at parity.

### Document-Project

Implement in a follow-on story with the following scope:

1. Create `skills/bmad-lens-document-project/SKILL.md` from old-codebase specification.
2. Add `lens-document-project.prompt.md` prompt stub to `prompts/`.
3. Register in `module.yaml`:
   - Add `lens-document-project.prompt.md` to `prompts:` list.
   - Add `bmad-lens-document-project` to `skills:` list (or note in a deferred skills comment).
4. Write integration tests: feature-context path routing, no-context fallback, governance copy contract.

**Estimated effort:** M (medium, 1–2 dev sessions).
**Priority:** Non-blocking for current dogfood validation. Should be scheduled in the next release cycle.

---

## module.yaml Deferred Gap Comment

The following comment has been added to `module.yaml` (see that file for details):

```yaml
# DEFERRED: bmad-lens-document-project — skill absent in new-codebase.
# Full specification available in old-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-document-project/SKILL.md.
# Scope: feature-aware output path routing, governance docs copy, prompt stub, module.yaml registration.
# Schedule for next release cycle.
```
