using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppEvents
{
    public delegate void MouseCursorEnabledEvent(bool enabled);

    public static event MouseCursorEnabledEvent MouseCursorEnable;

    public static void InvokeMouseCursorEnabled(bool enabled)
    {
        MouseCursorEnable?.Invoke(enabled);
    }
}
