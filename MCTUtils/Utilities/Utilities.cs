namespace MCTUtils.Utilities;

/// <summary>
/// General-purpose utility methods.
/// </summary>
public static class CommonUtilities
{
    /// <summary>
    /// Makes a string safe for use as a Windows file name by replacing invalid characters with a specified replacement string.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="replaceWith"></param>
    /// <returns>The sanitized string</returns>
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
    /// Truncates a nullable double to the specified number of decimal places without rounding.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="digits"></param>
    /// <returns>The truncated double value</returns>
    public static double TruncateDouble(double? value, int digits)
    {
        double mult = Math.Pow(10.0, digits);
        return Math.Truncate(value ?? 0 * mult) / mult;
    }



}
