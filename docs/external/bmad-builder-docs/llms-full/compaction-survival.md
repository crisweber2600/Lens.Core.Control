# Compaction Survival

Long-running workflows risk losing context when the conversation compresses. The **document-as-cache pattern** solves this: the output document itself stores the workflow's state.

| Component             | Purpose                                                |
| --------------------- | ------------------------------------------------------ |
| **YAML front matter** | Paths to input files, current stage status, timestamps |
| **Draft sections**    | Progressive content built across stages                |
| **Status marker**     | Which stage is complete, for resumption                |

Each stage reads the output document to restore context, does its work, and writes results back to the same document. If context compacts mid-workflow, the next stage recovers by reading the document and reloading the input files listed in front matter.

```markdown
---
title: 'Analysis: Research Topic'
status: 'analysis'
inputs:
  - '{project_root}/docs/brief.md'
  - '{project_root}/data/sources.json'
---
```

This avoids separate cache files, file collisions when running multiple workflows, and state synchronization complexity.
