using System;
using UnityEngine;

public static class GPSUtil
{
    public struct Location
    {
        public double lat;
        public double lon;
    }

    public struct Elevation
    {
        public double ele;
    }

    public struct Vector3d
    {
        public double x, y, z;
        public Vector3d(double x, double y, double z) { this.x = x; this.y = y; this.z = z; }
        public static Vector3d operator +(Vector3d a, Vector3d b) { return new Vector3d(a.x + b.x, a.y + b.y, a.z + b.z); }
        public static Vector3d operator -(Vector3d a, Vector3d b) { return new Vector3d(a.x - b.x, a.y - b.y, a.z - b.z); }
        public static Vector3d operator *(Vector3d a, Vector3d b) { return new Vector3d(a.x * b.x, a.y * b.y, a.z * b.z); }
        public static Vector3d operator /(Vector3d a, Vector3d b) { return new Vector3d(a.x / b.x, a.y / b.y, a.z / b.z); }
        public static Vector3d operator *(Vector3d a, float s) { return new Vector3d(a.x * s, a.y * s, a.z * s); }
        public static Vector3d operator /(Vector3d a, float d) { return new Vector3d(a.x / d, a.y / d, a.z / d); }

        public static implicit operator Vector3(Vector3d v) { return new Vector3((float)v.x, (float)v.y, (float)v.z); }
    }

    public const double DEG2RAD = Math.PI / 180;
    public const double R = 6378137d;

    // https://stackoverflow.com/questions/16266809/convert-from-latitude-longitude-to-x-y
    public static Vector3[] Convert(GPX.TrackPoint[] points)
    {
        int length = points.Length;

        double centerLat = 0d;
        double centerLon = 0d;
        for (int i = 0; i < length; ++i)
        {
            centerLat += points[i].lat;
            centerLon += points[i].lon;
        }
        centerLat /= length;
        centerLon /= length;
        double phi = Math.Cos(centerLat * DEG2RAD);

        Vector3[] vectors = new Vector3[length];
        for (int i = 0; i < length; ++i)
        {
            vectors[i] = ToVector(points[i], centerLat, centerLon, phi);
        }

        return vectors;
    }

    public static Vector3d ToVector(GPX.TrackPoint point, double centerLat, double centerLon, double phi)
    {
        return ToVector(point.lat - centerLat, point.lon - centerLon, point.ele.value, phi);
    }

    public static Vector3d ToVector(double lat, double lon, double ele, double phi)
    {
        double x = R * lon * DEG2RAD * phi;
        double z = R * lat * DEG2RAD;
        double y = ele;
        return new Vector3d(x, y, z);
    }
}