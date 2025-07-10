using System.Data.Common;

public abstract class Sensor : ISensor
{
    // Automated Id assignment for a sensor
    private static int _nextId = 0;

    public int Id { get; private set; }
    public string Name { get; private set; }

    public Sensor(string name)
    {
        Id = _nextId++;
        Name = name;
    }

    public bool IsAlive()
    {
        throw new NotImplementedException();
    }

    public void Restart()
    {
        throw new NotImplementedException();
    }

    public void SendHeartbeat()
    {
        throw new NotImplementedException();
    }
}