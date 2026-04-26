---
feature: lens-dev-new-codebase-new-domain
doc_type: business-plan
status: draft
goal: "Deliver a clean-room reimplementation of the new-domain command that bootstraps governance domains with full behavioral parity to the old codebase"
key_decisions:
  - new-domain is retained as a first-class published command (#2 of 17) delegating to bmad-lens-init-feature create-domain; it is not a standalone skill
  - The command produces domain.yaml, constitution.md, optional TargetProjects scaffold, optional docs scaffold, and context.yaml atomically
  - governance-main auto-commit is preserved via --execute-governance-git flag; absence of the flag returns git commands for manual execution
  - context.yaml schema is frozen; clean-room rewrite must not change any field names or semantics (domain, service, updated_at, updated_by)
  - domain.yaml schema is frozen (kind, id, name, domain, status, owner, created, updated)
  - constitution.md frontmatter schema is frozen (permitted_tracks, required_artifacts, gate_mode, sensing_gate_mode, additional_review_participants, enforce_stories, enforce_review)
  - Duplicate detection is fail-fast: if domain.yaml already exists, return status fail before writing anything
  - Dry-run mode returns full planned-operations JSON without creating any files or executing any git commands
  - new-domain must bootstrap the personal context.yaml with service=null after successful domain creation
open_questions: []
depends_on:
  - lens-dev-new-codebase-baseline
blocks:
  - lens-dev-new-codebase-new-service
  - lens-dev-new-codebase-new-feature
updated_at: 2026-04-26T00:00:00Z
---

# Business Plan — new-domain Command (lens-dev-new-codebase-new-domain)

**Author:** CrisWeber  
**Date:** 2026-04-26

---

## Executive Summary

The `new-domain` command is retained command #2 in the lens-work 17-command surface. Its sole job is bootstrapping a new governance domain: creating the domain container marker (`domain.yaml`), the domain-level constitution stub (`constitution.md`), optional TargetProjects and docs workspace scaffolds, and persisting the newly created domain as the user's active context in the local personal folder (`context.yaml`). It is the prerequisite for both `new-service` and `new-feature`, which require a domain to exist before they can proceed. The clean-room constraint means the reimplementation must be freshly authored from the behavioral specification and old-codebase tests — without copying source — while producing output that is byte-for-byte schema-equivalent to the old codebase on every path. This is a parity delivery, not a feature expansion.

---

## Business Context

The lens-work rewrite reduces the published command surface from 54 stubs to 17 retained commands. `new-domain` is one of three scaffolding commands (`new-domain`, `new-service`, `new-feature`) that form the entry path for all new Lens work. In the current system:

- Users who need to create a new project domain run `/new-domain` and receive a complete governance scaffold and a ready-to-use constitution stub.
- The command also replaced the deprecated `/onboard` and `/new-project` bootstrapping paths: there is no other way to initialize a domain programmatically through Lens.
- `new-service` enforces that the parent domain already exists before proceeding; `new-feature` enforces that both domain and service exist. If `new-domain` is absent or broken, the entire scaffolding pipeline is broken.

**What happens if this is not built correctly:** Users who run `/new-domain` on the new codebase would encounter either a missing command (no prompt stub) or a command that writes the wrong schema, producing governance artifacts that downstream commands cannot read. Either failure would break the whole scaffolding track for every new project started on the new codebase — a visible, immediate regression.

The clean-room constraint is intentional: it forces the new implementation to be independently maintainable and verifiable, and it ensures the rewrite proves the product can be rebuilt from its own specifications rather than copy-pasted into existence.

---

## Stakeholders

| Stakeholder | Role | Interest |
|---|---|---|
| CrisWeber | Feature lead; module maintainer | Correct, testable implementation; no behavioral regression |
| New Lens users | Primary beneficiary | Clear domain bootstrapping path; command works on first invocation |
| Existing Lens users (adding new domains) | Secondary beneficiary | No behavioral regression; existing `domain.yaml` and `constitution.md` files remain valid and readable |
| `new-service` and `new-feature` features | Downstream dependents | Block on new-domain being complete: both features have domain-existence checks that depend on the `domain.yaml` schema new-domain writes |
| Lens module reviewers | Sign-off | Clear end-to-end requirement traceability; regression evidence before approval |

---

## Success Criteria

The following outcomes define a successful delivery. Each is measurable against the old-codebase behavior.

| Criterion | Passing Condition |
|---|---|
| Domain marker created | `{governance_repo}/features/{domain}/domain.yaml` exists with the correct schema after `create-domain` runs |
| Constitution stub created | `{governance_repo}/constitutions/{domain}/constitution.md` exists with correct default content (frontmatter + body sections) |
| TargetProjects scaffold | `{target_projects_path}/{domain}/.gitkeep` is created when `--target-projects-root` is provided |
| Docs scaffold | `{docs_root}/{domain}/.gitkeep` is created when `--docs-root` is provided |
| Personal context written | `.lens/personal/context.yaml` contains `domain: {domain}`, `service: null`, `updated_by: new-domain` |
| Governance auto-commit | `--execute-governance-git` performs checkout/pull/add/commit/push and returns `governance_commit_sha` |
| Duplicate detection | Returns `status: fail` immediately if `domain.yaml` already exists; no files are written |
| Dry-run fidelity | `--dry-run` returns the complete planned-operations JSON without writing any file or executing any git command |
| Regression suite passes | All domain-creation tests from the old-codebase `test-init-feature-ops.py` suite pass against the new implementation |
| No schema changes | All output schemas (domain.yaml, constitution.md, context.yaml) are identical to old-codebase; no field additions, removals, or renames |

---

## Scope

### In Scope

- `bmad-lens-init-feature` SKILL.md rewrite of the `Create Domain` capability
- `init-feature-ops.py` `create-domain` subcommand reimplementation (clean-room)
- `.github/prompts/lens-new-domain.prompt.md` stub (retained as published command)
- `lens.core/_bmad/lens-work/prompts/lens-new-domain.prompt.md` release prompt
- All outputs: `domain.yaml`, `constitution.md`, TargetProjects scaffold, docs scaffold, `context.yaml`
- `--execute-governance-git`, `--dry-run`, and `--personal-folder` flag behavior
- Regression test coverage for domain creation paths

### Out of Scope

- `create-service` subcommand (owned by `lens-dev-new-codebase-new-service`)
- `create` subcommand (owned by `lens-dev-new-codebase-new-feature`)
- `fetch-context` and `read-context` subcommands (internal utilities; not user-facing)
- Any change to the `domain.yaml`, `constitution.md`, or `context.yaml` schemas
- Improvements or expansions to the constitution default content beyond parity
- BMAD core module changes

---

## Risks and Mitigations

| Risk | Severity | Probability | Mitigation |
|---|---|---|---|
| Domain slug derivation drift — normalization rules change, existing domains unreachable | High | Low | Freeze `SAFE_ID_PATTERN = ^[a-z0-9][a-z0-9._-]{0,63}$`; regression test derivation against old-codebase fixture slugs |
| Constitution path template change — new-service and new-feature look in wrong location | High | Low | Anchor to `constitutions/{domain}/constitution.md` as a hard frozen contract in the tech plan |
| `context.yaml` schema break — field rename breaks `read-context` and downstream commands | High | Low | Freeze schema fields; include write/read round-trip test; no field may be added, removed, or renamed |
| Partial create (governance written, git push failed) — governance in inconsistent state | High | Medium | Implement governance git preflight (`sync_governance_main`) before any write; fail fast if governance repo is not clean |
| Duplicate detection bypass — two parallel invocations both pass the existence check | Medium | Low | Existence check happens immediately before write; governance git preflight ensures fresh state; risk documented as acceptable |
| Constitution default content divergence — default `permitted_tracks` or `gate_mode` differs from old codebase | Medium | Medium | Compare `make_domain_constitution_md` output character-for-character against old-codebase fixture; add content-equality assertion to regression suite |
| Clean-room constraint violated — accidental copy of old-codebase logic | Medium | Low | Peer review confirms no copied source; clean-room rationale documented in PRs |

---

## Dependencies

| Dependency | Type | Relationship |
|---|---|---|
| `lens-dev-new-codebase-baseline` | Feature | Defines the 17-command surface, rewrite invariants, frozen contracts, and clean-room policy that new-domain must observe |
| `bmad-lens-constitution` (internal) | Skill dependency | Invoked by the skill prompt for constitution context when presenting scaffold results |
| `bmad-lens-git-orchestration` (internal) | Shared utility | Referenced for governance-main sync semantics; create-domain handles its own governance git directly |
| Old-codebase `test-init-feature-ops.py` | Test reference | Domain-creation test cases are the parity acceptance baseline for the new implementation |

---

## Open Questions

None. All blocking questions from the baseline PrePlan adversarial review have been resolved:

- **C1 (schema version):** Resolved as v4.0 drop-in. No schema changes in this rewrite.
- **H3 (pause-resume audit):** Out of scope for new-domain; applies to `bmad-lens-dev`.

---

## Timeline Expectations

No external business deadline. Delivery is gated on `lens-dev-new-codebase-baseline` acceptance. new-domain is on the critical path for `new-service` and `new-feature`, so it should be delivered early in the rewrite sprint order. No urgency beyond the rewrite release window.
