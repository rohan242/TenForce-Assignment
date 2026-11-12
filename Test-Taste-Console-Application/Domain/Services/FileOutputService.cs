using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Test_Taste_Console_Application.Constants;
using Test_Taste_Console_Application.Domain.Services.Interfaces;
using Test_Taste_Console_Application.Utilities;

namespace Test_Taste_Console_Application.Domain.Services
{
    /// <inheritdoc />
    public class FileOutputService : IFileOutputService
    {
        private readonly IPlanetService _planetService;
        private readonly IMoonService _moonService;

        public FileOutputService(IPlanetService planetService, IMoonService moonService)
        {
            _planetService = planetService;
            _moonService = moonService;
        }

        public bool OutputAllPlanetsAndTheirAverageMoonTemperatureToFile()
        {
            var planets = _planetService.GetAllPlanets().ToArray();
            if (!planets.Any())
            {
                Console.WriteLine(OutputString.NoPlanetsFound);
                return false;
            }

            var planetsWithMoons = planets.Where(planet => planet.HasMoons()).ToArray();
            if (!planetsWithMoons.Any())
            {
                Console.WriteLine(OutputString.NoPlanetsWithMoonsFound);
                return false;
            }

            try
            {
                EnsureOutputFolderExists();

                var filePath = Path.Combine(PathName.PathToOutputFolder, PathName.AllPlanetsAndTheirAverageMoonTemperatureFile);

                using (var writer = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    writer.WriteLine($"{OutputString.PlanetId},{OutputString.PlanetMoonAverageTemperature}");

                    foreach (var planet in planetsWithMoons)
                    {
                        var averageTemperature = planet.AverageMoonTemperature;

                        var averageTemperatureDisplay = averageTemperature.HasValue
                            ? averageTemperature.Value.ToString(CultureInfo.InvariantCulture)
                            : "-";

                        writer.WriteLine($"{CultureInfoUtility.TextInfo.ToTitleCase(planet.Id)},{averageTemperatureDisplay}");
                    }
                }

                Logger.Instance.Info($"{OutputString.FileCreated}{filePath}");
                Console.WriteLine($"{OutputString.FileCreated}{filePath}");
                return true;
            }
            catch (Exception exception)
            {
                Logger.Instance.Error($"{LoggerMessage.FileOutputOperationFailed}{exception.Message}");
                Console.WriteLine($"{ExceptionMessage.FileOutputOperationFailed}{exception.Message}");
                return false;
            }
        }

        public bool OutputAllPlanetsAndTheirMoonsToFile()
        {
            var planets = _planetService.GetAllPlanets().ToArray();
            if (!planets.Any())
            {
                Console.WriteLine(OutputString.NoPlanetsFound);
                return false;
            }

            try
            {
                EnsureOutputFolderExists();

                var filePath = Path.Combine(PathName.PathToOutputFolder, PathName.AllPlanetsAndTheirMoonsFile);

                using (var writer = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    writer.WriteLine($"{OutputString.PlanetId},{OutputString.PlanetSemiMajorAxis},{OutputString.TotalMoons},{OutputString.MoonId}");

                    foreach (var planet in planets)
                    {
                        if (planet.HasMoons())
                        {
                            foreach (var moon in planet.Moons)
                            {
                                writer.WriteLine($"{CultureInfoUtility.TextInfo.ToTitleCase(planet.Id)},{planet.SemiMajorAxis},{planet.Moons.Count},{CultureInfoUtility.TextInfo.ToTitleCase(moon.Id)}");
                            }
                        }
                        else
                        {
                            writer.WriteLine($"{CultureInfoUtility.TextInfo.ToTitleCase(planet.Id)},{planet.SemiMajorAxis},0,-");
                        }
                    }
                }

                Logger.Instance.Info($"{OutputString.FileCreated}{filePath}");
                Console.WriteLine($"{OutputString.FileCreated}{filePath}");
                return true;
            }
            catch (Exception exception)
            {
                Logger.Instance.Error($"{LoggerMessage.FileOutputOperationFailed}{exception.Message}");
                Console.WriteLine($"{ExceptionMessage.FileOutputOperationFailed}{exception.Message}");
                return false;
            }
        }

        public bool OutputAllMoonsAndTheirMassToFile()
        {
            var moons = _moonService.GetAllMoons().ToArray();
            if (!moons.Any())
            {
                Console.WriteLine(OutputString.NoMoonsFound);
                return false;
            }

            try
            {
                EnsureOutputFolderExists();

                var filePath = Path.Combine(PathName.PathToOutputFolder, PathName.AllMoonsAndTheirMassFile);

                using (var writer = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    writer.WriteLine($"{OutputString.MoonId},{OutputString.MoonMassExponent},{OutputString.MoonMassValue}");

                    foreach (var moon in moons)
                    {
                        writer.WriteLine($"{CultureInfoUtility.TextInfo.ToTitleCase(moon.Id)},{moon.MassExponent},{moon.MassValue.ToString(CultureInfo.InvariantCulture)}");
                    }
                }

                Logger.Instance.Info($"{OutputString.FileCreated}{filePath}");
                Console.WriteLine($"{OutputString.FileCreated}{filePath}");
                return true;
            }
            catch (Exception exception)
            {
                Logger.Instance.Error($"{LoggerMessage.FileOutputOperationFailed}{exception.Message}");
                Console.WriteLine($"{ExceptionMessage.FileOutputOperationFailed}{exception.Message}");
                return false;
            }
        }

        private static void EnsureOutputFolderExists()
        {
            if (!Directory.Exists(PathName.PathToOutputFolder))
            {
                Directory.CreateDirectory(PathName.PathToOutputFolder);
                Logger.Instance.Info($"{OutputString.FolderCreated}{PathName.PathToOutputFolder}");
                Console.WriteLine($"{OutputString.FolderCreated}{PathName.PathToOutputFolder}");
            }
        }
    }
}
