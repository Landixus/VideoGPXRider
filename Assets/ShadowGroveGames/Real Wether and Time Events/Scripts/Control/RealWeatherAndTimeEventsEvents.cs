using ShadowGroveGames.RealWeatherAndTimeEvents.Scripts.OpenWeatherApi.DTO;
using System;
using UnityEngine.Events;

namespace ShadowGroveGames.RealWeatherAndTimeEvents.Scripts.Control
{
    [Serializable]
    public class RealWeatherAndTimeEventsEvents
    {

        [Serializable]
        public class OnReadyEvent : UnityEvent<WeatherInformation> { }

        [Serializable]
        public class OnUpdateEvent : UnityEvent<WeatherInformation> { }

        [Serializable]
        public class OnFailedEvent : UnityEvent { }

        public OnReadyEvent OnReady = new OnReadyEvent();
        public OnUpdateEvent OnUpdate = new OnUpdateEvent();
        public OnFailedEvent OnFailed = new OnFailedEvent();
    }
}