public class HeartbeatMonitor
{
    const double HEALTHY_CAP = .9;

    Dictionary<string, ISensor> sensors;
    List<string> heartbeatLogs;
    public bool HEALTHY { get; private set; }

    public HeartbeatMonitor()
    {
        sensors = new Dictionary<string, ISensor>();
        heartbeatLogs = new List<string>();
        HEALTHY = true;
    }

    public void Add(string id, ISensor sensor)
    {
        if (sensors.ContainsKey(id))
            throw new ArgumentException($"Sensor with ID {id} already exists.");

        sensors.Add(id, sensor);
        Log(id, sensor.Name + " added to heartbeat monitor.");
    }

    public void Log(string sensorId, string log)
    {
        if (!sensors.ContainsKey(sensorId))
            throw new ArgumentException($"Sensor with ID {sensorId} does not exist.");

        heartbeatLogs.Add(
            $"[{DateTime.Now:MM-dd-yyyy hh:mm:ss tt}] - Sensor ID: {sensorId}, Log: {log}"
        );
    }
}