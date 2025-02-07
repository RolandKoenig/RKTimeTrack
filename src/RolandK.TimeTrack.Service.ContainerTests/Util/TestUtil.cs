namespace RolandK.TimeTrack.Service.ContainerTests.Util;

internal static class TestUtil
{
    private const string SOLUTION_FILE_NAME = "RKTimeTrack.sln";
    
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
}