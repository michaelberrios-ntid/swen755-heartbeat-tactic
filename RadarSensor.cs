public class RadarSensor : Sensor
{
    public RadarSensor(string id, string name) : base(id, name)
    {
        Log(
            SensorStatus.ADDED,
            $"Radar Sensor '{name}' initialized."
        );
    }
}