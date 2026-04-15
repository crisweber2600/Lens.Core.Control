# Adversarial Review: hermes-lens-plugin / businessplan

**Reviewed:** 2026-04-14T02:30:00Z
**Source:** phase-complete
**Overall Rating:** pass-with-warnings

## Summary

The PRD is complete, coherent, and actionable. All three M-class items from the preplan review (M1 session-context contract, M2 prompt metadata schema, M3 risk taxonomy and preview payload) are fully resolved. The 40 FRs and 16 NFRs are internally consistent and properly grounded in the Hermes plugin API contracts verified through four rounds of upstream documentation research. Two high-severity findings require resolution in TechPlan: the preview-enforcement hook's concurrent-batch detection mechanism is undefined, and the sequential constraint in FR38 is over-scoped to read-only actions where no user confirmation is needed. Four medium findings cover a tool with no backing FR, an unspecified catalog file filter, a missing CLI confirmation flow, and unspecified session-resume semantics. UX design was intentionally excluded from this businessplan run per explicit user scope decision; this is an accepted lifecycle gap, not a logic flaw. TechPlan can proceed on the current PRD foundation.

---

## Findings

### Critical

None.

---

### High

| # | Dimension | Finding | Recommendation |
|---|-----------|---------|----------------|
| H1 | Logic Flaws | FR38 prevents `lens_run_action` and `lens_preview_action` from appearing in the same concurrent Hermes tool batch, but the mechanism for detecting "same batch" is undefined. Hermes fires `pre_tool_call` concurrently via `ThreadPoolExecutor`; without a reliable batch-detection signal the hook cannot enforce this reliably. | TechPlan must define the batch-detection implementation contract — e.g., track whether a preview flag was set in `LensSessionContext` before the current `pre_tool_call` fires, using `task_id` scoping or an atomic flag. |
| H2 | Logic Flaws | FR38 scopes the preview-before-run sequential constraint to all risk classes, but `read` actions (FR26) require no user confirmation. Preventing concurrent `lens_preview_action` + `lens_run_action` dispatch for read actions adds friction with no safety benefit. | Limit the FR38 sequential constraint to `mutating` and `elevated` risk classes only. Read actions may have preview and run dispatched in the same concurrent batch. |

---

### Medium / Low

| # | Dimension | Severity | Finding | Recommendation |
|---|-----------|----------|---------|----------------|
| M1 | Coverage Gaps | Medium | `lens_clarify_input` is declared in the tool surface table but has no backing FR. FR19–22 cover input inference and collection in aggregate but do not specify the named tool's behavior, return contract, or failure modes. | Add an FR for `lens_clarify_input` that covers: invocation conditions, parameter-name and prompt-text args, return JSON contract, and behavior when the user provides no value. |
| M2 | Coverage Gaps | Medium | The catalog file filter for `.github/prompts/` is unspecified. Repos may contain READMEs, config stubs, and subdirectory indexes alongside prompt files. The PRD does not define which file types or naming patterns qualify for catalog inclusion; without this the catalog completeness guarantee in FR16 is unverifiable. | Specify the file inclusion policy: e.g., include all `.md` files, exclude files matching `README*` or `_*`, treat subdirectory structure as intent grouping. Document the policy in the Domain Constraints or Scope section. |
| M3 | Coverage Gaps | Medium | The CLI confirmation flow for `hermes lens run <action>` on mutating and elevated actions is not specified. FR34 routes through "preview-then-confirm-then-execute" but no FR defines what confirmation looks like in CLI mode (interactive prompt, `--yes` / `--confirm` flag, or an explicit second invocation). The tool surface has `lens_preview_action` and `lens_run_action` but CLI runs outside the chat tool loop. | Add an FR or a clause in FR34 defining CLI confirmation behavior and any flags that bypass it. |
| M4 | Assumptions and Blind Spots | Medium | `on_session_start` behavior under `hermes --resume` is unspecified. If a user resumes an existing session, it is unclear whether `on_session_start` fires and, if so, whether it correctly initializes or incorrectly resets an active `LensSessionContext`. FR36 is silent on resume semantics. | Document resume behavior — either `on_session_start` does not fire on resume (Hermes contract), and therefore `LensSessionContext` carries forward its prior state, or it does fire and intentionally resets the context. Either is acceptable; the choice must be explicit. |
| M5 | Coverage Gaps | Medium | UX design artifact intentionally excluded. `lifecycle.yaml` lists `ux-design.md` as a required `businessplan` reviewed artifact. User confirmed scope as `prd` only in `businessplan-batch-input.md`. This is an accepted scope gap, not a logic flaw. | No action needed for v1 PRD. If UX design is deferred to a future businessplan update, document that deferral in `feature.yaml` open questions or a post-businessplan amendment. |
| M6 | Complexity and Risk | Medium | `execution_plan` generation (FR24) is harder than the field description implies. "Human-readable description of what will run" derived from prompt file body requires non-trivial parsing or summarization — it is not a format string. | TechPlan should treat `execution_plan` generation as a mini-summarization task with a defined extraction strategy (e.g., first N lines of the prompt body, a YAML frontmatter `description` field if present, or a Hermes auxiliary LLM call). Define the fallback when no summary is inferrable. |
| L1 | Coverage Gaps | Low | Parameter inference from "prompt body patterns" (FR19) is undefined. The PRD does not specify what textual patterns qualify as parameter signals (e.g., `{param_name}`, `$PARAM`, comment annotations). | Specify the pattern grammar in TechPlan. At PRD level this is acceptable but must not remain undefined by TechPlan start. |
| L2 | Assumptions and Blind Spots | Low | No minimum Hermes version is declared. The plugin targets `hermes_agent.plugins` API contracts that may evolve. | TechPlan should declare the minimum compatible Hermes version and pin it in `plugin.yaml` or a compatibility header. |

---

## Accepted Risks

- **UX design exclusion:** UX design was explicitly scoped out of this businessplan run by user decision. The lifecycle requires this artifact but the scope constraint is intentional and documented in `businessplan-batch-input.md`. TechPlan proceeds without a UX design artifact.
- **FR38 over-scoping (H2):** The current PRD text applies the preview-before-run sequential constraint to all risk classes. This is accepted as a conservative default that TechPlan will narrow. It does not introduce safety regressions; it only adds unnecessary friction for read actions.
- **`execution_plan` generation complexity (M6):** The generation approach is deferred to TechPlan. The PRD's field definition is sufficient for planning; implementation complexity is acknowledged and accepted as a TechPlan design decision.

---

## Party-Mode Challenge

**Winston (Architect):** FR38 says reject if preview and run are in the same concurrent batch. But `pre_tool_call` runs inside the ThreadPoolExecutor concurrently — hooks for both tools fire at roughly the same time. You cannot reliably check "did preview run first" if both hooks are in flight simultaneously. Your TechPlan either needs an atomic flag set before the concurrent dispatch begins, or you need a different enforcement point. The current design is a race condition.

**Quinn (QA Engineer):** The catalog completeness guarantee in FR16 says every prompt file appears in the catalog, but without a file inclusion policy you cannot write a test that validates this. "All files" without filtering means test fixtures will include README.md and accidentally-committed debug files in the catalog. The policy needs to exist before QA can write a passing acceptance test.

**Mary (Analyst):** The journeys and success criteria talk about first-run users being able to discover and execute actions in under three minutes. But the CLI confirmation flow is unspecified. A first-run user hits a mutating action, the preview shows up, and then what? If the CLI confirmation experience is clunky — "type 'yes' exactly" or an obscure flag — the three-minute target is at risk. You need at least a one-sentence description of what confirmation looks like at the CLI before TechPlan starts.

---

## Gaps You May Not Have Considered

1. **Concurrent preview-gate enforcement**: Do you have a concrete implementation in mind for detecting that `lens_run_action` is being invoked in the same batch as `lens_preview_action`? The current language "must not appear in the same concurrent dispatch batch" implies the hook can detect this — but the mechanism is not defined.
2. **Hermes session resume semantics**: When a user runs `hermes --resume`, does `on_session_start` fire? If not, does the restored session carry the previous active-project? The PRD says context does not persist across sessions, but resume is not a new session — it is a continuation. How should the plugin behave?
3. **Read action concurrent dispatch**: Now that FR38 blocks concurrent preview+run across all risk classes, read-only preview workflows require two sequential LLM turns even when the user never needs to confirm. Is this the intended UX for read actions?
4. **CLI confirmation UX**: When `hermes lens run <action>` reaches the confirmation gate for a mutating action, what does the user actually see and type? A `y/N` prompt? A typed-out action name? A `--confirm` flag that must be re-added? This is not specified anywhere in the current PRD.
5. **Prompt file format assumption**: The PRD assumes `.github/prompts/` contains Hermes-compatible prompt files. Are there Lens repos where prompts use a different format (e.g., raw text, JSON, or `.prompt` extension)? Does the catalog builder fail gracefully on these or silently skip them?

---

## Open Questions Surfaced

- Should FR38's sequential constraint be narrowed to `mutating` and `elevated` risk classes only, allowing concurrent `lens_preview_action` + `lens_run_action` for `read` actions? (Recommend: yes — see H2)
- What is the batch-detection mechanism for the FR38 `pre_tool_call` enforcement? (Must be resolved in TechPlan)
- What file types and naming patterns in `.github/prompts/` are included in the catalog? (Must be specified before TechPlan)
- Should UX design be deferred to a future businessplan amendment or skipped entirely for v1?
- What is the `execution_plan` extraction strategy for prompt files with no frontmatter description?
- What is the minimum Hermes version this plugin targets?
