# Common Questions

## Do I need to ideate before creating?

No. If you already know what your module should contain, skip straight to Create Module (CM). Ideation helps when you're still shaping the concept.

## Can I add skills to a module later?

Yes. Build the new skill and re-run Create Module (CM) on the folder. The anti-zombie pattern ensures the existing setup skill is replaced cleanly.

## What if my module only has one skill?

The Module Builder handles this automatically. Give it a single skill and it recommends the **standalone self-registering** approach, where registration embeds directly in the skill and triggers on first run or when the user passes `setup`/`configure`.

## Can my module extend another module?

Yes. Tell the builder during ideation or creation that your module is an expansion. Your help CSV entries can reference the parent module's capabilities in their before/after ordering fields.
