# MCTUtils.DCS

[![NuGet](https://img.shields.io/nuget/v/MCTUtils.DCS.svg)](https://www.nuget.org/packages/MCTUtils.DCS/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/MCTUtils.DCS.svg)](https://www.nuget.org/packages/MCTUtils.DCS/)
[![License](https://img.shields.io/github/license/Odskee/MCTUtils)](LICENSE)

<br />

DCS World terrain projection and coordinate types for MCTUtils. Provides bidirectional coordinate conversion between DCS in-game coordinates (Vec2/Vec3) and decimal-degree latitude/longitude, using theatre-specific Transverse Mercator projection parameters.

```
dotnet add package MCTUtils.DCS
```

| | |
|---|---|
| **Target** | .NET 8 |
| **Version** | 1.0.0 |
| **License** | [LICENSE](LICENSE) |
| **Repository** | [github.com/odskee/MCTUtils](https://github.com/odskee/MCTUtils) |
| **IntelliSense** | Full XML docs for all public APIs |
| **Debugging** | SourceLink support — step into MCTUtils.DCS from your IDE |

---

## Namespaces

```
MCTUtils.DCS
├── DCSEnvironment          Theatre-specific coordinate conversion
└── Internal.DCS            Vec2, Vec3, TheatreTranslation
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

#### Example

```csharp
using MCTUtils.DCS;
using MCTUtils.Internal.DCS;
using Proj4Net.Core;

var env = new DCSEnvironment(new TheatreTranslation
{
    Central_meridian = 147,
    False_northing = -1491840.000000048,
    False_easting = 238417.99999989968,
    Scale_factor = 0.9996
});

// DCS → lat/lon
var coord = env.DCSVec2ToDecimalDegrees(new Vec2(13056.832576364, 10030.962119321));
// BasicCoordinate(){ Latitude=13.576672104045052, Longitude=144.91731189173802 }

// lat/lon → DCS
var dcsVec = env.DecimalDegreesToDCSVec2(new Coordinate(13.576672104045052, 144.91731189173802));
// Vec2(){ X=13056.832576364, Y=10030.962119321 }
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

## Dependencies

| Package | Version | Used by |
|---------|---------|---------|
| Proj4Net.Core | 1.25.1501 | DCS coordinate projection |
| MCTUtils | — | `BasicCoordinate`, `BarryPoint` shared types |
| Microsoft.SourceLink.GitHub | 8.x | Source-level debugging (PrivateAssets) |
