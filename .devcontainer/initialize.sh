#!/usr/bin/env bash
set -exo pipefail

function random-string() {
  # Generate a random string of the specified length (default: 16)
  local length=${1:-16}
  tr -dc 'a-zA-Z0-9' < /dev/urandom | head -c $length || true
}

touch .env

# If PHOTOPRISM_DATABASE_PASSWORD is not set in the .env file: generate a random password and set in .env
if ! grep -q -e "^PHOTOPRISM_DATABASE_PASSWORD=" .env; then
  PHOTOPRISM_DATABASE_PASSWORD=$(random-string 32)
  echo "PHOTOPRISM_DATABASE_PASSWORD=$PHOTOPRISM_DATABASE_PASSWORD" >> .env
fi

# If MARIADB_ROOT_PASSWORD is not set in the .env file: generate a random password and set in .env
if ! grep -q -e "^MARIADB_ROOT_PASSWORD=" .env; then
  MARIADB_ROOT_PASSWORD=$(random-string 32)
  echo "MARIADB_ROOT_PASSWORD=$MARIADB_ROOT_PASSWORD" >> .env
fi

docker compose --file compose.dev.yaml run --rm devcontainer-setup
