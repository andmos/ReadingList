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

@test "GET: All endpoint should return array of book notes with non-empty title in first element" {
    result="$(curl -s $readingListUrl/all | jq '.[0].title' --raw-output)"
    echo $result
    [ ! -z "$result" ]
}

@test "GET: Book endpoint should return correct book notes with given title" {
    result="$(curl -s $readingListUrl/book?title=why.we.sleep | jq '.authors[0]' --raw-output)"
    echo $result
    [ "$result" == "Matthew Walker" ]
}

function teardown {
  echo "Teardown: result value was $result"
}