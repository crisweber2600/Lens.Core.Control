---
description: "Use when implementing changes, editing code, modifying files, writing features, or making any development updates in this workspace. Enforces the control-repo pattern: all changes go to TargetProjects/lens/src/Lens.Core.Src, never lens.core. Covers commit, push, GitHub Actions monitoring, git pull lens.core, and preflight checks."
---

# Lens.Core.Control — Change Management Rules

## This Is a Control Repo

`lens.core/` is the **read-only distribution payload** pulled from the release pipeline.
**Never edit files inside `lens.core/` directly.**

All implementation work — new features, fixes, configuration changes — happens exclusively in:

```
TargetProjects/lens/src/Lens.Core.Src/
```

## Change Workflow

Follow this sequence for every change, no exceptions:

### 1. Implement in Lens.Core.Src

Make all edits inside `TargetProjects/lens/src/Lens.Core.Src/`.

### 2. Commit and Push

```bash
cd TargetProjects/lens/src/Lens.Core.Src
git add .
git commit -m "<conventional commit message>"
git push
```

### 3. Monitor the GitHub Actions Job

The push triggers the `promote-to-release.yml` workflow. Wait for it to complete:

```bash
gh run list --workflow=promote-to-release.yml --limit=1 --json databaseId,status,conclusion
```

Poll until `status` is `completed` and `conclusion` is `success`.
If `conclusion` is `failure`, fix the issue in `Lens.Core.Src`, push again, and restart from step 2.

### 4. Pull into lens.core

Once the workflow succeeds:

```bash
cd /path/to/workspace-root
git -C lens.core pull
```

### 5. Run Preflight Checks

After pulling, validate the workspace state:

```bash
./lens.core/_bmad/lens-work/scripts/preflight.sh
```

## Hard Rules

- **NEVER** edit files in `lens.core/` — it is a pulled artifact, not a source
- **NEVER** skip the GitHub Actions monitoring step before pulling `lens.core`
- **NEVER** run preflight before confirming the workflow `conclusion` is `success`
- **NEVER** propose edits to `lens.core` as the resolution — always trace back to `Lens.Core.Src`
