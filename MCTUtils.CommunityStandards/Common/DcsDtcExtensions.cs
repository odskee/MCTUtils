namespace MCTUtils.CommunityStandards.Common;

/// <summary>
/// DCS-DTC / native DTC export extension data.
/// </summary>
public class DcsDtcExtensions
{
    /// <summary>
    /// DTC profile name (e.g. 'f16-viper', 'f18-hornet').
    /// </summary>
    [JsonPropertyName("profile")]
    public string? Profile { get; set; }

    /// <summary>
    /// Selected countermeasures (CMDS) program number.
    /// </summary>
    [JsonPropertyName("cmds_program")]
    public int? CmdsProgram { get; set; }

    /// <summary>
    /// MFD page configuration, structure depends on airframe.
    /// </summary>
    [JsonPropertyName("mfd_pages")]
    public Dictionary<string, object?>? MfdPages { get; set; }
}
