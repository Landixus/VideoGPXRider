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
    // 1 = 20Km/h, 2 = 40Km/h, 
    [SerializeField] public VideoPlayer videoPlayer;

    public float videoSpeed = 0f;
    public float maxVideoSpeed = 10f;
    public float minVideoSpeed = 0f;
    public FitnessEquipmentDisplay fec;
    public float fecSpeed;

    public string pathTotheVideo;
   // public TMP_InputField inputField;
    public Button okButton;
    public KeyCode toggleKey = KeyCode.V;
    public KeyCode videoRotate = KeyCode.R;
    public RectTransform videoPlane;

    public AudioSource audioSource;
    public AudioListener audioListener;
    public GameObject AudioListenerGO;
    //  public string newVideoURL;
    public KeyCode muteAudio = KeyCode.M;
    private bool muted = false;

    //For half the Video Speed
    public bool halfSpeed = false;
    public Button VSpeedButton;
    public Text  buttenText;
    public TMP_Text timeText;

    //GetTrackLength of GPX
    public ElevationMap elevationMap;
  //  private float lastCheckTime = 0f;
   // private float checkInterval = 1f; // Wie oft überprüft wird, z.B. jede Sekunde
  //  private float lastDistance = 0f; // Um zu überprüfen, ob sich die Distanz ändert
    public float checkInterval = 1f;
    private float speedMultiplier;
    private float referenceSpeed; // get video Length to have a reference speed

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

        if (fec.GetComponent<FitnessEquipmentDisplay>().speed >= 1)
        {
            float speed = fec.GetComponent<FitnessEquipmentDisplay>().speed;
            //  if (motorCam) speed /= 40f;
            //  else
            //get video length to calculate a reference speed. 
            speed /= referenceSpeed;
            videoPlayer.playbackSpeed = speed * speedMultiplier;

          //  Debug.Log("RefSpeed" + referenceSpeed.ToString());
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
    }

  

  

    void AdjustSpeed()
    {
        // Only execute if the GPX exists and track length has been stored in the distance slider
        if (elevationMap.distanceSlider.maxValue <= 0f) return;
        // Get the percentage we've travelled
        float distPercent = fec.distanceTraveled / elevationMap.distanceSlider.maxValue;
        // Get the time of the video that matches the travel distance
        float expectedVideoTime = (float)(distPercent * videoPlayer.length);
        // If the time doesn't match, we speed the video up/down
        float timeDifference = expectedVideoTime - (float)videoPlayer.time;
        if (Mathf.Abs(timeDifference) > 0.1f)
        {
            speedMultiplier = MathUtils.Map(timeDifference, -10, 10, 0.9f, 1.1f);
            speedMultiplier = Mathf.Clamp(speedMultiplier, 0.9f, 1.1f);
        }
        else
        {
            speedMultiplier = 1f;
        }


    }

    void GetRefSpeed()
    {

        //if (elevationMap.distanceSlider.maxValue <= 0f) return;

        if (elevationMap.distanceSlider.maxValue > 5f)
        {
            // Berechne die verbleibende Zeit
            referenceSpeed = (elevationMap.distanceSlider.maxValue / (float)videoPlayer.length) * 3.6f;
        }
        else
        {
            referenceSpeed = 25f;
        }

      //  CameraVideoScript.GetComponent<VideoSpeed>().enabled = true;
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
        /* if (halfSpeed == false)
         {
             halfSpeed = true;
         }
         else
         {
             SetVolume(0f);
         }*/
        

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

    /*
    private void ToggleInputField()
    {
        inputField.gameObject.SetActive(!inputField.gameObject.activeSelf);
        okButton.gameObject.SetActive(!okButton.gameObject.activeSelf);
    }
    */
    /*
    public void ChangeVideoURL()
    {

        if (!string.IsNullOrEmpty(newVideoURL))
        {
            videoPlayer.Stop();
            videoPlayer.url = newVideoURL;
            videoPlayer.Play();
        }
    }*/
}
