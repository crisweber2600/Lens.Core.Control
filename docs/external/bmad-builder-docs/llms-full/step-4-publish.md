# Step 4: Publish

Push your repository to a Git host (GitHub, GitLab, Bitbucket, or self-hosted). Once the repo is accessible, anyone with permission can install it.

## Installing your module

Users install custom modules through the BMad installer:

```bash
# Interactive: the installer prompts for a custom source URL or path
npx bmad-method install

# Non-interactive: specify the source directly
npx bmad-method install --custom-source https://github.com/your-org/my-module --tools claude-code --yes
```

The installer accepts HTTPS URLs, SSH URLs, URLs with deep paths (e.g., `/tree/main/subdir`), and local file paths.

## Private or organization modules

For private repos, users need Git access to clone. The installer uses whatever Git authentication is configured on the machine.

## Versioning

Tag releases with semantic versions. Installs pull from the default branch unless the user specifies a tag or branch.
