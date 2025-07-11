// Setup
var monitor = new HeartbeatMonitor();

// Add sensors
monitor.Add("lidar-1", new LIDARSensor("LIDAR Front"));
monitor.Add("lidar-2", new LIDARSensor("LIDAR Rear"));

monitor.Add("camera-1", new CameraSensor("Camera Left"));
monitor.Add("camera-2", new CameraSensor("Camera Right"));

monitor.Add("radar-1", new RadarSensor("Radar Front"));
monitor.Add("radar-2", new RadarSensor("Radar Rear"));

// Main loop
while (monitor.HEALTHY)
{
    Console.Clear();

    Console.WriteLine($"\nSensor Logs {DateTime.Now:MM-dd-yyyy hh:mm:ss tt}:\n");

    Thread.Sleep(1000);
}