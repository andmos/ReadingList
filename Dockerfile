FROM mono:latest
MAINTAINER Andreas Mosti (andreas.mosti[at]gmail.com)

ADD docker-entrypoint.sh docker-entrypoint.sh
ADD ReadingList.sln ReadingList.sln
ADD ReadingList ReadingList

RUN nuget restore ReadingList.sln
RUN xbuild /property:Configuration=Release ReadingList.sln

ADD https://github.com/kelseyhightower/confd/releases/download/v0.11.0/confd-0.11.0-linux-amd64 /usr/local/bin/confd
RUN chmod +x /usr/local/bin/confd
ADD confd /etc/confd

expose 1337

ENTRYPOINT ["./docker-entrypoint.sh"]
