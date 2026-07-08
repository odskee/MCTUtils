namespace MCTUtils.CommunityStandards.OpTaskAir;

using MCTUtils.CommunityStandards.Common;

/// <summary>
/// Bullseye reference points per coalition.
/// </summary>
public class Bullseye
{
    /// <summary>
    /// Blue coalition bullseye reference.
    /// </summary>
    [JsonPropertyName("blue")]
    public LatLon? Blue { get; set; }

    /// <summary>
    /// Red coalition bullseye reference.
    /// </summary>
    [JsonPropertyName("red")]
    public LatLon? Red { get; set; }
}
