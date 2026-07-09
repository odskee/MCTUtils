namespace MCTUtils.CommunityStandards.Common;

/// <summary>
/// A flight asset participating in the package.
/// </summary>
public class Asset
{
    /// <summary>
    /// Unique identifier for this asset.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Unique identifier of the package the asset belongs to.
    /// </summary>
    [JsonPropertyName("package_id")]
    public Guid PackageId { get; set; }

    /// <summary>
    /// Flight callsign (e.g. 'VIPER 1-1').
    /// </summary>
    [JsonPropertyName("callsign")]
    public string Callsign { get; set; } = string.Empty;

    /// <summary>
    /// Tactical callsign, if blank assumed to be the same as callsign.
    /// </summary>
    [JsonPropertyName("tactical_callsign")]
    public string? TacticalCallsign { get; set; }

    /// <summary>
    /// The number of expected flight members for an asset.
    /// </summary>
    [JsonPropertyName("flight_number")]
    public int FlightNumber { get; set; }

    /// <summary>
    /// ICAO Compliant Flight Type value.
    /// </summary>
    [JsonPropertyName("flight_type")]
    public FlightType? FlightType { get; set; }

    /// <summary>
    /// ICAO Compliant Flight Rules value.
    /// </summary>
    [JsonPropertyName("flight_rules")]
    public FlightRules? FlightRules { get; set; }

    /// <summary>
    /// Flight environment visibility.
    /// </summary>
    [JsonPropertyName("flight_oversight")]
    public FlightOversight? FlightOversight { get; set; }

    /// <summary>
    /// Flight control type.
    /// </summary>
    [JsonPropertyName("control_type")]
    public ControlType? ControlType { get; set; }

    /// <summary>
    /// Flight member information.
    /// </summary>
    [JsonPropertyName("flight_members")]
    public List<FlightMember>? FlightMembers { get; set; }

    /// <summary>
    /// Aircraft type designation (e.g. 'F-16CM', 'F/A-18C').
    /// </summary>
    [JsonPropertyName("airframe")]
    public string Airframe { get; set; } = string.Empty;

    /// <summary>
    /// Controlled vocabulary of mission types.
    /// </summary>
    [JsonPropertyName("mission_type")]
    public MissionType MissionType { get; set; }

    /// <summary>
    /// Free-text description of the primary target or objective.
    /// </summary>
    [JsonPropertyName("primary_target")]
    public string? PrimaryTarget { get; set; }

    /// <summary>
    /// Array of DMPI coordinates assigned to this package.
    /// </summary>
    [JsonPropertyName("dmpi_refs")]
    public List<LatLon>? DmpiRefs { get; set; }

    /// <summary>
    /// Reference to the Route object (routes[].id) for this asset.
    /// </summary>
    [JsonPropertyName("route_id")]
    public Guid RouteId { get; set; }

    /// <summary>
    /// Free-text notes specific to this asset.
    /// </summary>
    [JsonPropertyName("notes")]
    public string? Notes { get; set; }
}
