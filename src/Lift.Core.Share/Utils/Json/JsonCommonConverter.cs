using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Lift.Core.Utils.Json;
using JsonAttribute = Lift.Core.Utils.Json.JsonAttribute;

namespace Lift.Core.Share.Utils.Json;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
internal class JsonCommonConverter<T> : JsonConverter<T>
{
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => JsonSerializer.Deserialize<T>(ref reader, options);

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        if (value is null)
            throw new InvalidException("The input value must be not null.");

        if (Attribute.GetCustomAttribute(typeof(T), typeof(JsonAttribute)) is not JsonAttribute attr)
            return;


        writer.WriteStartObject();

        var props = typeof(T).GetProperties();
        foreach (var prop in props)
        {
            // ignore the property with ExcludeAttribute
            if (prop.GetCustomAttribute<ExcludeAttribute>() != null)
                continue;


            if (!attr.IncludeAll
                && prop.GetCustomAttribute<IncludeAttribute>() == null) continue;


            writer.WritePropertyName(prop.Name);
            JsonSerializer.Serialize(writer, prop.GetValue(value), options);
        }

        writer.WriteEndObject();
    }
}
