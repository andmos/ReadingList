#!/bin/bash

nuget restore ReadingList.sln
msbuild /property:Configuration=Release ReadingList.sln

