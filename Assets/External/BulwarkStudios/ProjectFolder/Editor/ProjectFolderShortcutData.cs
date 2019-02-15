using System.Collections.Generic;
using UnityEngine;

namespace BulwarkStudios.Utils {

    public class ProjectFolderShortcutData : ScriptableObject {
        
        /// <summary>
        /// Shortcuts
        /// </summary>
        public List<ProjectFolderShortcut> shortcuts = new List<ProjectFolderShortcut>();

        /// <summary>
        /// Reset
        /// </summary>
        public void Reset() {
            shortcuts = new List<ProjectFolderShortcut>();
        }

    }

}