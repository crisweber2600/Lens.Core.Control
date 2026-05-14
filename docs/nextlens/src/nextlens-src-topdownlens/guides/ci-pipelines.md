---
feature: nextlens-src-topdownlens
doc_type: guide
story_id: TL-11
title: GitHub Actions Pipelines
updated_at: 2026-05-14T05:10:00Z
---

# GitHub Actions Pipelines

TopDownLens needs three self-hosting pipeline surfaces. In this control workspace, root `.github/` is intentionally ignored and sync-generated, so TL-11 stores workflow templates with the rest of the feature documents under `docs/nextlens/src/nextlens-src-topdownlens/workflows/`. Installing them into a repo's root `.github/workflows/` directory still requires an approved setup or publication operation that owns that generated surface.

## Contract Validation

Template: `docs/nextlens/src/nextlens-src-topdownlens/workflows/contract-validation.yml`

Install target: `.github/workflows/contract-validation.yml` in the control repo or an equivalent validation repo.

Trigger: pull requests touching TopDownLens schemas/examples, plus manual dispatch.

Success criteria: every JSON Schema parses, and all entity, relationship, graph, BMAD packet, ontology, and Salmon signal examples validate against their schemas.

Failure routing: fix the schema or example in the control docs path before publication.

## Governance Publish Gate

Template: `docs/nextlens/src/nextlens-src-topdownlens/workflows/governance-publish-gate.yml`

Install target: `.github/workflows/governance-publish-gate.yml` in the governance-aware publishing repo.

Trigger: pull requests touching the TopDownLens feature docs path, plus manual dispatch for a feature ID.

Success criteria: required planning and dev artifacts exist before publication. The job is read-only and does not publish by itself.

Failure routing: restore the missing artifact or rerun the appropriate Lens lifecycle command. Governance publication must still occur through `publish-to-governance` or `lens-git-orchestration`.

## Regression And Doctor

Template: `docs/nextlens/src/nextlens-src-topdownlens/workflows/regression-and-doctor.yml`

Install target: `.github/workflows/regression-and-doctor.yml` in the target repo or integration repo that has the TopDownLens CLI scripts available.

Trigger: pushes to `*-dev`, relevant pull requests, and manual dispatch.

Success criteria: TL-5 rebuild runs in dry-run/check mode and TL-7 doctor returns no blocking findings in strict JSON mode.

Failure routing: fix broken references, stale derived graph output, missing packet traceability, or open blocking Salmon signals.

Skip behavior: `SKIP_DOCTOR=true` is an explicit non-publication mode. The job still exits non-zero in strict mode. To skip for local/non-publication exploration, dispatch with `skip_doctor=true` and `strict=false`; never use that mode as a publication gate.