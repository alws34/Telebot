namespace CPUID.Models
{
    public class Sensor
    {
        public string Name { get; }
        public float Value { get; }
        public float Min { get; }
        public float Max { get; }

        public Sensor(string name, float value, float min, float max)
        {
            Name = name;
            Value = value;
            Min = min;
            Max = max;
        }
    }
}
