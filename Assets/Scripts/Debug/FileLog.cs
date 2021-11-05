using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GameLogSystem
{
    public class FileLog : MonoBehaviour
    {
        public static void Subscribe()
        {
            GameLogPublisher.RiseLogEvent += LogFile;
        }

        private static void LogFile(object sender, CustomEventArgs args)
        {
            string filePath = Path.Combine(Application.persistentDataPath, GameConfig.GameLog.fileLogName);
            string message = args.Message + System.Environment.NewLine;
            try
            {
                File.AppendAllText(filePath, message);
                Debug.Log($"Log to {filePath}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Log Write Error in {filePath} {ex.Message}");
            }
        }
    }
}
