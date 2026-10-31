using System.Text.Json;

namespace RolandK.TimeTrack.Service.ContainerTests.Util;

internal static class TestUtil
{
    private const string SOLUTION_FILE_NAME = "RolandK.TimeTrack.slnx";
    
    public static string GetSolutionDirectory()
    {
        var thisAssembly = typeof(TestUtil).Assembly;
        
        var currentDirectory = Path.GetDirectoryName(thisAssembly.Location);
        if (string.IsNullOrEmpty(currentDirectory))
        {
            throw new InvalidOperationException("Unable to find solution directory!");
        }
        
        var currentSolutionPath = Path.Combine(currentDirectory, SOLUTION_FILE_NAME);
        while (!File.Exists(currentSolutionPath))
        {
            currentDirectory = Directory.GetParent(currentDirectory)?.FullName;
            if (string.IsNullOrEmpty(currentDirectory))
            {
                throw new InvalidOperationException("Unable to find solution directory!");
            }
            
            currentSolutionPath = Path.Combine(currentDirectory, SOLUTION_FILE_NAME);
        }

        if (!File.Exists(currentSolutionPath))
        {
            throw new InvalidOperationException("Unable to find solution directory!");
        }

        return currentDirectory;
    }
    
    public static string GetDotNetSdkVersion(string solutionDirectory)
    {
        var globalJsonPath = Path.Combine(solutionDirectory, "global.json");
        using var globalJsonStream = File.OpenRead(globalJsonPath);
        using var globalJson = JsonDocument.Parse(globalJsonStream);

        return globalJson.RootElement
            .GetProperty("sdk")
            .GetProperty("version")
            .GetString() ?? throw new InvalidOperationException("No .NET SDK version found in global.json.");
    }
}