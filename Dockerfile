FROM mono:latest
ADD ReadingList.sln ReadingList.sln 
ADD ReadingList ReadingList

RUN nuget restore ReadingList.sln
RUN xbuild /property:Configuration=Release ReadingList.sln 

expose 1337

CMD mono /ReadingList/bin/Release/ReadingList.exe
