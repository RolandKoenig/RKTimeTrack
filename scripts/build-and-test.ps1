# Common cleanup
dotnet clean "../RolandK.TimeTrack.slnx"

# Build and test
dotnet build -c Debug "../RolandK.TimeTrack.slnx"
dotnet test -c Debug "../RolandK.TimeTrack.slnx"