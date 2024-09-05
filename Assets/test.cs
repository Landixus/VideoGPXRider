using UnityEngine;
using SatorImaging.AppWindowUtility;
using TMPro;
using UnityEngine.UI;
public class test : MonoBehaviour
{

    public Button transOnOff;
    public GameObject videoPlane;
    public GameObject videoCamera;
    public GameObject cameraTrans;
    public KeyCode key;


    void Start()
    {
        
        AppWindowUtility.Transparent = false;
        AppWindowUtility.AlwaysOnTop = true;

        videoPlane = GameObject.Find("VideoPlane");
        videoCamera = GameObject.Find("CameraVideo");
    }

    public void TransOnOff()
    {
        
        AppWindowUtility.Transparent = !AppWindowUtility.Transparent;
        videoCamera.SetActive(!videoCamera.activeSelf);
        videoPlane.SetActive(!videoPlane.activeSelf);
        cameraTrans.SetActive(!cameraTrans.activeSelf);

    }

    private void Update()
    {
        if (Input.GetKeyDown(key))
        {
            transOnOff.GetComponent<Button>().onClick.Invoke();
        }
    }




}