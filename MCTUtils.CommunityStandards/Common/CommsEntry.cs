namespace MCTUtils.CommunityStandards.Common;

/// <summary>
/// A single communications channel definition.
/// </summary>
public class CommsEntry
{
    /// <summary>
    /// Functional role for this frequency (e.g. 'ASSET', 'GCI', 'TANKER').
    /// </summary>
    [JsonPropertyName("role")]
    public CommsRole? Role { get; set; }

    /// <summary>
    /// Human-readable label for the channel (e.g. 'STRIKE COMMON', 'MAGIC 1').
    /// </summary>
    [JsonPropertyName("label")]
    public string? Label { get; set; }

    /// <summary>
    /// Frequency in megahertz.
    /// </summary>
    [JsonPropertyName("frequency_mhz")]
    public double FrequencyMhz { get; set; }

    /// <summary>
    /// Radio modulation type.
    /// </summary>
    [JsonPropertyName("modulation")]
    public Modulation Modulation { get; set; }

    /// <summary>
    /// Callsign of the station on this frequency (e.g. 'MAGIC', 'SHELL 1').
    /// </summary>
    [JsonPropertyName("callsign")]
    public string? Callsign { get; set; }
}
