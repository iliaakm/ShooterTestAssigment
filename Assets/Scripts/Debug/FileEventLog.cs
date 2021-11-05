using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GameLogSystem
{
    public class FileEventLog : MonoBehaviour
    {
        public static void Subscribe()
        {
            GameLogPublisher.RiseLogEvent += LogEventFile;
        }

        private static void LogEventFile(object sender, CustomEventArgs args)
        {
            string filePath = Path.Combine(Application.persistentDataPath, GameConfig.GameLog.eventFileLogName);
            string message = $"Test Event: {args.Message}";
            File.WriteAllText(filePath, message);
            Debug.Log($"Log to {filePath}");
        }
    }
}
