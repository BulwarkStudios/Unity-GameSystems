using System.Collections;
using UnityEngine;
using BulwarkStudios.GameSystems.Contexts;
using BulwarkStudios.GameSystems.Utils.EventTriggers;
using UnityEngine.SceneManagement;

namespace BulwarkStudios.Tests {

    public class MainMenuInitialize : MonoBehaviour {

        /// <summary>
        /// Start is called before the first frame update
        /// </summary>
        /// <returns></returns>
        private IEnumerator Start() {

            // Load the game setup scene
            if (!SceneManager.GetSceneByName(SceneConfig.GAME_SETUP_SCENE_NAME).isLoaded) {
                SceneManager.LoadScene(SceneConfig.GAME_SETUP_SCENE_NAME, LoadSceneMode.Additive);
                yield return 0f;
            }

            // Prepare objects
            foreach (GameObject rootGo in gameObject.scene.GetRootGameObjects()) {
                foreach (IPreparable child in rootGo.GetComponentsInChildren<IPreparable>(true)) {
                    child.Prepare();
                }
            }

            // Initialize objects
            foreach (GameObject rootGo in gameObject.scene.GetRootGameObjects()) {
                foreach (IInitializable child in rootGo.GetComponentsInChildren<IInitializable>(true)) {
                    child.Initialize();
                }
            }

            // Set a context
            GameContextSystem.SetContext(GameContextMainMenu.Instance, GameContextSystem.INDEX.MAIN);

        }

    }
    
}
