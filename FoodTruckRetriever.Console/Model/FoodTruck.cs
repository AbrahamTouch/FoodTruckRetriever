using System;

namespace FoodTruckPOC.Models
{
    [Serializable]
    public sealed class FoodTruck
    {
        public string Applicant { get; set; }

        public string Location { get; set; }

        public string start24 { get; set; }

        public string end24 { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public string optionaltext { get; set; }

        public override string ToString()
        {
            return ($"{Applicant}{Environment.NewLine}{Location}{Environment.NewLine}{Latitude}{Environment.NewLine}{Longitude}{Environment.NewLine}{optionaltext}{Environment.NewLine}{Environment.NewLine}");
        }

    }
}
