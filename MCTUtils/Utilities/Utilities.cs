using System.Text.RegularExpressions;

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
    /// Attempts to find an enum value of type TEnum that matches the provided string value, ignoring case. Returns true if a match is found, otherwise false.
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="value"></param>
    /// <param name="result"></param>
    /// <returns>Enum if found.</returns>
    public static bool TryFindEnum<TEnum>(string value, out TEnum result)
    where TEnum : struct, Enum
    {
        return Enum.TryParse(value, ignoreCase: true, out result);
    }


    /// <summary>
    /// Attempts to find an enum value of type TEnum that matches the provided string value, ignoring case.
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="value"></param>
    /// <returns>Enum if found or null if not found.</returns>
    public static TEnum? FindEnumByString<TEnum>(string value)
    where TEnum : struct, Enum
    {
        if (Enum.TryParse<TEnum>(value, ignoreCase: true, out var result))
        {
            return result;
        }

        return null;
    }
}
