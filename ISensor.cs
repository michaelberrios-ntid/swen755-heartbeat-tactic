public interface ISensor
{
    void SendHeartbeat();
    bool IsAlive();
    void Restart();
}