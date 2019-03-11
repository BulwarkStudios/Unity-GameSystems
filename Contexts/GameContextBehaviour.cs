namespace BulwarkStudios.GameSystems.Contexts {

    public abstract class GameContextBehaviour {

        /// <summary>
        /// Context enabled
        /// </summary>
        /// <param name="index"></param>
        public abstract void Enable(GameContextSystem.INDEX index);

        /// <summary>
        /// Context disabled
        /// </summary>
        /// <param name="index"></param>
        public abstract void Disable(GameContextSystem.INDEX index);

    }

}
