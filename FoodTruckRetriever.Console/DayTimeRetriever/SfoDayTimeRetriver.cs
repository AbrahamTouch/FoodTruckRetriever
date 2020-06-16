using FoodTruckPOC.Models;
using System;
using System.Collections.Generic;
using System.Text;
using TimeZoneConverter;

namespace FoodTruckRetriever.Console.DayTimeRetriever
{
    public class SfoDayTimeRetriver : IDayTimeRetriever
    {
        private const string SANFRANCISCO_TIMEZONEID = "Pacific Standard Time";


        public DayTime GetCurrentDayTime()
        {
            //Using TZConvert to allow for an operating system agnostic lookup and retrival of TimeZone
            DateTime currentSFOTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TZConvert.GetTimeZoneInfo(SANFRANCISCO_TIMEZONEID));

            return new DayTime { Day = currentSFOTime.DayOfWeek.ToString().ToLower(), Hour = currentSFOTime.Hour, MinutesInTheHour = currentSFOTime.Minute };
        }
    }
}
