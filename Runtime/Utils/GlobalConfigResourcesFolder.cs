//-----------------------------------------------------------------------// <copyright file="GlobalConfig.cs" company="Sirenix IVS"> // Copyright (c) Sirenix IVS. All rights reserved.// </copyright>//-----------------------------------------------------------------------
//-----------------------------------------------------------------------
// <copyright file="GlobalConfig.cs" company="Sirenix IVS">
// Copyright (c) Sirenix IVS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Sirenix.Utilities {

    using System.IO;
    using System.Linq;
    using UnityEngine;

    /// <summary>
    /// <para>
    /// A GlobalConfig singleton, automatically created and saved as a ScriptableObject in the project at the specified path.
    /// This only happens if the UnityEditor is present. If it's not, a non-persistent ScriptableObject is created at run-time.
    /// </para>
    /// <para>
    /// Remember to locate the path within a resources folder if you want the config file to be loaded at runtime without the Unity editor being present.
    /// </para>
    /// <para>
    /// The asset path is specified by defining a <see cref="GlobalConfigAttribute"/>. If no attribute is defined it will be saved in the root assets folder.
    /// </para>
    /// </summary>
    /// <example>
    /// <code>
    /// [GlobalConfig("Resources/MyConfigFiles/")]
    /// public class MyGlobalConfig : GlobalConfig&lt;MyGlobalConfig&gt;
    /// {
    ///     public int MyGlobalVariable;
    /// }
    ///
    /// void SomeMethod()
    /// {
    ///     int value = MyGlobalConfig.Instance.MyGlobalVariable;
    /// }
    /// </code>
    /// </example>
    public abstract class GlobalConfigResourcesFolder<T> : ScriptableObject
        where T : GlobalConfigResourcesFolder<T>, new() {

        private static GlobalConfigAttribute configAttribute;

        // Referenced via reflection by EditorOnlyModeConfig
        private static GlobalConfigAttribute ConfigAttribute {
            get {
                if (configAttribute == null) {
                    configAttribute = typeof(T).GetCustomAttribute<GlobalConfigAttribute>();

                    if (configAttribute == null) {
                        configAttribute = new GlobalConfigAttribute(typeof(T).GetNiceName());
                    }
                }

                return configAttribute;
            }
        }

        protected static T instance;

        /// <summary>
        /// Gets a value indicating whether this instance has instance loaded.
        /// </summary>
        public static bool HasInstanceLoaded {
            get { return instance != null; }
        }

        /// <summary>
        /// Gets the singleton instance.
        /// </summary>
        public static T Instance {
            get {
                if (instance == null) {

                    LoadInstanceIfAssetExists();

                    T inst = instance;

#if UNITY_EDITOR
                    string fullPath = Application.dataPath + "/" + ConfigAttribute.AssetPath + typeof(T).GetNiceName() +
                                      ".asset";

                    if (inst == null && UnityEditor.EditorPrefs.HasKey("PREVENT_SIRENIX_FILE_GENERATION")) {
                        Debug.LogWarning(ConfigAttribute.AssetPath + typeof(T).GetNiceName() + ".asset" +
                                         " was prevented from being generated because the PREVENT_SIRENIX_FILE_GENERATION key was defined in Unity's EditorPrefs.");

                        instance = CreateInstance<T>();
                        return instance;
                    }

                    if (inst == null) {
                        if (File.Exists(fullPath) && UnityEditor.EditorSettings.serializationMode ==
                            UnityEditor.SerializationMode.ForceText) {
                            if (Editor.AssetScriptGuidUtility.TryUpdateAssetScriptGuid(fullPath, typeof(T))) {
                                Debug.Log(
                                    "Could not load config asset at first, but successfully detected forced text asset serialization, and corrected the config asset m_Script guid.");

                                LoadInstanceIfAssetExists();
                                inst = instance;
                            }
                            else {
                                Debug.LogWarning(
                                    "Could not load config asset, and failed to auto-correct config asset m_Script guid.");
                            }
                        }
                    }
#endif

                    if (inst == null) {
                        instance = CreateInstance<T>();
                        inst = instance;

#if UNITY_EDITOR

                        if (!Directory.Exists(ConfigAttribute.FullPath)) {
                            Directory.CreateDirectory(new DirectoryInfo(ConfigAttribute.FullPath).FullName);
                            UnityEditor.AssetDatabase.Refresh();
                        }

                        string niceName = typeof(T).GetNiceName();
                        string assetPath = "Assets/" + ConfigAttribute.AssetPath + niceName + ".asset";

                        if (File.Exists(fullPath)) {
                            Debug.LogWarning(
                                "Could not load config asset of type " + niceName + " from project path '" + assetPath +
                                "', " +
                                "but an asset file already exists at the path, so could not create a new asset either. The config " +
                                "asset for '" + niceName +
                                "' has been lost, probably due to an invalid m_Script guid. Set forced " +
                                "text serialization in Edit -> Project Settings -> Editor -> Asset Serialization -> Mode and trigger " +
                                "a script reload to allow Odin to auto-correct this.");
                        }
                        else {
                            UnityEditor.AssetDatabase.CreateAsset(inst, assetPath);
                            UnityEditor.AssetDatabase.SaveAssets();
                            UnityEditor.AssetDatabase.Refresh();
                        }
#endif
                    }

                    instance = inst;
                }

                return instance;
            }
        }

        /// <summary>
        /// Tries to load the singleton instance.
        /// </summary>
        private static void LoadInstanceIfAssetExists() {

//            Debug.Log("ConfigAttribute.AssetPath " + ConfigAttribute.AssetPath);
//            Debug.Log("LoadInstanceIfAssetExists " + ConfigAttribute.IsInResourcesFolder);
//            Debug.Log("niceName " + typeof(T).GetNiceName());
//            Debug.Log("GetResourcesPath(ConfigAttribute.AssetPath) + niceName " + GetResourcesPath(ConfigAttribute.AssetPath) + typeof(T).GetNiceName());

            string niceName = typeof(T).GetNiceName();

            instance = Resources.Load<T>(GetResourcesPath(ConfigAttribute.AssetPath) + niceName);

#if UNITY_EDITOR

            // If it was relocated
            if (instance == null) {
                var relocatedScriptableObject = UnityEditor.AssetDatabase.FindAssets("t:" + typeof(T).Name);

                if (relocatedScriptableObject.Length > 0) {
                    instance =
                        UnityEditor.AssetDatabase.LoadAssetAtPath<T>(
                            UnityEditor.AssetDatabase.GUIDToAssetPath(relocatedScriptableObject[0]));
                }
            }
#endif
        }

        /// <summary>
        /// Gets the resources path. Only relevant if IsInResourcesFolder is true.
        /// </summary>
        private static string GetResourcesPath(string fullPath) {

            string resourcesPath = "";
            Stack<string> folders = new Stack<string>();

            // Find nearest resource folder.
            var currDir = new DirectoryInfo(fullPath);

            while (currDir.Name.Equals("resources", StringComparison.OrdinalIgnoreCase) == false) {
                folders.Push(currDir.Name);
                currDir = currDir.Parent;
            }

            while (folders.Any()) {
                resourcesPath += folders.Pop() + "/";
            }

            return resourcesPath;

        }

        /// <summary>
        /// Opens the config in a editor window. This is currently only used internally by the Sirenix.OdinInspector.Editor assembly.
        /// </summary>
        public void OpenInEditor() {
#if UNITY_EDITOR

            var windowType =
                AssemblyUtilities.GetTypeByCachedFullName("Sirenix.OdinInspector.Editor.SirenixPreferencesWindow");

            if (windowType != null) {
                windowType.GetMethods().Where(x => x.Name == "OpenWindow" && x.GetParameters().Length == 1).First()
                    .Invoke(null, new object[] {this});
            }
            else {
                Debug.LogError(
                    "Failed to open window, could not find Sirenix.OdinInspector.Editor.SirenixPreferencesWindow");
            }
#else
            Debug.Log("Downloading, installing and launching the Unity Editor so we can open this config window in the editor, please stand by until pigs can fly and hell has frozen over...");
#endif
        }

    }

}