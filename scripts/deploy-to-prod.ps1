Param(
    [Parameter(Mandatory = $true)]
    [string]$DockerHubUser,

    [Parameter(Mandatory = $true)]
    [string]$DockerHubPassword
)

# Get infos from git
$commitSha = git rev-parse --short HEAD

# Check whether the image exists
skopeo login --username $DockerHubUser --password $DockerHubPassword registry.hub.docker.com
if($LastExitCode){
    throw "Unable to login to dockerhub!"
}

skopeo inspect docker://registry.hub.docker.com/rolandk87/rktimetrack:$commitSha
if($LastExitCode){
    throw "The image rolandk87/rktimetrack:$commitSha does not exist!"
}

# Push to Azure Container Apps
az containerapp update -n rolandk-timetrack-prod-app `
                       --resource-group RolandK-TimeTrack-PROD `
                       --image registry.hub.docker.com/rolandk87/rktimetrack:$commitSha