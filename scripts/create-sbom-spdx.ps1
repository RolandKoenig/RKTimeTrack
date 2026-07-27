$ErrorActionPreference = "Stop"

##### Publish the application to ../publish directory first
# Clean publish directory
if (Test-Path "../publish") {
  Remove-Item -Path "../publish/*" -Recurse -Force
}

# Common cleanup
dotnet clean "../RolandK.TimeTrack.slnx"

# Build and test
dotnet build -c Release "../src/RolandK.TimeTrack.Service/RolandK.TimeTrack.Service.csproj"
dotnet publish -c Release -o "../publish" "../src/RolandK.TimeTrack.Service/RolandK.TimeTrack.Service.csproj"

##### Build docker image
docker build -f ../src/RolandK.TimeTrack.Service/Dockerfile -t rk-time-track-service ../

##### Generate sbom
sbom-tool generate `
  -b ../publish/ `
  -bc ../src/RolandK.TimeTrack.Service/ `
  -di rk-time-track-service `
  -pn RolandK.TimeTrack.Service `
  -pv 1.0.0 `
  -ps "RolandK Consulting GmbH" `
  -pm

# copy sbom to sbom directory
Copy-Item -Path "../publish/_manifest/spdx_2.2" -Destination "../sbom" -Recurse -Force