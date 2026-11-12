#!/usr/bin/env sh
# Fails when an authored source file reaches 700 lines (the project maximum is 699).
set -eu

MAX_LINES=699
root="$(cd "$(dirname "$0")/.." && pwd)"
status=0

files=$(find "$root/back" "$root/front" \
  \( -path '*/bin' -o -path '*/obj' -o -path '*/build' -o -path '*/.dart_tool' \
     -o -path '*/ios/Pods' -o -path '*/l10n/app_localizations*.dart' \) -prune -o \
  -type f \( -name '*.cs' -o -name '*.dart' -o -name '*.csproj' -o -name '*.props' \) -print)

for file in $files; do
  case "$file" in
    */l10n/app_localizations*.dart) continue ;;
  esac
  lines=$(wc -l < "$file")
  if [ "$lines" -gt "$MAX_LINES" ]; then
    echo "Too long ($lines lines, max $MAX_LINES): ${file#"$root"/}"
    status=1
  fi
done

[ "$status" -eq 0 ] && echo "All source files have $MAX_LINES lines or fewer."
exit "$status"
