FROM mono:latest
LABEL maintainer="Andreas Mosti(andreas.mosti[at]gmail.com)"

COPY docker-entrypoint.sh docker-entrypoint.sh
COPY ReadingList.sln ReadingList.sln
COPY ReadingList ReadingList

RUN nuget restore ReadingList.sln
RUN msbuild /property:Configuration=Release ReadingList.sln

ADD https://github.com/kelseyhightower/confd/releases/download/v0.14.0/confd-0.14.0-linux-amd64 /usr/local/bin/confd
RUN chmod +x /usr/local/bin/confd
COPY confd /etc/confd

RUN apt-get remove \
    -y curl nuget fsharp mono-vbnc \
     && rm -rf /var/lib/apt/lists/*

EXPOSE 1337

ENTRYPOINT ["./docker-entrypoint.sh"]
