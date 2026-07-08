namespace MCTUtils.CommunityStandards.OpTaskAir;

using MCTUtils.CommunityStandards.Common;

/// <summary>
/// FARP-specific properties. Populated only when AirfieldDefinition.Type is FARP.
/// </summary>
public class FarpProperties
{
    /// <summary>
    /// FARP designation or callsign (e.g. 'FARP EAGLE').
    /// </summary>
    [JsonPropertyName("designation")]
    public string? Designation { get; set; }

    /// <summary>
    /// Available fuel grades.
    /// </summary>
    [JsonPropertyName("fuel_types")]
    public List<FuelType>? FuelTypes { get; set; }

    /// <summary>
    /// Total fuel capacity in pounds.
    /// </summary>
    [JsonPropertyName("capacity_lbs")]
    public int? CapacityLbs { get; set; }

    /// <summary>
    /// Whether rearming capability is available.
    /// </summary>
    [JsonPropertyName("rearming")]
    public bool? Rearming { get; set; }

    /// <summary>
    /// FARP operations frequency in MHz.
    /// </summary>
    [JsonPropertyName("frequency_mhz")]
    public double? FrequencyMhz { get; set; }

    /// <summary>
    /// Number of simultaneous servicing pads.
    /// </summary>
    [JsonPropertyName("pad_count")]
    public int? PadCount { get; set; }
}
