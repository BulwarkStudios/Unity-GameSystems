using UnityEngine;
using BulwarkStudios.GameSystems.Contexts;
using BulwarkStudios.GameSystems.Ui;
using BulwarkStudios.GameSystems.Utils.EventTriggers;

namespace BulwarkStudios.Tests {

    public class MainMenu : MonoBehaviour, IInitializable {

        // Windows
        [SerializeField] private WindowSettings settingsWindow = null;
        [SerializeField] private WindowExit exitWindow = null;

        // Test overrided button state
        [SerializeField] private UiButton overridedButton = null;

        #region Implementation of IInitializable

        /// <summary>
        /// Initialize the object
        /// </summary>
        void IInitializable.Initialize() {

            // Get a library
            Log.Info(GameLibraryUi.Instance.testButton);

            // Test overrided states
            overridedButton.SetOverridedState(UiButton.STATE.CUSTOM1);

            // Reset the state
            //overridedButton.SetOverridedState(null);

        }

        #endregion

        private void OnEnable() {

            // Listen an event
            GameEventNewGame.Listen(this, NewGame).AddGameContextConstraint(GameContextMainMenu.Instance);
            GameEventSettingsOpen.Listen(this, SettingsOpen).AddGameContextConstraint(GameContextMainMenu.Instance);
            GameEventExitOpen.Listen(this, ExitOpen).AddGameContextConstraint(GameContextMainMenu.Instance);

            // Test events
            GameEventTest0.Listen(this, Test0);
            GameEventTest1.Listen(this, Test1);
            GameEventTest2.Listen(this, Test2);
            GameEventTest3.Listen(this, Test3);
            GameEventTest4.Listen(this, Test4);

        }

        private void OnDisable() {

            // Unlisten an event
            GameEventSettingsOpen.Unlisten(this, SettingsOpen);
            GameEventNewGame.Unlisten(this, NewGame);
            GameEventExitOpen.Unlisten(this, ExitOpen);

            // Unlisten test event
            GameEventTest0.Unlisten(this, Test0);
            GameEventTest1.Unlisten(this, Test1);
            GameEventTest2.Unlisten(this, Test2);
            GameEventTest3.Unlisten(this, Test3);
            GameEventTest4.Unlisten(this, Test4);

        }

        /// <summary>
        /// New game
        /// </summary>
        private void NewGame() {

            GameContextSystem.SetContext(GameContextNewGame.Instance, 0);

        }

        /// <summary>
        /// Open settings
        /// </summary>
        private void SettingsOpen() {

            settingsWindow.Open();

        }

        /// <summary>
        /// Open exit
        /// </summary>
        private void ExitOpen() {

            exitWindow.Open();

        }

        private void Test0() { }
        private void Test1(int obj) {}
        private void Test2(int arg1, float arg2) {}
        private void Test3(int arg1, float arg2, int arg3) {}
        private void Test4(int arg1, float arg2, int arg3, int arg4) {}

    }
}
