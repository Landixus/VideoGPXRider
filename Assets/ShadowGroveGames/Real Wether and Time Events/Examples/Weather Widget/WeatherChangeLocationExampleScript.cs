using ShadowGroveGames.RealWeatherAndTimeEvents.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace ShadowGroveGames.RealWeatherAndTimeEvents.Example
{
    public class WeatherChangeLocationExampleScript : MonoBehaviour
    {
        [SerializeField] private InputField _cityInputField;
        public Toggle toggleCelsius;  // Toggle f�r Celsius
        public Toggle toggleFahrenheit; // Toggle f�r Fahrenheit
        private const string TemperaturePrefKey = "TemperatureUnit"; // Schl�ssel f�r PlayerPrefs

        public void OnSubmit()
        {
            RealWeatherAndTimeEventsScript.Instance.ChangeLocation(_cityInputField.text);
        }

        void Start()
        {
            // Initialisieren der Toggles basierend auf gespeicherten PlayerPrefs
            if (PlayerPrefs.HasKey(TemperaturePrefKey))
            {
                string savedUnit = PlayerPrefs.GetString(TemperaturePrefKey);
                if (savedUnit == "Fahrenheit")
                {
                    toggleFahrenheit.isOn = true;
                    toggleCelsius.isOn = false;
                }
                else // Standardm��ig Celsius
                {
                    toggleCelsius.isOn = true;
                    toggleFahrenheit.isOn = false;
                }
            }
            else
            {
                // Standardm��ig Celsius, wenn keine Einstellungen vorhanden sind
                toggleCelsius.isOn = true;
                toggleFahrenheit.isOn = false;
                PlayerPrefs.SetString(TemperaturePrefKey, "Celsius");
            }

            // Hinzuf�gen der Listener f�r die Toggles
            toggleCelsius.onValueChanged.AddListener(delegate { OnToggleChanged(toggleCelsius); });
            toggleFahrenheit.onValueChanged.AddListener(delegate { OnToggleChanged(toggleFahrenheit); });
        }

        void OnToggleChanged(Toggle changedToggle)
        {
            if (changedToggle == toggleCelsius && toggleCelsius.isOn)
            {
                // Celsius aktiviert, Fahrenheit deaktivieren
                toggleFahrenheit.isOn = false;
                PlayerPrefs.SetString(TemperaturePrefKey, "Celsius");
            }
            else if (changedToggle == toggleFahrenheit && toggleFahrenheit.isOn)
            {
                // Fahrenheit aktiviert, Celsius deaktivieren
                toggleCelsius.isOn = false;
                PlayerPrefs.SetString(TemperaturePrefKey, "Fahrenheit");
            }
        }
    }
}

