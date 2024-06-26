FROM mcr.microsoft.com/dotnet/sdk:8.0.301-alpine3.18 AS builder

WORKDIR /app

COPY ReadingList.sln ReadingList.sln
COPY ReadingList.Logic ReadingList.Logic
COPY ReadingList.Trello ReadingList.Trello
COPY ReadingList.Logging ReadingList.Logging
COPY ReadingList.Notes.Logic ReadingList.Notes.Logic
COPY ReadingList.Notes.Github ReadingList.Notes.Github
COPY ReadingList.Carter ReadingList.Carter

WORKDIR /app/ReadingList.Carter

RUN dotnet restore
RUN dotnet publish ReadingList.Carter.csproj -c Release -o ../publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0.6-alpine3.18 AS runtime

LABEL org.opencontainers.image.source="https://github.com/andmos/ReadingList"
LABEL maintainer="Andreas Mosti(andreas.mosti[at]gmail.com)"

ENV PORT 1337
ENV ASPNETCORE_URLS=http://+:$PORT

WORKDIR /app

COPY --from=builder /app/publish .

EXPOSE $PORT

ENTRYPOINT ["dotnet", "ReadingList.Carter.dll"]
