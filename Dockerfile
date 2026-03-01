# Stage 1: Build
# Start from the .NET SDK image for building the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
# Copy the project file and download dependencies
COPY ["WinterSportAcademy.csproj", "./"]
RUN dotnet restore "WinterSportAcademy.csproj"

# Copy everything else and build
# Copy the rest of the code and compile
COPY . .
RUN dotnet build "WinterSportAcademy.csproj" -c Release -o /app/build

# Stage 2: Publish the app (preparing it for production)
FROM build AS publish
RUN dotnet publish "WinterSportAcademy.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Final Runtime
# Final image (only runtime, no heavy tools)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Copy published files from build stage
COPY --from=publish /app/publish .

# Security: Run as non-root user
# Environment configuration and Security
ENV ASPNETCORE_ENVIRONMENT=Development
USER $APP_UID

ENTRYPOINT ["dotnet", "WinterSportAcademy.dll"]

# Dockerfile ensures the application runs the same way in every environment (development, testing, and production)
#