namespace MCTUtils.CommunityStandards.Validation;

/// <summary>
/// The result of schema validation containing any errors found.
/// </summary>
public class ValidationResult
{
    private readonly List<ValidationError> _errors = [];

    /// <summary>
    /// Whether the document is valid according to the schema.
    /// </summary>
    public bool IsValid => _errors.Count == 0;

    /// <summary>
    /// The list of validation errors found.
    /// </summary>
    public IReadOnlyList<ValidationError> Errors => _errors.AsReadOnly();

    /// <summary>
    /// Adds a validation error.
    /// </summary>
    internal void AddError(ValidationError error) => _errors.Add(error);

    /// <summary>
    /// Adds a range of validation errors.
    /// </summary>
    internal void AddErrors(IEnumerable<ValidationError> errors) => _errors.AddRange(errors);
}
