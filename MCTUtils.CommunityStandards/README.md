# MCTUtils.CommunityStandards

Models, serialization, and validation for the MCT Community Standards schemas — Community Flight Plans and Operational Air Tasks used by the DCS milsim community.
* Op Task Air - https://mctoolbox.uk/schema/v1.1.0/op-task-air.schema.json
* Flight Plan - https://mctoolbox.uk/schema/v1.1.0/community-flightplan.schema.json

```
dotnet add package MCTUtils.CommunityStandards
```

| | |
|---|---|
| **Target** | .NET 8 |
| **Version** | 0.2.6 |
| **Dependency** | MCTUtils (core) — added automatically |
| **Repository** | [github.com/odskee/MCTUtils](https://github.com/odskee/MCTUtils) |
| **IntelliSense** | Full XML docs for all public APIs |
| **Debugging** | SourceLink support |

---

## Namespace Structure

```
MCTUtils.CommunityStandards
├── Common                  Enums (18) + shared model classes (16)
├── CommunityFlightPlan     CommunityFlightPlan document
├── OpTaskAir               OpTaskAir document + weather, airfield, track models (12)
├── Serialization           JsonSerializerOptionsFactory, EnumMemberJsonConverterFactory, SchemaVersions
├── Validation              ISchemaValidator, JsonSchemaValidator, ValidationResult, ValidationError
└── Utilities               FlightPlanExtensions, OpTaskAirExtensions
```

---

## Enums (18)

All enums use `[EnumMember(Value = "...")]` attributes where JSON values differ from C# names, serialized via `EnumMemberJsonConverterFactory`.

### `Coalition`

| Member |
|--------|
| `BLUE` |
| `RED` |
| `NEUTRAL` |
| `OTHER` |

### `MissionType` (uses `[EnumMember]`)

| Member | JSON Value |
|--------|-----------|
| `AAR` | `"AAR"` |
| `AI` | `"AI"` |
| `Airlift` | `"Airlift"` |
| `Airmobile` | `"AIRMOBILE"` |
| `AmbushCap` | `"AMBUSHCAP"` |
| `Asuw` | `"ASUW"` |
| `Asw` | `"ASW"` |
| `Awacs` | `"AWACS"` |
| `Bai` | `"BAI"` |
| `BarCap` | `"BARCAP"` |
| `Cap` | `"CAP"` |
| `Cas` | `"CAS"` |
| `CasOnCall` | `"CAS_ON_CALL"` |
| `Dca` | `"DCA"` |
| `Dead` | `"DEAD"` |
| `Escort` | `"ESCORT"` |
| `FacA` | `"FAC(A)"` |
| `HavCap` | `"HAVCAP"` |
| `Interdiction` | `"INTERDICTION"` |
| `Intercept` | `"INTERCEPT"` |
| `OcaStrike` | `"OCA_STRIKE"` |
| `ResCap` | `"RESCAP"` |
| `Recon` | `"RECON"` |
| `Sead` | `"SEAD"` |
| `Stealth` | `"STEALTH"` |
| `Strike` | `"STRIKE"` |
| `TarCap` | `"TARCAP"` |
| `Tasmo` | `"TASMO"` |
| `Training` | `"TRAINING"` |
| `Transport` | `"TRANSPORT"` |
| `Other` | `"OTHER"` |

### `WaypointType`

`DEPARTURE`, `ARRIVAL`, `DIVERT`, `TURNAROUND`, `FLYOVER`, `FLYBY`, `MARKPOINT`, `PUSH`, `IP`, `EGRESS`, `HOLDING`, `REFUEL`, `HAZZARD`, `BINGO`, `BULLSEYE`, `CP`, `TGT`, `FENCE_IN`, `FENCE_OUT`, `CUSTOM`

### `FlightRules`

| Member | Meaning |
|--------|---------|
| `I` | IFR |
| `V` | VFR |
| `Y` | SVFR |
| `Z` | VFR-on-top |

### `EmergencyCode` (uses `[EnumMember]`)

| Member | JSON Value |
|--------|-----------|
| `_7500` | `"7500"` |
| `_7600` | `"7600"` |
| `_7700` | `"7700"` |

### `SpeedType`

`IAS`, `TAS`, `GS`, `MACH`

### `AltitudeReference`

`MSL`, `AGL`, `FL`

### `AirfieldType`

`ICAO`, `CARRIER`, `ROAD_BASE`, `FARP`, `CUSTOM`

### `CommsRole`

`ASSET`, `ATC`, `AWACS`, `FAC`, `GCI`, `JTAC`, `OTHER`, `PACKAGE`, `TANKER`

### `Modulation`

`AM`, `FM`

### `FlightType`

`Scheduled`, `Nonscheduled`, `General`

### `FlightOversight`

`VFR`, `IFR`, `SVFR`

### `FuelType`

`JP4`, `JP5`, `JP8`, `AVGAS`

### `SurfaceType`

`HARD`, `SOFT`, `WATER`

### `TacanBand`

`X`, `Y`

### `DeckState`

`LAUNCH`, `RECOVERY`, `BOTH`

### `Mode4Status`

`CODE_A`, `CODE_B`, `CODE_C`, `HOLD`, `ZAP`

### `FlightMemberRole`

`LEAD`, `WINGMAN`, `SPARE`

### Enum Round-Trip

```csharp
using MCTUtils.CommunityStandards.Serialization;

var json = JsonSerializer.Serialize(Coalition.BLUE, JsonSerializerOptionsFactory.CreateDefault());
// "BLUE"

var parsed = JsonSerializer.Deserialize<Coalition>("\"BLUE\"", JsonSerializerOptionsFactory.CreateDefault());
// Coalition.BLUE
```

---

## Model Classes

### `CommunityFlightPlan` (`MCTUtils.CommunityStandards.CommunityFlightPlan`)

Annotated with `[JsonObjectCreationHandling(Populate)]` — getter-only collections are populated on deserialization.

| Property | Type | JSON | Description |
|----------|------|------|-------------|
| `Schema` | `string` | `schema` | Document type identifier (`"community-flightplan"`) |
| `SchemaVersion` | `string` | `schema_version` | Schema version |
| `Id` | `Guid` | `id` | UUID v7 |
| `CreatedAt` | `DateTimeOffset` | `created_at` | ISO 8601 creation timestamp |
| `UpdatedAt` | `DateTimeOffset?` | `updated_at` | ISO 8601 last modified |
| `ToolSource` | `string?` | `tool_source` | Producing tool/user-agent |
| `Coalition` | `Coalition` | `coalition` | Coalition |
| `Package` | `List<Package>` | `package` | Package definitions (getter-only) |
| `Assets` | `List<Asset>` | `assets` | Flight assets (getter-only) |
| `Routes` | `List<Route>` | `routes` | Route definitions (getter-only) |
| `Waypoints` | `List<Waypoint>?` | `waypoints` | Waypoints |
| `Extensions` | `Extensions?` | `extensions` | Tool extensions |

**Methods:**

| Method | Return | Description |
|--------|--------|-------------|
| `ToJson(bool writeIndented)` | `string` | Serialize to JSON string |
| `FromJson(string json)` | `CommunityFlightPlan` | Deserialize from JSON string |
| `Load(string path)` | `CommunityFlightPlan` | Load from file |
| `Save(string path, bool writeIndented)` | `void` | Save to file |

### `OpTaskAir` (`MCTUtils.CommunityStandards.OpTaskAir`)

Annotated with `[JsonObjectCreationHandling(Populate)]`.

| Property | Type | JSON | Description |
|----------|------|------|-------------|
| `Schema` | `string` | `schema` | Document type (`"community-op-task-air"`) |
| `SchemaVersion` | `string` | `schema_version` | Schema version |
| `Id` | `Guid` | `id` | UUID v7 |
| `CreatedAt` | `DateTimeOffset` | `created_at` | ISO 8601 creation |
| `UpdatedAt` | `DateTimeOffset?` | `updated_at` | ISO 8601 last modified |
| `ToolSource` | `string?` | `tool_source` | Producing tool |
| `Coalition` | `Coalition` | `coalition` | Coalition |
| `MissionContext` | `MissionContext` | `mission_context` | Mission environment |
| `Package` | `List<Package>` | `package` | Package definitions |
| `Assets` | `List<Asset>` | `assets` | Flight assets |
| `Routes` | `List<Route>` | `routes` | Route definitions |
| `Waypoints` | `List<Waypoint>?` | `waypoints` | Waypoints |
| `Tracks` | `List<Track>?` | `tracks` | Track definitions |
| `Airfields` | `List<AirfieldDefinition>?` | `airfields` | Custom airfields |
| `Extensions` | `Extensions?` | `extensions` | Tool extensions |

**Methods:**

| Method | Return | Description |
|--------|--------|-------------|
| `ToJson(bool writeIndented)` | `string` | Serialize to JSON |
| `FromJson(string json)` | `OpTaskAir` | Deserialize from JSON |
| `Load(string path)` | `OpTaskAir` | Load from file |
| `Save(string path, bool writeIndented)` | `void` | Save to file |

### `Package`

| Property | Type | JSON | Description |
|----------|------|------|-------------|
| `Id` | `Guid` | `id` | Unique ID |
| `Name` | `string` | `name` | Package name |
| `PackageCommander` | `string?` | `package_commander` | Commander callsign |
| `CommsPlan` | `List<CommsEntry>?` | `comms_plan` | Shared comms channels |
| `Notes` | `string?` | `notes` | Free-text notes |

### `Asset`

| Property | Type | JSON | Description |
|----------|------|------|-------------|
| `Id` | `Guid` | `id` | Unique ID |
| `PackageId` | `Guid` | `package_id` | Reference to Package |
| `Callsign` | `string` | `callsign` | Flight callsign |
| `TacticalCallsign` | `string?` | `tactical_callsign` | Tactical callsign |
| `FlightNumber` | `int` | `flight_number` | Expected flight members |
| `FlightType` | `FlightType?` | `flightType` | ICAO flight type |
| `FlightRules` | `FlightRules?` | `flightRules` | ICAO flight rules |
| `FlightOversight` | `FlightOversight?` | `flightOversight` | Flight visibility |
| `FlightMember` | `FlightMember?` | `flight_member` | Pilot info |
| `Airframe` | `string` | `airframe` | Aircraft type (e.g. `"F-16CM"`) |
| `MissionType` | `MissionType` | `mission_type` | Mission type |
| `PrimaryTarget` | `string?` | `primary_target` | Target description |
| `DmpiRefs` | `List<LatLon>?` | `dmpi_refs` | DMPI coordinates |
| `RouteId` | `Guid` | `route_id` | Reference to Route |
| `Notes` | `string?` | `notes` | Free-text notes |

### `Route`

Annotated with `[JsonObjectCreationHandling(Populate)]`.

| Property | Type | JSON | Description |
|----------|------|------|-------------|
| `Id` | `Guid` | `id` | Unique ID |
| `AssetId` | `Guid` | `asset_id` | Reference to Asset |
| `TotOffsetSeconds` | `int?` | `tot_offset_seconds` | Time-off-Target offset |
| `FuelPlan` | `FuelPlan?` | `fuel_plan` | Fuel planning data |
| `Legs` | `List<RouteLeg>` | `legs` | Leg sequence (getter-only) |

### `RouteLeg`

| Property | Type | JSON | Description |
|----------|------|------|-------------|
| `LegName` | `string` | `leg_name` | Leg identifier |
| `StartWaypoint` | `Guid` | `start_waypoint` | Starting waypoint ref |
| `EndWaypoint` | `Guid` | `end_waypoint` | Ending waypoint ref |
| `FlightRules` | `FlightRules?` | `flightRules` | Leg flight rules |
| `FlightOversight` | `FlightOversight?` | `flightOversight` | Leg visibility |

### `Waypoint`

| Property | Type | JSON | Description |
|----------|------|------|-------------|
| `Id` | `Guid` | `id` | Unique ID |
| `Type` | `WaypointType` | `type` | Waypoint type |
| `TrackId` | `Guid?` | `track_id` | Reference to Track |
| `CustomType` | `string?` | `custom_type` | Custom type label |
| `Name` | `string?` | `name` | Waypoint name |
| `Latitude` | `double` | `latitude` | WGS-84 latitude |
| `Longitude` | `double` | `longitude` | WGS-84 longitude |
| `AltitudeFt` | `double?` | `altitude_ft` | Planned altitude |
| `AltitudeRef` | `AltitudeReference?` | `altitude_ref` | Altitude reference |
| `Speed` | `double?` | `speed` | Planned speed |
| `SpeedType` | `SpeedType?` | `speed_type` | Speed type |
| `EtaSeconds` | `int?` | `eta_seconds` | ETA (seconds since midnight) |
| `FixedSeconds` | `int?` | `fixed_seconds` | Hard TOT |
| `LeaveSeconds` | `int?` | `leave_seconds` | Leave time |
| `TrackDeg` | `double?` | `track_deg` | Planned track |
| `Activity` | `string?` | `activity` | Activity description |
| `Notes` | `string?` | `notes` | Free-text notes |
| `TacanRef` | `string?` | `tacan_ref` | TACAN reference |
| `AirfieldRef` | `string?` | `airfield_ref` | Airfield reference |

### `LatLon`

| Property | Type | JSON | Description |
|----------|------|------|-------------|
| `Latitude` | `double` | `latitude` | WGS-84 latitude |
| `Longitude` | `double` | `longitude` | WGS-84 longitude |

### `CommsEntry`

| Property | Type | JSON | Description |
|----------|------|------|-------------|
| `Role` | `CommsRole?` | `role` | Functional role |
| `Label` | `string?` | `label` | Channel label |
| `FrequencyMhz` | `double` | `frequency_mhz` | Frequency (MHz) |
| `Modulation` | `Modulation` | `modulation` | Modulation type |
| `Callsign` | `string?` | `callsign` | Station callsign |

### `Transponder`

| Property | Type | JSON | Description |
|----------|------|------|-------------|
| `Mode3A` | `string` | `mode_3a` | Mode 3/A code |
| `Mode4Status` | `Mode4Status?` | `mode_4` | Mode 4 status |

### `FlightMember`

| Property | Type | JSON | Description |
|----------|------|------|-------------|
| `Role` | `FlightMemberRole?` | `role` | Flight role |
| `Name` | `string?` | `name` | Pilot name |

### `FuelPlan`

| Property | Type | JSON | Description |
|----------|------|------|-------------|
| `FuelType` | `FuelType?` | `fuel_type` | Fuel type |
| `Internal` | `double?` | `internal` | Internal fuel |
| `External` | `double?` | `external` | External fuel |

### `Codes`

| Property | Type | JSON | Description |
|----------|------|------|-------------|
| `IffMode1` | `string?` | `iff_mode1` | IFF Mode 1 |
| `IffMode2` | `string?` | `iff_mode2` | IFF Mode 2 |
| `IffMode3A` | `string?` | `iff_mode3a` | IFF Mode 3/A |
| `IffMode4` | `Mode4Status?` | `iff_mode4` | IFF Mode 4 |

### `Extensions`

| Property | Type | JSON | Description |
|----------|------|------|-------------|
| `Mctoolbox` | `MctoolboxExtensions?` | `mctoolbox` | MCToolbox extensions |
| `LotAtc` | `LotAtcExtensions?` | `lotatc` | LotATC extensions |
| `DcsDtc` | `DcsDtcExtensions?` | `dcs_dtc` | DCS-DTC extensions |
| `OtherExtensions` | `Dictionary<string, JsonElement>?` | — | Other tool extensions (JsonExtensionData) |

### `MctoolboxExtensions`

| Property | Type | JSON | Description |
|----------|------|------|-------------|
| `Version` | `string?` | `version` | Extension version |

### `LotAtcExtensions`

| Property | Type | JSON | Description |
|----------|------|------|-------------|
| `Radars` | `List<LotAtcRadar>?` | `radars` | LotATC radars |

### `LotAtcRadar`

| Property | Type | JSON | Description |
|----------|------|------|-------------|
| `FrequencyHz` | `double` | `frequency` | Radar frequency |
| `ScanVolume` | `LotAtcScanVolume` | `scan_volume` | Scan volume settings |

### `LotAtcScanVolume`

| Property | Type | JSON | Description |
|----------|------|------|-------------|
| `AzimuthStart` | `double` | `azimuth_start` | Azimuth start |
| `AzimuthEnd` | `double` | `azimuth_end` | Azimuth end |
| `ElevationStart` | `double` | `elevation_start` | Elevation start |
| `ElevationEnd` | `double` | `elevation_end` | Elevation end |
| `RangeNm` | `double` | `range_nm` | Range (NM) |

### `DcsDtcExtensions`

| Property | Type | JSON | Description |
|----------|------|------|-------------|
| `Data` | `string?` | `data` | DCS-DTC data blob |

### --- OpTaskAir Models ---

### `MissionContext`

| Property | Type | JSON | Description |
|----------|------|------|-------------|
| `Theatre` | `string` | `theatre` | Map/theatre name |
| `Date` | `DateOnly` | `date` | Mission UTC date |
| `TimeSeconds` | `int` | `time_seconds` | Seconds since midnight UTC |
| `Bullseye` | `Bullseye?` | `bullseye` | Bullseye references |
| `Weather` | `Weather?` | `weather` | Weather conditions |

### `Bullseye`

| Property | Type | JSON | Description |
|----------|------|------|-------------|
| `Blue` | `LatLon?` | `blue` | Blue coalition bullseye |
| `Red` | `LatLon?` | `red` | Red coalition bullseye |

### `Weather`

| Property | Type | JSON | Description |
|----------|------|------|-------------|
| `TemperatureC` | `double?` | `temperature_c` | Surface temp (°C) |
| `QnhHpa` | `double?` | `qnh_hpa` | QNH (hPa) |
| `VisibilityM` | `int?` | `visibility_m` | Visibility (m) |
| `Winds` | `Winds?` | `winds` | Wind layers |
| `Clouds` | `CloudLayer?` | `clouds` | Cloud layers |

### `Winds`

| Property | Type | JSON | Description |
|----------|------|------|-------------|
| `Surface` | `WindLayer?` | `surface` | Surface wind |
| `Altitude` | `List<WindLayer>?` | `altitude` | Upper wind layers |

### `WindLayer`

| Property | Type | JSON | Description |
|----------|------|------|-------------|
| `FromDeg` | `double` | `from_deg` | Wind direction (FROM) |
| `SpeedKts` | `double` | `speed_kts` | Wind speed (knots) |
| `AltitudeFt` | `int?` | `altitude_ft` | Altitude MSL (feet) |

### `CloudLayer`

| Property | Type | JSON | Description |
|----------|------|------|-------------|
| `Density` | `int` | `density` | Cloud density (0–9) |
| `BaseFt` | `int?` | `base_ft` | Cloud base (feet MSL) |
| `ThicknessFt` | `int?` | `thickness_ft` | Cloud thickness (feet) |

### `Track`

| Property | Type | JSON | Description |
|----------|------|------|-------------|
| `Id` | `Guid` | `id` | Unique ID |
| `FixName` | `string?` | `fix_name` | Initial fix name |
| `InitialBearing` | `int` | `initial_bearing` | Entry bearing (true) |
| `InitialAltitude` | `int` | `initial_altitude` | Entry altitude (feet) |
| `AltitudeSeparation` | `int?` | `altitude_seperation` | Stack separation (feet) |
| `TrackLength` | `int` | `track_length` | Track length (NM) |
| `TrackWidth` | `int` | `track_width` | Track width (NM) |
| `Points` | `List<LatLon>?` | `points` | Track shape points |

### `AirfieldDefinition`

| Property | Type | JSON | Description |
|----------|------|------|-------------|
| `Id` | `Guid` | `id` | Unique ID |
| `Type` | `AirfieldType` | `type` | Airfield classification |
| `Name` | `string` | `name` | Human-readable name |
| `IcaoCode` | `string?` | `icao_code` | ICAO code |
| `Latitude` | `double` | `latitude` | Latitude |
| `Longitude` | `double` | `longitude` | Longitude |
| `ElevationFt` | `double?` | `elevation_ft` | Elevation (feet MSL) |
| `MagneticVariationDeg` | `double?` | `magnetic_variation_deg` | Mag variation |
| `Runways` | `List<RunwayDefinition>?` | `runways` | Runway definitions |
| `Carrier` | `CarrierProperties?` | `carrier` | Carrier properties |
| `RoadBase` | `RoadBaseProperties?` | `road_base` | Road base properties |
| `Farp` | `FarpProperties?` | `farp` | FARP properties |
| `Comms` | `List<CommsEntry>?` | `comms` | Communications |
| `Notes` | `string?` | `notes` | Free-text notes |

### `RunwayDefinition`

| Property | Type | JSON | Description |
|----------|------|------|-------------|
| `Designation` | `string` | `designation` | Runway designation (e.g. `"06/24"`) |
| `LengthFt` | `double?` | `length_ft` | Length (feet) |
| `WidthFt` | `double?` | `width_ft` | Width (feet) |
| `Surface` | `string?` | `surface` | Surface material |
| `HeadingDeg` | `double?` | `heading_deg` | Heading |
| `ThresholdLat` | `double?` | `threshold_lat` | Threshold latitude |
| `ThresholdLon` | `double?` | `threshold_lon` | Threshold longitude |
| `IlcFreq` | `double?` | `ils_freq` | ILS frequency |
| `IlcCourse` | `double?` | `ils_course` | ILS course |

### `CarrierProperties`

| Property | Type | JSON | Description |
|----------|------|------|-------------|
| `Name` | `string` | `name` | Carrier name |
| `Callsign` | `string?` | `callsign` | Carrier callsign |
| `DeckState` | `DeckState?` | `deck_state` | Deck ops state |
| `TacanChannel` | `string?` | `tacan_channel` | TACAN channel |
| `TacanBand` | `TacanBand?` | `tacan_band` | TACAN band |
| `IlsFreq` | `double?` | `ils_freq` | ILS frequency |
| `IlsCourse` | `double?` | `ils_course` | ILS course |
| `IlsMls` | `bool?` | `ils_mls` | MLS available |
| `HeadingDeg` | `double?` | `heading_deg` | Ship heading |
| `SpeedKts` | `double?` | `speed_kts` | Ship speed |
| `WindOverDeckKts` | `double?` | `wod_kts` | Wind over deck |

### `RoadBaseProperties`

| Property | Type | JSON | Description |
|----------|------|------|-------------|
| `HeadingDeg` | `double?` | `heading_deg` | Strip heading |

### `FarpProperties`

| Property | Type | JSON | Description |
|----------|------|------|-------------|
| `Name` | `string?` | `name` | FARP name |
| `TacanChannel` | `string?` | `tacan_channel` | TACAN channel |
| `TacanBand` | `TacanBand?` | `tacan_band` | TACAN band |

---

## Serialization

### `JsonSerializerOptionsFactory` (static)

Creates pre-configured `JsonSerializerOptions` with `EnumMember` support.

| Method | Return | Description |
|--------|--------|-------------|
| `CreateDefault()` | `JsonSerializerOptions` | Default options (not indented) |
| `Create(bool writeIndented)` | `JsonSerializerOptions` | Options with configurable indentation |

Options configured:
- `PropertyNamingPolicy = null` — preserves `[JsonPropertyName]` values as-authored
- `WriteIndented` — as specified
- `DefaultIgnoreCondition = Never` — nulls are serialized
- `Converters` includes `EnumMemberJsonConverterFactory`

### `EnumMemberJsonConverterFactory` (class)

A `JsonConverterFactory` that serializes enums using `[EnumMember(Value = "...")]` when present, falling back to the member name.

| Method | Description |
|--------|-------------|
| `CanConvert(Type)` | Returns `true` for enum types |
| `CreateConverter(Type, JsonSerializerOptions)` | Creates the inner converter |

### `SchemaVersions` (static)

| Field | Value |
|-------|-------|
| `CurrentVersion` | `"1.1.0"` |
| `CommunityFlightPlanSchemaUrl` | `"https://mctoolbox.uk/schema/v1.1.0/community-flightplan.schema.json"` |
| `OpTaskAirSchemaUrl` | `"https://mctoolbox.uk/schema/v1.1.0/op-task-air.schema.json"` |

---

## Validation

### `ISchemaValidator` (interface)

| Method | Return | Description |
|--------|--------|-------------|
| `ValidateAsync(string json, Uri schemaUri, CancellationToken)` | `Task<ValidationResult>` | Validate against remote schema |
| `Validate(string json, string schema)` | `ValidationResult` | Validate against inline schema |

### `JsonSchemaValidator` (class, implements `ISchemaValidator`)

Uses `JsonSchema.Net` for evaluation.

| Method | Return | Description |
|--------|--------|-------------|
| `ValidateAsync(string json, Uri schemaUri, CancellationToken)` | `Task<ValidationResult>` | Fetches schema via HTTP, validates |
| `Validate(string json, string schema)` | `ValidationResult` | Validates against inline schema text |

### `ValidationResult` (class)

| Property | Type | Description |
|----------|------|-------------|
| `IsValid` | `bool` | Whether no errors were found |
| `Errors` | `IReadOnlyList<ValidationError>` | Validation errors |

### `ValidationError` (class)

| Property | Type | Description |
|----------|------|-------------|
| `Path` | `string` | JSON path where error occurred |
| `Message` | `string` | Human-readable error description |
| `ErrorCode` | `string?` | Schema keyword (e.g. `"required"`, `"type"`) |

---

## Utilities

### `FlightPlanExtensions` (static)

Extension methods for `CommunityFlightPlan`.

| Method | Return | Description |
|--------|--------|-------------|
| `DeepClone(this CommunityFlightPlan)` | `CommunityFlightPlan` | Deep clone via JSON round-trip |
| `Copy(this CommunityFlightPlan)` | `CommunityFlightPlan` | Same as DeepClone |
| `IsValid(this CommunityFlightPlan)` | `Task<ValidationResult>` | Validate against published schema |

### `OpTaskAirExtensions` (static)

Extension methods for `OpTaskAir`.

| Method | Return | Description |
|--------|--------|-------------|
| `DeepClone(this OpTaskAir)` | `OpTaskAir` | Deep clone via JSON round-trip |
| `Copy(this OpTaskAir)` | `OpTaskAir` | Same as DeepClone |
| `IsValid(this OpTaskAir)` | `Task<ValidationResult>` | Validate against published schema |

---

## Full Examples

### Community Flight Plan

```csharp
using MCTUtils.CommunityStandards.Common;
using FlightPlan = MCTUtils.CommunityStandards.CommunityFlightPlan.CommunityFlightPlan;

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

string json = plan.ToJson(writeIndented: true);
var loaded = FlightPlan.FromJson(json);
var clone = plan.DeepClone();
plan.Save("flightplan.json", writeIndented: true);
var fromFile = FlightPlan.Load("flightplan.json");

// Validate
var result = await plan.IsValid();
if (!result.IsValid)
    foreach (var error in result.Errors)
        Console.WriteLine($"[{error.Path}] {error.Message}");
```

### Op Task Air with Weather

```csharp
using MCTUtils.CommunityStandards.Common;
using OpTaskAirDoc = MCTUtils.CommunityStandards.OpTaskAir.OpTaskAir;
using MCTUtils.CommunityStandards.OpTaskAir;

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
        TimeSeconds = 25200,
        Bullseye = new Bullseye { Red = new LatLon { Latitude = 42.0, Longitude = 41.0 } },
        Weather = new Weather
        {
            TemperatureC = 22.5,
            QnhHpa = 1013.2,
            VisibilityM = 10000,
            Winds = new Winds
            {
                Surface = new WindLayer { FromDeg = 180, SpeedKts = 12 },
                Altitude = new List<WindLayer>
                {
                    new() { AltitudeFt = 3000, FromDeg = 200, SpeedKts = 25 }
                }
            },
            Clouds = new CloudLayer { Density = 5, BaseFt = 3000 }
        }
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

string json = opTaskAir.ToJson(writeIndented: true);
var loaded = OpTaskAirDoc.FromJson(json);
var clone = opTaskAir.DeepClone();
```

---

## Dependencies

| Package | Version | Usage |
|---------|---------|-------|
| MCTUtils | 0.2.6 | Shared types and serialization infrastructure (ProjectReference) |
| JsonSchema.Net | 9.2.2 | JSON Schema evaluation |
| Microsoft.SourceLink.GitHub | 8.x | Source-level debugging (PrivateAssets) |





