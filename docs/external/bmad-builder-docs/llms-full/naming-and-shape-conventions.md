# Naming and Shape Conventions

When you do expose a scalar, name it like a contract.

| Pattern | Use for | Example |
| --- | --- | --- |
| `<purpose>_template` | File paths for templates the skill loads | `brief_template = "resources/brief.md"` |
| `<purpose>_output_path` | Writable destinations | `report_output_path = "{project-root}/docs/reports"` |
| `on_<event>` | Hook scalars (prompts or commands) | `on_complete = ""` |

A scalar named `brief_template` tells the user what changes if they override it. A scalar named `style_config` or `format_options_file` doesn't.

For arrays of tables (menus, capability rosters), give every item a `code` or `id` field. The resolver uses that key to merge by code: matching entries replace in place, new entries append. Mixing `code` on some items and `id` on others falls back to append-only, which is rarely what authors want and almost never what users expect.

There's no removal mechanism. If you need users to suppress a default menu item, have them override it by `code` with a no-op description or prompt. If the natural override flow requires deleting defaults, your surface is probably wrong, and you should reconsider what belongs in the skill body.
