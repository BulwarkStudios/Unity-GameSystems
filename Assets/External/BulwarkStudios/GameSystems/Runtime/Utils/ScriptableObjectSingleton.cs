using System.IO;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace BulwarkStudios.GameSystems.Utils {

    public abstract class ScriptableObjectSingleton<T> : ScriptableObject, IScriptableObjectSingleton
        where T : ScriptableObject, new() {

        protected static T instance;

        public static T Instance {
            get { return instance; }
        }

        /// <summary>
        /// Debug
        /// </summary>
        [ShowInInspector, ReadOnly]
        private int DebugHashCode {
            get { return GetHashCode(); }
        }

#if UNITY_EDITOR

        /// <summary>
        /// Create the asset
        /// </summary>
        protected static T CreateSingleton(string path) {

            GlobalConfigAttribute configAttribute = new GlobalConfigAttribute(path);

            T inst = CreateInstance<T>();

            if (!Directory.Exists(configAttribute.FullPath)) {
                Directory.CreateDirectory(new DirectoryInfo(configAttribute.FullPath).FullName);
                UnityEditor.AssetDatabase.Refresh();
            }

            string niceName = typeof(T).GetNiceName();
            string assetPath = "Assets/" + configAttribute.AssetPath + niceName + ".asset";

            UnityEditor.AssetDatabase.CreateAsset(inst, assetPath);
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();

            return inst;

        }

#endif

        #region Implementation of IScriptableObjectSingleton

        /// <summary>
        /// Set the instance
        /// </summary>
        /// <param name="so"></param>
        void IScriptableObjectSingleton.SetInstance(ScriptableObject so) {
            instance = so as T;
        }

        #endregion

    }

}