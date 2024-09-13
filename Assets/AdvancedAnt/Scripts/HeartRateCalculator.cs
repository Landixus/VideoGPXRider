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

        //for Speed
        if (speedVals.Count > 10000)  //Remove the oldest when we have more than 10
        {
            speedVals.RemoveAt(0);
        }
        float spd_total = 0f;
        foreach (float f in speedVals)  //Calculate the total of all floats
        {
            spd_total += f;
        }
        float spd_average = spd_total / (float)speedVals.Count;  //average is of course the total divided by the number of floats
        //
        //for Power
        if (pwrVals.Count > 10000)  //Remove the oldest when we have more than 10
        {
            pwrVals.RemoveAt(0);
        }
        float pwr_total = 0f;
        foreach (float f in pwrVals)  //Calculate the total of all floats
        {
            pwr_total += f;
        }
        float pwr_average = pwr_total / (float)pwrVals.Count;  //average is of course the total divided by the number of floats
        //
        //for Cadence
        if (cadVals.Count > 10000)  //Remove the oldest when we have more than 10
        {
            cadVals.RemoveAt(0);
        }
        float cad_total = 0f;
        foreach (float f in cadVals)  //Calculate the total of all floats
        {
            cad_total += f;
        }
        float cad_average = cad_total / (float)cadVals.Count;  //average is of course the total divided by the number of floats
        //
        //for Speed

        if (hrVals.Count > 10000)  //Remove the oldest when we have more than 10
        {
            hrVals.RemoveAt(0);
        }
        float hr_total = 0f;
        foreach (float f in hrVals)  //Calculate the total of all floats
        {
            hr_total += f;
        }
        float hr_average = hr_total / (float)hrVals.Count;  //average is of course the total divided by the number of floats
        //
        AuiText_Avg_SPEED_VALUE.text = spd_average.ToString("F0");
        AuiText_Avg_PWR_Value.text = pwr_average.ToString("F0");
        AuiText_Avg_CAD_Value.text = cad_average.ToString("F0");
        AuiText_Avg_HR_Value.text = hr_average.ToString("F0");


        uiText_Avg_SPEED_Value.text = (spd_average).ToString("F0");
        uiText_Avg_PWR_Value.text = pwr_average.ToString("F0");
        uiText_Avg_CAD_Value.text = cad_average.ToString("F0");
        uiText_Avg_HR_Value.text = hr_average.ToString("F0");
        

    //   Debug.Log("Status :"+status );
    }

    

}