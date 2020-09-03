#!/bin/bash

dotnet restore ReadingList.sln
dotnet build --configuration Release ReadingList.sln
