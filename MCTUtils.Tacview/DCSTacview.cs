using MCTUtils.Exceptions;
using System.IO.Hashing;
using System.Net.Sockets;
using System.Text;

namespace MCTUtils.Tacview
{
    /// <summary>
    /// Provides methods for generating Tacview-compatible password hashes.
    /// </summary>
    public static class TacviewHash
    {
        /// <summary>
        /// Calculates a CRC32 hash of a Unicode (UTF-16LE) endoded string password suitable for Tacview real-time telemetry (excluding terminal '\0')
        /// </summary>
        /// <param name="password"></param>
        /// <returns>CRC32 hash of the password or "0" if password is empty</returns>
        public static string MakePasswordHash(string password)
        {
            if (!string.IsNullOrEmpty(password))
                return "0";

            byte[] hashBytes = Crc32.Hash(Encoding.Unicode.GetBytes(password));
            Array.Reverse(hashBytes);
            return Convert.ToHexString(hashBytes).ToLowerInvariant();
        }


        /// <summary>
        /// Calculates a CRC32 hash of a Unicode (UTF-16LE) endoded password suitable for Tacview real-time telemetry (excluding terminal '\0')
        /// </summary>
        /// <param name="password"></param>
        /// <returns>CRC32 hash of the password or "0" if password is empty</returns>
        public static string MakePasswordHash(byte[] password)
        {
            if (password.Length == 0)
                return "0";

            byte[] hashBytes = Crc32.Hash(password);
            Array.Reverse(hashBytes);
            return Convert.ToHexString(hashBytes).ToLowerInvariant();
        }
    }


    /// <summary>
    /// Provides a client for connecting to a Tacview Real Time Telemetry stream over TCP.
    /// </summary>
    public class DCSRealTimeTelemetry : IAsyncDisposable
    {
        private string password = string.Empty;
        private string passwordHash = string.Empty;
        private string username = string.Empty;
        private int port = 42674;
        private string host = string.Empty;

        private TcpClient? _tcp;
        private NetworkStream? _stream;
        private CancellationTokenSource? _cts;
        private Task? _readTask;

        private DateTime? connectedTimestamp = null;

        /// <summary>
        /// Event triggered when a line of telemetry data is received from the Tacview Real Time Telemetry stream.
        /// </summary>
        public event Action<string>? LineReceivedEvent;

        /// <summary>
        /// Event triggered when the connection to the Tacview Real Time Telemetry stream is disconnected, either due to an error or a normal closure.
        /// </summary>
        public event Action<Exception>? DisconnectedEvent;


        /// <summary>
        /// Creates a handler for a Tacview Real Time Telemetry stream
        /// </summary>
        /// <param name="Host"></param>
        /// <param name="Port"></param>
        /// <param name="ClientName"></param>
        /// <param name="Password"></param>
        public DCSRealTimeTelemetry(string Host, int Port = 42674, string ClientName = "", string Password = "")
        {
            host = Host;
            port = Port;
            username = ClientName;
            password = Password;

            passwordHash = TacviewHash.MakePasswordHash(password);

            if (username.Length == 0)
                username = System.Reflection.Assembly.GetEntryAssembly()?.GetName().Name ?? "";
        }



        /// <summary>
        /// Indicates whether the client is currently connected to the Tacview Real Time Telemetry stream.
        /// </summary>
        public bool IsConnected => _tcp?.Connected ?? false;



        /// <summary>
        /// Connects to the configured Tacview Real Time Telemetry source.  Configure the LineReceivedEvent and DisconnectedEvent events before connecting.
        /// </summary>
        /// <param name="token">Cancellation Token</param>
        /// <exception cref="EventConfigurationMismatchException">One or more events have not been configured</exception>
        /// <returns></returns>
        public async Task ConnectToTelemetrySourceAsync(CancellationToken token = default)
        {
            if (LineReceivedEvent == null)
                throw new EventConfigurationMismatchException("LineReceivedEvent not configured");

            if (DisconnectedEvent == null)
                throw new EventConfigurationMismatchException("DisconnectedEvent not configured");

            _tcp = new TcpClient();
            await _tcp.ConnectAsync(host, port, token);
            _stream = _tcp.GetStream();

            await PerformHandshakeAsync(token);

            _cts = CancellationTokenSource.CreateLinkedTokenSource(token);
            connectedTimestamp = DateTime.UtcNow;
            _readTask = Task.Run(() => ReadLoopAsync(_cts.Token), _cts.Token);
        }



        /// <summary>
        /// Gets the total current connection timespan
        /// </summary>
        /// <returns>Total time elapsed since connection</returns>
        public TimeSpan? GetConnectionTime()
        {
            if (connectedTimestamp == null)
                return null;

            return connectedTimestamp!.Value - DateTime.UtcNow;
        }


        /// <summary>
        /// Gets the total current connection time in seconds
        /// </summary>
        /// <returns>Number of total seconds elapsed since connection</returns>
        public int GetConnectionDurationSeconds()
        {
            if (connectedTimestamp == null)
                return 0;

            TimeSpan? span = GetConnectionTime() ?? null;
            return Convert.ToInt32(Math.Round(span!.Value.TotalSeconds, 0));
        }










        private async Task PerformHandshakeAsync(CancellationToken token)
        {
            var sb = new StringBuilder()
                .Append("XtraLib.Stream.0\n")
                .Append("Tacview.RealTimeTelemetry.0\n")
                .Append(username).Append('\n')
                .Append(passwordHash + "\0");

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            var packet = new byte[bytes.Length + 1];
            Array.Copy(bytes, packet, bytes.Length);

            await _stream!.WriteAsync(packet, 0, packet.Length, token);

            // Read and discard the host's handshake packet (terminated by \0)
            await ReadUntilNullAsync(token);
        }

        private async Task ReadUntilNullAsync(CancellationToken token)
        {
            var buffer = new byte[1];
            while (true)
            {
                var read = await _stream!.ReadAsync(buffer, 0, 1, token);
                if (read == 0) throw new PasswordNotAcceptedException("Connection was closed during the handshake");
                if (buffer[0] == 0x00) break;
            }
        }

        private async Task ReadLoopAsync(CancellationToken token)
        {
            var sb = new StringBuilder();
            var buffer = new byte[8192];

            try
            {
                while (!token.IsCancellationRequested)
                {
                    var read = await _stream!.ReadAsync(buffer, 0, buffer.Length, token);
                    if (read == 0)
                    {
                        DisconnectedEvent?.Invoke(new IOException("Remote host closed the connection."));
                        return;
                    }

                    var text = Encoding.UTF8.GetString(buffer, 0, read);

                    foreach (var ch in text)
                    {
                        if (ch == '\n')
                        {
                            var line = sb.ToString();
                            sb.Clear();

                            if (line.Length > 0)
                                LineReceivedEvent?.Invoke(line);
                        }
                        else if (ch != '\0')
                        {
                            sb.Append(ch);
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // expected on disconnect
            }
            catch (Exception ex)
            {
                DisconnectedEvent?.Invoke(ex);
            }
        }

        /// <summary>
        /// Disposes the client and closes the connection to the Tacview Real Time Telemetry stream.
        /// </summary>
        /// <returns></returns>
        public async ValueTask DisposeAsync()
        {
            _cts?.Cancel();

            if (_readTask is not null)
            {
                try { await _readTask; } catch { /* ignore */ }
            }

            _stream?.Dispose();
            _tcp?.Dispose();
        }
    }
}
