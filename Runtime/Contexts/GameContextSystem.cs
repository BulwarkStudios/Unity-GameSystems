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
using JetBrains.Annotations;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace BulwarkStudios.GameSystems.Contexts {

    [GlobalConfig(GameContextConstants.RESOURCE_GAMESYSTEM_CONTEXT_FOLDER)]
    public class GameContextSystem : GlobalConfigResourcesFolder<GameContextSystem> {

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
        /// Context in the layer 4
        /// </summary>
        [ReadOnly, ShowInInspector]
        private List<ScriptableObject> layer4Contexts;

        /// <summary>
        /// Context in the layer 5
        /// </summary>
        [ReadOnly, ShowInInspector]
        private List<ScriptableObject> layer5Contexts;

        /// <summary>
        /// Context in the layer 6
        /// </summary>
        [ReadOnly, ShowInInspector]
        private List<ScriptableObject> layer6Contexts;

        /// <summary>
        /// Context in the layer 7
        /// </summary>
        [ReadOnly, ShowInInspector]
        private List<ScriptableObject> layer7Contexts;

        /// <summary>
        /// Context in the layer 8
        /// </summary>
        [ReadOnly, ShowInInspector]
        private List<ScriptableObject> layer8Contexts;

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

            MAIN,

            SUB1,

            SUB2,

            SUB3

        }

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
                    ResetContexts(false);
                    break;
            }

        }

        /// <summary>
        /// When the compiler has ended clear data
        /// </summary>
        [Button("Scan", ButtonSizes.Medium)]
        public static void Scan() {

            string log = "Scan Game System Contexts in project...\n";

            GameContextSystem instance = GetAssetInstance();

            bool dataModified = false;

            List<ScriptableObject> data = new List<ScriptableObject>();

            // Clean available
            for (int i = Instance.availableContexts.Count - 1; i >= 0; i--) {
                if (Instance.availableContexts[i] == null) {
                    Instance.availableContexts.RemoveAt(i);
                    log += "Clean a null object in the available list.\n";
                }
            }

            Instance.availableContexts = Instance.availableContexts.Distinct().ToList();

            // Check all context and create contexts
            foreach (Type type in ReflectionUtils.ListAllDerivedTypes(typeof(GameContext<>))) {

                if (type == null) {
                    continue;
                }

                if (type.ContainsGenericParameters) {
                    continue;
                }

                // Type already here
                bool alreadyAdded = false;

                foreach (ScriptableObject so in instance.availableContexts) {

                    if (so == null) {
                        continue;
                    }

                    if (so.GetType() == type) {
                        alreadyAdded = true;
                        data.Add(so);
                        log += "The context " + so.name + " is already in the list.\n";
                        break;
                    }
                }

                if (!alreadyAdded) {

                    // File already created?
                    string[] guids = AssetDatabase.FindAssets("t:" + typeof(ScriptableObject).Name, new[] {
                        "Assets" + Path.DirectorySeparatorChar +
                        GameContextConstants.RESOURCE_GAMESYSTEM_CONTEXT_LIST_FOLDER
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
            string[] guidsAssets1 = AssetDatabase.FindAssets("t:" + typeof(System.Object).Name, new[] {
                "Assets/" + GameContextConstants.RESOURCE_GAMESYSTEM_CONTEXT_LIST_FOLDER
            });

            // Loop on all scriptable objects
            for (int i = 0; i < guidsAssets1.Length; i++) {

                string path = AssetDatabase.GUIDToAssetPath(guidsAssets1[i]);
                UnityEngine.Object sObj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);

                // Check if errors 
                if (string.IsNullOrEmpty(guidsAssets1[i])
                    || string.IsNullOrEmpty(path)
                    || (sObj == null)
                    || string.IsNullOrEmpty(sObj.GetType().ToString())
                    || (sObj.GetType() == typeof(UnityEngine.Object))) {

                    log += "A context has been deleted guid: " + guidsAssets1[i] +
                           " path: " + path + " object: " + sObj + "\n";

                    AssetDatabase.DeleteAsset(path);
                    dataModified = true;
                }
            }

            if (dataModified) {
                instance.availableContexts = data;
                EditorUtility.SetDirty(instance);
                AssetDatabase.SaveAssets();
            }

            log += "Scan done.\n";

            Debug.Log(log);

        }

        private static GameContextSystem GetAssetInstance() {
            string[] systemGuids = AssetDatabase.FindAssets("t:" + typeof(GameContextSystem).Name);
            Debug.Assert(systemGuids.Length == 1);
            string instancePath = AssetDatabase.GUIDToAssetPath(systemGuids[0]);
            return AssetDatabase.LoadAssetAtPath<GameContextSystem>(instancePath);
        }

        /// <summary>
        /// Available data contains the file?
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private static bool ContainsFile(string file) {

            foreach (ScriptableObject so in Instance.availableContexts) {
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
        /// Get the current context list
        /// </summary>
        /// <returns></returns>
        private List<ScriptableObject> GetContextList(int layerIndex = -1) {

            // Get the index
            if (layerIndex < 0) {
                layerIndex = Instance.layer;
            }

            // Get the current context list depending on the layer
            switch (layerIndex) {
                case 0:
                    return contexts;

                case 1:
                    return layer1Contexts;

                case 2:
                    return layer2Contexts;

                case 3:
                    return layer3Contexts;

                case 4:
                    return layer4Contexts;

                case 5:
                    return layer5Contexts;

                case 6:
                    return layer6Contexts;

                case 7:
                    return layer7Contexts;

                case 8:
                    return layer8Contexts;
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

            GameLogSystem.Info("Initialize contexts", GameContextConstants.LOG_TAG);

            // Setup singletons
            foreach (ScriptableObject availableContext in availableContexts) {
                IScriptableObjectSingleton singleton = availableContext as IScriptableObjectSingleton;
                singleton?.SetInstance(availableContext);
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

                    IGameContext context = Instance.contexts[(int) index] as IGameContext;
                    context?.Disable(index);

                    IGameContext layer1 = Instance.layer1Contexts[(int) index] as IGameContext;
                    layer1?.Disable(index);

                    IGameContext layer2 = Instance.layer2Contexts[(int) index] as IGameContext;
                    layer2?.Disable(index);

                    IGameContext layer3 = Instance.layer3Contexts[(int) index] as IGameContext;
                    layer3?.Disable(index);

                    IGameContext layer4 = Instance.layer4Contexts[(int) index] as IGameContext;
                    layer4?.Disable(index);

                    IGameContext layer5 = Instance.layer5Contexts[(int) index] as IGameContext;
                    layer5?.Disable(index);

                    IGameContext layer6 = Instance.layer6Contexts[(int) index] as IGameContext;
                    layer6?.Disable(index);

                    IGameContext layer7 = Instance.layer7Contexts[(int) index] as IGameContext;
                    layer7?.Disable(index);

                    IGameContext layer8 = Instance.layer8Contexts[(int) index] as IGameContext;
                    layer8?.Disable(index);

                }

            }

            // Data
            Instance.contexts = new List<ScriptableObject>();
            Instance.layer1Contexts = new List<ScriptableObject>();
            Instance.layer2Contexts = new List<ScriptableObject>();
            Instance.layer3Contexts = new List<ScriptableObject>();
            Instance.layer4Contexts = new List<ScriptableObject>();
            Instance.layer5Contexts = new List<ScriptableObject>();
            Instance.layer6Contexts = new List<ScriptableObject>();
            Instance.layer7Contexts = new List<ScriptableObject>();
            Instance.layer8Contexts = new List<ScriptableObject>();
            Instance.layer = 0;

            // Set default context
            foreach (INDEX index in Enum.GetValues(typeof(INDEX))) {
                Instance.contexts.Add(GameContextNone.Instance);
                Instance.layer1Contexts.Add(GameContextNone.Instance);
                Instance.layer2Contexts.Add(GameContextNone.Instance);
                Instance.layer3Contexts.Add(GameContextNone.Instance);
                Instance.layer4Contexts.Add(GameContextNone.Instance);
                Instance.layer5Contexts.Add(GameContextNone.Instance);
                Instance.layer6Contexts.Add(GameContextNone.Instance);
                Instance.layer7Contexts.Add(GameContextNone.Instance);
                Instance.layer8Contexts.Add(GameContextNone.Instance);
            }

            // Update context
            OnUpdateContext?.Invoke();

        }

        /// <summary>
        /// Set a context to the first layer
        /// </summary>
        /// <param name="context"></param>
        /// <param name="index"></param>
        public static void SetFirstLayerContext(IGameContext context, INDEX index) {

            // Initialize
            Instance.Initialize();

            // Context null ?
            if (context == null) {
                return;
            }

            // If the current layer is already the first, just set the context normally
            if (Instance.layer == 0) {
                SetContext(context, index);
                return;
            }

            // Set the context
            Instance.GetContextList(0)[(int) index] = context as ScriptableObject;

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
            IGameContext oldContext = Instance.GetContextList()[(int) index] as IGameContext;

            // Disable old context
            oldContext?.Disable(index);

            // Save ref
            Instance.GetContextList()[(int) index] = context as ScriptableObject;

            // Update context
            OnUpdateContext?.Invoke();

            // Active new context
            context.Enable(index);

        }

        /// <summary>
        /// Add a temporary context layer
        /// </summary>
        public static void AddTemporaryLayer() {

            AddEmptyLayer(false);

            SetContext(GameContextTemporary.Instance, INDEX.MAIN);

        }

        /// <summary>
        /// Add a context layer
        /// <param name="context"></param>
        /// <param name="index"></param>
        /// </summary>
        public static void AddLayer(IGameContext context, INDEX index = INDEX.MAIN) {

            AddEmptyLayer(false);

            SetContext(context, index);

        }

        /// <summary>
        /// Add a context layer
        /// </summary>
        public static void AddEmptyLayer(bool triggerOnUpdateContext = true) {

            GameLogSystem.Info("Context add layer", GameContextConstants.LOG_TAG);

            Instance.layer++;

            // Update context
            if (triggerOnUpdateContext) {
                OnUpdateContext?.Invoke();
            }

        }

        /// <summary>
        /// Remove a context layer
        /// </summary>
        public static void RemoveLayer(IGameContext context, bool resetContexts = true) {

            if (context != null) {

                GameLogSystem.Info("Context remove layer " + context.GetScriptableObject().name,
                    GameContextConstants.LOG_TAG);

                if (!HasContext(context.GetScriptableObject())) {
                    GameLogSystem.Info("Context remove layer didnot have the requested context",
                        GameContextConstants.LOG_TAG);

                    return;
                }
            }

            // Disable
            DisableCurrentContexts(true);

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
                IGameContext context = Instance.GetContextList()[(int) index] as IGameContext;
                context?.Enable(index);
            }

        }

        /// <summary>
        /// Disable current contexts
        /// </summary>
        private static void DisableCurrentContexts(bool removeLayer) {

            // Do not disable if the previous layer has a Temporary context
            bool disableContext = true;

            if (removeLayer && Instance.layer > 1) {

                if (HasContext((IGameContext) GameContextTemporary.Instance, Instance.layer - 1)) {
                    disableContext = false;
                }

            }

            foreach (INDEX index in Enum.GetValues(typeof(INDEX))) {
                IGameContext context = Instance.GetContextList()[(int) index] as IGameContext;

                if (context == null) {
                    continue;
                }

                if (disableContext) {
                    context.Disable(index);
                }

                if (removeLayer) {
                    context.RemoveLayer();
                }
            }

        }

        /// <summary>
        /// Has contexts?
        /// </summary>
        /// <param name="contexts"></param>
        /// <returns></returns>
        public static bool HasContexts(IEnumerable<ScriptableObject> contexts, int layerIndex = -1) {

            foreach (ScriptableObject gameContext in contexts) {
                if (!HasContext(gameContext, layerIndex)) {
                    return false;
                }
            }

            return true;

        }

        /// <summary>
        /// Has a context?
        /// </summary>
        /// <param name="context"></param>
        /// <param name="layerIndex"></param>
        /// <returns></returns>
        public static bool HasContext([NotNull] IGameContext context, int layerIndex = -1) {

            if (HasContext(context.GetScriptableObject(), layerIndex)) {
                return true;
            }

            return false;

        }

        /// <summary>
        /// Has a context?
        /// </summary>
        /// <param name="context"></param>
        /// <param name="layerIndex"></param>
        /// <returns></returns>
        private static bool HasContext(ScriptableObject context, int layerIndex = -1) {

            if (Instance == null || Instance.GetContextList(layerIndex) == null) {
                return false;
            }

            if (Instance.GetContextList(layerIndex).Contains(context)) {
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
                if (Instance.contexts == null || Instance.contexts.Count < (int) index) {
                    continue;
                }

                if (Instance.contexts[(int) index] == null) {
                    continue;
                }

                data += index + ": " + Instance.contexts[(int) index].GetHashCode() + " " +
                        Instance.contexts[(int) index] + "\n";
            }

            data += "\n";

            data += "Layer 1: " + "\n";

            foreach (INDEX index in Enum.GetValues(typeof(INDEX))) {
                if (Instance.layer1Contexts == null || Instance.layer1Contexts.Count < (int) index) {
                    continue;
                }

                if (Instance.layer1Contexts[(int) index] == null) {
                    continue;
                }

                data += index + ": " + Instance.layer1Contexts[(int) index].GetHashCode() + " " +
                        Instance.layer1Contexts[(int) index] + "\n";
            }

            data += "\n";

            data += "Layer 2: " + "\n";

            foreach (INDEX index in Enum.GetValues(typeof(INDEX))) {
                if (Instance.layer2Contexts == null || Instance.layer2Contexts.Count < (int) index) {
                    continue;
                }

                if (Instance.layer2Contexts[(int) index] == null) {
                    continue;
                }

                data += index + ": " + Instance.layer2Contexts[(int) index].GetHashCode() + " " +
                        Instance.layer2Contexts[(int) index] + "\n";
            }

            data += "\n";

            data += "Layer 3: " + "\n";

            foreach (INDEX index in Enum.GetValues(typeof(INDEX))) {
                if (Instance.layer3Contexts == null || Instance.layer3Contexts.Count < (int) index) {
                    continue;
                }

                if (Instance.layer3Contexts[(int) index] == null) {
                    continue;
                }

                data += index + ": " + Instance.layer3Contexts[(int) index].GetHashCode() + " " +
                        Instance.layer3Contexts[(int) index] + "\n";
            }

            data += "\n";

            data += "Layer 4: " + "\n";

            foreach (INDEX index in Enum.GetValues(typeof(INDEX))) {
                if (Instance.layer4Contexts == null || Instance.layer4Contexts.Count < (int) index) {
                    continue;
                }

                if (Instance.layer4Contexts[(int) index] == null) {
                    continue;
                }

                data += index + ": " + Instance.layer4Contexts[(int) index].GetHashCode() + " " +
                        Instance.layer4Contexts[(int) index] + "\n";
            }

            data += "\n";

            data += "Layer 5: " + "\n";

            foreach (INDEX index in Enum.GetValues(typeof(INDEX))) {
                if (Instance.layer5Contexts == null || Instance.layer5Contexts.Count < (int) index) {
                    continue;
                }

                if (Instance.layer5Contexts[(int) index] == null) {
                    continue;
                }

                data += index + ": " + Instance.layer5Contexts[(int) index].GetHashCode() + " " +
                        Instance.layer5Contexts[(int) index] + "\n";
            }

            data += "\n";

            data += "Layer 6: " + "\n";

            foreach (INDEX index in Enum.GetValues(typeof(INDEX))) {
                if (Instance.layer6Contexts == null || Instance.layer6Contexts.Count < (int) index) {
                    continue;
                }

                if (Instance.layer6Contexts[(int) index] == null) {
                    continue;
                }

                data += index + ": " + Instance.layer6Contexts[(int) index].GetHashCode() + " " +
                        Instance.layer6Contexts[(int) index] + "\n";
            }

            data += "\n";

            data += "Layer 7: " + "\n";

            foreach (INDEX index in Enum.GetValues(typeof(INDEX))) {
                if (Instance.layer7Contexts == null || Instance.layer7Contexts.Count < (int) index) {
                    continue;
                }

                if (Instance.layer7Contexts[(int) index] == null) {
                    continue;
                }

                data += index + ": " + Instance.layer7Contexts[(int) index].GetHashCode() + " " +
                        Instance.layer7Contexts[(int) index] + "\n";
            }

            data += "\n";

            data += "Layer 8: " + "\n";

            foreach (INDEX index in Enum.GetValues(typeof(INDEX))) {
                if (Instance.layer8Contexts == null || Instance.layer8Contexts.Count < (int) index) {
                    continue;
                }

                if (Instance.layer8Contexts[(int) index] == null) {
                    continue;
                }

                data += index + ": " + Instance.layer8Contexts[(int) index].GetHashCode() + " " +
                        Instance.layer8Contexts[(int) index] + "\n";
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