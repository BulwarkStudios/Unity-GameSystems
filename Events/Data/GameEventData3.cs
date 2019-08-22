namespace BulwarkStudios.GameSystems.Events {

    /// <summary>
    /// Event data
    /// </summary>
    public sealed class GameEventData3<P1, P2, P3> : GameEventData {

        /// <summary>
        /// Callback
        /// </summary>
        public System.Action<P1, P2, P3> callback;

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="callback"></param>
        public GameEventData3(object listener, System.Action<P1, P2, P3> callback) : base(listener) {
            this.callback = callback;
        }

    }

}