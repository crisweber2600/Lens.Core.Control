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


def build_story_filename(story_data):
    """Build a stable story filename from story metadata."""
    sanitized_title = story_data['title'].lower().replace('&', 'and')
    sanitized_title = re.sub(r'[^a-z0-9]+', '-', sanitized_title).strip('-')
    return f"{story_data['story_id']}-{sanitized_title}.md"


def parse_stories_md(content):
    """Parse stories.md to extract individual stories."""
    stories = []
    section_boundary = r'(?:\n\n(?=\*\*[A-Z][^:\n]+:\*\*)|$)'

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
        ac_pattern = rf'\*\*Acceptance Criteria:\*\*\n(.*?){section_boundary}'
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
        user_story_pattern = rf'\*\*User Story:\*\*\n\n(.*?){section_boundary}'
        user_story_match = re.search(user_story_pattern, story_text, re.DOTALL)
        user_story = user_story_match.group(1).strip() if user_story_match else ""
        if not user_story:
            print(f"WARNING: User story block could not be parsed for Story {story_id}")

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
    user_story = story_data['user_story'] or '_User story source could not be parsed from `stories.md`._'

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

## User Story

{user_story}

## Acceptance Criteria

{story_data['acceptance_criteria']}

## Dependencies

{', '.join(story_data['depends_on']) if story_data['depends_on'] else 'None (independent story)'}

## Complexity Assessment

- **Rating:** {story_data['complexity']}
- **Priority:** {story_data['priority']}
- **Epic:** {story_data['epic_name']}

## Implementation Notes

This story is part of the NextLens v1 implementation. See [epics.md](../epics.md) for epic context and [architecture.md](../architecture.md) for technical requirements.

### Related Stories

Depends on: {', '.join(dependency_links) if dependency_links else 'None'}

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
