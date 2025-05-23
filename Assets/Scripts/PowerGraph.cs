using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
[ExecuteInEditMode]
public class PowerGraph : Graphic
{
    public PrefabDemoDisplay Display;

    [System.Serializable]
    public struct DataPoint
    {
        public float value;
        public Color color;

        public DataPoint(float value, Color color)
        {
            this.value = value;
            this.color = color;
        }
    }


    [Tooltip("How often to sample the Power value per second")]
    public float SampleRate = 1f;
    [Tooltip("Max number of data points on the graph. For a sample rate of 1Hz, 6000 points will show data of the last 10 minutes.")]
    public int MaxDataPoints = 6000;
    public float thickness = 10f;
    public new Color color = Color.white;
    [Range(0,max: 1)]
    public float areaOpacity = 0.25f;

    public List<DataPoint> points = new() { new(0, Color.white), new(0.5f, Color.red), new(1, Color.green) };
    
    private float _time = 0f;

    protected override void Awake()
    {
        base.Awake();
        points = new(MaxDataPoints);
    }

    private void Update()
    {
        if (!Application.isPlaying) return;
        if (!Display) return;
        _time -= Time.deltaTime;
        if (_time > 0f) return;
        if (SampleRate > 0) _time += 1f / SampleRate;
        Sample();
        UpdateGeometry();
    }

    public void Sample()
    {
        float powerValue = Display.GetNormalizedPowerValue(Display.t_power);
        Color color = Display.GetPowerZoneColor(Display.t_power);
        while (points.Count >= MaxDataPoints - 1) points.RemoveAt(0);
        points.Add(new(powerValue, color));
    }


#if UNITY_EDITOR
    protected override void OnValidate()
    {
        if (points.Count > MaxDataPoints) points.RemoveRange(MaxDataPoints, points.Count - MaxDataPoints);
        points.Capacity = MaxDataPoints;
        UpdateGeometry();
    }
#endif

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        if (points.Count < 2) return;

        for (int i = 0; i < points.Count - 1; ++i)
        {
            CreateSegment(i, i + 1, vh);
            int index = i * 8;
            vh.AddTriangle(index + 4, index + 5, index + 7);
            vh.AddTriangle(index + 7, index + 6, index + 4);
            vh.AddTriangle(index, index + 1, index + 3);
            vh.AddTriangle(index + 3, index + 2, index);
        }
    }

    private void CreateSegment(int fromIndex, int toIndex, VertexHelper vh)
    {
        UIVertex vertex = UIVertex.simpleVert;

        Vector2 point1 = new Vector2(1f - ((float)fromIndex / (MaxDataPoints - 1)), Mathf.Min(1f, points[fromIndex].value / Display.GetMaxPower()));
        Vector2 point2 = new Vector2(1f - ((float)toIndex / (MaxDataPoints - 1)), Mathf.Min(1f, points[toIndex].value / Display.GetMaxPower()));

        // Transform point from normalized space to rect space
        point1 -= rectTransform.pivot;
        point1 *= rectTransform.rect.size;
        point2 -= rectTransform.pivot;
        point2 *= rectTransform.rect.size;

        // Start of line with thickness
        vertex.color = color;
        vertex.position = point1;
        vertex.position += new Vector3(0, -thickness * 0.5f);
        vh.AddVert(vertex);
        vertex.position += new Vector3(0, thickness);
        vh.AddVert(vertex);

        // End of line with thickness
        vertex.color = color;
        vertex.position = point2;
        vertex.position += new Vector3(0, -thickness * 0.5f);
        vh.AddVert(vertex);
        vertex.position += new Vector3(0, thickness);
        vh.AddVert(vertex);

        // Start of shaded area
        vertex.color = points[fromIndex].color;
        vertex.color.a = (byte)(areaOpacity * 255);
        vertex.position = point1;
        vh.AddVert(vertex);
        vertex.position.y = 0;
        vh.AddVert(vertex);

        // End of shaded area
        vertex.color = points[toIndex].color;
        vertex.color.a = (byte)(areaOpacity * 255);
        vertex.position = point2;
        vh.AddVert(vertex);
        vertex.position.y = 0;
        vh.AddVert(vertex);
    }
}
