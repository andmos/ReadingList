#! /bin/bash
set -e
binary=/ReadingList/bin/Release/ReadingList.exe

test $APIKey
test $UserToken

/usr/local/bin/confd -onetime -backend env

mono $binary
