# ReadingList
Simple service abstraction for my Trello-powered readinglist.

Simple build with Docker: `docker run -v $(pwd):/workspace -w "/workspace" -it mono ./build.sh`

### Running API tests:
```
docker run --name readinglist -dt -p 1337:1337 -e APIKey=$TrelloAPIKey -e UserToken=$TrelloUserToken andmos/readinglist
docker run --link readinglist:readlinglist --rm -v $(pwd):/app graze/bats /app/batsTests
```

[![Build Status](https://travis-ci.org/andmos/ReadingList.svg?branch=master)](https://travis-ci.org/andmos/ReadingList)
