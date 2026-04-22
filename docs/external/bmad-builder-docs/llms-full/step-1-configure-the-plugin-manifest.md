# Step 1: Configure the Plugin Manifest

Modules are discovered through a `.claude-plugin/marketplace.json` manifest at the repository root. Create Module generates this file for you. Verify and complete it before publishing.

:::tip[Installer Support]
The BMad Method installer (`npx bmad-method install`) supports installing custom modules from any Git host or local path. Users can install interactively or via `--custom-source <url-or-path>`. See the [BMad Method install guide](https://docs.bmad-method.org/how-to/install-custom-modules/) for details.
:::

This format works for any skills-capable platform, not just Claude, we just utilize the claude file as a convention to support any skills based platform.

A minimal manifest for a single module:

```json
{
  "name": "my-module",
  "owner": { "name": "Your Name" },
  "license": "MIT",
  "homepage": "https://github.com/your-github/my-module",
  "repository": "https://github.com/your-github/my-module",
  "keywords": ["bmad", "your-domain"],
  "plugins": [
    {
      "name": "my-module",
      "source": "./",
      "description": "What your module does in one sentence.",
      "version": "1.0.0",
      "author": { "name": "Your Name" },
      "skills": [
        "./skills/my-agent",
        "./skills/my-workflow"
      ]
    }
  ]
}
```

| Field | Purpose |
| ----- | ------- |
| **name** | Package identifier, lowercase and hyphenated |
| **plugins[].source** | Path from repo root to the module's skill folder parent |
| **plugins[].skills** | Array of relative paths to each skill directory |
| **plugins[].version** | Semantic version; bump on each release |

For repositories that ship multiple modules, add an entry to the `plugins` array for each one, pointing to its own skill directories.
