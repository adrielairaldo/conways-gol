FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
EXPOSE 5000

ENV ASPNETCORE_URLS=http://+:5000

# Install cultures (same approach as Alpine SDK image)
RUN apk add --no-cache icu-libs
# Disable the invariant mode (set in base image)
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build

COPY ["src/Conways.Service.Application/Conways.Service.Application.csproj", "src/Conways.Service.Application/"]
COPY ["src/Conways.Service.Domain/Conways.Service.Domain.csproj", "src/Conways.Service.Domain/"]
COPY ["src/Conways.Service.HttpApi/Conways.Service.HttpApi.csproj", "src/Conways.Service.HttpApi/"]
COPY ["src/Conways.Service.Infrastructure.MongoDb/Conways.Service.Infrastructure.MongoDb.csproj", "src/Conways.Service.Infrastructure.MongoDb/"]
COPY ["src/Conways.Service.Infrastructure.Redis/Conways.Service.Infrastructure.Redis.csproj", "src/Conways.Service.Infrastructure.Redis/"]

RUN dotnet restore "src/Conways.Service.HttpApi/Conways.Service.HttpApi.csproj"
COPY ./src ./src
WORKDIR "/src/Conways.Service.HttpApi"
RUN dotnet publish "Conways.Service.HttpApi.csproj" -c Release --no-restore -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Conways.Service.HttpApi.dll"]