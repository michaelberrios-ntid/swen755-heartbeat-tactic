using System.Data.Common;

public abstract class Sensor : ISensor
{
    // Automated Id assignment for a sensor
    private static int _nextId = 0;

    public int Id { get; private set; }
    public string Name { get; private set; }

    private static readonly Random random = new();
    public double health;
    private const double MIN_HEALTH = .9;

    private List<string> log;
    private IReadOnlyList<string> Log => log;

    private enum SensorStatus { OK, FAIL };

    public string LastLog { get; private set; }

    public Sensor(string name)
    {
        Id = _nextId++;
        Name = name;
        log = [];
        LastLog = string.Empty;
    }

    public bool IsAlive()
    {
        bool isHealthy = random.NextDouble() < 0.9;

        health = isHealthy
            ? 0.9 + random.NextDouble() * 0.1
            : random.NextDouble() * 0.1;

        SendHeartbeat();

        return isHealthy;
    }

    public void Restart()
    {
        bool recovered = random.NextDouble() < 0.7;

        health = recovered
            ? 0.9 + random.NextDouble() * 0.1
            : random.NextDouble() * 0.1;

        SendHeartbeat();
    }

    public void SendHeartbeat()
    {
        string status = (health > MIN_HEALTH ? SensorStatus.OK : SensorStatus.FAIL).ToString();

        LastLog = $"[{status}] - Health: {health:F2} - DateTime: {DateTime.Now:MM-dd-yyyy HH:mm:ss.fff}";

        log.Add(LastLog);
    }
}