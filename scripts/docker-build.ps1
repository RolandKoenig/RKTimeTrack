$dotnetSdkVersion = (Get-Content ../global.json -Raw | ConvertFrom-Json).sdk.version
$dotnetRuntimeVersion = ([xml](Get-Content ../src/RolandK.TimeTrack.Service/RolandK.TimeTrack.Service.csproj -Raw)).Project.ItemGroup.PackageReference |
        Where-Object { $_.Include -eq "Microsoft.AspNetCore.SpaProxy" } |
        Select-Object -ExpandProperty Version

echo "#############"
echo "Sdk: $dotnetSdkVersion"
echo "Runtime: $dotnetRuntimeVersion"
echo ""

docker build `
  --build-arg DOTNET_SDK_VERSION="$dotnetSdkVersion" `
  --build-arg DOTNET_RUNTIME_VERSION="$dotnetRuntimeVersion" `
  -f ../src/RolandK.TimeTrack.Service/Dockerfile `
  -t rk-time-track-service `
  ../