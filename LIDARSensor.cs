public class LIDARSensor : Sensor
{
    public LIDARSensor(string name) : base(name)
    {
        Log(
            SensorStatus.ADDED,
            $"LIDAR sensor '{name}' initialized."
        );
    }
}