namespace BulwarkStudios.GameSystems.Events {

    /// <summary>
    /// Event data
    /// </summary>
    public sealed class GameEventData1<P1> : GameEventData {

        /// <summary>
        /// Callback
        /// </summary>
        public System.Action<P1> callback;

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="callback"></param>
        public GameEventData1(object listener, System.Action<P1> callback) : base(listener) {
            this.callback = callback;
        }

    }

}
