Param(
    [Parameter(Mandatory = $true)]
    [string]$DockerHubUser,

    [Parameter(Mandatory = $true)]
    [string]$DockerHubPassword
)

$ErrorActionPreference = "Stop"

# Get infos from git
$branchName = git rev-parse --abbrev-ref HEAD
$commitSha = git rev-parse --short HEAD

# Docker build-and-push
docker login -u $DockerHubUser -p $DockerHubPassword
docker build -t docker.io/rolandk87/rktimetrack:latest-$branchName `
             -t docker.io/rolandk87/rktimetrack:latest `
             -t docker.io/rolandk87/rktimetrack:$commitSha `
             --platform linux/amd64 `
             -f ../src/RolandK.TimeTrack.Service/Dockerfile ../
docker push -a docker.io/rolandk87/rktimetrack