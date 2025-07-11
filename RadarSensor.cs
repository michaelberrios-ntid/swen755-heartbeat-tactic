public class RadarSensor : Sensor
{
    public RadarSensor(string name) : base(name)
    {
        Log(
            SensorStatus.ADDED,
            $"Radar sensor '{name}' initialized."
        );
    }
}