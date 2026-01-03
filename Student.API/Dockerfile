# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Student.API/Student.API.csproj", "Student.API/"]
COPY ["Student.Application/Student.Application.csproj", "Student.Application/"]
COPY ["Student.Domain/Student.Domain.csproj", "Student.Domain/"]
COPY ["Student.Infrastructure/Student.Infrastructure.csproj", "Student.Infrastructure/"]
RUN dotnet restore "./Student.API/Student.API.csproj"
COPY . .
WORKDIR "/src/Student.API"
RUN dotnet build "./Student.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Student.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final

# 🔑 switch to root ONLY for folder + permission setup
USER root

WORKDIR /app
COPY --from=publish /app/publish .

# 📁 create upload folder & assign ownership
RUN mkdir -p /app/wwwroot/uploads/students \
    && chown -R $APP_UID:$APP_UID /app/wwwroot

# 🔒 switch back to non-root user (SECURE)
USER $APP_UID

ENTRYPOINT ["dotnet", "Student.API.dll"]