using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BulwarkStudios.GameSystems.Logs;
using BulwarkStudios.GameSystems.Utils;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace BulwarkStudios.GameSystems.Libraries {

    [GlobalConfig(GameLibraryConstants.RESOURCE_GAMESYSTEM_LIBRARY_FOLDER)]
    public class GameLibrarySystem : GlobalConfigResourcesFolder<GameLibrarySystem> {

        /// <summary>
        /// List of all libraries
        /// </summary>

        //[ReadOnly]
        public List<ScriptableObject> availableLibraries = new List<ScriptableObject>();

        /// <summary>
        /// Initialized?
        /// </summary>
        [ReadOnly, ShowInInspector]
        private bool initialized;

#if UNITY_EDITOR

        /// <summary>
        /// Reset the initialisation
        /// </summary>
        [InitializeOnEnterPlayMode]
        static void OnProjectLoadedInEditor() {
            Instance.initialized = false;
            EditorApplication.playModeStateChanged -= PlayModeChanged;
            EditorApplication.playModeStateChanged += PlayModeChanged;
        }

        /// <summary>
        /// Playmode chaged
        /// </summary>
        /// <param name="mode"></param>
        private static void PlayModeChanged(PlayModeStateChange mode) {

            switch (mode) {
                case PlayModeStateChange.ExitingPlayMode:
                    Instance.initialized = false;
                    break;
            }

        }

        /// <summary>
        /// When the compiler has ended clear data
        /// </summary>
        [Button("Scan", ButtonSizes.Medium)]
        public static void Scan() {

            string log = "Scan Game System Libraries in project...\n";

            List<ScriptableObject> data = new List<ScriptableObject>();

            bool dataModified = false;
            string assetListFolder = "Assets/" + GameLibraryConstants.RESOURCE_GAMESYSTEM_LIBRARY_LIST_FOLDER;

            // Clean available
            for (int i = Instance.availableLibraries.Count - 1; i >= 0; i--) {
                if (Instance.availableLibraries[i] == null) {
                    Instance.availableLibraries.RemoveAt(i);
                    log += "Clean a null object in the available list.\n";
                }
            }

            Instance.availableLibraries = Instance.availableLibraries.Distinct().ToList();

            // Check all context and create libraries
            foreach (Type type in ReflectionUtils.ListAllDerivedTypes(typeof(GameLibrary<>))) {

                if (type == null) {
                    continue;
                }

                if (type.ContainsGenericParameters) {
                    continue;
                }

                // Type already here
                bool alreadyAdded = false;

                foreach (ScriptableObject so in Instance.availableLibraries) {

                    if (so == null) {
                        continue;
                    }

                    if (so.GetType() == type) {
                        alreadyAdded = true;
                        data.Add(so);
                        log += "The library " + so.name + " is already in the list.\n";
                        break;
                    }
                }

                if (Directory.Exists(assetListFolder) && !alreadyAdded) {

                    // File already created?
                    string[] guids = AssetDatabase.FindAssets("t:" + typeof(ScriptableObject).Name, new[] {
                        assetListFolder
                    });

                    // Check file names
                    foreach (string guid in guids) {
                        string p = AssetDatabase.GUIDToAssetPath(guid);
                        ScriptableObject sObj = AssetDatabase.LoadAssetAtPath<ScriptableObject>(p);

                        if (sObj == null || !sObj.name.Contains(type.Name)) {
                            continue;
                        }

                        log += "File already exist: " + sObj + " Added?: " + !ContainsFile(sObj.name) + "\n";

                        if (!ContainsFile(sObj.name)) {
                            data.Add(sObj);
                            dataModified = true;
                        }

                        alreadyAdded = true;
                        break;
                    }
                }

                // Skip if already added
                if (alreadyAdded) {
                    continue;
                }

                MethodInfo method = type.GetMethod("EditorInitialize",
                    BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static |
                    BindingFlags.FlattenHierarchy);

                // Add context
                if (method != null) {
                    dataModified = true;
                    object context = method.Invoke(null, null);
                    data.Add(context as ScriptableObject);

                    log += "New type added: " + context.GetType() + "\n";

                }
            }

            // Remove deleted events
            if (Directory.Exists(assetListFolder)) {

                string[] guidsAssets1 = AssetDatabase.FindAssets("t:" + typeof(System.Object).Name, new[] {
                    assetListFolder
                });

                // Loop on all scriptable objects
                for (int i = 0; i < guidsAssets1.Length; i++) {

                    string path = AssetDatabase.GUIDToAssetPath(guidsAssets1[i]);
                    UnityEngine.Object sObj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);

                    // Check if errors 
                    if (string.IsNullOrEmpty(guidsAssets1[i]) || string.IsNullOrEmpty(path) || sObj == null ||
                        string.IsNullOrEmpty(sObj.GetType().ToString()) ||
                        sObj.GetType() == typeof(UnityEngine.Object)) {

                        log += "A library has been deleted guid: " + guidsAssets1[i] + " path: " +
                               path + " object: " + sObj + "\n";

                        AssetDatabase.DeleteAsset(path);
                        dataModified = true;
                    }
                }
            }

            if (dataModified) {
                Instance.availableLibraries = data;
                EditorUtility.SetDirty(Instance);
                AssetDatabase.SaveAssets();
            }

            log += "Scan done.\n";

            Debug.Log(log);

        }

        /// <summary>
        /// Available data contains the file?
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private static bool ContainsFile(string file) {

            foreach (ScriptableObject so in Instance.availableLibraries) {
                if (so == null) {
                    continue;
                }
                if (file.Equals(so.name)) {
                    return true;
                }
            }

            return false;

        }

#endif

        /// <summary>
        /// Load all available objects
        /// </summary>
        public static void Load() {
            Instance.Initialize();
        }

        /// <summary>
        /// Initialize the context system
        /// </summary>
        private void Initialize() {

            // Already initialized?
            if (Instance.initialized) {
                return;
            }

            // Initialize
            initialized = true;

            GameLogSystem.Info("Initialize libraries", GameLibraryConstants.LOG_TAG);

            // Setup singletons
            foreach (ScriptableObject availableLibrary in availableLibraries) {
                IScriptableObjectSingleton singleton = availableLibrary as IScriptableObjectSingleton;
                singleton?.SetInstance(availableLibrary);
            }

        }

    }

}