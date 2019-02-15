using UnityEngine;
using UnityEditor;

namespace BulwarkStudios.Utils {

    public class ProjectFolderShortcutWindow : EditorWindow {
        
        /// <summary>
        /// Shortcut data
        /// </summary>
        private static ProjectFolderShortcutData shortcutsData;

        [MenuItem("Bulwark Studios/Project Folder Shortcut")]
        public static void Open() {
            GetWindow<ProjectFolderShortcutWindow>("Project Shortcut");
        }

        /// <summary>
        /// Display
        /// </summary>
        private void OnGUI() {

            // Get the shortcuts
            GetShortcuts();

            // Show all shortcut
            for(int i = 0; i < shortcutsData.shortcuts.Count; i++) {

                if(i % 3 == 0) {
                    if (i != 0) {
                        GUILayout.EndHorizontal();
                    }
                    GUILayout.BeginHorizontal();
                }

                // Ping
                if(GUILayout.Button(shortcutsData.shortcuts[i].shortcutName)) {
                    EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath(shortcutsData.shortcuts[i].relativePath, typeof(Object)));
                }

            }

            // Add new
            if(GUILayout.Button("Add new shortcut")) {
                Selection.SetActiveObjectWithContext(shortcutsData, shortcutsData);
            }

            // End
            GUILayout.EndHorizontal();

        }

        /// <summary>
        /// Get the asset
        /// </summary>
        private static void GetShortcuts() {

            // Load asset
            shortcutsData = AssetDatabase.LoadAssetAtPath<ProjectFolderShortcutData>(@"Assets/ProjectFolderShortcut.asset");

            // Create the asset if not exist
            if(shortcutsData == null) {

                // Create
                shortcutsData = ScriptableObject.CreateInstance<ProjectFolderShortcutData>();
                AssetDatabase.CreateAsset(shortcutsData, @"Assets/ProjectFolderShortcut.asset");

                // Reset
                shortcutsData.Reset();

                // Save
                EditorUtility.SetDirty(shortcutsData);
                AssetDatabase.SaveAssets();
            }

        }

    }
}