using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
[ExecuteInEditMode]
public class ElevationMap : Graphic
{
    public Gradient ElevationColor = new Gradient();
    public float thickness = 10f;
    public FitnessEquipmentDisplay fitnessEquipmentDisplay;
    public float length = 0f;
    public float restLength;
    public Slider distanceSlider;

    public Vector2[] points = { new(0, 0), new(0.5f, 1), new(1, 0) };

    protected override void OnValidate()
    {
        UpdateGeometry();
    }

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
        var points = GPSUtil.Convert(gpx.trk.trkseg);

        // Store calculated distances (we need them twice)
        var distances = new float[points.Length];
        distances[0] = 0f;

        // Total track length
        float length = 0f;

        // Track elevation range
        float minElevation = points[0].y;
        float maxElevation = points[0].y;

        for (int i = 1; i < points.Length; ++i)
        {
            var p1 = points[i - 1];
            var p2 = points[i];

            // Distance ignoring elevation
            float distance = Mathf.Sqrt(Mathf.Pow(p1.x - p2.x, 2) + Mathf.Pow(p1.z - p2.z, 2));
            distances[i] = distance;
            length += distance;


            // Update elevation range
            minElevation = Mathf.Min(minElevation, p2.y);
            maxElevation = Mathf.Max(maxElevation, p2.y);
        }

        this.points = new Vector2[points.Length];
        float sum = 0f;
        for (int i = 0; i < points.Length; ++i)
        {
            sum += distances[i];
            float x = sum / length;
            float y = MathUtils.Map(points[i].y, minElevation, maxElevation, 0, 1);
            this.points[i] = new Vector2(x, y);
        }


        distanceSlider.maxValue = length;
       


        UpdateGeometry();

    }

    private void Update()
    {
        if (fitnessEquipmentDisplay && fitnessEquipmentDisplay.distanceTraveled < distanceSlider.maxValue)
        {
            float traveled = fitnessEquipmentDisplay.distanceTraveled;
            distanceSlider.value = distanceSlider.maxValue - traveled;
        }



    }




}
