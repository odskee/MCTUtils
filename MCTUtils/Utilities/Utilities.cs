namespace MCTUtils.Utilities;

/// <summary>
/// General-purpose utility methods.
/// </summary>
public static class CommonUtilities
{
    /// <summary>
    /// Replaces characters that are invalid in Windows file names with the specified replacement string.
    /// </summary>
    public static string MakeSafeWindowsFileName(string value, string replaceWith = "_")
    {
        return value.Replace("<", replaceWith)
                    .Replace(">", replaceWith)
                    .Replace(":", replaceWith)
                    .Replace("\"", replaceWith)
                    .Replace("/", replaceWith)
                    .Replace("\\", replaceWith)
                    .Replace("|", replaceWith)
                    .Replace("?", replaceWith)
                    .Replace("*", replaceWith);
    }

    /// <summary>
    /// Truncates a double to the specified number of decimal digits.
    /// </summary>
    public static double TruncateDouble(double? value, int digits)
    {
        double mult = Math.Pow(10.0, digits);
        return Math.Truncate(value ?? 0 * mult) / mult;
    }
}
