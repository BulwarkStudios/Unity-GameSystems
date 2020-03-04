namespace BulwarkStudios.GameSystems.Ui {

    public interface IUiButtonEffect {

        /// <summary>
        /// Preview the state
        /// </summary>
        /// <param name="newState"></param>
        void PreviewState(UiButton.STATE newState);

        /// <summary>
        /// Update the state
        /// </summary>
        /// <param name="oldState"></param>
        /// <param name="newState"></param>
        void UpdateState(UiButton.STATE oldState, UiButton.STATE newState);

    }

}