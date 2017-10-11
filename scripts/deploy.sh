#!/bin/bash
SERVER_ADDRESS=139.59.157.1
FRAMEWORK_VERSION=netcoreapp2.0
CONFIGURATION=Release

dotnet publish src/NaCoDoKina.Api/NaCoDoKina.Api.csproj -c $CONFIGURATION -f $FRAMEWORK_VERSION
rsync -r --delete-after src/NaCoDoKina.Api/bin/$CONFIGURATION/$FRAMEWORK_VERSION/ arasz@$SERVER_ADDRESS:/var/www/nacodokinaapi/


dotnet publish src/HangfireHost/HangfireHost.csproj -c $CONFIGURATION -f $FRAMEWORK_VERSION
rsync -r --delete-after src/HangfireHost/bin/$CONFIGURATION/$FRAMEWORK_VERSION/  arasz@$SERVER_ADDRESS:/var/www/hangfirehost/

