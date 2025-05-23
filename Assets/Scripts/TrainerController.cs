using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro für das UI-Textfeld

public class TrainerController : MonoBehaviour
{
    public FitnessEquipmentDisplay trainer; // Ziehe dein FitnessEquipmentDisplay-Objekt hier rein
    public HeartRateDisplay heartRateDisplay;
    public Slider resistanceSlider;
    public Button ergModeButton;
    public Button simulationModeButton;
    public Button freeRideButton;
    public TMP_Text statusText; // Textfeld zur Anzeige der Werte

    private int resistanceLevel; // Widerstand in Prozent
    private int trainerMode; // 0 = Simulationsmodus, 1 = ERG-Modus

    private Color activeColor = Color.green;
    private Color inactiveColor = Color.red;
    public Button refreshStatusButton;

    void Start()
    {
        trainer = Object.FindFirstObjectByType<FitnessEquipmentDisplay>();
        heartRateDisplay = Object.FindFirstObjectByType<HeartRateDisplay>();

        LoadSettings();
        UpdateUI();
        // Standard-Status anzeigen
        if (statusText != null)
        {
            statusText.text = "Warte auf Trainerdaten...";
        }

        resistanceSlider.wholeNumbers = true; // Stellt sicher, dass nur ganze Zahlen verwendet werden
        resistanceSlider.onValueChanged.AddListener(delegate { SnapSliderValue(); });

        ergModeButton.onClick.AddListener(SetErgMode);
        simulationModeButton.onClick.AddListener(SetSimulationMode);
        freeRideButton.onClick.AddListener(SetFreeRideMode);

        CleanupUnconnectedDevices();

        RequestTrainerStatus();
    }



    void SnapSliderValue()
    {
        int newValue = Mathf.RoundToInt(resistanceSlider.value / 25f) * 25; // Rundet auf 25er-Schritte
        resistanceSlider.value = newValue;
        SetResistance(newValue);
    }
    void SetResistance(int value)
    {
        resistanceLevel = value;
        if (trainer != null)
        {
            trainer.SetTrainerResistance(resistanceLevel);
            SaveSettings();
        }
    }

    void SetErgMode()
    {
        if (trainerMode == 1) return; // Falls bereits aktiv, nichts tun

        trainerMode = 1;
        if (trainer != null)
        {
            trainer.SetTrainerTargetPower(150); // Beispiel: 150 W   // this is for a Workout that we want load from a ZWO XML File
            trainer.SetTrainerSlope(0); // Simulationsmodus deaktivieren
            SaveSettings();
        }
        UpdateUI();
    }

    void SetSimulationMode()
    {
        if (trainerMode == 0) return; // Falls bereits aktiv, nichts tun

        trainerMode = 0;
        if (trainer != null)
        {
            trainer.SetTrainerSlope(1); // 1% Steigung setzen, damit Simulation aktiv ist
            trainer.SetTrainerTargetPower(0); // ERG-Modus deaktivieren
            SaveSettings();
        }
        UpdateUI();
    }

    void SetFreeRideMode() // Neuer Modus für freies Fahren
    {
        trainerMode = 2;
        if (trainer != null)
        {
            trainer.SetTrainerSlope(0); // Kein Einfluss auf Widerstand
            trainer.SetTrainerTargetPower(0); // Kein ERG-Modus
            SaveSettings();
        }
        UpdateUI();
    }
    void SaveSettings()
    {
        PlayerPrefs.SetInt("TrainerResistance", resistanceLevel);
        PlayerPrefs.SetInt("TrainerMode", trainerMode);
        PlayerPrefs.Save();
    }

    void LoadSettings()
    {
        resistanceLevel = PlayerPrefs.GetInt("TrainerResistance", 25);
        trainerMode = PlayerPrefs.GetInt("TrainerMode", 0);

        resistanceSlider.value = resistanceLevel;

        if (trainer != null)
        {
            trainer.SetTrainerResistance(resistanceLevel);
            if (trainerMode == 1)
                trainer.SetTrainerTargetPower(150);
            if (trainerMode == 2)
                trainer.SetTrainerTargetPower(0);
            else
                trainer.SetTrainerSlope(0);
        }
        UpdateUI();
    }

    void UpdateUI()
    {
        // Button-Farben updaten
        SetButtonColor(ergModeButton, trainerMode == 1);
        SetButtonColor(simulationModeButton, trainerMode == 0);
        SetButtonColor(freeRideButton, trainerMode == 2);
    }

    void SetButtonColor(Button button, bool isActive)
    {
        ColorBlock cb = button.colors;
        cb.normalColor = isActive ? activeColor : inactiveColor;
        cb.highlightedColor = isActive ? activeColor : inactiveColor;
        cb.pressedColor = isActive ? activeColor : inactiveColor;
        cb.selectedColor = isActive ? activeColor : inactiveColor;
        button.colors = cb;
    }

    // Diese Methode wird aufgerufen, wenn ein neuer Status empfangen wird
    public void UpdateTrainerStatus(CommandStatus status)
    {
        string statusTextContent = "";

        if (status.lastReceivedCommandId == 51) // Simulationsmodus (Slope)
        {
            int rawSlopeValue = (status.byte_5) | (status.byte_6 << 8);
            float slope = (rawSlopeValue - 20000) / 100f;
            statusTextContent = $"Simulationsmodus aktiv\nSteigung: {slope}%";
        }
        else if (status.lastReceivedCommandId == 48) // Widerstandsmodus
        {
            int resistance = status.byte_7;
            statusTextContent = $"Widerstandsmodus aktiv\nWiderstand: {resistance / 2}%";
        }
        else if (status.lastReceivedCommandId == 49) // ERG-Modus (Zielwatt)
        {
            int power = (status.byte_6) | (status.byte_7 << 8);
            statusTextContent = $"ERG-Modus aktiv\nZielleistung: {power / 4} W";
        }

        if (statusText != null)
        {
            statusText.text = statusTextContent;
        }
    }

    void RequestTrainerStatus()
    {
        if (trainer != null)
        {
            trainer.RequestCommandStatus();
        }
    }

    private void CleanupUnconnectedDevices()
    {
        if (trainer != null && !trainer.connected)
        {
            Debug.Log("[SceneController] FEC Trainer not connected ➔ destroy.");
            Destroy(trainer.gameObject);
        }
       
        if (heartRateDisplay != null && !heartRateDisplay.connected)
        {
            Debug.Log("[SceneController] HeartRateANTClient not connected ➔ destroy.");
            Destroy(heartRateDisplay.gameObject);
          //  Debug.Log("[TrainerController] Nicht verbunden ➔ zerstöre mich selbst.");
         //   Destroy(gameObject);
        }
    }
}
