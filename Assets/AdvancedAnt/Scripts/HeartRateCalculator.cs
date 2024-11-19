using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class HeartRateCalculator : MonoBehaviour
{
   
    public TMP_Text uiText_Avg_HR_Value;
    public TMP_Text uiText_Avg_PWR_Value;
    public TMP_Text uiText_Avg_CAD_Value;
    public TMP_Text uiText_Avg_SPEED_Value;
    public TMP_Text AuiText_Avg_SPEED_VALUE;
    public TMP_Text AuiText_Avg_HR_Value;
    public TMP_Text AuiText_Avg_PWR_Value;
    public TMP_Text AuiText_Avg_CAD_Value;

    public TMP_Text AuiText_MAX_SPEED_VALUE;
    public TMP_Text AuiText_MAX_HR_Value;
    public TMP_Text AuiText_MAX_PWR_Value;
    public TMP_Text AuiText_MAX_CAD_Value;
    /*
    private float heartRateFromDevice;
    private int pwrFromDevice;
    private int cadFromDevice;
    private float speedFromDevice;
    */
    public bool connectionFECActive = false; // Angenommen, dies steuert die Verbindungsaktivität
    public bool connectionHRActive = false; // Angenommen, dies steuert die Verbindungsaktivität

    public List<float> speedVals = new List<float>();
    public List<float> pwrVals = new List<float>();
    public List<float> cadVals = new List<float>();
    public List<float> hrVals = new List<float>();

    public float hr_average;



    private void Start()
    {
        InvokeRepeating("getAverage", 2.0f, 1.0f);
    }

    public void getAverage()
    {
        speedVals.Add(GameObject.Find("FitnessEquipmentDisplay").GetComponent<FitnessEquipmentDisplay>().speed);
        pwrVals.Add(GameObject.Find("FitnessEquipmentDisplay").GetComponent<FitnessEquipmentDisplay>().instantaneousPower);
        cadVals.Add(GameObject.Find("FitnessEquipmentDisplay").GetComponent<FitnessEquipmentDisplay>().cadence);
        hrVals.Add(GameObject.Find("HeartRateDisplay").GetComponent<HeartRateDisplay>().heartRate);

        if (speedVals.Count > 10000)
        {
            speedVals.RemoveAt(0);
        }
        float spd_total = 0f;
        float maxSpeed = float.MinValue; // Initialisiere mit dem kleinstmöglichen Wert
        foreach (float f in speedVals)
        {
            spd_total += f;
            if (f > maxSpeed)
            {
                maxSpeed = f; // Aktualisiere den größten Wert
            }
        }
        float spd_average = spd_total / (float)speedVals.Count;

        // Handling for Power
        if (pwrVals.Count > 10000)
        {
            pwrVals.RemoveAt(0);
        }
        float pwr_total = 0f;
        float maxPower = float.MinValue;
        foreach (float f in pwrVals)
        {
            pwr_total += f;
            if (f > maxPower)
            {
                maxPower = f;
            }
        }
        float pwr_average = pwr_total / (float)pwrVals.Count;

        // Handling for Cadence
        if (cadVals.Count > 10000)
        {
            cadVals.RemoveAt(0);
        }
        float cad_total = 0f;
        float maxCadence = float.MinValue;
        foreach (float f in cadVals)
        {
            cad_total += f;
            if (f > maxCadence)
            {
                maxCadence = f;
            }
        }
        float cad_average = cad_total / (float)cadVals.Count;

        // Handling for Heart Rate
        if (hrVals.Count > 10000)
        {
            hrVals.RemoveAt(0);
        }
        float hr_total = 0f;
        float maxHeartRate = float.MinValue;
        foreach (float f in hrVals)
        {
            hr_total += f;
            if (f > maxHeartRate)
            {
                maxHeartRate = f;
            }
        }
        hr_average = hr_total / (float)hrVals.Count;
        AuiText_Avg_SPEED_VALUE.text = spd_average.ToString("F0");
        AuiText_Avg_PWR_Value.text = pwr_average.ToString("F0");
        AuiText_Avg_CAD_Value.text = cad_average.ToString("F0");
        AuiText_Avg_HR_Value.text = hr_average.ToString("F0");


        uiText_Avg_SPEED_Value.text = (spd_average).ToString("F0");
        uiText_Avg_PWR_Value.text = pwr_average.ToString("F0");
        uiText_Avg_CAD_Value.text = cad_average.ToString("F0");
        uiText_Avg_HR_Value.text = hr_average.ToString("F0");

        AuiText_MAX_SPEED_VALUE.text = maxSpeed.ToString("F0");
        AuiText_MAX_PWR_Value.text = maxPower.ToString("F0");
        AuiText_MAX_CAD_Value.text = maxCadence.ToString("F0");
        AuiText_MAX_HR_Value.text = maxHeartRate.ToString("F0");


        //   Debug.Log("Status :"+status );
    }






}