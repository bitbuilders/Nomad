using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Colors
{
    public enum ColorType
    {
        WHITE,
        BLACK
    }

    public static string ColorPrefix { get { return "<color="; } }
    public static string ColorSuffix { get { return "</color>"; } }

    public static string White { get { return ColorPrefix + "\"white\">"; } }
    public static string Black { get { return ColorPrefix + "\"black\">"; } }

    public static string ConvertToColor(string message, ColorType color)
    {
        string newColorString = "";

        switch (color)
        {
            case ColorType.WHITE:
                newColorString = White + message + ColorSuffix;
                break;
            case ColorType.BLACK:
                newColorString = Black + message + ColorSuffix;
                break;
        }

        return newColorString;
    }

    public static Color StringToColor(string color)
    {
        Color c;
        ColorUtility.TryParseHtmlString(color, out c);

        return c;
    }

    public static string ColorToString(Color c)
    {
        string color = ColorUtility.ToHtmlStringRGBA(c);
        return "#" + color;
    }
}
