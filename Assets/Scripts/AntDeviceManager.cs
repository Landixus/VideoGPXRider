using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using ANT_Managed_Library;

public class AntDeviceManager : MonoBehaviour
{
    // UI-Elemente für ANT⁺
    public Transform antDeviceListPanel;
    public GameObject antDeviceButtonPrefab;

    // Verwaltung der Buttons und Geräte
    private Dictionary<int, Button> antDeviceButtons = new();
    private AntDevice pairedFitnessDevice;
    private AntDevice pairedHeartRateDevice;

    public GameObject hr_prefab1;
    public GameObject fec_prefab2;

    public GameObject warningTextObject;  //Where is your ANT+ Stick?

    public GameObject connectionPanel; // Zuweisung im Inspector
    public TMP_Text connectionText;     // Zuweisung im Inspector
    public float panelDisplayTime = 2f; // Sekunden sichtbar
    public float fadeDuration = 1f;      // Sekunden zum Ausfaden

    private List<string> connectedDevicesText = new();
    private Coroutine fadeCoroutine;
    /*
    [SerializeField] private GameObject BTManager;
    [SerializeField] private GameObject PairBTManager;
    [SerializeField] private GameObject HR_GATT;
    [SerializeField] private GameObject KICKR_GATT;
   */
    [SerializeField] private TMP_InputField weightInput;
    [SerializeField] private TMP_InputField ftpInput;
    [SerializeField] private TMP_InputField maxHRInput;

    void Start()
    {
        CheckAntStick();
        LoadPairedDevices();
        LoadUserSettingsToInputs();
        // StartCoroutine(ScanForAntDevices());
        /*  BTManager = GameObject.Find("BTManager");
          PairBTManager = GameObject.Find("PairBTManager");
          HR_GATT = GameObject.Find("HR_GATT");
          KICKR_GATT = GameObject.Find("KICKR_GATT");
          KICKR_GATT = GameObject.Find("PairingBTPanel");
          StartCoroutine(CheckAndCleanupBluetooth());
        */

    }

    // Lädt die gespeicherten ANT⁺-Geräte
    void LoadPairedDevices()
    {
        int savedFitnessDeviceNumber = PlayerPrefs.GetInt("PairedFitnessDevice", -1);
        if (savedFitnessDeviceNumber != -1)
            pairedFitnessDevice = new AntDevice { deviceNumber = savedFitnessDeviceNumber };

        int savedHeartRateDeviceNumber = PlayerPrefs.GetInt("PairedHeartRateDevice", -1);
        if (savedHeartRateDeviceNumber != -1)
            pairedHeartRateDevice = new AntDevice { deviceNumber = savedHeartRateDeviceNumber };
    }

    private void CheckAntStick()
    {
        try
        {
            int numDevices = (int)ANT_Common.getNumDetectedUSBDevices();

            if (numDevices > 0)
            {
                Debug.Log("AntStick OK");

                if (warningTextObject != null)
                    warningTextObject.SetActive(false); // Warntext ausblenden

                StartCoroutine(ScanForAntDevices());
            }
            else
            {
                Debug.LogWarning("no AntStick detected");
                if (warningTextObject != null)
                    warningTextObject.SetActive(true); // Warntext einblenden
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error while checking for ANT Stick: " + e.Message);
        }
    }

    // Startet den Scan-Prozess für ANT⁺-Geräte
    IEnumerator ScanForAntDevices()
    {
        // Die Displays für Fitnessgeräte und Herzfrequenz werden gesucht.
        var fitnessDisplay = FindFirstObjectByType<FitnessEquipmentDisplay>();
        var heartRateDisplay = FindFirstObjectByType<HeartRateDisplay>();

        if (fitnessDisplay == null || heartRateDisplay == null)
        {
            Debug.LogError("FitnessEquipmentDisplay oder HeartRateDisplay nicht gefunden!");
            yield break;
        }

        // Scanst beide Gerätearten
        fitnessDisplay.StartScan();
        heartRateDisplay.StartScan();
        yield return new WaitForSeconds(3f);

        // Kombinieren der gefundenen ANT⁺-Geräte
        var foundDevices = new List<AntDevice>();
        foundDevices.AddRange(fitnessDisplay.scanResult);
        foundDevices.AddRange(heartRateDisplay.scanResult);

        // >>> NEU: Automatisch verbinden, wenn gespeichertes Gerät gefunden wird
        foreach (var device in foundDevices)
        {
            if (pairedFitnessDevice != null && device.deviceNumber == pairedFitnessDevice.deviceNumber)
            {
                Debug.Log($"Auto-Verbinden mit gespeichertem Fitnessgerät {device.deviceNumber}");
                fitnessDisplay.ConnectToDevice(device);
                AddConnectedDeviceAndShowPanel($"FitnessDevice {device.deviceNumber} connected");
            }
            if (pairedHeartRateDevice != null && device.deviceNumber == pairedHeartRateDevice.deviceNumber)
            {
                Debug.Log($"Auto-Verbinden mit gespeichertem Herzfrequenzgerät {device.deviceNumber}");
                heartRateDisplay.ConnectToDevice(device);
                AddConnectedDeviceAndShowPanel($"HeartRate {device.deviceNumber} connected");
            }

        }

        ArrangeAntDeviceButtons(foundDevices);
    }

    // Ordnet die ANT⁺-Buttons im UI an
    void ArrangeAntDeviceButtons(List<AntDevice> devices)
    {
        // Entferne zunächst alle existierenden Buttons im Panel
        foreach (Transform child in antDeviceListPanel)
            Destroy(child.gameObject);

        // Beispielhafte Layout-Berechnung (an deine Bedürfnisse anpassen)
        int buttonsPerRow = 3;
        float buttonWidth = 200f;
        float spacing = 80f;
        float startX = 150f; // Fester Startpunkt für die X-Position
        float startY = -100f; // Start-Y-Position

        for (int i = 0; i < devices.Count; i++)
        {
            var device = devices[i];
            int row = i / buttonsPerRow;
            int col = i % buttonsPerRow;

            float xPos = startX + col * (buttonWidth + spacing);
            float yPos = startY - row * (buttonWidth + spacing);

            AddAntDeviceButton(device, xPos, yPos);
        }
    }

    // Fügt einen ANT⁺-Button dem UI hinzu
    void AddAntDeviceButton(AntDevice device, float xPos, float yPos)
    {
        GameObject buttonObj = Instantiate(antDeviceButtonPrefab, antDeviceListPanel);
        Button button = buttonObj.GetComponent<Button>();
        TMP_Text buttonText = buttonObj.GetComponentInChildren<TMP_Text>();
        Image buttonImage = buttonObj.GetComponent<Image>();
        Image icon = buttonObj.transform.Find("Icon").GetComponent<Image>();

        // Beschriftung des Buttons, z.B. "ANT+ HR(Nummer)" oder "ANT+ FEC(Nummer)"
        string label = device.deviceType == AntplusDeviceType.HeartRate
            ? $"HR({device.deviceNumber})"
            : $"FEC({device.deviceNumber})";
        buttonText.text = $"ANT+ {label}";

        // Verknüpft die Toggle-Pairing-Funktion
        button.onClick.AddListener(() => TogglePairing(device, buttonImage, icon));
        antDeviceButtons[device.deviceNumber] = button;

        // Setzt die Position des Buttons
        RectTransform rectTransform = buttonObj.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(xPos, yPos);

        UpdateAntDeviceButtonAppearance(device, buttonImage, icon);
    }

    // Wechselt den Pairing-Status eines ANT⁺-Geräts
    void TogglePairing(AntDevice device, Image buttonImage, Image icon)
    {
        if (device.deviceType == AntplusDeviceType.HeartRate)
        {
            if (pairedHeartRateDevice?.deviceNumber == device.deviceNumber)
                UnpairHeartRateDevice(buttonImage, icon);
            else
                PairHeartRateDevice(device, buttonImage, icon);
        }
        else
        {
            if (pairedFitnessDevice?.deviceNumber == device.deviceNumber)
                UnpairFitnessDevice(buttonImage, icon);
            else
                PairFitnessDevice(device, buttonImage, icon);
        }
    }

    // Methoden zum Pairing und Unpairing (Fitnessgerät)
    void PairFitnessDevice(AntDevice device, Image buttonImage, Image icon)
    {
        var display = FindFirstObjectByType<FitnessEquipmentDisplay>();
        display?.ConnectToDevice(device);

        pairedFitnessDevice = device;
        PlayerPrefs.SetInt("PairedFitnessDevice", device.deviceNumber);
        PlayerPrefs.Save();
        UpdateAntDeviceButtons();
        AddConnectedDeviceAndShowPanel($"FitnessDevice {device.deviceNumber} connected");
    }

    void UnpairFitnessDevice(Image buttonImage, Image icon)
    {
        var display = FindFirstObjectByType<FitnessEquipmentDisplay>();
        if (display != null && pairedFitnessDevice != null)
        {
            display.connected = false;
            display.deviceChannel?.Close();
            display.deviceChannel = null;
        }
        pairedFitnessDevice = null;
        PlayerPrefs.DeleteKey("PairedFitnessDevice");
        PlayerPrefs.Save();
        UpdateAntDeviceButtons();
    }

    // Methoden zum Pairing und Unpairing (Herzfrequenzgerät)
    void PairHeartRateDevice(AntDevice device, Image buttonImage, Image icon)
    {
        var display = FindFirstObjectByType<HeartRateDisplay>();
        display?.ConnectToDevice(device);

        pairedHeartRateDevice = device;
        PlayerPrefs.SetInt("PairedHeartRateDevice", device.deviceNumber);
        PlayerPrefs.Save();
        UpdateAntDeviceButtons();
        AddConnectedDeviceAndShowPanel($"HeartRate {device.deviceNumber} connected");
    }

    void UnpairHeartRateDevice(Image buttonImage, Image icon)
    {
        var display = FindFirstObjectByType<HeartRateDisplay>();
        if (display != null && pairedHeartRateDevice != null)
        {
            display.connected = false;
            display.deviceChannel?.Close();
            display.deviceChannel = null;
        }
        pairedHeartRateDevice = null;
        PlayerPrefs.DeleteKey("PairedHeartRateDevice");
        PlayerPrefs.Save();
        UpdateAntDeviceButtons();
    }

    // Aktualisiert alle ANT⁺-Buttons (z. B. Farbwechsel oder Icon-Update)
    void UpdateAntDeviceButtons()
    {
        foreach (var kvp in antDeviceButtons)
        {
            UpdateAntDeviceButtonAppearance(
                new AntDevice { deviceNumber = kvp.Key },
                kvp.Value.GetComponent<Image>(),
                kvp.Value.transform.Find("Icon").GetComponent<Image>()
            );
        }
    }

    // Setzt das Erscheinungsbild eines ANT⁺-Buttons (geparrt / ungeparrt)
    void UpdateAntDeviceButtonAppearance(AntDevice device, Image buttonImage, Image icon)
    {
        bool isPaired = (pairedFitnessDevice?.deviceNumber == device.deviceNumber) ||
                        (pairedHeartRateDevice?.deviceNumber == device.deviceNumber);
        buttonImage.color = isPaired ? Color.green : Color.red;
        icon.sprite = Resources.Load<Sprite>(isPaired ? "paired_icon" : "unpaired_icon");
    }

   
    IEnumerator FadeOutPanel(CanvasGroup canvasGroup)
    {
        yield return new WaitForSeconds(panelDisplayTime);

        float elapsedTime = 0f;
        float startAlpha = canvasGroup.alpha;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        connectionPanel.SetActive(false);
        connectedDevicesText.Clear(); // Text wieder leeren für nächste Anzeige
    }

    void AddConnectedDeviceAndShowPanel(string deviceInfo)
    {
        if (connectionPanel == null || connectionText == null)
        {
            Debug.LogWarning("Connection Panel oder Text nicht gesetzt!");
            return;
        }

        connectionPanel.SetActive(true);

        connectedDevicesText.Add(deviceInfo);

        // Aktualisiere den gesamten Text
        connectionText.text = string.Join("\n", connectedDevicesText);

        CanvasGroup canvasGroup = connectionPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = connectionPanel.AddComponent<CanvasGroup>();
        }

        canvasGroup.alpha = 1f;

        // Wenn schon eine Fade-Coroutine läuft, stoppen und neu starten
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeOutPanel(canvasGroup));
    }

   /* private IEnumerator CheckAndCleanupBluetooth()
    {
        yield return new WaitForSeconds(13f);

        Debug.Log("[CareerMenu] Überprüfe Bluetooth-Cleanup...");

        if (fec_prefab2 == null)
        {
            Debug.LogError("[CareerMenu] Fehler: prefab2 ist NULL!");
            yield break;
        }

        var feDisplay = Object.FindFirstObjectByType<FitnessEquipmentDisplay>();
        if (feDisplay == null)
        {
            Debug.LogError("[CareerMenu] Fehler: FitnessEquipmentDisplay Komponente nicht gefunden!");
            yield break;
        }

        Debug.Log($"[CareerMenu] FitnessEquipmentDisplay.connected = {feDisplay.connected}");
   */
     /*   if (feDisplay.connected)
        {
            Debug.Log("[CareerMenu] ANT+ Trainer connected ➔ Bluetooth wird bereinigt.");

            if (BluetoothManager.Instance != null)
            {
                // BluetoothManager.Instance.StopScan();
            }

            DestroyIfExists("BTManager");
            DestroyIfExists("PairBTManager");
            DestroyIfExists("HR_GATT");
            DestroyIfExists("KICKR_GATT");
            DestroyIfExists("PairingBTPanel");
        }
        else
        {
            Debug.LogWarning("[CareerMenu] Kein ANT+ Trainer verbunden ➔ Bluetooth bleibt aktiv.");
        }
    }*/


    private void DestroyIfExists(string objectName)
    {
        GameObject obj = GameObject.Find(objectName);
        if (obj != null)
        {
            Destroy(obj);
            Debug.Log($"[CareerMenu] {objectName} zerstört.");
        }
    }

    public void SaveUserSettingsFromInputs()
    {
        if (int.TryParse(weightInput.text, out int userWeight) &&
            int.TryParse(ftpInput.text, out int userFTP) &&
            int.TryParse(maxHRInput.text, out int userMaxHR))
        {
            PlayerPrefs.SetInt("UserWeight", userWeight);
            PlayerPrefs.SetInt("UserFTP", userFTP);
            PlayerPrefs.SetInt("UserMaxHR", userMaxHR);
            PlayerPrefs.Save();

            Debug.Log($"Gespeichert: Gewicht={userWeight} kg, FTP={userFTP} W, MaxHR={userMaxHR} bpm");
        }
        else
        {
            Debug.LogWarning("Eingabe ungültig – bitte überprüfe alle Felder!");
        }
    }

    private void LoadUserSettingsToInputs()
    {
        int userWeight = PlayerPrefs.GetInt("UserWeight", 70);
        int userFTP = PlayerPrefs.GetInt("UserFTP", 200);
        int userMaxHR = PlayerPrefs.GetInt("UserMaxHR", 180);

        weightInput.text = userWeight.ToString();
        ftpInput.text = userFTP.ToString();
        maxHRInput.text = userMaxHR.ToString();

        Debug.Log("Benutzereinstellungen geladen.");
    }

    public void RescanAntDevices()
    {
        StopAllCoroutines(); // Falls gerade ein Scan läuft
        StartCoroutine(ScanForAntDevices());
        Debug.Log("Erneuter ANT+ Scan gestartet.");
    }
}
