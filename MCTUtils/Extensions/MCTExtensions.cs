namespace MCTUtils;

public static class MCTExtensions
{
    /// <summary>
    /// Converts the first character of a string to uppercase.
    /// </summary>
    /// <param name="input"></param>
    /// <returns>The string with the first character converted to uppercase</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static string FirstCharToUpper(this string input) =>
    input switch
    {
        null => throw new ArgumentNullException(nameof(input)),
        "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
        _ => string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1))
    };


    /// <summary>
    /// Truncates a nullable double to the specified number of decimal places without rounding.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="digits"></param>
    /// <returns>A truncated double</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static double TruncateTo(this double? input, int digits) =>
    input switch
    {
        null => throw new ArgumentNullException(nameof(input)),
        0 => throw new ArgumentException($"{nameof(input)} cannot be zero", nameof(input)),
        _ => Math.Truncate(input ?? 0 * Math.Pow(10.0, digits)) / Math.Pow(10.0, digits)
    };


    /// <summary>
    /// Truncates a double to the specified number of decimal places without rounding.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="digits"></param>
    /// <returns>A truncated double</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static double TruncateTo(this double input, int digits) =>
    input switch
    {
        0 => throw new ArgumentException($"{nameof(input)} cannot be zero", nameof(input)),
        _ => Math.Truncate(input * Math.Pow(10.0, digits)) / Math.Pow(10.0, digits)
    };


    /// <summary>
    /// Selects a specified number of random elements from a list without replacement.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="input"></param>
    /// <param name="number"></param>
    /// <returns>List of randomly selected elements</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static List<T> SelectRandom<T>(this List<T> input, int number)
    {
        if (input == null) throw new ArgumentNullException(nameof(input));
        if (number < 0 || number > input.Count) throw new ArgumentOutOfRangeException(nameof(number));

        // Copy to avoid mutating original list
        var copy = new List<T>(input);

        // Partial Fisher–Yates shuffle (only shuffle what we need)
        for (int i = 0; i < number; i++)
        {
            int j = Random.Shared.Next(i, copy.Count);
            (copy[i], copy[j]) = (copy[j], copy[i]);
        }

        return copy.GetRange(0, number);
    }


}