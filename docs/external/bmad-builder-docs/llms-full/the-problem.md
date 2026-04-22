# The Problem

Every customization knob you ship is a promise. Users pin values to it, teams commit overrides to git, and future releases have to respect the shape you locked in. Over-exposing makes the skill harder to evolve and invites drift; under-exposing forces forks for changes that should have been a three-line TOML file.

Aim to expose what varies naturally across your users, and nothing else.
