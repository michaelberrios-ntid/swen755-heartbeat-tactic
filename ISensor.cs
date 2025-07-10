public interface ISensor
{
    public string LastLog { get; }

    void SendHeartbeat();
    bool IsAlive();
    void Restart();
}