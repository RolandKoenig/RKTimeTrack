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
$commitHash = (git rev-parse HEAD).Substring(0,16)
$branchName = git rev-parse --abbrev-ref HEAD

# Docker build-and-push
docker login -u $DockerHubUser -p $DockerHubPassword
docker build -t rolandk87/rktimetrack:latest `
             -t rolandk87/rktimetrack:latest-$branchName `
             -t rolandk87/rktimetrack:$branchName-$commitHash `
             --platform linux/amd64 `
             -f ../src/RKTimeTrack.Service/Dockerfile ../
docker push -a rolandk87/rktimetrack