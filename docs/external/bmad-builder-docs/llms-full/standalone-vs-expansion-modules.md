# Standalone vs. Expansion Modules

| Type           | Description                                                                                                               |
| -------------- | ------------------------------------------------------------------------------------------------------------------------- |
| **Standalone** | Provides complete, independent value. Does not depend on another module being installed                                   |
| **Expansion**  | Extends an existing module with new capabilities. Should still provide utility even if the parent module is not installed |

Expansion modules can reference the parent module's capabilities in their help CSV ordering (before/after fields). This lets a new capability slot into the parent module's natural workflow sequence.

Even expansion modules should be designed to work independently. The parent module being absent should degrade gracefully, not break the expansion.
