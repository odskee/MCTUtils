using System.Text.Json;
using Json.Schema;

namespace MCTUtils.CommunityStandards.Validation;

/// <summary>
/// Validates serialized JSON documents against the published JSON schemas using JsonSchema.Net.
/// </summary>
public class JsonSchemaValidator : ISchemaValidator
{
    private static readonly HttpClient HttpClient = new();

    /// <inheritdoc />
    public async Task<ValidationResult> ValidateAsync(string json, Uri schemaUri, CancellationToken cancellationToken = default)
    {
        var schemaJson = await HttpClient.GetStringAsync(schemaUri, cancellationToken);
        var schema = JsonSchema.FromText(schemaJson);
        return ValidateInternal(json, schema);
    }

    /// <inheritdoc />
    public ValidationResult Validate(string json, string schema)
    {
        var schemaObj = JsonSchema.FromText(schema);
        return ValidateInternal(json, schemaObj);
    }

    private static ValidationResult ValidateInternal(string json, JsonSchema schema)
    {
        var result = new ValidationResult();

        JsonDocument document;
        try
        {
            document = JsonDocument.Parse(json);
        }
        catch (JsonException ex)
        {
            result.AddError(new ValidationError
            {
                Path = "(root)",
                Message = $"Failed to parse JSON: {ex.Message}",
                ErrorCode = "parse"
            });
            return result;
        }

        var evaluationResults = schema.Evaluate(document.RootElement, new EvaluationOptions
        {
            OutputFormat = OutputFormat.List
        });

        if (!evaluationResults.IsValid)
        {
            CollectErrors(evaluationResults, result);
        }

        return result;
    }

    private static void CollectErrors(EvaluationResults results, ValidationResult result)
    {
        if (results.Errors is { Count: > 0 })
        {
            foreach (var kvp in results.Errors)
            {
                var error = new ValidationError
                {
                    Path = results.InstanceLocation.ToString(),
                    Message = kvp.Value,
                    ErrorCode = kvp.Key
                };
                result.AddError(error);
            }
        }

        if (results.Details is { Count: > 0 })
        {
            foreach (var detail in results.Details)
            {
                CollectErrors(detail, result);
            }
        }
    }
}
