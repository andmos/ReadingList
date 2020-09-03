# ReadingList
Simple service proxy for my Trello-powered readinglist.

Simple build with Docker: `docker run -v $(pwd):/workspace -w "/workspace" -it mcr.microsoft.com/dotnet/core/sdk:3.1 ./build.sh`

### Running API tests:

```shell
docker run --name readinglist -dt -p 1337:1337 -e TrelloAuthSettings__TrelloAPIKey=$TrelloAPIKey -e TrelloAuthSettings__TrelloUserToken=$TrelloUserToken andmos/readinglist

docker run --link readinglist:readinglist --rm -e TrelloAPIKey=$TrelloAPIKey -e TrelloUserToken=$TrelloUserToken -v $(pwd):/app graze/bats /app/batsTests
```

[![Build Status](https://travis-ci.com/andmos/ReadingList.svg?branch=master)](https://travis-ci.com/andmos/ReadingList)
[![Docker Project](https://img.shields.io/docker/pulls/andmos/readinglist.svg)](https://hub.docker.com/r/andmos/readinglist/)
[![Dependabot Status](https://api.dependabot.com/badges/status?host=github&repo=andmos/ReadingList)](https://dependabot.com)
