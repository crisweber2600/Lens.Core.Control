from pathlib import Path

import generate_story_files as generator


def test_generated_story_uses_dev_ready_sections(tmp_path):
    docs_path = tmp_path / "docs" / "nextlens" / "src" / generator.FEATURE_ID
    story_data = {
        "story_id": "1.1",
        "title": "Implement Command Entry Point and Argument Parser",
        "epic_num": "1",
        "epic_name": "EP1: Command Spine & Interactive Entry Point",
        "priority": "Critical",
        "complexity": "Medium",
        "depends_on": [],
        "user_story": "As a **NextLens operator**, I want the system to accept command-line arguments so that I can invoke different operational paths.",
        "acceptance_criteria": "1. **Command Parsing - New Mode**\n   - Given: `nextlens new --context path/to/context.yaml`\n   - When: command is parsed\n   - Then: mode=`new`\n\n2. **Help Text**\n   - Given: `nextlens --help`\n   - When: help is requested\n   - Then: usage is printed",
        "definition_of_done": [
            "Command parser accepts all three modes.",
            "Unit tests cover parsing paths.",
        ],
        "full_text": "",
    }

    filename = generator.create_story_file(
        story_data,
        docs_path,
        {"1.1": generator.build_story_filename(story_data)},
    )

    rendered = filename.read_text(encoding="utf-8")

    assert "## Context" in rendered
    assert "## Implementation Steps" in rendered
    assert "## Acceptance Criteria" in rendered
    assert "## Notes For Dev" in rendered
    assert "## Dev Agent Record" in rendered
    assert "- [ ] Command Parsing - New Mode" in rendered
    assert generator.TARGET_REPO_ROOT in rendered


def test_parse_numbered_items_preserves_nested_lines():
    block = "1. **First**\n   - Given: one\n   - Then: two\n2. **Second**\n   - Given: three"

    items = generator.parse_numbered_items(block)

    assert items == [
        ["**First**", "   - Given: one", "   - Then: two"],
        ["**Second**", "   - Given: three"],
    ]