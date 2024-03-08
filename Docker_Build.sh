#!/bin/sh

CONTAINER_NAME="config_map_test"

NUGET_SOURCE_PUBLIC="https://api.nuget.org/v3/index.json"

BUILD_DIR="."
DOCKERFILE="Dockerfile"

echo "docker build ..."

docker build -f "$DOCKERFILE" "$BUILD_DIR" \
	--build-arg NUGET_SOURCE_PUBLIC=$NUGET_SOURCE_PUBLIC \
	--build-arg VERSION_ASSEMBLY="0.0.0" \
	--build-arg VERSION_INFORMATIONAL="0.0.0" \
	-t "${CONTAINER_NAME}:latest" \
	2>&1

EXIT_CODE=$?
if [ $EXIT_CODE -ne 0 ]; then
	echo "BUILD FAILED !!"
	exit $EXIT_CODE
fi

echo "docker build finished"