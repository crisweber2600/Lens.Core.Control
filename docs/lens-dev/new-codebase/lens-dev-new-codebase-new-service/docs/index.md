# Project Index: bmad-lens-init-feature (new-service enhancement)

**Generated:** 2026-04-27  
**Feature:** lens-dev-new-codebase-new-service  
**Scope:** `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/skills/bmad-lens-init-feature/`  
**Project type:** Python CLI library (uv-runnable inline-dependency script)

---

## Project Structure

```
bmad-lens-init-feature/
├── SKILL.md                          — LLM skill definition; guides the agent through create-domain and create-service intent flows
├── assets/                           — Static templates and reference assets
└── scripts/
    ├── init-feature-ops.py           — Core CLI script; exposes create-domain and create-service subcommands
    └── tests/
        ├── test-init-feature-ops.py  — Contract tests for create-domain
        └── test-create-service-ops.py — Contract and boundary tests for create-service (new, this feature)
```

---

## Key Files

| File | Type | Description |
|------|------|-------------|
| `SKILL.md` | Agent skill | Intent flow definitions for new-domain and new-service; frontmatter description, non-negotiables |
| `scripts/init-feature-ops.py` | Python CLI (798 lines) | Core implementation: domain + service container creation, governance YAML writers, git orchestration |
| `scripts/tests/test-create-service-ops.py` | pytest | 16 tests covering CLI contract, boundary (not-a-feature), and discovery surfaces |
| `scripts/tests/test-init-feature-ops.py` | pytest | Existing tests for create-domain (11 passing, 1 pre-existing failure) |

---

## Parts Documentation

- [project-overview.md](./project-overview.md) — Architecture, commands, API contracts, and key decisions
