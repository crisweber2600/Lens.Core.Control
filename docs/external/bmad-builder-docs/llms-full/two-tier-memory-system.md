# Two-Tier Memory System

## Session Logs

Raw, append-only notes written after each session to `sessions/YYYY-MM-DD.md`. Format: what happened, key outcomes, observations, follow-up items. Session logs are never loaded on rebirth. They exist as material for curation.

## Curated Memory

MEMORY.md holds distilled, high-value knowledge extracted from session logs. It loads on every rebirth and stays under 200 lines. The curation process (manual during session close, automated during PULSE) reviews session logs, extracts what's worth keeping, and prunes logs older than 14 days once their value has been captured.

| Layer            | When Written       | Loaded on Rebirth | Lifespan        | Purpose                     |
| ---------------- | ------------------ | ------------------ | --------------- | --------------------------- |
| **Session logs** | End of each session| No                 | ~14 days        | Raw material for curation   |
| **MEMORY.md**    | During curation    | Yes                | Permanent       | Distilled long-term knowledge |

## Session Close Discipline

At the end of every session, the agent:

1. Appends a session log to `sessions/YYYY-MM-DD.md`
2. Updates sanctum files with anything learned during the session
3. Notes what's worth curating into MEMORY.md
