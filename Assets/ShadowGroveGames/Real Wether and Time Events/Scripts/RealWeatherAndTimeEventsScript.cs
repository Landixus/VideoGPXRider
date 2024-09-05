using ShadowGroveGames.RealWeatherAndTimeEvents.Scripts.Control;
using ShadowGroveGames.RealWeatherAndTimeEvents.Scripts.OpenWeatherApi;
using ShadowGroveGames.RealWeatherAndTimeEvents.Scripts.OpenWeatherApi.DTO;
using System;
using System.Threading.Tasks;
using UnityEngine;
using static ShadowGroveGames.RealWeatherAndTimeEvents.Scripts.OpenWeatherApi.OpenWeatherApiClient;

namespace ShadowGroveGames.RealWeatherAndTimeEvents.Scripts
{
    public class RealWeatherAndTimeEventsScript : MonoBehaviour
    {
        public static RealWeatherAndTimeEventsScript Instance = null;

        [Header("API Configruation")]
        /// <summary>
        /// API Key from https://home.openweathermap.org/api_keys
        /// </summary>
        [Tooltip("API Key from\nhttps://home.openweathermap.org/api_keys")]
        [SerializeField] private string _apiKey;

        /// <summary>
        /// Translation is applied for the city name and description fields. A detailed list of all langauge codes: https://openweathermap.org/current#data
        /// </summary>
        [Tooltip("Translation is applied for the city name and description fields.\nA detailed list of all langauge codes: https://openweathermap.org/current#data")]
        [SerializeField] private LanguageKey _language = LanguageKey.EN;

        [Header("Location configuration")]
        /// <summary>
        /// City name with country 
        /// Example: Amsterdam, NL
        /// </summary>
        [Tooltip("City name with country\nExample: Amsterdam, NL")]
        [SerializeField] private string _cityName;

        [Header("Interval configuration")]
        /// <summary>
        /// Refresh weather data automatically.
        /// As an alternative you can call the function UpdateWeatherInformation
        /// </summary>
        [Tooltip("Refresh weather data automatically.\nAs an alternative you can call the function UpdateWeatherInformation")]
        [SerializeField] private bool _automaticallyRefreshWeatherData = true;

        /// <summary>
        /// Weather Data update interval in secounds.
        /// Keep in mind that the free plan allow per account only 60 requests per secound!
        /// </summary>
        [Min(5)]
        [Tooltip("Weather Data update interval in secounds.\nKeep in mind that the free plan allow per account only 60 requests per secound!\nMinimum 5 secounds")]
        [SerializeField] private int _updateIntervalInSecounds = 10;

        /// <summary>
        /// Contains the real weather and time events
        /// </summary>
        [Tooltip("Contains the real weather and time events")]
        [SerializeField] private RealWeatherAndTimeEventsEvents _events;

        /// <summary>
        /// Weather information (updated automaticlly)
        /// </summary>
        public WeatherInformation WeatherInformation { get; private set; }

        /// <summary>
        /// OpenWeatherApi client
        /// </summary>
        private OpenWeatherApiClient _apiClient = null;

        /// <summary>
        /// Threshold for next update tick
        /// </summary>
        private float _nextUpdateInterval = 0;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("You can have only one instance of RealWeatherAndTimeEventsScript!");
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
            Instance = this;
        }

        // Start is called before the first frame update
        void OnEnable()
        {
            _apiKey = _apiKey.Trim();

            if (string.IsNullOrEmpty(_apiKey))
            {
                Debug.LogError("Missing OpenWeatherMap API Client. Get one from https://home.openweathermap.org/api_keys");
                return;
            }

            _apiClient = new OpenWeatherApiClient(_apiKey);
        }

        // Update is called once per frame
        async void Update()
        {
            if (_apiClient == null)
                return;

            if (!_automaticallyRefreshWeatherData)
                return;

            if (_nextUpdateInterval <= 0)
            {
                _nextUpdateInterval = _updateIntervalInSecounds;
                await UpdateWeatherInformation();
            }

            _nextUpdateInterval -= Time.deltaTime;
        }

        public async Task UpdateWeatherInformation()
        {
            try
            {
                var weatherInformation = await _apiClient.QueryAsync(_cityName, _language);

                // Call ready event
                if (WeatherInformation == null)
                    _events.OnReady?.Invoke(weatherInformation);

                WeatherInformation = weatherInformation;

                _events.OnUpdate?.Invoke(weatherInformation);
            }
            catch (Exception exception)
            {
                _events.OnFailed?.Invoke();
                Debug.LogError("Cant connect to OpenWeatherMap API, check your api key and internet connection\n" + exception.Message);
                return;
            }
        }

        public async Task<bool> ChangeLocation(string cityName)
        {
            try
            {
                WeatherInformation = await _apiClient.QueryAsync(cityName, _language);
                _cityName = cityName;
                _events.OnUpdate?.Invoke(WeatherInformation);

                return true;
            }
            catch (Exception exception)
            {
                Debug.LogWarning("Cant find given city!\n" + exception.Message);
            }

            return false;
        }
    }
}
