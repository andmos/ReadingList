FROM mono:latest
MAINTAINER Andreas Mosti (andreas.mosti[at]gmail.com)

ADD docker-entrypoint.sh docker-entrypoint.sh 
ADD ReadingList.sln ReadingList.sln 
ADD ReadingList ReadingList

RUN nuget restore ReadingList.sln
RUN xbuild /property:Configuration=Release ReadingList.sln 

expose 1337

ENTRYPOINT ["./docker-entrypoint.sh"]
