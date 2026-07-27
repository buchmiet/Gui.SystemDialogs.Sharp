#!/usr/bin/env python3
"""Fails when platform/framework concepts leak into Gui.SystemDialogs.Sharp."""

from pathlib import Path
import sys
import xml.etree.ElementTree as ET

root = Path(__file__).resolve().parents[1]
contracts = root / "Gui.SystemDialogs.Sharp"
forbidden_tokens = (
    "Avalonia",
    "Microsoft.Maui",
    "Microsoft.UI",
    "Microsoft.Win32",
    "System.Windows",
    "Windows.Storage",
    "WinRT",
    "WPF",
    "WinUI",
    "HWND",
    "WindowHandle",
)

violations: list[str] = []
# AssemblyInfo.cs may list adapter names only in InternalsVisibleTo — skip it.
for path in contracts.glob("*.cs"):
    if path.name == "AssemblyInfo.cs":
        continue
    text = path.read_text(encoding="utf-8")
    for token in forbidden_tokens:
        if token.casefold() in text.casefold():
            violations.append(f"{path.relative_to(root)} contains {token!r}")

project = contracts / "Gui.SystemDialogs.Sharp.csproj"
tree = ET.parse(project)
for item_name in ("ProjectReference", "PackageReference", "FrameworkReference"):
    for item in tree.findall(f".//{item_name}"):
        violations.append(
            f"{project.relative_to(root)} contains forbidden {item_name}: "
            f"{item.attrib.get('Include', '<unknown>')}"
        )

if violations:
    print("Contract boundary violations:", file=sys.stderr)
    for violation in violations:
        print(f"- {violation}", file=sys.stderr)
    raise SystemExit(1)

print("Contract boundary OK")
