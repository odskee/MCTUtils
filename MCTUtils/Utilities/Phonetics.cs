namespace MCTUtils.Utilities;

public static class Phonetics
{
    private static Dictionary<char, string> PhoneticDict = new Dictionary<char, string>()
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

    public static string Word(char ch)
    {
        return PhoneticDict[ch];
    }

    public static string Word(int index)
    {
        return PhoneticDict.ToList().ElementAt(index - 1).Value;
    }
}