#!/usr/bin/env bash

set -xe

dotnet run --project "$(pwd)/src/Officify.Build.Host/Officify.Build.Host.csproj" -- "$@"