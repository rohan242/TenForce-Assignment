# Test-Taste-Console-Application

## Overview

This is a C# .NET 5 console application that retrieves data from the Solar System OpenData API and displays information about planets and moons.

The application currently supports the following operations:

- Display all planets along with their associated moons.
- Display all moons and their mass information.
- Display the average moon gravity for each planet.
- Display planets that have at least one moon along with the average temperature of their moons.
- Export the average moon temperature results to a CSV file.

## New Functionality

The application has been extended to calculate and display the average temperature of moons belonging to each planet.

For this functionality, the application:

1. Retrieves the average temperature (`avgTemp`) of each moon from the API.
2. Associates each moon with its corresponding planet.
3. Selects planets that have at least one moon.
4. Calculates the average temperature using the available moon temperature values.
5. Displays the results in the console.
6. Exports the results to a CSV file.

Temperatures are kept in Kelvin, as provided by the API.

## Average Temperature Calculation

The average temperature is calculated using the following formula:

Average Temperature = Sum of valid moon temperatures / Number of valid moon temperatures

Only moon temperatures greater than zero are included in the calculation.

The following cases are handled:

- Planets without moons are excluded from the results.
- Moons with missing or null temperature values are skipped.
- A temperature of `0 K` is treated as unavailable data.
- If none of a planet's moons have a valid temperature, the average is returned as `null`.
- Division by zero is avoided.

When no valid average temperature is available, the output displays `-`.

## Implementation Details

### 1. Domain Models

**Moon.cs**

- Added an `AvgTemp` property to store the moon's average temperature.
- Updated the existing constructor to map temperature information from `MoonDto`.
- Used a nullable `float` to handle missing temperature values.

**Planet.cs**

- Added the `AverageMoonTemperature` property.
- Implemented the calculation using LINQ.
- Reused the existing moon collection and `HasMoons()` method.
- Preserved the existing `AverageMoonGravity` functionality.

### 2. API Data Mapping

**MoonDto.cs**

- Added the `avgTemp` property mapping using `JsonProperty`.
- Used a nullable data type to handle missing values.
- Kept the existing mass and other property mappings unchanged.

**PlanetService.cs**

- Added HTTP response status checks when retrieving individual moon details.
- Added null checks for deserialized moon responses.
- Preserved the existing planet and moon retrieval flow.

### 3. Console Output

**ScreenOutputService.cs**

Added `OutputAllPlanetsAndTheirAverageMoonTemperatureToConsole()` to display a formatted table containing:

- Planet ID
- Average moon temperature in Kelvin

The method also handles cases where no planets or planets with moons are found.

### 4. CSV File Output

Added a separate file output service:

- `IFileOutputService.cs`
- `FileOutputService.cs`

The new functionality exports the results to:

`FileOutput/AllPlanetsAndTheirAverageMoonTemperature.csv`

The output directory is created automatically if it does not already exist.

The CSV contains the planet ID and its average moon temperature.

### 5. Application Configuration

Updated `Program.cs` to register the file output service through Dependency Injection and execute the new console and file output operations.

Added execution messages to make the different processing steps clear while the application runs.

## Project Structure

The application follows a simple separation of responsibilities:

- **Constants:** Application messages and file-related constants.
- **Domain/DataTransferObjects:** API response models.
- **Domain/Objects:** Planet and Moon domain models.
- **Domain/Services:** API communication and output operations.
- **Program.cs:** Application startup, dependency registration, and execution.

## How to Run

### Prerequisites

- .NET SDK compatible with the `net5.0` target framework.
- Internet connection for API requests.

### Build the application

```bash
dotnet build
```

### Run the application

```bash
dotnet run
```

The application displays the results in the console and saves the new average moon temperature report in the `FileOutput` directory.

## Known Limitations

- **API Authentication:** The Solar System OpenData API may return HTTP 401 Unauthorized responses without a valid Bearer token. Authentication has intentionally not been implemented, as it is outside the scope of this exercise.
- **.NET Version:** The project currently targets .NET 5, which is out of support. Building with a newer SDK may display an end-of-support warning.

## Notes

The existing application structure and functionality have been retained wherever possible. Changes were kept focused on the requested average moon temperature functionality and the required output handling.