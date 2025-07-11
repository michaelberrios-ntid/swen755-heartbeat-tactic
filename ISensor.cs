public interface ISensor
{
    public string Name { get; }
    public double Health { get; }
    public bool FallbackMode { get; }
    public List<string> Logs { get; }

    void CheckHealth();
    bool IsAlive();
    void Calibrate();
    void Restart();
    void Log(Sensor.SensorStatus status, string log);
    string GetLogs();
}