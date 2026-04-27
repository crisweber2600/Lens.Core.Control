# Story NS-2: Assert service-not-feature boundary

Status: ready-for-dev

## Story

As a governance enforcer,
I want tests that prove `create-service` never writes feature lifecycle artifacts,
so that I can be confident the service-container boundary is respected.

## Acceptance Criteria

1. Test confirms `create-service` does not create `feature.yaml`
2. Test confirms `create-service` does not create `summary.md`
3. Test confirms `create-service` does not create feature-index entries (no modification to `feature-index.yaml`)
4. Test confirms `create-service` does not create control branches
5. **Integration test — pre-existing domain container:** running `create-service` when a domain marker already exists must not overwrite or duplicate the existing domain marker or constitution; the command must detect the existing domain, skip parent-domain creation, and continue successfully

## Tasks / Subtasks

- [ ] Task 1: Add boundary tests to `tests/test_create_service.py` (AC: 1–4)
  - [ ] `test_create_service_no_feature_yaml` — run create-service; scan governance tree; assert no feature.yaml
  - [ ] `test_create_service_no_summary_md` — run create-service; assert no summary.md created
  - [ ] `test_create_service_no_feature_index_mutation` — snapshot feature-index before; run; assert file unchanged
  - [ ] `test_create_service_no_branch_creation` — run create-service; assert no new git branches in governance repo
- [ ] Task 2: Add idempotent parent-domain integration test (AC: 5)
  - [ ] Pre-populate governance repo with an existing domain marker and domain constitution
  - [ ] Run `create-service --domain {pre-existing} --service {new}`
  - [ ] Assert: existing domain marker content unchanged; no duplicate; service created; command exits successfully

## Dev Notes

- These tests belong in `tests/test_create_service.py` alongside NS-1 tests
- **ADR-2 enforced:** `create-service` must not create feature.yaml, summary.md, feature-index entries, or branches — these are strictly `new-feature` domain artifacts
- **ADR-3 pre-existing domain test (AC 5):** the domain marker path to check is `{governance_repo}/domains/{domain}/domain.yaml`
- Use `tmp_path` fixture to create an isolated governance tree; pre-populate the domain marker before running the command
- Verify by diffing the domain marker content before and after the command run

### Project Structure Notes

- Governance domain marker path: `{governance_repo}/domains/{domain}/domain.yaml` (verify against `new-domain` tests for exact path)
- Feature-index path: `{governance_repo}/feature-index.yaml`
- These tests may be red until NS-4–NS-5 implement `create-service`

### References

- [tech-plan.md § ADR-2 Preserve Service-as-Container Boundary](../tech-plan.md)
- [tech-plan.md § ADR-3 Auto-Establish Missing Parent Domain Container](../tech-plan.md)
- [sprint-plan.md § NS-2](../sprint-plan.md)
- [stories.md § NS-2](../stories.md)

## Dev Agent Record

### Agent Model Used

_to be filled by dev agent_

### Debug Log References

### Completion Notes List

### File List
