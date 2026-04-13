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
copies the GitHub Copilot adapter (.github/), and creates the standard
workspace directory structure.

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
https://github.wellsfargo.com/NonApp-CLAUT/NonApp-claut-SDD-Config.git

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
import re
import shutil
import stat
import subprocess
import sys
import argparse
try:
    import yaml
except ImportError:
    yaml = None

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


def save_user_profile(project_root: str, experience: str, role: str) -> None:
    """Persist experience_mode and primary_role to .github/lens/personal/profile.yaml."""
    profile_dir = os.path.join(project_root, ".github", "lens", "personal")
    os.makedirs(profile_dir, exist_ok=True)
    profile_path = os.path.join(profile_dir, "profile.yaml")
    with open(profile_path, "w", encoding="utf-8") as f:
        f.write(f"experience_mode: {experience}\n")
        f.write(f"primary_role: {role}\n")
    print(f"{GREEN}Profile saved: experience={experience}, role={role}{RESET}")


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
        description="Clone a repo branch into a worktree folder."
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

    save_user_profile(root, experience, role)

    for folder in ["docs", "TargetProjects"]:
        folder_path = os.path.join(root, folder)
        os.makedirs(folder_path, exist_ok=True)
        print(f"Created directory: {folder_path}")

    # --- Governance clone ---
    default_governance_url = "https://github.com/crisweber2600/Lens.Core.Governance.git"

    governance_url = args.governance_url or default_governance_url

    # Resolve destination from bmadconfig.yaml if available
    bmadconfig_path = os.path.join(root, "lens.core", "_bmad", "lens-work", "bmadconfig.yaml")
    gov_dest = None
    if yaml and os.path.exists(bmadconfig_path):
        with open(bmadconfig_path, "r", encoding="utf-8") as f:
            cfg = yaml.safe_load(f)
        raw_path = cfg.get("governance_repo_path", "")
        if raw_path:
            resolved = raw_path.replace("{project-root}", root)
            gov_dest = os.path.normpath(resolved)
            print(f"{CYAN}governance_repo_path from bmadconfig.yaml: {gov_dest}{RESET}")

    if gov_dest is None:
        gov_name = os.path.splitext(os.path.basename(governance_url.rstrip("/")))[0]
        gov_dest = os.path.join(root, "TargetProjects", gov_name)
        print(f"{YELLOW}bmadconfig.yaml not found or unreadable — using default path: {gov_dest}{RESET}")

    print(f"\nCloning governance repo into {gov_dest}...")
    if os.path.exists(gov_dest):
        force_remove(gov_dest)

    result = run(["git", "clone", governance_url, gov_dest])
    if result.returncode != 0:
        print(f"{RED}Failed to clone governance repo{RESET}")
        sys.exit(1)

    print(f"{GREEN}Cloned governance repo into {gov_dest}{RESET}")

    # --- Clone repos from governance repo-inventory.yaml ---
    inventory_path = os.path.join(gov_dest, "repo-inventory.yaml")
    if yaml and os.path.exists(inventory_path):
        print(f"\n{CYAN}Reading repo-inventory.yaml from governance...{RESET}")
        with open(inventory_path, "r", encoding="utf-8") as f:
            inv = yaml.safe_load(f) or {}
        repos = inv.get("repositories", inv.get("repos", []))
        cloned = skipped = failed = 0
        for entry in repos:
            name = entry.get("name", "")
            remote_url = entry.get("remote_url", "")
            local_path = entry.get("local_path", "")
            if not name or not remote_url:
                print(f"  {YELLOW}[skip] Incomplete entry (missing name or remote_url): {entry}{RESET}")
                skipped += 1
                continue
            # local_path is project-root-relative (e.g. "TargetProjects/…")
            if local_path:
                dest = os.path.normpath(os.path.join(root, local_path))
            else:
                dest = os.path.join(root, "TargetProjects", name)
            if os.path.exists(os.path.join(dest, ".git")):
                print(f"  [ok] Already on disk, skipping: {name}")
                skipped += 1
                continue
            os.makedirs(dest, exist_ok=True)
            print(f"  Cloning {name} → {dest}")
            r = run(["git", "clone", remote_url, dest])
            if r.returncode == 0:
                print(f"  {GREEN}[ok] Cloned {name}{RESET}")
                cloned += 1
            else:
                print(f"  {RED}[fail] Clone failed: {name}{RESET}")
                failed += 1
        print(f"\n{GREEN}Repo inventory sync: {cloned} cloned | {skipped} skipped | {failed} failed{RESET}")
    else:
        print(f"{YELLOW}No repo-inventory.yaml found in governance repo — skipping inventory clone.{RESET}")

    # --- Preflight ---
    preflight_script = os.path.join(root, "lens.core", "_bmad", "lens-work", "scripts", "preflight.py")
    if os.path.exists(preflight_script):
        print(f"\n{CYAN}Running preflight...{RESET}")
        result = run(["uv", "run", preflight_script], cwd=root)
        if result.returncode != 0:
            print(f"{YELLOW}Warning: preflight exited with code {result.returncode}{RESET}")
    else:
        print(f"{YELLOW}preflight.py not found — skipping.{RESET}")


if __name__ == "__main__":
    main()