using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Video;
using SFB;
using System.IO;


public class CanvasSampleOpenFileImage : MonoBehaviour
{

    public VideoPlayer vp;
    public GPX.GPXParser gpxParser;
    public ElevationMap elevationMap;

    public void OnFileSelectButtonDown()
    {
        string[] FilePath = OpenFile();
        if (FilePath.Length <= 0) return;

        string video = FilePath[0];
        string name = Path.GetFileNameWithoutExtension(video);
        vp = vp.GetComponent<VideoPlayer>();

        if (gpxParser)
        {
            bool hasGpx = false;

            var gpxDirectory = Path.Combine(Application.dataPath, "../Overlay_data/gpx");
            foreach (string file in Directory.EnumerateFiles(gpxDirectory, $"{name}.gpx", SearchOption.AllDirectories))
            {
                gpxParser.Import(file);
                hasGpx = true;
                break;
            }

            elevationMap.enabled = hasGpx;
        }


        if (vp.isPlaying)
            vp.Stop();
        vp.url = "file://" + video;
        vp.Play();

    }
    public string[] OpenFile()
    {
        var extensions = new[]
        {
      new ExtensionFilter("Movie Files", "mp4", "mov", "avi", "webm", "mkv"),

    };
        var FilePath = StandaloneFileBrowser.OpenFilePanel("Select Movie File", "", extensions, true);
        return FilePath;
    }

    public void ExitApplication()
    {
        vp.Stop();
        Application.Quit();

    }
}