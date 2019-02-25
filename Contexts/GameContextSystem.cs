using System;
using System.Collections.Generic;
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
        [ReadOnly]
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
        /// When the compiler has ended clear data
        /// </summary>
        [UnityEditor.Callbacks.DidReloadScripts(-50)]
        private static void OnScriptsReloaded() {

            EditorApplication.delayCall += () => {

                List<ScriptableObject> data = new List<ScriptableObject>();

                // Check all context and create contexts
                foreach (Type type in ReflectionUtils.ListAllDerviedTypes(typeof(GameContext<>))) {

                    if (type == null) {
                        continue;
                    }

                    // Type already here
                    bool alreadyAdded = false;
                    foreach (ScriptableObject soEvent in Instance.availableContexts) {

                        if (soEvent == null) {
                            continue;
                        }

                        if (soEvent.GetType() == type) {
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

        }
#endif

        /// <summary>
        /// Load all available objects
        /// </summary>
        public static void Load() {

            Instance.Initialize(true);

            GameLogSystem.Info("Load all contexts", GameContextConstants.LOG_TAG);

            foreach (ScriptableObject context in Instance.availableContexts) { 
                IGameContext gContext = context as IGameContext;
                gContext?.Load();
            }

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
        private void Initialize(bool force = false) {

            // Already initialized?
            if (!force && Instance.initialized) {
                return;
            }

            GameLogSystem.Info("Initialiaze contexts", GameContextConstants.LOG_TAG);

            // Data
            ResetContexts();

            // Initialize
            initialized = true;

        }

        /// <summary>
        /// Reset the contexts
        /// </summary>
        public static void ResetContexts() {

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

            // Active new context
            context.Enable(index);

            // Update context
            OnUpdateContext?.Invoke();

        }

        /// <summary>
        /// Add a context layer
        /// <param name="context"></param>
        /// <param name="index"></param>
        /// </summary>
        public static void AddLayer(IGameContext context, INDEX index) {

            AddLayer();

            SetContext(context, index);

        }

        /// <summary>
        /// Add a context layer
        /// </summary>
        public static void AddLayer() {

            GameLogSystem.Info("Context add layer", GameContextConstants.LOG_TAG);

            Instance.layer++;

            // Update context
            OnUpdateContext?.Invoke();

        }

        /// <summary>
        /// Add a context layer
        /// </summary>
        public static void RemoveLayer(bool resetContexts = true) {

            GameLogSystem.Info("Context remove layer", GameContextConstants.LOG_TAG);

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

            data += "Context initialized: " + Instance.initialized + "\n";
            data += "Context current layer: " + Instance.layer + "\n";

            data += "Contexts: " + "\n";
            foreach (INDEX index in Enum.GetValues(typeof(INDEX))) {
                if (Instance.contexts == null || Instance.contexts.Count < (int)index) {
                    continue;
                }
                data += index + ": " + Instance.contexts[(int)index] + "\n";
            }

            data += "Layer 1: " + "\n";
            foreach (INDEX index in Enum.GetValues(typeof(INDEX))) {
                if (Instance.layer1Contexts == null || Instance.layer1Contexts.Count < (int)index) {
                    continue;
                }
                data += index + ": " + Instance.layer1Contexts[(int)index] + "\n";
            }

            data += "Layer 2: " + "\n";
            foreach (INDEX index in Enum.GetValues(typeof(INDEX))) {
                if (Instance.layer2Contexts == null || Instance.layer2Contexts.Count < (int)index) {
                    continue;
                }
                data += index + ": " + Instance.layer2Contexts[(int)index] + "\n";
            }

            data += "Layer 3: " + "\n";
            foreach (INDEX index in Enum.GetValues(typeof(INDEX))) {
                if (Instance.layer3Contexts == null || Instance.layer3Contexts.Count < (int)index) {
                    continue;
                }
                data += index + ": " + Instance.layer3Contexts[(int)index] + "\n";
            }

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
