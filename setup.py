r"""
===========================================================
lens-local-setup.py — LENS Workbench
Local Environment Bootstrap
===========================================================

USAGE

Run this script from the ROOT of your new control repo folder.
Example:
    cd
    C:\Users\<you>\source\MyProject
    python lens-local-setup.py

WHAT IT DOES

Clones the LENS release module (lens.core) into the current directory,
copies the GitHub Copilot adapter (.github/), validates onboarding
prerequisites, bootstraps governance and TargetProjects, writes local
onboarding metadata, and creates the standard workspace directory structure.

Selecting all default options sets up the CIS LENS instance.
Specifying different values (--repo-url, --branch) allows isolated
setups for personal repos or other teams.

-----------------------------------------------------------

VOCABULARY

Root
The folder you run this script from. All paths below are relative to Root.
This folder becomes your control repo — the single source of truth for
your LENS workspace. It is not committed to source control directly;
instead it acts as a local workspace container.

lens.core (brains)
The read-only LENS release module. Contains agents, prompts, workflows,
and scripts, cloned from the central LENS repository. Never edit directly.

lens.governance (constitutions and docs)
The governance repo for your organization or team. Holds constitutional
rules, policies, and shared documentation that all target projects adhere to.
Cloned under TargetProjects/lens/ by setup-control-repo.

TargetProjects (your repos)
A local folder that holds clones of the repos you are actively working on,
organized by domain. These are your real source repositories — the ones
LENS operates on.

===========================================================
GUIDANCE: Governance Repo
===========================================================

The default governance repo is continually developed to be broadly
applicable across teams and use cases. For most pods, using the default
as-is (or pulling updates from it) is the right choice.

If your pod or organization requires significant structural changes
to governance — custom lifecycle stages, different constitutional rules,
or team-specific policies — it may be desirable to FORK the default repo
and maintain your own governance baseline.

Default governance repo:


Fork conservatively. Forks that drift far from the default become harder
to re-sync with upstream improvements.

-----------------------------------------------------------

Workspace layout after setup:

Root/
├── lens.core/
│   └── LENS release module (read-only)
├── lens.governance/
│   └── Constitutional authority for your org/team
├── .github/
│   └── GitHub Copilot adapter (copied from lens.core)
├── docs/
│   └── Planning and initiative artifacts
├── TargetProjects/
│   ├── Domain1/
│   │   ├── Repo1/
│   │   └── Repo2/
│   ├── Domain2/
│   │   └── Repo3/
└── LENS_VERSION

===========================================================
"""

import os
import json
import re
import shutil
import stat
import subprocess
import sys
import argparse
import urllib.error
import urllib.parse
import urllib.request
from datetime import datetime, timezone

CYAN = "\033[96m"
YELLOW = "\033[93m"
GREEN = "\033[92m"
RED = "\033[91m"
RESET = "\033[0m"

# ---------------------------------------------------------------------------
# Prompt catalog metadata — (experience, role)
# experience: "full" | "both" | "any"
# role: "any" | "plan" | "dev" | "admin" | None
# Only Keep=Y prompts are listed. Unknown stems are always included.
# ---------------------------------------------------------------------------
PROMPT_METADATA: dict[str, tuple[str, str | None]] = {
    "lens-adversarial-review":          ("full",  "any"),
    "lens-batch":                        ("both",  "any"),
    "lens-bmad-brainstorming":           ("full",  "plan"),
    "lens-bmad-code-review":             ("full",  "dev"),
    "lens-bmad-create-architecture":     ("full",  "plan"),
    "lens-bmad-create-epics-and-stories":("full",  "plan"),
    "lens-bmad-create-prd":              ("full",  "plan"),
    "lens-bmad-create-story":            ("full",  "plan"),
    "lens-bmad-create-ux-design":        ("full",  "plan"),
    "lens-bmad-domain-research":         ("full",  "plan"),
    "lens-bmad-market-research":         ("full",  "plan"),
    "lens-bmad-product-brief":           ("full",  "plan"),
    "lens-bmad-quick-dev":               ("full",  "dev"),
    "lens-bmad-sprint-planning":         ("full",  "plan"),
    "lens-bmad-technical-research":      ("full",  "plan"),
    "lens-businessplan":                 ("both",  "plan"),
    "lens-complete":                     ("both",  "any"),
    "lens-constitution":                 ("full",  "admin"),
    "lens-dev":                          ("both",  "dev"),
    "lens-discover":                     ("both",  "any"),
    "lens-expressplan":                  ("both",  "any"),
    "lens-finalizeplan":                 ("both",  "plan"),
    "lens-help":                         ("both",  "any"),
    "lens-log-problem":                  ("full",  None),
    "lens-move-feature":                 ("full",  "plan"),
    "lens-new-domain":                   ("any",   "plan"),
    "lens-new-feature":                  ("both",  "any"),
    "lens-new-service":                  ("both",  "any"),
    "lens-next":                         ("both",  "any"),
    "lens-preflight":                    ("both",  "any"),
    "lens-preplan":                      ("both",  "plan"),
    "lens-split-feature":                ("both",  "plan"),
    "lens-switch":                       ("both",  "any"),
    "lens-techplan":                     ("both",  "plan"),
    "lens-theme":                        ("both",  "any"),
    "lens-upgrade":                      ("full",  "admin"),
}


def should_include_prompt(stem: str, experience: str, role: str) -> bool:
    """Return True if a prompt should be installed for the given profile."""
    meta = PROMPT_METADATA.get(stem)
    if meta is None:
        return True  # unknown stem — always include (forward-compatible)

    exp, prole = meta

    # Experience filter: "lite" excludes prompts whose experience is "full"
    if experience == "lite" and exp == "full":
        return False

    # Admin role sees everything
    if role == "admin":
        return True

    # Admin-only prompts are excluded for non-admin roles
    if prole == "admin":
        return False

    # Role filter
    if role == "planner":
        return prole in ("plan", "any", None)
    if role == "dev":
        return prole in ("dev", "any", None)
    # "both" — include everything non-admin
    return True


def prompt_experience_mode() -> str:
    """Ask user to select Experience Mode and return 'lite' or 'full'."""
    print(f"\n{CYAN}Experience Mode{RESET}")
    print(f"  [1] lite  — essential prompts only")
    print(f"  [2] full  — complete prompt set (default)")
    print()
    sel = input("Select experience mode (1-2) [default: full]: ").strip().lower()
    if sel in ("1", "lite"):
        return "lite"
    return "full"  # default


def prompt_primary_role() -> str:
    """Ask user to select Primary Role and return 'planner', 'dev', 'both', or 'admin'."""
    print(f"\n{CYAN}Primary Role{RESET}")
    print(f"  [1] planner — planning, design, and strategy prompts")
    print(f"  [2] dev     — development and implementation prompts")
    print(f"  [3] both    — full prompt set (default)")
    print()
    sel = input("Select primary role (1-3) [default: both]: ").strip().lower()
    if sel in ("1", "planner"):
        return "planner"
    if sel in ("2", "dev"):
        return "dev"
    if sel == "admin":
        return "admin"
    return "both"  # default


def _yaml_quote(value: str) -> str:
    if value == "" or re.search(r"[:#\[\]{}]|^\s|\s$", value):
        return json.dumps(value)
    return value


def _read_simple_yaml_scalar(path: str, key: str) -> str:
    if not os.path.exists(path):
        return ""
    pattern = re.compile(rf"^\s*{re.escape(key)}\s*:\s*(.+?)\s*$")
    with open(path, "r", encoding="utf-8") as handle:
        for line in handle:
            match = pattern.match(line)
            if not match:
                continue
            value = match.group(1).split("#", 1)[0].strip()
            if value[:1] in {"'", '"'} and value[-1:] == value[:1]:
                return value[1:-1]
            return value
    return ""


def _resolve_project_path(project_root: str, raw_path: str, fallback_parts: tuple[str, ...]) -> str:
    if raw_path:
        resolved = raw_path.replace("{project-root}", project_root)
        if os.path.isabs(resolved):
            return resolved
        return os.path.normpath(os.path.join(project_root, resolved))
    return os.path.join(project_root, *fallback_parts)


def save_user_profile(
    project_root: str,
    experience: str,
    role: str,
    provider: str,
    auth_method: str,
    target_projects_path: str,
) -> None:
    """Persist onboarding profile fields to .lens/personal/profile.yaml."""
    profile_dir = os.path.join(project_root, ".lens", "personal")
    os.makedirs(profile_dir, exist_ok=True)
    profile_path = os.path.join(profile_dir, "profile.yaml")
    created = datetime.now(tz=timezone.utc).strftime("%Y-%m-%dT%H:%M:%SZ")
    with open(profile_path, "w", encoding="utf-8") as f:
        f.write(f"name: {_yaml_quote(os.path.basename(project_root) or 'unknown')}\n")
        f.write(f"git_provider: {_yaml_quote(provider)}\n")
        f.write("default_remote: origin\n")
        f.write(f"auth_method: {_yaml_quote(auth_method)}\n")
        f.write(f"experience_mode: {_yaml_quote(experience)}\n")
        f.write(f"primary_role: {_yaml_quote(role)}\n")
        f.write("role: contributor\n")
        f.write("domain: null\n")
        f.write(f"provider: {_yaml_quote(provider)}\n")
        f.write("batch_preferences:\n")
        f.write("  question_mode: guided\n")
        f.write("  auto_checkpoint: true\n")
        f.write(f"target_projects_path: {_yaml_quote(target_projects_path)}\n")
        f.write(f"created: {_yaml_quote(created)}\n")
        f.write(f"created_at: {_yaml_quote(created)}\n")
        f.write(f"updated_at: {_yaml_quote(created)}\n")
    print(f"{GREEN}Profile saved: experience={experience}, role={role}, provider={provider}{RESET}")


def detect_git_origin(project_root: str) -> str:
    result = run(
        ["git", "-C", project_root, "remote", "get-url", "origin"],
        capture_output=True,
        text=True,
    )
    if result.returncode != 0:
        return ""
    return result.stdout.strip()


def detect_provider(remote_url: str) -> dict[str, str | None]:
    remote = remote_url.strip()
    if not remote:
        return {"provider": "unknown", "host": "", "api_base": None, "remote_url": remote}

    host = ""
    ssh_match = re.match(r"^(?:ssh://)?git@([^:/]+)[:/].+$", remote)
    if ssh_match:
        host = ssh_match.group(1)
    else:
        parsed = urllib.parse.urlparse(remote)
        host = parsed.hostname or ""

    host_lower = host.lower()
    provider = "unknown"
    api_base = None
    if "dev.azure.com" in host_lower or "visualstudio.com" in host_lower or "ssh.dev.azure.com" in host_lower:
        provider = "azure-devops"
        api_base = f"https://{host}" if host else None
    elif host_lower == "github.com" or "github" in host_lower:
        provider = "github"
        api_base = "https://api.github.com" if host_lower == "github.com" else f"https://{host}/api/v3"

    return {"provider": provider, "host": host, "api_base": api_base, "remote_url": remote}


def governance_repo_name_from_url(governance_url: str) -> str:
    name = os.path.splitext(os.path.basename(governance_url.rstrip("/")))[0]
    return name or "lens-governance"


def derive_governance_repo_name(project_root: str) -> str:
    folder_name = os.path.basename(os.path.abspath(project_root))
    if not folder_name:
        return "lens-governance"

    for suffix, replacement in (
        (".Control", ".Governance"),
        (".control", ".governance"),
        ("-control", "-governance"),
        ("_control", "_governance"),
        (".src", ".bmad.governance"),
    ):
        if folder_name.endswith(suffix):
            return f"{folder_name[:-len(suffix)]}{replacement}"

    return f"{folder_name}.governance"


def derive_governance_url(remote_url: str, governance_repo_name: str) -> str:
    remote = remote_url.strip()
    if not remote:
        return ""

    ssh_match = re.match(r"^((?:ssh://)?git@[^:/]+[:/].*/)[^/]+?(?:\.git)?/?$", remote)
    if ssh_match:
        return f"{ssh_match.group(1)}{governance_repo_name}.git"

    parsed = urllib.parse.urlparse(remote)
    if parsed.scheme and parsed.netloc and parsed.path:
        parts = parsed.path.rstrip("/").split("/")
        if len(parts) >= 2:
            parts[-1] = f"{governance_repo_name}.git"
            return urllib.parse.urlunparse(parsed._replace(path="/".join(parts)))

    return remote


def resolve_governance_clone(project_root: str, governance_url: str | None, remote_template: str) -> dict[str, str]:
    if governance_url:
        repo_name = governance_repo_name_from_url(governance_url)
        resolved_url = governance_url
        source = "explicit"
    else:
        repo_name = derive_governance_repo_name(project_root)
        resolved_url = derive_governance_url(remote_template, repo_name)
        source = "derived"

    return {
        "path": os.path.join(project_root, "TargetProjects", "lens", repo_name),
        "repo_name": repo_name,
        "source": source,
        "url": resolved_url,
    }


def _first_env(*names: str) -> tuple[str, str] | None:
    for name in names:
        value = os.getenv(name, "").strip()
        if value:
            return name, value
    return None


def validate_github_auth(api_base: str, token: str) -> tuple[bool, str]:
    request = urllib.request.Request(
        f"{api_base}/user",
        headers={
            "Authorization": f"token {token}",
            "Accept": "application/vnd.github+json",
            "User-Agent": "lens-local-setup",
        },
    )
    try:
        with urllib.request.urlopen(request, timeout=10) as response:
            payload = json.loads(response.read().decode("utf-8"))
        return True, str(payload.get("login", "authenticated"))
    except urllib.error.HTTPError as exc:
        return False, f"HTTP {exc.code}"
    except Exception as exc:
        return False, str(exc)


def validate_auth(provider_info: dict[str, str | None]) -> dict[str, str | bool]:
    provider = provider_info.get("provider", "unknown") or "unknown"
    host = provider_info.get("host", "") or ""
    if provider == "github":
        env_names = ("GITHUB_PAT", "GH_TOKEN") if host == "github.com" else ("GH_ENTERPRISE_TOKEN", "GH_TOKEN", "GITHUB_PAT")
        resolved = _first_env(*env_names)
        if resolved is None:
            return {
                "status": "warn",
                "method": "missing",
                "message": "No GitHub PAT found in environment. Run store-github-pat.py outside chat.",
                "validated": False,
            }
        env_name, token = resolved
        valid, detail = validate_github_auth(str(provider_info.get("api_base") or "https://api.github.com"), token)
        return {
            "status": "ok" if valid else "warn",
            "method": f"env:{env_name}",
            "message": f"GitHub authentication {'validated' if valid else 'failed'} ({detail}).",
            "validated": valid,
        }
    if provider == "azure-devops":
        resolved = _first_env("AZURE_DEVOPS_PAT", "AZDO_PAT")
        if resolved is None:
            return {
                "status": "warn",
                "method": "missing",
                "message": "No Azure DevOps PAT found in environment.",
                "validated": False,
            }
        env_name, _token = resolved
        return {"status": "ok", "method": f"env:{env_name}", "message": "Azure DevOps PAT detected.", "validated": True}
    return {
        "status": "warn",
        "method": "unknown",
        "message": "Unable to detect provider from git remote. Provider auth check skipped.",
        "validated": False,
    }


def ensure_workspace_structure(project_root: str, lens_root: str, governance_path: str) -> dict[str, str]:
    config_path = os.path.join(lens_root, "_bmad", "lens-work", "bmadconfig.yaml")
    resolved_paths: dict[str, str] = {}
    for key, config_key, fallback_parts in (
        ("personal_output", "personal_output_folder", (".lens", "personal")),
        ("target_projects", "target_projects_path", ("TargetProjects",)),
    ):
        raw = _read_simple_yaml_scalar(config_path, config_key)
        resolved_path = _resolve_project_path(project_root, raw, fallback_parts)
        os.makedirs(resolved_path, exist_ok=True)
        resolved_paths[key] = resolved_path

    os.makedirs(governance_path, exist_ok=True)
    return resolved_paths


def ensure_gitignore_entries(project_root: str) -> None:
    gitignore_path = os.path.join(project_root, ".gitignore")
    required_entries = [
        ".lens/",
        ".github/",
        "lens.core/",
        "TargetProjects/",
    ]
    existing = set()
    if os.path.exists(gitignore_path):
        with open(gitignore_path, "r", encoding="utf-8") as handle:
            existing = set(handle.read().splitlines())
    with open(gitignore_path, "a", encoding="utf-8") as handle:
        for entry in required_entries:
            if entry not in existing:
                handle.write(f"{entry}\n")


def write_governance_setup(project_root: str, lens_root: str, governance_path: str, governance_url: str) -> str:
    lifecycle_path = os.path.join(lens_root, "_bmad", "lens-work", "lifecycle.yaml")
    raw_setup_path = _read_simple_yaml_scalar(lifecycle_path, "setup_file")
    setup_path = _resolve_project_path(project_root, raw_setup_path, (".github", "lens-work", "governance-setup.yaml"))
    os.makedirs(os.path.dirname(setup_path), exist_ok=True)
    created = datetime.now(tz=timezone.utc).strftime("%Y-%m-%dT%H:%M:%SZ")
    with open(setup_path, "w", encoding="utf-8") as handle:
        handle.write(f"# Generated by setup.py — {created}\n")
        handle.write(f"governance_repo_path: {_yaml_quote(governance_path)}\n")
        handle.write(f"governance_remote_url: {_yaml_quote(governance_url)}\n")
    return setup_path


def write_lens_version(project_root: str, lens_root: str) -> str:
    schema_version = _read_simple_yaml_scalar(os.path.join(lens_root, "_bmad", "lens-work", "lifecycle.yaml"), "schema_version")
    if not schema_version:
        return ""
    version = schema_version if "." in schema_version else f"{schema_version}.0.0"
    with open(os.path.join(project_root, "LENS_VERSION"), "w", encoding="utf-8") as handle:
        handle.write(version)
    return version


def read_module_version(lens_root: str) -> str:
    version = _read_simple_yaml_scalar(os.path.join(lens_root, "_bmad", "lens-work", "module.yaml"), "version")
    return version or _read_simple_yaml_scalar(os.path.join(lens_root, "_bmad", "lens-work", "lifecycle.yaml"), "schema_version")


def run_preflight(project_root: str, preflight_script: str, governance_path: str, caller: str | None = None) -> int | None:
    if not os.path.exists(preflight_script):
        return None
    cmd = ["uv", "run", preflight_script]
    if caller:
        cmd.extend(["--caller", caller])
    cmd.extend(["--governance-path", governance_path])
    result = run(cmd, cwd=project_root)
    return result.returncode


def run_bootstrap_target_projects(project_root: str, lens_root: str, inventory_path: str, target_root: str) -> dict:
    script_path = os.path.join(lens_root, "_bmad", "lens-work", "scripts", "bootstrap-target-projects.py")
    if not os.path.exists(inventory_path):
        return {"repos": [], "errors": 1, "error": f"Inventory not found: {inventory_path}"}
    if not os.path.exists(script_path):
        return {"repos": [], "errors": 1, "error": f"Bootstrap script not found: {script_path}"}
    result = run(
        [
            "uv",
            "run",
            script_path,
            "--inventory-path",
            inventory_path,
            "--target-root",
            target_root,
            "--json",
        ],
        cwd=project_root,
        capture_output=True,
        text=True,
    )
    if not result.stdout.strip():
        return {"repos": [], "errors": 1, "error": result.stderr.strip() or "bootstrap-target-projects.py produced no output"}
    try:
        return json.loads(result.stdout)
    except json.JSONDecodeError:
        return {"repos": [], "errors": 1, "error": result.stderr.strip() or result.stdout.strip()}


def summarize_bootstrap_results(data: dict) -> dict[str, int | list[dict[str, str]]]:
    rows: list[dict[str, str]] = []
    counts = {"cloned": 0, "present": 0, "failed": 0, "skipped": 0, "dry_run": 0}
    for repo in data.get("repos", []):
        name = str(repo.get("name") or "?")
        dest = str(repo.get("dest") or "")
        action = str(repo.get("action") or "skip")
        status = "skipped"
        if repo.get("dry_run"):
            counts["dry_run"] += 1
            status = "dry-run"
        elif action == "verify" and repo.get("success"):
            counts["present"] += 1
            status = "present"
        elif action == "clone" and repo.get("success"):
            counts["cloned"] += 1
            status = "cloned"
        elif repo.get("error") == "no url found":
            counts["skipped"] += 1
        elif action == "clone":
            counts["failed"] += 1
            status = "failed"
        else:
            counts["skipped"] += 1
        rows.append({"repo": name, "path": dest, "action": action, "status": status})
    return {"rows": rows, **counts}


def print_bootstrap_table(summary: dict[str, int | list[dict[str, str]]]) -> None:
    rows = summary.get("rows", [])
    if not isinstance(rows, list) or not rows:
        return
    print("\n| Repo | Path | Action | Status |")
    print("|------|------|--------|--------|")
    for row in rows:
        print(f"| {row['repo']} | {row['path']} | {row['action']} | {row['status']} |")


def print_health_summary(checks: list[dict[str, str]]) -> None:
    print(f"\n{CYAN}Health Check{RESET}")
    for check in checks:
        color = GREEN if check["status"] == "ok" else YELLOW if check["status"] == "warn" else RED
        print(f"  {color}[{check['status'].upper()}]{RESET} {check['name']}: {check['detail']}")


def force_remove(path: str):
    """Remove a directory tree, clearing read-only flags on Windows if needed."""
    def on_error(func, path, _):
        os.chmod(path, stat.S_IWRITE)
        func(path)
    shutil.rmtree(path, onerror=on_error)


def run(cmd: list[str], **kwargs) -> subprocess.CompletedProcess:
    return subprocess.run(cmd, **kwargs)


def fetch_branches(repo_url: str) -> list[str]:
    print(f"\n{CYAN}Fetching available branches from {repo_url}...{RESET}")
    result = run(
        ["git", "ls-remote", "--heads", repo_url],
        capture_output=True,
        text=True
    )

    if result.returncode != 0:
        print(f"{RED}Error: Could not retrieve branches from {repo_url}:{RESET}")
        print(result.stderr.strip())
        sys.exit(1)

    branches = []
    for line in result.stdout.splitlines():
        match = re.search(r"refs/heads/(.+)$", line)
        if match:
            branches.append(match.group(1))

    return branches


def prompt_branch(branches: list[str], preselected: str | None) -> str:
    if preselected:
        return preselected

    default = "beta" if "beta" in branches else branches[0]

    print(f"\n{CYAN}Available branches:{RESET}")
    for i, branch in enumerate(branches):
        label = f"[{i + 1}] {branch}"
        if branch == default:
            print(f"{YELLOW}{label} (default){RESET}")
        else:
            print(label)

    print()
    selection = input(
        f"Select a branch (1-{len(branches)}) [default: {default}]: "
    ).strip()

    if selection == "":
        return default
    elif selection.isdigit() and 1 <= int(selection) <= len(branches):
        return branches[int(selection) - 1]
    else:
        print(f"{RED}Invalid selection. Exiting.{RESET}")
        sys.exit(1)


def main():
    parser = argparse.ArgumentParser(
        description="Bootstrap a Lens control repo with release, governance, profile, TargetProjects, and preflight setup."
    )

    parser.add_argument(
        "--folder-name",
        default="lens.core",
        help="Target folder name to clone into (default: lens.core)"
    )

    parser.add_argument(
        "--repo-url",
        default="https://github.com/crisweber2600/Lens.Core.Release.git",
        help="Remote repository URL"
    )

    parser.add_argument(
        "--branch",
        default=None,
        help="Branch to clone (optional, prompts if omitted)"
    )

    parser.add_argument(
        "--governance-url",
        default=None,
        help="URL of governance repo to clone into TargetProjects/"
    )

    args = parser.parse_args()

    root = os.getcwd()
    target = os.path.join(root, args.folder_name)
    origin_remote = detect_git_origin(root)
    provider_info = detect_provider(origin_remote or args.repo_url)
    auth_status = validate_auth(provider_info)

    if origin_remote:
        print(f"{CYAN}Detected provider {provider_info['provider']} from origin:{RESET} {origin_remote}")
    else:
        print(f"{YELLOW}No origin remote configured. Provider detection used {args.repo_url}.{RESET}")
    if auth_status["status"] == "ok":
        print(f"{GREEN}{auth_status['message']}{RESET}")
    else:
        print(f"{YELLOW}{auth_status['message']}{RESET}")

    branches = fetch_branches(args.repo_url)
    if not branches:
        print(f"{RED}Error: No branches found at {args.repo_url}{RESET}")
        sys.exit(1)

    branch = prompt_branch(branches, args.branch)
    print(f"\n{GREEN}Using branch: {branch}{RESET}\n")

    experience = prompt_experience_mode()
    role = prompt_primary_role()
    print(f"\n{GREEN}Profile: experience={experience}, role={role}{RESET}\n")

    if os.path.exists(target):
        force_remove(target)

    os.makedirs(target, exist_ok=True)
    print(f"Created directory: {target}")

    result = run(
        ["git", "clone", "-b", branch, args.repo_url, target]
    )

    if result.returncode != 0:
        print(f"{RED}Error: git clone failed.{RESET}")
        sys.exit(1)

    print(f"Cloned {args.repo_url} (branch {branch}) into {target}")

    lens_version = write_lens_version(root, target)
    module_version = read_module_version(target)

    github_source = os.path.join(target, ".github")
    github_dest = os.path.join(root, ".github")

    if os.path.exists(github_source):
        os.makedirs(github_dest, exist_ok=True)
        for item in os.listdir(github_source):
            src = os.path.join(github_source, item)
            dst = os.path.join(github_dest, item)
            if os.path.exists(dst):
                if os.path.isdir(dst):
                    force_remove(dst)
                else:
                    os.remove(dst)
            if os.path.isdir(src):
                shutil.copytree(src, dst)
            else:
                shutil.copy2(src, dst)
        print(f"Copied lens.core/.github contents to {github_dest}")
    else:
        print(f"{YELLOW}Warning: .github folder not found in lens.core, skipping copy.{RESET}")

    # Filter prompts based on selected experience mode and role
    prompts_dest = os.path.join(github_dest, "prompts")
    if os.path.isdir(prompts_dest):
        removed = 0
        for fname in list(os.listdir(prompts_dest)):
            if not fname.endswith(".prompt.md"):
                continue
            stem = fname[: -len(".prompt.md")]
            if not should_include_prompt(stem, experience, role):
                os.remove(os.path.join(prompts_dest, fname))
                removed += 1
        if removed:
            print(f"Removed {removed} prompt(s) excluded by your experience/role profile.")

    # --- Governance clone ---
    governance_clone = resolve_governance_clone(root, args.governance_url, origin_remote or args.repo_url)
    governance_url = governance_clone["url"]
    gov_dest = governance_clone["path"]

    if not governance_url:
        print(f"{RED}Error: Unable to derive governance remote from current folder or repo configuration.{RESET}")
        sys.exit(1)

    if governance_clone["source"] == "derived":
        print(
            f"{CYAN}Derived governance repo from folder '{os.path.basename(root)}':{RESET} "
            f"{governance_clone['repo_name']} -> {gov_dest}"
        )
        print(f"{CYAN}Derived governance remote:{RESET} {governance_url}")
    else:
        print(f"{CYAN}Using governance repo from explicit URL:{RESET} {governance_url}")
        print(f"{CYAN}Default governance clone path:{RESET} {gov_dest}")

    early_preflight = run_preflight(
        root,
        os.path.join(root, args.folder_name, "_bmad", "lens-work", "scripts", "preflight.py"),
        gov_dest,
        caller="onboard",
    )
    if early_preflight not in (None, 0):
        print(f"{YELLOW}Warning: early preflight exited with code {early_preflight}; continuing onboarding bootstrap.{RESET}")

    print(f"\nCloning governance repo into {gov_dest}...")
    if os.path.exists(gov_dest):
        force_remove(gov_dest)

    result = run(["git", "clone", governance_url, gov_dest])
    if result.returncode != 0:
        print(f"{RED}Failed to clone governance repo{RESET}")
        sys.exit(1)

    print(f"{GREEN}Cloned governance repo into {gov_dest}{RESET}")

    workspace_paths = ensure_workspace_structure(root, target, gov_dest)
    ensure_gitignore_entries(root)
    write_governance_setup(root, target, gov_dest, governance_url)
    save_user_profile(
        root,
        experience,
        role,
        str(provider_info.get("provider") or "unknown"),
        str(auth_status.get("method") or "unknown"),
        os.path.relpath(workspace_paths["target_projects"], root),
    )

    # --- Clone repos from governance repo-inventory.yaml ---
    inventory_path = os.path.join(gov_dest, "repo-inventory.yaml")
    bootstrap_data = run_bootstrap_target_projects(root, target, inventory_path, workspace_paths["target_projects"])
    bootstrap_summary = summarize_bootstrap_results(bootstrap_data)
    if bootstrap_data.get("error"):
        print(f"{YELLOW}{bootstrap_data['error']}{RESET}")
    print_bootstrap_table(bootstrap_summary)

    # --- Preflight ---
    preflight_script = os.path.join(root, "lens.core", "_bmad", "lens-work", "scripts", "preflight.py")
    final_preflight = run_preflight(root, preflight_script, gov_dest)
    if final_preflight not in (None, 0):
        print(f"{YELLOW}Warning: final preflight exited with code {final_preflight}{RESET}")
    elif final_preflight == 0:
        print(f"\n{GREEN}Preflight completed successfully.{RESET}")
    else:
        print(f"{YELLOW}preflight.py not found — skipping.{RESET}")

    health_checks = [
        {"name": "Origin remote", "status": "ok" if origin_remote else "warn", "detail": origin_remote or "No origin remote configured."},
        {"name": "Provider auth", "status": str(auth_status["status"]), "detail": str(auth_status["message"])} ,
        {"name": "Governance repo", "status": "ok" if os.path.isdir(gov_dest) else "fail", "detail": gov_dest},
        {"name": "Repo inventory", "status": "ok" if os.path.exists(inventory_path) else "warn", "detail": inventory_path},
        {"name": "TargetProjects bootstrap", "status": "warn" if int(bootstrap_summary.get("failed", 0)) else "ok", "detail": f"{bootstrap_summary.get('cloned', 0)} cloned, {bootstrap_summary.get('present', 0)} already present, {bootstrap_summary.get('failed', 0)} failed, {bootstrap_summary.get('skipped', 0)} skipped"},
        {"name": "Release module version", "status": "ok" if module_version else "fail", "detail": module_version or "version missing"},
        {"name": "LENS_VERSION", "status": "ok" if lens_version else "warn", "detail": lens_version or "not written"},
        {"name": "Workspace structure", "status": "ok", "detail": f"personal={workspace_paths['personal_output']}, target_projects={workspace_paths['target_projects']}"},
        {"name": "Preflight", "status": "ok" if final_preflight in (None, 0) else "warn", "detail": f"early={early_preflight} final={final_preflight}"},
    ]
    print_health_summary(health_checks)

    print(f"\n{CYAN}Next Steps{RESET}")
    if auth_status["status"] != "ok":
        print(f"  1. Store your GitHub PAT outside chat: uv run {args.folder_name}/_bmad/lens-work/scripts/store-github-pat.py")
    else:
        print("  1. Authentication looks ready.")
    print("  2. Run /next to see what to work on next.")
    print("  3. Run /status for a workspace health snapshot.")
    print("  4. Run /new-domain to create your first initiative structure.")


if __name__ == "__main__":
    main()