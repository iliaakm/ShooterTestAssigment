using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConsoleLog
{
    public static void Subscribe()
    {
        GameLogPublisher.RiseLogEvent += PrintConsole;
    }

    private static void PrintConsole(object sender, CustomEventArgs args)
    {
        Debug.Log(args.Message);
    }
}
