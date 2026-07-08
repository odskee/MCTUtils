namespace MCTUtils.CommunityStandards.OpTaskAir;

using MCTUtils.CommunityStandards.Common;

/// <summary>
/// Road base / highway strip specific properties. Populated only when AirfieldDefinition.Type is ROAD_BASE.
/// </summary>
public class RoadBaseProperties
{
    /// <summary>
    /// Road or highway designation (e.g. 'Valtatie 4', 'E18 Kotka Strip').
    /// </summary>
    [JsonPropertyName("road_name")]
    public string? RoadName { get; set; }

    /// <summary>
    /// Surface type.
    /// </summary>
    [JsonPropertyName("surface")]
    public SurfaceType? Surface { get; set; }

    /// <summary>
    /// Usable landing/take-off run in metres after obstacles are cleared.
    /// </summary>
    [JsonPropertyName("usable_length_m")]
    public int? UsableLengthM { get; set; }

    /// <summary>
    /// Usable strip width in metres.
    /// </summary>
    [JsonPropertyName("width_m")]
    public int? WidthM { get; set; }

    /// <summary>
    /// Whether road furniture has been cleared for the exercise.
    /// </summary>
    [JsonPropertyName("obstacles_cleared")]
    public bool? ObstaclesCleared { get; set; }

    /// <summary>
    /// Temporary approach aids deployed.
    /// </summary>
    [JsonPropertyName("approach_aids")]
    public List<string>? ApproachAids { get; set; }
}
