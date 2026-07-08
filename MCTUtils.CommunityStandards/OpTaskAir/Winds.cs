namespace MCTUtils.CommunityStandards.OpTaskAir;

/// <summary>
/// Wind layers. Surface wind required; altitude layers optional.
/// </summary>
public class Winds
{
    /// <summary>
    /// Surface wind observation.
    /// </summary>
    [JsonPropertyName("surface")]
    public WindLayer? Surface { get; set; }

    /// <summary>
    /// Upper wind layers ordered by ascending altitude.
    /// </summary>
    [JsonPropertyName("altitude")]
    public List<WindLayer>? Altitude { get; set; }
}
