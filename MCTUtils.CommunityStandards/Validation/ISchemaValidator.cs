namespace MCTUtils.CommunityStandards.Validation;

/// <summary>
/// Validates serialized JSON documents against their published JSON schemas.
/// </summary>
public interface ISchemaValidator
{
    /// <summary>
    /// Validates a JSON string against a JSON schema loaded from the given URI.
    /// </summary>
    /// <param name="json">The JSON string to validate.</param>
    /// <param name="schemaUri">The URI of the JSON schema to validate against.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A <see cref="ValidationResult"/> containing any errors found.</returns>
    Task<ValidationResult> ValidateAsync(string json, Uri schemaUri, CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates a JSON string against a pre-loaded JSON schema.
    /// </summary>
    /// <param name="json">The JSON string to validate.</param>
    /// <param name="schema">The pre-loaded JSON schema text.</param>
    /// <returns>A <see cref="ValidationResult"/> containing any errors found.</returns>
    ValidationResult Validate(string json, string schema);
}
