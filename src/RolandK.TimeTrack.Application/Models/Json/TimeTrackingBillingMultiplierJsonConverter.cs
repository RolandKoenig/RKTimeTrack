using System.Text.Json;
using System.Text.Json.Serialization;

namespace RolandK.TimeTrack.Application.Models.Json;

public class TimeTrackingBillingMultiplierJsonConverter : JsonConverter<TimeTrackingBillingMultiplier>
{
    public override TimeTrackingBillingMultiplier Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options) =>reader.GetDouble();

    public override void Write(
        Utf8JsonWriter writer,
        TimeTrackingBillingMultiplier value,
        JsonSerializerOptions options) =>
        writer.WriteNumberValue(value.Multiplier);
}