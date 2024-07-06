# Common cleanup
dotnet clean "../RKTimeTrack.sln"

# Build and test
dotnet build -c Debug "../RKTimeTrack.sln"
dotnet test -c Debug "../RKTimeTrack.sln"