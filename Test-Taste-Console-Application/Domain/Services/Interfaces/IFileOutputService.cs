namespace Test_Taste_Console_Application.Domain.Services.Interfaces
{
    /// <summary>
    /// An output service that can write data from the Solar System OpenData API to files.
    /// </summary>
    public interface IFileOutputService
    {
        bool OutputAllPlanetsAndTheirAverageMoonTemperatureToFile();
        bool OutputAllPlanetsAndTheirMoonsToFile();
        bool OutputAllMoonsAndTheirMassToFile();
    }
}
