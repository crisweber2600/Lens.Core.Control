---
feature: nextlens-src-topdownlens
doc_type: diagram
story_id: TL-8
title: TopDownLens Self-Hosting Topology
updated_at: 2026-05-14T04:40:00Z
---

# TopDownLens Self-Hosting Topology

```mermaid
flowchart LR
    operator[Operator / Agent]
    control[nextlens-control\nplanning + feature branches]
    governance[nextlens-governance\nmetadata + published docs on main]
    release[nextlens-release\npublished read-only payload]
    target[nextlens-domain target repos\nimplementation branches]

    operator -->|Lens planning/dev commands| control
    control -->|publish-to-governance| governance
    control -->|lens-git-orchestration prepare/push/PR| target
    control -->|promote-to-release| release
    governance -->|feature.yaml, constitutions, index| control
    target -->|PR evidence + implementation refs| control

    classDef protected fill:#f8f8f8,stroke:#555,stroke-width:1px;
    class governance,release protected;
```

Governance and release are protected publication surfaces. Control branches stage planning and dev artifacts, while target repos carry implementation work. All cross-repo mutation flows through approved orchestration boundaries.