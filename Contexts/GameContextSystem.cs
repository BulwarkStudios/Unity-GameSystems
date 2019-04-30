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

namespace BulwarkStudios.GameSystems.Contexts {

    [GlobalConfig(GameContextConstants.RESOURCE_GAMESYSTEM_CONTEXT_FOLDER)]
    public class GameContextSystem : GlobalConfig<GameContextSystem> {

        /// <summary>
        /// List of all contexts
        /// </summary>
        //[ReadOnly]
        public List<ScriptableObject> availableContexts = new List<ScriptableObject>();

        /// <summary>
        /// Context in the layer 0
        /// </summary>
        [ReadOnly, ShowInInspector]
        private List<ScriptableObject> contexts;

        /// <summary>
        /// Context in the layer 1
        /// </summary>
        [ReadOnly, ShowInInspector]
        private List<ScriptableObject> layer1Contexts;

        /// <summary>
        /// Context in the layer 2
        /// </summary>
        [ReadOnly, ShowInInspector]
        private List<ScriptableObject> layer2Contexts;

        /// <summary>
        /// Context in the layer 3
        /// </summary>
        [ReadOnly, ShowInInspector]
        private List<ScriptableObject> layer3Contexts;

        /// <summary>
        /// Current layer
        /// </summary>
        [ReadOnly, ShowInInspector]
        private int layer;

        /// <summary>
        /// Initialized?
        /// </summary>
        [ReadOnly, ShowInInspector]
        private bool initialized;

        /// <summary>
        /// On update context
        /// </summary>
        public static event System.Action OnUpdateContext;

        /// <summary>
        /// Context index
        /// </summary>
        public enum INDEX {
            MAIN, SUB1, SUB2, SUB3
        }

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
                    ResetContexts(false);
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

                    // Check all context and create contexts
                    foreach (Type type in ReflectionUtils.ListAllDerviedTypes(typeof(GameContext<>))) {

                        if (type == null) {
                            continue;
                        }

                        // Type already here
                        bool alreadyAdded = false;
                        foreach (ScriptableObject so in Instance.availableContexts) {

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
                                "Assets" + Path.DirectorySeparatorChar + GameContextConstants.RESOURCE_GAMESYSTEM_CONTEXT_LIST_FOLDER
                            });

                            // Check file names
                            foreach (string guid in guids) {
                                string p = AssetDatabase.GUIDToAssetPath(guid);
                                ScriptableObject sObj = AssetDatabase.LoadAssetAtPath<ScriptableObject>(p);
                                if (sObj == null || !sObj.name.Contains(type.Name)) {
                                    continue;
                                }

                                GameLogSystem.Info("File already exist: " + sObj + " Added?: " + (!ContainsFile(sObj.name)).ToString(), GameContextConstants.LOG_TAG);

                                if (!ContainsFile(sObj.name)) {
                                    data.Add(sObj);
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
                            object context = method.Invoke(null, null);
                            data.Add(context as ScriptableObject);
                        }

                    }

                    Instance.availableContexts = data;

                    // Remove deleted events
                    string[] guidsAssets1 = AssetDatabase.FindAssets("t:" + typeof(System.Object).Name, new[] {
                        "Assets/" + GameContextConstants.RESOURCE_GAMESYSTEM_CONTEXT_LIST_FOLDER
                    });

                    // Loop on all scriptable objects
                    for (int i = 0; i < guidsAssets1.Length; i++) {

                        string path = AssetDatabase.GUIDToAssetPath(guidsAssets1[i]);
                        UnityEngine.Object sObj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);

                        // Check if errors 
                        if (string.IsNullOrEmpty(guidsAssets1[i]) || string.IsNullOrEmpty(path) || sObj == null ||
                            string.IsNullOrEmpty(sObj.GetType().ToString()) ||
                            sObj.GetType() == typeof(UnityEngine.Object)) {

                            UnityEngine.Debug.LogError("A context has been deleted guid: " + guidsAssets1[i] +
                                                       " path: " +
                                                       path + " object: " + sObj);

                            AssetDatabase.DeleteAsset(path);

                        }

                    }

                    EditorUtility.SetDirty(Instance);
                    AssetDatabase.SaveAssets();

                }; // End delay call
            }; // End delay call

        }

        /// <summary>
        /// Available data contains the file?
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private static bool ContainsFile(string file) {

            foreach (ScriptableObject so in Instance.availableContexts) {
                if (file.Contains(so.name)) {
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
        /// Get the current context list
        /// </summary>
        /// <returns></returns>
        private List<ScriptableObject> GetContextList() {

            // Get the current context list depending on the layer
            switch (Instance.layer) {
                case 0:
                    return contexts;
                case 1:
                    return layer1Contexts;
                case 2:
                    return layer2Contexts;
                case 3:
                    return layer2Contexts;
            }

            GameLogSystem.Info("Context layer doesn't exist!", GameContextConstants.LOG_TAG);

            return contexts;

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

            GameLogSystem.Info("Initialiaze contexts", GameContextConstants.LOG_TAG);

            // Setup singletons
            foreach (ScriptableObject availableContext in availableContexts) {
                IScriptableObjectSingleton singleton = availableContext as IScriptableObjectSingleton;
                if (singleton == null) {
                    continue;
                }
                singleton.SetInstance(availableContext);
            }

            // Data
            ResetContexts(false);

        }

        /// <summary>
        /// Reset the contexts
        /// </summary>
        public static void ResetContexts(bool disable = true) {

            // Disable old contexts
            if (disable) {

                foreach (INDEX index in Enum.GetValues(typeof(INDEX))) {
                    IGameContext context = Instance.contexts[(int)index] as IGameContext;
                    if (context != null) {
                        context.Disable(index);
                    }
                    IGameContext layer1 = Instance.layer1Contexts[(int)index] as IGameContext;
                    if (layer1 != null) {
                        layer1.Disable(index);
                    }
                    IGameContext layer2 = Instance.layer2Contexts[(int)index] as IGameContext;
                    if (layer2 != null) {
                        layer2.Disable(index);
                    }
                    IGameContext layer3 = Instance.layer3Contexts[(int)index] as IGameContext;
                    if (layer3 != null) {
                        layer3.Disable(index);
                    }
                }

            }

            // Data
            Instance.contexts = new List<ScriptableObject>();
            Instance.layer1Contexts = new List<ScriptableObject>();
            Instance.layer2Contexts = new List<ScriptableObject>();
            Instance.layer3Contexts = new List<ScriptableObject>();
            Instance.layer = 0;

            // Set default context
            foreach (INDEX index in Enum.GetValues(typeof(INDEX))) {
                Instance.contexts.Add(GameContextNone.Instance);
                Instance.layer1Contexts.Add(GameContextNone.Instance);
                Instance.layer2Contexts.Add(GameContextNone.Instance);
                Instance.layer3Contexts.Add(GameContextNone.Instance);
            }

            // Update context
            OnUpdateContext?.Invoke();

        }

        /// <summary>
        /// Set a context
        /// </summary>
        /// <param name="context"></param>
        /// <param name="index"></param>
        public static void SetContext(IGameContext context, INDEX index) {

            // Initialize
            Instance.Initialize();

            // Context null ?
            if (context == null) {
                return;
            }

            GameLogSystem.Info("Set Context: " + context + " at index " + index, GameContextConstants.LOG_TAG);

            // Old context
            IGameContext oldContext = Instance.GetContextList()[(int)index] as IGameContext;

            // Disable old context
            oldContext?.Disable(index);

            // Save ref
            Instance.GetContextList()[(int)index] = context as ScriptableObject;

            // Update context
            OnUpdateContext?.Invoke();

            // Active new context
            context.Enable(index);

        }

        /// <summary>
        /// Add a context layer
        /// <param name="context"></param>
        /// <param name="index"></param>
        /// </summary>
        public static void AddLayer(IGameContext context, INDEX index) {

            AddLayer(false);

            SetContext(context, index);

        }

        /// <summary>
        /// Add a context layer
        /// </summary>
        public static void AddLayer(bool triggerOnUpdateContext = true) {

            GameLogSystem.Info("Context add layer", GameContextConstants.LOG_TAG);

            Instance.layer++;

            // Update context
            if (triggerOnUpdateContext) {
                OnUpdateContext?.Invoke();
            }

        }

        /// <summary>
        /// Add a context layer
        /// </summary>
        public static void RemoveLayer(IGameContext context, bool resetContexts = true) {

            GameLogSystem.Info("Context remove layer", GameContextConstants.LOG_TAG);

            if (context != null) {
                if (!HasContext(context.GetScriptableObject())) {
                    GameLogSystem.Info("Context remove layer didnot have the requested context", GameContextConstants.LOG_TAG);
                    return;
                }
            }

            // Disable
            DisableCurrentContexts();

            // Reset contexts
            if (resetContexts) {
                Instance.GetContextList().Clear();
                foreach (INDEX index in Enum.GetValues(typeof(INDEX))) {
                    Instance.GetContextList().Add(GameContextNone.Instance);
                }
            }

            Instance.layer--;

            // Update context
            OnUpdateContext?.Invoke();

            // Enable
            EnableCurrentContexts();

        }

        /// <summary>
        /// Enable current contexts
        /// </summary>
        private static void EnableCurrentContexts() {

            foreach (INDEX index in Enum.GetValues(typeof(INDEX))) {
                IGameContext context = Instance.GetContextList()[(int)index] as IGameContext;
                context?.Enable(index);
            }

        }

        /// <summary>
        /// Disable current contexts
        /// </summary>
        private static void DisableCurrentContexts() {

            foreach (INDEX index in Enum.GetValues(typeof(INDEX))) {
                IGameContext context = Instance.GetContextList()[(int)index] as IGameContext;
                context?.Disable(index);
            }

        }

        /// <summary>
        /// Has contexts?
        /// </summary>
        /// <param name="contexts"></param>
        /// <returns></returns>
        public static bool HasContexts(IEnumerable<ScriptableObject> contexts) {

            foreach (ScriptableObject gameContext in contexts) {
                if (!HasContext(gameContext)) {
                    return false;
                }
            }

            return true;

        }

        /// <summary>
        /// Has a context?
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool HasContext(IGameContext context) {

            if (HasContext(context.GetScriptableObject())) {
                return true;
            }

            return false;

        }

        /// <summary>
        /// Has a context?
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static bool HasContext(ScriptableObject context) {

            if (Instance.GetContextList().Contains(context)) {
                return true;
            }

            return false;

        }

        /// <summary>
        /// Get a string containing all contexts
        /// </summary>
        /// <returns></returns>
        public static string GetLogStringContexts() {

            string data = string.Empty;

            data += "Context instance: " + Instance + "\n";

            if (Instance == null) {
                return data;
            }

            data += "Context available: " + Instance.availableContexts + "\n";
            if (Instance.availableContexts != null) {
                foreach (ScriptableObject so in Instance.availableContexts) {
                    data += so.GetHashCode() + " : " + so + "\n";
                }
            }
            else {
                data += "Context available: null \n";
            }

            data += "\n";

            data += "Context initialized: " + Instance.initialized + "\n";
            data += "Context current layer: " + Instance.layer + "\n";

            data += "\n";

            data += "Contexts: " + "\n";
            foreach (INDEX index in Enum.GetValues(typeof(INDEX))) {
                if (Instance.contexts == null || Instance.contexts.Count < (int)index) {
                    continue;
                }

                if (Instance.contexts[(int)index] == null) {
                    continue;
                }
                data += index + ": " + Instance.contexts[(int)index].GetHashCode() + " " + Instance.contexts[(int)index] + "\n";
            }
            data += "\n";

            data += "Layer 1: " + "\n";
            foreach (INDEX index in Enum.GetValues(typeof(INDEX))) {
                if (Instance.layer1Contexts == null || Instance.layer1Contexts.Count < (int)index) {
                    continue;
                }
                if (Instance.layer1Contexts[(int)index] == null) {
                    continue;
                }
                data += index + ": " + Instance.layer1Contexts[(int)index].GetHashCode() + " " + Instance.layer1Contexts[(int)index] + "\n";
            }
            data += "\n";

            data += "Layer 2: " + "\n";
            foreach (INDEX index in Enum.GetValues(typeof(INDEX))) {
                if (Instance.layer2Contexts == null || Instance.layer2Contexts.Count < (int)index) {
                    continue;
                }
                if (Instance.layer2Contexts[(int)index] == null) {
                    continue;
                }
                data += index + ": " + Instance.layer2Contexts[(int)index].GetHashCode() + " " + Instance.layer2Contexts[(int)index] + "\n";
            }
            data += "\n";

            data += "Layer 3: " + "\n";
            foreach (INDEX index in Enum.GetValues(typeof(INDEX))) {
                if (Instance.layer3Contexts == null || Instance.layer3Contexts.Count < (int)index) {
                    continue;
                }
                if (Instance.layer3Contexts[(int)index] == null) {
                    continue;
                }
                data += index + ": " + Instance.layer3Contexts[(int)index].GetHashCode() + " " + Instance.layer3Contexts[(int)index] + "\n";
            }
            data += "\n";

            return data;

        }

        /// <summary>
        /// Update the context
        /// </summary>
        [Button(nameof(EditorUpdateContext), ButtonSizes.Medium)]
        private void EditorUpdateContext(IGameContext context, INDEX index) {
            SetContext(context, index);
        }

        /// <summary>
        /// Update the layer
        /// </summary>
        [Button(nameof(EditorUpdateLayer), ButtonSizes.Medium)]
        private void EditorUpdateLayer(int layer) {
            this.layer = layer;
        }

    }

}
