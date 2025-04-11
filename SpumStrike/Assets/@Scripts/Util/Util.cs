using System;
using UnityEngine;

public static class Util
{
    public static bool IsTypeEnemy(Type type)
    {
        if (type.IsSubclassOf(typeof(EnemyController)) || type == typeof(EnemyController))
            return true;
        return false;
    }

    public static Color HexCodeToColor(string hexaCode)
    {
        Color color;
        if (ColorUtility.TryParseHtmlString(hexaCode, out color))
            return color;

        Debug.Log("HexaCode Error - " + hexaCode);
        return Color.white;
    }
}
