# Customization Surface

Workflow customization is fully opt-in. If you don't need users to override anything, don't ship a `customize.toml` at all; the workflow runs with hardcoded paths and defaults. If you do opt in, the builder walks you through Configurability Discovery, where you name the scalars (templates, output paths, hooks) you want to expose. Users override them through the three-layer model: your shipped defaults at `{skill-root}/customize.toml`, team overrides at `_bmad/custom/{skill-name}.toml`, and personal overrides at `_bmad/custom/{skill-name}.user.toml`.

See [Customization for Authors](/explanation/customization-for-authors.md) for the decision guide and [How to Make a Skill Customizable](/how-to/make-a-skill-customizable.md) for the build-time steps.
