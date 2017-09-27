#!/bin/bash
set -ev
if [ "${RUN_UNIT_TESTS}" = "true" ]; then
	 dotnet test --no-build --no-restore  ./tests/NaCoDoKina.Api.Tests/*.csproj
fi

