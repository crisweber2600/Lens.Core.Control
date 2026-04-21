# lens-preplan Prompt Audit

## Purpose Summary

The prompt at `.github/prompts/lens-preplan.prompt.md` is an adapter stub, not the implementation surface. Its effective behavior is resolved through a two-step delegation chain:

1. `.github` stub delegates to `lens.core/_bmad/lens-work/prompts/lens-preplan.prompt.md`.
2. The release-module prompt stub delegates again to `lens.core/_bmad/lens-work/skills/bmad-lens-preplan/SKILL.md`.

The actual PrePlan contract is therefore skill-authored. It is explicitly "brainstorm-first," limits scope to PrePlan artifacts (`product-brief.md`, `research.md`, `brainstorm.md`), enforces governance-only grounding, and defers governance publication until BusinessPlan.

## BMAD Skill Mapping

Primary Lens conductor mapping:

- `bmad-lens-preplan` (`preplan`, code `QP`) is registered in module help as the phase command for PrePlan.
- It is wired to run between init-feature and businessplan (`after=bmad-lens-init-feature:create`, `before=bmad-lens-businessplan:plan`).

Delegated BMAD capability mapping inside PrePlan:

- Brainstorm leg: `bmad-lens-bmad-skill` -> `bmad-brainstorming`.
- Product brief synthesis: `bmad-lens-bmad-skill` -> `bmad-product-brief`.
- Research synthesis: `bmad-lens-bmad-skill` -> one of `bmad-domain-research`, `bmad-market-research`, `bmad-technical-research`.

Registry consistency check:

- All delegated BMAD skills listed above exist in `skill-manifest.csv`.
- `module-help.csv` contains Lens-specific wrapper rows for all five delegates.
- `bmad-help.csv` contains the base BMAD entries, but does not surface Lens-specific `bmad-lens-preplan` or Lens wrapper aliases.

## Lifecycle Fit

Alignment with lifecycle contract is strong:

- `lifecycle.yaml` defines `preplan` as phase 1 with required artifacts `[product-brief, research, brainstorm]` and auto-advance to `/businessplan`.
- Canonical phase order starts with `preplan` (`[preplan, businessplan, techplan, finalizeplan]`).
- Track-level fit is coherent:
  - `full` starts at `preplan`.
  - `spike` is preplan-only.

The `bmad-lens-preplan` skill contract is consistent with this lifecycle metadata:

- It explicitly forbids PRD work in PrePlan ("No PRD leap").
- It includes a review-ready fast path and phase gate handoff behavior that references lifecycle validation and adversarial review before `preplan-complete`.

## Evidence Refs

- `.github adapter stub`: `.github/prompts/lens-preplan.prompt.md:2`, `:5`, `:10`
- `release prompt stub`: `lens.core/_bmad/lens-work/prompts/lens-preplan.prompt.md:2`, `:5`, `:10`
- `module registration (skill + prompt)`: `lens.core/_bmad/lens-work/module.yaml:104-106`, `:210`, `:220`, `:286`
- `Lens command surface for preplan and delegates`: `lens.core/_bmad/lens-work/module-help.csv:48`, `:55-59`
- `global BMAD help baseline (non-Lens rows)`: `lens.core/_bmad/_config/bmad-help.csv:13`, `:19`, `:22-23`, `:29`, `:37`
- `lifecycle preplan contract`: `lens.core/_bmad/lens-work/lifecycle.yaml:109`, `:120`, `:125`, `:238`, `:244`, `:290`
- `preplan skill guardrails and artifacts`: `lens.core/_bmad/lens-work/skills/bmad-lens-preplan/SKILL.md:2`, `:41`, `:43`, `:70`, `:74`, `:78-80`
- `underlying BMAD skill registry presence`: `lens.core/_bmad/_config/skill-manifest.csv:3`, `:17-20`

## Confidence

High (0.91).

Rationale:

- The audit is grounded in direct file evidence for all user-requested sources.
- The prompt resolution chain and lifecycle positioning are explicitly declared in source metadata.
- Mapping claims are backed by both Lens module help and BMAD skill manifests.

## Gaps

1. `bmad-help.csv` does not include Lens-specific `bmad-lens-preplan` and Lens wrapper command rows, so users relying on the global BMAD help surface may miss Lens phase commands.
2. The prompt execution path is double-stubbed (`.github` stub -> release prompt stub -> skill). This is functional but increases indirection and can obscure where operative logic lives.
3. Naming is slightly fragmented across surfaces (`phase-1` in `module-help.csv` vs `preplan` in lifecycle/skills), which can create minor discoverability friction for operators and tooling.