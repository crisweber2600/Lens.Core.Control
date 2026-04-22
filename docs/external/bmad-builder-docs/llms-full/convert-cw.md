# Convert (CW)

One-command conversion of any existing skill into a BMad-compliant, outcome-driven equivalent. Takes a non-conformant skill (bloated, poorly structured, or just not following BMad practices) and produces a clean version. Unlike the Build Process's edit/rebuild modes, `--convert` always runs headless and produces a visual comparison report.

## Usage

```
--convert <path-or-url> [-H]
```

The `--convert` flag implies headless mode. Accepts a local skill path or a URL (not limited to remote; local file paths work too).

## Process

| Step | What Happens |
| ---- | ------------ |
| **1. Capture** | Fetch or read the original skill, save a copy for comparison |
| **2. Rebuild** | Full headless rebuild from intent: extract what the skill achieves, apply BMad outcome-driven best practices  |
| **3. Report** | Measure both versions, categorize what changed and why, generate an interactive HTML comparison report |

## Comparison Report

The HTML report includes:

| Section | Content |
| ------- | ------- |
| **Hero banner** | Overall token reduction percentage |
| **Metrics table** | Lines, words, characters, sections, files, estimated tokens, with visual bars |
| **What changed** | Categorized differences (bloat removal, structural reorganization, best-practice alignment) with severity and examples |
| **What survived** | Content that earns its place: instructions the LLM wouldn't follow correctly without being told  |
| **Verdict** | One-sentence summary of the conversion |

Reports are saved to `{bmad_builder_reports}/convert-{skill-name}/`.

## When to Use Convert vs Build Process

| Scenario | Use |
| -------- | --- |
| You have any non-BMad-compliant skill and want it converted fast | `--convert` |
| You have a bloated skill and want a lean replacement with a comparison report | `--convert` |
| You want to interactively discuss what to change | Build Process (Edit mode) |
| You want to rethink a skill from scratch with full discovery | Build Process (Rebuild mode) |
| You want a detailed quality analysis without rebuilding | Quality Optimize |
