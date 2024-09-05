using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using TMPro;
using System.Threading;


public class VideoSpeed : MonoBehaviour
{
    // 1 = 20Km/h, 2 = 40Km/h, 
    [SerializeField] public VideoPlayer videoPlayer;

    public float videoSpeed = 1f;
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

    }

    // Update is called once per frame
    void Update()
    {
        /*   if (Input.GetKeyDown(toggleKey))
           {
               ToggleInputField();
           }*/
        if (halfSpeed == false)
        {
            fecSpeed = fec.GetComponent<FitnessEquipmentDisplay>().speed;
            videoPlayer.playbackSpeed = fecSpeed / 20f;
        }
        else
        {
            // fi it was a motorCam the video could be to fast, so we reduce to 50%
            fecSpeed = fec.GetComponent<FitnessEquipmentDisplay>().speed;
            videoPlayer.playbackSpeed = fecSpeed / 40f;
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
