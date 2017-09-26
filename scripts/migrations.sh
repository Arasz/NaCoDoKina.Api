#!/usr/bin/env sh
cd src/NaCoDoKina.Api/
dotnet ef database update -c ApplicationIdentityContext
dotnet ef database update -c ApplicationContext
cd ../..