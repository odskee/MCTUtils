namespace MCTUtils.CommunityStandards.OpTaskAir;

/// <summary>
/// A single wind observation at a given level.
/// </summary>
public class WindLayer
{
    /// <summary>
    /// Wind direction in degrees true (the direction the wind is coming FROM).
    /// </summary>
    [JsonPropertyName("from_deg")]
    public double FromDeg { get; set; }

    /// <summary>
    /// Wind speed in knots.
    /// </summary>
    [JsonPropertyName("speed_kts")]
    public double SpeedKts { get; set; }

    /// <summary>
    /// Altitude MSL in feet (present only in altitude wind layer entries).
    /// </summary>
    [JsonPropertyName("altitude_ft")]
    public int? AltitudeFt { get; set; }
}
