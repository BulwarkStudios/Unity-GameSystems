using BulwarkStudios.GameSystems.Logs;

namespace BulwarkStudios.Tests {

    public class LogConfigTest : LogConfigDefault {

        /// <summary>
        /// Log tags
        /// </summary>
        public enum TAG {
            DEFAULT, TASK, SAVE, UI,
        }

        #region Overrides of LogConfig

        /// <summary>
        /// Setup the tags
        /// </summary>
        protected override void SetupTags() {
            base.SetupTags();
            AddTag(TAG.DEFAULT.ToString());
            AddTag(TAG.TASK.ToString());
            AddTag(TAG.SAVE.ToString());
            AddTag(TAG.UI.ToString());
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
