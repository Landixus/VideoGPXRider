using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using ANT_Managed_Library;
using System.Collections.Generic;
using System;



public class AntDemo : MonoBehaviour
{
    //output ant responses and data to unity UI text
    public Text dataDisplay;
    List<string> logDisplay;

    AntChannel hrChannel;
    AntChannel speedChannel;
    AntChannel backgroundScanChannel;

    //variable use for speed display
    int prevRev;
    int prevMeasTime = 0;

    //fake HRM beat
    byte hbCount = 0;

    void Start()
    {
        logDisplay = new List<string>();
        dataDisplay.text = "";
        //EncodeFitFile(); //the FIT library is included to encode and decode FIT files
    }

    void init_Device()
    {
        if (AntManager.Instance.device == null)
        {
            AntManager.Instance.Init();
            AntManager.Instance.onDeviceResponse += OnDeviceResponse;
            AntManager.Instance.onSerialError += OnSerialError; //if usb dongle is unplugged for example
        }

    }

    public void Onclick_Init_HRSensor()
    {
        init_Device();

        //call the ConfigureAnt method for a master channel with the correct ANT speed display settings 
        //found in ANT_Device_Profile_Heart_Rate_Monitor.pdf on thisisant.com
        //using channel 1
        hrChannel = AntManager.Instance.OpenChannel(ANT_ReferenceLibrary.ChannelType.BASE_Master_Transmit_0x10, 1, 255, 120, 0, 57, 8070, false); //hr sensor
        hrChannel.onChannelResponse += OnChannelResponse;

        InvokeRepeating("HRMTransmit", 1, 1 / 4f); // let's fake a HRM by updating a HR beat every 1/4 second
    }
    public void Onclick_Init_SpeedDisplay()
    {
        init_Device();

        //call the ConfigureAnt method for a slave channel with the correct ANT speed display settings 
        //found in ANT+_Device_Profile_-_Bicycle_Speed_and_Cadence_2.0.pdf on thisisant.com
        //using channel 2
        //note: collision is a normal behaviour searching for a master.

        speedChannel = AntManager.Instance.OpenChannel(ANT_ReferenceLibrary.ChannelType.BASE_Slave_Receive_0x00, 2, 0, 123, 0, 57, 8118, false); //bike speed Display
        speedChannel.onChannelResponse += OnChannelResponse;
        speedChannel.onReceiveData += ReceivedSpeedAntData;

    }

    public void Onclick_Start_BackgroundScan()
    {
        init_Device();
        backgroundScanChannel = AntManager.Instance.OpenBackgroundScanChannel(0);
        backgroundScanChannel.onReceiveData += ReceivedBackgroundScanData;
        backgroundScanChannel.onChannelResponse += OnChannelResponse;

    }


    public void Onclick_Close()
    {

        if (hrChannel)
        {
            hrChannel.onChannelResponse -= OnChannelResponse;
            hrChannel.Close();
            CancelInvoke("HRMTransmit");
            hrChannel = null;

        }
        if (speedChannel)
        {
            speedChannel.onReceiveData -= ReceivedSpeedAntData;
            speedChannel.Close();
            speedChannel = null;
        }
        if (backgroundScanChannel)
        {
            backgroundScanChannel.onReceiveData -= ReceivedBackgroundScanData;
            backgroundScanChannel.onChannelResponse -= OnChannelResponse;
            backgroundScanChannel.Close();
            backgroundScanChannel = null;
        }

        StopCoroutine("Reconnect");
    }


    void OnDeviceResponse(ANT_Response response)
    {
        DisplayText("device:" + response.getMessageID().ToString());
    }

    void OnSerialError(SerialError serialError)
    {
        DisplayText("Error:" + serialError.error.ToString());

        //attempt to auto reconnect if the USB was unplugged
        if (serialError.error == ANT_Device.serialErrorCode.DeviceConnectionLost)
        {
            foreach (AntChannel channel in AntManager.Instance.channelList)
                channel.PauseChannel();

            StartCoroutine("Reconnect", serialError.sender.getSerialNumber());
        }

    }

    IEnumerator Reconnect(uint serial)
    {
        Debug.Log("looking for usb device " + serial.ToString());
        // polling to try and find the USB device
        while (true)
        {

            if (ANT_Common.getNumDetectedUSBDevices() > 0)
            {
                ANT_Device device = new ANT_Device();
                if (device.getSerialNumber() == serial)
                {
                    Debug.Log("usb found!");
                    AntManager.Instance.Reconnect(device);
                    foreach (AntChannel channel in AntManager.Instance.channelList)
                        channel.ReOpen();

                    yield break;
                }
                else
                    device.Dispose();

            }

            yield return new WaitForSeconds(0.1f);
        }

    }

    void OnChannelResponse(ANT_Response response)
    {

        if (response.getChannelEventCode() == ANT_ReferenceLibrary.ANTEventID.EVENT_RX_FAIL_0x02)
            DisplayText("channel " + response.antChannel.ToString() + " " + "RX fail");
        else if (response.getChannelEventCode() == ANT_ReferenceLibrary.ANTEventID.EVENT_RX_FAIL_GO_TO_SEARCH_0x08)
            DisplayText("channel " + response.antChannel.ToString() + " " + "Go to search");
        else if (response.getChannelEventCode() == ANT_ReferenceLibrary.ANTEventID.EVENT_TX_0x03)
            DisplayText("channel " + response.antChannel.ToString() + " " + "Tx: (" + response.antChannel.ToString() + ")" + BitConverter.ToString(hrChannel.txBuffer));
        else
            DisplayText("channel " + response.antChannel.ToString() + " " + response.getChannelEventCode());

    }

    void HRMTransmit()
    {
        hbCount++;
        //super simple HRM
        hrChannel.txBuffer[0] = 0;
        hrChannel.txBuffer[1] = 0;
        hrChannel.txBuffer[2] = 0;
        hrChannel.txBuffer[3] = 0;
        hrChannel.txBuffer[4] = 0;
        hrChannel.txBuffer[5] = 0;
        hrChannel.txBuffer[6] = hbCount;
        hrChannel.txBuffer[7] = (byte)UnityEngine.Random.Range(90, 100);

    }

    void ReceivedSpeedAntData(Byte[] data)
    {
        //output the data to our log window
        string dataString = "RX: ";
        foreach (byte b in data)
            dataString += b.ToString("X") + " ";
        DisplayText(dataString);

        //speed formula as described in the ant+ device profile doc
        int currentRevCount = (data[6]) | data[7] << 8;

        if (currentRevCount != prevRev && prevRev > 0)
        {
            int currentMeasTime = (data[4]) | data[5] << 8;
            float speed = (2.070f * (currentRevCount - prevRev) * 1024) / (currentMeasTime - prevMeasTime);
            speed *= 3.6f;
            prevMeasTime = currentMeasTime;
            DisplayText("speed: " + speed.ToString("F2") + "km/h");
        }

        prevRev = currentRevCount;


    }

    void ReceivedBackgroundScanData(Byte[] data)
    {

        byte deviceType = (data[12]); // extended info Device Type byte
                                      //use the Extended Message Formats to identify nodes
        Debug.Log("scan found ID : "+ ((data[10]) | data[11] << 8)+ "of TYPE: "+ deviceType);
        switch (deviceType)
        {
            case AntplusDeviceType.Antfs:
                {
                    DisplayText("Scan Found Antfs");
                    break;
                }
            case AntplusDeviceType.BikePower:
                {
                    DisplayText("Scan Found BikePower");
                    break;
                }
            case AntplusDeviceType.EnvironmentSensorLegacy:
                {
                    DisplayText("Scan Found EnvironmentSensorLegacy");
                    break;
                }
            case AntplusDeviceType.MultiSportSpeedDistance:
                {
                    DisplayText("Scan Found MultiSportSpeedDistance");
                    break;
                }
            case AntplusDeviceType.Control:
                {
                    DisplayText("Scan Found Control");
                    break;
                }
            case AntplusDeviceType.FitnessEquipment:
                {
                    DisplayText("Scan Found FitnessEquipment");
                    break;
                }
            case AntplusDeviceType.BloodPressure:
                {
                    DisplayText("Scan Found BloodPressure");
                    break;
                }
            case AntplusDeviceType.GeocacheNode:
                {
                    DisplayText("Scan Found GeocacheNode");
                    break;
                }
            case AntplusDeviceType.LightElectricVehicle:
                {
                    DisplayText("Scan Found LightElectricVehicle");
                    break;
                }
            case AntplusDeviceType.EnvSensor:
                {
                    DisplayText("Scan Found EnvSensor");
                    break;
                }
            case AntplusDeviceType.Racquet:
                {
                    DisplayText("Scan Found Racquet");
                    break;
                }
            case AntplusDeviceType.ControlHub:
                {
                    DisplayText("Scan Found ControlHub");
                    break;
                }
            case AntplusDeviceType.MuscleOxygen:
                {
                    DisplayText("Scan Found MuscleOxygen");
                    break;
                }
            case AntplusDeviceType.BikeLightMain:
                {
                    DisplayText("Scan Found BikeLightMain");
                    break;
                }
            case AntplusDeviceType.BikeLightShared:
                {
                    DisplayText("Scan Found BikeLightShared");
                    break;
                }
            case AntplusDeviceType.BikeRadar:
                {
                    DisplayText("Scan Found BikeRadar");
                    break;
                }
            case AntplusDeviceType.WeightScale:
                {
                    DisplayText("Scan Found WeightScale");
                    break;
                }
            case AntplusDeviceType.HeartRate:
                {
                    DisplayText("Scan Found HeartRate");
                    break;
                }
            case AntplusDeviceType.BikeSpeedCadence:
                {
                    DisplayText("Scan Found BikeSpeedCadence");
                    break;
                }
            case AntplusDeviceType.BikeCadence:
                {
                    DisplayText("Scan Found BikeCadence");
                    break;
                }
            case AntplusDeviceType.BikeSpeed:
                {
                    DisplayText("Scan Found BikeSpeed");
                    break;
                }
            case AntplusDeviceType.StrideSpeedDistance:
                {
                    DisplayText("Scan Found StrideSpeedDistance");
                    break;
                }
            default:
                {
                    DisplayText("Scan Found Found Unknown device");
                    break;
                }
        }

    }

   
    //Display content of the string list to the UI text
    void DisplayText(string s)
    {
        dataDisplay.text = "";
        logDisplay.Add(s + "\n");
        if (logDisplay.Count > 13)
            logDisplay.RemoveAt(0);

        foreach (string log in logDisplay)
            dataDisplay.text += log;
    }




}
