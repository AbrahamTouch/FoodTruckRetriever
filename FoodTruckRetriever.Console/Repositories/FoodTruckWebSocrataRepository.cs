using FoodTruckPOC.Models;
using FoodTruckRetriever.Console.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FoodTruckRetriever.Console.Repositories
{
    public class FoodTruckWebSocrataRepository : IFoodTruckRepository
    {
        /// <summary>
        /// String to query socrata web api.
        /// Query will select the data we need and partially filter out unwanted foodtrucks as well as order the data by name 
        ///{0}: day
        ///{1}: Latitude
        ///{2}: Longitude
        ///{3}: rangeInMeters
        /// </summary>

        private static readonly string QUERYSTRING = @"$query=SELECT Applicant,location,end24,start24,Latitude,Longitude,optionaltext WHERE lower(DayOfWeekStr)=='{0}' and within_circle(location_2,{1},{2},{3}) ORDER BY Applicant";

        private static readonly double METERS_IN_MILES = 1609.34;

        private WebSettingsConfiguration WebSettings { get; set; }


        public FoodTruckWebSocrataRepository(IOptions<WebSettingsConfiguration> settings)
        {
            WebSettings = settings.Value;
        }

        public async Task<IEnumerable<FoodTruck>> GetOpenFoodTruckDataAsync(DayTime currentDayTime, double originLatitude, double originLongitude, double rangeInMiles)
        {
            IList<FoodTruck> foodTrucks = await GetFoodTruckFromWeb(WebSettings, currentDayTime.Day, originLatitude, originLongitude, rangeInMiles * METERS_IN_MILES);

            return foodTrucks.Where(foodTruck => IsOpenFoodTruck(foodTruck, currentDayTime));

        }

        private bool IsOpenFoodTruck(FoodTruck foodTruck, DayTime currentDayTime)
        {
            string[] startTime = foodTruck.start24.Split(":");
            string[] endTime = foodTruck.end24.Split(":");

            int startHour = Int32.Parse(startTime[0].Trim());
            int startMin = Int32.Parse(startTime[1].Trim());

            int endHour = Int32.Parse(endTime[0].Trim());
            int endMin = Int32.Parse(endTime[1].Trim());

            if ((startHour < currentDayTime.Hour || (startHour == currentDayTime.Hour && startMin <= currentDayTime.MinutesInTheHour)) &&
                (endHour > currentDayTime.Hour || (endHour == currentDayTime.Hour && endMin > currentDayTime.MinutesInTheHour)))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private static async Task<IList<FoodTruck>> GetFoodTruckFromWeb(WebSettingsConfiguration webSettings, string day, double originLatitude, double originLongitude, double rangeInMiles)
        {
            using (HttpClient webClient = new HttpClient())
            {
                //Setup AppToken Header
                webClient.DefaultRequestHeaders.Add("X-App-Token", webSettings.AppToken);

                //Setup URI
                UriBuilder uriBuilder = new UriBuilder(webSettings.Schema, webSettings.HostName);
                uriBuilder.Path = webSettings.Path;

                //Query will select the data we need and partially filter out unwanted foodtrucks as well as order the data by application
                uriBuilder.Query = String.Format(QUERYSTRING, day, originLatitude, originLongitude, rangeInMiles);

                HttpResponseMessage response = webClient.GetAsync(uriBuilder.Uri).Result;

                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<IList<FoodTruck>>(await response.Content.ReadAsStringAsync());
                }
                else
                {
                    throw new Exception(await response.Content.ReadAsStringAsync());
                }
            }
        }

    }
}
