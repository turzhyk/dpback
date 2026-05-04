FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DPBack.API/DPBack.API.csproj", "DPBack.API/"]
COPY ["DPBack.Application/DPBack.Application.csproj", "DPBack.Application/"]
COPY ["DPBack.Domain/DPBack.Domain.csproj", "DPBack.Domain/"]
COPY ["DPBack.Infrastructure/DPBack.Infrastructure.csproj", "DPBack.Infrastructure/"]
RUN dotnet restore "DPBack.API/DPBack.API.csproj"
COPY . .
WORKDIR "/src/DPBack.API"
RUN dotnet build "./DPBack.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DPBack.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DPBack.API.dll"]
