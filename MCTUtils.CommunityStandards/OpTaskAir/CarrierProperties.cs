namespace MCTUtils.CommunityStandards.OpTaskAir;

using MCTUtils.CommunityStandards.Common;

/// <summary>
/// Carrier-specific properties. Populated only when AirfieldDefinition.Type is CARRIER.
/// </summary>
public class CarrierProperties
{
    /// <summary>
    /// Full ship name / hull designation.
    /// </summary>
    [JsonPropertyName("ship_name")]
    public string ShipName { get; set; } = string.Empty;

    /// <summary>
    /// Ship class (e.g. 'Nimitz', 'Ford', 'Queen Elizabeth').
    /// </summary>
    [JsonPropertyName("ship_class")]
    public string? ShipClass { get; set; }

    /// <summary>
    /// Planned Base Recovery Course (BRC) in degrees true.
    /// </summary>
    [JsonPropertyName("course_deg")]
    public double? CourseDeg { get; set; }

    /// <summary>
    /// Planned carrier speed in knots.
    /// </summary>
    [JsonPropertyName("speed_kts")]
    public double? SpeedKts { get; set; }

    /// <summary>
    /// TACAN channel.
    /// </summary>
    [JsonPropertyName("tacan_channel")]
    public int TacanChannel { get; set; }

    /// <summary>
    /// TACAN band.
    /// </summary>
    [JsonPropertyName("tacan_band")]
    public TacanBand TacanBand { get; set; }

    /// <summary>
    /// TACAN callsign (max 4 chars).
    /// </summary>
    [JsonPropertyName("tacan_callsign")]
    public string? TacanCallsign { get; set; }

    /// <summary>
    /// ICLS channel.
    /// </summary>
    [JsonPropertyName("icls_channel")]
    public int? IclsChannel { get; set; }

    /// <summary>
    /// Whether the carrier supports ACLS.
    /// </summary>
    [JsonPropertyName("acls_capable")]
    public bool? AclsCapable { get; set; }

    /// <summary>
    /// Link 4A datalink frequency in MHz for ACLS.
    /// </summary>
    [JsonPropertyName("link4a_frequency_mhz")]
    public double? Link4aFrequencyMhz { get; set; }

    /// <summary>
    /// Marshal / approach control frequency in MHz.
    /// </summary>
    [JsonPropertyName("marshal_frequency_mhz")]
    public double? MarshalFrequencyMhz { get; set; }

    /// <summary>
    /// LSO frequency in MHz.
    /// </summary>
    [JsonPropertyName("lso_frequency_mhz")]
    public double? LsoFrequencyMhz { get; set; }

    /// <summary>
    /// Planned deck state at recovery time.
    /// </summary>
    [JsonPropertyName("deck_state")]
    public DeckState? DeckState { get; set; }
}
