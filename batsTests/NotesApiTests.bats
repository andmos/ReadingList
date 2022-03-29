#!/usr/bin/env bats

readingListUrl="http://readinglist:1337/api/notes"

@test "GET: Random endpoint should return random book note" {
    result="$(curl -s $readingListUrl/random | jq '.title' --raw-output)"
    echo $result
    [ ! -z "$result" ]
}

function teardown {
  echo "Teardown: result value was $result"
}