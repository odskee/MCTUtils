namespace MCTUtils.Utilities;

/// <summary>
/// Provides phonetic representations for letters of the alphabet using the NATO phonetic alphabet.
/// </summary>
public static class Phonetics
{
    private static readonly Dictionary<char, string> PhoneticDict = new()
    {
        { 'a', "Alpha"},
        { 'b', "Bravo"},
        { 'c', "Charlie"},
        { 'd', "Delta"},
        { 'e', "Echo"},
        { 'f', "Foxtrot"},
        { 'g', "Golf"},
        { 'h', "Hotel"},
        { 'i', "India"},
        { 'j', "Juliet"},
        { 'k', "Kilo"},
        { 'l', "Lima"},
        { 'm', "Mike"},
        { 'n', "November"},
        { 'o', "Oscar"},
        { 'p', "Papa"},
        { 'q', "Quebec"},
        { 'r', "Romeo"},
        { 's', "Sierra"},
        { 't', "Tango"},
        { 'u', "Uniform"},
        { 'v', "Victor"},
        { 'w', "Whisky"},
        { 'x', "X-Ray"},
        { 'y', "Yankee"},
        { 'z', "Zulu"}
    };

    /// <summary>
    /// Returns the phonetic representation of a given character using the NATO phonetic alphabet.
    /// </summary>
    /// <param name="ch"></param>
    /// <returns>The phonetic representation of the character.</returns>
    public static string Word(char ch)
    {
        return PhoneticDict[Char.ToLower(ch)];
    }


    /// <summary>
    /// Returns the phonetic representation of a letter based on its position in the alphabet (1 for 'a', 2 for 'b', ..., 26 for 'z').
    /// </summary>
    /// <param name="index"></param>
    /// <returns>The phonetic representation of the letter at the specified index.</returns>
    public static string Word(int index)
    {
        return PhoneticDict.ToList().ElementAt(index - 1).Value;
    }
}