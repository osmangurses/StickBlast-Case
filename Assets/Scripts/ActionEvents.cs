using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ActionEvents
{
    public static event Action ShapePlaced;
    public static event Action NoAvailableSpace;

    public static void InvokeShapePlaced()
    {
        ShapePlaced?.Invoke();
    }

    public static void InvokeNoAvailableSpace()
    {
        NoAvailableSpace?.Invoke();
    }
}
