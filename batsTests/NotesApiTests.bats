#!/usr/bin/env bats

readingListUrl="${1:-http://readinglist:1337}"

@test "GET: Random endpoint should return random book note with non-empty title attribute" {
    result="$(curl -s $readingListUrl/api/notes/random | jq '.title' --raw-output)"
    echo $result
    [ ! -z "$result" ]
}

@test "GET: Random endpoint should return random book note with non-empty note attribute" {
    result="$(curl -s $readingListUrl/api/notes/random | jq '.note' --raw-output)"
    echo $result
    [ ! -z "$result" ]
}

@test "GET: Random endpoint should return random book note with non-empty authors attribute" {
    result="$(curl -s $readingListUrl/api/notes/random | jq '.authors[0]' --raw-output)"
    echo $result
    [ ! -z "$result" ]
}

@test "GET: All endpoint should return array of book notes with non-empty title in first element" {
    result="$(curl -s $readingListUrl/api/notes/all | jq '.[0].title' --raw-output)"
    echo $result
    [ ! -z "$result" ]
}

@test "GET: Book endpoint should return correct book notes with given title" {
    result="$(curl -s $readingListUrl/api/notes/book?title=why.we.sleep | jq '.authors[0]' --raw-output)"
    echo $result
    [ "$result" == "Matthew Walker" ]
}

@test "GET: Book endpoint should return correct book notes with enough part of given title" {
    result="$(curl -s $readingListUrl/api/notes/book?title=why.we.sl | jq '.authors[0]' --raw-output)"
    echo $result
    [ "$result" == "Matthew Walker" ]
}

function teardown {
  echo "Teardown: result value was $result"
}