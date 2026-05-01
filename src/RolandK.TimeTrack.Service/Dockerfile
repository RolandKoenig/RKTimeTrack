#####################################
# Build image
FROM mcr.microsoft.com/dotnet/sdk:10.0.203 AS build
ARG RESOURCE_REAPER_SESSION_ID="00000000-0000-0000-0000-000000000000"
LABEL "org.testcontainers.resource-reaper-session"=$RESOURCE_REAPER_SESSION_ID

# Install node to be able to use npm during build time
RUN bash -E $(curl -fsSL https://deb.nodesource.com/setup_22.x | bash - ); apt install -y nodejs

# Copy all *.csproj files, the switch to /src directory
COPY --parents **/*.csproj ./
WORKDIR /src

# Copy global project settings
COPY "global.json" "global.json"
COPY "Directory.Build.props" "Directory.Build.props"

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
FROM mcr.microsoft.com/dotnet/aspnet:10.0.7-noble-chiseled-extra AS final
EXPOSE 80

## Copy application and define entrypoint
WORKDIR /app
COPY --from=publish --chown=app /app/publish . 
ENTRYPOINT ["dotnet", "RolandK.TimeTrack.Service.dll"]