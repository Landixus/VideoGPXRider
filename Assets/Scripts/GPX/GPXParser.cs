using UnityEngine;
using SFB;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine.Events;

namespace GPX
{
    [XmlRoot("gpx", Namespace = "http://www.topografix.com/GPX/1/1", IsNullable = false)]
    public class GPX
    {
        public Metadata metadata;
        public Track trk;
    }

    public class Text
    {
        [XmlText(typeof(string))]
        public string value;
    }

    public class Link
    {
        [XmlAttribute("href", DataType = "string")]
        public string href;
    }

    public class Metadata
    {
        public Text name;
        public Text desc;
        public Author author;
        public Link link;
    }

    public class Author
    {
        public Text name;
        public Link link;
    }

    public class Track
    {
        public Text name;
        public Text desc;
        public Link link;
        public Text type;
        [XmlArrayItem("trkpt", Type = typeof(TrackPoint))]
        [XmlArray("trkseg")]
        public TrackPoint[] trkseg;
    }

    public class TrackSegment
    {
        public TrackPoint[] trkpt;
    }

    public class TrackPoint
    {
        [XmlAttribute("lat", DataType = "double")]
        public double lat;
        [XmlAttribute("lon", DataType = "double")]
        public double lon;
        public Elevation ele;
    }

    public class Elevation
    {
        [XmlText(typeof(double))]
        public double value;
    }

    public class GPXParser : MonoBehaviour
    {
        [System.Serializable] public class ImportEvent : UnityEvent<GPX> { }
        [System.Serializable] public class ErrorEvent : UnityEvent { }


        static ExtensionFilter[] gpxFilter = new[] { new ExtensionFilter("GPX files", "gpx") };

        public ImportEvent OnImport = new ImportEvent();
        public ErrorEvent OnError = new ErrorEvent();

        public void Import(string path = null)
        {
            if (string.IsNullOrEmpty(path))
            {
                var paths = StandaloneFileBrowser.OpenFilePanel("Select GPX File", "", gpxFilter, false);
                if (paths == null || paths.Length != 1) return;
                path = paths[0];
            }
            if (Parse(path, out GPX data)) OnImport.Invoke(data);
            else OnError.Invoke();
        }

        private bool Parse(string file, out GPX data)
        {
            data = null;
            try
            {
                using (FileStream stream = new FileStream(file, FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(GPX));
                    data = serializer.Deserialize(stream) as GPX;
                    return true;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to parse GPX file {file}");
                Debug.LogException(ex);
                return false;
            }
        }
    }
}
