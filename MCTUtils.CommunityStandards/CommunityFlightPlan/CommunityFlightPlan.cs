namespace MCTUtils.CommunityStandards.CommunityFlightPlan;

using System.Text.Json;
using System.Text.Json.Serialization;
using MCTUtils.CommunityStandards.Common;
using MCTUtils.CommunityStandards.Serialization;

/// <summary>
/// Community standard schema for simulated military themes flight plans.
/// </summary>
[JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
public class CommunityFlightPlan
{
    /// <summary>
    /// Fixed identifier confirming this is a community-flightplan document.
    /// </summary>
    [JsonPropertyName("schema")]
    public string Schema { get; set; } = "community-flightplan";

    /// <summary>
    /// Semantic version of the schema this file conforms to.
    /// </summary>
    [JsonPropertyName("schema_version")]
    public string SchemaVersion { get; set; } = string.Empty;

    /// <summary>
    /// Globally unique identifier for this flight plan (UUID v7).
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// ISO 8601 UTC timestamp of when this plan was first created.
    /// </summary>
    [JsonPropertyName("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// ISO 8601 UTC timestamp of the last modification. Null if never updated after creation.
    /// </summary>
    [JsonPropertyName("updated_at")]
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// Free-form user-agent string identifying the tool and version that produced this file.
    /// </summary>
    [JsonPropertyName("tool_source")]
    public string? ToolSource { get; set; }

    /// <summary>
    /// The coalition this flight plan belongs to.
    /// </summary>
    [JsonPropertyName("coalition")]
    public Coalition Coalition { get; set; }

    /// <summary>
    /// Package definitions grouping assets into tactical units.
    /// </summary>
    [JsonPropertyName("package")]
    public List<Package> Package { get; } = [];

    /// <summary>
    /// Flight assets participating in the package.
    /// </summary>
    [JsonPropertyName("assets")]
    public List<Asset> Assets { get; } = [];

    /// <summary>
    /// Route definitions. Each Asset references one route by route_id.
    /// </summary>
    [JsonPropertyName("routes")]
    public List<Route> Routes { get; } = [];

    /// <summary>
    /// List of waypoints available for use in route legs.
    /// </summary>
    [JsonPropertyName("waypoints")]
    public List<Waypoint>? Waypoints { get; set; }

    /// <summary>
    /// Tool-specific extension data.
    /// </summary>
    [JsonPropertyName("extensions")]
    public Extensions? Extensions { get; set; }

    /// <summary>
    /// Loads a <see cref="CommunityFlightPlan"/> from a JSON file path.
    /// </summary>
    /// <param name="path">The file path to load from.</param>
    /// <returns>The deserialized flight plan.</returns>
    public static CommunityFlightPlan Load(string path)
    {
        var json = File.ReadAllText(path);
        return FromJson(json);
    }

    /// <summary>
    /// Saves this flight plan to a JSON file.
    /// </summary>
    /// <param name="path">The file path to save to.</param>
    /// <param name="writeIndented">Whether to format the JSON with indentation.</param>
    public void Save(string path, bool writeIndented = true)
    {
        var json = ToJson(writeIndented);
        File.WriteAllText(path, json);
    }

    /// <summary>
    /// Deserializes a <see cref="CommunityFlightPlan"/> from a JSON string.
    /// </summary>
    /// <param name="json">The JSON string to deserialize.</param>
    /// <returns>The deserialized flight plan.</returns>
    public static CommunityFlightPlan FromJson(string json)
    {
        var options = JsonSerializerOptionsFactory.CreateDefault();
        return JsonSerializer.Deserialize<CommunityFlightPlan>(json, options)!;
    }

    /// <summary>
    /// Serializes this flight plan to a JSON string.
    /// </summary>
    /// <param name="writeIndented">Whether to format the JSON with indentation.</param>
    /// <returns>The JSON string representation.</returns>
    public string ToJson(bool writeIndented = false)
    {
        var options = JsonSerializerOptionsFactory.Create(writeIndented);
        return JsonSerializer.Serialize(this, options);
    }
}
