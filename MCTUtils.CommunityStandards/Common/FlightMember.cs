namespace MCTUtils.CommunityStandards.Common;

/// <summary>
/// One entry per flight member in the asset.
/// </summary>
public class FlightMember
{
    /// <summary>
    /// Role within the flight.
    /// </summary>
    [JsonPropertyName("role")]
    public FlightMemberRole? Role { get; set; }

    /// <summary>
    /// Aircraft tail / side number.
    /// </summary>
    [JsonPropertyName("tail_number")]
    public string? TailNumber { get; set; }

    /// <summary>
    /// DCS onboard number (used for kneeboard and DTC generation).
    /// </summary>
    [JsonPropertyName("onboard_number")]
    public string? OnboardNumber { get; set; }

    /// <summary>
    /// Pilot's callsign or name for personalisation.
    /// </summary>
    [JsonPropertyName("pilot_name")]
    public string? PilotName { get; set; }

    /// <summary>
    /// Identifies whether the flight member is currently controlled by a human
    /// participant or by AI. Supersedes control_type at the asset level if supplied.
    /// </summary>
    [JsonPropertyName("control_type")]
    public ControlType? ControlType { get; set; }

    /// <summary>
    /// IFF transponder settings.
    /// </summary>
    [JsonPropertyName("transponder")]
    public Transponder? Transponder { get; set; }

    /// <summary>
    /// Additional IFF and weapon guidance codes.
    /// </summary>
    [JsonPropertyName("codes")]
    public Codes? Codes { get; set; }
}
