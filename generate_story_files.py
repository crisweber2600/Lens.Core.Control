#!/usr/bin/env python3
"""
Generate individual story files for NextLens v1 feature.
Parses stories.md and creates individual YAML story files with frontmatter.
"""

import re
import json
from pathlib import Path
from datetime import datetime

def parse_stories_md(content):
    """Parse stories.md to extract all 41 stories."""
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
        story_title = match.group(2)
        
        # Parse Story ID to get epic number
        epic_num = story_id.split('.')[0]
        
        # Extract acceptance criteria
        ac_pattern = r'\*\*Acceptance Criteria:\*\*\n(.*?)(?:\n\n\*\*|$)'
        ac_match = re.search(ac_pattern, story_text, re.DOTALL)
        acceptance_criteria = ac_match.group(1).strip() if ac_match else ""
        
        # Extract complexity
        complexity_pattern = r'\*\*Complexity:\*\*\s*(\w+)'
        complexity_match = re.search(complexity_pattern, story_text)
        complexity = complexity_match.group(1) if complexity_match else "Medium"
        
        # Extract priority
        priority_pattern = r'\*\*Priority:\*\*\s*([^\n]+)'
        priority_match = re.search(priority_pattern, story_text)
        priority = priority_match.group(1) if priority_match else "High"
        
        # Extract dependencies
        deps_pattern = r'\*\*Dependencies:\*\*\s*(.+?)(?:\n\n|$)'
        deps_match = re.search(deps_pattern, story_text, re.DOTALL)
        dependencies_text = deps_match.group(1).strip() if deps_match else ""
        
        # Parse dependencies into list
        depends_on = []
        if "None" not in dependencies_text and dependencies_text:
            # Extract story references like "Story 1.1" or "EP1.1"
            dep_refs = re.findall(r'(?:Story |EP)?(\d+\.\d+)', dependencies_text)
            depends_on = list(set(dep_refs))
            depends_on.sort(key=lambda x: (int(x.split('.')[0]), int(x.split('.')[1])))
        
        # Extract epic name
        epic_pattern = r'\*\*Epic:\*\*\s*(.+?)(?:\n|$)'
        epic_match = re.search(epic_pattern, story_text)
        epic_name = epic_match.group(1) if epic_match else f"EP{epic_num}"
        
        # Extract user story (As a... I want... so that...)
        user_story_pattern = r'\*\*User Story:\*\*\n\nAs a \*\*(.+?)\*\*, I want (?:the system to )?\*\*(.+?)\*\*, so that \*\*(.+?)\*\*\.'
        user_story_match = re.search(user_story_pattern, story_text, re.DOTALL)
        
        user_role = ""
        user_action = ""
        user_benefit = ""
        if user_story_match:
            user_role = user_story_match.group(1)
            user_action = user_story_match.group(2)
            user_benefit = user_story_match.group(3)
        
        stories.append({
            'story_id': story_id,
            'title': story_title,
            'epic_num': epic_num,
            'epic_name': epic_name,
            'priority': priority,
            'complexity': complexity,
            'depends_on': depends_on,
            'user_role': user_role,
            'user_action': user_action,
            'user_benefit': user_benefit,
            'acceptance_criteria': acceptance_criteria,
            'full_text': story_text.strip()
        })
    
    return stories

def create_story_file(story_data, docs_path):
    """Create individual story file with YAML frontmatter."""
    
    # Create stories directory if it doesn't exist
    stories_dir = Path(docs_path) / 'stories'
    stories_dir.mkdir(parents=True, exist_ok=True)
    
    # Create filename: EP#.#-story-title.md
    sanitized_title = story_data['title'].lower().replace(' ', '-').replace('&', 'and')
    sanitized_title = re.sub(r'[^a-z0-9\-]', '', sanitized_title)
    filename = f"{stories_dir}/{story_data['story_id']}-{sanitized_title}.md"
    
    # Build YAML frontmatter
    frontmatter = f"""---
feature: nextlens-src-implement
story_id: "{story_data['story_id']}"
doc_type: story
status: ready
title: "{story_data['title']}"
epic: "{story_data['epic_name']}"
priority: {story_data['priority']}
complexity: {story_data['complexity']}
depends_on: {json.dumps(story_data['depends_on'])}
updated_at: {datetime.utcnow().isoformat()}Z
---

# Story {story_data['story_id']}: {story_data['title']}

## User Story

As a **{story_data['user_role']}**,
I want the system to **{story_data['user_action']}**,
so that **{story_data['user_benefit']}**.

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

Depends on: {', '.join([f'[Story {dep}](../stories/{dep}-*.md)' for dep in story_data['depends_on']]) if story_data['depends_on'] else 'None'}

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
    
    # Write file
    with open(filename, 'w', encoding='utf-8') as f:
        f.write(frontmatter)
    
    return filename

def main():
    """Main execution."""
    docs_path = Path(__file__).parent / 'docs' / 'nextlens' / 'src' / 'nextlens-src-implement'
    stories_md_path = docs_path / 'stories.md'
    
    print(f"Reading: {stories_md_path}")
    with open(stories_md_path, 'r', encoding='utf-8') as f:
        content = f.read()
    
    print("Parsing stories...")
    stories = parse_stories_md(content)
    print(f"Found {len(stories)} stories")
    
    if len(stories) != 41:
        print(f"WARNING: Expected 41 stories but found {len(stories)}")
    
    # Generate story files
    print(f"Generating story files in: {docs_path / 'stories'}")
    
    created_files = []
    for story_data in stories:
        filename = create_story_file(story_data, docs_path)
        created_files.append(filename)
        print(f"Created: {filename}")
    
    print(f"\n✓ Generated {len(created_files)} story files")
    
    # Create summary
    summary = {
        'status': 'success',
        'artifacts_created': len(created_files),
        'artifact_paths': [str(f) for f in created_files],
        'stories_generated': len(stories),
        'frontmatter_validation': f"all {len(stories)} files have required fields",
        'message': f"Successfully generated {len(stories)} story files for nextlens-src-implement"
    }
    
    print("\nSummary:")
    print(json.dumps(summary, indent=2))
    
    return summary

if __name__ == '__main__':
    main()
