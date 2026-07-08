# MCTUtils.Tacview

Tacview Real-Time Telemetry support for MCTUtils. Connects to Tacview telemetry streams, handles the handshake protocol, and provides real-time ACMI data.

```
dotnet add package MCTUtils.Tacview
```

Requires the `MCTUtils` core package (added automatically as a dependency).

## DCSRealTimeTelemetry

Connect to a Tacview Real-Time Telemetry source and receive ACMI lines as they arrive.

```csharp
using MCTUtils.Tacview;

var telemetry = new DCSRealTimeTelemetry(
    host: "192.168.1.100",
    port: 42674,
    clientName: "MyAnalyzer",
    password: "optional");

telemetry.LineReceivedEvent += line =>
{
    Console.WriteLine($"ACMI: {line}");
};

telemetry.DisconnectedEvent += ex =>
{
    Console.WriteLine($"Disconnected: {ex.Message}");
};

await telemetry.ConnectToTelemetrySourceAsync();

// ... later ...
Console.WriteLine($"Connected for {telemetry.GetConnectionDurationSeconds()}s");
await telemetry.DisposeAsync();
```

### Events

| Event | Description |
|-------|-------------|
| `LineReceivedEvent` | Fires for each complete ACMI line received |
| `DisconnectedEvent` | Fires when the remote host closes the connection or an error occurs |

## TacviewHash

Compute CRC32 password hashes for Tacview protocol authentication.

```csharp
using MCTUtils.Tacview;

string hash = TacviewHash.MakePasswordHash("myPassword");
// Returns CRC32 hex (lowercase) or "0" for empty passwords
```

### Methods

| Method | Description |
|--------|-------------|
| `MakePasswordHash(string)` | CRC32 of UTF-16LE encoded string |
| `MakePasswordHash(byte[])` | CRC32 of raw byte array |

## Dependencies

- **MCTUtils** (core) — exceptions and shared types
- **System.IO.Hashing** — CRC32 hash computation
