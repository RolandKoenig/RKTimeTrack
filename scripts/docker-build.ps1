$dotnetSdkVersion = (Get-Content ../global.json -Raw | ConvertFrom-Json).sdk.version

echo "#############"
echo "Sdk: $dotnetSdkVersion"
echo ""

docker build `
  --build-arg DOTNET_SDK_VERSION="$dotnetSdkVersion" `
  --build-arg DOTNET_RUNTIME_VERSION="$dotnetRuntimeVersion" `
  -f ../src/RolandK.TimeTrack.Service/Dockerfile `
  -t rk-time-track-service `
  ../