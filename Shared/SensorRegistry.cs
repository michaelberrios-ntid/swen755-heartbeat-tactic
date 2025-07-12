namespace Shared
{
    public static class SensorRegistry
    {
        public static readonly List<SensorInfo> All = new()
        {
            new SensorInfo("lidar-1", "LIDAR Front", 9001),
            new SensorInfo("lidar-2", "LIDAR Rear", 9002),
            new SensorInfo("lidar-1-backup", "LIDAR Front Backup", 9003, true),
            new SensorInfo("lidar-2-backup", "LIDAR Rear Backup", 9004, true),

            new SensorInfo("camera-1", "Camera Left", 9005),
            new SensorInfo("camera-2", "Camera Right", 9006),
            new SensorInfo("camera-1-backup", "Camera Left Backup", 9007, true),
            new SensorInfo("camera-2-backup", "Camera Right Backup", 9008, true),

            new SensorInfo("radar-1", "Radar Front", 9009),
            new SensorInfo("radar-2", "Radar Rear", 9010),
            new SensorInfo("radar-1-backup", "Radar Front Backup", 9011, true),
            new SensorInfo("radar-2-backup", "Radar Rear Backup", 9012, true)
        };
    }

    public record SensorInfo(string Id, string Name, int Port, bool IsBackup = false);
}
