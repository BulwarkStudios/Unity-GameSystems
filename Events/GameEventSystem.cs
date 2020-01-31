using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BulwarkStudios.GameSystems.Logs;
using BulwarkStudios.GameSystems.Utils;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BulwarkStudios.GameSystems.Events {

    [GlobalConfig(GameEventConstants.RESOURCE_GAMESYSTEM_EVENT_FOLDER)]
    public class GameEventSystem : GlobalConfigResourcesFolder<GameEventSystem> {

        /// <summary>
        /// List of all contexts
        /// </summary>

        //[ReadOnly]
        public List<ScriptableObject> availableEvents = new List<ScriptableObject>();

        /// <summary>
        /// Initialized?
        /// </summary>
        [ReadOnly, ShowInInspector]
        private bool initialized;

#if UNITY_EDITOR

        /// <summary>
        /// Reset the initialisation
        /// </summary>
        [InitializeOnLoadMethod]
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
        [UnityEditor.Callbacks.DidReloadScripts(-50)]
        private static void OnScriptsReloaded() {

            EditorApplication.delayCall += () => {
                EditorApplication.delayCall += () => {

                    List<ScriptableObject> data = new List<ScriptableObject>();

                    var dataChanged = false;

                    dataChanged = CheckEventType(typeof(GameEvent<>), data) || dataChanged;
                    dataChanged = CheckEventType(typeof(GameEvent<,>), data) || dataChanged;
                    dataChanged = CheckEventType(typeof(GameEvent<,,>), data) || dataChanged;
                    dataChanged = CheckEventType(typeof(GameEvent<,,,>), data) || dataChanged;
                    dataChanged = CheckEventType(typeof(GameEvent<,,,,>), data) || dataChanged;

                    if (dataChanged) {
                        Instance.availableEvents = data;
                        EditorUtility.SetDirty(Instance);
                        AssetDatabase.SaveAssets();
                    }

                    // Remove deleted events
                    var gameEventsAssetFolder = "Assets/" + GameEventConstants.RESOURCE_GAMESYSTEM_EVENT_LIST_FOLDER;
                    if (Directory.Exists(gameEventsAssetFolder)) {

                        string[] guidsAssets1 = AssetDatabase.FindAssets("t:" + typeof(System.Object).Name, new[] {
                            gameEventsAssetFolder
                        });

                        // Loop on all scriptable objects
                        for (int i = 0; i < guidsAssets1.Length; i++) {

                            string path = AssetDatabase.GUIDToAssetPath(guidsAssets1[i]);
                            UnityEngine.Object sObj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);

                            // Check if errors 
                            if (string.IsNullOrEmpty(guidsAssets1[i]) || string.IsNullOrEmpty(path) || sObj == null ||
                                string.IsNullOrEmpty(sObj.GetType().ToString()) ||
                                sObj.GetType() == typeof(UnityEngine.Object)) {

                                Debug.LogError("An event has been deleted guid: " + guidsAssets1[i] + " path: " + path +
                                               " object: " + sObj);

                                AssetDatabase.DeleteAsset(path);
                            }
                        }
                    }

                }; // End of delay call
            }; // End delay call
        }

        /// <summary>
        /// Check, find and create events
        /// </summary>
        /// <param name="eventType"></param>
        private static bool CheckEventType(Type eventType, List<ScriptableObject> data) {

            var dataChanged = false;

            // Check all context and create events
            foreach (Type type in ReflectionUtils.ListAllDerivedTypes(eventType)) {

                if (type == null) {
                    continue;
                }

                // Type already here
                bool alreadyAdded = false;
                foreach (ScriptableObject so in Instance.availableEvents) {
                     
                    if (so == null) {
                        continue;
                    }

                    if (so.GetType() == type) {
                        alreadyAdded = true;
                        data.Add(so);
                        break;
                    }
                }

                if (!alreadyAdded) {

                    // File already created?
                    string[] guids = AssetDatabase.FindAssets("t:" + typeof(ScriptableObject).Name, new[] {
                        "Assets" + Path.DirectorySeparatorChar + GameEventConstants.RESOURCE_GAMESYSTEM_EVENT_LIST_FOLDER
                    });

                    // Check file names
                    foreach (string guid in guids) {
                        string p = AssetDatabase.GUIDToAssetPath(guid);
                        ScriptableObject sObj = AssetDatabase.LoadAssetAtPath<ScriptableObject>(p);
                        if (sObj == null || !sObj.name.Contains(type.Name)) {
                            continue;
                        }

                        GameLogSystem.Info("File already exist: " + sObj, GameEventConstants.LOG_TAG);
                        data.Add(sObj);
                        alreadyAdded = true;
                        break;
                    }
                }

                // Skip if already added
                if (alreadyAdded) {
                    continue;
                }

                MethodInfo method = type.GetMethod("EditorInitialize", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

                // Add context
                if (method != null) {
                    object context = method.Invoke(null, null);
                    data.Add(context as ScriptableObject);
                    dataChanged = true;
                }
            }

            return dataChanged;
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

            GameLogSystem.Info("Initialize events", GameEventConstants.LOG_TAG);

            // Setup singletons
            foreach (ScriptableObject availableEvent in availableEvents) {
                IScriptableObjectSingleton singleton = availableEvent as IScriptableObjectSingleton;
                singleton?.SetInstance(availableEvent);
            }

        }

    }

}