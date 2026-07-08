using System.Text.Json;

namespace MCTUtils.CommunityStandards.Common;

/// <summary>
/// Tool-specific extension data. Each key is a tool namespace (e.g. 'mctoolbox', 'lotatc', 'dcs_dtc').
/// Parsers from other tools must silently ignore namespaces they do not recognise.
/// </summary>
public class Extensions
{
    /// <summary>
    /// MCToolbox-specific extension data.
    /// </summary>
    [JsonPropertyName("mctoolbox")]
    public MctoolboxExtensions? Mctoolbox { get; set; }

    /// <summary>
    /// LotATC-specific extension data.
    /// </summary>
    [JsonPropertyName("lotatc")]
    public LotAtcExtensions? LotAtc { get; set; }

    /// <summary>
    /// DCS-DTC / native DTC export extension data.
    /// </summary>
    [JsonPropertyName("dcs_dtc")]
    public DcsDtcExtensions? DcsDtc { get; set; }

    /// <summary>
    /// Tool-specific extension data from other tools. Each key is a tool namespace.
    /// </summary>
    [JsonExtensionData]
    public Dictionary<string, JsonElement>? OtherExtensions { get; set; }
}
