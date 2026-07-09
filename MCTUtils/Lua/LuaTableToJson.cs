
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace MCTUtils.Lua
{
    /// <summary>
    /// Converts serialised Lua tables (DCS World mission / warehouse files) to JSON.
    /// <para>
    ///   Array detection: a table whose keys are all consecutive integers
    ///   starting at 1 is written as a JSON array; any other key combination
    ///   becomes a JSON object.
    /// </para>
    /// </summary>
    public static class LuaTableToJson
    {
        /// <summary>
        /// Convert from an input <see cref="Stream"/> (read) to an output
        /// <see cref="Stream"/> (write).  Streams may be arbitrarily large;
        /// only one table node at a time is held in memory.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        /// <param name="indented"></param>
        public static void Convert(Stream input, Stream output, bool indented = false)
        {
            using var reader = new StreamReader(input, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, bufferSize: 65536, leaveOpen: true);
            Convert(reader, output, indented);
            output.Position = 0;    // Caller can read from the beginning of the output stream after
        }

        /// <summary>Convert from a <see cref="TextReader"/>.</summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        /// <param name="indented"></param>
        public static void Convert(TextReader input, Stream output, bool indented = false)
        {
            var lexer = new Lexer(input);
            var parser = new Parser(lexer);
            LuaValue root = parser.ParseTopLevel();

            var opts = new JsonWriterOptions { Indented = indented, SkipValidation = false };
            using var writer = new Utf8JsonWriter(output, opts);
            root.WriteTo(writer);
            writer.Flush();
            output.Position = 0;    // Caller can read from the beginning of the output stream after
        }

        /// <summary>
        /// Convert from a string containing Lua table text to a JSON string.
        /// </summary>
        /// <param name="luaText"></param>
        /// <param name="indented"></param>
        /// <returns>string representing JSON</returns>
        public static string Convert(string luaText, bool indented = false)
        {
            using var ms = new MemoryStream(Encoding.UTF8.GetBytes(luaText));
            using var outMs = new MemoryStream();
            Convert(ms, outMs, indented);
            return Encoding.UTF8.GetString(outMs.ToArray());
        }
    }

    /// <summary>
    /// Abstract base class for a Lua value (string, number, boolean, nil, or table).
    /// </summary>
    internal abstract class LuaValue
    {
        public abstract void WriteTo(Utf8JsonWriter w);
    }

    internal sealed class LuaString(string value) : LuaValue
    {
        public string Value { get; } = value;
        public override void WriteTo(Utf8JsonWriter w) => w.WriteStringValue(Value);
    }

    internal sealed class LuaInteger(long value) : LuaValue
    {
        public long Value { get; } = value;
        public override void WriteTo(Utf8JsonWriter w) => w.WriteNumberValue(Value);
    }

    internal sealed class LuaFloat(double value) : LuaValue
    {
        public double Value { get; } = value;
        public override void WriteTo(Utf8JsonWriter w) => w.WriteNumberValue(Value);
    }

    internal sealed class LuaBool(bool value) : LuaValue
    {
        public bool Value { get; } = value;
        public override void WriteTo(Utf8JsonWriter w) => w.WriteBooleanValue(Value);
    }

    internal sealed class LuaNil : LuaValue
    {
        public static readonly LuaNil Instance = new();
        public override void WriteTo(Utf8JsonWriter w) => w.WriteNullValue();
    }

    /// <summary>
    /// A Lua table entry.  <see cref="Key"/> is one of:
    /// <list type="bullet">
    ///   <item><c>null</c>  � positional (implicit integer index)</item>
    ///   <item><c>long</c>  � explicit integer key  <c>[N]</c></item>
    ///   <item><c>string</c> � explicit string key  <c>["k"]</c></item>
    /// </list>
    /// </summary>
    internal readonly struct TableEntry(object? key, LuaValue value)
    {
        public object? Key { get; } = key;
        public LuaValue Value { get; } = value;
    }


    /// <summary>
    /// A Lua table, which is a collection of <see cref="TableEntry"/> items.
    /// </summary>
    internal sealed class LuaTable : LuaValue
    {
        private readonly List<TableEntry> _entries;

        /// <summary>
        /// Initializes a new instance of the <see cref="LuaTable"/> class with the specified entries.
        /// </summary>
        /// <param name="entries"></param>
        public LuaTable(List<TableEntry> entries) => _entries = entries;

        private enum TableKind
        {
            /// <summary>All integer keys, sequential 1..N � emit as a plain JSON array.</summary>
            SequentialArray,
            /// <summary>All integer keys but with gaps, all values are objects � emit as a JSON array of objects with "_index" injected.</summary>
            IndexedObjectArray,
            /// <summary>Anything else � emit as a JSON object.</summary>
            Object
        }

        private TableKind Classify()
        {
            // Any string key ? plain object
            foreach (var e in _entries)
                if (e.Key is string) return TableKind.Object;

            // All-integer keys: check if sequential 1..N
            long expected = 1;
            bool sequential = true;
            foreach (var e in _entries)
            {
                long k = e.Key switch { null => expected, long l => l, _ => -1 };
                if (k != expected) { sequential = false; break; }
                expected++;
            }

            if (sequential)
                return TableKind.SequentialArray;

            // Non-sequential integer keys: only use IndexedObjectArray if every
            // value is a table that is itself NOT a sequential array (i.e. it's
            // an object-like table we can inject _index into).
            bool allObjects = _entries.TrueForAll(
                e => e.Value is LuaTable t && !t.IsSequentialIntegerKeyed()
            );
            return allObjects ? TableKind.IndexedObjectArray : TableKind.Object;
        }

        /// <summary>
        /// True when this table qualifies as a SequentialArray. Used by
        /// parent tables to decide whether injecting _index makes sense.
        /// </summary>
        internal bool IsSequentialIntegerKeyed()
        {
            long expected = 1;
            foreach (var e in _entries)
            {
                if (e.Key is string) return false;
                long k = e.Key switch { null => expected, long l => l, _ => -1 };
                if (k != expected) return false;
                expected++;
            }
            return true;
        }

        /// <summary>
        /// Write this Lua table to a <see cref="Utf8JsonWriter"/> as JSON.
        /// </summary>
        /// <param name="w"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public override void WriteTo(Utf8JsonWriter w)
        {
            if (_entries.Count == 0)
            {
                w.WriteNullValue();
                return;
            }

            switch (Classify())
            {
                case TableKind.SequentialArray:
                w.WriteStartArray();
                foreach (var e in _entries)
                {
                    e.Value.WriteTo(w);
                }
                w.WriteEndArray();
                break;

                case TableKind.IndexedObjectArray:
                // Array of objects, each with _index holding the original key
                w.WriteStartArray();
                long pos = 1;
                foreach (var e in _entries)
                {
                    long idx = e.Key switch { null => pos, long l => l, _ => pos };
                    if (e.Key is null) 
                        pos++;

                    w.WriteStartObject();
                    w.WriteNumber("_index", idx);
                    
                    
                    
                    // Inline the child table's own properties
                    var child = (LuaTable)e.Value;
                    long childPos = 1;
                    foreach (var ce in child._entries)
                    {
                        string key = ce.Key switch
                        {
                            null => (childPos++).ToString(),
                            long l => l.ToString(),
                            string s => s,
                            _ => throw new InvalidOperationException("Unexpected key type")
                        };
                        w.WritePropertyName(key);
                        ce.Value.WriteTo(w);
                    }
                    w.WriteEndObject();
                }
                w.WriteEndArray();
                break;

                default: // TableKind.Object
                w.WriteStartObject();
                long objPos = 1;
                foreach (var e in _entries)
                {
                    string key = e.Key switch
                    {
                        null => (objPos).ToString(),
                        long l => l.ToString(),
                        string s => s,
                        _ => throw new InvalidOperationException("Unexpected key type")
                    };
                    if (e.Key is null) objPos++;
                    w.WritePropertyName(key);
                    e.Value.WriteTo(w);
                }
                w.WriteEndObject();
                break;
            }
        }
    }





    // -------------------------------------------------------------------------
    //  Lexer
    // -------------------------------------------------------------------------

    internal enum TokenKind
    {
        LeftBrace, RightBrace,
        LeftBracket, RightBracket,
        Equals, Comma,
        String,
        Integer, Float,
        Identifier,
        EOF
    }

    internal readonly struct Token
    {
        public TokenKind Kind { get; init; }
        public string? StringValue { get; init; }
        public long IntValue { get; init; }
        public double FloatValue { get; init; }

        public override string ToString() =>
            Kind switch
            {
                TokenKind.String => $"String(\"{StringValue}\")",
                TokenKind.Integer => $"Integer({IntValue})",
                TokenKind.Float => $"Float({FloatValue})",
                TokenKind.Identifier => $"Id({StringValue})",
                _ => Kind.ToString()
            };
    }

    internal sealed class Lexer
    {
        private readonly TextReader _reader;

        // Single-character look-ahead buffer
        private int _peeked = int.MinValue; // int.MinValue == "not set"

        public Lexer(TextReader reader) => _reader = reader;



        // low-level character helpers
        private int PeekChar()
        {
            if (_peeked == int.MinValue)
                _peeked = _reader.Read();
            return _peeked;
        }

        private int ReadChar()
        {
            if (_peeked != int.MinValue)
            {
                var c = _peeked;
                _peeked = int.MinValue;
                return c;
            }
            return _reader.Read();
        }

        private void SkipChar() => ReadChar();





        // whitespace / comment skipping
        private void SkipWhitespace()
        {
            while (true)
            {
                int c = PeekChar();
                if (c == ' ' || c == '\t' || c == '\r' || c == '\n')
                    SkipChar();
                else
                    break;
            }
        }

        /// <summary>
        /// Skip everything up to and including the closing <c>]]</c>.
        /// Handles arbitrarily nested long-bracket levels (=[...=[).
        /// </summary>
        private void SkipLongString(int level)
        {
            // We've already procd --[=*[; now proc until ]=*]
            while (true)
            {
                int c = ReadChar();
                if (c == -1)
                    throw new LuaParseException("Unterminated long string/comment");
                if (c != ']') continue;

                // Count '=' signs
                int eq = 0;
                while (PeekChar() == '=') { SkipChar(); eq++; }

                if (PeekChar() == ']' && eq == level)
                {
                    SkipChar(); // closing ]
                    return;
                }
            }
        }


        /// <summary>Read a long string value into a StringBuilder (for string tokens).</summary>
        private string ReadLongStringContent(int level)
        {
            var sb = new StringBuilder();
            // Skip the first newline after the opening bracket if present
            if (PeekChar() == '\r') SkipChar();
            if (PeekChar() == '\n') SkipChar();

            while (true)
            {
                int c = ReadChar();
                if (c == -1)
                    throw new LuaParseException("Unterminated long string");
                if (c == ']')
                {
                    int eq = 0;
                    while (PeekChar() == '=') { SkipChar(); eq++; sb.Append('='); }

                    if (PeekChar() == ']' && eq == level)
                    {
                        SkipChar(); // closing ]
                        sb.Remove(sb.Length - eq, eq);  // Remove trailing '='
                        return sb.ToString();
                    }
                    sb.Insert(sb.Length - eq, ']'); // the opening ] not yet added yet
                }
                else
                {
                    sb.Append((char)c);
                }
            }
        }




        /// <summary>
        /// Read the next token from the input stream.  Skips whitespace and comments.
        /// </summary>
        /// <returns>Token</returns>
        public Token Next()
        {
            while (true) // loop to retry after comments
            {
                SkipWhitespace();
                int ch = PeekChar();

                if (ch == -1)
                    return new Token { Kind = TokenKind.EOF };

                // single-char tokens
                switch (ch)
                {
                    case '{': SkipChar(); return new Token { Kind = TokenKind.LeftBrace };
                    case '}': SkipChar(); return new Token { Kind = TokenKind.RightBrace };
                    case ']': SkipChar(); return new Token { Kind = TokenKind.RightBracket };
                    case '=': SkipChar(); return new Token { Kind = TokenKind.Equals };
                    case ',': SkipChar(); return new Token { Kind = TokenKind.Comma };
                }

                // '[' � key bracket or long string
                if (ch == '[')
                {
                    SkipChar();
                    if (PeekChar() == '[')
                    {
                        SkipChar();
                        string s = ReadLongStringContent(0);
                        return new Token { Kind = TokenKind.String, StringValue = s };
                    }
                    if (PeekChar() == '=')
                    {
                        // Long bracket like [==[...]==]
                        int level = 0;
                        while (PeekChar() == '=') { SkipChar(); level++; }
                        if (PeekChar() == '[')
                        {
                            SkipChar();
                            string s = ReadLongStringContent(level);
                            return new Token { Kind = TokenKind.String, StringValue = s };
                        }
                        // Not a long string � put back somehow
                    }
                    return new Token { Kind = TokenKind.LeftBracket };
                }

                // comment or negative number
                if (ch == '-')
                {
                    SkipChar();
                    int next = PeekChar();

                    if (next == '-')
                    {
                        // Comment
                        SkipChar(); // second '-'

                        // Long comment? --[=*[
                        int level = -1;
                        if (PeekChar() == '[')
                        {
                            SkipChar();
                            int eq = 0;
                            while (PeekChar() == '=') { SkipChar(); eq++; }
                            if (PeekChar() == '[')
                            {
                                SkipChar();
                                level = eq;
                            }
                        }

                        if (level >= 0)
                            SkipLongString(level);
                        else
                        {
                            // Short comment � skip to end of line
                            while (PeekChar() != '\n' && PeekChar() != -1)
                                SkipChar();
                        }
                        continue; // retry
                    }

                    // Negative number
                    if (next >= '0' && next <= '9' || next == '.')
                        return ReadNumber(negative: true);

                    // Unexpected lone '-'; skip and retry
                    continue;
                }

                // strings
                if (ch == '"' || ch == '\'')
                {
                    SkipChar();
                    return ReadQuotedString((char)ch);
                }

                // numbers 
                if (ch >= '0' && ch <= '9' || ch == '.')
                    return ReadNumber(negative: false);

                // identifiers: true / false / nil / bare keys
                if (char.IsLetter((char)ch) || ch == '_')
                    return ReadIdentifier();

                // Unknown character � skip and retry (e.g. semicolon)
                SkipChar();
            }
        }




        // quoted string token
        private Token ReadQuotedString(char delimiter)
        {
            var sb = new StringBuilder();
            while (true)
            {
                int c = ReadChar();
                if (c == -1 || c == '\n')
                    throw new LuaParseException("Unterminated string literal");

                if (c == delimiter)
                    return new Token { Kind = TokenKind.String, StringValue = sb.ToString() };

                if (c != '\\')
                {
                    sb.Append((char)c);
                    continue;
                }

                // Escape sequence
                int esc = ReadChar();
                switch (esc)
                {
                    case 'a': sb.Append('\a'); break;
                    case 'b': sb.Append('\b'); break;
                    case 'f': sb.Append('\f'); break;
                    case 'n': sb.Append('\n'); break;
                    case 'r': sb.Append('\r'); break;
                    case 't': sb.Append('\t'); break;
                    case 'v': sb.Append('\v'); break;
                    case '\\': sb.Append('\\'); break;
                    case '\'': sb.Append('\''); break;
                    case '"': sb.Append('"'); break;
                    case '\n': sb.Append('\n'); break;
                    case '\r':
                    sb.Append('\n');
                    if (PeekChar() == '\n') SkipChar();
                    break;

                    case 'x':
                    {
                        // Hex escape \xXX
                        char h1 = (char)ReadChar();
                        char h2 = (char)ReadChar();
                        sb.Append((char)System.Convert.ToInt32(new string([h1, h2]), 16));
                        break;
                    }

                    case 'u':
                    {
                        // Unicode escape \u{XXXX}
                        if (ReadChar() != '{')
                            throw new LuaParseException("Expected '{' after \\u");
                        var hex = new StringBuilder();
                        while (PeekChar() != '}')
                            hex.Append((char)ReadChar());
                        SkipChar(); // '}'
                        int codepoint = int.Parse(hex.ToString(), NumberStyles.HexNumber);
                        sb.Append(char.ConvertFromUtf32(codepoint));
                        break;
                    }

                    case 'z':
                    // Skip following whitespace
                    while (PeekChar() == ' ' || PeekChar() == '\t' ||
                           PeekChar() == '\r' || PeekChar() == '\n')
                        SkipChar();
                    break;

                    default:
                    if (esc >= '0' && esc <= '9')
                    {
                        // Decimal escape \ddd (up to 3 digits)
                        var digits = new StringBuilder();
                        digits.Append((char)esc);
                        if (PeekChar() >= '0' && PeekChar() <= '9') digits.Append((char)ReadChar());
                        if (PeekChar() >= '0' && PeekChar() <= '9') digits.Append((char)ReadChar());
                        sb.Append((char)int.Parse(digits.ToString()));
                    }
                    else
                    {
                        // Unknown escape � keep as-is
                        sb.Append('\\');
                        sb.Append((char)esc);
                    }
                    break;
                }
            }
        }




        // number token
        private Token ReadNumber(bool negative)
        {
            var sb = new StringBuilder();
            if (negative) sb.Append('-');

            int first = PeekChar();

            // Hex literal 0x�
            if (first == '0')
            {
                sb.Append((char)ReadChar());
                if (PeekChar() == 'x' || PeekChar() == 'X')
                {
                    sb.Append((char)ReadChar());
                    while (IsHexDigit(PeekChar()))
                        sb.Append((char)ReadChar());

                    long hexVal = System.Convert.ToInt64(sb.ToString(), 16);
                    return new Token { Kind = TokenKind.Integer, IntValue = hexVal };
                }
            }

            // Integer / float part
            bool isFloat = false;
            while (PeekChar() >= '0' && PeekChar() <= '9')
                sb.Append((char)ReadChar());

            if (PeekChar() == '.')
            {
                isFloat = true;
                sb.Append((char)ReadChar());
                while (PeekChar() >= '0' && PeekChar() <= '9')
                    sb.Append((char)ReadChar());
            }

            // Exponent
            if (PeekChar() == 'e' || PeekChar() == 'E')
            {
                isFloat = true;
                sb.Append((char)ReadChar());
                if (PeekChar() == '+' || PeekChar() == '-')
                    sb.Append((char)ReadChar());
                while (PeekChar() >= '0' && PeekChar() <= '9')
                    sb.Append((char)ReadChar());
            }

            string raw = sb.ToString();
            if (isFloat)
            {
                double d = double.Parse(raw, CultureInfo.InvariantCulture);
                return new Token { Kind = TokenKind.Float, FloatValue = d };
            }

            if (long.TryParse(raw, out long l))
                return new Token { Kind = TokenKind.Integer, IntValue = l };

            // Overflow (unsigned long)
            return new Token { Kind = TokenKind.Float, FloatValue = double.Parse(raw, CultureInfo.InvariantCulture) };
        }

        private static bool IsHexDigit(int c) =>
            (c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F');

        // identifier token

        private Token ReadIdentifier()
        {
            var sb = new StringBuilder();
            while (true)
            {
                int c = PeekChar();
                if (char.IsLetterOrDigit((char)c) || c == '_')
                {
                    sb.Append((char)c);
                    SkipChar();
                }
                else break;
            }
            return new Token { Kind = TokenKind.Identifier, StringValue = sb.ToString() };
        }
    }





    // -------------------------------------------------------------------------
    //  Parser
    // ------------------------------------------------------------------------

    internal sealed class Parser
    {
        private readonly Lexer _lexer;
        private Token _current;
        private bool _hasCurrent;

        public Parser(Lexer lexer) => _lexer = lexer;

        // token helpers 

        private Token Peek()
        {
            if (!_hasCurrent)
            {
                _current = _lexer.Next();
                _hasCurrent = true;
            }
            return _current;
        }

        private Token Consume()
        {
            var t = Peek();
            _hasCurrent = false;
            return t;
        }

        private Token Expect(TokenKind kind)
        {
            var t = Consume();
            if (t.Kind != kind)
                throw new LuaParseException($"Expected {kind} but got {t}");
            return t;
        }

        private bool TryConsume(TokenKind kind)
        {
            if (Peek().Kind == kind) { Consume(); return true; }
            return false;
        }




        /// <summary>
        /// Handles two forms:
        /// <list type="bullet">
        ///   <item><c>name = value</c> � variable assignment; returns the value</item>
        ///   <item><c>value</c>       � bare value</item>
        /// </list>
        /// </summary>
        public LuaValue ParseTopLevel()
        {
            // Peek: if identifier followed by '=' it's an assignment
            var t = Peek();
            if (t.Kind == TokenKind.Identifier)
            {
                Consume();
                if (Peek().Kind == TokenKind.Equals)
                {
                    Consume(); // '='
                    return ParseValue();
                }

                // Not an assignment; treat identifier as a value
                return IdentifierToValue(t.StringValue!);
            }
            return ParseValue();
        }

        // value

        private LuaValue ParseValue()
        {
            var t = Peek();
            return t.Kind switch
            {
                TokenKind.LeftBrace => ParseTable(),
                TokenKind.String => ParseString(),
                TokenKind.Integer => ParseInteger(),
                TokenKind.Float => ParseFloat(),
                TokenKind.Identifier => ParseIdentifierValue(),
                _ => throw new LuaParseException($"Unexpected token {t} while reading value")
            };
        }

        private LuaString ParseString()
        {
            var t = Expect(TokenKind.String);
            return new LuaString(t.StringValue!);
        }

        private LuaInteger ParseInteger()
        {
            var t = Expect(TokenKind.Integer);
            return new LuaInteger(t.IntValue);
        }

        private LuaFloat ParseFloat()
        {
            var t = Expect(TokenKind.Float);
            return new LuaFloat(t.FloatValue);
        }

        private LuaValue ParseIdentifierValue()
        {
            var t = Expect(TokenKind.Identifier);
            return IdentifierToValue(t.StringValue!);
        }

        private static LuaValue IdentifierToValue(string name) =>
            name switch
            {
                "true" => new LuaBool(true),
                "false" => new LuaBool(false),
                "nil" => LuaNil.Instance,
                _ => new LuaString(name)   // bare unquoted string
            };

        // table

        private LuaTable ParseTable()
        {
            Expect(TokenKind.LeftBrace);
            var entries = new List<TableEntry>();

            while (Peek().Kind != TokenKind.RightBrace && Peek().Kind != TokenKind.EOF)
            {
                entries.Add(ParseEntry());

                // Optional trailing comma
                TryConsume(TokenKind.Comma);
            }

            Expect(TokenKind.RightBrace);
            return new LuaTable(entries);
        }

        /// <summary>
        /// Parse one table entry which is one of:
        /// <list type="bullet">
        ///   <item><c>["key"] = value</c></item>
        ///   <item><c>[N] = value</c></item>
        ///   <item><c>name = value</c>  (unquoted identifier key)</item>
        ///   <item><c>value</c>         (positional)</item>
        /// </list>
        /// </summary>
        private TableEntry ParseEntry()
        {
            var t = Peek();

            if (t.Kind == TokenKind.LeftBracket)
            {
                Consume(); // '['
                var keyToken = Consume();
                Expect(TokenKind.RightBracket);
                Expect(TokenKind.Equals);
                LuaValue value = ParseValue();

                object? key = keyToken.Kind switch
                {
                    TokenKind.Integer => (object)keyToken.IntValue,
                    TokenKind.String => keyToken.StringValue!,
                    TokenKind.Identifier => keyToken.StringValue!,
                    _ => throw new LuaParseException($"Unexpected key token {keyToken}")
                };
                return new TableEntry(key, value);
            }


            // Unquoted identifier key: name = value
            if (t.Kind == TokenKind.Identifier)
            {
                // Could be identifier-as-key OR a bare value (true/false/nil)
                // Look ahead: if next after identifier is '=', it's a key
                Consume();
                if (Peek().Kind == TokenKind.Equals)
                {
                    Consume(); // '='
                    return new TableEntry(t.StringValue!, ParseValue());
                }

                // It's a bare value (e.g. positional true/false/nil)
                LuaValue val = IdentifierToValue(t.StringValue!);
                return new TableEntry(null, val);
            }

            // Positional value
            return new TableEntry(null, ParseValue());
        }
    }





    // -------------------------------------------------------------------------
    //  Exception
    // -------------------------------------------------------------------------

    public sealed class LuaParseException(string message) : Exception(message);

}
