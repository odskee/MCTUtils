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




}
