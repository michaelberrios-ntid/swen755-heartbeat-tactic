// Setup
var monitor = new HeartbeatMonitor();

// Add sensors
monitor.Add("lidar-1", new LIDARSensor("lidar-1", "LIDAR Front"));
monitor.Add("lidar-2", new LIDARSensor("lidar-2", "LIDAR Rear"));

monitor.Add("camera-1", new CameraSensor("camera-1", "Camera Left"));
monitor.Add("camera-2", new CameraSensor("camera-2", "Camera Right"));

monitor.Add("radar-1", new RadarSensor("radar-1", "Radar Front"));
monitor.Add("radar-2", new RadarSensor("radar-2", "Radar Rear"));

try
{
    while (monitor.healthy)
    {
        Console.Clear();
        monitor.CheckHealth();

        Console.WriteLine($"\nSensor Logs {DateTime.Now:MM-dd-yyyy hh:mm:ss tt}:\n");

        Console.WriteLine(monitor.GetHeartbeatLogs());
        Console.WriteLine("========================================\n");

        Thread.Sleep(1000);
    }

    throw new Exception("Heartbeat monitor is not healthy. Catastrophic Failure.");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}