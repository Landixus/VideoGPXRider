using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using TMPro;
using System;
using System.Threading;



public class VideoSpeed : MonoBehaviour
{
    [SerializeField] public VideoPlayer videoPlayer;

    public float videoSpeed = 0f;
    public float maxVideoSpeed = 10f;
    public float minVideoSpeed = 0f;
    public FitnessEquipmentDisplay fec;
    public float fecSpeed;

    public string pathTotheVideo;
    public Button okButton;
    public KeyCode toggleKey = KeyCode.V;
    public KeyCode videoRotate = KeyCode.R;
    public RectTransform videoPlane;

    public AudioSource audioSource;
    public AudioListener audioListener;
    public KeyCode muteAudio = KeyCode.M;
    private bool muted = false;

    //For half the Video Speed
    public bool halfSpeed = false;
    public Button VSpeedButton;
    public Text  buttenText;
    public TMP_Text timeText;

    //GetTrackLength of GPX
    public ElevationMap elevationMap;
    public float checkInterval = 1f;
    private float speedMultiplier;
    private float referenceSpeed; // get video Length to have a reference speed
    private float distanceCalculated = 0f;
    //  private float speedCalculated = 0f;

    private float updateInterval = 0.25f; // Zeitintervall in Sekunden
    private float timeSinceLastUpdate = 0f;

    private void Start()
    {
        videoPlayer.isLooping = true;
        // Set mode to Audio Source.
        videoPlayer.audioOutputMode = VideoAudioOutputMode.Direct;

        // We want to control one audio track with the video player
        videoPlayer.controlledAudioTrackCount = 1;

        // We enable the first track, which has the id zero
        videoPlayer.EnableAudioTrack(0, true);

        // ...and we set the audio source for this track
        videoPlayer.SetTargetAudioSource(0, audioSource);
        //videoPlayer.url = pathTotheVideo;

        //video set to halfSpeed if MotorCam was used
        halfSpeed = false;

        InvokeRepeating(nameof(AdjustSpeed), 2, checkInterval);
        InvokeRepeating(nameof(GetRefSpeed), 2, 5);




    }

    // Update is called once per frame
    void Update()
    {

        timeSinceLastUpdate += Time.deltaTime;

        // Überprüfe, ob der Timer das Intervall erreicht hat
        if (timeSinceLastUpdate >= updateInterval)
        {
            // Setze den Timer zurück
            timeSinceLastUpdate = 0f;

            if (fec.GetComponent<FitnessEquipmentDisplay>().speed >= 1)
            {
                float speed = fec.GetComponent<FitnessEquipmentDisplay>().speed;
                Debug.Log("FECSpeed" + fec.GetComponent<FitnessEquipmentDisplay>().speed);
                //get video length to calculate a reference speed. 
                speed /= referenceSpeed;
                videoPlayer.playbackSpeed = speed * speedMultiplier;

                //  Debug.Log("RefSpeed" + referenceSpeed.ToString());
            }
        }
       

        if (Input.GetKeyDown(videoRotate))
        {

            if (videoPlane != null)
            {
                // Rotiere das RectTransform-Objekt um 180 Grad um die Z-Achse (Uhrzeigersinn).
                videoPlane.Rotate(Vector3.forward, 180f);
            }
        }

        if (Input.GetKeyDown(muteAudio)) // Ändere die Taste nach Bedarf
        {
            if (muted)
            {
                SetVolume(1f);
            }
            else
            {
                SetVolume(0f);
            }
            muted = !muted;
        }
        if (videoPlayer != null && timeText != null && videoPlayer.isPlaying)
        {
            // Berechne die verbleibende Zeit
            double remainingTime = videoPlayer.length - videoPlayer.time;

            // Formatiere die verbleibende Zeit als Minuten und Sekunden
            int minutes = Mathf.FloorToInt((float)remainingTime / 60F);
            int seconds = Mathf.FloorToInt((float)remainingTime - minutes * 60);

            // Setze den Text des UI-Textfeldes
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        float distanceThisFrame = (fec.GetComponent<FitnessEquipmentDisplay>().speed /3.6f) * Time.deltaTime;

        // Addiere die Distanz zum Gesamtwert
        distanceCalculated += distanceThisFrame;

        // Optional: Die Distanz in der Konsole ausgeben
       // Debug.Log("Zurückgelegte Distanz: " + distanceCalculated + " Meter");

    }

  

  

    void AdjustSpeed()
    {
        // Only execute if the GPX exists and track length has been stored in the distance slider
        if (elevationMap.distanceSlider.maxValue <= 0f) return;
        // Get the percentage we've travelled
     //   float distPercent = fec.distanceTraveled / elevationMap.distanceSlider.maxValue;
        float distPercent = distanceCalculated / elevationMap.distanceSlider.maxValue;
        // Get the time of the video that matches the travel distance
        float expectedVideoTime = (float)(distPercent * videoPlayer.length);
        // If the time doesn't match, we speed the video up/down
        float timeDifference = (expectedVideoTime - (float)videoPlayer.time) ;
      //  Debug.Log("Time Dif: " + timeDifference + " seconds");
      //  Debug.Log("ExpVT: " + expectedVideoTime + " seconds");
      //  Debug.Log("ExpVT: " + distPercent + " dis%");
        if (Mathf.Abs(timeDifference) > 0.1f)
        {
            speedMultiplier = MathUtils.Map(timeDifference, -10, 10, 0.9f, 1.1f);
            speedMultiplier = Mathf.Clamp(speedMultiplier, 0.9f, 1.1f);
          //  Debug.Log("SpeMulti: " + speedMultiplier + " dis%");
        }
        else
        {
            speedMultiplier = 1f;
        }


    }

    void GetRefSpeed()
    {

        if (elevationMap.distanceSlider.maxValue > 5f)
        {
            // Berechne die verbleibende Zeit
            referenceSpeed = (elevationMap.distanceSlider.maxValue / (float)videoPlayer.length) * 3.6f;
            Debug.Log("refSpeed" + referenceSpeed);
        }
        else
        {
            referenceSpeed = 25f;
        }

    }

    public void SetVolume(float volume)
    {
        if (videoPlayer != null)
        {
            videoPlayer.SetDirectAudioVolume(0, volume); // Ändere die 0 auf die entsprechende Audio-Track-ID, wenn nötig
        }
        else
        {
            Debug.LogWarning("VideoPlayer nicht zugewiesen!");
        }
    }

    public void SetVolumeWithClick()
    {
        if (muted)
        {
            SetVolume(1f);
        }
        else
        {
            SetVolume(0f);
        }
        muted = !muted;
    }

    public void SetVideoSpeed50()
    {
      
        if (halfSpeed)
        {
            
            if (buttenText != null)
            {
                buttenText.text = "100%";
            }
                       
        }
        if (!halfSpeed)
        {

            if (buttenText != null)
            {
                buttenText.text = "50%";
            }

        }
        halfSpeed = !halfSpeed;


    }

}
