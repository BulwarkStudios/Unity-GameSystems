namespace BulwarkStudios.GameSystems.Events {

    /// <summary>
    /// Event data
    /// </summary>
    public sealed class GameEventData0 : GameEventData {

        /// <summary>
        /// Callback
        /// </summary>
        public System.Action callback;

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="callback"></param>
        public GameEventData0(object listener, System.Action callback) : base(listener) {
            this.callback = callback;
        }

    }

}