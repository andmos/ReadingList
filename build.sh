#!/bin/sh

dotnet restore ReadingList.sln
dotnet build --configuration Release ReadingList.sln
