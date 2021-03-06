﻿# Use Microsoft's official build .NET image.
FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build
WORKDIR /app

# Install production dependencies.
# Copy csproj and restore as distinct layers.
COPY ./JapaneseGraph/*.csproj ./JapaneseGraph/
COPY ./JapaneseGraph.Shared/*.csproj ./JapaneseGraph.Shared/
RUN dotnet restore ./JapaneseGraph/

# Copy local code to the container image.
COPY ./JapaneseGraph/ ./JapaneseGraph
COPY ./JapaneseGraph.Shared/ ./JapaneseGraph.Shared
WORKDIR /app

# Build a release artifact.
RUN dotnet publish -c Release -o out ./JapaneseGraph

# Use Microsoft's official runtime .NET image.
# https://hub.docker.com/_/microsoft-dotnet-core-aspnet/
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# Some wierd GRPC fix (https://github.com/GoogleCloudPlatform/google-cloud-dotnet-powerpack/issues/22)
RUN apt update && apt install -y libc-dev

# Run the web service on container startup.
ENTRYPOINT ["dotnet", "JapaneseGraph.dll"]