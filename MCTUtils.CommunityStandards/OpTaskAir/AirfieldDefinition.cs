namespace MCTUtils.CommunityStandards.OpTaskAir;

using MCTUtils.CommunityStandards.Common;

/// <summary>
/// Definition of a non-standard or custom airfield / operating location.
/// </summary>
public class AirfieldDefinition
{
    /// <summary>
    /// Unique identifier for this airfield definition, used as the airfield_ref value on waypoints.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Classification of the operating location.
    /// </summary>
    [JsonPropertyName("type")]
    public AirfieldType Type { get; set; }

    /// <summary>
    /// Human-readable name (e.g. 'Valtatie 4 Strip', 'CVN-71 Theodore Roosevelt', 'FARP EAGLE').
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// ICAO code if one exists.
    /// </summary>
    [JsonPropertyName("icao_code")]
    public string? IcaoCode { get; set; }

    /// <summary>
    /// Latitude in decimal degrees (WGS-84).
    /// </summary>
    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }

    /// <summary>
    /// Longitude in decimal degrees (WGS-84).
    /// </summary>
    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }

    /// <summary>
    /// Elevation above MSL in feet.
    /// </summary>
    [JsonPropertyName("elevation_ft")]
    public double? ElevationFt { get; set; }

    /// <summary>
    /// Local magnetic variation in degrees. Positive = East, negative = West.
    /// </summary>
    [JsonPropertyName("magnetic_variation_deg")]
    public double? MagneticVariationDeg { get; set; }

    /// <summary>
    /// Runway or landing surface definitions at this location.
    /// </summary>
    [JsonPropertyName("runways")]
    public List<RunwayDefinition>? Runways { get; set; }

    /// <summary>
    /// Carrier-specific properties. Populated only when Type is CARRIER.
    /// </summary>
    [JsonPropertyName("carrier")]
    public CarrierProperties? Carrier { get; set; }

    /// <summary>
    /// Road base / highway strip specific properties. Populated only when Type is ROAD_BASE.
    /// </summary>
    [JsonPropertyName("road_base")]
    public RoadBaseProperties? RoadBase { get; set; }

    /// <summary>
    /// FARP-specific properties. Populated only when Type is FARP.
    /// </summary>
    [JsonPropertyName("farp")]
    public FarpProperties? Farp { get; set; }

    /// <summary>
    /// Communications channels for this airfield.
    /// </summary>
    [JsonPropertyName("comms")]
    public List<CommsEntry>? Comms { get; set; }

    /// <summary>
    /// Free-text notes (NOTAMs, special instructions, etc.).
    /// </summary>
    [JsonPropertyName("notes")]
    public string? Notes { get; set; }
}
