# PULSE: Autonomous Wake

Autonomous agents include a PULSE.md file that defines behavior when the agent wakes without a human present (via `--headless` flag, cron job, or orchestrator).

## Default PULSE Behavior

Memory curation is always the first priority on autonomous wake:

1. Review recent session logs in `sessions/`
2. Extract insights worth keeping into MEMORY.md
3. Prune session logs older than 14 days
4. Update BOND.md and INDEX.md with anything new

## Domain Tasks

After curation, the agent can perform domain-specific autonomous work:

| Domain          | Example PULSE Tasks                                                   |
| --------------- | --------------------------------------------------------------------- |
| Creative muse   | Incubate ideas from recent sessions, generate creative sparks         |
| Research agent  | Track topics of interest, surface new findings                        |
| Project monitor | Check project health, flag risks, update status                       |
| Content curator | Review saved sources, organize and summarize                          |

PULSE also defines named task routing (`--headless {task-name}`), frequency preferences, and quiet hours.
