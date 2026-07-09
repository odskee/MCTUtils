# MCTUtils

Milsim Coordination Toolbox - A Core library for the MCTUtils .NET toolkit — geographic utilities, airspeed conversion, Lua table parsing, DCS terrain projection, and shared types for flight simulation tooling.

```
dotnet add package MCTUtils
```

| | |
|---|---|
| **Target** | .NET 8 |
| **Version** | 0.1.0 |
| **License** | [LICENSE.txt](LICENSE.txt) |
| **Repository** | [github.com/odskee/MCTUtils](https://github.com/odskee/MCTUtils) |
| **IntelliSense** | Full XML docs for all public APIs |
| **Debugging** | SourceLink support — step into MCTUtils from your IDE |

---

## Namespaces

```
MCTUtils
├── Airspeed      Airspeed conversion (IAS, CAS, EAS, TAS, Mach, GS), METAR parsing
├── DCS           DCS World coordinate projection (Vec2/Vec3 → lat/lon)
├── Extensions    Extension methods: TruncateTo, SelectRandom, FirstCharToUpper
├── Geo           Angle/unit conversion, barycentric, distance, FuelType
├── Lua           Lua table → JSON conversion
├── Utilities     File name sanitizer, NATO phonetics, GPX navaid parser
├── Internal.Geo  BasicCoordinate, BarryPoint
├── Internal.DCS  Vec2, Vec3, TheatreTranslation
└── Exceptions    EventConfigurationMismatchException, MissingTheatreTranslationException, PasswordNotAcceptedException
```

---

## MCTUtils.Extensions

Extension methods directly on `string`, `double`, and `List<T>` — available by importing the `MCTUtils` namespace.

### `MCTExtensions` (static)

| Method | Return | Description |
|--------|--------|-------------|
| `TruncateTo(this double input, int digits)` | `double` | Truncates to N decimal places without rounding (e.g. `3.14159.TruncateTo(2)` → `3.14`) |
| `TruncateTo(this double? input, int digits)` | `double` | Nullable overload — throws `ArgumentNullException` on null |
| `FirstCharToUpper(this string input)` | `string` | Capitalises first character (`"hello"` → `"Hello"`) |
| `SelectRandom<T>(this List<T> input, int number)` | `List<T>` | Selects N random elements without replacement (partial Fisher-Yates shuffle) |

```csharp
using MCTUtils;

// TruncateTo — no rounding
double precise = 3.1415926535;
double short = precise.TruncateTo(4);   // 3.1415
double whole = precise.TruncateTo(0);   // 3.0

// FirstCharToUpper
string name = "viper".FirstCharToUpper();  // "Viper"

// SelectRandom — no replacement
var flights = new List<string> { "Viper 1", "Viper 2", "Viper 3", "Viper 4" };
List<string> selected = flights.SelectRandom(2);  // e.g. ["Viper 2", "Viper 4"]
```

---

## MCTUtils.Airspeed

### `AirspeedConverter` (static)

Advanced airspeed conversions with atmospheric corrections from METAR data.

| Method | Return | Description |
|--------|--------|-------------|
| `IASToCAS(double ias)` | `double` | IAS → CAS (1:1, IAS ≈ CAS at sea level) |
| `CASToIAS(double cas)` | `double` | CAS → IAS |
| `CASToEAS(double casKnots, AtmosphereState atm)` | `double` | CAS → EAS with atmospheric state |
| `EASToCAS(double easKnots, AtmosphereState atm)` | `double` | EAS → CAS with atmospheric state |
| `EASToTAS(double easKnots, AtmosphereState atm)` | `double` | EAS → TAS with atmospheric state |
| `TASToEAS(double tasKnots, AtmosphereState atm)` | `double` | TAS → EAS with atmospheric state |
| `TASToIAS(double tas, double altitudeMeters, MetarData? metar)` | `double` | TAS → IAS at altitude with optional METAR |
| `TASToGS(double tasKnots, double headingDeg, double? windSpeedKnots, double? windDirDeg)` | `double` | TAS → ground speed with wind vector |
| `TASToMach(double tasKnots, double altitudeMeters, MetarData? metar)` | `double` | TAS → Mach at altitude |
| `IASToTAS(double ias, double altitudeMeters, MetarData? metar)` | `double` | IAS → TAS at altitude |
| `IASToMach(double ias, double altitudeMeters)` | `double` | IAS → Mach at altitude |
| `MachToTAS(double mach, double altitudeMeters, MetarData? metar)` | `double` | Mach → TAS at altitude |
| `MachToIAS(double mach, double altitudeMeters)` | `double` | Mach → IAS at altitude |
| `MachToGS(double mach, double altitudeMeters, MetarData? metar)` | `double` | Mach → GS at altitude (no wind) |
| `MachToGS(double mach, double altitudeMeters, double trackDeg, MetarData? metar)` | `double` | Mach → GS with track and wind |
| `GSToTAS(double gsKnots, double trackDeg, double? windSpeedKnots, double? windDirDeg)` | `double` | GS → TAS with wind vector |
| `GSToTAS(double gsKnots, double trackDeg, MetarData? metar)` | `double` | GS → TAS with METAR wind |
| `GSToIAS(double gsKnots, double trackDeg, double altitudeMeters, MetarData? metar)` | `double` | GS → IAS at altitude |
| `GSToMach(double gsKnots, double trackDeg, double altitudeMeters, MetarData? metar)` | `double` | GS → Mach at altitude |
| `GetAltitudes(double altitudeMeters, MetarData? metar)` | `(double pressureAlt, double densityAlt)` | Pressure and density altitude in meters |

### `MetarData` (class)

Parsed METAR weather data.

| Property | Type | Description |
|----------|------|-------------|
| `WindDirectionDeg` | `double?` | Wind direction (0–360°) |
| `WindSpeedKnots` | `double?` | Wind speed in knots |
| `TemperatureC` | `double?` | Temperature in °C |
| `PressureHPa` | `double?` | Pressure in hPa |

### `MetarParser` (static)

| Method | Return | Description |
|--------|--------|-------------|
| `Parse(string metar)` | `MetarData` | Parses a METAR string for wind, temp, and QNH |

### `AtmosphereState` (class)

Atmospheric conditions at a given altitude.

| Property | Type | Description |
|----------|------|-------------|
| `TemperatureK` | `double` | Temperature in Kelvin |
| `PressurePa` | `double` | Pressure in Pascals |
| `Density` | `double` | Density in kg/m³ |
| `PressureAltitudeMeters` | `double` | Pressure altitude in meters |
| `DensityAltitudeMeters` | `double` | Density altitude in meters |
| `DeltaISA` | `double` | Deviation from ISA (K) |

#### Airspeed Example

```csharp
using MCTUtils.Airspeed;

// IAS → TAS at altitude
double tas = AirspeedConverter.IASToTAS(250.0, altitudeMeters: 3000);

// With METAR
var metar = MetarParser.Parse("EGLL 151220Z 18015KT 9999 ...");
double mach = AirspeedConverter.TASToMach(420.0, altitudeMeters: 8000, metar);

// Pressure/density altitude
var (pressureAlt, densityAlt) = AirspeedConverter.GetAltitudes(3000, metar);

// TAS → GS with wind
double gs = AirspeedConverter.TASToGS(450.0, headingDeg: 270,
    windSpeedKnots: 25, windDirDeg: 180);
```

---

## MCTUtils.Geo

### `AnglesAndMeasurements` (static)

Angle, distance, and unit conversion utilities.

| Method | Return | Description |
|--------|--------|-------------|
| `ConvertRadiansToDegrees(double radians)` | `double` | Radians → degrees |
| `ConvertDegreesToRadians(double degrees)` | `double` | Degrees → radians |
| `ConvertToFeet(double meters)` | `int` | Meters → feet (rounded) |
| `ConvertToMeter(int feet)` | `double` | Feet → meters |
| `ConvertToMeter(double feet)` | `double` | Feet → meters |
| `TonsToLitres(int tons, FuelType fuelType)` | `double` | Weight → volume by fuel type |
| `ConvertNmToMeter(double nauticalMile)` | `double` | Nautical miles → meters |
| `ConvertMeterToKm(double meter)` | `double` | Meters → kilometers |
| `ConvertMeterToNm(double meter)` | `double` | Meters → nautical miles |
| `GetBarycentric(Vector2 p, Vector2 a, Vector2 b, Vector2 c)` | `BarryPoint` | Barycentric coords of p in triangle abc |
| `GetSimpleDistance(double latStart, double latEnd, double lonStart, double lonEnd)` | `double` | Haversine distance (meters) |
| `GetSimpleDistance(double[] point1, double[] point2)` | `double` | Haversine distance (meters) |

### `FuelType` (enum)

| Member | Value |
|--------|-------|
| `JetFuel` | 0 |
| `Diesel` | 1 |
| `Gasoline` | 2 |
| `Methanol` | 3 |

---

## MCTUtils.Lua

### `LuaTableToJson` (static)

Converts Lua serialised tables (DCS mission/warehouse files) to JSON. Array detection: consecutive integer keys starting at 1 → JSON array; other keys → JSON object.

| Method | Return | Description |
|--------|--------|-------------|
| `Convert(Stream input, Stream output, bool indented)` | `void` | Stream → stream conversion |
| `Convert(TextReader input, Stream output, bool indented)` | `void` | TextReader → stream conversion |
| `Convert(string luaText, bool indented)` | `string` | Lua string → JSON string |

### `LuaParseException` (class)

Thrown on malformed Lua input.

| Constructor | Description |
|-------------|-------------|
| `LuaParseException(string message)` | Creates exception with message |

#### Lua Example

```csharp
using MCTUtils.Lua;

string lua = "{ name = \"F-16CM\", weapons = { \"AIM-120\", \"AIM-9\" } }";
string json = LuaTableToJson.Convert(lua);
// {"name":"F-16CM","weapons":["AIM-120","AIM-9"]}
```

---

## MCTUtils.DCS

### `DCSEnvironment` (class)

Bidirectional coordinate conversion between DCS World in-game coordinates (Vec2/Vec3) and decimal-degree latitude/longitude, using theatre-specific Transverse Mercator projection parameters. Projection is configured once via the constructor and both directions are immediately available.

| Constructor | Description |
|-------------|-------------|
| `DCSEnvironment(TheatreTranslation translation)` | Initialises with theatre projection parameters — prepares both geo→DCS and DCS→geo transforms |

| Method | Return | Description |
|--------|--------|-------------|
| `DecimalDegreesToDCSVec2(Coordinate coordinate)` | `Vec2` | Lat/lon → DCS Vec2 (takes `Proj4Net.Core.Coordinate`) |
| `DCSVec2ToDecimalDegrees(Vec2 dcsVec2)` | `BasicCoordinate` | DCS Vec2 → lat/lon |
| `DCSVec3ToDecimalDegrees(Vec3 dcsVec3)` | `BasicCoordinate` | DCS Vec3 → lat/lon |
| `DCSProjectionString(int centralMeridian, double scaleFactor, double falseEasting, double falseNorthing)` | `string` | Generates Proj4 projection string for the configured theatre |

#### DCS Example

```csharp
using MCTUtils.DCS;
using MCTUtils.Internal.DCS;
using Proj4Net.Core;

var env = new DCSEnvironment(new TheatreTranslation
{
    Central_meridian = 33,
    Scale_factor = 0.9996,
    False_easting = 500000,
    False_northing = 0
});

// DCS → lat/lon
var coord = env.DCSVec2ToDecimalDegrees(new Vec2(500000, 5000000));

// lat/lon → DCS
var dcsVec = env.DecimalDegreesToDCSVec2(new Coordinate(42.0, 33.0));
```

---

## MCTUtils.Internal.DCS

### `IVec2` (interface)

| Property | Type | Description |
|----------|------|-------------|
| `X` | `double` | X coordinate |
| `Y` | `double` | Y coordinate |

### `Vec2` (class, implements `IVec2`)

| Constructor | Description |
|-------------|-------------|
| `Vec2()` | Default (0, 0) |
| `Vec2(double x, double y)` | From coordinates |
| `Vec2(Vec3 dcsVec3)` | From Vec3 (drops Z) |

| Property | Type | Description |
|----------|------|-------------|
| `X` | `double` | X coordinate |
| `Y` | `double` | Y coordinate |

### `IVec3` (interface, extends `IVec2`)

| Property | Type | Description |
|----------|------|-------------|
| `Z` | `double` | Z coordinate |

### `Vec3` (class, extends `Vec2`, implements `IVec3`)

| Constructor | Description |
|-------------|-------------|
| `Vec3()` | Default (0, 0, 0) |
| `Vec3(double x, double y, double z)` | From coordinates |
| `Vec3(Vec2 dcsVec2)` | From Vec2 (Z = 0) |

| Property | Type | Description |
|----------|------|-------------|
| `Z` | `double` | Z coordinate |

### `TheatreTranslation` (class)

DCS theatre projection parameters.

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `Central_meridian` | `int` | 0 | Central meridian (lon₀) |
| `False_easting` | `double` | 0 | False easting (x₀) |
| `False_northing` | `double` | 0 | False northing (y₀) |
| `Scale_factor` | `double` | 0 | Scale factor (k₀) |

---

## MCTUtils.Internal.Geo

### `IBasicCoordinate` (interface)

| Property | Type | Description |
|----------|------|-------------|
| `Latitidue` | `double` | Latitude |
| `Longtitude` | `double` | Longitude |

### `BasicCoordinate` (class, implements `IBasicCoordinate`)

| Property | Type | Description |
|----------|------|-------------|
| `Latitidue` | `double` | Latitude |
| `Longtitude` | `double` | Longitude |

### `BarryPoint` (class)

Barycentric coordinates within a triangle.

| Property | Type | Description |
|----------|------|-------------|
| `wA` | `float` | Weight for vertex A |
| `wB` | `float` | Weight for vertex B |
| `wC` | `float` | Weight for vertex C |

---

## MCTUtils.Utilities

### `CommonUtilities` (static)

| Method | Return | Description |
|--------|--------|-------------|
| `MakeSafeWindowsFileName(string value, string replaceWith)` | `string` | Replaces `<>:"/\|?*` with replacement char |
| `TruncateDouble(double? value, int digits)` | `double` | Truncates to N decimal places (no rounding) |

### `Phonetics` (static)

NATO phonetic alphabet lookups.

| Method | Return | Description |
|--------|--------|-------------|
| `Word(char ch)` | `string` | NATO word for letter (e.g. `'a'` → `"Alpha"`) |
| `Word(int index)` | `string` | NATO word by alphabet position (1 = Alpha, 2 = Bravo, …) |

### GpxNavaidParser (static)

Parses GPX navaid files from [navaid.com](http://navaid.com).

| Method | Return | Description |
|--------|--------|-------------|
| `Parse(Stream fileStream)` | `List<GPXNavaid>` | Parses GPX from stream |
| `Parse(string filePath)` | `List<GPXNavaid>` | Parses GPX from file path |

### `GPXNavaid` (class)

| Field | Type | Description |
|-------|------|-------------|
| `TypeId` | `GPXNavaidType` | Navaid type |
| `Ident` | `string` | Identifier |
| `Name` | `string` | Full name |
| `Comment` | `string` | Comment |
| `Latitude` | `double` | Latitude |
| `Longitude` | `double` | Longitude |
| `WorldX` | `float` | Mercator-projected X |
| `WorldY` | `float` | Mercator-projected Y |
| `ElevationFt` | `float?` | Elevation in feet |
| `BearingMagVar` | `float?` | GPX magvar |
| `MagneticVariation` | `float?` | Navaid extension magvar |
| `Country` | `string` | Country |
| `State` | `string` | State/region |
| `Frequencies` | `GPXNavaidFrequency[]?` | Frequencies |
| `Runways` | `GPXRunway[]?` | Runways |
| `Fixes` | `GPXFixDefinition[]?` | Fix definitions |

### `GPXNavaidType` (enum)

```
Unknown, Airport, Heliport, Platform, Waypoint, RepPt, MilRepPt,
MilWaypoint, RNAVWaypoint, NRSWaypoint, VFRWaypoint, CNF,
VOR, VORDME, VORTAC, TACAN, DME, NDB, NDBDME, VOT, Radar
```

### `GPXNavaidFrequency` (class)

| Field | Type | Description |
|-------|------|-------------|
| `Type` | `string` | Frequency type |
| `Name` | `string` | Frequency name |
| `RawFrequency` | `string` | Raw frequency string |
| `FrequencyHz` | `double?` | Normalised frequency in Hz |

### `GPXRunway` (class)

| Field | Type | Description |
|-------|------|-------------|
| `Designation` | `string` | Runway designation |
| `LengthFt` | `ushort` | Length in feet |
| `WidthFt` | `ushort` | Width in feet |
| `Surface` | `string` | Surface type |
| `Beginning` | `GPXRunwayPosition` | Beginning position |
| `Ending` | `GPXRunwayPosition` | Ending position |

### `GPXRunwayPosition` (class)

| Field | Type | Description |
|-------|------|-------------|
| `Latitude` | `double` | Latitude |
| `Longitude` | `double` | Longitude |
| `Heading` | `float` | Heading |

### `GPXFixDefinition` (class)

| Field | Type | Description |
|-------|------|-------------|
| `Navaid` | `string` | Navaid identifier |
| `TypeId` | `GPXNavaidType` | Navaid type |
| `Radial` | `float` | Radial |
| `DistanceNm` | `float` | Distance in nautical miles |

---

## MCTUtils.Exceptions

| Exception | Inherits | Description |
|-----------|----------|-------------|
| `EventConfigurationMismatchException` | `Exception` | Events not configured before connecting |
| `MissingTheatreTranslationException` | `Exception` | Theatre translation not set |
| `PasswordNotAcceptedException` | `Exception` | Tacview handshake rejected |

---

## Dependencies

| Package | Version | Used by |
|---------|---------|---------|
| Proj4Net.Core | 1.25.1501 | DCS coordinate projection |
| Microsoft.SourceLink.GitHub | 8.x | Source-level debugging (PrivateAssets) |

---

## NuGet Packages

| Package | Description |
|---------|-------------|
| `MCTUtils` | Core library — geo, airspeed, Lua, DCS terrain, utilities, GPX parsing |
| `MCTUtils.Tacview` | Tacview Real-Time Telemetry client and protocol helpers |
| `MCTUtils.CommunityStandards` | Community Flight Plan & Op Task Air schemas |
