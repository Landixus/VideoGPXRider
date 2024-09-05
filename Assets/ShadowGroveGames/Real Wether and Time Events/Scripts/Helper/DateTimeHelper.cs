using System;

namespace ShadowGroveGames.RealWeatherAndTimeEvents.Scripts.Helper
{
    public static class DateTimeHelper
    {
        public static DateTime ConvertUnixToDateTime(double unixTime)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            return dateTime.AddSeconds(unixTime).ToLocalTime();
        }
    }
}