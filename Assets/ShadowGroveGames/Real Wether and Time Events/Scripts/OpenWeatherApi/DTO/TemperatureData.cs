using System;

namespace ShadowGroveGames.RealWeatherAndTimeEvents.Scripts.OpenWeatherApi.DTO
{
    [Serializable]
    public class TemperatureData
    {
        /// <summary>
        /// Current temperature in celsius
        /// </summary>
        public readonly double CelsiusCurrent;

        /// <summary>
        /// Current temperature in fahrenheit
        /// </summary>
        public readonly double FahrenheitCurrent;

        /// <summary>
        /// Current temperature in Kelvin
        /// </summary>
        public readonly double KelvinCurrent;

        /// <summary>
        /// This celsius temperature parameter accounts for the human perception of weather. 
        /// </summary>
        public readonly double CelsiusFeelLike;

        /// <summary>
        /// This fahrenheit temperature parameter accounts for the human perception of weather. 
        /// </summary>
        public readonly double FahrenheitFeelLike;

        /// <summary>
        /// This kelvin temperature parameter accounts for the human perception of weather. 
        /// </summary>
        public readonly double KelvinFeelLike;

        /// <summary>
        /// Current minimum celsius temperature. This is the lowest temperature currently observed.
        /// </summary>
        public readonly double CelsiusMinimum;

        /// <summary>
        /// The current maximum celsius temperature. This is the maximum temperature currently observed
        /// </summary>
        public readonly double CelsiusMaximum;

        /// <summary>
        /// Current minimum fahrenheit temperature. This is the lowest temperature currently observed.
        /// </summary>
        public readonly double FahrenheitMinimum;

        /// <summary>
        /// The current maximum fahrenheit temperature. This is the maximum temperature currently observed
        /// </summary>
        public readonly double FahrenheitMaximum;

        /// <summary>
        /// Current minimum kelvin temperature. This is the lowest temperature currently observed.
        /// </summary>
        public readonly double KelvinMinimum;

        /// <summary>
        /// The current maximum kelvin temperature. This is the maximum temperature currently observed
        /// </summary>
        public readonly double KelvinMaximum;

        public TemperatureData(double temperature, double feelTemprature, double min, double max)
        {
            KelvinCurrent = temperature;
            KelvinFeelLike = feelTemprature;
            KelvinMaximum = max;
            KelvinMinimum = min;

            CelsiusCurrent = ConvertToCelsius(KelvinCurrent);
            CelsiusFeelLike = ConvertToCelsius(KelvinFeelLike);
            CelsiusMaximum = ConvertToCelsius(KelvinMaximum);
            CelsiusMinimum = ConvertToCelsius(KelvinMinimum);

            FahrenheitCurrent = ConvertToFahrenheit(CelsiusCurrent);
            FahrenheitFeelLike = ConvertToFahrenheit(CelsiusFeelLike);
            FahrenheitMaximum = ConvertToFahrenheit(CelsiusMaximum);
            FahrenheitMinimum = ConvertToFahrenheit(CelsiusMinimum);
        }

        private static double ConvertToFahrenheit(double celsius)
        {
            return Math.Round(((9.0 / 5.0) * celsius) + 32, 3);
        }

        private static double ConvertToCelsius(double kelvin)
        {
            return Math.Round(kelvin - 273.15, 3);
        }
    }
}
