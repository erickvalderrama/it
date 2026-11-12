#!/usr/bin/env sh
# Creates (or updates the password of) a project role and its own database in the
# shared postgres-local container. Usage:
#   sh tools/create-local-database.sh <database> <role> <password>
set -eu

database="$1"
role="$2"
password="$3"

docker exec -i postgres-local psql -v ON_ERROR_STOP=1 -U postgres \
  -v db="$database" -v role="$role" -v pw="$password" <<'SQL'
SELECT format('CREATE ROLE %I LOGIN PASSWORD %L', :'role', :'pw')
WHERE NOT EXISTS (SELECT FROM pg_roles WHERE rolname = :'role') \gexec
SELECT format('ALTER ROLE %I PASSWORD %L', :'role', :'pw') \gexec
SELECT format('CREATE DATABASE %I OWNER %I', :'db', :'role')
WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = :'db') \gexec
SELECT format('REVOKE ALL ON DATABASE %I FROM PUBLIC', :'db') \gexec
\connect :db
CREATE EXTENSION IF NOT EXISTS postgis;
SQL
echo "Database '$database' ready for role '$role'."
