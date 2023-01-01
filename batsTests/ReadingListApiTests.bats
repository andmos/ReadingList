#!/usr/bin/env bats

readingListUrl="${1:-http://readinglist:1337}"

 @test "GET: Swagger endpoint should be HTTP Statuscode 200" {
     result="$(curl -s -o /dev/null -w '%{http_code}' $readingListUrl/swagger/v1/swagger.json)"
     [ "$result" -eq 200 ]
 }

@test "GET: Swagger UI endpoint should be HTTP Statuscode 200 after redirect" {
    result="$(curl -L -s -o /dev/null -w '%{http_code}' $readingListUrl/openapi/ui)"
    [ "$result" -eq 200 ]
}

@test "GET: Swagger UI endpoint should contain ReadingList as title element" {
    title="<title>ReadingList</title>"
    result="$(curl -L -s $readingListUrl/openapi/ui | grep $title | xargs)"
    [ "$result" = "$title" ]
}

@test "GET: ping endpoint should return pong" {
    result="$(curl -s $readingListUrl/api/ping )"
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
    result="$(curl -s $readingListUrl/api/doneList | jq '.[] | select(.title=="Inferno") | .label' --raw-output)"
    [ "$result" == "Fiction" ]
}
@test "GET: doneList endpoint should return JSON containing the title 'Promise of the Witch-King' with label 'fiction' and '-' character in name" {
    result="$(curl -s $readingListUrl/api/doneList | jq '.[] | select(.title=="Promise of the Witch-King: Forgotten Realms: The Sellswords, Book 2") | .label')"
    [ "$result" == '"Fiction"' ]
}

@test "GET: doneList endpoint should return the first book in the list as JSON containing the title 'Trist Som Faen' with label 'fiction'" {
    result="$(curl -s $readingListUrl/api/doneList | jq '.[] | select(.title=="Trist Som Faen") | .label')"
    [ "$result" == '"Fiction"' ]
}

@test "GET: allLists endpoint should return JSON with 'done' element containing book entry" {
    result="$(curl -s $readingListUrl/api/allLists | jq '.readingLists.Done | .[1]')"
    [ "$result" != "null" ]
}

@test "GET: allList endpoint should only return JSON with 'Fact' books when 'fact' label is passed" {
    result="$(curl -s $readingListUrl/api/allLists?label=fact | jq '.readingLists.Done[1].label' --raw-output)"
    [ "$result" == "Fact" ]
}

@test "GET: allList endpoint should only return JSON with 'Fiction' books when 'fiction' label is passed" {
    result="$(curl -s $readingListUrl/api/allLists?label=fiction | jq '.readingLists.Done[1].label'  --raw-output)"
    [ "$result" == "Fiction" ]
}

@test "GET: health endpoint should return TrelloHelthCheck" {
    result="$(curl -s $readingListUrl/health | jq '.checks[0].service' --raw-output)"
    [ "$result" == "TrelloHealthCheck" ]
}

@test "POST: backlogList endpoint should return 401 UNAUTHORIZED request is done without correct APIKey and UserToken in header" {
    result="$(curl -s -o /dev/null -w '%{http_code}' -X POST "$readingListUrl/api/backlogList?author=Test%20Author&title=Test%20Title&label=fact" -H 'cache-control: no-cache' -H 'content-type: application/json' -H 'TrelloAPIKey: 77777777' -H 'TrelloUserToken: 7777777' -d '{ "data": "this is my new testdata from a POST" }')"
    [ "$result" -eq 401 ]
}

@test "PUT: doneList endpoint should return 401 UNAUTHORIZED request is done without correct APIKey and UserToken in header" {
    result="$(curl -s -o /dev/null -w '%{http_code}' -X PUT "$readingListUrl/api/doneList?title=Test%20Title" -H 'cache-control: no-cache' -H 'content-type: application/json' -H 'TrelloAPIKey: 77777777' -H 'TrelloUserToken: 7777777' -d '{ "data": "this is my new testdata from a POST" }')"
    [ "$result" -eq 401 ]
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
