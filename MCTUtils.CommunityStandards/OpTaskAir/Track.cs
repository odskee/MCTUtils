namespace MCTUtils.CommunityStandards.OpTaskAir;

using MCTUtils.CommunityStandards.Common;

/// <summary>
/// A track that one or more assets can use.
/// </summary>
public class Track
{
    /// <summary>
    /// Unique identifier for this track.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Name of the initial fix for this track, if known.
    /// </summary>
    [JsonPropertyName("fix_name")]
    public string? FixName { get; set; }

    /// <summary>
    /// The True bearing to enter this track.
    /// </summary>
    [JsonPropertyName("initial_bearing")]
    public int InitialBearing { get; set; }

    /// <summary>
    /// The altitude in feet to enter this track.
    /// </summary>
    [JsonPropertyName("initial_altitude")]
    public int InitialAltitude { get; set; }

    /// <summary>
    /// The altitude in feet between each stacked aircraft.
    /// </summary>
    [JsonPropertyName("altitude_seperation")]
    public int? AltitudeSeparation { get; set; }

    /// <summary>
    /// The length in nautical miles of this track.
    /// </summary>
    [JsonPropertyName("track_length")]
    public int TrackLength { get; set; }

    /// <summary>
    /// The width in nautical miles of this track.
    /// </summary>
    [JsonPropertyName("track_width")]
    public int TrackWidth { get; set; }

    /// <summary>
    /// A list of outer coordinates that define this track's shape, starting with the entry.
    /// </summary>
    [JsonPropertyName("points")]
    public List<LatLon>? Points { get; set; }
}
