public class CameraSensor : Sensor
{
    public CameraSensor(string id, string name) : base(id, name)
    {
        Log(
            SensorStatus.ADDED,
            $"Camera Sensor '{name}' initialized."
        );
    }
}