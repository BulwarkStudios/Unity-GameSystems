using System.Collections.Generic;
using UnityEngine;

namespace BulwarkStudios.GameSystems.Logs {

    public abstract class LogConfig : ILogConfig {

        /// <summary>
        /// Logger
        /// </summary>
        private static ILogger logger = UnityEngine.Debug.unityLogger;

        /// <summary>
        /// List of available tags
        /// </summary>
        private readonly HashSet<string> availableTags = new HashSet<string>();

        /// <summary>
        /// Initialize
        /// </summary>
        protected LogConfig() {
            Setup();
        }

        /// <summary>
        /// Setup the config
        /// </summary>
        private void Setup() {
            SetupTags();
        }

        /// <summary>
        /// Setup the tags
        /// </summary>
        protected abstract void SetupTags();

        /// <summary>
        /// Active the logs ?
        /// </summary>
        protected abstract bool ActiveLogs();

        /// <summary>
        /// Add a tag
        /// </summary>
        /// <param name="tag"></param>
        protected void AddTag(string tag) {
            availableTags.Add(tag);
        }

        /// <summary>
        /// Get the logger
        /// </summary>
        /// <returns></returns>
        ILogger ILogConfig.GetLogger() {
            return logger;
        }

        /// <summary>
        /// Get available tags
        /// </summary>
        /// <returns></returns>
        HashSet<string> ILogConfig.GetAvailableTags() {
            return availableTags;
        }

        /// <summary>
        /// Active the logs ?
        /// </summary>
        /// <returns></returns>
        bool ILogConfig.ActiveLog() {
            return ActiveLogs();
        }

    }

}
