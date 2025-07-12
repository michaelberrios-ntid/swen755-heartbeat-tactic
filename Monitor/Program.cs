using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Shared;

namespace MonitorApp
{
    class Program
    {
        static readonly Dictionary<string, (string name, int port)> sensors = new()
        {
            { "lidar-1", ("LIDAR Front", 9001) },
            { "camera-1", ("Camera Left", 9002) },
            { "radar-1", ("Radar Front", 9003) },
        };

        static void Main()
        {
            Console.WriteLine("Heartbeat Monitor starting up...");

            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Sensor Status - {DateTime.Now:HH:mm:ss}\n");

                foreach (var sensor in SensorRegistry.All)
                {
                    string status = PingSensor("127.0.0.1", sensor.Port);
                    Console.WriteLine($"{sensor.Name} [{sensor.Id}] → {status}");
                }

                Console.WriteLine("\nPress Ctrl+C to exit.");
                Thread.Sleep(1000);
            }
        }

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