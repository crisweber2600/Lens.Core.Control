# Steps

## 1. Answer "Yes" to the Opt-In Question

During the build, both builders ask a version of:

> Should this skill support end-user customization (activation hooks, swappable templates, output paths)? If no, it ships fixed. Users who need changes fork it.

Answer **yes** when you want overrides supported. The builder records this as `{customizable} = yes` and routes to the Configurability Discovery phase.

If you're running headless (`--headless` or `-H`), pass `--customizable` to opt in. The headless default is **no**.

## 2. Walk Through Configurability Discovery

The builder proposes candidates auto-detected from your skill design and asks which should be exposed. Typical candidates:

- **Templates** the skill loads (strongest case)
- **Output destination paths** if the skill writes artifacts
- **`on_<event>` hooks** (prompts or commands executed at lifecycle points)
- **Additional persistent facts** beyond the default `project-context.md` glob

For each candidate you accept, the builder asks for a name and a default value.

## 3. Name Your Scalars Well

Use the suffix conventions below so a user can tell what a scalar does from its name alone.

| Pattern | Use for | Example |
| --- | --- | --- |
| `<purpose>_template` | File paths for templates the skill loads | `brief_template = "resources/brief.md"` |
| `<purpose>_output_path` | Writable destinations | `report_output_path = "{project-root}/docs/reports"` |
| `on_<event>` | Hook scalars | `on_complete = ""` |

Specific names like `brief_template` tell the user exactly what the knob does. Vague names like `style_config` or `format_options` force the user to read your SKILL.md to figure it out.

## 4. Set Good Defaults

Every scalar you expose needs a default that works on first run. Bare paths resolve from the skill root. Use `{project-root}/...` when the default lives somewhere in the user's project.

```toml
[workflow]
brief_template = "resources/brief-template.md"   # ships inside the skill
on_complete = ""                                  # no default post-hook
persistent_facts = [
  "file:{project-root}/**/project-context.md",    # glob into the user's project
]
```

For arrays of tables (menus, capability rosters), give every item a `code` or `id` field so the resolver can merge by key:

```toml
[[agent.menu]]
code = "BR"
description = "Run a brainstorm"
skill = "bmad-brainstorming"
```

Without a `code` or `id` on every item, the array falls back to append-only merging. That's rarely what users actually want.

## 5. Wire `{workflow.X}` or `{agent.X}` References in SKILL.md

The builder does this automatically during emission, but know what's happening: instead of hardcoding `resources/brief-template.md` in your SKILL.md body, the relevant step becomes:

```markdown
Load the brief template from `{workflow.brief_template}`.
```

At runtime, the resolver swaps in whatever the merged scalar is (default, team override, or user override).

## 6. Test an Override

After the skill is built, verify overrides work. In the project where you're testing:

```bash
mkdir -p _bmad/custom
cat > _bmad/custom/{skill-name}.toml <<'EOF'
[workflow]
on_complete = "Print the word CUSTOMIZED to stdout."
EOF
```

Run the resolver directly to confirm your override takes effect:

```bash
python3 _bmad/scripts/resolve_customization.py \
  --skill /path/to/built/skill \
  --key workflow.on_complete
```

Output should be `"Print the word CUSTOMIZED to stdout."`. If you see the default, check that your TOML filename matches the skill directory basename exactly and that the `[workflow]` (or `[agent]`) block header is present.

Then invoke the skill and confirm the customized behavior fires at the expected lifecycle point.
