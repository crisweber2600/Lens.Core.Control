# Step 3: Verify the Manifest

Before publishing, confirm the manifest is accurate.

## Check skill paths

Every path in the `skills` array must point to a directory containing a `SKILL.md` file.

## Check module registration files

Multi-skill modules need `assets/module.yaml` and `assets/module-help.csv` in the setup skill. Standalone modules keep these files in the skill's own `assets/` folder.

## Run Validate Module

```
"Validate my module at ./skills"
```

Validate Module (VM) checks for missing files, orphan entries, and other structural problems. Fix anything it flags before publishing.
