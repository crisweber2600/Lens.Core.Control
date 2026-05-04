# Story PF-2.3: Implement post-request touched-repo publish policy

**Feature:** lens-dev-new-codebase-preandpostflight
**Epic:** Epic 2 - Explicit Request Lifecycle Sync Policy
**Status:** ready-for-dev
**Story Points:** 5

---

## Story

As a Lens maintainer,
I want post-request repo publication to be explicit and touched-repo driven,
so that qualifying work pushes or publishes by default while untouched repos remain
unchanged.

## Acceptance Criteria

1. Post-request handling inspects touched control and governance repos explicitly and no-ops untouched repos.
2. For qualifying touched repos, the default outcome is commit plus push or publish through the git orchestration surface, not local staging only.
3. Post-request failure taxonomy is explicit and testable: auth or push failure, branch-policy failure, reconcile conflict, publish CLI failure, and other errors that occur after user-visible work completes.
4. Post-request failures surface as a distinct "work completed but sync requires reconciliation" outcome; they do not silently downgrade to success.
5. Read-only requests never auto-publish when no eligible repo was touched.
6. The post-request path preserves the current log surface and does not add a second status channel.

## Tasks / Subtasks

- [ ] Implement touched-repo detection or explicit touched signals for control and governance repos.
- [ ] No-op untouched repos and default qualifying touched repos to commit plus push or publish.
- [ ] Encode the approved post-request failure taxonomy without reopening policy.
- [ ] Surface reconciliation-needed outcomes distinctly from clean success.
- [ ] Preserve the current log surface and keep read-only requests non-publishing when nothing eligible changed.

## Dev Notes

### Implementation Notes

- Governance publication must continue to route through `publish-to-governance`; do not hand-copy governance files.
- This story owns the post-request taxonomy copied into `implementation-readiness.md`.
- The desired default is publish or push for qualifying touched repos, not local-only staging.

### References

- [epics.md](../epics.md)
- [stories.md](../stories.md)
- [implementation-readiness.md](../implementation-readiness.md)
- [finalizeplan-review.md](../finalizeplan-review.md)
- `TargetProjects/lens-dev/new-codebase/lens.core.src/_bmad/lens-work/bmad-lens-git-orchestration/scripts/git-orchestration-ops.py`

## Dev Agent Record

### Agent Model Used

_(to be filled by dev agent)_

### Debug Log References

### Completion Notes List

### File List