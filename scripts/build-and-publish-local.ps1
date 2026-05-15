$PublishDirectory = "../publish"

# Clean publish directory
if (Test-Path $PublishDirectory) {
    Remove-Item -Path "$PublishDirectory/*" -Recurse -Force
}

# Common cleanup
dotnet clean "../RolandK.TimeTrack.slnx"

# Build and test
dotnet build -c Release "../RolandK.TimeTrack.slnx"
dotnet publish -c Release -o $PublishDirectory "../RolandK.TimeTrack.slnx"