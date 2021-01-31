#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build
WORKDIR /src
COPY ["JapaneseGraph/JapaneseGraph.csproj", "JapaneseGraph/"]
COPY ["JapaneseGraph.Shared/JapaneseGraph.Shared.csproj", "JapaneseGraph.Shared/"]
RUN dotnet restore "JapaneseGraph/JapaneseGraph.csproj"
COPY . .
WORKDIR "/src/JapaneseGraph"
RUN dotnet build "JapaneseGraph.csproj" -c Release -o /app/build

# Some wierd GRPC fix (https://github.com/GoogleCloudPlatform/google-cloud-dotnet-powerpack/issues/22)
RUN apk update && apk add libc-dev

FROM build AS publish
RUN dotnet publish "JapaneseGraph.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JapaneseGraph.dll"]