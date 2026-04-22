# Anti-Zombie Pattern

Both merge scripts use an anti-zombie pattern: before writing new values for a module, they remove all existing entries for that module's code. This prevents stale configuration or help entries from persisting across module updates. Running setup a second time is always safe.
