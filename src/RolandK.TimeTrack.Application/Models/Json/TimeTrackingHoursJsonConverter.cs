using System.Text.Json;
using System.Text.Json.Serialization;

namespace RolandK.TimeTrack.Application.Models.Json;

public class TimeTrackingHoursJsonConverter : JsonConverter<TimeTrackingHours>
{
    public override TimeTrackingHours Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options) =>reader.GetDouble();

    public override void Write(
        Utf8JsonWriter writer,
        TimeTrackingHours value,
        JsonSerializerOptions options) =>
        writer.WriteNumberValue(value.Hours);
}