# Configuration and Registration

Modules register with a project through three files in `{project-root}/_bmad/`:

| File               | Purpose                                                                |
| ------------------ | ---------------------------------------------------------------------- |
| `config.yaml`      | Shared settings committed to git, module section keyed by module code |
| `config.user.yaml` | Personal settings (gitignored), user name, language preferences       |
| `module-help.csv`  | Capability registry, one row per action users can discover            |

Registration is what makes a module visible to `bmad-help`. Without it, the help system cannot discover, recommend, or track completion of the module's capabilities.

Not every module needs configuration. If skills work with sensible defaults, registration can focus purely on help entries. See **[Module Configuration](/explanation/module-configuration.md)** for details on when configuration adds value and how the help CSV columns work.
