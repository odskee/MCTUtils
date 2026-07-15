namespace MCTUtils.CommunityStandards.Serialization;

/// <summary>
/// Contains the current schema version information for MCT Community Standards.
/// </summary>
public static class SchemaVersions
{
    /// <summary>
    /// The current version of the SDK / schemas.
    /// </summary>
    public static readonly string CurrentVersion = "2.0.0";

    /// <summary>
    /// Schema URL for the Community Flight Plan schema.
    /// </summary>
    public static readonly string CommunityFlightPlanSchemaUrl =
        "https://mctoolbox.uk/schema/v2.0.0/community-flightplan.schema.json";

    /// <summary>
    /// Schema URL for the Operational Air Task schema.
    /// </summary>
    public static readonly string OpTaskAirSchemaUrl =
        "https://mctoolbox.uk/schema/v2.0.0/op-task-air.schema.json";
}
