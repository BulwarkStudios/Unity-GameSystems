using System.Collections.Generic;
using UnityEngine;

namespace BulwarkStudios.GameSystems.Logs {

    public interface ILogConfig {

        /// <summary>
        /// Logger
        /// </summary>
        ILogger GetLogger();

        /// <summary>
        /// List of available tags
        /// </summary>
        HashSet<string> GetAvailableTags();

        /// <summary>
        /// Active the logs ?
        /// </summary>
        /// <returns></returns>
        bool ActiveLog();

    }

}