using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using DentedPixel;
using UnityEditor;



public class PrefabDemoDisplay : MonoBehaviour
{
    #region TextFields
    [Categorized, HideInInspector] public TMP_Text uiText_Speed;
    [Categorized, HideInInspector] public TMP_Text uiText_Cad;
    [Categorized, HideInInspector] public TMP_Text uiText_Pwr;
    [Categorized, HideInInspector] public TMP_Text uiText_Hr;
    [Categorized, HideInInspector] public TMP_Text uiText_Aer;
    [Categorized, HideInInspector] public TMP_Text uiText_AnAer;
    [Categorized, HideInInspector] public TMP_Text Lay3_uiText_Speed;
    [Categorized, HideInInspector] public TMP_Text Lay3_uiText_Cad;
    [Categorized, HideInInspector] public TMP_Text Lay3_uiText_Pwr;
    [Categorized, HideInInspector] public TMP_Text Lay3_uiText_Hr;
    [Categorized, HideInInspector] public TMP_Text Lay4_uiText_Speed;
    [Categorized, HideInInspector] public TMP_Text Lay4_uiText_Cad;
    [Categorized, HideInInspector] public TMP_Text Lay4_uiText_Pwr;
    [Categorized, HideInInspector] public TMP_Text Lay4_uiText_Hr;
    [Categorized, HideInInspector] public TMP_Text Lay5_uiText_Pwr;
    [Categorized, HideInInspector] public TMP_Text Lay6_uiText_Speed;
    [Categorized, HideInInspector] public TMP_Text Lay6_uiText_Cad;
    [Categorized, HideInInspector] public TMP_Text Lay6_uiText_Pwr;
    [Categorized, HideInInspector] public TMP_Text Lay6_uiText_Hr;
    [Categorized, HideInInspector] public TMP_Text Lay6_WKG;
    [Categorized, HideInInspector] public TMP_Text maxPowerText;
    [Categorized, HideInInspector] public TMP_Text maxCadenceText;
    [Categorized, HideInInspector] public TMP_Text maxSpeedText;
    [Categorized, HideInInspector] public TMP_Text maxHeartRateText;
    [Categorized, HideInInspector] public TMP_Text uiText_SysTime;
    [Categorized("Time"), HideInInspector] public TMP_Text m_gameTime;
    [Categorized, HideInInspector] public TMP_Text Lay4_m_gameTime;
    [Categorized, HideInInspector] public TMP_Text Lay8_EPOC;
    [Categorized, HideInInspector] public TMP_Text Lay_Calories_KCAL;
    [Categorized, HideInInspector] public TMP_Text Lay8_Distance;
    [Categorized, HideInInspector] public TMP_Text AuiText_Speed;
    [Categorized, HideInInspector] public TMP_Text AuiText_Cad;
    [Categorized, HideInInspector] public TMP_Text AuiText_Pwr;
    [Categorized, HideInInspector] public TMP_Text AuiText_Hr;
    [Categorized, HideInInspector] public TMP_Text AuiText_MoveTime;
    [Categorized, HideInInspector] public TMP_Text AuiText_Slope;
    [Categorized, HideInInspector] public TMP_Text AuiText_Calc;
    [Categorized, HideInInspector] public TMP_Text AuiText_Distance;
    [Categorized, HideInInspector] public TMP_Text AuiText_WKG;
    [Categorized, HideInInspector] public TMP_Text wattoMeterText;
    [Categorized, HideInInspector] public TMP_Text hrMeterText;
    [Categorized, HideInInspector] public TMP_Text tssDisplay;          // UI-Element für die TSS-Anzeige
    [Categorized, HideInInspector] public TMP_Text switchValueText;
    #endregion


    private int maxPower = 0;
    private float maxCadence = 0;
    private float maxSpeed = 0f;
    private float maxHeartRateUI = 0;

   
    public float switchInterval = 5f; // Switch interval in seconds
    private int currentDisplayIndex = 0;
    private string[] displayOptions = { "Pwr", "Cad", "Spd", "Hr" };

    private float lastSwitchTime;

   

    public HeartRateCalculator heartRateCalculator;
    //  public GameObject BikeTrainerButtons;

    //For Indicator
    [SerializeField] private Image myIndicator;
    [SerializeField] private Color myOnColor;
    [SerializeField] private Color myOffColor;

    private int restingHeartRate = 45; // Ruhepuls in Schlägen pro Minute
    private int maxHeartRate = 183; // Maximalpuls in Schlägen pro Minute
    private int weightKg = 84;
    public int age = 51;
    public Slider ftpZoneSlider;
    public Image sliderFill;
    public Color[] zoneColors;
 


    private float ftpValue = 150f; // 150f is a good default value
    //Zones for Slider -> 0-59, 60-79,80-90,91-104,105-120,121
    // we need % heartrate %ftp %power %vo2max image indicator // grey blue green orange red
    public TMP_Text[] zoneTimerTexts;
    private float[] zoneTimers;
    private bool[] inZone;
    private float[] zoneTimeAtExit;


    private float aerobicTrainingEffect = 0f;
    private float interval = 1f; // Intervall in Minuten
    private float totalTime = 0f; // Gesamtdauer des Trainings in Minuten
  //  private float currentTime = 0f;
    public float seconds;
    public float minutes;
    public float gameTimer;
   
    public bool gamePaused = true;

   

    public FitnessEquipmentDisplay fec;
    public HeartRateDisplay hr;

    public int   t_power;
    public float t_cad;
    public float t_speed;
    public float t_heartrate;

    public float timeInSeconds;
    private const float MET_ConversionFactor = 3.5f;



    public float VO2Max = 0;
    public string gender; // Geschlecht (z.B. "männlich" oder "weiblich")

    private float epocValue;
  
    public Button openVideoButton;
    public GameObject bikeRider;
    public KeyCode bikeRiderOffOnKey = KeyCode.X;
    public KeyCode timerKey;

    public GameObject alternateComp;
    public GameObject computer;
    public GameObject lowerLeftComp;
    public KeyCode alternateComputer = KeyCode.Y;
    public KeyCode lowerLeftComputer = KeyCode.Q;
    public KeyCode PowerGraphKey = KeyCode.G;
    public GameObject powerGraph;
    public GameObject nextB;
    public GameObject prevB;
    public GameObject OnOffB;



    public TMP_InputField fecInputField;
    public TMP_InputField hrInputField;

    public TMP_InputField weightInputField;
    public TMP_InputField ftpField;
    public TMP_InputField maxHrField;

    public string screenshotNamePrefix = "Screenshot_";
    public int screenshotCount = 0;

    //Alternate Computer
   


    public Image[] zoneImages; // Array der Bilder für die Zonen
    public float scaleWhenInZone = 1.2f; // Skalierungsfaktor, wenn in der Zone
  //  private bool isInitialized = false;
    private float[] zoneThresholds = { 0.55f, 0.75f, 0.9f, 1.05f, 1.2f, 1.5f, 2.0f };

    public GameObject imageToShow;
    private float lastPowerValue = 0f;
    private bool wasImageActivated = false;

    public float caloriesRealBurned;
    public float lastCaloriesBurned;

    public Slider wattometer;
    public Slider hrMeter;
  
    public Color backgroundColorWattoMeter = Color.blue;

    //Alternate2
    public Slider hr_zoneSlider2;
    public Slider ftp_zoneSlider2;

    private float startTime;
    public float updateInterval = 10f;
    public float movetime;

    public GameObject searchBox;

    public Image speedKMH;
    float speedImage, maxSpeedKMH = 50f;

    public GameObject camera1;
    public GameObject camera2;

    public GameObject bike;
    public GameObject rider;



    /*private List<float> powerData = new List<float>();
    public LineRenderer lineRenderer;
    public Gradient colorGradient; // Ein Gradient, der die Farben für die Zonen enthält
    private float xOffset = 0.5f; // Der Abstand zwischen den Punkten
    private int maxPoints = 100;  // Maximal anzuzeigende Punkte
    */

  //  private float elapsedTime = 0f;  // Zeit in Sekunden
  //  private float totalTSS = 0f;     // Gesamt-TSS
    private float cumulativeTSS = 0f; // Kumulativer TSS

    private float tss;
    private float recoveryTime;


    private List<float> powerData = new List<float>();  // Liste zur Speicherung der Leistungsdaten
    public int averagingInterval = 30; // Gleitender Durchschnitt über 30 Sekunden

    public bool riderStarted = false;

    void Start()
    {
       
        myOnColor.a = 1;
        myOffColor.a = 1;
        InvokeRepeating("CalculateAerobicTrainingEffect", 2.0f, 1.0f);
        gamePaused = true;
        riderStarted = false;

        ftpZoneSlider.maxValue = ftpValue * zoneThresholds[zoneThresholds.Length - 1];
        zoneTimers = new float[zoneThresholds.Length];
        inZone = new bool[zoneThresholds.Length];
        zoneTimeAtExit = new float[zoneThresholds.Length];
        speedImage = maxSpeedKMH;

        for (int i = 0; i < zoneTimers.Length; i++)
        {
            zoneTimers[i] = 0f;
            inZone[i] = false;
            zoneTimeAtExit[i] = 0f;
        }

        for (int i = 0; i < zoneImages.Length - 1; i++)
        {
            UpdateZoneImageScale(i, false);
        }
        UpdateUI();
        lastSwitchTime = Time.time;
      //  InvokeRepeating("TakeScreenshot", 0f, 300f); // 300 Sekunden = 5 Minuten
        
        //To make sure to load values weight ftp and device IDs
        ConnectToDevices();
        wattometer.minValue = 0.6f;
        wattometer.maxValue = 1.30f;

        hrMeter.minValue = 0.5f;
        hrMeter.maxValue = 1.05f;

        ftp_zoneSlider2.minValue = 0.001f;
        ftp_zoneSlider2.maxValue = 1.325f;

        hr_zoneSlider2.minValue = 0.0001f;
        hr_zoneSlider2.maxValue = 0.98f;

        startTime = Time.time;
        StartCoroutine(CalculateCalories());

        InvokeRepeating("MoverOfStuff", 0f, movetime);

        // Aktiviert die erste Kamera und deaktiviert die zweite
        camera1.SetActive(true);
        camera2.SetActive (false);

        //  lineRenderer = GetComponent<LineRenderer>();
        //  lineRenderer.positionCount = 0; // Starte ohne Punkte
        cumulativeTSS = 0f;
        // totalTSS = 0f;

        // Lade die gespeicherten Werte und setze sie in die Input Felder
        if (PlayerPrefs.HasKey("FEC_Value"))
        {
            fecInputField.text = PlayerPrefs.GetInt("FEC_Value").ToString();
        }

        if (PlayerPrefs.HasKey("HR_Value"))
        {
            hrInputField.text = PlayerPrefs.GetInt("HR_Value").ToString();
        }

        if (PlayerPrefs.HasKey("Weight_Value"))
        {
            weightInputField.text = PlayerPrefs.GetInt("Weight_Value").ToString();
        }

        if (PlayerPrefs.HasKey("FTP_Value"))
        {
            ftpField.text = PlayerPrefs.GetInt("FTP_Value").ToString();
        }

        if (PlayerPrefs.HasKey("HRMax_Value"))
        {
            maxHrField.text = PlayerPrefs.GetInt("HRMax_Value").ToString();
        }

    }
    
   
        //For Computer Indicator
        public void OnClick()
    {
        if (fec.GetComponent<FitnessEquipmentDisplay>().connected == true)
        {
            myIndicator.color = myOnColor;
        }
        else
        {
            myIndicator.color = myOffColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        t_power = fec.GetComponent<FitnessEquipmentDisplay>().instantaneousPower;
        t_cad = fec.GetComponent<FitnessEquipmentDisplay>().cadence;
        t_speed = fec.GetComponent<FitnessEquipmentDisplay>().speed;
        t_heartrate = hr.GetComponent<HeartRateDisplay>().heartRate;

        if (hr.GetComponent<HeartRateDisplay>().connected == true)
        {
            uiText_Hr.text = t_heartrate.ToString();
            Lay3_uiText_Hr.text = t_heartrate.ToString();
            Lay4_uiText_Hr.text = t_heartrate.ToString();
            Lay6_uiText_Hr.text = t_heartrate.ToString();
        }

        if (GameObject.Find("FitnessEquipmentDisplay"))
        {
            uiText_Pwr.text = t_power.ToString();
            uiText_Speed.text = t_speed.ToString("F0");
            uiText_Cad.text = t_cad.ToString();

            Lay3_uiText_Pwr.text = t_power.ToString();
            Lay3_uiText_Speed.text = t_speed.ToString("F0");
            Lay3_uiText_Cad.text = t_cad.ToString();

            Lay4_uiText_Pwr.text = t_power.ToString();
            Lay4_uiText_Speed.text = t_speed.ToString("F0");
            Lay4_uiText_Cad.text = t_cad.ToString();

            Lay5_uiText_Pwr.text = t_power.ToString();

            Lay6_uiText_Pwr.text = t_power.ToString();
            Lay6_uiText_Speed.text = t_speed.ToString("F0");
            Lay6_uiText_Cad.text = t_cad.ToString();
            float wkgfloat = (float)t_power / weightKg;
            Lay6_WKG.text = wkgfloat.ToString("F1");

            //SystemTime
            uiText_SysTime.text = System.DateTime.Now.ToString("HH:mm:ss");

            if (fec.GetComponent<FitnessEquipmentDisplay>().connected == true)
            {
                heartRateCalculator.GetComponent<HeartRateCalculator>().enabled = true;
            }
            //Timer
            if (!gamePaused)
            {
                gameTimer += Time.deltaTime;
                //gameTimer = fec.GetComponent<FitnessEquipmentDisplay>().elapsedTime; //+= Time.deltaTime;
                int hours = Mathf.FloorToInt(gameTimer / 3600);
                int minutes = Mathf.FloorToInt((gameTimer % 3600) / 60);
                int seconds = Mathf.FloorToInt(gameTimer % 60);
                string timerString = string.Format("{0:0}:{1:00}:{2:00}", hours, minutes, seconds);
                m_gameTime.text = timerString;
                Lay4_m_gameTime.text = timerString;
                AuiText_MoveTime.text = timerString;
            }

            float distanceTrav = fec.GetComponent<FitnessEquipmentDisplay>().distanceTraveled;
            Lay8_Distance.text = (distanceTrav / 1000).ToString("F2");
            //for Alternate Computer
            
            AuiText_Hr.text = t_heartrate.ToString();
            AuiText_WKG.text = wkgfloat.ToString("F1");
            AuiText_Pwr.text = t_power.ToString();
            AuiText_Speed.text = t_speed.ToString("F0");
            AuiText_Cad.text = t_cad.ToString();
            AuiText_Distance.text = (distanceTrav / 1000).ToString("F2");



        }

        float powerValue = t_power;
        ftpZoneSlider.value = powerValue;
        UpdateZoneColorAndTimers(powerValue);
        UpdateZoneImagesScale(powerValue);

        // Update max values
        maxPower = Mathf.Max(maxPower, t_power);
        maxCadence = Mathf.Max(maxCadence, t_cad);
        maxSpeed = Mathf.Max(maxSpeed, t_speed);
        maxHeartRateUI = Mathf.Max(maxHeartRateUI, t_heartrate);
        UpdateUI();

        if (Time.time - lastSwitchTime >= switchInterval)
        {
            SwitchDisplay();
            lastSwitchTime = Time.time;
        }
        /*
        //this should be overviewed
        float caloriesBurned = CalculateCaloriesBurned(t_power, weightKg, (int)t_heartrate, timeInSeconds, age);
        float caloriesBurnedDelta = caloriesBurned - lastCaloriesBurned;
        caloriesRealBurned += caloriesBurned * caloriesBurnedDelta;
        lastCaloriesBurned = caloriesBurned;
        */

        //calc the magic of Calories burned
           // v_calories = v_elapsedTime * ((0.6309f * (float.Parse(heartRate.text) + 0.19988f * weight + 0.2017f * age - 20.4022f)
           // caloriesRealBurned = fec.GetComponent<FitnessEquipmentDisplay>().elapsedTime * ((0.6309f * ( (t_heartrate) + 0.19988f * weightKg + 0.2017f * age - 20.4022f) / 4.184f)) / 100;
            Lay_Calories_KCAL.text = caloriesRealBurned.ToString("F0");
      //  Debug.Log($"Calories Burned: {caloriesRealBurned} kcal");

        /*    else
            {
                v_calories = v_elapsedTime * ((0.6309f * ((130) + 0.19988f * weight + 0.2017f * age - 20.4022f) / 4.184f)) / 1000;
                caloriesM.text = v_calories.ToString("F0");
                Lay_Calories_KCAL.text = (caloriesRealBurned / 100000).ToString("F0");
        */

        //  Debug.Log($"Calories Burned: {caloriesBurned} kcal");
        //For Alternate Computer
        AuiText_Calc.text =  (caloriesRealBurned / 2).ToString("F0");

        CalculateVO2Max();
        CalculateEPOC();
        hideButton();

        if (Input.GetKeyDown(bikeRiderOffOnKey))
        {
            bikeRider.SetActive(!bikeRider.activeSelf);
        }

        if (Input.GetKeyDown(PowerGraphKey))
        {
            powerGraph.SetActive(!powerGraph.activeSelf);
        }

        if (Input.GetKeyDown(alternateComputer))
        {
            alternateComp.SetActive(!alternateComp.activeSelf);
            computer.SetActive(!computer.activeSelf);
            prevB.SetActive(!prevB.activeSelf);
            nextB.SetActive(!nextB.activeSelf);
            OnOffB.SetActive(!OnOffB.activeSelf);
        }

        if(Input.GetKeyDown(lowerLeftComputer))
        {
            lowerLeftComp.SetActive(!lowerLeftComp.activeSelf);

        }

        if(t_speed >= 0.5)
        {
            riderStarted = true;
        }

        //if (Input.GetKeyDown(timerKey))
        if (riderStarted == true)
        {
            gamePaused = false;
        }
        if (riderStarted == false)
        {
            gamePaused = true;
        }

            if (powerValue >= ftpValue * zoneThresholds[zoneThresholds.Length - 1])
        {
            if (!wasImageActivated)
            {
                imageToShow.SetActive(true); // Aktiviere das Bild, wenn der powerValue den letzten Schwellenwert erreicht oder überschreitet
                wasImageActivated = true;
            }
        }
        else
        {
            if (wasImageActivated)
            {
                imageToShow.SetActive(false); // Deaktiviere das Bild, wenn der powerValue unter dem letzten Schwellenwert liegt
                wasImageActivated = false;
            }
        }

        lastPowerValue = powerValue;

        WattsAndHeartZones();

        if (t_speed > 1 && t_power > 1)
        {
            ticker.gameObject.SetActive(false);
           
        }
        else
        {
            ticker.gameObject.SetActive(true);
        }

        speedBarFiller();

        if (Input.GetKeyDown(KeyCode.C))
        {
            // Wechselt die Kameras
            SwitchCameras();
        }

        CollectPowerData(t_power);

        // Berechne die Normalized Power
        float normalizedPower = CalculateNormalizedPower();

        /*  // Berechne den aktuellen TSS-Wert basierend auf der Normalized Power und der FTP
          totalTSS = CalculateTSS(normalizedPower, ftpValue, gameTimer);

          // Optional: TSS im UI anzeigen
          if (tssDisplay != null)
          {
              tssDisplay.text = totalTSS.ToString("F0");
          }
        */
        // Kumulative Berechnung des TSS
        /* float currentTSSIncrement = CalculateTSSIncrement(normalizedPower, ftpValue, Time.deltaTime);
         cumulativeTSS += currentTSSIncrement;

         // Optional: TSS im UI anzeigen
         if (tssDisplay != null)
         {
             tssDisplay.text = cumulativeTSS.ToString("F0");
         }*/
        // Beispiel: Neue Leistungsdaten hinzufügen (in einem echten Fall kommen diese von einem Sensor)
        /*   float newPower = GetPowerData();
           powerData.Add(newPower);

           // Begrenze die Anzahl der Punkte in der Liste
           if (powerData.Count > maxPoints)
           {
               powerData.RemoveAt(0);
           }

           // Update die Power-Grafik
           UpdatePowerGraph();
   */
        // TSS in Echtzeit berechnen
        // TSS in Echtzeit berechnen
        // Berechnung des Intensitätsfaktors (IF)
        float intensityFactor = t_power / ftpValue;
        tss = (gameTimer % 3600 / 60) * intensityFactor * intensityFactor * 100;

        // Herzfrequenz-Intensitätsfaktor berechnen
        float hrIntensityFactor = heartRateCalculator.hr_average / maxHeartRate;

        // Erholungszeit berechnen mit Herzfrequenz als zusätzlichem Faktor
        recoveryTime = CalculateRecoveryTime(tss, hrIntensityFactor);

        // Erholungszeit auf 72 Stunden begrenzen
        recoveryTime = Mathf.Min(recoveryTime, 72f);

      //  Debug.Log("Erholungszeit (in Stunden): " + recoveryTime);
        if (tssDisplay != null)
        {
            tssDisplay.text = recoveryTime.ToString("F0");
        }
        

    }

    public void speedBarFiller()
    {
        speedKMH.fillAmount = fec.speed / maxSpeedKMH;
        if (fec.speed > maxSpeedKMH) speedKMH.fillAmount = maxSpeedKMH;
    }
    public void CalculateVO2Max()
    {
        // float genderFactor = (gender == 1) ? 1f : 0f; // Geschlechtsfaktor

        float timeMinutes = fec.GetComponent<FitnessEquipmentDisplay>().elapsedTime / 60;
                VO2Max = (132.853f - 0.0769f * weightKg - 0.3877f * age + 6.315f * 1) / timeMinutes + 3.2649f;

      //  Debug.Log("Estimated VO2Max: " + VO2Max);
    }


    void CalculateEPOC()
    {
        // Hier sollte eine Formel oder ein Modell verwendet werden, um den EPOC-Wert zu schätzen.
        // Dieses Beispiel verwendet eine vereinfachte Formel.
                
        float intensityFactor = t_power / VO2Max; // Vereinfachte Intensitätsberechnung
        float ageFactor = 10; // age * 0.2f; // Vereinfachter Altersfaktor
        float hrFactor = t_heartrate / 100.0f; // Normalisierte Herzfrequenz
      //  float genderFactor = (gender == "männlich") ? 0.1f : 0.0f; // Geschlechtsfaktor

        epocValue = fec.GetComponent<FitnessEquipmentDisplay>().elapsedTime * (intensityFactor + ageFactor + hrFactor) * weightKg * 0.1f;

       // Debug.Log("Estimated EPOC Value: " + epocValue);
        Lay8_EPOC.text = (epocValue / 10000).ToString("F0");
    }

    private float CalculateCaloriesBurned(float power, float weight, int heartRate, float timeInSeconds, int age)
    {
        float met = power / MET_ConversionFactor;
        float caloriesPerMinute = met * 3.5f * weight / 200; // Formel zur Berechnung der Kalorien pro Minute

        float caloriesBurned = caloriesPerMinute * (Time.time / 60); // Kalorien für die gesamte Zeit in Minuten

        // Berücksichtigung des Alters
        float ageFactor = 0.2017f * age + 0.6309f; // Vereinfachter Age Factor (kann je nach Bedarf angepasst werden)
        caloriesBurned *= ageFactor;

        return caloriesBurned;
    }

    private void UpdateUI()
    {
        maxPowerText.text = maxPower.ToString("F1");
        maxCadenceText.text = maxCadence.ToString();
        maxSpeedText.text = maxSpeed.ToString("F1");
        maxHeartRateText.text = maxHeartRate.ToString();
    }

    public bool ExceedsMaxPowerZone(float powerValue)
    {
        return powerValue > ftpZoneSlider.maxValue;
    }

    /// <summary>
    /// Get the power value in percentage compared to the user's Functional Threshold Power
    /// </summary>
    /// <returns>The current power value</returns>
    public float GetPowerValue()
    {
        return t_power / ftpValue;
    }


    /// <summary>
    /// Get the power value in percentage compared to the user's Functional Threshold Power
    /// </summary>
    /// <returns>The normalized power value</returns>
    public float GetNormalizedPowerValue(float powerValue)
    {
        return powerValue / ftpValue;
    }

    /// <summary>
    /// Get the power threshold value of the last zone
    /// </summary>
    /// <returns>The maximum power value</returns>
    public float GetMaxPower()
    { 
        return zoneThresholds[zoneThresholds.Length - 1];
    }

    /// <summary>
    /// Get the color of the corresponding Power Zone
    /// </summary>
    /// <param name="powerValue">The Power value</param>
    /// <returns>The Power Zone color. If power exceeds last zone, defaults to a dark purple tint.</returns>
    /// <exception cref="System.ArgumentException"></exception>
    public Color GetPowerZoneColor(float powerValue)
    {
        if (ExceedsMaxPowerZone(powerValue)) return new Color(0.3f, 0f, 0.4f, 1f);

        for (int i = 0; i < zoneThresholds.Length; i++)
        {
            if (powerValue <= ftpValue * zoneThresholds[i]) return zoneColors[i];
        }
        throw new System.ArgumentException($"GetPozerZoneColor: powerValue ({powerValue}) is not within zoneThresholds, nor exceeding maxValue!");
    }

    /// <summary>
    /// Get the index of the corresponding Power Zone
    /// </summary>
    /// <param name="powerValue">The Power value</param>
    /// <returns>The Power Zone index. If power exceeds last zone, the index will be the number of zones!</returns>
    public int GetPowerZoneIndex(float powerValue)
    {
        for (int i = 0; i < zoneThresholds.Length; i++)
        {
            if (powerValue <= ftpValue * zoneThresholds[i]) return i;
        }
        return zoneThresholds.Length;
    }
    
    private void UpdateZoneColorAndTimers(float powerValue)
    {
        bool exceededMaxZone = ExceedsMaxPowerZone(powerValue);

        sliderFill.color = GetPowerZoneColor(powerValue);
        UpdateTimers(GetPowerZoneIndex(powerValue), !exceededMaxZone);

        if (exceededMaxZone)
        {
            UpdateTimers(zoneTimers.Length - 1, true);
        }
    }

    private void UpdateTimers(int activeZoneIndex, bool isActiveZone)
    {
        for (int i = 0; i < zoneTimers.Length; i++)
        {
            if (i == activeZoneIndex)
            {
                if (isActiveZone)
                {
                    if (!inZone[i])
                    {
                        inZone[i] = true;
                        UpdateZoneImageScale(i, true);
                    }

                    zoneTimers[i] += Time.deltaTime;
                }
                else
                {
                    inZone[i] = false;
                    UpdateZoneImageScale(i, false);
                }
            }
            else
            {
                inZone[i] = false;
                UpdateZoneImageScale(i, false);
            }

            // Berechne Minuten und Sekunden
            int minutes = (int)(zoneTimers[i] / 60);
            int seconds = (int)(zoneTimers[i] % 60);

            // Aktualisiere die Textkomponenten der Timer im UI im Format Minuten:Sekunden
            zoneTimerTexts[i].text = $"{minutes:00}:{seconds:00}";
        }
    }

    public void SaveValuesToPlayerPrefs()
    {
        int value1, value2, value3, value4, value5;

        // Überprüfe, ob die Eingabefelder gültige Integer-Werte enthalten
        if (int.TryParse(fecInputField.text, out value1) && int.TryParse(hrInputField.text, out value2) && int.TryParse(weightInputField.text, out value3) && int.TryParse(ftpField.text, out value4) && int.TryParse(maxHrField.text, out value5))
        {
            // Speichere die Werte in den PlayerPrefs mit eindeutigen Schlüsseln
            PlayerPrefs.SetInt("FEC_Value", value1);
            PlayerPrefs.SetInt("HR_Value", value2);
            PlayerPrefs.SetInt("Weight_Value", value3);
            PlayerPrefs.SetInt("FTP_Value", value4);
            PlayerPrefs.SetInt("HRMax_Value", value5);
            PlayerPrefs.Save(); // Speichert die PlayerPrefs-Daten sofort

          //  Debug.Log("Integer-Werte erfolgreich gespeichert!");
            ConnectToDevices();
        }
        else
        {
            Debug.LogWarning("Eingabe ist keine gültige Ganzzahl!");
        }
    }

    public void ConnectToDevices()
    {
        if (PlayerPrefs.HasKey("FEC_Value") && PlayerPrefs.HasKey("HR_Value") && PlayerPrefs.HasKey("Weight_Value") && PlayerPrefs.HasKey("FTP_Value") && PlayerPrefs.HasKey("HRMax_Value"))
        {
            int loadedValue1 = PlayerPrefs.GetInt("FEC_Value");
            int loadedValue2 = PlayerPrefs.GetInt("HR_Value");
            int loadedValue3 = PlayerPrefs.GetInt("Weight_Value");
            int loadedValue4 = PlayerPrefs.GetInt("FTP_Value");
            int loadedValue5 =  PlayerPrefs.GetInt("HRMax_Value");

            fec.GetComponent<FitnessEquipmentDisplay>().deviceID = loadedValue1;
            hr.GetComponent<HeartRateDisplay>().deviceID = loadedValue2;
            weightKg = loadedValue3;
            ftpValue = loadedValue4;
            maxHeartRate = loadedValue5;
            //i need to get the values here


            Debug.Log("Integer-Werte erfolgreich geladen!");
        }
        else
        {
            Debug.LogWarning("Keine gespeicherten Integer-Werte gefunden!");
        }
    }

    public void ConnectAfterSave()

    {
        int loadedValue1 = PlayerPrefs.GetInt("FEC_Value");
        int loadedValue2 = PlayerPrefs.GetInt("HR_Value");


        fec.GetComponent<FitnessEquipmentDisplay>().deviceID = loadedValue1;
        hr.GetComponent<HeartRateDisplay>().deviceID = loadedValue2;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void TakeScreenshot()
    {
        // Erstelle einen Dateinamen für den Screenshot
        string screenshotName = screenshotNamePrefix + screenshotCount.ToString() + ".png";

        // Speichere den Screenshot
        ScreenCapture.CaptureScreenshot(screenshotName);

        // Erhöhe den Zähler für die Screenshot-Nummerierung
        screenshotCount++;

       // Debug.Log("Screenshot " + screenshotName + " wurde erstellt.");
    }

    public void UnPause()
    {
        Debug.Log("Klicked");
        gamePaused = !gamePaused;

    }

    private void SwitchDisplay()
    {
        currentDisplayIndex = (currentDisplayIndex + 1) % displayOptions.Length;

        string displayOption = displayOptions[currentDisplayIndex];
        string valueToShow = GetValueForDisplayOption(displayOption);

        switchValueText.text = $"{displayOption} {valueToShow}";
    }

    private string GetValueForDisplayOption(string displayOption)
    {
        switch (displayOption)
        {
            case "Pwr":
                return t_power.ToString("F0");
            case "Cad":
                return t_cad.ToString("F0");
            case "Spd":
                return t_speed.ToString("F0");
            case "Hr":
                return t_heartrate.ToString("F0");
            default:
                return "N/A";
        }
    }


   
    /*
    public void FitnessButton1() {
        GameObject.Find("FitnessEquipmentDisplay").GetComponent<FitnessEquipmentDisplay>().SetTrainerResistance(0);
    }
    public void FitnessButton2() {
        GameObject.Find("FitnessEquipmentDisplay").GetComponent<FitnessEquipmentDisplay>().SetTrainerResistance(50);
    }
    public void FitnessButton3() {
        GameObject.Find("FitnessEquipmentDisplay").GetComponent<FitnessEquipmentDisplay>().SetTrainerResistance(100);
    }
    public void FitnessButton4() {
        GameObject.Find("FitnessEquipmentDisplay").GetComponent<FitnessEquipmentDisplay>().RequestTrainerCapabilities();
    }
    public void FitnessButton5() {
        GameObject.Find("FitnessEquipmentDisplay").GetComponent<FitnessEquipmentDisplay>().SetTrainerUserConfiguration(10, 80); //set the user weight
    }
    public void FitnessButton6() {
        GameObject.Find("FitnessEquipmentDisplay").GetComponent<FitnessEquipmentDisplay>().SetTrainerTargetPower(200); // set the target power in watt
    }
    public void FitnessButton7() {
        GameObject.Find("FitnessEquipmentDisplay").GetComponent<FitnessEquipmentDisplay>().SetTrainerSlope(5); //set the trainer simulation slope in % (grade) between -200 & 200%
    }
    public void FitnessButton8() {
        GameObject.Find("FitnessEquipmentDisplay").GetComponent<FitnessEquipmentDisplay>().RequestCommandStatus();
    }
    public void FitnessButton9() {
        GameObject.Find("FitnessEquipmentDisplay").GetComponent<FitnessEquipmentDisplay>().RequestUserConfig();
    }*/



        private void CalculateAerobicTrainingEffect()
    {
        totalTime = fec.GetComponent<FitnessEquipmentDisplay>().elapsedTime / 60;
        
            float currentHeartRate = t_heartrate; // Hier die Methode zum Abrufen der Herzfrequenz aufrufen
            int currentPower = t_power;
            float relativeIntensity = (currentHeartRate - restingHeartRate) / (maxHeartRate - restingHeartRate);
            //float relativeIntensity = currentPower / (weightKg * currentPower);
            aerobicTrainingEffect += relativeIntensity * interval / 100;
           // currentTime += interval;
 
        // Ausgabe des aeroben Trainingseffekts
       // Debug.Log($"Aerober Trainingseffekt (ATL): {aerobicTrainingEffect}");
        uiText_Aer.text = aerobicTrainingEffect.ToString("F1");
        //uiText_AnAer.text = anaerobicTrainingEffect.ToString("F1");
    }

    private void hideButton()
    {
        if(fec.GetComponent<FitnessEquipmentDisplay>().speed > 0)
        {
            openVideoButton.gameObject.SetActive(false);
            searchBox.gameObject.SetActive(false);
           // inputField.gameObject.SetActive(!inputField.gameObject.activeSelf);
        }
        else
        {
            openVideoButton.gameObject.SetActive(true);
            searchBox.gameObject.SetActive(true);
        }
    }



  //  private bool isLastImageScaled = false;

    private void UpdateZoneImagesScale(float powerValue)
    {
        for (int i = 0; i < zoneThresholds.Length; i++)
        {
            if (powerValue <= ftpValue * zoneThresholds[i])
            {
                UpdateZoneImageScale(i, true);
                return; // Beende die Schleife, wenn die Zone gefunden wurde
            }
           
        }
        // Skaliere alle Bilder auf Normalgröße, wenn keine Zone gefunden wurde
        for (int i = 0; i < zoneImages.Length; i++)
        {
            UpdateZoneImageScale(i, false);
        }
    }


    private void UpdateZoneImageScale(int index, bool inZone)
    {
        if (index >= 0 && index < zoneImages.Length)
        {
            if (inZone || index == zoneImages.Length - 1)
            {

                zoneImages[index].rectTransform.localScale = new Vector3(scaleWhenInZone, scaleWhenInZone, 1f);

            }
            else
            {
                zoneImages[index].rectTransform.localScale = Vector3.one;
            }
        }
    }

    //WattoMeter

    public Image bgImageWatt;
    public Image bgImageHr;
    public void WattsAndHeartZones()
    {
        if (t_heartrate > 0)
        {

            float forHrMeter = t_heartrate / maxHeartRate;
           // float forWattoMeter = t_power / ftpValue;

           // wattometer.value = forWattoMeter;
            hrMeter.value = forHrMeter;

            hr_zoneSlider2.value = forHrMeter;
          //  ftp_zoneSlider2.value = forWattoMeter;


           // wattoMeterText.text = t_power.ToString();
            hrMeterText.text = t_heartrate.ToString("F0");

            if (forHrMeter < 0.50f)
            {
                bgImageHr.color = new Color32(203, 203, 203, 150);
            }
            if (forHrMeter > 0.50f && forHrMeter <= 0.60f)
            {
                bgImageHr.color = new Color32(203, 203, 203, 150);
            }
            if (forHrMeter > 0.601f && forHrMeter <= 0.70f)
            {
                bgImageHr.color = new Color32(75, 159, 254, 150);
            }
            if (forHrMeter > 0.701f && forHrMeter <= 0.80f)
            {
                bgImageHr.color = new Color32(0, 255, 66, 150);
            }
            if (forHrMeter > 0.801f && forHrMeter <= 0.90f)
            {
                bgImageHr.color = new Color32(252, 175, 23, 150);
            }
            if (forHrMeter > 0.901f && forHrMeter <= 1.0f)
            {
                bgImageHr.color = new Color32(244, 26, 4, 150);
            }
            if (forHrMeter > 1.01f)
            {
                bgImageHr.color = new Color32(246, 0, 255, 150);
            }

            //
        }
        if (t_power > 0)
        {
           // float forHrMeter = t_heartrate / maxHeartRate;
            float forWattoMeter = t_power / ftpValue;

            wattometer.value = forWattoMeter;
           // hrMeter.value = forHrMeter;

          //  hr_zoneSlider2.value = forHrMeter;
            ftp_zoneSlider2.value = forWattoMeter;


            wattoMeterText.text = t_power.ToString();
            hrMeterText.text = t_heartrate.ToString("F0");

            if (forWattoMeter < 0.55f)
            {
                bgImageWatt.color = new Color32(203, 203, 203, 150);
            }
            if (forWattoMeter > 0.55f && forWattoMeter <= 0.75f)
            {
                bgImageWatt.color = new Color32(75, 159, 254, 150);
            }
            if (forWattoMeter > 0.75f && forWattoMeter <= 0.90f)
            {
                bgImageWatt.color = new Color32(0, 255, 66, 150);
            }
            if (forWattoMeter > 0.90f && forWattoMeter <= 1.05f)
            {
                bgImageWatt.color = new Color32(241, 244, 4, 150);
            }
            if (forWattoMeter > 1.05f && forWattoMeter <= 1.20f)
            {
                bgImageWatt.color = new Color32(252, 175, 23, 150);
            }
            if (forWattoMeter > 1.20f && forWattoMeter <= 1.30f)
            {
                bgImageWatt.color = new Color32(244, 26, 4, 150);
            }
            if (forWattoMeter > 1.30f)
            {
                bgImageWatt.color = new Color32(246, 0, 255, 150);
            }
        }
        else  return;

    }

    IEnumerator CalculateCalories()
    {
        while (true)
        {
            // caloriesRealBurned = fec.GetComponent<FitnessEquipmentDisplay>().elapsedTime * ((0.6309f * ( (t_heartrate) + 0.19988f * weightKg + 0.2017f * age - 20.4022f) / 4.184f)) / 100;
            float currentTime = fec.GetComponent<FitnessEquipmentDisplay>().elapsedTime;
            float duration = (currentTime - startTime) / 3600f; // Dauer in Stunden

            float met = CalculateMET(t_power, t_heartrate, age, gender);
            float currentCalories = CalculateCaloriesBurned(weightKg, met, duration) - caloriesRealBurned;

            caloriesRealBurned += currentCalories / 2;
            //Debug.Log("Verbrannte Kalorien: " + caloriesRealBurned.ToString("F2"));

            yield return new WaitForSeconds(updateInterval);
        }
    }
    float CalculateMET(float watt, float heartRate, int age, string gender)
    {
        float met;

        if (gender == "m")
        {
            met = (0.2017f * age) + (0.6309f * heartRate) - (0.09036f * watt) - 55.0969f;
        }
        else
        {
            met = (0.074f * age) + (0.4472f * heartRate) - (0.05741f * watt) - 20.4022f;
        }

        return Mathf.Max(met / 3.5f, 1f); // Sicherstellen, dass der MET-Wert nicht unter 1 liegt
    }

    float CalculateCaloriesBurned(float weight, float met, float duration)
    {
        return met * weight * duration;
    }


    //information and gimmicks time based
    // Move Example
    public GameObject weatherMove;
    public GameObject tvLogoMove;
    public GameObject ticker;
    void MoverOfStuff()
    {
        LeanTween.move(weatherMove, weatherMove.transform.position + new Vector3(339f, 0f, 1f), 2f).setEase(LeanTweenType.easeInQuad).setDelay(10f);
        // Delay
        LeanTween.move(weatherMove, weatherMove.transform.position + new Vector3(0f, 0f, 1f), 2f).setDelay(600f);


        LeanTween.move(tvLogoMove, tvLogoMove.transform.position + new Vector3(339f, 0f, 1f), 2f).setEase(LeanTweenType.easeInQuad).setDelay(10f);
        // Delay
        LeanTween.move(tvLogoMove, tvLogoMove.transform.position + new Vector3(0f, 0f, 1f), 2f).setDelay(600f);

       // LeanTween.move(ticker, ticker.transform.position + new Vector3(-22339f, 0f, 1f), 150f).setDelay(10f);
       
    }

    // Funktion zum Wechseln der Kameras
    void SwitchCameras()
    {
        // Wenn Kamera 1 aktiviert ist, schalte auf Kamera 2 und umgekehrt
        if (camera1.activeSelf)
        {
            camera1.SetActive(false);
            camera2.SetActive(true);
            bike.gameObject.GetComponent<Animator>().enabled = false;
            rider.gameObject.GetComponent<Animator>().enabled = false;

        }
        else
        {
            camera1.SetActive(true);
            camera2.SetActive(false);
            bike.gameObject.GetComponent<Animator>().enabled = true;
            rider.gameObject.GetComponent<Animator>().enabled = true;
        }
    }

    // Methode zur Berechnung der Normalized Power
    float CalculateNormalizedPower()
    {
        if (powerData.Count < averagingInterval)
        {
            return 0f;  // Zu wenige Daten für die Berechnung eines 30-Sekunden-Durchschnitts
        }

        // Berechne den gleitenden 30-Sekunden-Durchschnitt (Beachte: nur die letzten 30 Sekunden verwenden)
        List<float> last30SecondsPower = powerData.GetRange(powerData.Count - averagingInterval, averagingInterval);

        // Berechne die 4. Potenz des Durchschnitts jeder Leistung
        float avgPower4th = 0f;
        foreach (float power in last30SecondsPower)
        {
            avgPower4th += Mathf.Pow(power, 4);
        }

        avgPower4th /= averagingInterval;

        // Berechne die Normalized Power (4. Wurzel des Durchschnitts der 4. Potenz)
        float normalizedPower = Mathf.Pow(avgPower4th, 0.25f);

        return normalizedPower;
    }

    // Methode zur Berechnung des TSS-Werts
    /*float CalculateTSS(float NP, float FTP, float timeInSeconds)
    {
        float IF = NP / FTP;  // Intensitätsfaktor
        float timeInHours = timeInSeconds / 3600f;  // Zeit in Stunden umrechnen
        float TSS = (timeInHours * NP * IF) / FTP * 100;
        return TSS;
    }*/
    // Methode zur Berechnung des TSS-Inkrements basierend auf der Zeitdifferenz
    float CalculateTSSIncrement(float NP, float FTP, float deltaTime)
    {
        if (FTP == 0 || NP == 0) return 0f;  // Vermeidung von Division durch null

        float IF = NP / FTP;  // Intensitätsfaktor
        float timeInHours = deltaTime / 3600f;  // Zeit in Stunden umrechnen
        float TSSIncrement = (timeInHours * NP * IF) / FTP * 100;

        return TSSIncrement;
    }       

    // Methode zur Sammlung der Leistungsdaten in jedem Frame
    void CollectPowerData(float power)
    {
        // Neue Leistungsdaten zur Liste hinzufügen
        powerData.Add(power);

        // Um Speicher zu sparen, nur die letzten 30 Sekunden an Daten behalten
        if (powerData.Count > averagingInterval)
        {
            powerData.RemoveAt(0);  // Entfernt älteste Einträge
        }
    }

    /*
    float GetPowerData()
    {

        return t_power;
    }

    void UpdatePowerGraph()
    {
        lineRenderer.positionCount = powerData.Count;

        for (int i = 0; i < powerData.Count; i++)
        {
            float power = powerData[i];
            Vector3 pos = new Vector3(i * xOffset, power / ftpValue, 0); // x und y-Wert der Linie
            lineRenderer.SetPosition(i, pos);

            // Berechne den Farbanteil basierend auf der aktuellen Power und dem FTP
            float relativePower = power / ftpValue;
            Color color = GetColorForPower(relativePower);
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
        }
    }
    */
    /*
    Color GetColorForPower(float relativePower)
    {
        // Berechne den Farbwert basierend auf den 7 FTP-Zonen
        if (relativePower < 0.55f)
            return Color.blue; // Zone 1: Erholung (Blau)
        else if (relativePower < 0.75f)
            return Color.green; // Zone 2: Ausdauer (Grün)
        else if (relativePower < 0.90f)
            return Color.yellow; // Zone 3: Tempo (Gelb)
        else if (relativePower < 1.05f)
            return new Color(1.0f, 0.64f, 0.0f); // Zone 4: Schwellenleistung (Orange)
        else if (relativePower < 1.20f)
            return Color.red; // Zone 5: VO2 Max (Rot)
        else if (relativePower < 1.50f)
            return Color.magenta; // Zone 6: Anaerobe Kapazität (Magenta)
        else
            return new Color(0.5f, 0.0f, 0.0f); // Zone 7: Neuromuskulär (Dunkelrot)
    }
    */

    /*  float CalculateTSS()
      {
          // Berechnung des Intensitätsfaktors (IF)
          float intensityFactor = t_power / ftpValue;

          // TSS berechnen
          return (gameTimer/60f * intensityFactor * intensityFactor) * 100;
      }*/

    float CalculateRecoveryTime(float tss, float hrIntensityFactor)
    {
        // Dynamische Berechnung: Grundwert von 12 Stunden + zusätzlicher Faktor basierend auf TSS
        float baseRecoveryTime = 12f + (tss / 50f) * 12f;  // Skaliert die Erholungszeit flexibel bis zu 72 Stunden

        // Herzfrequenz-Intensitätsfaktor anpassen (0.7 ist die Basis, kann angepasst werden)
        float adjustedRecoveryTime = baseRecoveryTime * (1 + hrIntensityFactor - 0.7f);

        // Begrenzen der Erholungszeit auf maximal 72 Stunden
        return Mathf.Min(adjustedRecoveryTime, 72f);
    
}
}







