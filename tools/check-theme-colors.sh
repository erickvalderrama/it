#!/usr/bin/env sh
# Fails when Flutter code outside the theme defines colors directly.
# Widgets must read colors from the shared theme tokens (lib/app/theme).
set -eu

root="$(cd "$(dirname "$0")/.." && pwd)"
matches=$(grep -rnE 'Color\(0x|Colors\.[a-z]' "$root/front/lib" \
  --include='*.dart' \
  | grep -v "/front/lib/app/theme/" \
  | grep -v "/front/lib/l10n/" || true)

if [ -n "$matches" ]; then
  echo "Hardcoded colors found outside lib/app/theme:"
  echo "$matches" | sed "s#$root/##"
  exit 1
fi
echo "No hardcoded colors outside the theme."
