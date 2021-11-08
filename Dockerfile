FROM mcr.microsoft.com/dotnet/sdk:5.0.402-alpine3.13 AS builder
LABEL maintainer="Andreas Mosti(andreas.mosti[at]gmail.com)"

WORKDIR /app

COPY ReadingList.sln ReadingList.sln
COPY ReadingList.Logic ReadingList.Logic
COPY ReadingList.Trello ReadingList.Trello
COPY ReadingList.Logging ReadingList.Logging
COPY ReadingList.Carter ReadingList.Carter

WORKDIR /app/ReadingList.Carter

RUN dotnet restore
RUN dotnet publish ReadingList.Carter.csproj -c Release -o ../publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine3.13 AS runtime
ENV PORT 1337
ENV ASPNETCORE_URLS=http://+:$PORT

WORKDIR /app

COPY --from=builder /app/publish .

EXPOSE $PORT

ENTRYPOINT ["dotnet", "ReadingList.Carter.dll"]
