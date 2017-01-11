#!/bin/bash

nuget restore ReadingList.sln
xbuild /property:Configuration=Release ReadingList.sln
