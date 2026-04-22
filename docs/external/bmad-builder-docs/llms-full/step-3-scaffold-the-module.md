# Step 3: Scaffold the Module

Run Create Module (CM) to package your finished skills.

:::note[Example]
**You:** "I want to create a module" or provide the path to your skills folder (or a single skill).

**Builder:** Reads your skills, detects whether this is a multi-skill or single-skill module, confirms the approach, and scaffolds the output.
:::

## Multi-skill modules

The builder generates a dedicated setup skill:

```
your-skills-folder/
├── {code}-setup/                # Generated setup skill
│   ├── SKILL.md                 # Setup instructions
│   ├── scripts/                 # Config merge and cleanup scripts
│   │   ├── merge-config.py
│   │   ├── merge-help-csv.py
│   │   └── cleanup-legacy.py
│   └── assets/
│       ├── module.yaml          # Module identity and config vars
│       └── module-help.csv      # Capability entries
├── your-agent-skill/
├── your-workflow-skill/
└── ...
```

## Standalone modules

The builder embeds registration into the skill itself:

```
your-skill/
├── SKILL.md                     # Updated with registration check
├── assets/
│   ├── module-setup.md          # Self-registration reference
│   ├── module.yaml              # Module identity and config vars
│   └── module-help.csv          # Capability entries
├── scripts/
│   ├── merge-config.py          # Config merge script
│   └── merge-help-csv.py        # Help CSV merge script
└── ...
```

A `.claude-plugin/marketplace.json` is also generated at the parent level for distribution.
