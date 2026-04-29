---
review_format: abc-choice-v1
feature: lens-dev-new-codebase-discover
doc_type: finalizeplan-review
status: in-review
goal: "Final cross-artifact review of the discover command expressplan bundle before PR handoff and downstream bundle generation"
key_decisions:
  - Express track artifact set (business-plan, tech-plan, sprint-plan) is accepted as the complete planning set for FinalizePlan entry
  - All expressplan pass-with-warnings findings are documented and accepted for resolution before Story 5.4.1 begins
  - Governance impact is advisory only (gate_mode informational per domain constitution)
open_questions:
  - Hash comparison ownership (inline agent vs script subcommand) — resolve before Story 5.4.2
  - SC-5 scope (feature-contained or domain-wide audit+remediation) — resolve before Story 5.4.7
  - No-remote edge case behavior for untracked repos — resolve before Story 5.4.1 AC finalization
depends_on: [business-plan, tech-plan, sprint-plan, expressplan-adversarial-review]
blocks: []
updated_at: 2026-04-28T00:00:00Z
---

# FinalizePlan Review: lens-dev-new-codebase-discover

**Reviewed:** 2026-04-28T00:00:00Z
**Source:** manual-rerun (FinalizePlan Step 1)
**Track:** express
**Artifact Set:** expressplan bundle (business-plan.md, tech-plan.md, sprint-plan.md, expressplan-adversarial-review.md)
**Overall Rating:** pass-with-warnings

---

## Summary

The discover command expressplan bundle is internally consistent, tightly scoped, and correctly bounded around two behavioral contracts unique to this command: bidirectional inventory sync and the governance-main auto-commit exception. The expressplan adversarial review (pass-with-warnings) surfaced five medium findings and one high finding — all are documented, accepted, and mapped to specific pre-work items in the sprint plan. No new critical findings are identified at the finalizeplan review stage. 

**Governance impact assessment:** All sensing is advisory. No other active `lens-dev/new-codebase` feature is in a state that conflicts with this work package. Most sibling features are `preplan` or `archived`.

**Recommendation:** Proceed to Step 2 — open the `{featureId}-plan` → `{featureId}` planning PR.

---

## Cross-Artifact Consistency Check

| Check | Result | Note |
|---|---|---|
| Business plan goal aligns with tech plan scope | ✓ Pass | Both target the same two behavioral contracts |
| Tech plan ADRs are grounded in business plan decisions | ✓ Pass | ADR-1 through ADR-4 each trace to a business plan constraint |
| Sprint stories cover all business plan success criteria | ✓ Pass | SC-1 through SC-7 each have at least one story AC |
| Sprint stories reference tech plan test matrix | ✓ Pass | Stories 5.4.4 and 5.4.5 explicitly reference T1–T8 |
| Open questions in business plan reflected in sprint ACs | ✓ Pass | OQ1–OQ5 from adversarial review mapped to Story 5.4.1 / 5.4.2 pre-work |
| Adversarial review findings acknowledged in sprint plan | ✓ Pass | Sprint plan DoD references review findings as pre-work items |

---

## Governance Impact Assessment

**Cross-initiative sensing:** `lens-dev / new-codebase` domain.

| Feature | Status | Conflict Risk |
|---|---|---|
| lens-dev-new-codebase-baseline | preplan | Low — baseline is the parent split; shared ancestry but no active conflict |
| lens-dev-new-codebase-preflight | preplan | Low — different command; no shared file scope |
| lens-dev-new-codebase-new-feature | preplan | Low — different command; no shared file scope |
| lens-dev-new-codebase-upgrade | preplan | Medium (watch) — upgrade command may touch `bmadconfig.yaml` schema; discover's config resolution depends on same config structure |
| lens-dev-new-codebase-finalizeplan | preplan | Low — different command; no shared file scope |
| lens-dev-new-codebase-new-service | archived | None |
| lens-dev-new-codebase-new-domain | archived | None |

**Notable governance impact — `lens-dev-new-codebase-upgrade`:** The upgrade command feature (currently `preplan`) will eventually modify `lifecycle.yaml` schema or `bmadconfig.yaml` structure. The discover command's config resolution depends on `bmadconfig.yaml` being stable. If upgrade modifies the `governance_repo_path` config key name or moves it, discover's fallback path resolution will break silently. This is a dependency to watch, not a blocker.

**Action item:** When `lens-dev-new-codebase-upgrade` reaches techplan, the discover team should verify that `bmadconfig.yaml` path conventions are stable or that discover's config resolution handles both old and new keys.

---

## Prior Review Findings Resolution Status

All five medium findings and one high finding from `expressplan-adversarial-review.md` are carried forward as pre-work items for Sprint 1. None were resolved during the review period. This is acceptable for `pass-with-warnings` — findings are documented and acknowledged.

| Finding | Severity | Resolution Target | Status |
|---|---|---|---|
| H1 — Story 5.4.1 scope unbounded without pre-assessment | High | Story 5.4.1 start — add bounded assessment step | Open (pre-work) |
| M1 — Hash comparison ownership ambiguous | Medium | Story 5.4.2 before coding | Open (pre-work) |
| M2 — SC-5 vs Story 5.4.7 conflict | Medium | Story 5.4.7 scope decision | Open (pre-work) |
| M3 — No-remote edge case unhandled | Medium | Story 5.4.1 AC finalization | Open (pre-work) |
| M4 — Dry-run regression test absent | Medium | Story 5.4.4 scope | Open (pre-work) |
| M5 — T8 validates scan not commit absence | Medium | Story 5.4.5 scope | Open (pre-work) |

---

## Party-Mode Challenge

**Elena (Governance Custodian):** I want to make sure we're clear on the dependency risk with the upgrade command. Discover's entire runtime behavior depends on `bmadconfig.yaml` being stable. If someone changes `governance_repo_path` to `governance-repo-path` in the upgrade work, discover fails silently in production with no error until someone runs it and gets confused about why the config resolves wrong. That's a runtime bug with no test coverage. Before `dev-complete`, I'd want a test that explicitly validates config resolution against a known key name.

**Marcus (Implementation Lead):** The thing that keeps me up at night is the no-remote edge case. In practice, roughly 20% of local repos in a fresh workspace are locally-initialised without a remote yet — test repos, scratch work, etc. If `discover` crashes or skips them silently every time, users will get incomplete sync reports and won't understand why. This isn't an edge case anymore — it's a mainstream case in the way people actually work. Story 5.4.1 needs to address this before any other story starts.

**Yuki (QA Champion):** The story dependency graph in the sprint plan says 5.4.4 and 5.4.5 depend on 5.4.2 and 5.4.3 finishing first. But what if Story 5.4.1 (the SKILL.md review) discovers that the existing implementation has a completely different structure than the tech plan assumes? We'd have to rewrite the test cases. I'd sequence it as: 5.4.1 assessment first → if scope expands, update 5.4.2 and 5.4.3 accordingly → only then start 5.4.4 and 5.4.5. The current plan assumes the implementation is close to spec; the assessment gate should validate that assumption before later stories are committed to.

---

## Gaps You May Not Have Considered

1. **Config key stability dependency on the upgrade command.** Discover's runtime depends on `bmadconfig.yaml` key conventions being stable. No test validates the config resolution path end-to-end. This gap would cause a silent failure if upgrade renames a config key.

2. **Story sequence risk: if 5.4.1 expands, the later stories' ACs become stale.** The sprint plan story dependency graph assumes 5.4.2 and 5.4.3 are additive patches. If 5.4.1 reveals structural gaps, the test strategy in 5.4.4 and 5.4.5 will need revision before those stories can proceed as written.

3. **No integration test layer is planned.** The test suite (`test-discover-ops.py`) only covers the Python script layer. No story or AC validates the full skill → script → git-commit chain end-to-end. For the auto-commit exception specifically, integration coverage matters.

---

## Open Questions Surfaced

### OQ-FP1 — Config Key Stability Risk

**Question:** Should Story 5.4.1 add a test case for config key resolution? Or should this be tracked as a dependency note against `lens-dev-new-codebase-upgrade`?

**Response (choose A–E):**
- [ ] A — Accept finding and implement exactly as recommended.
- [ ] B — Accept finding and implement an alternative remediation with equivalent control.
- [ ] C — Partially accept finding; narrow scope and document deferred work.
- [ ] D — Dispute finding; provide evidence the risk is already controlled.
- [ ] E — Defer/no action for this phase; record rationale and owner.

---

### OQ-FP2 — No-Remote Case Priority

**Question:** Should the no-remote edge case be resolved in Story 5.4.1 (SKILL.md spec) or in a new Story 5.4.0 that runs before all others?

**Response (choose A–E):**
- [ ] A — Accept finding and implement exactly as recommended.
- [ ] B — Accept finding and implement an alternative remediation with equivalent control.
- [ ] C — Partially accept finding; narrow scope and document deferred work.
- [ ] D — Dispute finding; provide evidence the risk is already controlled.
- [ ] E — Defer/no action for this phase; record rationale and owner.

---

### OQ-FP3 — Integration Test Layer

**Question:** Is a skill-level integration test (covering the full agent → script → git chain) required for `dev-complete`, or is the T-series script-level test coverage sufficient?

**Response (choose A–E):**
- [ ] A — Accept finding and implement exactly as recommended.
- [ ] B — Accept finding and implement an alternative remediation with equivalent control.
- [ ] C — Partially accept finding; narrow scope and document deferred work.
- [ ] D — Dispute finding; provide evidence the risk is already controlled.
- [ ] E — Defer/no action for this phase; record rationale and owner.

---

## Verdict

**pass-with-warnings**

All expressplan findings are documented and carried forward as pre-work items. No new critical findings. Proceed to Step 2.
