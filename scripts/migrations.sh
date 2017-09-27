#!/bin/bash
set -ev
if [ "${RUN_MIGRATIONS}" = "true" ]; then
	cd src/NaCoDoKina.Api/
    dotnet ef database update -c ApplicationIdentityContext
    dotnet ef database update -c ApplicationContext
    cd ../..
fi