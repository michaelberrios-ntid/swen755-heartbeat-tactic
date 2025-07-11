using System.Data.Common;

public abstract class Sensor : ISensor
{
    public string Name { get; private set; }
    public double Health { get; private set; }

    public List<string> Logs { get; private set; }
    public enum SensorStatus
    {
        HEALTHY,
        UNHEALTHY,
        FAILED,
        RESTART,
        ADDED
    }

    public Sensor(string name)
    {
        Name = name;
        Health = 1.0;
        Logs = [];
    }

    public bool IsAlive()
    {
        throw new NotImplementedException();
    }

    public void Calibrate()
    {
        throw new NotImplementedException();
    }

    public void Restart()
    {
        throw new NotImplementedException();
    }

    public void Log(SensorStatus status, string log)
    {
        Logs.Add(
            $"[{status}] - Name: {Name}\tLog: {log}"
        );
    }
}