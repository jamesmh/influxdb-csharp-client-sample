namespace app.Models
{
    public class AltitudeModel
    {
        public string Time { get; init; }
        public int Altitude { get; init; }
        public string DisplayText => $"Plane was at altitude {Altitude} ft. at {Time}.";
    }
}
