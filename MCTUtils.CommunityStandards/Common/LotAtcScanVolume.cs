namespace MCTUtils.CommunityStandards.Common;

/// <summary>
/// Defines the scan volume of a radar.
/// </summary>
public class LotAtcScanVolume
{
    /// <summary>
    /// Azimuth range [min, max].
    /// </summary>
    [JsonPropertyName("azimuth")]
    public List<double>? Azimuth { get; set; }

    /// <summary>
    /// Elevation range [min, max].
    /// </summary>
    [JsonPropertyName("elevation")]
    public List<double>? Elevation { get; set; }
}
