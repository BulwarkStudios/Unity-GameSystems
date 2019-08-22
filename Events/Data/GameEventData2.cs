namespace BulwarkStudios.GameSystems.Events {

    /// <summary>
    /// Event data
    /// </summary>
    public sealed class GameEventData2<P1, P2> : GameEventData {

        /// <summary>
        /// Callback
        /// </summary>
        public System.Action<P1, P2> callback;

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="callback"></param>
        public GameEventData2(object listener, System.Action<P1, P2> callback) : base(listener) {
            this.callback = callback;
        }

    }

}