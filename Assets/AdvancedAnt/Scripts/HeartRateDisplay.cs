using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using ANT_Managed_Library;
using System;

/*

    Frequencies such as 4.06 Hz (ANT+ Heart
Rate) and 4.005 Hz (ANT+ Bike Power) will
periodically “drift” into each other, or into
other channel periods that may be present
in the vicinity. During this overlap, channel
collisions may occur as the radio can only
service channel at a time. 

    */

public class HeartRateDisplay : MonoBehaviour {

    public bool autoStartScan = true; //start scan on play
    public bool connected = false; //will be set to true once connected

    //windows and mac settings
    public bool autoConnectToFirstSensorFound = false; //for windows and mac, either connect to the first sensor found or let you pick a sensor manually in the scanResult list with your own UI and call ConnectToDevice(AntDevice device)
    public List<AntDevice> scanResult;

    //android settings
    public bool useAndroidUI = true; //will open the unified ant+ UI on the android app if set to true, otherwise will connect to the first found device
    public bool skipPreferredSearch = true;  //- True = Don't automatically connect to user's preferred device, but always go to search for other devices.

    //the sensor values we receive fron the onReceiveData event
    public float heartRate; // the computed HR count  in BPM

    private AntChannel backgroundScanChannel;
    public AntChannel deviceChannel;
    public int deviceID = 0; //set this to connect to a specific device ID

  //  public TMP_Text  HR_Device_Found;
  

    void Start() {

      //  deviceID = PlayerPrefs.GetInt("HR_Value");
        if (autoStartScan)
            StartScan();
        DontDestroyOnLoad(gameObject);
       // Invoke("RequestCommandStatus", 2f);
    }

    //Start a background Scan to find the device
    public void StartScan() {

        Debug.Log("Looking for ANT + HeartRate sensor");


   #if UNITY_ANDROID && !UNITY_EDITOR
        
        //java: connect_heartrate(String gameobjectName, boolean useAndroidUI, boolean skipPreferredSearch, int deviceID)
        AndroidJNI.AttachCurrentThread();
        using (AndroidJavaClass javaClass = new AndroidJavaClass("com.ant.plugin.Ant_Connector")) {
            using (AndroidJavaObject activity = javaClass.GetStatic<AndroidJavaObject>("mContext")) {
                activity.Call("connect_heartrate", this.gameObject.name,useAndroidUI,skipPreferredSearch,deviceID);
            }
        }
#else


        AntManager.Instance.Init();
        scanResult = new List<AntDevice>();
        backgroundScanChannel = AntManager.Instance.OpenBackgroundScanChannel(0);
        backgroundScanChannel.onReceiveData += ReceivedBackgroundScanData;
#endif
    
    }

    public void Update()
    {

        if (DataManager.Instance == null) return;
      //  Invoke("RequestCommandStatus", 2f);
        DataManager.Instance.UpdateHeartRate((int)heartRate);

    }
    //Android functions
    void ANTPLUG_ConnectEvent(string resultCode) {
        switch (resultCode) {
            case "SUCCESS":
                connected = true;
                Debug.Log("HR MONITOR IS CONMNNECTED");
                break;
            case "CHANNEL_NOT_AVAILABLE":
                //Channel Not Available

                break;
            case "ADAPTER_NOT_DETECTED":
                //ANT Adapter Not Available. Built-in ANT hardware or external adapter required.
                Debug.Log("ANT Adapter Not Available. Built-in ANT hardware or external adapter required.");
                break;
            case "BAD_PARAMS":

                //Bad request parameters.

                break;
            case "OTHER_FAILUR":

                //RequestAccess failed. See logcat for details.

                break;
            case "DEPENDENCY_NOT_INSTALLED":
            //You need to install the ANT+ Plugins service or you may need to update your existing version if you already have it. 

            case "USER_CANCELLED":
                //USER_CANCELLED
                break;
            case "UNRECOGNIZED":
                //UNRECOGNIZED. PluginLib Upgrade Required?",

                break;
            default:
                //UNRECOGNIZED
                break;
        }
    }

    void ANTPLUG_StateChange(string newDeviceState) {
        switch (newDeviceState) {
            case "DEAD":
                connected = false;
                Debug.Log("HR MONITOR IS DEAD");
                break;
            case "CLOSED":
            
                break;
            case "SEARCHING":
                //searching
                break;
            case "TRACKING":
                //tracking
                break;
            case "PROCESSING_REQUEST":

                break;
            default:
                //UNRECOGNIZED
                break;
        }
    }
    void ANTPLUG_Receive_computedHeartRate(string s) {
        heartRate = (int)float.Parse(s);
    }

    //Windows and mac 
    //If the device is found 
    void ReceivedBackgroundScanData(Byte[] data) {

        byte deviceType = (data[12]); // extended info Device Type byte

        switch (deviceType) {

            case AntplusDeviceType.HeartRate: {
                    int deviceNumber = (data[10]) | data[11] << 8;
                 //   deviceID = PlayerPrefs.GetInt("HR_Value");
                    //int deviceNumber = deviceID;
                    byte transType = data[13];
                    foreach (AntDevice d in scanResult) {
                        if (d.deviceNumber == deviceNumber && d.transType == transType) //device already found
                            return;
                    }

                    Debug.Log("Heart rate sensor found " + deviceNumber);

                    AntDevice foundDevice = new AntDevice();
                    foundDevice.deviceType = deviceType;
                    foundDevice.deviceNumber = deviceNumber;
                    foundDevice.transType = transType;
                    foundDevice.period = 8070;
                    foundDevice.radiofreq = 57;
                    foundDevice.name = "heartrate(" + foundDevice.deviceNumber + ")";
                    scanResult.Add(foundDevice);
                    if (autoConnectToFirstSensorFound && foundDevice.deviceNumber == deviceID)
                    {
                        ConnectToDevice(foundDevice);
                    }
                  //  HR_Device_Found.text = "HR(" + foundDevice.deviceNumber.ToString() + ")";
                    break;
                }

            default: {

                    break;
                }
        }

    }

    public void ConnectToDevice(AntDevice device) {
        AntManager.Instance.CloseBackgroundScanChannel();
        byte channelID = AntManager.Instance.GetFreeChannelID();
        deviceChannel = AntManager.Instance.OpenChannel(ANT_ReferenceLibrary.ChannelType.BASE_Slave_Receive_0x00, channelID, (ushort)device.deviceNumber, device.deviceType, device.transType, (byte)device.radiofreq, (ushort)device.period, false);
        connected = true;
        deviceChannel.onReceiveData += Data;
        deviceChannel.onChannelResponse += ChannelResponse;
        
        deviceChannel.hideRXFAIL = true;
    }


    //Deal with the received Data
    public void Data(Byte[] data) {

        //HR
        heartRate = (data[7]);
        


    }



    void ChannelResponse(ANT_Response response) {


    }

}
