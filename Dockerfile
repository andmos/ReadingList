FROM mcr.microsoft.com/dotnet/core/sdk:3.1
LABEL maintainer="Andreas Mosti(andreas.mosti[at]gmail.com)"
ENV confd_version 0.14.0
ENV PORT 5000

COPY docker-entrypoint.sh docker-entrypoint.sh
COPY ReadingList.sln ReadingList.sln
COPY ReadingList ReadingList
COPY ReadingList.Logic ReadingList.Logic
COPY ReadingList.Trello ReadingList.Trello
COPY ReadingList.Logging ReadingList.Logging
COPY ReadingList.Carter ReadingList.Carter

RUN dotnet restore
RUN dotnet build ReadingList.Carter

RUN curl -L https://github.com/kelseyhightower/confd/releases/download/v${confd_version}/confd-${confd_version}-linux-amd64 -o /usr/local/bin/confd \
    && chmod +x /usr/local/bin/confd \
    && apt-get remove \
    && rm -rf /var/lib/apt/lists/*

COPY confd /etc/confd

EXPOSE $PORT

ENTRYPOINT ["dotnet", "ReadingList.Carter/bin/Release/netcoreapp3.1/ReadingList.Carter.dll"]
