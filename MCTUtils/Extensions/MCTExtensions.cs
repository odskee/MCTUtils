using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MCTUtils;

public static class MCTExtensions
{
    private static readonly ConcurrentDictionary<Enum, string> _enumToString = new();
    private static readonly ConcurrentDictionary<Type, Dictionary<string, object>> _stringToEnum = new();



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

    /// <summary>
    /// Gets the display name for an enum value using its <see cref="DisplayAttribute"/>,
    /// or the member name if no attribute is present. Results are cached in a
    /// <see cref="ConcurrentDictionary{TKey, TValue}"/> for performance.
    /// </summary>
    /// <param name="value">The enum value to convert.</param>
    /// <returns>The display name, or the member name if no <c>[Display]</c> attribute exists.</returns>
    public static string ToString(Enum value)
    {
        return _enumToString.GetOrAdd(value, e =>
        {
            var fieldInfo = e.GetType().GetField(e.ToString());

            var displayAttributes = fieldInfo?.GetCustomAttributes(
                typeof(DisplayAttribute), false) as DisplayAttribute[];

            if (displayAttributes == null || displayAttributes.Length == 0)
                return e.ToString();

            return displayAttributes[0].Name ?? e.ToString();
        });
    }

    /// <summary>
    /// Parses a display name string back to its enum value using <see cref="DisplayAttribute"/>.
    /// The lookup is case-insensitive and results are cached in a
    /// <see cref="ConcurrentDictionary{TKey, TValue}"/> for performance.
    /// </summary>
    /// <typeparam name="T">The enum type to parse into.</typeparam>
    /// <param name="value">The display name to parse.</param>
    /// <returns>The matching enum value.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="value"/> does not match any member's display name or member name.</exception>
    public static T FromString<T>(string value) where T : struct, Enum
    {
        var lookup = _stringToEnum.GetOrAdd(typeof(T), type =>
        {
            var dict = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var enumValue = field.GetValue(null)!;

                var displayAttributes = field.GetCustomAttributes(
                    typeof(DisplayAttribute), false) as DisplayAttribute[];

                var displayName = (displayAttributes != null && displayAttributes.Length > 0)
                    ? displayAttributes[0].Name
                    : field.Name;

                dict[displayName!] = enumValue;
            }

            return dict;
        });

        if (lookup.TryGetValue(value, out var result))
            return (T)result;

        throw new ArgumentException(
            $"'{value}' is not a valid display value for enum {typeof(T).Name}.",
            nameof(value));
    }
}