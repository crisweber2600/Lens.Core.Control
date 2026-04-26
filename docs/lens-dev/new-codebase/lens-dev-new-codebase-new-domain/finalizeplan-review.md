---
feature: lens-dev-new-codebase-new-domain
doc_type: finalizeplan-review
status: approved
goal: "Final cross-artifact review of the new-domain command planning set, governance impact findings, and party-mode blind-spot challenge"
verdict: pass-with-warnings
findings_count:
  total: 6
  critical: 0
  high: 1
  medium: 5
  low: 0
key_decisions:
  - Verdict pass-with-warnings; no hard blockers; proceed to FinalizePlan bundle
  - Winston-P (High) — duplicate check must run AFTER sync_governance_main pull; must be captured in implementation story
  - F3/R1 (Medium) — SAFE_ID_PATTERN vs FEATURE_SLUG_PATTERN discrepancy must be resolved before implementation starts
open_questions: []
depends_on:
  - lens-dev-new-codebase-baseline
updated_at: 2026-04-26T00:00:00Z
---

# FinalizePlan Review — new-domain Command (lens-dev-new-codebase-new-domain)

**Author:** Lens FinalizePlan Conductor  
**Date:** 2026-04-26  
**Phase:** finalizeplan / step-1  
**Artifacts reviewed:** business-plan.md, tech-plan.md  
**Context inherited from baseline:** brainstorm.md, prd.md, product-brief.md, research.md, preplan-adversarial-review.md  
**Prior review verdict:** expressplan adversarial review — pass-with-warnings (inline, not committed; findings carried forward)

---

## Verdict Summary

**Overall verdict: `pass-with-warnings`**

No finding is a hard blocker. The planning set is coherent, complete, and implementation-ready subject to the carry-forward items below. Proceed to FinalizePlan bundle (epics, stories, implementation-readiness, sprint plan).

---

## Layer 1 — Cross-artifact Consistency

### F1 — Pass

Business plan success criteria and tech plan API contract are fully aligned. Every measurable success criterion in the business plan (domain marker created, constitution stub created, scaffolds, personal context, governance auto-commit, duplicate detection, dry-run fidelity, regression, schema stability) maps to a corresponding implementation signal in the tech plan's testing strategy.

### F2 — Pass

Schema freeze is consistent across both documents. The domain.yaml, constitution.md frontmatter, and context.yaml field definitions are identical in business-plan key decisions and tech-plan Data Model sections.

### F3 — Warning (carry-forward R1) — Medium

**Finding:** `SAFE_ID_PATTERN` is specified as `^[a-z0-9][a-z0-9-]{0,63}$` in the business plan and `^[a-z0-9][a-z0-9._-]{0,63}$` in the tech plan. These are different patterns.

**Impact:** If the implementation uses the looser pattern (dots and underscores), domain slugs like `lens.dev` or `my_domain` would be created that the `new-service` feature's validator could later reject, breaking the scaffolding pipeline.

**Action:** Resolve the exact pattern from the old-codebase `SAFE_ID_PATTERN` constant before implementation starts. The implementation story must embed the authoritative pattern with an explicit citation to the old-codebase source.

### F4 — Pass

`depends_on` and `blocks` relationships are symmetric and consistent across both artifacts.

---

## Layer 2 — Governance Cross-check

### F5 — Pass

The `lens-dev` domain constitution (`constitutions/lens-dev/constitution.md`) governs this feature. `gate_mode: informational` applies. All findings become implementation guidance, not hard phase gates.

### F6 — Pass

No concurrent governance write conflict. `lens-dev-new-codebase-new-service` and `lens-dev-new-codebase-new-feature` are both `phase: preplan` (not-started). They have not yet written any domain-related artifacts. The `domain.yaml`, `constitution.md`, and `context.yaml` paths are exclusively written by the new-domain feature.

### F7 — Pass (Low Background Risk)

`bmad-lens-constitution` reads `constitutions/{domain}/constitution.md` but does not write it. The tech plan correctly identifies it as a read-only inbound dependency. No write conflict.

### Governance Impact Summary

| Impact Area | Risk | Disposition |
|---|---|---|
| `features/{domain}/domain.yaml` write path | None (exclusive ownership) | Cleared |
| `constitutions/{domain}/constitution.md` write path | None (exclusive ownership) | Cleared |
| new-service, new-feature unblocked | Depends on F3 resolution | **Conditional: resolve slug pattern first** |
| Backward compatibility with existing domain.yaml files | None (schema frozen, no changes) | Cleared |

---

## Layer 3 — Implementation Readiness

### F8 — Pass

All 5 ADRs in the tech plan are decided with clear rationale and no open alternatives. No blocked decisions remain.

### F9 — Warning (carry-forward Murat/TEA) — Medium

**Finding:** Integration test isolation model is not specified. If integration tests create a real `domain.yaml` in a shared governance clone and fail mid-test, they leave dirty state that can corrupt subsequent test runs.

**Action:** Implementation story must specify: "All integration tests use isolated temp-dir governance repo fixtures. No shared governance clone is referenced in any test."

### F10 — Warning (carry-forward Winston/Architect) — Medium

**Finding:** The `make_domain_constitution_md()` parity test must reconstruct the expected constitution body from the spec, not from an old-codebase source copy (which would violate the clean-room constraint). The current tech plan describes the body only as "domain-name-interpolated default text."

**Action:** Implementation story must include the verbatim expected constitution body template (with `{domain}` as a single interpolation variable) as the authoritative test fixture spec.

---

## Party Mode — Blind Spot Challenge Round

### John (PM) — Medium: Slug derivation UX

**Finding:** The tech plan states "minimum ask: domain name" but does not specify whether the prompt stub auto-derives the domain slug from the display name (with a confirmation step) or expects the user to input the slug directly. If auto-derived, users with names containing spaces, special characters, or version numbers (e.g., "My Platform 2.0") could receive unexpected slug results with no override path.

**Recommendation:** Implementation story should specify: the skill prompt derives the slug from the name, shows it explicitly before any write, and requires a Yes/No confirmation. The user must be able to override the derived slug before proceeding.

### Winston (Architect) — **High: Duplicate check must follow sync pull**

**Finding:** The tech plan's duplicate check (`domain.yaml` exists → fail) is listed before `sync_governance_main` in the architecture diagram. If the check runs against a stale local governance clone that predates a recent remote write (e.g., another machine created the domain between our last pull and our check), the check passes on stale state, we pull, and then we overwrite an already-existing domain.yaml.

**Recommendation:** Duplicate check MUST run AFTER `sync_governance_main` pull, not before. The architecture diagram ordering in the tech plan must be updated. This is the single highest-priority implementation correctness fix in this planning set.

**Corrected implementation order (when --execute-governance-git):**
1. `validate_safe_id(domain)` — input validation first (no I/O)
2. `sync_governance_main(governance_repo)` — pull governance to latest
3. `duplicate check` — now against current state
4. Write artifacts
5. Governance git sequence

### Mary (Analyst) — Medium: Slug pattern cross-feature dependency gap

**Finding:** The business plan lists `new-service` and `new-feature` as blocked features but does not carry a dependency note about slug pattern resolution. If new-service development starts before F3/R1 is resolved, new-service may implement a different pattern, creating a future incompatibility for users who create domains with dots or underscores.

**Recommendation:** Add an explicit dependency note: "new-service and new-feature must not implement their slug validators until SAFE_ID_PATTERN is canonically resolved in lens-dev-new-codebase-new-domain and documented as a shared constant."

---

## Complete Findings Register

| ID | Layer | Severity | Description | Action Required |
|---|---|---|---|---|
| F3/R1 | Cross-artifact | **Medium** | SAFE_ID_PATTERN discrepancy between business plan and tech plan | Resolve from old-codebase source before implementation |
| F9 | Implementation readiness | **Medium** | Integration test isolation not specified | Specify temp-dir isolated fixtures in story |
| F10 | Implementation readiness | **Medium** | Constitution body template not verbatim in spec | Include verbatim template in story file |
| John-P | Party mode | **Medium** | Slug derivation UX: no confirmation step specified | Story must require slug-confirmation before write |
| Winston-P | Party mode | **High** | Duplicate check before pull creates race condition | Duplicate check MUST run after sync_governance_main |
| Mary-P | Party mode | **Medium** | Cross-feature slug pattern dependency not explicit | Add dependency note to blocks relationship |

**Total: 0 critical, 1 high, 5 medium, 0 low. Verdict: pass-with-warnings. No hard blockers.**

---

## Implementation Guidance Summary

The following guidance must be carried into the story files for development:

1. **Slug pattern:** Embed `SAFE_ID_PATTERN` from old-codebase `init-feature-ops.py` verbatim as a module constant, with a citation comment. Do not derive or guess the pattern.
2. **Operation order:** `validate_safe_id` → `sync_governance_main` → `duplicate_check` → `write_artifacts` → `governance_git_sequence` (Winston-P corrected order)
3. **UX:** Skill prompt derives slug from display name and presents it for explicit user confirmation before invoking the script.
4. **Test isolation:** All integration tests use `tmp_path` (pytest) or equivalent for governance repo root; no real governance clone touched.
5. **Parity test:** Constitution body template is spec-derived; the story must include the full verbatim template string as the expected fixture.
6. **Cross-feature:** Do not begin new-service or new-feature implementation until SAFE_ID_PATTERN is resolved and committed.
