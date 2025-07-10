public class HeartbeatMonitor
{
    Dictionary<string, ISensor> sensors;

    public HeartbeatMonitor()
    {
        sensors = [];
    }

    public void Add(ISensor sensor, string id)
    {
        if (sensors.TryAdd(id, sensor))
            return;

        throw new Exception("Sensor with ID already exists");
    }

    public void CheckSensorsHealth()
    {
        foreach (KeyValuePair<string, ISensor> sensorKVP in sensors)
        {
            ISensor sensor = sensorKVP.Value;
            string key = sensorKVP.Key;

            Console.Write($"Sensor ID: {key} ");

            if (sensor.IsAlive())
            {
                Console.WriteLine($"[OK]");
            }
            else
            {
                Console.WriteLine($"[WARN] - unresponsive. Attempting to restart");

                sensor.Restart();

                if (sensor.IsAlive())
                {
                    Console.WriteLine($"[SUCCESS] - sensor successfully restart and alive");
                }
                else
                {
                    Console.WriteLine($"[FAIL] - sensor failed to recover from failure.");
                    // TODO: TriggerFallback Tactics
                    // ...
                }
            }
        }
    }

    public List<string> GetSensorLogs()
    {
        List<string> logs = [];

        foreach (KeyValuePair<string, ISensor> sensorKVP in sensors)
            logs.Add(sensorKVP.Value.LastLog);

        return logs;
    }
}