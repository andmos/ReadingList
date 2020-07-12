#! /bin/bash

set -e

docker build -t andmos/readinglist:$(git rev-parse --short HEAD) .
docker run -v /var/run/docker.sock:/var/run/docker.sock -v $(pwd):/tests gcr.io/gcp-runtimes/container-structure-test:v1.8.0 test --image andmos/readinglist:$(git rev-parse --short HEAD) --config /tests/imageTests/readinglist_container_tests_config.yaml
docker run --name readinglist -dt -p 1337:1337 -e TrelloAuthSettings__TrelloAPIKey=$TrelloAPIKey -e TrelloAuthSettings__TrelloUserToken=$TrelloUserToken andmos/readinglist:$(git rev-parse --short HEAD)
sleep 5
docker run --link readinglist:readinglist --rm -e TrelloAuthSettings__TrelloAPIKey=$TrelloAPIKey -e TrelloAuthSettings__TrelloUserToken=$TrelloUserToken -v $(pwd):/app graze/bats /app/batsTests
docker stop readinglist
docker rm readinglist
