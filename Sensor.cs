using System.Data.Common;

public abstract class Sensor : ISensor
{
    protected const double HEALTHY_CAP = 0.9;
    protected const double WARN_CAP = 0.8;

    public string Id { get; private set; }
    public string Name { get; private set; }
    public double Health { get; private set; }
    public bool FallbackMode { get; private set; }

    public List<string> Logs { get; private set; }
    private readonly Random random;

    public enum SensorStatus
    {
        HEALTHY,
        WARN,
        FAILED,
        RESTART,
        ADDED
    }

    public Sensor(string sensorId, string name)
    {
        Id = sensorId;
        Name = name;
        Health = 1.0;
        FallbackMode = false;
        Logs = [];
        random = new Random();

        Log(SensorStatus.ADDED, $"{GetType().Name} '{name}' initialized.");
    }

    public void CheckHealth()
    {
        int attempts = 0, max = 3;

        while (/*attempts < max && */!FallbackMode)
        {
            attempts++;

            if (IsAlive())
            {
                return;
            }
            else if (Health >= WARN_CAP)
            {
                Log(SensorStatus.WARN, $"Sensor '{Name}' health is below threshold: {Health:F2}. Calibrating.");
                Calibrate();
            }
            else
            {
                Log(SensorStatus.FAILED, $"Sensor '{Name}' health is critically low: {Health:F2}. Restarting.");
                Restart();
            }
        }

        // if (attempts >= max)
        // {
        //     Log(SensorStatus.FAILED, $"Sensor '{Name}' failed after {max} attempts. Catastrophic failure.");
        //     Fallback();
        // }
    }

    public bool IsAlive()
    {
        double roll = random.NextDouble();

        if (roll < WARN_CAP)
            Health = 0.9 + random.NextDouble() * 0.1;
        else if (roll < HEALTHY_CAP)
            Health = 0.8 + random.NextDouble() * 0.1;
        else
            Health = random.NextDouble() * 0.8;

        Log(SensorStatus.HEALTHY, $"Alive");

        return Health >= HEALTHY_CAP;
    }

    public void Calibrate()
    {
        Health += Health * 0.1;

        if (Health > 1.0)
            Health = 1.0;

        Log(SensorStatus.RESTART, $"Calibrated.");
    }

    private int attempts = 0, max = 3;
    public void Restart()
    {
        Health = random.NextDouble();

        if (Health > 1.0)
            Health = 1.0;

        if (Health >= WARN_CAP)
        {
            attempts = 0;
            Log(SensorStatus.HEALTHY, $"Restarted successfully.");
        }
        else
        {
            attempts++;
            Log(SensorStatus.FAILED, $"Restart failed");

            if (attempts >= max)
            {
                Log(SensorStatus.FAILED, $"Sensor '{Name}' failed after {max} attempts. Catastrophic failure.");
                Fallback();
                
                return;
            }
            else
                Restart();
        }
    }

    public void Fallback()
    {
        FallbackMode = true;
        Log(SensorStatus.FAILED, $"Sensor '{Name}' is in fallback mode. Health: {Health:F2}");
    }

    public void Log(SensorStatus status, string log = "")
    {
        Logs.Add(
            $"[{status}] - Name: {Name}\tHealth: {Health:F2}\tLog: {log}"
        );
    }

    public string GetLogs()
    {
        string logs = "Sensor Logs:\n";
        foreach (var log in Logs)
        {
            logs += $"{log}\n";
        }

        return logs;
    }
}