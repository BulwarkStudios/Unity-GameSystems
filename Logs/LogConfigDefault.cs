using BulwarkStudios.GameSystems.Contexts;
using BulwarkStudios.GameSystems.Events;
using BulwarkStudios.GameSystems.Libraries;

namespace BulwarkStudios.GameSystems.Logs {

    public class LogConfigDefault : LogConfig {

        #region Overrides of LogConfig

        /// <summary>
        /// Setup the tags
        /// </summary>
        protected override void SetupTags() {
            AddTag(GameContextConstants.LOG_TAG);
            AddTag(GameEventConstants.LOG_TAG);
            AddTag(GameLibraryConstants.LOG_TAG);
        }

        /// <summary>
        /// Active the logs ?
        /// </summary>
        protected override bool ActiveLogs() {
            return true;
        }

        #endregion

    }

}
