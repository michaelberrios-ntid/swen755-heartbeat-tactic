namespace Shared
{
    public static class SensorRegistry
    {
        public static readonly List<SensorInfo> All = new()
        {
            new SensorInfo("lidar-1", "LIDAR Front", 9001),
            new SensorInfo("lidar-2", "LIDAR Rear", 9002),
            new SensorInfo("camera-1", "Camera Left", 9003),
            new SensorInfo("camera-2", "Camera Right", 9004),
            new SensorInfo("radar-1", "Radar Front", 9005),
            new SensorInfo("radar-2", "Radar Rear", 9006)
        };
    }

    public record SensorInfo(string Id, string Name, int Port);
}
