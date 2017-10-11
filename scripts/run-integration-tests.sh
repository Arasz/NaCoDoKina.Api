#!/bin/bash
set -ev
if [ "${RUN_INTEGRATION_TESTS}" = "true" ]; then
	 dotnet test --no-build --no-restore  ./tests/NaCoDoKina.Api.IntegrationTests/*.csproj
	 dotnet test --no-build --no-restore  ./tests/HangfireHost.Integration.Tests/*.csproj
fi

