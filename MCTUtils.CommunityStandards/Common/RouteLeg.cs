namespace MCTUtils.CommunityStandards.Common;

/// <summary>
/// A navigational leg connecting two waypoints.
/// </summary>
public class RouteLeg
{
    /// <summary>
    /// Unique name for this leg within the route.
    /// </summary>
    [JsonPropertyName("leg_name")]
    public string LegName { get; set; } = string.Empty;

    /// <summary>
    /// Reference to the starting Waypoint object (waypoints[].id) for this leg.
    /// </summary>
    [JsonPropertyName("start_waypoint")]
    public Guid StartWaypoint { get; set; }

    /// <summary>
    /// Reference to the ending Waypoint object (waypoints[].id) for this leg.
    /// </summary>
    [JsonPropertyName("end_waypoint")]
    public Guid EndWaypoint { get; set; }

    /// <summary>
    /// ICAO Compliant Flight Rules value. Overrides Asset level value for this leg only.
    /// </summary>
    [JsonPropertyName("flightRules")]
    public FlightRules? FlightRules { get; set; }

    /// <summary>
    /// Flight environment visibility. Overrides Asset level value for this leg only.
    /// </summary>
    [JsonPropertyName("flightOversight")]
    public FlightOversight? FlightOversight { get; set; }
}
