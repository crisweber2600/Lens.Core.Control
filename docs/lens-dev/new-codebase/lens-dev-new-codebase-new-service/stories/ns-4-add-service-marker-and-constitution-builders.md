# Story NS-4: Add service marker and constitution builders

Status: ready-for-dev

## Story

As a script developer,
I want small, explicit helper functions for service marker and constitution creation,
so that `create-service` has a clean, testable foundation and does not duplicate domain-marker logic.

## Acceptance Criteria

1. `make_service_yaml(domain, service, name, username)` returns a dict matching the `tech-plan.md` Data Model schema
2. `make_service_constitution_md(domain, service)` returns a string with an inherited service constitution with stable fields
3. `get_service_marker_path(governance_repo, domain, service)` returns the correct governance filesystem path
4. `get_service_constitution_path(governance_repo, domain, service)` returns the correct governance filesystem path
5. **ADR-3 delegation boundary documented:** code comments in the `create-service` implementation and in NS-13 handoff notes confirm that parent-domain creation calls `make_domain_yaml`, `make_domain_constitution_md`, `get_domain_marker_path`, `get_domain_constitution_path` — no parallel domain-marker path inside `create-service`
6. NS-1 contract tests for helper function output fields turn green

## Tasks / Subtasks

- [ ] Task 1: Add helper functions to `init-feature-ops.py` (AC: 1–4)
  - [ ] Add `make_service_yaml(domain, service, name, username) -> dict`
  - [ ] Add `make_service_constitution_md(domain, service) -> str`
  - [ ] Add `get_service_marker_path(governance_repo, domain, service) -> Path`
  - [ ] Add `get_service_constitution_path(governance_repo, domain, service) -> Path`
- [ ] Task 2: Document ADR-3 delegation in a code comment (AC: 5)
  - [ ] Add a comment block near the `create-service` handler noting the exact `create-domain` helpers called for parent-domain creation
- [ ] Task 3: Verify NS-1 helper tests turn green (AC: 6)

## Dev Notes

- Script location: `lens.core/_bmad/lens-work/skills/bmad-lens-init-feature/scripts/init-feature-ops.py`
- **ADR-3 delegation boundary (H1):** the ONLY code path that creates a `domain.yaml` or domain constitution must be the existing `create-domain` family (`make_domain_yaml`, `make_domain_constitution_md`). Do NOT add a new implementation inside `create-service`. This eliminates the dual-mutation-path risk identified in the FinalizePlan review.
- Governance marker path convention (check existing `create-domain` for the exact folder structure): typically `{governance_repo}/domains/{domain}/services/{service}/service.yaml`
- Service constitution path: `{governance_repo}/domains/{domain}/services/{service}/service-constitution.md`
- Data model schema from `tech-plan.md`:
  ```yaml
  kind: service
  id: {domain}-{service}
  name: {display_name}
  domain: {domain}
  service: {service}
  status: active
  owner: {username}
  created: {timestamp}
  updated: {timestamp}
  ```

### Project Structure Notes

- Add helpers in the same section as existing `create-domain` helpers to keep the init-feature family cohesive
- Follow the existing atomic write pattern (write to temp, rename) used by `create-domain` helpers

### References

- [tech-plan.md § Data Model Changes](../tech-plan.md)
- [tech-plan.md § ADR-3 Auto-Establish Missing Parent Domain Container](../tech-plan.md)
- [finalizeplan-review.md § H1 response](../finalizeplan-review.md)
- [stories.md § NS-4](../stories.md)

## Dev Agent Record

### Agent Model Used

_to be filled by dev agent_

### Debug Log References

### Completion Notes List

### File List
