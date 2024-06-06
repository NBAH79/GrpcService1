#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5229
EXPOSE 7138

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

RUN dotnet dev-certs https -ep /https/aspnetapp.pfx -p 12345678

ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["GrpcService2/GrpcService2.csproj", "GrpcService2/"]
RUN dotnet restore "./GrpcService2/./GrpcService2.csproj"
COPY . .
WORKDIR "/src/GrpcService2"
RUN dotnet build "./GrpcService2.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./GrpcService2.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GrpcService2.dll"]