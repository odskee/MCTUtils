using System.Text.Json;
using FlightPlanDocument = MCTUtils.CommunityStandards.CommunityFlightPlan.CommunityFlightPlan;
using MCTUtils.CommunityStandards.Serialization;
using MCTUtils.CommunityStandards.Validation;

namespace MCTUtils.CommunityStandards.Utilities;

/// <summary>
/// Extension methods for <see cref="FlightPlanDocument"/>.
/// </summary>
public static class FlightPlanExtensions
{
    /// <summary>
    /// Creates a deep clone of this flight plan via JSON round-trip serialization.
    /// </summary>
    public static FlightPlanDocument DeepClone(this FlightPlanDocument plan)
    {
        var json = plan.ToJson();
        return FlightPlanDocument.FromJson(json);
    }

    /// <summary>
    /// Creates a copy of this flight plan (same as DeepClone).
    /// </summary>
    public static FlightPlanDocument Copy(this FlightPlanDocument plan)
    {
        return plan.DeepClone();
    }

    /// <summary>
    /// Validates this flight plan against the published JSON schema.
    /// </summary>
    /// <param name="plan">The flight plan to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> containing any errors found.</returns>
    public static async Task<ValidationResult> IsValid(this FlightPlanDocument plan)
    {
        var json = plan.ToJson();
        var validator = new JsonSchemaValidator();
        var schemaUri = new Uri(SchemaVersions.CommunityFlightPlanSchemaUrl);
        return await validator.ValidateAsync(json, schemaUri);
    }
}
