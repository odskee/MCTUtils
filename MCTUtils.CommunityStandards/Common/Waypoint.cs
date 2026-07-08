namespace MCTUtils.CommunityStandards.Common;

/// <summary>
/// A single navigation point on a route.
/// </summary>
public class Waypoint
{
    /// <summary>
    /// Unique identifier for this waypoint.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Semantic type of this waypoint.
    /// </summary>
    [JsonPropertyName("type")]
    public WaypointType Type { get; set; }

    /// <summary>
    /// Reference to a Track object (tracks[].id) for this waypoint.
    /// </summary>
    [JsonPropertyName("track_id")]
    public Guid? TrackId { get; set; }

    /// <summary>
    /// When type is CUSTOM, a free-text label describing the waypoint purpose.
    /// </summary>
    [JsonPropertyName("custom_type")]
    public string? CustomType { get; set; }

    /// <summary>
    /// Human-readable waypoint name or identifier (e.g. 'IP ALPHA', 'UGKO').
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Latitude in decimal degrees (WGS-84). Positive = North.
    /// </summary>
    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }

    /// <summary>
    /// Longitude in decimal degrees (WGS-84). Positive = East.
    /// </summary>
    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }

    /// <summary>
    /// Planned altitude in feet. Null if not altitude-constrained.
    /// </summary>
    [JsonPropertyName("altitude_ft")]
    public double? AltitudeFt { get; set; }

    /// <summary>
    /// Altitude reference: mean sea level (MSL), above ground level (AGL), or flight level (FL).
    /// </summary>
    [JsonPropertyName("altitude_ref")]
    public AltitudeReference? AltitudeRef { get; set; }

    /// <summary>
    /// Planned airspeed. Null if unconstrained.
    /// </summary>
    [JsonPropertyName("speed")]
    public double? Speed { get; set; }

    /// <summary>
    /// Speed type qualifier.
    /// </summary>
    [JsonPropertyName("speed_type")]
    public SpeedType? SpeedType { get; set; }

    /// <summary>
    /// Estimated time of arrival at this waypoint in seconds since midnight (UTC).
    /// </summary>
    [JsonPropertyName("eta_seconds")]
    public int? EtaSeconds { get; set; }

    /// <summary>
    /// Hard time-on-point in seconds since midnight.
    /// </summary>
    [JsonPropertyName("fixed_seconds")]
    public int? FixedSeconds { get; set; }

    /// <summary>
    /// Time expected to leave this waypoint in seconds since midnight.
    /// </summary>
    [JsonPropertyName("leave_seconds")]
    public int? LeaveSeconds { get; set; }

    /// <summary>
    /// Planned track / heading in degrees true from this waypoint.
    /// </summary>
    [JsonPropertyName("track_deg")]
    public double? TrackDeg { get; set; }

    /// <summary>
    /// Short description of what occurs at this waypoint.
    /// </summary>
    [JsonPropertyName("activity")]
    public string? Activity { get; set; }

    /// <summary>
    /// Extended free-text notes for this waypoint.
    /// </summary>
    [JsonPropertyName("notes")]
    public string? Notes { get; set; }

    /// <summary>
    /// TACAN station identifier associated with this waypoint.
    /// </summary>
    [JsonPropertyName("tacan_ref")]
    public string? TacanRef { get; set; }

    /// <summary>
    /// Reference to an airfield at this waypoint.
    /// </summary>
    [JsonPropertyName("airfield_ref")]
    public string? AirfieldRef { get; set; }
}
