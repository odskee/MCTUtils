namespace MCTUtils.CommunityStandards.Common;

using System.Text.Json.Serialization;

/// <summary>
/// Ordered sequence of waypoints for a single asset.
/// </summary>
[JsonObjectCreationHandling(JsonObjectCreationHandling.Populate)]
public class Route
{
    /// <summary>
    /// Unique identifier for this route.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Reference back to the Asset (assets[].id) that flies this route.
    /// </summary>
    [JsonPropertyName("asset_id")]
    public Guid AssetId { get; set; }

    /// <summary>
    /// Offset in seconds from the package TOT reference used to stagger assets. Negative = early.
    /// </summary>
    [JsonPropertyName("tot_offset_seconds")]
    public int? TotOffsetSeconds { get; set; }

    /// <summary>
    /// Basic fuel planning data.
    /// </summary>
    [JsonPropertyName("fuel_plan")]
    public FuelPlan? FuelPlan { get; set; }

    /// <summary>
    /// List of legs that make the total route.
    /// </summary>
    [JsonPropertyName("legs")]
    public List<RouteLeg> Legs { get; } = [];
}
