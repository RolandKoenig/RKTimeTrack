using System.Text.Json;
using System.Text.Json.Serialization;

namespace RolandK.TimeTrack.Application.Models.Json;

public class TimeTrackingBudgetJsonConverter : JsonConverter<TimeTrackingBudget>
{
    public override TimeTrackingBudget Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options) =>reader.GetDouble();

    public override void Write(
        Utf8JsonWriter writer,
        TimeTrackingBudget value,
        JsonSerializerOptions options) =>
        writer.WriteNumberValue(value.Hours);
}