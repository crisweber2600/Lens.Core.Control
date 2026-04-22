# Distribution: Plugins and Marketplaces

At the distribution level, a BMad module is a **plugin**: a package of skills with a `.claude-plugin/` manifest. How you structure it depends on what you're shipping:

| Structure           | When to Use                                                  | Manifest                                                  |
| ------------------- | ------------------------------------------------------------ | --------------------------------------------------------- |
| **Single plugin**   | One module (standalone or multi-skill)                       | `.claude-plugin/marketplace.json` with one plugin entry   |
| **Marketplace**     | A repo that ships multiple modules                           | `.claude-plugin/marketplace.json` with multiple plugin entries |

The `.claude-plugin/` convention originates from Claude Code, but the format works across multiple skills platforms. The BMad installer supports installing custom modules from any Git host (GitHub, GitLab, Bitbucket, self-hosted) or local file paths. See the [BMad Method install guide](https://docs.bmad-method.org/how-to/install-custom-modules/) for details.

The Module Builder generates the appropriate `marketplace.json` during the Create Module (CM) step - but you will want to verify it lists the proper relative paths to the skills you want to deliver with your module.

This also means you can include remote URL skills in your own module to combine them.
