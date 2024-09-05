using ShadowGroveGames.RealWeatherAndTimeEvents.Scripts.SimpleJSONLib;
using System;
using System.Globalization;

namespace ShadowGroveGames.RealWeatherAndTimeEvents.Scripts.OpenWeatherApi.DTO
{
    [Serializable]
    public class Coordinates
    {
        /// <summary>
        /// City geo location, longitude 
        /// </summary>
        public readonly double Longitude;

        /// <summary>
        /// City geo location, longitude 
        /// </summary>
        public readonly double Latitude;

        public Coordinates(JSONNode coordinatInformationData, CultureInfo cultureInfo)
        {
            Longitude = double.Parse(coordinatInformationData["lon"], cultureInfo);
            Latitude = double.Parse(coordinatInformationData["lat"], cultureInfo);
        }
    }
}
