#!/usr/bin/env bash
if ["$RUN_UNIT_TESTS" == "true"]; then
    dotnet test --no-build --no-restore  ./tests/NaCoDoKina.Api.IntegrationTests/*.csproj
fi