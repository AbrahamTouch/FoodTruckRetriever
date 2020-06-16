namespace FoodTruckPOC.Models
{
    public sealed class DayTime
    {
        public string Day { get; set; }
        public int Hour { get; set; }
        public int MinutesInTheHour { get; set; }

        public override string ToString()
        {
            return $"{Day} {Hour} {MinutesInTheHour}";
        }
    }
}

