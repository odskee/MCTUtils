namespace MCTUtils.CommunityStandards.Validation;

/// <summary>
/// Represents a single validation error found during schema validation.
/// </summary>
public class ValidationError
{
    /// <summary>
    /// The JSON path where the error occurred.
    /// </summary>
    public string Path { get; init; } = string.Empty;

    /// <summary>
    /// A human-readable description of the validation error.
    /// </summary>
    public string Message { get; init; } = string.Empty;

    /// <summary>
    /// The error code or keyword that caused the failure (e.g. "required", "type", "enum").
    /// </summary>
    public string? ErrorCode { get; init; }

    /// <inheritdoc />
    public override string ToString() => $"{Path}: {Message}";
}
