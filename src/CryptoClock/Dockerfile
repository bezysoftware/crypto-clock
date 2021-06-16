#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0-buster-slim AS base
RUN apt-get update && apt-get install -y libgdiplus
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
RUN apt-get update \
 && apt-get install --no-install-recommends --assume-yes protobuf-compiler-grpc
ENV PROTOBUF_PROTOC=/usr/bin/protoc
WORKDIR /src
COPY ["CryptoClock.csproj", "CryptoClock/"]
RUN dotnet restore "CryptoClock/CryptoClock.csproj"
WORKDIR "/src/CryptoClock"
COPY . .
RUN dotnet build "CryptoClock.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CryptoClock.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CryptoClock.dll"]