# The module.yaml File

Each module declares its identity and configurable variables in an `assets/module.yaml` file. For multi-skill modules, this lives inside the setup skill. For standalone modules, it lives in the skill's own `assets/` folder. This file drives both the prompts shown to the user and the values written to config.

```yaml
code: mymod
name: 'My Module'
description: 'What this module does'
module_version: 1.0.0
default_selected: false
module_greeting: >
  Welcome message shown after setup completes.

my_output_folder:
  prompt: 'Where should output be saved?'
  default: '{project-root}/_bmad-output/my-module'
  result: '{project-root}/{value}'
```

Variables with a `prompt` field are presented to the user during setup. The `default` value is used when the user accepts defaults. Adding `user_setting: true` to a variable routes it to `config.user.yaml` instead of the shared config.

:::caution[Literal Token]
`{project-root}` is a literal token in config values. Never substitute it with an actual path. It signals to the consuming tool that the value is relative to the project root.
:::
