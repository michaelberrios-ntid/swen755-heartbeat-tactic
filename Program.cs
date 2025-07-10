// Setup
var monitor = new HeartbeatMonitor();

// Add sensors
monitor.Add(new LIDARSensor("LIDAR Front"), "lidar-1");
monitor.Add(new LIDARSensor("LIDAR Rear"), "lidar-2");

monitor.Add(new CameraSensor("Camera Left"), "camera-1");
monitor.Add(new CameraSensor("Camera Right"), "camera-2");

monitor.Add(new RadarSensor("Radar Front"), "radar-1");
monitor.Add(new RadarSensor("Radar Rear"), "radar-2");

// Main loop
while (true)
{
    Console.Clear();
    monitor.CheckSensorsHealth();

    Console.WriteLine("\nSensor Logs (latest entry):\n");

    foreach (string logs in monitor.GetSensorLogs())
    {
        if (logs.Contains("OK"))
            Console.ForegroundColor = ConsoleColor.Green;
        else if (logs.Contains("FAIL"))
            Console.ForegroundColor = ConsoleColor.Red;
        else
            Console.ResetColor();

        Console.WriteLine(logs);
        Console.ResetColor();
    }

    Thread.Sleep(1000); // 300ms heartbeat interval
}
