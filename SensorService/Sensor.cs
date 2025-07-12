using System;
using Shared;

namespace SensorService
{
    public class Sensor
    {
        public string Id { get; }
        public string Name { get; }
        public double Health { get; private set; }
        public bool FallbackMode { get; private set; }

        private static readonly Random random = new();
        private const double HEALTHY_CAP = 0.9;
        private const double WARN_CAP = 0.8;

        public Sensor(string id, string name)
        {
            Id = id;
            Name = name;
            Health = 1.0;
            FallbackMode = false;
        }

        public string CheckHealth()
        {
            if (FallbackMode)
                return $"FALLBACK {Health:F2}";

            double roll = random.NextDouble();

            if (roll < 0.90)
                Health = 0.9 + random.NextDouble() * 0.1;
            else if (roll < 0.98)
                Health = 0.8 + random.NextDouble() * 0.1;
            else
                Health = 0.5 + random.NextDouble() * 0.3;

            if (Health < WARN_CAP)
            {
                FallbackMode = true;
                return $"FAIL {Health:F2}";
            }

            if (Health < HEALTHY_CAP)
                return $"WARN {Health:F2}";

            return $"HEALTHY {Health:F2}";
        }

        public string RestartAndReport()
        {
            FallbackMode = false;
            Health = 0.95;
            return $"RESTARTED {Health:F2}";
        }
    }
}