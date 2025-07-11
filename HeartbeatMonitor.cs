public class HeartbeatMonitor
{
    Dictionary<string, ISensor> sensors;
    List<string> heartbeatLogs;
    public bool healthy { get; private set; }

    public HeartbeatMonitor()
    {
        sensors = new Dictionary<string, ISensor>();
        heartbeatLogs = new List<string>();
        healthy = true;
    }

    public void Add(string id, ISensor sensor)
    {
        if (sensors.ContainsKey(id))
            throw new ArgumentException($"Sensor with ID {id} already exists.");

        sensors.Add(id, sensor);
        Log($"Sensor '{sensor.Name}' with ID '{id}' added.");
    }

    public void CheckHealth()
    {
        int totalHealhtySensors = 0;

        foreach (var sensor in sensors.Values)
        {
            sensor.CheckHealth();
            if (sensor.FallbackMode)
                totalHealhtySensors++;
        }

        healthy = totalHealhtySensors != sensors.Count;
    }

    public void Log(string log)
    {
        heartbeatLogs.Add(log);
    }

    public string GetHeartbeatLogs()
    {
        string logs = "Heartbeat Logs:\n";
        foreach (var log in heartbeatLogs)
        {
            logs += $"{log}\n";
        }
        logs += "=========================\n";

        logs += "Sensor Status:\n";
        foreach (var sensor in sensors.Values)
        {
            logs += sensor.GetLogs() + "\n";
        }

        return logs;
    }
}