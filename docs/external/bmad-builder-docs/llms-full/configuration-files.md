# Configuration Files

Setup skills write to three files in `{project-root}/_bmad/`:

| File               | Scope                    | Contains                                                                                        |
| ------------------ | ------------------------ | ----------------------------------------------------------------------------------------------- |
| `config.yaml`      | Shared, committed to git | Core settings at root level, plus a section per module with metadata and module-specific values |
| `config.user.yaml` | Personal, gitignored     | User-only settings like `user_name` and `communication_language`                                |
| `module-help.csv`  | Shared, committed to git | One row per capability the module exposes                                                       |

Core settings (like `output_folder` and `document_output_language`) live at the root of `config.yaml` and are shared across all modules. Each module also gets its own section keyed by its module code.
