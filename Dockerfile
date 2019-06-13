FROM mono:latest
LABEL maintainer="Andreas Mosti(andreas.mosti[at]gmail.com)"
ENV confd_version 0.14.0

COPY docker-entrypoint.sh docker-entrypoint.sh
COPY ReadingList.sln ReadingList.sln
COPY ReadingList ReadingList

RUN nuget restore ReadingList.sln
RUN msbuild /property:Configuration=Release ReadingList.sln

RUN curl -L https://github.com/kelseyhightower/confd/releases/download/v${confd_version}/confd-${confd_version}-linux-amd64 -o /usr/local/bin/confd \
    && chmod +x /usr/local/bin/confd \
    && apt-get remove \
        -y curl nuget fsharp mono-vbnc \
    && rm -rf /var/lib/apt/lists/*

COPY confd /etc/confd

EXPOSE 1337

ENTRYPOINT ["./docker-entrypoint.sh"]
