using System;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;
using Microsoft.Extensions.DependencyInjection;
using Test_Taste_Console_Application.Constants;
using Test_Taste_Console_Application.Domain.Services;
using Test_Taste_Console_Application.Domain.Services.Interfaces;
using Test_Taste_Console_Application.Utilities;

namespace Test_Taste_Console_Application
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();

            // Configure services
            ConfigureServices(serviceCollection);

            // Execute service operations
            RunServiceOperations(serviceCollection);
        }

        private static void RunServiceOperations(IServiceCollection serviceCollection)
        {
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var screenOutputService = serviceProvider.GetService<IOutputService>();
            var fileOutputService = serviceProvider.GetService<IFileOutputService>();

            try
            {
                Console.WriteLine("Loading Solar System data...");
                screenOutputService.OutputAllPlanetsAndTheirAverageMoonGravityToConsole();
                screenOutputService.OutputAllMoonsAndTheirMassToConsole();
                screenOutputService.OutputAllPlanetsAndTheirMoonsToConsole();

                Console.WriteLine("Processing planets and moons...");
                Console.WriteLine("Calculating average moon temperatures...");
                screenOutputService.OutputAllPlanetsAndTheirAverageMoonTemperatureToConsole();
            }
            catch (Exception exception)
            {
                Logger.Instance.Error(
                    $"{LoggerMessage.ScreenOutputOperationFailed}{exception.Message}");

                Console.WriteLine(
                    $"{ExceptionMessage.ScreenOutputOperationFailed}{exception.Message}");

                System.Diagnostics.Debug.WriteLine(
                    $"{ExceptionMessage.ScreenOutputOperationFailed}{exception.Message}");
            }

            try
            {
                Console.WriteLine("Writing results to file...");
                if (fileOutputService.OutputAllPlanetsAndTheirAverageMoonTemperatureToFile())
                {
                    Console.WriteLine("Operation completed successfully.");
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.Error(
                    $"{LoggerMessage.FileOutputOperationFailed}{exception.Message}");

                Console.WriteLine(
                    $"{ExceptionMessage.FileOutputOperationFailed}{exception.Message}");

                System.Diagnostics.Debug.WriteLine(
                    $"{ExceptionMessage.FileOutputOperationFailed}{exception.Message}");
            }

            serviceProvider.Dispose();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // Configure logging
            XmlConfigurator.Configure(
                LogManager.GetRepository(Assembly.GetEntryAssembly()),
                new FileInfo(ConfigurationFileName.Logger));

            // Register services
            serviceCollection.AddHttpClient<HttpClientService>();
            serviceCollection.AddSingleton<IPlanetService, PlanetService>();
            serviceCollection.AddSingleton<IOutputService, ScreenOutputService>();
            serviceCollection.AddSingleton<IFileOutputService, FileOutputService>();
            serviceCollection.AddSingleton<IMoonService, MoonService>();
        }
    }
}
