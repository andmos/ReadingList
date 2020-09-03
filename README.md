# ReadingList
Simple service proxy for my Trello-powered readinglist.

Simple build with Docker: `docker run -v $(pwd):/workspace -w "/workspace" -it mono ./build.sh`

### Running API tests:
```
docker run --name readinglist -dt -p 1337:1337 -e APIKey=$TrelloAPIKey -e UserToken=$TrelloUserToken andmos/readinglist
docker run --link readinglist:readlinglist --rm -e APIKey=$TrelloAPIKey -e UserToken=$TrelloUserToken -v $(pwd):/app graze/bats /app/batsTests
```

[![Build Status](https://travis-ci.com/andmos/ReadingList.svg?branch=master)](https://travis-ci.com/andmos/ReadingList)
[![Docker Project](https://img.shields.io/docker/pulls/andmos/readinglist.svg)](https://hub.docker.com/r/andmos/readinglist/)
[![Dependabot Status](https://api.dependabot.com/badges/status?host=github&repo=andmos/ReadingList)](https://dependabot.com)