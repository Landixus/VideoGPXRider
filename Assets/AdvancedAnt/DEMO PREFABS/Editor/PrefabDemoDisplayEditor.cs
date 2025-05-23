#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using System.Reflection;

[CustomEditor(typeof(PrefabDemoDisplay))]
public class PrefabDemoDisplayEditor : Editor
{
    private class Category
    {
        public bool open = false;
        public List<SerializedProperty> props = new();
        public void Add(SerializedProperty prop)
        {
            props.Add(prop);
        }
    }

    private Dictionary<string, Category> texts = new();
    private void OnEnable()
    {
        var fields = typeof(PrefabDemoDisplay).GetFields(BindingFlags.Public | BindingFlags.Instance);

        foreach (var field in fields)
        {
            var attribute = field.GetCustomAttribute<CategorizedAttribute>();
            if (attribute == null) continue;
            
            var category = attribute.Category;
            if (string.IsNullOrEmpty(category)) category = field.Name.Split("_")[0];

            var prop = serializedObject.FindProperty(field.Name);
            if (!texts.ContainsKey(category)) texts.Add(category, new());
            texts[category].Add(prop);
        }
    }

    public override void OnInspectorGUI()
    {
        foreach (var (key, value) in texts)
        {
            value.open = EditorGUILayout.Foldout(value.open, key);
            if (value.open)
            {
                EditorGUI.indentLevel++;
                foreach (var prop in value.props)
                {
                    EditorGUILayout.PropertyField(prop);
                }
                EditorGUI.indentLevel--;
            }
        }
        base.OnInspectorGUI();
    }
}
#endif