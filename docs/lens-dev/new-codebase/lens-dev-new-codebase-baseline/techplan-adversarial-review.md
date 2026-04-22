---
feature: lens-dev-new-codebase-baseline
doc_type: techplan-adversarial-review
phase: techplan
source: phase-complete
verdict: pass-with-warnings
reviewed_artifacts: [architecture]
predecessor_context: [prd, brainstorm, research, product-brief]
updated_at: 2026-04-23T00:00:00Z
---

# TechPlan Adversarial Review — lens-dev-new-codebase-baseline

**Phase:** techplan
**Source:** phase-complete
**Reviewed artifact:** `architecture.md`
**Predecessor context:** `prd.md`, `product-brief.md`, `research.md`, `brainstorm.md`
**Verdict:** **pass-with-warnings**

---

## Summary

The architecture is structurally complete and coherent. It correctly resolves the PRD's stability mandate into a dependency-tiered rewrite inventory, establishes frozen data contracts, and provides work packages with parity gates for all 17 retained commands. The rewrite sequencing in Section 6 is logically sound, and the shared utility extraction decision (ADR-001) is well-justified.

Two high-risk findings were identified at time of review. Post-review clarifications and architecture corrections have resolved or addressed all high and medium findings. Findings are annotated below with resolution status.

**Findings summary (at review):** 0 critical · 1 high · 3 medium · 2 low  
**Findings status (post-review):** 0 critical · 0 high · 0 medium unresolved · 2 low acknowledged

---

## Findings

### F-01 [HIGH] ~~BMB-first operational specification is missing~~ **[RESOLVED — post-review]**

**Resolution:** BMB is the bmad workflow builder module — it is used to author and validate bmad workflow skills, not a promotion pipeline for `lens.core.src` changes. The architecture §10 and ADR-003 have been corrected to accurately describe the governance constraint: changes to `lens.core.src` go through the Lens workflow process (init-feature → plan → dev → complete). BMB remains available as a tool within that process for authoring skill files. The operational specification gap is closed by this clarification.

**Section:** §10 — Implementation Channel: BMB-First *(now: Lens Workflow Dogfooding)*  
**Finding:** ADR-003 mandates that all modifications to `lens.core.src` are authored via `lens.core/_bmad/bmb`, not applied directly. This is declared as a governance requirement and a service constitution constraint. However, the architecture does not specify how BMB-authored changes are promoted to `lens.core.src`. The BMB module is referenced as the authoring surface but its output format, promotion workflow, and validation steps are undefined in this document.

Any developer or agent executing WP-07 through WP-17 will immediately need answers to:
- What is the BMB authoring format for a SKILL.md rewrite?
- What command or process promotes BMB output into `lens.core.src`?
- How is a BMB-authored change reviewed before promotion?

**Impact:** Without this, implementation cannot start on any work package that touches `lens.core.src`. Authors will either improvise the workflow (creating divergence) or block on it.

**Recommendation:** Add a section to `architecture.md` or produce a companion `bmb-workflow.md` that specifies the three steps: author in BMB → promote to `lens.core.src` → verify via parity gate. Reference the BMB module's own schema docs. This section must be in place before any WP author begins implementation.

---

### F-02 [MEDIUM] ~~BMB compliance cannot be tested from the current validation suite~~ **[ACKNOWLEDGED — partially addressed]**

**Resolution:** With the BMB-first framing corrected (F-01), the compliance concern shifts to the Lens workflow governance model. Tier transition gate = parity tests passing (confirmed in post-review Q&A). Direct-edit violations remain detectable only via code review; no automated enforcement is added in this rewrite scope. Accepted as implementation-phase tracking item.

**Section:** §12 — Validation Strategy  
**Finding:** The validation strategy lists 8 regression anchors covering feature identity, navigation, git-orchestration, upgrade, and preflight. None verify that `lens.core.src` changes were authored through BMB and not applied directly. If someone edits `lens.core.src` directly during implementation, the violation is undetectable from the regression suite.

**Impact:** ADR-003 becomes an honor system. The governance requirement is stated but not enforced. In a team environment or multi-agent execution, this creates compounding risk across the full WP-01 to WP-17 rewrite span.

**Recommendation:** Add a compliance check to the validation strategy. This could be a git-hook, a lint rule, or a diff-based audit that confirms all `lens.core.src` changes after the rewrite begins originated from a BMB-authored commit. Even a lightweight audit script would close this gap.

---

### F-03 [MEDIUM] ~~Constitution WP-15 sequencing leaves a known bug active through the entire planning tier rewrite~~ **[ADDRESSED — architecture updated]**

**Resolution:** Architecture updated. Section 6 now includes an explicit prerequisite constraint: WP-15 must deliver the org-level hard-fail bug fix before any planning conductor work package (WP-07–WP-11) begins. WP-15's §8.3 rewrite scope entry is updated with the same prerequisite note. This is the resequencing the user confirmed in Q4.

**Section:** §5.3, §8.3 — `constitution` command and WP-15  
**Finding:** The architecture correctly identifies that `bmad-lens-constitution`'s org-level hard-fail is a known bug (§5.3 note). WP-15 (constitution rewrite) is placed in implementation tier 5 — after all planning conductors (WP-07 to WP-11). This means the broken constitution resolution behavior will be active during the entire planning tier rewrite. Every planning conductor that calls `bmad-lens-constitution` (preplan, businessplan, techplan, finalizeplan, expressplan) will inherit the bug throughout development.

**Impact:** Medium. The bug only manifests when org-level constitution is absent. If the test environment has a complete constitution hierarchy, this may never trigger. But it is an undeclared risk assumption.

**Recommendation:** Either (a) resequence WP-15 before planning conductor work begins, or (b) explicitly call out in the architecture that planning tier development assumes a complete constitution hierarchy and that the WP-15 fix is only required before production release, not development start.

---

### F-04 [MEDIUM] ~~Retained-vs-removed skill inventory gate condition is undefined~~ **[CONFIRMED — gate cleared]**

**Resolution:** ADR-004 consequence updated: §7 inventory is confirmed as of this adversarial review (2026-04-23). Any post-TechPlan modifications to keep/remove decisions require a FinalizePlan gate item. Gate is now cleared.

**Section:** §9, ADR-004  
**Finding:** ADR-004 states "No skill directory is deleted until the inventory is confirmed" and labels the inventory a "TechPlan/FinalizePlan deliverable." Section 7 provides the inventory matrix, which is substantively complete. However, the architecture does not specify what "confirmed" means operationally: who reviews it, what format the confirmation takes, and when it gates implementation.

**Impact:** Low-medium. Section 7 already provides the inventory, so the risk is not that the inventory is missing — it exists and is reviewed here. The risk is that subsequent WP authors may make skill deletion decisions without a clear reference to when and how the ADR-004 gate is cleared.

**Recommendation:** State explicitly in ADR-004 or §9 that the inventory in §7 is the TechPlan-deliverable artifact and that it is considered confirmed when this adversarial review passes. Add a note that any post-TechPlan modifications to the keep/remove decisions require a FinalizePlan gate item.

---

### F-05 [LOW] ~~`ux-design` frozen filename listed without tech-change track qualification~~ **[ADDRESSED — architecture updated]**

**Resolution:** Architecture updated. A track qualifier note has been added after the Artifact File Conventions table: `ux-design.md` applies to the `full` track only; `tech-change` and `express` tracks do not produce this artifact.

**Section:** Data Model — Artifact File Conventions  
**Finding:** The artifact conventions table lists `ux-design.md` as a frozen filename. The `tech-change` track does not produce a `ux-design` output. A developer reading the frozen conventions table could interpret this as requiring `ux-design.md` for this feature, which would be incorrect.

**Recommendation:** Add a note clarifying that frozen conventions apply per track — `ux-design.md` is frozen for the `full` track; the `tech-change` track does not produce it.

---

### F-06 [LOW] No in-progress feature transition strategy at release time **[ACKNOWLEDGED — hard cut]**

**Resolution:** Confirmed as hard cut. Removed stubs are deleted with no redirect or deprecation shim. Users invoking removed commands after upgrade receive no output. Architecture §9 updated with this behavior note. Users must be informed at release time via upgrade notes. This remains a release-day concern outside the rewrite architecture scope.

**Section:** General  
**Finding:** The architecture specifies frozen schemas and a promotion output model (`lens.core/` as release payload), but does not address what happens to features that are actively in progress using the old 54-stub surface at release cutover. A user with `phase: dev` on a feature started under the old surface would continue working — but any command they invoke after upgrade would go through the new 17-command surface.

**Impact:** Low for this feature's scope — this is likely out of scope and may belong in the `upgrade` work package or the PRD's non-functional requirements. But if neither document owns this, the gap will surface at release time.

**Recommendation:** Confirm this is explicitly owned by the `upgrade` WP-17 scope or by a separate post-rewrite ops document. Add a sentence in §9 or the intro noting that in-progress feature transition is a release-day concern outside the rewrite architecture scope.

---

## Party-Mode Blind-Spot Challenge

Three reviewers were asked to push on gaps the document may have missed.

---

**Winston (Architect):** "The tier ordering in Section 6 is logically correct, but it assumes tiers don't bleed. Identity and navigation (WP-01 to WP-06) stabilize first, then planning conductors begin. But when you're rewriting `bmad-lens-init-feature` and realize it has an undocumented shared dependency with `bmad-lens-finalizeplan`'s bundle generation, how do you catch that before both are mid-rewrite? The architecture has work package parity gates but no cross-tier integration test. What is the signal that tells you tier N is actually stable enough for tier N+1 to start, aside from the unit-level parity gates passing?"

**Amelia (Dev):** "I'm about to start WP-09 (techplan rewrite). I want to follow BMB-first as required. But when I open `lens.core/_bmad/bmb/`, what do I find there and what do I do with it? Do I write a SKILL.md in some BMB format and then there's a script that promotes it? Does the `lens.agent.md` in BMB need to be updated separately from the one in `lens-work/`? The entire rewrite architecture depends on this workflow being clear, and right now it isn't — at least not in this document."

**Mary (Analyst):** "The PRD and architecture both say 100% backwards-compatible behavior for retained commands. I accept that. But 37 stubs are being removed. Users who have `/lessons`, `/pause`, `/theme`, `/audit` in their workflows will hit broken-stub errors after upgrade. Is there a deprecation period? A stub that redirects to an error message? Or is it a hard cut? The PRD doesn't address this and neither does the architecture. The users affected most are the ones who built habits around the removed commands — and that's a real support surface even if the governance model is technically intact."

---

### Blind-Spot Questions — Answered (2026-04-23)

1. **What is the BMB authoring-to-promotion workflow?**  
   **Answer:** BMB is the bmad workflow builder — it is used to build workflows for bmad (author/edit skills and agents within the bmad ecosystem). It is not a promotion pipeline for `lens.core.src` changes. The implementation channel is the Lens workflow process (init-feature → plan → dev → complete). Architecture §10 and ADR-003 corrected accordingly. F-01 resolved.

2. **What gates tier transition?**  
   **Answer:** Parity tests passing. When WP-06 (`next`) parity tests pass, the tier is considered stable and WP-07 (`preplan`) may begin.

3. **How does a user discover that a removed command no longer exists?**  
   **Answer:** Hard cut — nothing. Users who invoke a removed command after upgrade receive no output; the stub does not exist. No redirect, no deprecation shim. Release-time upgrade notes carry this information. Architecture §9 updated with this behavior note.

4. **Is WP-15 (constitution bug fix) required before any planning conductor is tested?**  
   **Answer:** Yes — fix it. WP-15 must deliver the org-level hard-fail bug fix before WP-07 begins. Architecture Section 6 and §8.3 WP-15 entry updated with this prerequisite constraint.

5. **Who signs off on the §7 skill retention matrix?**  
   **Answer:** Confirmed. The §7 matrix is the signed-off inventory as of this adversarial review. ADR-004 consequence updated to reflect this. Any post-TechPlan keep/remove changes require a FinalizePlan gate item.

---

## Verdict

**pass-with-warnings**

The architecture correctly addresses the PRD's structural mandate: 17-command surface, shared utility extraction, dependency-tiered rewrite, frozen schema contracts, BMB governance channel, and work packages with parity gates. No critical blockers were found.

All findings are resolved or acknowledged. F-01 (BMB) is resolved by clarification and architecture correction. F-02 is acknowledged (tier gate = parity tests; direct-edit compliance remains code-review enforced). F-03 is addressed in architecture (WP-15 prerequisite constraint added). F-04 is confirmed (§7 inventory cleared as of this review). F-05 is addressed in architecture (ux-design track qualifier added). F-06 is acknowledged (hard cut, no redirect, upgrade notes required). The skill retention matrix in §7 is confirmed as of this review.

**Phase transition:** `techplan` → `techplan-complete` may proceed.
