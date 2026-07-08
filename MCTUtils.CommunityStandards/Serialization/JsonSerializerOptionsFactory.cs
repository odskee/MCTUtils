using System.Text.Json;
using System.Text.Json.Serialization;

namespace MCTUtils.CommunityStandards.Serialization;

/// <summary>
    /// Creates pre-configured <see cref="JsonSerializerOptions"/> instances for the MCT Community Standards SDK.
    /// </summary>
    public static class JsonSerializerOptionsFactory
    {
        /// <summary>
        /// Creates the default serializer options with:
        /// - EnumMemberJsonConverterFactory (honors [EnumMember] attributes for enum serialization)
        /// - Camel case naming disabled (preserves PascalCase from JsonPropertyName attributes)
        /// - IgnoreNullValues = false (null properties are serialized)
        /// - WriteIndented = false by default
        /// </summary>
        public static JsonSerializerOptions CreateDefault()
        {
            return Create(writeIndented: false);
        }

        /// <summary>
        /// Creates serializer options with configurable indentation.
        /// </summary>
        /// <param name="writeIndented">Whether to format the JSON output with indentation.</param>
        public static JsonSerializerOptions Create(bool writeIndented)
        {
            return new JsonSerializerOptions
            {
                PropertyNamingPolicy = null, // Preserve JsonPropertyName values
                WriteIndented = writeIndented,
                DefaultIgnoreCondition = JsonIgnoreCondition.Never,
                PreferredObjectCreationHandling = JsonObjectCreationHandling.Populate,
                Converters =
                {
                    new EnumMemberJsonConverterFactory()
                }
            };
        }
    }
