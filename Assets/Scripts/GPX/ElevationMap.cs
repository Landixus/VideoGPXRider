using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GPX;
using System.Collections.Generic;

[RequireComponent(typeof(CanvasRenderer))]
[ExecuteInEditMode]
public class ElevationMap : Graphic
{
    private float[] distances = System.Array.Empty<float>();
    private float[] elevationGain = System.Array.Empty<float>();
    private int passedIndex = 0;

    public Gradient ElevationColor = new Gradient();
    public float thickness = 10f;
    public float totalElevationGain = 0f;

    public Vector2[] points = { new(0, 0), new(0.5f, 1), new(1, 0) };
    
    public Slider distanceSlider;
    public FitnessEquipmentDisplay fitnessEquipmentDisplay;

    public TMP_Text kilometer_remain;
    public TMP_Text kilometer_driven;
    public TMP_Text heightText;
    public TMP_Text trackHeightMeter;
    public GPXParser gpxParser;

    public TMP_Text minutesRemain;
    //  public TMP_Text minutesText;
    private float distanceCalculated;
    

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        UpdateGeometry();
    }
#endif

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        if (points.Length < 2) return;

        for (int i = 0; i < points.Length - 1; ++i)
        {
            CreateLineSegment(points[i], points[i + 1], vh);
            int index = i * 4;
            vh.AddTriangle(index, index + 1, index + 3);
            vh.AddTriangle(index + 3, index + 2, index);
        }
    }

    private void CreateLineSegment(Vector2 point1, Vector2 point2, VertexHelper vh)
    {
        UIVertex vertex = UIVertex.simpleVert;

        // Transform point from normalized space to rect space
        point1 -= rectTransform.pivot;
        point1 *= rectTransform.rect.size;
        point2 -= rectTransform.pivot;
        point2 *= rectTransform.rect.size;

        // Sample gradient by slope
        Vector2 direction = (point2 - point1).normalized;
        float angle = MathUtils.Map(Vector2.SignedAngle(Vector2.right, direction), -90, 90, 0, 1);
        vertex.color = ElevationColor.Evaluate(angle) * color;

        // Start of line with thickness
        vertex.position = point1;
        vertex.position += new Vector3(0, -thickness * 0.5f);
        vh.AddVert(vertex);
        vertex.position += new Vector3(0, thickness);
        vh.AddVert(vertex);

        // End of line with thickness
        vertex.position = point2;
        vertex.position += new Vector3(0, -thickness * 0.5f);
        vh.AddVert(vertex);
        vertex.position += new Vector3(0, thickness);
        vh.AddVert(vertex);
    }



    public void Create(GPX.GPX gpx)
    {
        var points = new List<Vector3>(GPSUtil.Convert(gpx.trk.trkseg));

        // Store calculated distances (we need them twice)
        var distances = new List<float>(points.Count) { 0 };

        // Total track length
        float length = 0f;

        for (int i = 1; i < points.Count; ++i)
        {
            var p1 = points[i - 1];
            var p2 = points[i];

            // Distance ignoring elevation
            float distance = Mathf.Sqrt(Mathf.Pow(p1.x - p2.x, 2) + Mathf.Pow(p1.z - p2.z, 2));

            // Remove overlapping points, keeping the last (assuming it's when GPS stabilized)
            if (distance < 0.0001f)
            {
                points.RemoveAt(i - 1);
                distances.RemoveAt(i - 1);
                --i;
            }

            distances.Add(distance);
            length += distance;
        }

        // Get elevation range and gain
        float minElevation = points[0].y;
        float maxElevation = points[0].y;
        elevationGain = new float[points.Count];
        elevationGain[0] = 0f;
        for (int i = 1; i < points.Count; ++i)
        {
            minElevation = Mathf.Min(minElevation, points[i].y);
            maxElevation = Mathf.Max(maxElevation, points[i].y);

            float elevationDifference = points[i].y - points[i - 1].y;
            if (elevationDifference > 0) totalElevationGain += elevationDifference;
            elevationGain[i] = totalElevationGain;
        }

        // Now you have the total elevation gain
       // Debug.Log("Total Elevation Gain: " + totalElevationGain + " meters");

        // Store distance sum and normalize points
        this.points = new Vector2[points.Count];
        this.distances = new float[points.Count];
        this.distances[0] = 0f;
        float distanceSum = 0f;
        for (int i = 0; i < points.Count; ++i)
        {
            distanceSum += distances[i];
            this.distances[i] = distanceSum;
            float x = distanceSum / length;
            float y = MathUtils.Map(points[i].y, minElevation, maxElevation, 0, 1);
            this.points[i] = new Vector2(x, y);
        }

        //  trackName.text = gpxParser.trackName.ToString();       
        distanceSlider.maxValue = length;
        UpdateGeometry();
        UnderMapText();
    }

    private void Update()
    {
        if (fitnessEquipmentDisplay && fitnessEquipmentDisplay.distanceTraveled < distanceSlider.maxValue)
        {
           // float traveled = fitnessEquipmentDisplay.distanceTraveled;
            float distanceThisFrame = (fitnessEquipmentDisplay.GetComponent<FitnessEquipmentDisplay>().speed / 3.6f) * Time.deltaTime;

            // Addiere die Distanz zum Gesamtwert
            distanceCalculated += distanceThisFrame;
            float traveled = distanceCalculated;

            // Display progress
            distanceSlider.value = distanceSlider.maxValue - traveled;

            // Find the first point that we haven't passed yet
            for (; passedIndex < distances.Length - 1; ++passedIndex)
            {
                if (distances[passedIndex] > traveled) break;
            }
            
            // Fallback to the passed point elevation gain
            float elevationGain = this.elevationGain[passedIndex];
            
            if (passedIndex > 0)
            {
                // Lerp between the elevation gain of the two nearest points
                float percent = MathUtils.Map(traveled, distances[passedIndex - 1], distances[passedIndex], 0, 1);
                percent = Mathf.Clamp(percent, 0, 1);
                elevationGain = Mathf.Lerp(this.elevationGain[passedIndex - 1], this.elevationGain[passedIndex], percent);
            }
            
            // Display the elevation gain
            heightText.text = elevationGain.ToString("F0");

            // Display the traveled distance
            float drivedKm = distanceCalculated / 1000f; // (fitnessEquipmentDisplay.distanceTraveled / 1000f);
            kilometer_driven.text = drivedKm.ToString("F1");
            timeRemainFunc();
        }
    }

    private void UnderMapText()
    {
        kilometer_remain.text = (distanceSlider.maxValue / 1000).ToString("F1");
        trackHeightMeter.text = totalElevationGain.ToString("F0");
       
       
        // trackName.text = trackName.ToString();
    }

    private void timeRemainFunc()
    {


        // Berechne die verbleibende Distanz
        float verbleibendeDistanz = distanceSlider.maxValue - distanceCalculated; // fitnessEquipmentDisplay.distanceTraveled;

        // Verbleibende Zeit (in Sekunden) = verbleibende Distanz / aktuelle Geschwindigkeit
        if (fitnessEquipmentDisplay.speed > 0)
        {
            float verbleibendeZeitInSekunden = verbleibendeDistanz / (fitnessEquipmentDisplay.speed * 1000 / 3600);

            // Konvertiere die verbleibende Zeit in Minuten
            minutesRemain.text = (verbleibendeZeitInSekunden / 60.0f).ToString("F0");
        }
        else
        {
            minutesRemain.text = 0f.ToString("F0"); // Geschwindigkeit ist 0, also keine Zeit verbleibt
        }
    }



}