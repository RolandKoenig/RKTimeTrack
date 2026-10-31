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

echo "#############"
echo "Branch: $branchName"
echo "Commit SHA: $commitSha"
echo ""

# Get infos regarding .NET SDK and Runtime
$dotnetSdkVersion = (Get-Content ../global.json -Raw | ConvertFrom-Json).sdk.version
$dotnetRuntimeVersion = ([xml](Get-Content ../src/RolandK.TimeTrack.Service/RolandK.TimeTrack.Service.csproj -Raw)).Project.ItemGroup.PackageReference |
        Where-Object { $_.Include -eq "Microsoft.AspNetCore.SpaProxy" } |
        Select-Object -ExpandProperty Version

echo "#############"
echo "Sdk: $dotnetSdkVersion"
echo "Runtime: $dotnetRuntimeVersion"
echo ""

# Docker build-and-push
docker login -u $DockerHubUser -p $DockerHubPassword
docker build -t docker.io/rolandk87/rktimetrack:latest-$branchName `
             -t docker.io/rolandk87/rktimetrack:latest `
             -t docker.io/rolandk87/rktimetrack:$commitSha `
             --platform linux/amd64 `
             -f ../src/RolandK.TimeTrack.Service/Dockerfile ../
docker push -a docker.io/rolandk87/rktimetrack