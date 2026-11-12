# Petyei

Pet adoption platform for Colombia. Internal project name: Mascotas.

| Folder | Content |
| --- | --- |
| `back/` | ASP.NET Core backend (.NET 10): Domain, Application, Infrastructure, API, and tests. |
| `front/` | Flutter app for web, Android, and iOS (Flutter 3.41, Riverpod). |
| `documents/` | Specification, work plan, legal drafts, brand, and research. |

Project rules: [CLAUDE.md](CLAUDE.md). Active plan: [work-plan-phase-1.md](documents/prompts/mascotas/work-plan-phase-1.md).

## Backend

```sh
cd back
dotnet build
dotnet test
dotnet run --project src/Petyei.Api   # http://localhost:5xxx, see launchSettings.json
```

The API reads enabled countries from `Countries:Enabled` and browser origins from `Cors:AllowedOrigins`.

## Frontend

```sh
cd front
flutter pub get
flutter gen-l10n
flutter analyze
flutter test
flutter run -d chrome --dart-define=API_BASE_URL=http://localhost:5180
```

User-facing text lives in `lib/l10n/app_en.arb` (template) and `lib/l10n/app_es.arb`.

## Containers

```sh
cp .env.example .env    # adjust per environment
docker compose build
docker compose up -d    # API on :5180, web on :5181
docker compose ps
```

## Checks

```sh
sh tools/check-file-length.sh   # every source file must have 699 lines or fewer
```
