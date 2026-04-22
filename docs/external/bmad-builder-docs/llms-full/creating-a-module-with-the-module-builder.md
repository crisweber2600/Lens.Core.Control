# Creating a Module with the Module Builder

The **Module Builder** (`bmad-module-builder`) automates module creation. It offers three capabilities:

| Capability          | Menu Code | What It Does                                                                            |
| ------------------- | --------- | --------------------------------------------------------------------------------------- |
| **Ideate Module**   | IM        | Brainstorm and plan a module through facilitative discovery; produces a plan document  |
| **Create Module**   | CM        | Package skills as an installable BMad module (setup skill or standalone self-registering)|
| **Validate Module** | VM        | Check that a module's structure is complete, accurate, and properly registered           |

**For a folder of skills (multi-skill module):**

1. Run **Ideate Module (IM)** to brainstorm and plan
2. Build each skill using the **Agent Builder (BA)** or **Workflow Builder (BW)**
3. Run **Create Module (CM)**. It generates a dedicated `-setup` skill with `module.yaml`, `module-help.csv`, and merge scripts
4. Run **Validate Module (VM)** to verify everything is wired correctly

**For a single skill (standalone module):**

1. Build the skill using the **Agent Builder (BA)** or **Workflow Builder (BW)**
2. Run **Create Module (CM)** with the skill path. It embeds self-registration directly into the skill (`assets/module-setup.md`, `assets/module.yaml`, `assets/module-help.csv`) and generates a `marketplace.json` for distribution
3. Run **Validate Module (VM)** to verify

The Module Builder auto-detects single vs. multi-skill input and recommends the appropriate approach.

See **[What Are Modules](/explanation/what-are-modules.md)** for concepts and architecture decisions, or the **[Builder Commands Reference](/reference/builder-commands.md)** for detailed capability documentation.
</document>

<document path="explanation/progressive-disclosure.md">
Progressive disclosure is what separates basic skills from powerful ones. The core idea: never load more context than the agent needs _right now_. This keeps token usage low, prevents context pollution, and lets skills survive long conversations.
