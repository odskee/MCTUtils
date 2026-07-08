# MCTUtils

Core library for the MCTUtils .NET toolkit — a set of utilities for developing military and civilian flight simulation software.

```
dotnet add package MCTUtils
```

## Modules

### MCTUtils.Geo

Geographic coordinate types and angle utilities.

```csharp
using MCTUtils.Geo;

// Angle conversions
double radians = Angle.ConvertDegreesToRadians(180.0);   // ≈ 3.1416
double degrees = Angle.ConvertRadiansToDegrees(Math.PI);  // 180.0

// Geographic coordinate
var coord = new BasicCoordinate { Latitidue = 51.5, Longtitude = -0.12 };
```

### MCTUtils.Airspeed

Advanced airspeed conversion between IAS, CAS, EAS, TAS, Mach, and GS, with atmospheric corrections.

```csharp
using MCTUtils.Airspeed;

// Simple conversion at altitude
double tas = AirspeedConverter.IASToTAS(250.0, altitudeMeters: 3000);

// With weather (METAR)
var metar = MetarParser.Parse("EGLL 151220Z 18015KT 9999 ...");
double mach = AirspeedConverter.TASToMach(420.0, altitudeMeters: 8000, metar);

// Get pressure/density altitude
var (pressureAlt, densityAlt) = AirspeedConverter.GetAltitudes(3000, metar);

// Ground speed with wind
double gs = AirspeedConverter.TASToGS(450.0, headingDeg: 270,
    windSpeedKnots: 25, windDirDeg: 180);
```

### MCTUtils.Lua

Parse Lua table data (e.g. DCS mission files) into JSON.

```csharp
using MCTUtils.Lua;

string lua = "{ name = \"F-16CM\", weapons = { \"AIM-120\", \"AIM-9\" } }";
string json = LuaTableToJson.Convert(lua);
// {"name":"F-16CM","weapons":["AIM-120","AIM-9"]}
```

### MCTUtils.Utilities

General-purpose helpers.

```csharp
using MCTUtils.Utilities;

string safe = CommonUtilities.MakeSafeWindowsFileName("MISSION: (night).lua");
// "MISSION_ _night_.lua"

string nato = Phonetics.Word('a');   // "Alpha"
string nato2 = Phonetics.Word(3);    // "Charlie"
```

### MCTUtils.Exceptions

Public exception types used by MCTUtils packages.

| Exception | Description |
|-----------|-------------|
| `EventConfigurationMismatch` | Thrown when events are not configured before connecting |
| `PasswordNotAcceptedException` | Thrown when Tacview handshake password is rejected |

### MCTUtils.Extensions

Internal extension helpers.

### MCTUtils.Internal.DCS

DCS World terrain projection utilities (used internally by MCTUtils.Tacview).

## Dependencies

- **Proj4Net.Core** — DCS coordinate projection support

## NuGet Packages

MCTUtils is designed as a modular toolkit. Install only what you need:

| Package | Description |
|---------|-------------|
| `MCTUtils` | Core library (this package) |
| `MCTUtils.Tacview` | Tacview Real-Time Telemetry |
| `MCTUtils.CommunityStandards` | Community Flight Plan & Op Task Air schemas |
