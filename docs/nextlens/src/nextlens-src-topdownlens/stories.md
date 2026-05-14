---
feature: nextlens-src-topdownlens
doc_type: stories
status: draft
track: express
phase: finalizeplan
depends_on: [epics]
blocks: []
updated_at: 2026-05-14T04:00:00Z
---

# Stories Index - TopDownLens Module

Per-story files live in `stories/`. Each file contains its own frontmatter, scope, acceptance criteria, and dev hand-off notes.

| Story | Title | Epic | Spine | Kind | Depends On |
|-------|-------|------|-------|------|------------|
| [TL-1](stories/TL-1.md) | Module Ontology And Storage Contract | 1 | yes | schema | (root) |
| [TL-2](stories/TL-2.md) | Top-Down Discovery Walkthrough | 3 | no | docs-only | TL-1 |
| [TL-3](stories/TL-3.md) | Bottom-Up Compatibility Rules | 3 | no | docs-only | TL-1 |
| [TL-4](stories/TL-4.md) | BMAD Bridge Packet | 1 | yes | schema | TL-1 |
| [TL-5](stories/TL-5.md) | Minimal Derived Graph Rebuild | 3 | no | cli | TL-1 |
| [TL-6](stories/TL-6.md) | Salmon Signal Contract | 1 | no | schema | TL-1 |
| [TL-7](stories/TL-7.md) | Doctor Checks | 3 | no | cli | TL-1, TL-4, TL-6 |
| [TL-8](stories/TL-8.md) | Self-Hosting Repo Topology Contract | 2 | yes | docs-only | TL-1 |
| [TL-9](stories/TL-9.md) | Constitution Layering For TopDownLens | 2 | yes | docs-only | TL-8 |
| [TL-10](stories/TL-10.md) | Bugfix Flow (Lens-Core-Bugfix Pattern) | 2 | no | docs-only | TL-6, TL-8 |
| [TL-11](stories/TL-11.md) | GitHub Actions Pipelines | 2 | no | cli | TL-8 |
| [TL-12](stories/TL-12.md) | Dogfooding Acceptance Run | 3 | yes | test | TL-1, TL-4, TL-8, TL-9 |

## Suggested Sprint Order

1. TL-1
2. TL-4
3. TL-8
4. TL-9
5. TL-2
6. TL-3
7. TL-6
8. TL-10
9. TL-11
10. TL-5
11. TL-7
12. TL-12 (closure gate, relaxed acceptance for first run)

## Notes

- Spine stories (TL-1, TL-4, TL-8, TL-9, TL-12) are mandatory for the first dev increment.
- TL-5 and TL-7 may slip to a later increment without blocking TL-12 closure.
- TL-12 acceptance for the first run skips `nextlens-release` verification.
