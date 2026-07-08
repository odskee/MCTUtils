namespace MCTUtils.CommunityStandards.Common;

/// <summary>
/// MCToolbox-specific extension data.
/// </summary>
public class MctoolboxExtensions
{
    /// <summary>
    /// MCToolbox internal package asset identifier.
    /// </summary>
    [JsonPropertyName("package_asset_id")]
    public string? PackageAssetId { get; set; }

    /// <summary>
    /// Name of the kneeboard template to use when generating kneeboards from this plan.
    /// </summary>
    [JsonPropertyName("kneeboard_template")]
    public string? KneeboardTemplate { get; set; }

    /// <summary>
    /// Hash of the Intel picture snapshot at the time of export, for consistency checking.
    /// </summary>
    [JsonPropertyName("intel_picture_hash")]
    public string? IntelPictureHash { get; set; }

    /// <summary>
    /// MCToolbox Operation ID this plan belongs to.
    /// </summary>
    [JsonPropertyName("op_id")]
    public Guid? OpId { get; set; }

    /// <summary>
    /// MCToolbox Mission ID this plan belongs to.
    /// </summary>
    [JsonPropertyName("mission_id")]
    public Guid? MissionId { get; set; }
}
