namespace MCTUtils.CommunityStandards.Common;

/// <summary>
/// A geographic coordinate pair (WGS-84 decimal degrees).
/// </summary>
public class LatLon
{
    /// <summary>
    /// Latitude in decimal degrees. Positive = North.
    /// </summary>
    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }

    /// <summary>
    /// Longitude in decimal degrees. Positive = East.
    /// </summary>
    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }
}
