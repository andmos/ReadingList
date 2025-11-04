# ReadingList
Simple service proxy for my Trello-powered readinglist.

Simple build with Docker: `docker run -v $(pwd):/workspace -w "/workspace" -it mcr.microsoft.com/dotnet/sdk:9.0-alpine ./build.sh`

### Running API tests:

```shell
docker run --name readinglist -dt -p 1337:1337 -e TrelloAuthSettings__TrelloAPIKey=$TrelloAPIKey -e TrelloAuthSettings__TrelloUserToken=$TrelloUserToken andmos/readinglist

docker run --link readinglist:readinglist --rm -e TrelloAPIKey=$TrelloAPIKey -e TrelloUserToken=$TrelloUserToken -v $(pwd):/app graze/bats /app/batsTests
```

[![CI / CD](https://github.com/andmos/ReadingList/actions/workflows/ci.yaml/badge.svg)](https://github.com/andmos/ReadingList/actions/workflows/ci.yaml)
