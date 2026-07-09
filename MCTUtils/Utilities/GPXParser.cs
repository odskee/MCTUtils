using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MCTUtils.Utilities
{

    #region Core Models

    /// <summary>
    /// Enumerates the types of navaids that can be represented in a GPX file.
    /// </summary>
    public enum GPXNavaidType : ushort
    {
        Unknown = 0,

        Airport,
        Heliport,
        Platform,

        Waypoint,
        RepPt,
        MilRepPt,
        MilWaypoint,
        RNAVWaypoint,
        NRSWaypoint,
        VFRWaypoint,
        CNF,

        VOR,
        VORDME,
        VORTAC,
        TACAN,
        DME,
        NDB,
        NDBDME,
        VOT,
        Radar
    }

    /// <summary>
    /// Represents a navaid waypoint parsed from a GPX file, including its core identity, coordinates, magnetic information, region, and optional aviation data.
    /// </summary>
    public sealed class GPXNavaid
    {
        // ------------------------------------------------------------------
        // Core Identity
        // ------------------------------------------------------------------

        public GPXNavaidType TypeId;

        public string Ident = string.Empty;

        public string Name = string.Empty;

        public string Comment = string.Empty;

        // ------------------------------------------------------------------
        // Coordinates
        // ------------------------------------------------------------------

        public double Latitude;

        public double Longitude;

        public float WorldX;

        public float WorldY;

        public float? ElevationFt;

        // ------------------------------------------------------------------
        // Magnetic
        // ------------------------------------------------------------------

        /// <summary>
        /// GPX magvar (0-360)
        /// </summary>
        public float? BearingMagVar;

        /// <summary>
        /// Navaid extension magvar (-/+)
        /// </summary>
        public float? MagneticVariation;

        // ------------------------------------------------------------------
        // Region
        // ------------------------------------------------------------------

        public string Country = string.Empty;

        public string State = string.Empty;

        // ------------------------------------------------------------------
        // Optional Aviation Data
        // ------------------------------------------------------------------

        public GPXNavaidFrequency[]? Frequencies;

        public GPXRunway[]? Runways;

        public GPXFixDefinition[]? Fixes;
    }

    /// <summary>
    /// Represents a frequency associated with a navaid, including its type, name, raw frequency string, and normalized frequency in Hertz.
    /// </summary>
    public sealed class GPXNavaidFrequency
    {
        public string Type = string.Empty;

        public string Name = string.Empty;

        public string RawFrequency = string.Empty;

        public double? FrequencyHz;
    }

    /// <summary>
    /// Represents a runway associated with a navaid, including its designation, dimensions, surface type, and positions for the beginning and ending of the runway.
    /// </summary>
    public sealed class GPXRunway
    {
        public string Designation = string.Empty;

        public ushort LengthFt;

        public ushort WidthFt;

        public string Surface = string.Empty;

        public GPXRunwayPosition Beginning = new();

        public GPXRunwayPosition Ending = new();
    }


    /// <summary>
    /// Represents a position associated with a runway, including its latitude, longitude, and heading.
    /// </summary>
    public sealed class GPXRunwayPosition
    {
        public double Latitude;

        public double Longitude;

        public float Heading;
    }

    /// <summary>
    /// Represents a fix definition associated with a navaid, including the navaid identifier, type, radial, and distance in nautical miles.
    /// </summary>
    public sealed class GPXFixDefinition
    {
        public string Navaid = string.Empty;

        public GPXNavaidType TypeId;

        public float Radial;

        public float DistanceNm;
    }

    #endregion

    #region Parser

    /// <summary>
    /// Provides functionality to parse GPX files containing navaid waypoints, extracting relevant information into structured objects.
    /// </summary>
    public static class GpxNavaidParser
    {
        private const string GPX_NS =
            "http://www.topografix.com/GPX/1/1";

        private const string NAVAID_NS =
            "http://navaid.com/GPX/NAVAID/1/0";

        /// <summary>
        /// Parses a GPX file stream and extracts navaid waypoints into a list of GPXNavaid objects.
        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns>List of GPXNavaid objects</returns>
        public static List<GPXNavaid> Parse(Stream fileStream)
        {
            List<GPXNavaid> results = new(140000);
            XmlReaderSettings settings = new()
            {
                IgnoreComments = true,
                IgnoreWhitespace = true,
                IgnoreProcessingInstructions = true
            };

            using XmlReader reader = XmlReader.Create(fileStream, settings);

            while (reader.Read())
            {
                if (reader.NodeType != XmlNodeType.Element)
                    continue;

                if (reader.LocalName != "wpt")
                    continue;

                GPXNavaid navaid = ParseWaypoint(reader);

                results.Add(navaid);
            }

            return results;
        }

        /// <summary>
        /// Parses a GPX file from the specified file path and extracts navaid waypoints into a list of GPXNavaid objects.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>List of GPXNavaid objects</returns>
        public static List<GPXNavaid> Parse(string filePath)
        {
            List<GPXNavaid> results = new(140000);
            using FileStream fs = File.OpenRead(filePath);
            return Parse(fs);
        }

        private static GPXNavaid ParseWaypoint(XmlReader reader)
        {
            GPXNavaid navaid = new();

            // ------------------------------------------------------------------
            // Attributes
            // ------------------------------------------------------------------

            navaid.Latitude =
                ParseDouble(reader.GetAttribute("lat"));

            navaid.Longitude =
                ParseDouble(reader.GetAttribute("lon"));

            ProjectMercator(
                navaid.Latitude,
                navaid.Longitude,
                out navaid.WorldX,
                out navaid.WorldY);

            // ------------------------------------------------------------------
            // Read subtree
            // ------------------------------------------------------------------

            using XmlReader subtree = reader.ReadSubtree();

            while (subtree.Read())
            {
                if (subtree.NodeType != XmlNodeType.Element)
                    continue;

                switch (subtree.LocalName)
                {
                    case "ele":
                    navaid.ElevationFt =
                        ParseFloat(subtree.ReadElementContentAsString());
                    break;

                    case "magvar":
                    navaid.BearingMagVar =
                        ParseFloat(subtree.ReadElementContentAsString());
                    break;

                    case "name":
                    navaid.Ident =
                        subtree.ReadElementContentAsString();
                    break;

                    case "cmt":
                    navaid.Comment =
                        subtree.ReadElementContentAsString();
                    break;

                    case "type":
                    navaid.TypeId =
                        ParseType(subtree.ReadElementContentAsString());
                    break;

                    case "extensions":
                    ParseExtensions(subtree, navaid);
                    break;
                }
            }

            return navaid;
        }

        private static void ParseExtensions(
            XmlReader reader,
            GPXNavaid navaid)
        {
            using XmlReader subtree = reader.ReadSubtree();

            List<GPXNavaidFrequency>? freqs = null;
            List<GPXRunway>? runways = null;
            List<GPXFixDefinition>? fixes = null;

            while (subtree.Read())
            {
                if (subtree.NodeType != XmlNodeType.Element)
                    continue;

                switch (subtree.LocalName)
                {
                    case "name":
                    if (subtree.NamespaceURI == NAVAID_NS)
                    {
                        navaid.Name =
                            subtree.ReadElementContentAsString();
                    }
                    break;

                    case "country":
                    navaid.Country =
                        subtree.ReadElementContentAsString();
                    break;

                    case "state":
                    navaid.State =
                        subtree.ReadElementContentAsString();
                    break;

                    case "magvar":
                    if (subtree.NamespaceURI == NAVAID_NS)
                    {
                        navaid.MagneticVariation =
                            ParseFloat(
                                subtree.ReadElementContentAsString());
                    }
                    break;

                    case "frequency":
                    freqs ??= new List<GPXNavaidFrequency>(2);

                    freqs.Add(ParseFrequency(subtree));
                    break;

                    case "runway":
                    runways ??= new List<GPXRunway>(4);

                    runways.Add(ParseRunway(subtree));
                    break;

                    case "fix":
                    fixes ??= new List<GPXFixDefinition>(2);

                    fixes.Add(ParseFix(subtree));
                    break;
                }
            }

            navaid.Frequencies = freqs?.ToArray();
            navaid.Runways = runways?.ToArray();
            navaid.Fixes = fixes?.ToArray();
        }

        private static GPXNavaidFrequency ParseFrequency(XmlReader reader)
        {
            GPXNavaidFrequency freq = new();

            freq.Type =
                reader.GetAttribute("type") ?? string.Empty;

            freq.Name =
                reader.GetAttribute("name") ?? string.Empty;

            freq.RawFrequency =
                reader.GetAttribute("frequency") ?? string.Empty;

            freq.FrequencyHz =
                NormalizeFrequency(freq.RawFrequency);

            return freq;
        }

        private static GPXRunway ParseRunway(XmlReader reader)
        {
            GPXRunway runway = new();

            runway.Designation =
                reader.GetAttribute("designation") ?? string.Empty;

            runway.LengthFt =
                ParseUShort(reader.GetAttribute("length"));

            runway.WidthFt =
                ParseUShort(reader.GetAttribute("width"));

            runway.Surface =
                reader.GetAttribute("surface") ?? string.Empty;

            using XmlReader subtree = reader.ReadSubtree();

            while (subtree.Read())
            {
                if (subtree.NodeType != XmlNodeType.Element)
                    continue;

                if (subtree.LocalName == "beginning")
                {
                    runway.Beginning =
                        ParseRunwayEnd(subtree);
                }
                else if (subtree.LocalName == "ending")
                {
                    runway.Ending =
                        ParseRunwayEnd(subtree);
                }
            }

            return runway;
        }

        private static GPXRunwayPosition ParseRunwayEnd(XmlReader reader)
        {
            GPXRunwayPosition end = new();

            end.Latitude =
                ParseDouble(reader.GetAttribute("lat"));

            end.Longitude =
                ParseDouble(reader.GetAttribute("lon"));

            using XmlReader subtree = reader.ReadSubtree();

            while (subtree.Read())
            {
                if (subtree.NodeType != XmlNodeType.Element)
                    continue;

                if (subtree.LocalName == "heading")
                {
                    end.Heading =
                        ParseFloat(subtree.ReadElementContentAsString());
                }
            }

            return end;
        }

        private static GPXFixDefinition ParseFix(XmlReader reader)
        {
            return new GPXFixDefinition
            {
                Navaid =
                    reader.GetAttribute("navaid") ?? string.Empty,

                TypeId =
                    ParseType(reader.GetAttribute("type")),

                Radial =
                    ParseFloat(reader.GetAttribute("radial")),

                DistanceNm =
                    ParseFloat(reader.GetAttribute("distance"))
            };
        }

    #endregion




        #region Utilities

        private static GPXNavaidType ParseType(string? value)
        {
            return value switch
            {
                "AIRPORT" => GPXNavaidType.Airport,
                "HELIPORT" => GPXNavaidType.Heliport,
                "PLATFORM" => GPXNavaidType.Platform,

                "WAYPOINT" => GPXNavaidType.Waypoint,
                "REP-PT" => GPXNavaidType.RepPt,
                "MIL-REP-PT" => GPXNavaidType.MilRepPt,
                "MIL-WAYPOINT" => GPXNavaidType.MilWaypoint,
                "RNAV-WP" => GPXNavaidType.RNAVWaypoint,
                "NRS-WAYPOINT" => GPXNavaidType.NRSWaypoint,
                "VFR-WP" => GPXNavaidType.VFRWaypoint,
                "CNF" => GPXNavaidType.CNF,

                "VOR" => GPXNavaidType.VOR,
                "VOR/DME" => GPXNavaidType.VORDME,
                "VORTAC" => GPXNavaidType.VORTAC,
                "TACAN" => GPXNavaidType.TACAN,
                "DME" => GPXNavaidType.DME,
                "NDB" => GPXNavaidType.NDB,
                "NDB/DME" => GPXNavaidType.NDBDME,
                "VOT" => GPXNavaidType.VOT,
                "RADAR" => GPXNavaidType.Radar,

                _ => GPXNavaidType.Unknown
            };
        }

        private static double ParseDouble(string? value)
        {
            if (double.TryParse(
                value,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out double result))
            {
                return result;
            }

            return 0;
        }

        private static float ParseFloat(string? value)
        {
            if (float.TryParse(
                value,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out float result))
            {
                return result;
            }

            return 0;
        }

        private static ushort ParseUShort(string? value)
        {
            if (ushort.TryParse(value, out ushort result))
                return result;

            return 0;
        }

        /// <summary>
        /// Normalizes frequency into Hz where possible.
        /// </summary>
        private static double? NormalizeFrequency(string raw)
        {
            if (!double.TryParse(
                raw,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out double value))
            {
                return null;
            }

            // crude heuristic:
            // < 1000 => MHz
            // >= 1000 => kHz

            if (value < 1000)
                return value * 1_000_000;

            return value * 1_000;
        }

        /// <summary>
        /// Precompute mercator world coordinates.
        /// Useful for rendering/spatial indexing.
        /// </summary>
        private static void ProjectMercator(
            double lat,
            double lon,
            out float x,
            out float y)
        {
            x = (float)((lon + 180.0) / 360.0);

            double sinLat =
                Math.Sin(lat * Math.PI / 180.0);

            y = (float)(
                0.5 -
                Math.Log((1 + sinLat) / (1 - sinLat)) /
                (4 * Math.PI));
        }

        #endregion
    }

}
