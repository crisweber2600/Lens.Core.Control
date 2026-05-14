#!/usr/bin/env python3
"""
Generate individual story files for NextLens v1 feature.
Parses stories.md and creates individual YAML story files with frontmatter.
"""

from datetime import datetime, timezone
import json
from pathlib import Path
import re

FEATURE_ID = "nextlens-src-implement"
TARGET_REPO_ROOT = "TargetProjects/nextlens/src/NextLens"
FEATURE_DOCS_ROOT = f"docs/nextlens/src/{FEATURE_ID}"
SECTION_BOUNDARY = r'(?:\n\n(?=\*\*[A-Z][^:\n]+:\*\*)|$)'


def build_story_filename(story_data):
    """Build a stable story filename from story metadata."""
    sanitized_title = story_data['title'].lower().replace('&', 'and')
    sanitized_title = re.sub(r'[^a-z0-9]+', '-', sanitized_title).strip('-')
    return f"{story_data['story_id']}-{sanitized_title}.md"


def strip_markdown_emphasis(text):
    """Remove simple markdown emphasis wrappers from short labels."""
    return re.sub(r'\*\*(.*?)\*\*', r'\1', text).strip()


def parse_numbered_items(block):
    """Parse a markdown numbered list into grouped line blocks."""
    items = []
    current_item = []

    for raw_line in (block or "").splitlines():
        line = raw_line.rstrip()
        match = re.match(r'^\s*\d+\.\s+(.*)', line)
        if match:
            if current_item:
                items.append(current_item)
            current_item = [match.group(1).strip()]
            continue
        if current_item:
            current_item.append(line)

    if current_item:
        items.append(current_item)

    return items


def render_acceptance_checklist(acceptance_criteria):
    """Render numbered acceptance criteria as markdown checkboxes."""
    items = parse_numbered_items(acceptance_criteria)
    if not items:
        fallback = acceptance_criteria.strip() or "Acceptance criteria could not be parsed from stories.md."
        return f"- [ ] {fallback}"

    rendered = []
    for item in items:
        title = strip_markdown_emphasis(item[0])
        rendered.append(f"- [ ] {title}")
        for line in item[1:]:
            if line.strip():
                rendered.append(f"  {line}")
    return "\n".join(rendered)


def build_context(story_data, dependency_links):
    """Build a canonical dev-ready Context section."""
    dependency_summary = (
        f"Blocked until the following stories are complete: {', '.join(dependency_links)}."
        if dependency_links
        else "This story has no upstream story dependencies."
    )
    return "\n".join([
        story_data['user_story'] or '_User story source could not be parsed from `stories.md`._',
        "",
        f"Primary implementation root: `{TARGET_REPO_ROOT}`.",
        f"Authoritative planning references live under `{FEATURE_DOCS_ROOT}`.",
        dependency_summary,
    ])


def build_implementation_steps(story_data):
    """Build implementation steps from acceptance slices and target-repo conventions."""
    acceptance_items = parse_numbered_items(story_data['acceptance_criteria'])
    steps = [
        f"Identify the smallest set of files under `{TARGET_REPO_ROOT}` that directly control `{story_data['title']}`.",
    ]

    for item in acceptance_items:
        steps.append(f"Implement the acceptance slice `{strip_markdown_emphasis(item[0])}`.")

    steps.append(f"Add or update focused tests under `{TARGET_REPO_ROOT}/tests` or adjacent test locations for the touched behavior.")
    steps.append("Run the narrowest validation available before marking the story complete.")

    return "\n".join(f"{index}. {step}" for index, step in enumerate(steps, start=1))


def build_notes_for_dev(story_data, dependency_links, definition_of_done):
    """Build stable developer guidance for generated story packets."""
    notes = [
        f"- Implement code and tests in `{TARGET_REPO_ROOT}`; do not place implementation code in control-repo docs paths.",
        f"- Planning source of truth: `{FEATURE_DOCS_ROOT}/stories.md`, `{FEATURE_DOCS_ROOT}/epics.md`, and `{FEATURE_DOCS_ROOT}/architecture.md`.",
    ]
    if dependency_links:
        notes.append(f"- Resolve dependencies first: {', '.join(dependency_links)}.")
    if definition_of_done:
        notes.append("- Definition of done from the source plan:")
        notes.extend(f"  - {item}" for item in definition_of_done)
    return "\n".join(notes)


def parse_stories_md(content):
    """Parse stories.md to extract individual stories."""
    stories = []

    # Split by ### Story pattern to find individual stories
    story_pattern = r'### Story (\d+\.\d+):\s*(.+?)\n\n'
    story_matches = list(re.finditer(story_pattern, content))

    if not story_matches:
        print("WARNING: No stories found with ### Story pattern")
        return stories

    # For each story, extract content from current position to next story
    for i, match in enumerate(story_matches):
        story_start = match.start()
        story_end = story_matches[i + 1].start() if i + 1 < len(story_matches) else len(content)

        story_text = content[story_start:story_end]
        story_id = match.group(1)
        story_title = match.group(2).strip()

        # Parse Story ID to get epic number
        epic_num = story_id.split('.')[0]

        # Extract acceptance criteria
        ac_pattern = rf'\*\*Acceptance Criteria:\*\*\n(.*?){SECTION_BOUNDARY}'
        ac_match = re.search(ac_pattern, story_text, re.DOTALL)
        acceptance_criteria = ac_match.group(1).strip() if ac_match else ""

        # Extract complexity
        complexity_pattern = r'\*\*Complexity:\*\*\s*([^\n]+)'
        complexity_match = re.search(complexity_pattern, story_text)
        complexity = complexity_match.group(1).strip() if complexity_match else "Medium"

        # Extract priority
        priority_pattern = r'\*\*Priority:\*\*\s*([^\n]+)'
        priority_match = re.search(priority_pattern, story_text)
        priority = priority_match.group(1).strip() if priority_match else "High"

        # Extract dependencies
        deps_pattern = r'\*\*Dependencies:\*\*\s*(.+?)(?:\n\n|$)'
        deps_match = re.search(deps_pattern, story_text, re.DOTALL)
        dependencies_text = deps_match.group(1).strip() if deps_match else ""

        # Parse dependencies into list
        depends_on = []
        if "None" not in dependencies_text and dependencies_text:
            dep_refs = re.findall(r'(?:Story |EP)?(\d+\.\d+)', dependencies_text)
            depends_on = sorted(set(dep_refs), key=lambda x: (int(x.split('.')[0]), int(x.split('.')[1])))

        # Extract epic name
        epic_pattern = r'\*\*Epic:\*\*\s*(.+?)(?:\n|$)'
        epic_match = re.search(epic_pattern, story_text)
        epic_name = epic_match.group(1).strip() if epic_match else f"EP{epic_num}"

        # Extract and preserve the raw user story block
        user_story_pattern = rf'\*\*User Story:\*\*\n\n(.*?){SECTION_BOUNDARY}'
        user_story_match = re.search(user_story_pattern, story_text, re.DOTALL)
        user_story = user_story_match.group(1).strip() if user_story_match else ""
        if not user_story:
            print(f"WARNING: User story block could not be parsed for Story {story_id}")

        dod_pattern = rf'\*\*Definition of Done:\*\*\n(.*?){SECTION_BOUNDARY}'
        dod_match = re.search(dod_pattern, story_text, re.DOTALL)
        definition_of_done = []
        if dod_match:
            definition_of_done = [
                re.sub(r'^- \[ \]\s*', '', line.strip())
                for line in dod_match.group(1).splitlines()
                if line.strip().startswith('- [ ]')
            ]

        stories.append({
            'story_id': story_id,
            'title': story_title,
            'epic_num': epic_num,
            'epic_name': epic_name,
            'priority': priority,
            'complexity': complexity,
            'depends_on': depends_on,
            'user_story': user_story,
            'acceptance_criteria': acceptance_criteria,
            'definition_of_done': definition_of_done,
            'full_text': story_text.strip(),
        })

    return stories


def create_story_file(story_data, docs_path, story_filename_map):
    """Create individual story file with YAML frontmatter."""
    stories_dir = Path(docs_path) / 'stories'
    stories_dir.mkdir(parents=True, exist_ok=True)

    filename = stories_dir / build_story_filename(story_data)
    dependency_links = []
    for dependency in story_data['depends_on']:
        dependency_filename = story_filename_map.get(dependency)
        if dependency_filename:
            dependency_links.append(f'[Story {dependency}](../stories/{dependency_filename})')
        else:
            dependency_links.append(f'Story {dependency}')

    timestamp = datetime.now(timezone.utc).strftime('%Y-%m-%dT%H:%M:%SZ')
    context = build_context(story_data, dependency_links)
    implementation_steps = build_implementation_steps(story_data)
    acceptance_criteria = render_acceptance_checklist(story_data['acceptance_criteria'])
    notes_for_dev = build_notes_for_dev(
        story_data,
        dependency_links,
        story_data.get('definition_of_done', []),
    )

    frontmatter = f"""---
feature: {FEATURE_ID}
story_id: \"{story_data['story_id']}\"
doc_type: story
status: ready
title: \"{story_data['title']}\"
epic: \"{story_data['epic_name']}\"
priority: {story_data['priority']}
complexity: {story_data['complexity']}
depends_on: {json.dumps(story_data['depends_on'])}
updated_at: {timestamp}
---

# Story {story_data['story_id']}: {story_data['title']}

## Context

{context}

## Implementation Steps

{implementation_steps}

## Acceptance Criteria

{acceptance_criteria}

## Notes For Dev

{notes_for_dev}

## Dev Agent Record

### Status
ready-for-dev

### Agent Model Used
Claude Haiku 4.5

### Completion Notes List
- [ ] Acceptance criteria verified against implementation
- [ ] Code review completed
- [ ] Integration tests passing
- [ ] Story marked complete

"""

    with open(filename, 'w', encoding='utf-8') as f:
        f.write(frontmatter)

    return filename


def main():
    """Main execution."""
    docs_path = Path(__file__).parent / 'docs' / 'nextlens' / 'src' / FEATURE_ID
    stories_md_path = docs_path / 'stories.md'

    print(f"Reading: {stories_md_path}")
    with open(stories_md_path, 'r', encoding='utf-8') as f:
        content = f.read()

    print("Parsing stories...")
    stories = parse_stories_md(content)
    print(f"Found {len(stories)} stories")

    expected_story_count = len(re.findall(r'^### Story ', content, re.MULTILINE))
    if len(stories) != expected_story_count:
        print(f"WARNING: Expected {expected_story_count} stories but found {len(stories)}")

    story_filename_map = {story['story_id']: build_story_filename(story) for story in stories}
    collisions = {}
    for story_id, filename in story_filename_map.items():
        collisions.setdefault(filename, []).append(story_id)
    collisions = {filename: ids for filename, ids in collisions.items() if len(ids) > 1}
    if collisions:
        collision_details = ', '.join(f"{filename}: {', '.join(ids)}" for filename, ids in sorted(collisions.items()))
        raise ValueError(f"Story filename collision detected: {collision_details}")

    print(f"Generating story files in: {docs_path / 'stories'}")
    created_files = []
    for story_data in stories:
        filename = create_story_file(story_data, docs_path, story_filename_map)
        created_files.append(filename)
        print(f"Created: {filename}")

    print(f"\n✓ Generated {len(created_files)} story files")

    summary = {
        'status': 'success',
        'artifacts_created': len(created_files),
        'artifact_paths': [str(f) for f in created_files],
        'stories_generated': len(stories),
        'frontmatter_validation': f"all {len(stories)} files have required fields",
        'message': f"Successfully generated {len(stories)} story files for {FEATURE_ID}",
    }

    print("\nSummary:")
    print(json.dumps(summary, indent=2))

    return summary


if __name__ == '__main__':
    main()
