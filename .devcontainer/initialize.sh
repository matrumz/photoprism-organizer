#!/usr/bin/env bash
set -exo pipefail

docker compose --file compose.dev.yaml run --rm devcontainer-setup
