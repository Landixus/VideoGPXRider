IMPORTANT: 
move the AndroidManifest.xml file from the Assets/AdvancedAnt/Plugins/Android folder to Assets/plugins/Android folder
If you are using any other pluging like VR, make sure to merge the services and activities

---

*set the minimum API level to Android 5.0 (API LEVEL 21)*

-----
The Android device must have the ANT radio service installed (a popup will ask the user to download it from google play if not installed, when you launch your app)
I have added all APK in a zip file at the root of the package

To debug on windows you can use the Android_PC_AndroidEmulatorBridge found in the ANT android SDK on thisisant.com ( you will need to install Android_ANTEmulatorConfig_2-0-0.apk on the device)

-----

The android plugin can use the built-in device selection UI by setting the useAndroidUI boolean to true on the prefabs.

If you use multiple prefabs with autoconnect, the java plugin will queue the requests and connect to the sensor one by one.

If you need any specific event from a device not implemented yet by the plugin, send me an email and I will add it. 
Here are the events the android plugin forwards to Unity for each sensor:

All sensors :isDisconnected , StateChange , ConnectEvent?

Cadence sensor: Receive_calculatedCadence

Speed & Cadence sensor: Receive_calculatedCadence?, Receive_calculatedSpeed, Receive_CalculatedAccumulatedDistance

Speed sensor: Receive_calculatedSpeed, Receive_CalculatedAccumulatedDistance

Power Meter sensor: Receive_calculatedPower, Receive_calculatedCadence?

Heart Rate sensor: Receive_computedHeartRate?

Fitness Equipment: Receive_instantaneousHeartRate, Receive_elapsedTime, Receive_instantaneousSpeed, Receive_cumulativeDistance?, Receive_trainer_instantaneousPowe, Receive_trainer_instantaneousCadence
SetTrainerResistance(resitance %)
SetTrainerTargetPower(power)
RequestTrainerCapabilities​
SetTrainerUserConfiguration

Minimum Android version: 5.0.1

The AndroidManifest.xml file has to be in the Assets\plugins\Android

---- 
If you set the device ID in a prefab, the plugin will attempt to connect to this device without using the Android ANT+ UI
You can also scan for device ID by filtering device type.
the AndroidMultipleDeviceSearch Gameobject in the  AntmultipleScan scene is set to look for speedAndCadence sensor and will connect to the first ID it found with SpeedCadenceDisplay prefab.

activity.Call("start_multiple_device_search",this.gameObject.name,speedCadence,hr,cadence,speed,power,fec);