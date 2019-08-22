namespace BulwarkStudios.GameSystems.Platform {

    public struct PlatformDlcResult {

        /// <summary>
        /// Result state
        /// </summary>
        public enum STATE {

            SUCCESS,

            FAIL

        }

        /// <summary>
        /// Result state
        /// </summary>
        public STATE state;

        /// <summary>
        /// Error message
        /// </summary>
        public string error;

        /// <summary>
        /// Create a new result
        /// </summary>
        /// <param name="state"></param>
        /// <param name="error"></param>
        public PlatformDlcResult(STATE state, string error = null) {

            this.state = state;
            this.error = error;

        }

    }

}