using FoodTruckPOC.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FoodTruckRetriever.Console.Repositories
{
    public interface IFoodTruckRepository
    {
        Task<IEnumerable<FoodTruck>> GetOpenFoodTruckDataAsync(DayTime dayTime, double originLatitude, double originLongitude, double rangeInMiles);
    }
}
