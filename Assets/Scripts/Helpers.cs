using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helpers
{
    public static float NormalizeAngle(float a)
    {
        if (a > 180f) return a - 360f;
        if (a < -180f) return a + 360f;
        return a;
    }
}
