# ──────────────────────────────────────────────────────
# BASE IMAGE (runtime)
# ──────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# ──────────────────────────────────────────────────────
# BUILD IMAGE (SDK)
# ──────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

# Copy only csproj first → improves Docker layer caching
COPY ["ProductManagement.API/ProductManagement.API.csproj", "ProductManagement.API/"]
COPY ["ProductManagement.Domain/ProductManagement.Domain.csproj", "ProductManagement.Domain/"]
COPY ["ProductManagement.Infrastructure/ProductManagement.Infrastructure.csproj", "ProductManagement.Infrastructure/"]
COPY ["ProductManagement.Application/ProductManagement.Application.csproj", "ProductManagement.Application/"]

RUN dotnet restore "ProductManagement.API/ProductManagement.API.csproj"

# Copy the rest of the source
COPY . .

WORKDIR "/src/ProductManagement.API"
RUN dotnet build "ProductManagement.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# ──────────────────────────────────────────────────────
# PUBLISH IMAGE
# ──────────────────────────────────────────────────────
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ProductManagement.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ──────────────────────────────────────────────────────
# FINAL IMAGE
# ──────────────────────────────────────────────────────
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "ProductManagement.API.dll"]
