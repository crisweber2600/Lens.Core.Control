# A Worked Example: `bmad-session-prep`

A weekly session-prep workflow for tabletop RPG game masters. It reads the last session's log, reviews open campaign threads, drafts the scene spine, stats NPCs and encounters, and produces a GM notes document to run from.

Here's how to think about its customization surface, field by field.

## `persistent_facts` (default globs the campaign bible)

```toml
persistent_facts = [
  "file:{project-root}/campaigns/**/campaign-bible.md",
  "file:{project-root}/campaigns/**/house-rules.md",
]
```

Every GM runs a different world. Without their campaign bible in context, the workflow is a generic fantasy prep tool that knows nothing about the party's rivals, the kingdom's politics, or last month's cliffhanger. The default glob is shaped so a GM can drop a `campaign-bible.md` in their project and the workflow picks it up. Forcing them to paste world context at the start of every session would burn trust. That's what persistent facts are for.

## `system_rules_template` (scalar, default to D&D 5e)

```toml
system_rules_template = "resources/dnd-5e-quick-reference.md"
```

D&D 5e, Pathfinder 2e, and Call of Cthulhu reason about encounters in very different ways. A PF2e GM who overrides this with their own rules reference gets correctly-calibrated encounter math without the workflow pretending to know a system it doesn't. The skill isn't trying to catalog every RPG; it ships one default that covers most users and lets everyone else swap in their own reference. The `*_template` suffix signals what changes if the user touches it.

## `session_notes_template` (scalar)

```toml
session_notes_template = "resources/session-notes-minimalist.md"
```

GM prep style is personal. Some GMs want theater-of-mind bullets; others want scene blocks with initiative trackers pre-filled and read-aloud boxes for boxed text. No single shipping default wins against that variance. The structural fact that "prep produces notes" is universal, though, so the override changes the shape of the notes file, not the stage sequence.

## `on_complete` (scalar, default empty)

```toml
on_complete = ""
```

The core skill ends when notes are drafted. Some GMs want the workflow to draft a Discord teaser for the group chat, others want encounter stat blocks pushed to Roll20, others want a pre-game meditation prompt. These are real patterns, but they're downstream of the skill's job, not part of it. An empty default means the skill doesn't presume. Override example:

```toml
on_complete = "Draft a 2-sentence Discord teaser ending on a cliffhanger. Save to {project-root}/teasers/next-session.md"
```

## `activation_steps_prepend` (pre-flight context load)

Before the workflow asks the GM anything, some tables want the most recent session log already loaded and summarized:

```toml
activation_steps_prepend = [
  "Scan {project-root}/session-logs/ and load the most recent log. Extract unresolved threads before asking the GM anything."
]
```

Not every GM keeps session logs. The ones who do want the pre-load; the ones who don't would get a broken activation if it were baked in. Opt-in via the prepend hook lets both tables use the same skill.

## What Not to Expose

The stage sequence (recap, threads, spine, NPCs, notes) is the skill's identity. A GM who wants a very different flow (solo journaling, West Marches gossip round) should fork. Every stage made optional erodes what the skill is.

Mechanical encounter math toggles like `auto_balance_cr` or `verbose_stat_blocks` stay out. The LLM handles those naturally once it has the system reference. Toggles here would amount to telling the executor how to do its job.

Per-stage question order stays out too. Too fiddly. If it matters enough to customize, you're describing a different skill.
