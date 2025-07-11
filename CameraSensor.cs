public class CameraSensor : Sensor
{
    public CameraSensor(string name) : base(name)
    {
        Log(
            SensorStatus.ADDED,
            $"Camera sensor '{name}' initialized."
        );
    }
}