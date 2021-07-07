#! /usr/bin/env bash

docker buildx build --platform=linux/amd64 --build-arg GIT_COMMIT=$(git log -1 --format=%h) -t nethermindeth/hive .
