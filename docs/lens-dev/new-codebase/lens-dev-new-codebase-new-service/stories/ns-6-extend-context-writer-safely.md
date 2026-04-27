# Story NS-6: Extend context writer safely

Status: ready-for-dev

## Story

As a domain setup operator,
I want context writing to support both domain-only and domain-plus-service activation through one shared helper,
so that there is no schema drift between `new-domain` and `new-service` context files.

## Acceptance Criteria

1. Context writer accepts `service: str | None` parameter without breaking existing `new-domain` calls (service=None produces unchanged output)
2. Domain-only context write (service=None) produces output identical to baseline `new-domain` context output
3. Domain-plus-service context write includes both `domain` and `service` fields in `.lens/personal/context.yaml`
4. Context YAML path remains `.lens/personal/context.yaml` — no change to the file location
5. NS-1 context output tests turn green
6. Existing `new-domain` context tests remain green (no regression)

## Tasks / Subtasks

- [ ] Task 1: Extend context writer helper (AC: 1–4)
  - [ ] Find the existing context write helper used by `create-domain`
  - [ ] Add `service: str | None = None` parameter
  - [ ] When `service` is provided, include `service` field in written YAML
  - [ ] When `service` is None, produce output identical to original (backward compatibility)
  - [ ] Ensure `updated_by` field is set to `new-service` when called from `create-service`
- [ ] Task 2: Call extended helper from `handle_create_service` (NS-5 placeholder) (AC: 5)
- [ ] Task 3: Verify `new-domain` context tests still pass (AC: 6)

## Dev Notes

- Context schema (from `tech-plan.md` Data Model):
  ```yaml
  domain: {domain}
  service: {service}        # only present when service provided
  updated_at: {timestamp}
  updated_by: new-service   # or new-domain, depending on caller
  ```
- Context file path: `.lens/personal/context.yaml` — this is a personal-output-folder path, not governance
- The `personal_output_folder` is passed in as an argument; resolve the context path as `{personal_output_folder}/context.yaml`
- **Do not create a separate context writer** — extend the existing one (ADR-4)
- Run the full init-feature test suite after this change to catch any `new-domain` regressions early

### Project Structure Notes

- Personal context path resolves from `--personal-folder` arg, not from a hardcoded path
- Existing `new-domain` tests likely call the context helper with no `service` param — confirm they still pass

### References

- [tech-plan.md § ADR-4 Extend Context Writer](../tech-plan.md)
- [tech-plan.md § Data Model Changes](../tech-plan.md)
- [stories.md § NS-6](../stories.md)

## Dev Agent Record

### Agent Model Used

_to be filled by dev agent_

### Debug Log References

### Completion Notes List

### File List
