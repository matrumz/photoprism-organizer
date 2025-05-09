# syntax=docker/dockerfile:1

ARG DOTNET_VERSION=9.0

#### SDK #######################################################################
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS sdk

#### Runtime ###################################################################
FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION} AS runtime

#### Devcontainer ##############################################################
FROM sdk AS devcontainer

# Dev tools
RUN <<DOCKERFILE_EOF
#!/usr/bin/env bash
export DEBIAN_FRONTEND=noninteractive
set -ex
apt-get update
apt-get install --yes --no-install-recommends \
    fish \
    git \
    openssh-client \
    sudo \
    vim
DOCKERFILE_EOF

# Create devcontainer user
ARG DEVCONTAINER_USER_NAME=dev
ARG DEVCONTAINER_USER_UID=1000
ARG DEVCONTAINER_USER_GID=${DEVCONTAINER_USER_UID}
RUN <<DOCKERFILE_EOF
#!/usr/bin/env bash
export DEBIAN_FRONTEND=noninteractive
set -ex
groupadd --gid $DEVCONTAINER_USER_GID $DEVCONTAINER_USER_NAME
groupadd --force docker
useradd --uid $DEVCONTAINER_USER_UID --gid $DEVCONTAINER_USER_GID --groups docker --shell /usr/bin/fish --create-home $DEVCONTAINER_USER_NAME
echo "$DEVCONTAINER_USER_NAME ALL=(ALL) NOPASSWD: ALL" > /etc/sudoers.d/$DEVCONTAINER_USER_NAME
chmod 0440 /etc/sudoers.d/$DEVCONTAINER_USER_NAME
DOCKERFILE_EOF
USER ${DEVCONTAINER_USER_NAME}
