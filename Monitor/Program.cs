using System.Net.Sockets;
using System.Text;
using Shared;

namespace MonitorApp
{
    class Program
    {
        static void Main()
        {
            // Initialize backup sensors from registry
            var activeSensors = new Dictionary<string, SensorInfo>();

            foreach (var sensor in SensorRegistry.All)
            {
                if (!sensor.IsBackup)
                    activeSensors[sensor.Id] = sensor;
            }

            // Used to switch TO backup if primary fails
            var backupLookup = SensorRegistry.All
                .Where(s => s.IsBackup)
                .ToDictionary(
                    backup => backup.Id.Replace("-backup", ""),
                    backup => backup
                );

            // Used to switch BACK TO primary if it recovers
            var primaryLookup = SensorRegistry.All
                .Where(s => !s.IsBackup)
                .ToDictionary(
                    primary => primary.Id,
                    primary => primary
                );

            Console.WriteLine("Heartbeat Monitor starting up...");

            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Sensor Status - {DateTime.Now:HH:mm:ss}\n");

                foreach (var (id, currentSensor) in activeSensors.ToList())
                {
                    string status = PingSensor("127.0.0.1", currentSensor.Port);
                    Console.WriteLine($"{currentSensor.Name} → {status}");
                    
                    // Handle the fallback logic if a primary sensor fails
                    if (status.StartsWith("FALLBACK") || status.StartsWith("FAIL"))
                    {
                        if (!currentSensor.IsBackup && backupLookup.TryGetValue(id, out var backup))
                        {
                            Console.WriteLine($"→ Switching to backup: {backup.Name}");
                            activeSensors[id] = backup;
                        }
                    }
                    // Handle the fallback logic if a primary sensor recovers, switch back to the primary sensor
                    else if (currentSensor.IsBackup && primaryLookup.TryGetValue(id, out var primary))
                    {
                        // Check if primary is now healthy
                        string primaryStatus = PingSensor("127.0.0.1", primary.Port);
                        if (primaryStatus.StartsWith("HEALTHY") || primaryStatus.StartsWith("WARN"))
                        {
                            Console.WriteLine($"→ Primary recovered. Switching back to: {primary.Name}");
                            activeSensors[id] = primary;
                        }
                    }
                }

                Console.WriteLine("\nPress Ctrl+C to exit.");
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// Pings a sensor and returns its status.
        /// </summary>
        /// <param name="host">The host address of the sensor.</param>
        /// <param name="port">The port number of the sensor.</param>
        /// <returns>Status message from the sensor.</returns>
        static string PingSensor(string host, int port)
        {
            try
            {
                using var client = new TcpClient(host, port);
                using var stream = client.GetStream();

                byte[] message = Encoding.UTF8.GetBytes(SensorMessages.PING);
                stream.Write(message, 0, message.Length);

                byte[] buffer = new byte[256];
                int read = stream.Read(buffer, 0, buffer.Length);
                return Encoding.UTF8.GetString(buffer, 0, read);
            }
            catch (Exception ex)
            {
                return $"UNREACHABLE ({ex.Message})";
            }
        }
    }
}