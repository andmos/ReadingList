#! /bin/bash

binary=/ReadingList/bin/Release/ReadingList.exe
configFile=/ReadingList/bin/Release/ReadingList.exe.config

echo "$APIKey"

sed -i -e "s/\<APIKey\>/$APIKey/g" $configFile
sed -i -e "s/\<UserToken\>/$UserToken/g" $configFile
sed -i -e "s/\<Url\>/$HostURL/g" $configFile 

mono $binary
