#! /bin/bash

set -eo pipefail

if [ -z "${TrelloAPIKey}" ]; then
    echo "TrelloAPIKey env variable not set"
    exit 1
fi

if [ -z "${TrelloUserToken}" ]; then
    echo "TrelloUserToken env variable not set"
    exit 1
fi

export GIT_REV=$(git rev-parse --short HEAD)

function cleanup {
    echo "cleaning up containers"
    docker stop readinglist
    echo "cleanup completed"
}

trap cleanup EXIT

echo "Building readinglist image with git rev $GIT_REV"
docker build -t andmos/readinglist:$(git rev-parse --short HEAD) .

echo "Running container structure tests on readinglist image"
docker run --rm -v /var/run/docker.sock:/var/run/docker.sock -v $(pwd):/tests gcr.io/gcp-runtimes/container-structure-test:v1.15.0 test --image andmos/readinglist:$GIT_REV --config /tests/imageTests/readinglist_container_tests_config.yaml

echo "Starting readinglist image and running bats tests"
docker run --rm --name readinglist -dt -p 1337:1337 -e TrelloAuthSettings__TrelloAPIKey=$TrelloAPIKey -e TrelloAuthSettings__TrelloUserToken=$TrelloUserToken andmos/readinglist:$GIT_REV
sleep 7
docker run --link readinglist:readinglist --rm -e TrelloAPIKey=$TrelloAPIKey -e TrelloUserToken=$TrelloUserToken -v $(pwd):/app graze/bats /app/batsTests

