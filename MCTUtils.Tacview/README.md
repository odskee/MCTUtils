# MCTUtils.Tacview

[![NuGet](https://img.shields.io/nuget/v/MCTUtils.Tacview.svg)](https://www.nuget.org/packages/MCTUtils.Tacview/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/MCTUtils.Tacview.svg)](https://www.nuget.org/packages/MCTUtils.Tacview/)
[![License](https://img.shields.io/github/license/Odskee/MCTUtils.Tacview)](LICENSE)

<br />

Tacview Real-Time Telemetry support for MCTUtils. Connects to Tacview telemetry streams, handles the handshake protocol, and provides real-time ACMI data.

```
dotnet add package MCTUtils.Tacview
```

| | |
|---|---|
| **Target** | .NET 8 |
| **Version** | 1.0.0 |
| **Dependency** | MCTUtils (core) — added automatically |
| **Repository** | [github.com/odskee/MCTUtils](https://github.com/odskee/MCTUtils) |
| **IntelliSense** | Full XML docs for all public APIs |
| **Debugging** | SourceLink support |

---

## Namespace: `MCTUtils.Tacview`

Two public types:

- `TacviewHash` — CRC32 password hash computation
- `DCSRealTimeTelemetry` — TCP telemetry client

---

## `TacviewHash` (static)

Computes CRC32 password hashes for Tacview protocol authentication using `System.IO.Hashing.Crc32`.

| Method | Return | Description |
|--------|--------|-------------|
| `MakePasswordHash(string password)` | `string` | CRC32 of UTF-16LE encoded string. Returns `"0"` for empty/null. |
| `MakePasswordHash(byte[] password)` | `string` | CRC32 of raw byte array. Returns `"0"` for empty. |

```csharp
using MCTUtils.Tacview;

string hash = TacviewHash.MakePasswordHash("myPassword");
string hashFromBytes = TacviewHash.MakePasswordHash(
    Encoding.Unicode.GetBytes("myPassword"));
```

---

## `DCSRealTimeTelemetry` (class, implements `IAsyncDisposable`)

TCP client for connecting to a Tacview Real-Time Telemetry source, performing the handshake, and reading ACMI data lines as they arrive.

### Constructor

| Constructor | Description |
|-------------|-------------|
| `DCSRealTimeTelemetry(string Host, int Port = 42674, string ClientName = "", string Password = "")` | Creates a new telemetry client. Empty client name falls back to the entry assembly name. Empty password produces hash `"0"`. |

### Properties

| Property | Type | Description |
|----------|------|-------------|
| `IsConnected` | `bool` | Whether the TCP connection is currently established |

### Events

| Event | Type | Description |
|-------|------|-------------|
| `LineReceivedEvent` | `Action<string>?` | Fires for each complete ACMI line (delimited by `\n`) |
| `DisconnectedEvent` | `Action<Exception>?` | Fires when the remote host closes the connection or an error occurs |

### Methods

| Method | Return | Description |
|--------|--------|-------------|
| `ConnectToTelemetrySourceAsync(CancellationToken token)` | `Task` | Connects, performs handshake, starts read loop. **Requires** both events to be subscribed. |
| `GetConnectionTime()` | `TimeSpan?` | Time elapsed since connection, or null if not connected |
| `GetConnectionDurationSeconds()` | `int` | Connection duration in seconds (rounded) |
| `DisposeAsync()` | `ValueTask` | Cancels read loop, closes TCP connection, disposes resources |

### Exceptions

| Exception | Condition |
|-----------|-----------|
| `EventConfigurationMismatchException` | `ConnectToTelemetrySourceAsync()` called without subscribing to both events |
| `PasswordNotAcceptedException` | Handshake failed — remote host closed connection |

### Complete Example

```csharp
using MCTUtils.Tacview;

await using var telemetry = new DCSRealTimeTelemetry(
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

try
{
    await telemetry.ConnectToTelemetrySourceAsync();
    Console.WriteLine($"Connected for {telemetry.GetConnectionDurationSeconds()}s");

    // Keep running — events fire on background thread
    await Task.Delay(TimeSpan.FromMinutes(5));
}
catch (EventConfigurationMismatchException ex)
{
    // Events not subscribed
}
catch (PasswordNotAcceptedException ex)
{
    // Bad password
}
catch (SocketException ex)
{
    // Connection failed
}
```

---

## Dependencies

| Package | Version | Usage |
|---------|---------|-------|
| MCTUtils | 1.0.0 | Shared exceptions and types (ProjectReference) |
| System.IO.Hashing | 8.0.0 | CRC32 hash computation |
| Microsoft.SourceLink.GitHub | 8.x | Source-level debugging (PrivateAssets) |

















