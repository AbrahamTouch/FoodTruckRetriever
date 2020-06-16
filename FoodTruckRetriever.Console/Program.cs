
using FoodTruckRetriever.Console.Configuration;
using FoodTruckRetriever.Console.DayTimeRetriever;
using FoodTruckRetriever.Console.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FoodTruckRetriever.Console
{
    class Program
    {
        public static async Task Main(string[] args)
        {

            var services = ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();
            FoodTruckRetriever foodTruckRetriever = serviceProvider.GetService<FoodTruckRetriever>();

            try
            {
                await foodTruckRetriever.Run();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }


        }

        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();

            //Config
            var config = LoadConfiguration();
            services.AddOptions();
            services.Configure<WebSettingsConfiguration>(options => config.GetSection(nameof(WebSettingsConfiguration)).Bind(options));

            services.AddTransient<IDayTimeRetriever, SfoDayTimeRetriver>();
            services.AddTransient<IFoodTruckRepository, FoodTruckWebSocrataRepository>();
            services.AddTransient<FoodTruckRetriever>();

            return services;
        }

        public static IConfigurationRoot LoadConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.prod.json", optional: true, reloadOnChange: true);

            return builder.Build();
        }
    }
}
