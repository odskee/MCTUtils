namespace MCTUtils.CommunityStandards.Common;

/// <summary>
/// Additional IFF and weapon guidance codes. Null if not applicable.
/// </summary>
public class Codes
{
    /// <summary>
    /// Laser designation code (4 digits, each 1-9).
    /// </summary>
    [JsonPropertyName("laser_code")]
    public int? LaserCode { get; set; }

    /// <summary>
    /// Laser pulse repetition frequency identifier if required.
    /// </summary>
    [JsonPropertyName("laser_pulse")]
    public string? LaserPulse { get; set; }

    /// <summary>
    /// SADL / JTIDS track identity for A-10 / F-15E coordination.
    /// </summary>
    [JsonPropertyName("sadl_id")]
    public string? SadlId { get; set; }
}
