# Common cleanup
dotnet clean "../RolandK.TimeTrack.sln"

# Build and test
dotnet build -c Debug "../RolandK.TimeTrack.sln"
dotnet test -c Debug "../RolandK.TimeTrack.sln"