public class LIDARSensor : Sensor
{
    public LIDARSensor(string id, string name) : base(id, name)
    {
        Log(
            SensorStatus.ADDED,
            $"LIDAR Sensor '{name}' initialized."
        );
    }
}