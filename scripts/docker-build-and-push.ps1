Param(
    [string]$DockerHubUser,
    [string]$DockerHubPassword
)

$ErrorActionPreference = "Stop"

# Argument checks
if(-Not $DockerHubUser){
    throw "DockerHubUser not set"
}
if(-Not $DockerHubPassword){
    throw "DockerHubPassword not set"
}

# Get infos from git
$branchName = git rev-parse --abbrev-ref HEAD

# Docker build-and-push
docker login -u $DockerHubUser -p $DockerHubPassword
docker build -t rolandk87/rktimetrack:latest-$branchName `
             --platform linux/amd64 `
             -f ../src/RolandK.TimeTrack.Service/Dockerfile ../
docker push -a rolandk87/rktimetrack