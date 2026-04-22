# How Authoring Customization Fits

BMad has a three-layer override model from the user's side:

```text
Priority 1 (wins): _bmad/custom/{skill-name}.user.toml  (personal, gitignored)
Priority 2:        _bmad/custom/{skill-name}.toml        (team/org, committed)
Priority 3 (last): skill's own customize.toml            (your defaults)
```

As an author you own Priority 3. You ship `customize.toml` next to `SKILL.md`. Every field you put there is a commitment to your users: this is what I support overriding. The resolver merges layers structurally (scalars win, arrays of tables keyed by `code` or `id` replace-by-key, other arrays append), so you don't write merge logic. You write defaults and trust the shape.
