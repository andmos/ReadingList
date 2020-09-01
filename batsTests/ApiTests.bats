#!/usr/bin/env bats

readingListUrl="http://readinglist:1337"

@test "GET: openapi endpoint should be HTTP Statuscode 200" {
    result="$(curl -s -o /dev/null -w '%{http_code}' $readingListUrl/openapi/)"
    [ "$result" -eq 200 ]
}

@test "GET: ping endpoint should return pong" {
    result="$(curl -s $readingListUrl/api/ping)"
    [ "$result" = '"pong"' ]
}

@test "GET: backlogList endpoint should return JSON element with title attribute" {
    result="$(curl -s $readingListUrl/api/backlogList | jq '.[0].title')"
    [ "$result" != "null" ]
}

@test "GET: backlogList endpoint should return JSON element with authors attribute" {
    result="$(curl -s $readingListUrl/api/backlogList | jq '.[0].authors')"
    [ "$result" != "null" ]
}

@test "GET: backlogList endpoint should return more than 5 JSON elements" {
    result="$(curl -s $readingListUrl/api/backlogList | jq '. | length')"
    [ "$result" -gt 5 ]
}

@test "GET: doneList endpoint should return JSON element with title attribute" {
    result="$(curl -s $readingListUrl/api/doneList | jq '.[0].title')"
    [ "$result" != "null" ]
}

@test "GET: doneList endpoint should return JSON element with authors attribute" {
    result="$(curl -s $readingListUrl/api/doneList | jq '.[0].authors')"
    [ "$result" != "null" ]
}

@test "GET: doneList endpoint with 'fact' label should return JSON element with authors attribute" {
    result="$(curl -s $readingListUrl/api/doneList?label=fact | jq '.[0].authors')"
    [ "$result" != "null" ]
}


@test "GET: doneList endpoint should return JSON containing the title 'Inferno' with label 'fiction'" {
    result="$(curl -s $readingListUrl/api/doneList | jq '.[] | select(.title=="Inferno") | .label')"
    [ "$result" == '"fiction"' ]
}
@test "GET: doneList endpoint should return JSON containing the title 'Promise of the Witch-King' with label 'fiction' and '-' character in name" {
    result="$(curl -s $readingListUrl/api/doneList | jq '.[] | select(.title=="Promise of the Witch-King: Forgotten Realms: The Sellswords, Book 2") | .label')"
    [ "$result" == '"fiction"' ]
}

@test "GET: allLists endpoint should return JSON with 'done' element containing book entry" {
    result="$(curl -s $readingListUrl/api/allLists | jq '.readingLists.Done | .[1]')"
    [ "$result" != "null" ]
}

@test "GET: allList endpoint should only return JSON with 'fact' books when 'fact' label is passed" {
    result="$(curl -s $readingListUrl/api/allLists?label=fact | jq '.readingLists.Done[1].label')"
    [ "$result" == '"fact"' ]
}

@test "GET: allList endpoint should only return JSON with 'fiction' books when 'fiction' label is passed" {
    result="$(curl -s $readingListUrl/api/allLists?label=fiction | jq '.readingLists.Done[1].label')"
    [ "$result" == '"fiction"' ]
}

@test "POST: backlogList endpoint should return FORBIDDEN request is done without correct APIKey and UserToken in header" {
    skip "exitcode is correct, but throws status 18 from curl for some reason"
    result="$(curl -s -o /dev/null -w '%{http_code}' -X POST "$readingListUrl/api/backlogList?author=Test%20Author&title=Test%20Title&label=fact" -H 'cache-control: no-cache' -H 'content-type: application/json' -H 'TrelloAPIKey: 77777777' -H 'TrelloUserToken: 7777777' -d '{ "data": "this is my new testdata from a POST" }')"
    [ "$result" -eq 403 ]
}

@test "PUT: doneList endpoint should return 'false' if request is done without booktitle present in ReadingList" {
    result="$(curl -s -X PUT "$readingListUrl/api/doneList?title=Test%20Title" -H 'cache-control: no-cache' -H 'content-type: application/json' -H "TrelloAPIKey:$TrelloAPIKey"  -H "TrelloUserToken:$TrelloUserToken" -d '{ "data": "this is my new testdata from a PUT" }')"
    [ "$result" == false ]
}

@test "HEAD: Callback setup endpoint should be HTTP Statuscode 200" {
    result="$(curl -s -o /dev/null --head -w '%{http_code}' $readingListUrl/api/callBack/)"
    [ "$result" -eq 200 ]
}

function teardown {
  echo "Teardown: result value was $result"
}
