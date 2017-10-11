#!/bin/bash
SERVER_ADDRESS=139.59.157.1
FRAMEWORK_VERSION=netcoreapp2.0
CONFIGURATION=Release
RUNTIME=ubuntu-x64

# deploy api
#ssh arasz@$SERVER_ADDRESS systemctl stop kestrel-nacodokinaapi.service


dotnet publish src/NaCoDoKina.Api/NaCoDoKina.Api.csproj -c $CONFIGURATION -f $FRAMEWORK_VERSION -r $RUNTIME

rsync -r --delete-after src/NaCoDoKina.Api/bin/$CONFIGURATION/$FRAMEWORK_VERSION/$RUNTIME arasz@$SERVER_ADDRESS:/var/www/nacodokinaapi/

#ssh arasz@$SERVER_ADDRESS systemctl start kestrel-nacodokinaapi.service

# deploy hangfire host

#ssh arasz@$SERVER_ADDRESS systemctl stop kestrel-hangfirehost.service


dotnet publish src/HangfireHost/HangfireHost.csproj -c $CONFIGURATION -f $FRAMEWORK_VERSION -r $RUNTIME
rsync -r --delete-after src/HangfireHost/bin/$CONFIGURATION/$FRAMEWORK_VERSION/$RUNTIME  arasz@$SERVER_ADDRESS:/var/www/hangfirehost/

#ssh arasz@$SERVER_ADDRESS systemctl start kestrel-hangfirehost.service






