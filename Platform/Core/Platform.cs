using UnityEngine;

namespace BulwarkStudios.GameSystems.Platform {

    public static class Platform {

        /// <summary>
        /// Current platform
        /// </summary>
        private static IPlatform current;

        /// <summary>
        /// Initialize the platform
        /// </summary>
        /// <param name="platform"></param>
        /// <param name="config"></param>
        public static void Initialize(IPlatform platform, IPlatformConfig config) {

            current = platform;

            // Unity editor
            if (current == null) {
                current = new UnknownPlatform();
                config = new UnknownPlatformConfig();
            }

            // Initialize
            try {
                current.Initialize(config);
            }
            catch (System.Exception e) {
                Debug.Log(e);
            }

        }

        /// <summary>
        /// Update
        /// </summary>
        public static void Update() {
            try {
                if (current != null) {
                    current.Update();
                }
            }
            catch (System.Exception e) {
                Debug.Log(e);
            }
        }

        /// <summary>
        /// On destroy
        /// </summary>
        public static void OnDestroy() {
            if (current != null) {
                try {
                    current.OnDestroy();
                }
                catch (System.Exception e) {
                    Debug.Log(e);
                }           
            }
        }

        /// <summary>
        /// Get the platform
        /// </summary>
        /// <returns></returns>
        public static IPlatform Get() {
            if (current == null) {
                return new UnknownPlatform();
            }

            return current;
        }

    }

}