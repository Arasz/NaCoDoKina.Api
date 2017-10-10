#!/bin/bash
set -ev
if [ "${RUN_MIGRATIONS}" = "true" ]; then
	cd src/NaCoDoKina.Api/
    dotnet ef database update -c ApplicationIdentityContext -s ../NaCoDoKina.Api/NaCoDoKina.Api.csproj
    dotnet ef database update -c ApplicationContext -s ../NaCoDoKina.Api/NaCoDoKina.Api.csproj
    cd ../..
fi