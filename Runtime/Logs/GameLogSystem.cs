using System;
using System.Diagnostics;
using BulwarkStudios.GameSystems.Configs;
using UnityEngine;

namespace BulwarkStudios.GameSystems.Logs {

    public class GameLogSystem {

        /// <summary>
        /// Log tag default
        /// </summary>
        private const string TAG_DEFAULT = "Default";

        /// <summary>
        /// Current config
        /// </summary>
        private static ILogConfig config = new LogConfigDefault();

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="config"></param>
        public static void Initialize(ILogConfig config) {

            // No config
            if (config == null) {
                throw new Exception("[Log] There is no config");
            }

            // Save
            GameLogSystem.config = config;

            // Active logs ?
            config.GetLogger().logEnabled = config.ActiveLog();

        }

        /// <summary>
        /// Log
        /// </summary>
        [Conditional(BuildConstants.DEBUG)]
        public static void Info(object message, string tag = TAG_DEFAULT) {
            Write(LogType.Log, tag, message);
        }

        /// <summary>
        /// Warning
        /// </summary>
        [Conditional(BuildConstants.DEBUG)]
        public static void Warning(object message, string tag = TAG_DEFAULT) {
            Write(LogType.Warning, tag, message);
        }

        /// <summary>
        /// Error
        /// </summary>
        [Conditional(BuildConstants.DEBUG)]
        public static void Error(object message, string tag = TAG_DEFAULT) {
            Write(LogType.Error, tag, message);
        }

        /// <summary>
        /// Exception
        /// </summary>
        [Conditional(BuildConstants.DEBUG)]
        public static void Exception(object message, string tag = TAG_DEFAULT) {
            Write(LogType.Exception, tag, message);
        }

        /// <summary>
        /// Write
        /// </summary>
        [Conditional(BuildConstants.DEBUG)]
        private static void Write(LogType type, string tag, object message) {

            // Pas de tag
            if (tag != TAG_DEFAULT && !config.GetAvailableTags().Contains(tag)) {
                return;
            }

            // Log
            config.GetLogger().Log(type, "[" + tag + "]", message, null);

        }

    }

}