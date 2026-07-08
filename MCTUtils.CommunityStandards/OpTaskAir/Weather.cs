namespace MCTUtils.CommunityStandards.OpTaskAir;

/// <summary>
/// Meteorological conditions for the mission. Null if unknown or not applicable.
/// </summary>
public class Weather
{
    /// <summary>
    /// Surface temperature in degrees Celsius.
    /// </summary>
    [JsonPropertyName("temperature_c")]
    public double? TemperatureC { get; set; }

    /// <summary>
    /// Altimeter setting (QNH) in hectopascals (hPa / mbar).
    /// </summary>
    [JsonPropertyName("qnh_hpa")]
    public double? QnhHpa { get; set; }

    /// <summary>
    /// Surface visibility in metres.
    /// </summary>
    [JsonPropertyName("visibility_m")]
    public int? VisibilityM { get; set; }

    /// <summary>
    /// Wind layers.
    /// </summary>
    [JsonPropertyName("winds")]
    public Winds? Winds { get; set; }

    /// <summary>
    /// Cloud layer definition.
    /// </summary>
    [JsonPropertyName("clouds")]
    public CloudLayer? Clouds { get; set; }
}
