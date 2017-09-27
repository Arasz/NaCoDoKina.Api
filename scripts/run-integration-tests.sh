#!/usr/bin/env sh
if [${RUN_INTEGRATION_TESTS} == "true"]; then
    dotnet test --no-build --no-restore  ./tests/NaCoDoKina.Api.IntegrationTests/*.csproj
fi
