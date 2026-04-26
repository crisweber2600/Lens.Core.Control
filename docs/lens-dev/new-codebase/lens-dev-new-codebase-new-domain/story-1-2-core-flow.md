---
feature_id: lens-dev-new-codebase-new-domain
story_key: "1-2-core-flow"
epic: 1
story: 2
title: "Implement create-domain core flow"
type: implementation
estimate: M
priority: P0
status: not-started
assigned: crisweber2600
sprint: 1
depends_on:
  - "1-1-safe-id-pattern"
blocks:
  - "1-3-dry-run"
  - "1-4-parity-tests"
  - "1-5-integration-tests"
created_at: 2026-04-26T00:00:00Z
updated_at: 2026-04-26T00:00:00Z
---

# Story 1.2 — Implement create-domain core flow

## ⚠️ Critical Correctness Requirement — READ FIRST

**Winston-P finding (High severity, from finalizeplan-review.md):**

The correct operation order when `--execute-governance-git` is used is:

```
1. validate_safe_id(domain)          ← fail fast on bad slug
2. sync_governance_main(governance_repo)  ← pull FIRST, before duplicate check
3. duplicate_check(governance_repo, domain)  ← check AFTER pull
4. write_domain_yaml(...)
5. write_constitution_md(...)
6. write_scaffolds(...) [if flags provided]
7. write_context_yaml(...) [if --personal-folder provided]
8. governance_git_sequence(...)
```

**The old-codebase architecture diagram shows the wrong order** (duplicate_check before sync_governance_main). Use the corrected order from finalizeplan-review.md. This is a race condition fix: running duplicate_check on a stale clone can allow two agents to create the same domain simultaneously.

When `--execute-governance-git` is NOT set:
```
1. validate_safe_id(domain)
2. duplicate_check(governance_repo, domain)  ← check on local clone, no pull needed
3. write_domain_yaml(...)
4. write_constitution_md(...)
5. write_scaffolds(...) [if flags provided]
6. write_context_yaml(...) [if --personal-folder provided]
```

---

## What To Build

The `create-domain` subcommand handler in the new `init-feature-ops.py`.

---

## File Locations

| File | Action |
|---|---|
| `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-init-feature/scripts/init-feature-ops.py` | Add create-domain subcommand |

---

## API Contract

**Invocation:**
```bash
uv run init-feature-ops.py create-domain \
  --governance-repo /path/to/governance \
  --domain {domain-slug} \
  [--name "{Display Name}"] \
  [--username {username}] \
  [--target-projects-root /path] \
  [--docs-root /path] \
  [--personal-folder /path] \
  [--execute-governance-git] \
  [--dry-run]
```

**Return JSON (stdout):**

The `create-domain` stdout JSON contract for this story is the canonical contract defined in the tech plan and must be reused verbatim here. Do not introduce a story-specific variant.

In particular:
- `status` must use `"pass" | "fail"` (not `"ok" | "fail"`).
- The response field set must exactly match the canonical tech-plan contract.
- Do not introduce alternate aliases such as `domain`/`scaffolds_created` if the canonical contract uses different field names.
**Exit codes:** 0 for success, 1 for any failure.

---

## Output File Schemas (frozen — do not deviate)

### domain.yaml
```yaml
kind: domain
id: {domain}
name: {name}
domain: {domain}
status: active
owner: {username}
created: {ISO-8601}
updated: {ISO-8601}
```

- `name` defaults to `{domain}` when `--name` is not provided  
- `username` defaults to `""` when `--username` is not provided  
- `created` and `updated` are set to the same timestamp at creation time  
- All fields are present even when `username` is empty  

### constitution.md

See Story 1.4 for the verbatim constitution body template. The `make_domain_constitution_md(domain)` function must produce output matching that template exactly.

### context.yaml
```yaml
domain: {domain}
service: null
updated_at: {ISO-8601}
updated_by: new-domain
```

- Only written when `--personal-folder` is provided  
- `service` is always `null` (literal YAML null) for domain-level context  
- `updated_by` is always the literal string `"new-domain"`  

---

## Acceptance Criteria

- [ ] `create-domain` subcommand registered in argparse (or equivalent)
- [ ] All flags listed in API contract above are accepted
- [ ] Operation order with `--execute-governance-git` matches the corrected sequence (see top of this file)
- [ ] Operation order without `--execute-governance-git` matches the no-pull sequence
- [ ] `domain.yaml` written at correct path with frozen schema
- [ ] `constitution.md` written at correct path with frozen body
- [ ] When `--target-projects-root` provided: `.gitkeep` created at `{target_projects_root}/{domain}/.gitkeep`
- [ ] When `--docs-root` provided: `.gitkeep` created at `{docs_root}/{domain}/.gitkeep`
- [ ] When `--personal-folder` provided: `context.yaml` written with frozen schema
- [ ] Returns `status: fail` and exit code 1 when `domain.yaml` already exists (duplicate_check)
- [ ] Returns `status: fail` and exit code 1 when `sync_governance_main` fails (with `--execute-governance-git`)
- [ ] Returns `status: ok` with all paths populated on success
- [ ] `governance_git_executed: true` and `governance_commit_sha` present when auto-git succeeds
- [ ] `remaining_git_commands` populated when `--execute-governance-git` NOT used
- [ ] Exit code 0 on success, 1 on failure (no unhandled exceptions reach the user)

## Design Review Requirement

Before merging Story 1.2: conduct a code walkthrough with a reviewer specifically checking the operation order. The reviewer must confirm that `sync_governance_main` is called before `duplicate_check` in the `--execute-governance-git` code path.
