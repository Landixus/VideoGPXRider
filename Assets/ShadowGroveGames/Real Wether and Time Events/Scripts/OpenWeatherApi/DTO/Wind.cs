using ShadowGroveGames.RealWeatherAndTimeEvents.Scripts.SimpleJSONLib;
using System;
using System.Globalization;

namespace ShadowGroveGames.RealWeatherAndTimeEvents.Scripts.OpenWeatherApi.DTO
{
    [Serializable]
    public class Wind
    {
        /// <summary>
        /// Wind speed in meters per hour
        /// </summary>
        public readonly double SpeedMetersPerHour;

        /// <summary>
        /// Wind speed in feets per hour
        /// </summary>
        public readonly double SpeedFeetPerHour;

        /// <summary>
        /// Wind direction see <c>WindDirection</c>
        /// </summary>
        public readonly WindDirection Direction;

        /// <summary>
        /// Wind direction in degrees 
        /// </summary>
        public readonly double Degree;

        /// <summary>
        /// Wind gust in meter per sec
        /// </summary>
        public readonly double Gust;

        public Wind(JSONNode windInformationData, CultureInfo cultureInfo)
        {
            SpeedMetersPerHour = double.Parse(windInformationData["speed"], CultureInfo.InvariantCulture);
            SpeedFeetPerHour = SpeedMetersPerHour * 3.28084;
            Degree = double.Parse(windInformationData["deg"], CultureInfo.InvariantCulture);
            Direction = parseDirectionFromDegree(Degree);

            if (windInformationData.HasKey("gust"))
                Gust = double.Parse(windInformationData["gust"], CultureInfo.InvariantCulture);
        }

        private WindDirection parseDirectionFromDegree(double degree)
        {
            if (fallsBetween(degree, 348.75, 360))
                return WindDirection.North;

            if (fallsBetween(degree, 0, 11.25))
                return WindDirection.North;

            if (fallsBetween(degree, 11.25, 33.75))
                return WindDirection.North_North_East;

            if (fallsBetween(degree, 33.75, 56.25))
                return WindDirection.North_East;

            if (fallsBetween(degree, 56.25, 78.75))
                return WindDirection.East_North_East;

            if (fallsBetween(degree, 78.75, 101.25))
                return WindDirection.East;

            if (fallsBetween(degree, 101.25, 123.75))
                return WindDirection.East_South_East;

            if (fallsBetween(degree, 123.75, 146.25))
                return WindDirection.South_East;

            if (fallsBetween(degree, 168.75, 191.25))
                return WindDirection.South;

            if (fallsBetween(degree, 191.25, 213.75))
                return WindDirection.South_South_West;

            if (fallsBetween(degree, 213.75, 236.25))
                return WindDirection.South_West;

            if (fallsBetween(degree, 236.25, 258.75))
                return WindDirection.West_South_West;

            if (fallsBetween(degree, 258.75, 281.25))
                return WindDirection.West;

            if (fallsBetween(degree, 281.25, 303.75))
                return WindDirection.West_North_West;

            if (fallsBetween(degree, 303.75, 326.25))
                return WindDirection.North_West;

            if (fallsBetween(degree, 326.25, 348.75))
                return WindDirection.North_North_West;

            return WindDirection.Unknown;
        }

        private bool fallsBetween(double val, double min, double max)
        {
            if ((min <= val) && (val <= max))
                return true;

            return false;
        }

        public static string WindDirectionToString(WindDirection dir)
        {
            switch (dir)
            {
                case WindDirection.East:
                    return "East";

                case WindDirection.East_North_East:
                    return "East North-East";

                case WindDirection.East_South_East:
                    return "East South-East";

                case WindDirection.North:
                    return "North";

                case WindDirection.North_East:
                    return "North East";

                case WindDirection.North_North_East:
                    return "North North-East";

                case WindDirection.North_North_West:
                    return "North North-West";

                case WindDirection.North_West:
                    return "North West";

                case WindDirection.South:
                    return "South";

                case WindDirection.South_East:
                    return "South East";

                case WindDirection.South_South_East:
                    return "South South-East";

                case WindDirection.South_South_West:
                    return "South South-West";

                case WindDirection.South_West:
                    return "South West";

                case WindDirection.West:
                    return "West";

                case WindDirection.West_North_West:
                    return "West North-West";

                case WindDirection.West_South_West:
                    return "West South-West";

                default:
                    return "Unknown";
            }
        }

        public enum WindDirection
        {
            North,
            North_North_East,
            North_East,
            East_North_East,
            East,
            East_South_East,
            South_East,
            South_South_East,
            South,
            South_South_West,
            South_West,
            West_South_West,
            West,
            West_North_West,
            North_West,
            North_North_West,
            Unknown
        }
    }
}
