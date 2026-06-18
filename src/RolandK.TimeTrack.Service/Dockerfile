#####################################
# Node.js image for frontend build tooling
FROM node:26-bookworm-slim AS node

#####################################
# Build image
FROM mcr.microsoft.com/dotnet/sdk:10.0.301 AS build
ARG RESOURCE_REAPER_SESSION_ID="00000000-0000-0000-0000-000000000000"
LABEL "org.testcontainers.resource-reaper-session"=$RESOURCE_REAPER_SESSION_ID

# Install current Node.js/npm from the official Node image into the build image
COPY --from=node /usr/local /usr/local
RUN node --version && npm --version

# Restore first for improved cache usage
WORKDIR /src
COPY "global.json" "global.json"
COPY "Directory.Build.props" "Directory.Build.props"
COPY "src/RolandK.TimeTrack.Service/RolandK.TimeTrack.Service.csproj" "RolandK.TimeTrack.Service/RolandK.TimeTrack.Service.csproj"
COPY "src/RolandK.TimeTrack.Application/RolandK.TimeTrack.Application.csproj" "RolandK.TimeTrack.Application/RolandK.TimeTrack.Application.csproj"
COPY "src/RolandK.TimeTrack.ExportAdapter/RolandK.TimeTrack.ExportAdapter.csproj" "RolandK.TimeTrack.ExportAdapter/RolandK.TimeTrack.ExportAdapter.csproj"
COPY "src/RolandK.TimeTrack.FileBasedTimeTrackingRepositoryAdapter/RolandK.TimeTrack.FileBasedTimeTrackingRepositoryAdapter.csproj" "RolandK.TimeTrack.FileBasedTimeTrackingRepositoryAdapter/RolandK.TimeTrack.FileBasedTimeTrackingRepositoryAdapter.csproj"
COPY "src/RolandK.TimeTrack.StaticTopicRepositoryAdapter/RolandK.TimeTrack.StaticTopicRepositoryAdapter.csproj" "RolandK.TimeTrack.StaticTopicRepositoryAdapter/RolandK.TimeTrack.StaticTopicRepositoryAdapter.csproj"

RUN dotnet restore "RolandK.TimeTrack.Service/RolandK.TimeTrack.Service.csproj"

# Restore all projects
RUN dotnet restore "RolandK.TimeTrack.Service/RolandK.TimeTrack.Service.csproj"

# Build the application
COPY src .
WORKDIR "/src/RolandK.TimeTrack.Service"
RUN dotnet build "RolandK.TimeTrack.Service.csproj" -c Release -o /app/build --no-restore

# Publish the application to /app/publish
FROM build AS publish
RUN dotnet publish "RolandK.TimeTrack.Service.csproj" -c Release -o /app/publish /p:UseAppHost=false  --no-restore

# Remove files, which we don't need in the runtime image
RUN rm /app/publish/appsettings.Development.json

#####################################
# Runtime image
## Configure OS
FROM mcr.microsoft.com/dotnet/aspnet:10.0.8-noble-chiseled-extra AS final
EXPOSE 80

## Copy application and define entrypoint
WORKDIR /app
COPY --from=publish --chown=app /app/publish . 
ENTRYPOINT ["dotnet", "RolandK.TimeTrack.Service.dll"]