# See https://aka.ms/customizecontainer to learn how to customize your debug container

# ---------------- BASE ----------------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# ---------------- BUILD ----------------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["School.API/School.API.csproj", "School.API/"]
COPY ["School.Application/School.Application.csproj", "School.Application/"]
COPY ["School.Domain/School.Domain.csproj", "School.Domain/"]
COPY ["School.Infrastructure/School.Infrastructure.csproj", "School.Infrastructure/"]
RUN dotnet restore "./School.API/School.API.csproj"
COPY . .
WORKDIR "/src/School.API"
RUN dotnet build "./School.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# ---------------- PUBLISH ----------------
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./School.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ---------------- FINAL ----------------
FROM base AS final

# 🔑 switch to root ONLY for folder + permission setup
USER root

WORKDIR /app
COPY --from=publish /app/publish .

# 📁 create upload folder & assign ownership
RUN mkdir -p /app/wwwroot/uploads/schools \
    && chown -R $APP_UID:$APP_UID /app/wwwroot

# 🔒 switch back to non-root user (SECURE)
USER $APP_UID

ENTRYPOINT ["dotnet", "School.API.dll"]
