using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameLog
{
    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        Start();
    }

    static void Start()
    {
        ConsoleLog.Subscribe();
    }

    public static void Log(string message)
    {
        GameLogPublisher.Log(message);
    }
}

static class GameLogPublisher 
{
    public delegate void CustomEventHandler(object sender, CustomEventArgs args);
    public static event CustomEventHandler RiseLogEvent;

    public static void Log(string message)
    {
        OnRaiseLogEvent(new CustomEventArgs(message));
    }

    public static void OnRaiseLogEvent(CustomEventArgs e)
    {
        CustomEventHandler raiseEvent = RiseLogEvent;

        if (RiseLogEvent != null)
        {
            raiseEvent(null, e);
        }
    }
}

public class CustomEventArgs : EventArgs
{
    public CustomEventArgs(string message)
    {
        Message = message;
    }

    public string Message { get; set; }
}
