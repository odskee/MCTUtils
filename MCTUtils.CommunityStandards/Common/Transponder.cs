namespace MCTUtils.CommunityStandards.Common;

/// <summary>
/// IFF transponder settings. Null if not applicable.
/// </summary>
public class Transponder
{
    /// <summary>
    /// Mode 1 code (2 digits). '00' = inactive.
    /// </summary>
    [JsonPropertyName("mode1")]
    public string? Mode1 { get; set; }

    /// <summary>
    /// Mode 2 code (4 digits). '0000' = inactive.
    /// </summary>
    [JsonPropertyName("mode2")]
    public string? Mode2 { get; set; }

    /// <summary>
    /// Mode 3/A code (4 octal digits). '0000' = inactive.
    /// </summary>
    [JsonPropertyName("mode3")]
    public string? Mode3 { get; set; }

    /// <summary>
    /// Mode 4 status.
    /// </summary>
    [JsonPropertyName("mode4")]
    public Mode4Status? Mode4 { get; set; }

    /// <summary>
    /// Mode S hex address (6 hex digits). '000000' = inactive.
    /// </summary>
    [JsonPropertyName("mode_s")]
    public string? ModeS { get; set; }

    /// <summary>
    /// Pre-set emergency squawk code.
    /// </summary>
    [JsonPropertyName("emergency_code")]
    public EmergencyCode? EmergencyCode { get; set; }
}
