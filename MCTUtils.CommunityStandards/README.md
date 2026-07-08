# MCTUtils.CommunityStandards

Models, serialization, and validation for the MCT Community Standards schemas — Community Flight Plans and Operational Air Tasks used by the DCS milsim community.

```
dotnet add package MCTUtils.CommunityStandards
```

Requires the `MCTUtils` core package (added automatically as a dependency).

## Namespace Structure

```
MCTUtils.CommunityStandards
├── Common                  Enums + shared model classes
├── CommunityFlightPlan     CommunityFlightPlan document
├── OpTaskAir               OpTaskAir document + weather, airfield, track models
├── Serialization           JSON serializer options, schema versions
├── Validation              JSON Schema validation
└── Utilities               Extension methods (DeepClone, Copy, IsValid)
```

## Community Flight Plan

Create, serialize, and validate a Community Flight Plan document.

```csharp
using MCTUtils.CommunityStandards.Common;
using FlightPlan = MCTUtils.CommunityStandards.CommunityFlightPlan.CommunityFlightPlan;
using MCTUtils.CommunityStandards.Serialization;

// Create a flight plan
var plan = new FlightPlan
{
    SchemaVersion = "1.1.0",
    Id = Guid.NewGuid(),
    CreatedAt = DateTimeOffset.UtcNow,
    Coalition = Coalition.BLUE
};

plan.Package.Add(new Package { Id = Guid.NewGuid(), Name = "PACKAGE VIPER" });
plan.Assets.Add(new Asset
{
    Id = Guid.NewGuid(),
    Callsign = "VIPER 1",
    Airframe = "F-16CM",
    MissionType = MissionType.Strike
});

// Serialize to JSON
string json = plan.ToJson(writeIndented: true);

// Deserialize from JSON
var loaded = FlightPlan.FromJson(json);

// Load from file
var fromFile = FlightPlan.Load("flightplan.json");

// Save to file
plan.Save("flightplan.json", writeIndented: true);

// Deep clone
var clone = plan.DeepClone();
```

## Operational Air Task

```csharp
using MCTUtils.CommunityStandards.Common;
using OpTaskAirDoc = MCTUtils.CommunityStandards.OpTaskAir.OpTaskAir;
using MCTUtils.CommunityStandards.OpTaskAir;
using MCTUtils.CommunityStandards.Serialization;

var opTaskAir = new OpTaskAirDoc
{
    SchemaVersion = "1.1.0",
    Id = Guid.NewGuid(),
    CreatedAt = DateTimeOffset.UtcNow,
    Coalition = Coalition.RED,
    MissionContext = new MissionContext
    {
        Theatre = "Caucasus",
        Date = new DateOnly(2025, 3, 15),
        TimeSeconds = 25200  // 07:00 local
    }
};

opTaskAir.Package.Add(new Package { Id = Guid.NewGuid(), Name = "PACKAGE SWIFT" });
opTaskAir.Assets.Add(new Asset
{
    Id = Guid.NewGuid(),
    Callsign = "VIPER 1",
    Airframe = "Su-27",
    MissionType = MissionType.CAP
});

// Weather
opTaskAir.MissionContext.Weather = new Weather
{
    Winds = new List<WindLayer>
    {
        new() { AltitudeFeet = 0, SpeedKnots = 12, DirectionDeg = 180 }
    },
    Clouds = new List<CloudLayer>
    {
        new() { BaseFeet = 3000, Coverage = "BKN" }
    }
};

string json = opTaskAir.ToJson();
var loaded = OpTaskAirDoc.FromJson(json);
```

## Enums

All enums use `[EnumMember(Value = "...")]` attributes and are serialized via `EnumMemberJsonConverterFactory`, which honors those attributes. C# names need not match JSON values.

```csharp
// Coalition values: BLUE, RED, NEUTRAL, OTHER
// MissionType values: Strike, CAP, SEAD, CAS, DEAD, etc.
// WaypointType values: DEPARTURE, ARRIVAL, IP, TGT, BULLSEYE, etc.
// FlightRules values: I, V, Y, Z
// EmergencyCode values: _7500, _7600, _7700 → JSON: "7500", "7600", "7700"
```

### Enum Round-Trip Example

```csharp
var json = JsonSerializer.Serialize(Coalition.BLUE, JsonSerializerOptionsFactory.CreateDefault());
// "BLUE"

var parsed = JsonSerializer.Deserialize<Coalition>("\"BLUE\"", JsonSerializerOptionsFactory.CreateDefault());
// Coalition.BLUE
```

## Serialization

Use `JsonSerializerOptionsFactory.CreateDefault()` for consistent options across the library.

```csharp
using MCTUtils.CommunityStandards.Serialization;

var options = JsonSerializerOptionsFactory.CreateDefault();
// - EnumMember-aware enum serialization
// - Property names preserved (no camelCase)
// - Null properties included
// - Populates existing collections on deserialization
```

## Validation

Validate documents against the published JSON schemas.

```csharp
using MCTUtils.CommunityStandards.Validation;

var validator = new JsonSchemaValidator();

// Validate against remote schema
ValidationResult result = await validator.ValidateAsync(json, 
    new Uri("https://mctoolbox.uk/schema/v1.1.0/community-flightplan.schema.json"));

if (!result.IsValid)
{
    foreach (var error in result.Errors)
    {
        Console.WriteLine($"{error.Path}: {error.Message}");
    }
}

// Validate against inline schema string
result = validator.Validate(json, schemaText);
```

### Extension Method Validation

```csharp
ValidationResult result = await plan.IsValid();

if (result.IsValid)
    Console.WriteLine("Flight plan is valid!");
```

## Schema Versions

```csharp
using MCTUtils.CommunityStandards.Serialization;

string version = SchemaVersions.CurrentVersion;  // "1.1.0"
string fpSchema = SchemaVersions.CommunityFlightPlanSchemaUrl;
string otaSchema = SchemaVersions.OpTaskAirSchemaUrl;
```

## Dependencies

- **MCTUtils** (core)
- **JsonSchema.Net** — JSON Schema evaluation for document validation
