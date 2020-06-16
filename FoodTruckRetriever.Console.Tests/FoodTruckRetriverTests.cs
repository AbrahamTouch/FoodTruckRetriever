using FoodTruckPOC.Models;
using FoodTruckRetriever.Console.DayTimeRetriever;
using FoodTruckRetriever.Console.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace FoodTruckRetriever.Console.Tests
{
    public class FoodTruckRetriverTests
    {
        [Fact]
        public async Task OutputOfFoodTrucksIsFormattedCorrectly()
        {
            //Arrange
            DayTime dayTime = new DayTime { Day = "Monday", Hour = 6, MinutesInTheHour = 30 };

            Mock<IDayTimeRetriever> mockDayTimeRetriever = new Mock<IDayTimeRetriever>();
            mockDayTimeRetriever.Setup(x => x.GetCurrentDayTime()).Returns(dayTime);

            Mock<IFoodTruckRepository> mockFoodTruckRepository = new Mock<IFoodTruckRepository>();
            mockFoodTruckRepository.Setup(x => x.GetOpenFoodTruckDataAsync(It.IsAny<DayTime>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>())).Returns(Task.FromResult<IEnumerable<FoodTruck>>(new List<FoodTruck>
            { new FoodTruck { Applicant = "Georges FoodTruck", start24 = "3:30", end24 = "6:40", Latitude = "Far", Longitude = "Farther", Location = "405 Allbright", optionaltext = "Indian Food" },
              new FoodTruck { Applicant = "Todds FoodTruck", start24 = "6:30", end24 = "7:30", Latitude = "Far", Longitude = "Farther", Location = "Unknown", optionaltext = "Not Indian Food" },
              new FoodTruck { Applicant = "Megans FoodTruck", start24 = "1:30", end24 = "7:00", Latitude = "Far", Longitude = "Farther", Location = "Unknown", optionaltext = "Cloudy with MeatBalls" },
            }));

            FoodTruckRetriever foodTruckRetriever = new FoodTruckRetriever(mockFoodTruckRepository.Object, mockDayTimeRetriever.Object);

            //Setup Console with dummy data and have console write to a string
            StringWriter stringWriter = new StringWriter();
            StringReader stringReader = new StringReader($"30{Environment.NewLine}-122{Environment.NewLine}500{Environment.NewLine}");

            System.Console.SetOut(stringWriter);
            System.Console.SetIn(stringReader);

            //Act
            await foodTruckRetriever.Run();

            //Assert
            Assert.Equal("Welcome to the San Francisco Food Truck Finder App\r\n\r\nPlease enter your Latitude\r\nPlease enter your Longitude\r\nPlease enter the range in miles you want to search within\r\n\r\nGeorges FoodTruck\r\n405 Allbright\r\nFar\r\nFarther\r\nIndian Food\r\n\r\n\r\nTodds FoodTruck\r\nUnknown\r\nFar\r\nFarther\r\nNot Indian Food\r\n\r\n\r\nMegans FoodTruck\r\nUnknown\r\nFar\r\nFarther\r\nCloudy with MeatBalls\r\n\r\n\r\n", value);
        }
    }
}
