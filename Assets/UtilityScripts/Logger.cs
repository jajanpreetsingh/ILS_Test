using UnityEngine;

namespace CustomUtilityScripts
{
    public static class Logger
    {
        private const string LogPrefix = "Logger : ";

        public static void LogMessage(object message)
        {
            Debug.Log(LogPrefix + message);
        }

        public static void LogWarning(object message)
        {
            Debug.LogWarning(LogPrefix + message);
        }

        public static void LogError(object message)
        {
            Debug.LogError(LogPrefix + message);
        }

        public static void LogMessage(object message, Color col)
        {
            Debug.Log((LogPrefix + message).ToHtmlStringRGB(col));
        }

        public static void LogWarning(object message, Color col)
        {
            Debug.LogWarning((LogPrefix + message).ToHtmlStringRGB(col));
        }

        public static void LogError(object message, Color col)
        {
            Debug.LogError((LogPrefix + message).ToHtmlStringRGB(col));
        }

        public static string ToHtmlStringRGB(this string message, Color color)
        {
            return string.Format("<color=#{0}>{1}</color>", ColorUtility.ToHtmlStringRGB(color), message);
        }
    }
}
