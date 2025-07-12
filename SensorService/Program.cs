using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Shared;

namespace SensorService
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var sensor in SensorRegistry.All)
            {
                var thread = new Thread(() => StartSensorServer(sensor));
                thread.Start();
            }

            Console.WriteLine("All sensors started.");
            Thread.Sleep(Timeout.Infinite); // keep app running
        }

        static void StartSensorServer(SensorInfo sensorInfo)
        {
            var sensor = new Sensor(sensorInfo.Id, sensorInfo.Name);
            var listener = new TcpListener(IPAddress.Loopback, sensorInfo.Port);
            listener.Start();

            Console.WriteLine($"Sensor '{sensorInfo.Name}' is listening on port {sensorInfo.Port}...");

            while (true)
            {
                using var client = listener.AcceptTcpClient();
                using var stream = client.GetStream();

                byte[] buffer = new byte[256];
                int read = stream.Read(buffer, 0, buffer.Length);
                string request = Encoding.UTF8.GetString(buffer, 0, read).Trim().ToUpper();

                string response = request switch
                {
                    SensorMessages.PING => sensor.CheckHealth(),
                    SensorMessages.RESTART => sensor.RestartAndReport(),
                    _ => "UNKNOWN_COMMAND"
                };

                byte[] reply = Encoding.UTF8.GetBytes(response);
                stream.Write(reply, 0, reply.Length);
            }
        }
    }
}
