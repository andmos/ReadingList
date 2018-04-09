#!/usr/bin/env bats

@test "ping endpoint should return pong" {
    result="$(curl -s http://readinglist:1337/api/ping)"
    [ "$result" = "pong" ]
}

@test "backlogList endpoint should return JSON element with title attribute" {
    result="$(curl -s http:/readinglist:1337/api/backlogList | jq '.[0].title')"
    [ "$result" != "null" ]
}

@test "backlogList endpoint should return JSON element with authors attribute" {
    result="$(curl -s http://readinglist:1337/api/backlogList | jq '.[0].authors')"
    [ "$result" != "null" ]
}