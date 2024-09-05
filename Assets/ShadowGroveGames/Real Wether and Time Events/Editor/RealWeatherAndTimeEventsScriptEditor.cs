using ShadowGroveGames.RealWeatherAndTimeEvents.Scripts;
using UnityEditor;
using UnityEngine;

namespace ShadowGroveGames.RealWeatherAndTimeEvents.Editor
{
    [CustomEditor(typeof(RealWeatherAndTimeEventsScript))]
    public class RealWeatherAndTimeEventsScriptEditor : UnityEditor.Editor
    {
        GUIStyle _labelWordWrapStyle;
        GUIStyle _smallLabelWordWrapStyle;

        public override void OnInspectorGUI()
        {
            PrepareGUIStyles();
            DrawHeaderImage();
            DrawNotice();
            base.OnInspectorGUI();
            DrawNote();
        }

        void PrepareGUIStyles()
        {
            _labelWordWrapStyle = new GUIStyle(EditorStyles.boldLabel);
            _labelWordWrapStyle.wordWrap = true;
            _smallLabelWordWrapStyle = new GUIStyle(EditorStyles.label);
            _smallLabelWordWrapStyle.wordWrap = true;
        }

        void DrawHeaderImage()
        {
            Texture2D headerImage = (Texture2D)Resources.Load("real-weather-and-time-events-banner", typeof(Texture2D));

            GUI.DrawTexture(new Rect((Screen.width / 2) - (headerImage.width / 2), 10, headerImage.width, headerImage.height), headerImage, ScaleMode.ScaleToFit, true, headerImage.width / headerImage.height);
            EditorGUILayout.Space(headerImage.height + 20);
        }

        void DrawNotice()
        {
            EditorGUILayout.LabelField("To get weather information you need an OpenWeatherMap Api key. You can get it quickly and easily here: ", _labelWordWrapStyle);

#if (UNITY_2021_1_OR_NEWER)
            EditorGUILayout.BeginHorizontal();
            if (EditorGUILayout.LinkButton("Get a OpenWeatherMap Api key"))
                Application.OpenURL("https://home.openweathermap.org/api_keys");
            EditorGUILayout.EndHorizontal();
#else
            EditorGUILayout.LabelField("https://home.openweathermap.org/api_keys", _labelWordWrapStyle);
#endif

            EditorGUILayout.Space(20);
        }

        void DrawNote()
        {
            EditorGUILayout.Space();

#if (UNITY_2021_1_OR_NEWER)
            EditorGUILayout.BeginHorizontal();
            if (EditorGUILayout.LinkButton("For support you can join our ShadowGroveGames Discord"))
                Application.OpenURL("https://discord.shadow-grove.org/");
            EditorGUILayout.EndHorizontal();
#else
            EditorGUILayout.LabelField("For support you can join our ShadowGroveGames Discord:", _smallLabelWordWrapStyle);
            EditorGUILayout.LabelField("https://discord.shadow-grove.org/", _labelWordWrapStyle);
#endif
        }
    }

}
