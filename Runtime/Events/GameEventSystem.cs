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

        private static string log;

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
                    UnlistenAll();
                    log = string.Empty;
                    break;
            }

        }

        /// <summary>
        /// When the compiler has ended clear data
        /// </summary>
        [Button("Scan", ButtonSizes.Medium)]
        public static void Scan() {

            log = "Scan Game System Events in project...\n";

            List<ScriptableObject> data = new List<ScriptableObject>();

            bool dataChanged = false;

            // Clean available
            for (int i = Instance.availableEvents.Count - 1; i >= 0; i--) {
                if (Instance.availableEvents[i] == null) {
                    Instance.availableEvents.RemoveAt(i);
                    log += "Clean a null object in the available list.\n";
                }
            }

            Instance.availableEvents = Instance.availableEvents.Distinct().ToList();

            dataChanged = CheckEventType(typeof(GameEvent<>), data) || dataChanged;
            dataChanged = CheckEventType(typeof(GameEvent<,>), data) || dataChanged;
            dataChanged = CheckEventType(typeof(GameEvent<,,>), data) || dataChanged;
            dataChanged = CheckEventType(typeof(GameEvent<,,,>), data) || dataChanged;
            dataChanged = CheckEventType(typeof(GameEvent<,,,,>), data) || dataChanged;

            // Remove deleted events
            string gameEventsAssetFolder = "Assets/" + GameEventConstants.RESOURCE_GAMESYSTEM_EVENT_LIST_FOLDER;

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

                        log += "An event has been deleted guid: " + guidsAssets1[i] + " path: " + path +
                               " object: " + sObj + "\n";

                        AssetDatabase.DeleteAsset(path);
                        dataChanged = true;
                    }
                }
            }

            if (dataChanged) {
                Instance.availableEvents = data;
                EditorUtility.SetDirty(Instance);
                AssetDatabase.SaveAssets();
            }

            log += "Scan done.\n";

            Debug.Log(log);

        }

        /// <summary>
        /// Check, find and create events
        /// </summary>
        /// <param name="eventType"></param>
        private static bool CheckEventType(Type eventType, List<ScriptableObject> data) {

            bool dataChanged = false;

            // Check all context and create events
            foreach (Type type in ReflectionUtils.ListAllDerivedTypes(eventType)) {

                if (type == null) {
                    continue;
                }

                if (type.ContainsGenericParameters) {
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
                        log += "The event " + so.name + " is already in the list.\n";
                        break;
                    }
                }

                if (!alreadyAdded) {

                    // File already created?
                    string[] guids = AssetDatabase.FindAssets("t:" + typeof(ScriptableObject).Name, new[] {
                        "Assets" + Path.DirectorySeparatorChar +
                        GameEventConstants.RESOURCE_GAMESYSTEM_EVENT_LIST_FOLDER
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
                            dataChanged = true;
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
                    BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

                // Add context
                if (method != null) {
                    object context = method.Invoke(null, null);
                    data.Add(context as ScriptableObject);
                    dataChanged = true;

                    log += "New type added: " + context.GetType() + "\n";
                }
            }

            return dataChanged;
        }
        
        /// <summary>
        /// Available data contains the file?
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private static bool ContainsFile(string file) {

            foreach (ScriptableObject so in Instance.availableEvents) {
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
        /// Unlisten all events
        /// </summary>
        public static void UnlistenAll() {

            foreach (ScriptableObject sEvent in Instance.availableEvents) {
                
                IGameEvent gEvent = sEvent as IGameEvent;
                gEvent?.UnlistenAll();

            }

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