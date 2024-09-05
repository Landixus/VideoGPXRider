using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using ANT_Managed_Library;
using System.Collections.Generic;
using System;

/*
 * ContinouScanExample
 *
 * With continuous scan you can connect up to ~75 sensors with a single usb dongle
 * open a channel with a given radio frequency  AntManager.Instance.OpenContinuousScanChannel(57); 
 * and then filter the incomming device type and ID in the Receive Data event
 */


//Speed sensor object, for every new device ID that match the speed sensor type, we create a new SpeedSensor
public class SpeedSensor {
    public int stopRevCounter_speed;
    public int prev_measTime_speed;
    public int prev_revCount_speed;
    public int revCountZero;
    public float wheelCircumference = 2.096f;
    public float speed;
    public int deviceID;

    public float GetSpeed(Byte[] data) {
        

        //SPEED
        int measTime_speed = (data[5]) | data[6] << 8;
        int revCount_speed = (data[7]) | data[8] << 8;



        if (prev_measTime_speed != 0 && measTime_speed != prev_measTime_speed && prev_measTime_speed < measTime_speed && prev_revCount_speed < revCount_speed) {
            
            speed = (wheelCircumference * (revCount_speed - prev_revCount_speed) * 1024) / (measTime_speed - prev_measTime_speed);
            speed *= 3.6f; // km/h
            stopRevCounter_speed = 0;

        } else
            stopRevCounter_speed++;

        if (stopRevCounter_speed >= 5) {
            stopRevCounter_speed = 5;
            speed = 0;
        }


        prev_measTime_speed = measTime_speed;
        prev_revCount_speed = revCount_speed;
        return speed;
    }
}

public class ContinousScanExample : MonoBehaviour {

    List<SpeedSensor> speedSensorList;
    // Use this for initialization
    void Start () {

        if (AntManager.Instance.device == null) {
            AntManager.Instance.Init();
          //  AntManager.Instance.onDeviceResponse += OnDeviceResponse;
          //  AntManager.Instance.onSerialError += OnSerialError; //if usb dongle is unplugged for example
        }

        AntChannel scanChannel = AntManager.Instance.OpenContinuousScanChannel(57); 
       // scanChannel.onChannelResponse += OnChannelResponse;
        scanChannel.onReceiveData += ReceiveContinuouScanData;
    }


    void ReceiveContinuouScanData(Byte[] data) {

        if (speedSensorList == null)
            speedSensorList = new List<SpeedSensor>();
        // first byte is the channel ID, 0
        int pageNumber = data[1] >> 1;
        //device number to filter devices
        int deviceNumber = ((data[10]) | data[11] << 8);
        int deviceType = (data[12]);
           
        SpeedSensor sensor = null;
        foreach (SpeedSensor s in speedSensorList) {
            if (s.deviceID == deviceNumber) {
                //WARNING  Byte[] data contains the channel ID in the first byte, shift the payload bytes from the array accordingly
                Debug.Log("speed for sensor #" + s.deviceID + ": " + s.GetSpeed(data));
                break;
            }
        }
        //if sensor object not created and is of correct type, create and store in list
        if (sensor == null && deviceType == AntplusDeviceType.BikeSpeed) {
            //found new sensor
            sensor = new SpeedSensor();
            sensor.deviceID = deviceNumber;
            speedSensorList.Add(sensor);
        }



    }


}
