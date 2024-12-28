#####################################
# Build image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG RESOURCE_REAPER_SESSION_ID="00000000-0000-0000-0000-000000000000"
LABEL "org.testcontainers.resource-reaper-session"=$RESOURCE_REAPER_SESSION_ID
WORKDIR /src

# Install node to be able to use npm during build time
RUN bash -E $(curl -fsSL https://deb.nodesource.com/setup_20.x | bash - ); apt install -y nodejs

# Restore first for improved cache usage
COPY "global.json" "global.json"
COPY "Directory.Build.props" "Directory.Build.props"
COPY "src/RKTimeTrack.Service/RKTimeTrack.Service.csproj" "RKTimeTrack.Service/RKTimeTrack.Service.csproj"
COPY "src/RKTimeTrack.Application/RKTimeTrack.Application.csproj" "RKTimeTrack.Application/RKTimeTrack.Application.csproj"
COPY "src/RKTimeTrack.FileBasedTimeTrackingRepositoryAdapter/RKTimeTrack.FileBasedTimeTrackingRepositoryAdapter.csproj" "RKTimeTrack.FileBasedTimeTrackingRepositoryAdapter/RKTimeTrack.FileBasedTimeTrackingRepositoryAdapter.csproj"
COPY "src/RKTimeTrack.StaticTopicRepositoryAdapter/RKTimeTrack.StaticTopicRepositoryAdapter.csproj" "RKTimeTrack.StaticTopicRepositoryAdapter/RKTimeTrack.StaticTopicRepositoryAdapter.csproj"
RUN dotnet restore "RKTimeTrack.Service/RKTimeTrack.Service.csproj"

# Build the application
COPY src .
WORKDIR "/src/RKTimeTrack.Service"
RUN dotnet build "RKTimeTrack.Service.csproj" -c Release -o /app/build

# Publish the application to /app/publish
FROM build AS publish
RUN dotnet publish "RKTimeTrack.Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Remove files, which we don't need in the runtime image
RUN rm /app/publish/appsettings.Development.json

#####################################
# Runtime image
## Configure OS
FROM mcr.microsoft.com/dotnet/aspnet:9.0-noble-chiseled-extra AS final
EXPOSE 80

## Copy application and define entrypoint
WORKDIR /app
COPY --from=publish --chown=app /app/publish . 
ENTRYPOINT ["dotnet", "RKTimeTrack.Service.dll"]