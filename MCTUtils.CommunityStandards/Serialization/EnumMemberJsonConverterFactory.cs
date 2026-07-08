using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MCTUtils.CommunityStandards.Serialization;

/// <summary>
/// A <see cref="JsonConverterFactory"/> that serializes enum values using
/// <see cref="EnumMemberAttribute.Value"/> when present, falling back to the
/// enum member name — exactly like <see cref="JsonStringEnumConverter"/> but
/// with <c>[EnumMember]</c> support.
/// </summary>
public class EnumMemberJsonConverterFactory : JsonConverterFactory
{
    /// <inheritdoc />
    public override bool CanConvert(Type typeToConvert) => typeToConvert.IsEnum;

    /// <inheritdoc />
    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var converterType = typeof(EnumMemberJsonConverter<>).MakeGenericType(typeToConvert);
        return (JsonConverter)Activator.CreateInstance(converterType)!;
    }

    private sealed class EnumMemberJsonConverter<T> : JsonConverter<T> where T : struct, Enum
    {
        private static readonly Dictionary<T, string> ToJson = new();
        private static readonly Dictionary<string, T> FromJson = new();

        static EnumMemberJsonConverter()
        {
            foreach (T value in Enum.GetValues<T>())
            {
                var memberInfo = typeof(T).GetMember(value.ToString())[0];
                var enumMemberAttr = memberInfo.GetCustomAttribute<EnumMemberAttribute>();
                var jsonValue = enumMemberAttr?.Value ?? value.ToString();
                ToJson[value] = jsonValue;
                // Avoid duplicate keys — first registration wins
                if (!FromJson.ContainsKey(jsonValue))
                {
                    FromJson[jsonValue] = value;
                }
            }
        }

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var str = reader.GetString();
            if (str is not null && FromJson.TryGetValue(str, out var value))
                return value;
            throw new JsonException($"Unknown enum value '{str}' for type {typeof(T).Name}.");
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(ToJson[value]);
        }
    }
}
