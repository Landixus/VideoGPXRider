using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms; // Notwendig für Screen.PrimaryScreen
using UnityEngine;

public class displayScreenie : MonoBehaviour
{
    private float timer = 0f;
    private readonly float interval = 300f; // 5 Minuten (in Sekunden)

    [DllImport("gdi32.dll")]
    private static extern bool BitBlt(IntPtr hdcDest, int xDest, int yDest, int width, int height, IntPtr hdcSrc, int xSrc, int ySrc, CopyPixelOperation rop);

    [DllImport("user32.dll")]
    private static extern IntPtr GetDesktopWindow();

    [DllImport("user32.dll")]
    private static extern IntPtr GetWindowDC(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);

    void Start()
    {
        // Verzeichnis festlegen (Bilderordner in deinem Anwendungsordner)
        DirectoryInfo root = Directory.GetParent(UnityEngine.Application.dataPath);
        string screenshotsDirectory = Path.Combine(root.FullName, "Overlay_data/screens");

        // Verzeichnis erstellen, falls es noch nicht existiert
        if (!Directory.Exists(screenshotsDirectory))
        {
            Directory.CreateDirectory(screenshotsDirectory);
        }

      //  Debug.Log("Screenshots will be saved in: " + screenshotsDirectory);
    }

    void Update()
    {
        // Timer hochzählen
        timer += Time.deltaTime;

        // Wenn 5 Minuten (300 Sekunden) vorbei sind
        if (timer >= interval)
        {
            // Screenshot erstellen
            string screenshotPath = GenerateScreenshotPath();
            CaptureFullScreen(screenshotPath);

            // Timer zurücksetzen
            timer = 0f;
        }
    }

    private string GenerateScreenshotPath()
    {
        DirectoryInfo root = Directory.GetParent(UnityEngine.Application.dataPath);
        string screenshotsDirectory = Path.Combine(root.FullName, "Overlay_data/screens");

        // Dateiname mit Zeitstempel erstellen
        string fileName = "screenshot_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
        return Path.Combine(screenshotsDirectory, fileName);
    }

    public void CaptureFullScreen(string filePath)
    {
        try
        {
            // Bildschirmgröße ermitteln
            int screenWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;

            // Bitmap erstellen, um den Screenshot zu speichern
            using (Bitmap bmp = new Bitmap(screenWidth, screenHeight))
            {
                // Einen Grafik-Handle des Desktop-Fensters erhalten
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp))
                {
                    IntPtr hdcBitmap = g.GetHdc();
                    IntPtr hdcDesktop = GetWindowDC(GetDesktopWindow());

                    // Screenshot aufnehmen
                    BitBlt(hdcBitmap, 0, 0, screenWidth, screenHeight, hdcDesktop, 0, 0, CopyPixelOperation.SourceCopy);

                    // Ressourcen freigeben
                    ReleaseDC(GetDesktopWindow(), hdcDesktop);
                    g.ReleaseHdc(hdcBitmap);
                }

                // Bild speichern
                bmp.Save(filePath, ImageFormat.Png);
            }

           // Debug.Log("Screenshot saved: " + filePath);
        }
        catch (Exception e)
        {
            Debug.LogError("Error taking screenshot: " + e.Message);
        }
    }
}

/*
 * void TakeScreenshot()
    {
        // Erstelle einen Dateinamen für den Screenshot
        string screenshotName = screenshotNamePrefix + screenshotCount.ToString() + ".png";

        // Speichere den Screenshot
        ScreenCapture.CaptureScreenshot(screenshotName);

        // Erhöhe den Zähler für die Screenshot-Nummerierung
        screenshotCount++;

       // Debug.Log("Screenshot " + screenshotName + " wurde erstellt.");
    }
*/