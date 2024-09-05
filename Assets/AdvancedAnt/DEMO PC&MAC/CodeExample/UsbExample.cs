using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using ANT_Managed_Library;
using System.Collections.Generic;
using System;
using System.IO;

/*
 * UsbExample
 *
 * you can specify the USB number in init_Device(int USBnum);
 * OpenChannel last argument is the device number, default to 0
 */


public class UsbExample : MonoBehaviour {

    uint nDeviceConnected = 0;
    void Start() {
     
        //INIT HR Display on usb device 0 and 1
        init_Device(0);
        AntChannel c = AntManager.Instance.OpenChannel(ANT_ReferenceLibrary.ChannelType.BASE_Slave_Receive_0x00, 0, 0, 120, 0, 57, 8070, false, 0); 
        c.onChannelResponse += OnChannelResponse;
        c.onReceiveData += USB0;
        c.hideRXFAIL = true;

        init_Device(1);
        AntChannel d = AntManager.Instance.OpenChannel(ANT_ReferenceLibrary.ChannelType.BASE_Slave_Receive_0x00, 0, 0, 120, 0, 57, 8070, false, 1);
        d.onChannelResponse += OnChannelResponse;
        d.onReceiveData += USB1;
        d.hideRXFAIL = true;
    }

    void USB0(Byte[] data) {
        Debug.Log("USB 0 received data");
    }
    void USB1(Byte[] data) {
        Debug.Log("USB 1 received data");
    }

    bool firstDeviceInitialized = false;
    void init_Device(byte USBDeviceNum) {
        if (AntManager.Instance.devices[USBDeviceNum] == null) {
            AntManager.Instance.Init(USBDeviceNum);

            if (firstDeviceInitialized == false) { // device events need to be added only once
                firstDeviceInitialized = true;
                AntManager.Instance.onDeviceResponse += OnDeviceResponse;
                AntManager.Instance.onSerialError += OnSerialError; //if usb dongle is unplugged for example
            }

        }

    }


    void OnDeviceResponse(ANT_Response response) {
        ANT_Device device = response.sender as ANT_Device;
        Debug.Log("device (usb " + device.getOpenedUSBDeviceNum() + ") : " + response.getMessageID().ToString());
    }

    void OnSerialError(SerialError serialError) {
        Debug.Log("Error:" + serialError.error.ToString());

        //attempt to auto reconnect if the USB was unplugged
        if (serialError.error == ANT_Device.serialErrorCode.DeviceConnectionLost) {
            foreach (AntChannel channel in AntManager.Instance.channelList) {
                if (channel.device == serialError.sender)
                    channel.PauseChannel();
            }
            nDeviceConnected = ANT_Common.getNumDetectedUSBDevices();
            StartCoroutine("Reconnect", serialError.sender.getSerialNumber());
        }

    }

    IEnumerator Reconnect(uint serial) {
        Debug.Log("looking for usb device " + serial.ToString());
        // polling to try and find the USB device
        while (true) {

            if (ANT_Common.getNumDetectedUSBDevices() > nDeviceConnected) {



                ANT_Device device = new ANT_Device();
                if (device.getSerialNumber() == serial) {
                    Debug.Log("usb found!");
                    AntManager.Instance.Reconnect(device);
                    foreach (AntChannel channel in AntManager.Instance.channelList)
                        channel.ReOpen(device);

                    yield break;
                } else
                    device.Dispose();

            }

            yield return new WaitForSeconds(0.1f);
        }

    }

    void OnChannelResponse(ANT_Response response) {
        

    }
    

}
