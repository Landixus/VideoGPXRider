using UnityEngine;

public class DataManager : MonoBehaviour
{
    // ANT+ Values come from HeartRateDisplay.cs and FitnessEquipmentDisplay.cs
    // We need to bring the Data also here from Bluetooth Devices to have a CentralDataDeviceManager for the values
    // If the user decide for BT only BT values are read from fitness machine
    // If the user have a cheststrap ANT+ and a Fitness machine BT we have a conflict :) Need to think about it. But today most have BT or ANT+ together

    public static DataManager Instance { get; private set; }

    public float SpeedKmh { get; private set; }
    public float DistanceKm { get; private set; }
    public float PowerWatts { get; private set; }
    public float CadenceRounds { get; private set; }
    public int HeartRate { get; private set; }

    public float TotalDistanceKm { get; private set; }

    public float RemainDistanceKm { get; private set; }

    public float HeightMeter { get; private set; }

    public float RemainHeightMeter { get; private set; }

    public float Slope { get; private set; }

   // private HeartRateConnector hrConnector;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Behalte das Objekt beim Szenenwechsel

        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void UpdateSpeed(float speedKmh)
    {
        //if data comes form BT we decide here what to use?
        SpeedKmh = speedKmh;
    }

    public void UpdateDistance(float distanceKm)
    {
        TotalDistanceKm = distanceKm;
    }

    public void UpdatePower(int watts)
    {
        //if data comes form BT we decide here what to use?
        PowerWatts = watts;
    }
    public void Updatecadence(int cadence)
    {
        //if data comes form BT we decide here what to use?
        CadenceRounds = cadence;
    }

    public void UpdateHeartRate(int bpm)
    {
        //if data comes form BT we decide here what to use?
        HeartRate = bpm;
    }

    public void UpdateRemainDistance(float remainDistanceKm)
    {
        RemainDistanceKm = remainDistanceKm;
    }

    public void UpdateRemainHeightMeter(float remainHeightMeter)
    {
        RemainHeightMeter = remainHeightMeter;
    }

    public void UpdateSlope(float slope)
    {
        Slope = slope;
    }


    // Konvertierungen
    //
    public float GetSpeedMph() => SpeedKmh * 0.621371f;
    public float GetSpeedKmh() => SpeedKmh;
    //  
    public float GetPower() => PowerWatts;
    //
    public float GetTotalDistanceKm() => TotalDistanceKm;
    public float GetTotalDistanceMi() => TotalDistanceKm * 0.621371f;
    //
    //
    public float GetDistanceRemainKm() => RemainDistanceKm * 2;
    public float GetDistanceRemainMi() => RemainDistanceKm * 0.621371f;

    public float GetRemainHeightMeter() => RemainHeightMeter;
    public float GetRemainHmtoMiles() => RemainHeightMeter * 0.621371f; // This converts km to mi but is height meter in km or m?

    public float GetSlope() => Slope;


    public float GetHeartRate() => HeartRate;
    public float GetCadence() => CadenceRounds;
    public float GetWKG() => PowerWatts / PlayerPrefs.GetInt("UserWeight", 80);

    //Missing Km_to_Finish + Miles_to_Finish
    //Missing Distance
    //Missing Distance_to_2_and_3_place




}


