using System;

/// <summary>
/// Add this Attribute together with the HideInInspector attribute to draw the field in a Foldout category
/// </summary>
public class CategorizedAttribute : Attribute
{
    public string Category;
    public CategorizedAttribute() : this(null) { }
    public CategorizedAttribute(string category)
    {
        Category = category;
    }
}