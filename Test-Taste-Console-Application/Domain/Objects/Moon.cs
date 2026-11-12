using System;
using Test_Taste_Console_Application.Domain.DataTransferObjects;

namespace Test_Taste_Console_Application.Domain.Objects
{
    public class Moon
    {
        public string Id { get; set; }
        public float MassValue { get; set; }
        public float MassExponent { get; set; }
        public float? AvgTemp { get; set; }
        public float? AverageTemperature => AvgTemp;

        public Moon()
        {
        }

        public Moon(MoonDto moonDto)
        {
            if (moonDto == null) return;
            Id = moonDto.Id;
            MassValue = moonDto.MassValue;
            MassExponent = moonDto.MassExponent;
            AvgTemp = moonDto.AvgTemp;
        }
    }
}
