namespace MCTUtils.CommunityStandards.OpTaskAir;

using MCTUtils.CommunityStandards.Common;

/// <summary>
/// Anchors the plan in time, space, and environment.
/// </summary>
public class MissionContext
{
    /// <summary>
    /// Map / theatre name as recognised by the target sim.
    /// </summary>
    [JsonPropertyName("theatre")]
    public string Theatre { get; set; } = string.Empty;

    /// <summary>
    /// Mission UTC date in ISO 8601 format (YYYY-MM-DD).
    /// </summary>
    [JsonPropertyName("date")]
    public DateOnly Date { get; set; }

    /// <summary>
    /// Mission start time expressed as seconds elapsed since midnight (UTC).
    /// </summary>
    [JsonPropertyName("time_seconds")]
    public int TimeSeconds { get; set; }

    /// <summary>
    /// Bullseye reference points per coalition.
    /// </summary>
    [JsonPropertyName("bullseye")]
    public Bullseye? Bullseye { get; set; }

    /// <summary>
    /// Meteorological conditions for the mission.
    /// </summary>
    [JsonPropertyName("weather")]
    public Weather? Weather { get; set; }
}
