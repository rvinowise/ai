using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace rvinowise.unity.persistence {
public class Convertor
{
    

    public static float[] ConvertFromVector2(Vector2 vector2) {
        float[] values = {vector2.x, vector2.y};
        return values;
    }

    public static Vector2 ConvertToVector2(float[] values) {
        return new Vector2(values[0], values[1]);
    }

    public static float[,] ConvertFromVector2Array(Vector2[] vector2) {
        if (vector2 == null) {
            return new float[0, 2];
        }

        float[,] values = new float[vector2.Length, 2];
        for (int i = 0; i < vector2.Length; i++) {
            values[i, 0] = vector2[i].x;
            values[i, 1] = vector2[i].y;
        }
        return values;
    }

    public static Vector2[] ConvertToVector2Array(float[,] array) {
        if (array.Length == 0) {
            return null;
        }

        Vector2[] vector2 = new Vector2[array.GetUpperBound(0) + 1];
        for (int i = 0; i < vector2.Length; i++) {
            vector2[i] = new Vector2(array[i, 0], array[i, 1]);
        }
        return vector2;
    }
}
}