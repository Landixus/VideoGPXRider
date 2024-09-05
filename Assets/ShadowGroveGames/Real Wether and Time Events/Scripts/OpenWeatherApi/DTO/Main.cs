using ShadowGroveGames.RealWeatherAndTimeEvents.Scripts.SimpleJSONLib;
using System;
using System.Globalization;

namespace ShadowGroveGames.RealWeatherAndTimeEvents.Scripts.OpenWeatherApi.DTO
{
    [Serializable]
    public class Main
    {
        /// <summary>
        /// Temprature data in diffrent units 
        /// </summary>
        public readonly TemperatureData Temperature;

        /// <summary>
        /// Humidity in %
        /// </summary>
        public readonly double Humidity;

        /// <summary>
        /// Atmospheric pressure in hPa
        /// </summary>
        public readonly double Pressure;

        /// <summary>
        /// Atmospheric pressure in hPa on the sea level
        /// </summary>
        public readonly double PressureOnSeaLevel;

        /// <summary>
        /// Atmospheric pressure in hPa on the ground level
        /// </summary>
        public readonly double PressureOnGroundLevel;

        public Main(JSONNode mainInformationData, CultureInfo cultureInfo)
        {
            Temperature = new TemperatureData(
                double.Parse(mainInformationData["temp"], cultureInfo),
                double.Parse(mainInformationData["feels_like"], cultureInfo),
                double.Parse(mainInformationData["temp_min"], cultureInfo),
                double.Parse(mainInformationData["temp_max"], cultureInfo)
            );

            Humidity = double.Parse(mainInformationData["humidity"], cultureInfo);
            Pressure = double.Parse(mainInformationData["pressure"], cultureInfo);

            if (mainInformationData.HasKey("sea_level"))
                PressureOnSeaLevel = double.Parse(mainInformationData["sea_level"], cultureInfo);

            if (mainInformationData.HasKey("grnd_level"))
                PressureOnGroundLevel = double.Parse(mainInformationData["grnd_level"], cultureInfo);
        }
    }
}
