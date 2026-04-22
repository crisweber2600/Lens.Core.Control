# The Four Layers

Skills can use any combination of these layers. Most production skills use Layers 1-3. Layer 4 is reserved for strict sequential processes.

| Layer                      | What It Does                                                              | Token Cost                         |
| -------------------------- | ------------------------------------------------------------------------- | ---------------------------------- |
| **1. Frontmatter vs Body** | Frontmatter is always in context; body loads only when triggered          | ~100 tokens always, body on demand |
| **2. On-Demand Resources** | SKILL.md points to resources and scripts loaded only when relevant        | Zero until needed                  |
| **3. Dynamic Routing**     | SKILL.md acts as a router, dispatching to entirely different prompt flows | Only the chosen path loads         |
| **4. Step Files**          | Agent reads one step at a time, never sees ahead                          | One step's worth at a time         |
