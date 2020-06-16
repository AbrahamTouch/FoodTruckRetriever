using FoodTruckPOC.Models;
using FoodTruckRetriever.Console.DayTimeRetriever;
using FoodTruckRetriever.Console.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodTruckRetriever.Console
{
    public class FoodTruckRetriever
    {
        private IFoodTruckRepository FoodTruckRepository { get; set; }
        private IDayTimeRetriever DayTimeRetriever { get; set; }

        public FoodTruckRetriever(IFoodTruckRepository foodTruckRepository, IDayTimeRetriever dayTimeRetreiver)
        {
            FoodTruckRepository = foodTruckRepository;
            DayTimeRetriever = dayTimeRetreiver;
        }


        public async Task Run()
        {
            double myLatitude;
            double myLongitude;
            double rangeInMiles;

            System.Console.WriteLine($"Welcome to the San Francisco Food Truck Finder App{Environment.NewLine}");

            System.Console.WriteLine($"Please enter your Latitude");
            while (!Double.TryParse(System.Console.ReadLine(), out myLatitude))
            {
                System.Console.WriteLine("Please enter a valid decimal number");
            }

            System.Console.WriteLine($"Please enter your Longitude");
            while (!Double.TryParse(System.Console.ReadLine(), out myLongitude))
            {
                System.Console.WriteLine("Please enter a valid decimal number");
            }

            System.Console.WriteLine($"Please enter the range in miles you want to search within");
            while (!Double.TryParse(System.Console.ReadLine(), out rangeInMiles))
            {
                System.Console.WriteLine("Please enter a valid decimal number");
            }

            IEnumerable<FoodTruck> openFoodTrucks = await FoodTruckRepository.GetOpenFoodTruckDataAsync(DayTimeRetriever.GetCurrentDayTime(), myLatitude, myLongitude, rangeInMiles);

            System.Console.Write(Environment.NewLine);

            if (openFoodTrucks.Count() == 0)
            {
                System.Console.WriteLine("There are no food trucks within the range specified");
            }
            else
            {
                foreach (FoodTruck foodTruck in openFoodTrucks)
                {
                    System.Console.WriteLine(foodTruck);
                }
            }

            System.Console.ReadLine();
        }
    }
}
