namespace MCTUtils.CommunityStandards.OpTaskAir;

using MCTUtils.CommunityStandards.Common;

/// <summary>
/// A single runway, landing strip, or carrier landing spot.
/// </summary>
public class RunwayDefinition
{
    /// <summary>
    /// Runway or spot identifier (e.g. '13', '31L', 'SPOT 3', 'N/S STRIP').
    /// </summary>
    [JsonPropertyName("identifier")]
    public string Identifier { get; set; } = string.Empty;

    /// <summary>
    /// True heading of the landing direction.
    /// </summary>
    [JsonPropertyName("track_deg")]
    public double? TrackDeg { get; set; }

    /// <summary>
    /// Length in metres.
    /// </summary>
    [JsonPropertyName("length_m")]
    public int? LengthM { get; set; }

    /// <summary>
    /// Width in metres.
    /// </summary>
    [JsonPropertyName("width_m")]
    public int? WidthM { get; set; }

    /// <summary>
    /// Surface type. Use DECK for carrier landing areas.
    /// </summary>
    [JsonPropertyName("surface")]
    public SurfaceType? Surface { get; set; }

    /// <summary>
    /// ILS localiser frequency in MHz. Null if no ILS.
    /// </summary>
    [JsonPropertyName("ils_frequency_mhz")]
    public double? IlsFrequencyMhz { get; set; }

    /// <summary>
    /// ILS inbound course in degrees true.
    /// </summary>
    [JsonPropertyName("ils_course_deg")]
    public double? IlsCourseDeg { get; set; }

    /// <summary>
    /// Latitude of the runway threshold.
    /// </summary>
    [JsonPropertyName("threshold_latitude")]
    public double? ThresholdLatitude { get; set; }

    /// <summary>
    /// Longitude of the runway threshold.
    /// </summary>
    [JsonPropertyName("threshold_longitude")]
    public double? ThresholdLongitude { get; set; }
}
