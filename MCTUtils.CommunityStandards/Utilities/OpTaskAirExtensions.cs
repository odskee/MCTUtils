using System.Text.Json;
using OpTaskAirDocument = MCTUtils.CommunityStandards.OpTaskAir.OpTaskAir;
using MCTUtils.CommunityStandards.Serialization;
using MCTUtils.CommunityStandards.Validation;

namespace MCTUtils.CommunityStandards.Utilities;

/// <summary>
/// Extension methods for <see cref="OpTaskAirDocument"/>.
/// </summary>
public static class OpTaskAirExtensions
{
    /// <summary>
    /// Creates a deep clone of this Op Task Air via JSON round-trip serialization.
    /// </summary>
    public static OpTaskAirDocument DeepClone(this OpTaskAirDocument opTaskAir)
    {
        var json = opTaskAir.ToJson();
        return OpTaskAirDocument.FromJson(json);
    }

    /// <summary>
    /// Creates a copy of this Op Task Air (same as DeepClone).
    /// </summary>
    public static OpTaskAirDocument Copy(this OpTaskAirDocument opTaskAir)
    {
        return opTaskAir.DeepClone();
    }

    /// <summary>
    /// Validates this Op Task Air against the published JSON schema.
    /// </summary>
    /// <param name="opTaskAir">The Op Task Air to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> containing any errors found.</returns>
    public static async Task<ValidationResult> IsValid(this OpTaskAirDocument opTaskAir)
    {
        var json = opTaskAir.ToJson();
        var validator = new JsonSchemaValidator();
        var schemaUri = new Uri(SchemaVersions.OpTaskAirSchemaUrl);
        return await validator.ValidateAsync(json, schemaUri);
    }
}
