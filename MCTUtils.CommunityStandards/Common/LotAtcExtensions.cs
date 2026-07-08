namespace MCTUtils.CommunityStandards.Common;

/// <summary>
/// LotATC-specific extension data for GCI picture generation.
/// </summary>
public class LotAtcExtensions
{
    /// <summary>
    /// Distance radius of maximum radar detection in metres.
    /// </summary>
    [JsonPropertyName("detection_range_m")]
    public double? DetectionRangeM { get; set; }

    /// <summary>
    /// Distance radius of threat in metres.
    /// </summary>
    [JsonPropertyName("threat_range_m")]
    public double? ThreatRangeM { get; set; }

    /// <summary>
    /// Radar cross-section value for this unit.
    /// </summary>
    [JsonPropertyName("rcs")]
    public double? Rcs { get; set; }

    /// <summary>
    /// Marks this asset as a carrier (enables approach / marshal procedures in LotATC).
    /// </summary>
    [JsonPropertyName("is_carrier")]
    public bool? IsCarrier { get; set; }

    /// <summary>
    /// LotATC track number (TN) for this asset.
    /// </summary>
    [JsonPropertyName("track_number")]
    public string? TrackNumber { get; set; }

    /// <summary>
    /// Radar parameters for assets that have an emitting radar.
    /// </summary>
    [JsonPropertyName("radar")]
    public LotAtcRadar? Radar { get; set; }
}
