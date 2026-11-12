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

### Local database

`compose.postgres-local.yaml` runs a shared PostgreSQL 17 + PostGIS server for local development (container `postgres-local`, port 5433, volume `postgres-local-data`). Each project gets its own database and role.

```sh
cp .env.example .env          # set the passwords; .env is ignored by git
docker compose -f compose.postgres-local.yaml up -d
sh tools/create-local-database.sh petyei_local petyei <PETYEI_DB_PASSWORD>
cd back
dotnet user-secrets set "ConnectionStrings:Petyei" "Host=localhost;Port=5433;Database=petyei_local;Username=petyei;Password=<PETYEI_DB_PASSWORD>" --project src/Petyei.Api
PETYEI_DESIGN_CONNECTION="<same connection string>" dotnet ef database update --project src/Petyei.Infrastructure --startup-project src/Petyei.Infrastructure
```

`docker compose -f compose.postgres-local.yaml down` keeps the data; add `-v` only to delete the volume.

### Database migrations

Schema changes use EF Core migrations only (no manual DDL). Tools are pinned in `back/dotnet-tools.json`.

```sh
cd back
dotnet tool restore
# Create a migration after changing the model
dotnet ef migrations add <Name> --project src/Petyei.Infrastructure --startup-project src/Petyei.Infrastructure --output-dir Persistence/Migrations
# Script to review and apply on a server (safe to run more than once)
dotnet ef migrations script --idempotent --project src/Petyei.Infrastructure --startup-project src/Petyei.Infrastructure -o migrate.sql
```

Apply migrations only to the authorized target. The API reads `ConnectionStrings:Petyei`.

### Catalog import

Loads DIVIPOLA locations and the bilingual species and breeds from `back/data/catalogs` (versions in `manifest.json`). Validates every file first, updates records by code, never deletes, and can run again safely.

```sh
cd back
ASPNETCORE_ENVIRONMENT=Development dotnet run --project src/Petyei.Api --no-launch-profile -- import-catalogs --data "$(pwd)/data/catalogs"
# In the container: dotnet Petyei.Api.dll import-catalogs --data /app/data/catalogs
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
