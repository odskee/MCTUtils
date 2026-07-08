namespace MCTUtils.CommunityStandards.Common;

/// <summary>
/// Radar parameters for assets that have an emitting radar.
/// </summary>
public class LotAtcRadar
{
    /// <summary>
    /// Radar status (on/off).
    /// </summary>
    [JsonPropertyName("radar_status")]
    public bool? RadarStatus { get; set; }

    /// <summary>
    /// Scan period.
    /// </summary>
    [JsonPropertyName("scan_period")]
    public double? ScanPeriod { get; set; }

    /// <summary>
    /// Radar aperture.
    /// </summary>
    [JsonPropertyName("aperture")]
    public double? Aperture { get; set; }

    /// <summary>
    /// Radar pitch.
    /// </summary>
    [JsonPropertyName("pitch")]
    public double? Pitch { get; set; }

    /// <summary>
    /// Scan volume definition.
    /// </summary>
    [JsonPropertyName("scan_volume")]
    public LotAtcScanVolume? ScanVolume { get; set; }
}
