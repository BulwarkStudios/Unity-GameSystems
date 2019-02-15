namespace BulwarkStudios.GameSystems.Events {

    /// <summary>
    /// Event data
    /// </summary>
    public sealed class GameEventData4<P1, P2, P3, P4> : GameEventData {

        /// <summary>
        /// Callback
        /// </summary>
        public System.Action<P1, P2, P3, P4> callback;

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="callback"></param>
        public GameEventData4(object listener, System.Action<P1, P2, P3, P4> callback) : base(listener) {
            this.callback = callback;
        }

    }

}
