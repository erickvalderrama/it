"""Localization checks for the Flutter app.

Fails when:
- a locale is missing keys from the English template (reported by gen-l10n),
- a template key is not used in the app source,
- a widget passes a hardcoded string literal to Text, Tooltip, or a label parameter.

Requires `flutter` on PATH; the script runs `flutter gen-l10n` itself.
"""

import json
import re
import shutil
import subprocess
import sys
from pathlib import Path

front = Path(__file__).resolve().parent.parent / "front"
lib = front / "lib"
errors: list[str] = []

untranslated_file = front / "build" / "l10n" / "untranslated.json"
untranslated_file.parent.mkdir(parents=True, exist_ok=True)
untranslated_file.unlink(missing_ok=True)
flutter = shutil.which("flutter") or shutil.which("flutter.bat")
if flutter is None:
    sys.exit("flutter was not found on PATH.")
generation = subprocess.run([flutter, "gen-l10n"], cwd=front, capture_output=True, text=True, check=False)
if generation.returncode != 0:
    sys.exit("flutter gen-l10n failed:\n" + generation.stdout + generation.stderr)

if untranslated_file.exists():
    untranslated = json.loads(untranslated_file.read_text(encoding="utf-8") or "{}")
    for locale, keys in untranslated.items():
        errors.append(f"Locale '{locale}' is missing translations: {', '.join(keys)}")

template = json.loads((lib / "l10n" / "app_en.arb").read_text(encoding="utf-8"))
keys = [key for key in template if not key.startswith("@")]
sources = [
    path
    for path in lib.rglob("*.dart")
    if not path.name.startswith("app_localizations")
]
code = "\n".join(path.read_text(encoding="utf-8") for path in sources)
for key in keys:
    if not re.search(rf"\.{re.escape(key)}\b", code):
        errors.append(f"Unused localization key: {key}")

literal = re.compile(
    r"""(?:\bText|\bTooltip|\b(?:labelText|hintText|tooltip|semanticLabel|title)\s*:)\s*\(?\s*(['"])(?!\1)""",
)
for path in sources:
    for number, line in enumerate(path.read_text(encoding="utf-8").splitlines(), start=1):
        if literal.search(line):
            errors.append(f"Hardcoded user-facing text: {path.relative_to(front)}:{number}: {line.strip()}")

for error in errors:
    print(error)
print("Localization check passed." if not errors else f"{len(errors)} localization problem(s).")
sys.exit(1 if errors else 0)
