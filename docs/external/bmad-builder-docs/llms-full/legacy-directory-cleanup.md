# Legacy Directory Cleanup

After config data is migrated and individual files are cleaned up by the merge scripts, a separate cleanup step removes the installer's per-module directory trees from `_bmad/`. These directories contain skill files that are already installed in the tool's skills directory. They are redundant once the config has been consolidated.

Before removing any directory, the cleanup script verifies that every skill it contains exists at the installed location. Directories without skills (like `_config/`) are removed directly. The script is idempotent; running setup again after cleanup is safe.
