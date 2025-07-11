public interface ISensor
{
    public string Name { get; }
    public double Health { get; }
    public List<string> Logs { get; }

    bool IsAlive();
    void Calibrate();
    void Restart();
    void Log(Sensor.SensorStatus status, string log);
}