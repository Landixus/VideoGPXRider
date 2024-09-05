using ShadowGroveGames.RealWeatherAndTimeEvents.Scripts.Helper;
using ShadowGroveGames.RealWeatherAndTimeEvents.Scripts.SimpleJSONLib;
using System;
using System.Globalization;

namespace ShadowGroveGames.RealWeatherAndTimeEvents.Scripts.OpenWeatherApi.DTO
{
    [Serializable]
    public class WeatherInformation
    {
        public const int REQUEST_STATUS_SUCCESS = 200;

        /// <summary>
        /// City geo location
        /// </summary>
        public readonly Coordinates Coordinates;

        /// <summary>
        /// Current weather
        /// </summary>
#nullable enable
        public readonly Weather? Weather = null;
#nullable disable

        /// <summary>
        /// Base station type
        /// </summary>
        public readonly string BasestationType;

        /// <summary>
        /// Main information
        /// </summary>
        public readonly Main Main;

        /// <summary>
        /// Visibility in meter. The maximum value of the visibility is 10km
        /// </summary>
        public readonly double? Visibility;

        /// <summary>
        /// Wind information
        /// </summary>
        public readonly Wind Wind;

        /// <summary>
        /// Cloudiness in %
        /// </summary>
        public readonly double Cloudiness;

        /// <summary>
        /// Shift in seconds from UTC
        /// </summary>
        public readonly int TimeZoneUTCOffset;

        /// <summary>
        /// Current local datetime
        /// </summary>
        public readonly DateTime DateTime;

        /// <summary>
        /// General information
        /// </summary>
        public readonly General General;

        /// <summary>
        /// City id
        /// </summary>
        public readonly int CityId;

        /// <summary>
        /// City name
        /// </summary>
        public readonly string CityName;

        /// <summary>
        /// Request status code
        /// </summary>
        public readonly int RequestStatus;

        public WeatherInformation(JSONNode weatherInformationData)
        {
            try
            {
                RequestStatus = (int)(weatherInformationData["cod"] ?? 0);

                if (RequestStatus != REQUEST_STATUS_SUCCESS)
                    throw new InvalidOperationException($"Recive unkwon request status code {RequestStatus}!");

                var apiCultureInfo = CultureInfo.GetCultureInfo("en-US");

                Coordinates = new Coordinates(weatherInformationData["coord"], apiCultureInfo);

                if (weatherInformationData.HasKey("weather"))
                {
                    foreach (JSONNode weather in weatherInformationData["weather"])
                    {
                        Weather = new Weather(weather, apiCultureInfo);
                        break;
                    }
                }

                BasestationType = weatherInformationData["base"];

                Main = new Main(weatherInformationData["main"], apiCultureInfo);

                if (weatherInformationData.HasKey("visibility"))
                    Visibility = double.Parse(weatherInformationData["visibility"], apiCultureInfo);

                Wind = new Wind(weatherInformationData["wind"], apiCultureInfo);
                Cloudiness = double.Parse(weatherInformationData["clouds"]["all"], apiCultureInfo);
                TimeZoneUTCOffset = int.Parse(weatherInformationData["timezone"], apiCultureInfo);
                DateTime = DateTimeHelper.ConvertUnixToDateTime(int.Parse(weatherInformationData["dt"], apiCultureInfo));
                General = new General(weatherInformationData["sys"], apiCultureInfo);
                CityId = int.Parse(weatherInformationData["id"], apiCultureInfo);
                CityName = weatherInformationData["name"];
            }
            catch
            {
                throw new InvalidCastException("Cant read weather information from API");
            }
        }
    }
}
