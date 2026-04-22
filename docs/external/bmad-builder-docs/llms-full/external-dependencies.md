# External Dependencies

Some modules depend on tools outside the BMad ecosystem.

| Dependency Type  | Examples                                             |
| ---------------- | ---------------------------------------------------- |
| **CLI tools**    | `docker`, `terraform`, `ffmpeg`                      |
| **MCP servers**  | Custom or third-party Model Context Protocol servers |
| **Web services** | APIs that require credentials or configuration       |

When a module has external dependencies, the setup skill should check for their presence and guide users through installation or configuration.
