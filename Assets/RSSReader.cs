using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml;
using CodeHollow.FeedReader;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;



public class RSSReader : MonoBehaviour
{
    [SerializeField] private TMP_Text textbox;
    [SerializeField] private Text errorText;
    [SerializeField] private string RSSLocation;
    private bool pulling = false;
    private int waitTime = 5;

    // Start is called before the first frame update

    // Update is called once per frame
    [Obsolete]
    void Update()
    {
        if (pulling == false)
            StartCoroutine(getMyFeedData());
    }

    [Obsolete]
    IEnumerator getMyFeedData()
    {
        pulling = true;
        StartCoroutine(getFeedAsString());
        yield return new WaitForSeconds(waitTime);
        pulling = false;
    }

    [Obsolete]
    private IEnumerator getFeedAsString()
    {
        UnityWebRequest uwr = UnityWebRequest.Get(RSSLocation);
      //  UnityWebRequest uwr = UnityWebRequest.Result.ConnectionError;
        
        yield return uwr.SendWebRequest();
        if (uwr.isNetworkError || uwr.isHttpError)
        {
            Debug.Log(uwr.error);
        }
        else
        {
            string text = ((DownloadHandler)uwr.downloadHandler).text;
            setRSSFeedData(text);
        }

    }

    string setRSSFeedData(string RSSString)
    {            
        string myString = "";
        try
        {
            var feedReader = FeedReader.ReadFromString(RSSString); 
            foreach (var item in feedReader.Items)
            {
                myString += "  ***  " + item.Title + "; Author:" + item.Author + "\n";
            }
            textbox.text = myString;
        }
        catch (Exception ex)
        {
            errorText.text += "Exception:";
            errorText.text += "Exception Name: " + ex.GetType().Name+ "\n";
            errorText.text += "Message: " + ex.Message +"\n";
            errorText.text += "Stack Trace:\n " + ex.StackTrace + "\n";
            errorText.text = ex.Message + "\n";
            if (ex.InnerException != null)
            {
                var ie = ex.InnerException;
                errorText.text += "   The Inner Exception:";
                errorText.text += "      Exception Name: " + ie.GetType().Name+ "\n";
                errorText.text += "      Message: " + ie.Message +"\n";
                errorText.text += "      Stack Trace:\n " + ie.StackTrace + "\n";
            }
        }
        return myString;
    }

}
