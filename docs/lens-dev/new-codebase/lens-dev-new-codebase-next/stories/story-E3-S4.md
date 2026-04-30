# Story E3-S4: Confirm release readiness and update target_repos

**Feature:** lens-dev-new-codebase-next
**Epic:** Epic 3 — Delegation and Release Hardening
**Status:** ready-for-dev

---

## Story

As a Lens module maintainer,
I want to confirm that all Slice 4 acceptance criteria are met and that `feature.yaml`
reflects the final release-ready state
so that the feature can be closed and its artifacts published to the governance mirror.

## Acceptance Criteria

1. All stories E1-S1 through E3-S3 are marked done
2. `feature.yaml` `target_repos` includes `lens.core.src`
3. `next` command is discoverable and invokable in a clean session
4. No outstanding high or medium open items in `implementation-readiness.md`
5. Paused-state decision is recorded and fixtures pass (E2-S1, E2-S4)
6. Constitution resolver outcome is documented (E3-S3)
7. No-write negative test passes (E3-S2)
8. Announce readiness for governance publish via `/publish` or equivalent

## Tasks / Subtasks

- [ ] Verify all prior stories E1-S1 through E3-S3 are marked done
- [ ] Update `feature.yaml` `target_repos` to include `lens.core.src` (AC #2)
- [ ] Smoke test: invoke `/next` in a clean Copilot session; confirm it reaches the expected
  phase skill (AC #3)
- [ ] Review `implementation-readiness.md`: confirm no open H/M items remain (AC #4)
- [ ] Confirm E2-S4 paused-state fixture is passing (AC #5)
- [ ] Read E3-S3 Dev Notes: confirm gate outcome documented (AC #6)
- [ ] Confirm E3-S2 no-write test is passing (AC #7)
- [ ] Commit `feature.yaml` update to governance repo (AC #2)
- [ ] Push governance repo
- [ ] Signal `/publish` or equivalent to trigger governance mirror publish (AC #8)

## Dev Notes

- **feature.yaml path:**
  `TargetProjects/lens/lens-governance/features/lens-dev/new-codebase/lens-dev-new-codebase-next/feature.yaml`
- **target_repos field format** (follow existing patterns):
  ```yaml
  target_repos:
    - name: lens.core.src
      branch: develop
  ```

### References
- [implementation-readiness.md — release gate checklist](../implementation-readiness.md)
- [stories.md — full story list](../stories.md)
- [epics.md — Epic 3 release scope](../epics.md)

## Dev Agent Record

### Agent Model Used

_(to be filled by dev agent)_

### Debug Log References

### Completion Notes List

### File List
