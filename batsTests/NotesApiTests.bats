#!/usr/bin/env bats

readingListUrl="http://readinglist:1337/api/notes"

@test "GET: Random endpoint should return random book note with non-empty title attribute" {
    result="$(curl -s $readingListUrl/random | jq '.title' --raw-output)"
    echo $result
    [ ! -z "$result" ]
}

@test "GET: Random endpoint should return random book note with non-empty note attribute" {
    result="$(curl -s $readingListUrl/random | jq '.note' --raw-output)"
    echo $result
    [ ! -z "$result" ]
}

@test "GET: Random endpoint should return random book note with non-empty authors attribute" {
    result="$(curl -s $readingListUrl/random | jq '.authors[0]' --raw-output)"
    echo $result
    [ ! -z "$result" ]
}

function teardown {
  echo "Teardown: result value was $result"
}