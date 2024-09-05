using ShadowGroveGames.RealWeatherAndTimeEvents.Scripts.SimpleJSONLib;
using System;
using System.Globalization;

namespace ShadowGroveGames.RealWeatherAndTimeEvents.Scripts.OpenWeatherApi.DTO
{
    [Serializable]
    public class Weather
    {
        /// <summary>
        /// Weather condition id. See https://openweathermap.org/weather-conditions
        /// </summary>
        public readonly WeatherTypeId WeatherId;

        /// <summary>
        /// Group of weather parameters (Rain, Snow, Extreme etc.)
        /// </summary>
        public readonly WeatherMainType Main;

        /// <summary>
        /// Weather condition within the group
        /// </summary>
        public readonly string Description;

        /// <summary>
        /// Weather icon id. See https://openweathermap.org/weather-conditions
        /// </summary>
        public readonly string Icon;

        public Weather(JSONNode weatherInformationData, CultureInfo cultureInfo)
        {
            WeatherId = (WeatherTypeId)int.Parse(weatherInformationData["id"], cultureInfo);


            if (!Enum.TryParse(weatherInformationData["main"], out Main))
                Main = WeatherMainType.Unknown;

            Description = weatherInformationData["description"];
            Icon = weatherInformationData["icon"];
        }

        public enum WeatherTypeId
        {
            Thunderstorm_With_Light_Rain = 200,
            Thunderstorm_With_Rain = 201,
            Thunderstorm_With_Heavy_Rain = 202,
            Thunderstorm_Light = 210,
            Thunderstorm = 211,
            Thunderstorm_Heavy = 212,
            Thunderstorm_Ragged = 221,
            Thunderstorm_With_Light_Drizzle = 230,
            Thunderstorm_With_Drizzle = 231,
            Thunderstorm_With_Heavy_Drizzle = 232,
            Drizzle_Light_Intensity = 300,
            Drizzle = 301,
            Drizzle_Heavy_Intensity = 302,
            Drizzle_Light_Intensity_Drizzle_Rain = 310,
            Drizzle_Rain = 311,
            Drizzle_Heavy_Intensity_Drizzle_Rain = 312,
            Drizzle_Shower_Rain_And_Drizzle = 313,
            Drizzle_Heavy_Shower_Rain_And_Drizzle = 314,
            Drizzle_Shower = 321,
            Rain_Light = 500,
            Rain_Moderate = 501,
            Rain_Heavy_Intensity = 502,
            Rain_Very_Heavy = 503,
            Rain_Extreme = 504,
            Rain_Freezing = 511,
            Rain_Light_Intensity_Shower = 520,
            Rain_Shower = 521,
            Rain_Heavy_Intensity_Shower = 522,
            Rain_Ragged_Shower = 531,
            Snow_Light = 600,
            Snow = 601,
            Snow_Heavy = 602,
            Snow_Sleet = 611,
            Snow_Light_Shower_Sleet = 612,
            Snow_Shower_Sleet = 613,
            Snow_Light_Rain_And_Snow = 615,
            Snow_Rain_And_Snow = 616,
            Snow_Light_Shower = 620,
            Snow_Shower = 621,
            Snow_Heavy_Shower = 622,
            Mist = 701,
            Smoke = 711,
            Haze = 721,
            Dust_Sand = 731,
            Fog = 741,
            Sand = 751,
            Dust = 761,
            Ash_Volcanic = 762,
            Squalls = 771,
            Tornado = 781,
            Clear = 800,
            Clouds_Few = 801,
            Clouds_Scattered = 802,
            Clouds_Broken = 803,
            Clouds_Overcast = 804,
        }

        public enum WeatherMainType
        {
            Unknown = 0,
            Thunderstorm,
            Drizzle,
            Rain,
            Snow,
            Mist,
            Smoke,
            Haze,
            Dust,
            Fog,
            Sand,
            Ash,
            Squall,
            Tornado,
            Clear,
            Clouds
        }
    }
}
