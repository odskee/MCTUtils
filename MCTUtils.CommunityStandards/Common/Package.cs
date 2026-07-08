namespace MCTUtils.CommunityStandards.Common;

/// <summary>
/// Groups assets into a tactical unit and holds shared comms and intel references.
/// </summary>
public class Package
{
    /// <summary>
    /// Unique identifier for this package.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Human-readable package name (e.g. 'PACKAGE SWIFT').
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Callsign of the package commander / mission commander.
    /// </summary>
    [JsonPropertyName("package_commander")]
    public string? PackageCommander { get; set; }

    /// <summary>
    /// Ordered list of communications entries shared across the package.
    /// </summary>
    [JsonPropertyName("comms_plan")]
    public List<CommsEntry>? CommsPlan { get; set; }

    /// <summary>
    /// Free-text planning notes for the package.
    /// </summary>
    [JsonPropertyName("notes")]
    public string? Notes { get; set; }
}
