#!/usr/bin/env bats

readingListUrl="http://readinglist:1337"

@test "ping endpoint should return pong" {
    result="$(curl -s $readingListUrl/api/ping)"
    [ "$result" = "pong" ]
}

@test "backlogList endpoint should return JSON element with title attribute" {
    result="$(curl -s $readingListUrl/api/backlogList | jq '.[0].title')"
    [ "$result" != "null" ]
}

@test "backlogList endpoint should return JSON element with authors attribute" {
    result="$(curl -s $readingListUrl/api/backlogList | jq '.[0].authors')"
    [ "$result" != "null" ]
}

@test "doneList endpoint should return JSON element with title attribute" {
    result="$(curl -s $readingListUrl/api/doneList | jq '.[0].title')"
    [ "$result" != "null" ]
}

@test "doneList endpoint should return JSON element with authors attribute" {
    result="$(curl -s $readingListUrl/api/doneList | jq '.[0].authors')"
    [ "$result" != "null" ]
}

@test "doneList endpoint with 'fact' label should return JSON element with authors attribute" {
    result="$(curl -s $readingListUrl/api/doneList?label=fact | jq '.[0].authors')"
    [ "$result" != "null" ]
}

@test "doneList endpoint should return JSON containing the title 'Inferno' with label 'fiction'" {
    result="$(curl -s $readingListUrl/api/doneList | jq '.[] | select(.title=="Inferno") | .label')"
    [ "$result" == '"fiction"' ]
}

@test "allLists endpoint should return JSON with 'done' element containing book entry" {
    result="$(curl -s $readingListUrl/api/allLists | jq '.readingLists.done | .[1]')"
    [ "$result" != "null" ]
}
