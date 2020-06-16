using FoodTruckPOC.Models;

namespace FoodTruckRetriever.Console.DayTimeRetriever
{
    public interface IDayTimeRetriever
    {
        DayTime GetCurrentDayTime();
    }
}
