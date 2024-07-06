# Common cleanup
dotnet clean "../RKTimeTrack.sln"

# Delete all bin and obj directories
$directories = Get-ChildItem "../src/" -include bin,obj,dist -Recurse | Where-Object { !$_.FullName.Contains("node_modules") }
foreach ($actDirectory in $directories)
{
    if(Test-Path $actDirectory.fullname)
    {
        "Deleting $actDirectory"
        remove-item $actDirectory.fullname -Force -Recurse
    }
}

# Build and test
dotnet build -c Debug "../RKTimeTrack.sln"
dotnet test -c Debug "../RKTimeTrack.sln"