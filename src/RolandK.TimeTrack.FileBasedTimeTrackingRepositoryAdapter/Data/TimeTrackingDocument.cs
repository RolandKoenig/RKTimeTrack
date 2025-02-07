using System.Text.Json;
using RolandK.TimeTrack.Application.Models;

namespace RolandK.TimeTrack.FileBasedTimeTrackingRepositoryAdapter.Data;

public record TimeTrackingDocument(string Version, IReadOnlyCollection<TimeTrackingDay> Days)
{
    /// <summary>
    /// Persists current data into json to the given output stream.
    /// </summary>
    public async Task WriteToStreamAsync(Stream outStream, bool writeIndented, CancellationToken cancellationToken)
    {
        var serializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        serializerOptions.WriteIndented = writeIndented;
    
        await JsonSerializer.SerializeAsync(outStream, this, serializerOptions, cancellationToken);
    }

    /// <summary>
    /// Loads data from given stream.
    /// </summary>
    public static async Task<TimeTrackingDocument> LoadFromStreamAsync(Stream inStream, CancellationToken cancellationToken)
    {
        var serializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        
        var document = await JsonSerializer.DeserializeAsync<TimeTrackingDocument>(inStream, serializerOptions, cancellationToken);
        if (document == null)
        {
            throw new Exception($"Unknown error while deserializing {nameof(TimeTrackingDocument)}!");
        }

        return document;
    }
}