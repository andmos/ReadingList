#! /bin/bash

set -e

export GIT_REV=$(git rev-parse --short HEAD)

cleanup(){
    echo "cleaning up containers"
    docker stop readinglist
    echo "cleanup completed"
}

trap "cleanup" ERR

echo "Building readinglist image with git rev $GIT_REV"
docker build -t andmos/readinglist:$(git rev-parse --short HEAD) .

echo "Running container structure tests on readinglist image"
docker run --rm -v /var/run/docker.sock:/var/run/docker.sock -v $(pwd):/tests gcr.io/gcp-runtimes/container-structure-test:v1.8.0 test --image andmos/readinglist:$GIT_REV --config /tests/imageTests/readinglist_container_tests_config.yaml

echo "Starting readinglist image and running bats tests"
docker run --rm --name readinglist -dt -p 1337:1337 -e TrelloAuthSettings__TrelloAPIKey=$TrelloAPIKey -e TrelloAuthSettings__TrelloUserToken=$TrelloUserToken andmos/readinglist:$GIT_REV
sleep 5
docker run --link readinglist:readinglist --rm -e TrelloAPIKey=$TrelloAPIKey -e TrelloUserToken=$TrelloUserToken -v $(pwd):/app graze/bats /app/batsTests


cleanup