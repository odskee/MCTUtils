namespace MCTUtils.CommunityStandards.OpTaskAir;

/// <summary>
/// Cloud layer definition.
/// </summary>
public class CloudLayer
{
    /// <summary>
    /// Cloud density from 0 (clear) to 9 (overcast).
    /// </summary>
    [JsonPropertyName("density")]
    public int Density { get; set; }

    /// <summary>
    /// Cloud base altitude MSL in feet.
    /// </summary>
    [JsonPropertyName("base_ft")]
    public int? BaseFt { get; set; }

    /// <summary>
    /// Cloud layer thickness in feet.
    /// </summary>
    [JsonPropertyName("thickness_ft")]
    public int? ThicknessFt { get; set; }
}
