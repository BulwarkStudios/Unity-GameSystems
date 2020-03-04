using BulwarkStudios.GameSystems.Contexts;
using BulwarkStudios.GameSystems.Events;
using BulwarkStudios.GameSystems.Libraries;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace BulwarkStudios.GameSystems.Editor {

    public class GameSystemsWindow : EditorWindow {

        private Button btnContextScan;
        private Button btnContextExplore;

        private Button btnEventScan;
        private Button btnEventExplore;

        private Button btnLibraryScan;
        private Button btnLibraryExplore;

        private Button btnAllScan;

        [MenuItem("Bulwark Studios/GameSystems/Manage")]
        public static void ShowWindow() {

            // Opens the window, otherwise focuses it if it’s already open.
            GameSystemsWindow window = GetWindow<GameSystemsWindow>();

            // Adds a title to the window.
            window.titleContent = new GUIContent("Game Systems");

            // Sets a minimum size to the window.
            window.minSize = new Vector2(250, 50);

        }

        private void OnEnable() {

            // Reference to the root of the window.
            VisualElement root = rootVisualElement;

            // Associates a stylesheet to our root. Thanks to inheritance, all root’s
            // children will have access to it.
            root.styleSheets.Add(Resources.Load<StyleSheet>("GameSystemsWindow_Styles"));

            // Loads and clones our VisualTree (eg. our UXML structure) inside the root.
            VisualTreeAsset quickToolVisualTree = Resources.Load<VisualTreeAsset>("GameSystemsWindow_Main");
            quickToolVisualTree.CloneTree(root);

            // Get buttons
            btnContextScan = root.Query<Button>("button-context-scan").First();
            btnContextScan.clicked -= BtnContextScanOnClicked; 
            btnContextScan.clicked += BtnContextScanOnClicked; 

            btnContextExplore = root.Query<Button>("button-context-explore").First();
            btnContextExplore.clicked -= BtnContextExploreOnClicked; 
            btnContextExplore.clicked += BtnContextExploreOnClicked; 

            btnEventScan = root.Query<Button>("button-event-scan").First();
            btnEventScan.clicked -= BtnEventScanOnClicked; 
            btnEventScan.clicked += BtnEventScanOnClicked; 

            btnEventExplore = root.Query<Button>("button-event-explore").First();
            btnEventExplore.clicked -= BtnEventExploreOnClicked; 
            btnEventExplore.clicked += BtnEventExploreOnClicked; 

            btnLibraryScan = root.Query<Button>("button-library-scan").First();
            btnLibraryScan.clicked -= BtnLibraryScanOnClicked; 
            btnLibraryScan.clicked += BtnLibraryScanOnClicked; 

            btnLibraryExplore = root.Query<Button>("button-library-explore").First();
            btnLibraryExplore.clicked -= BtnLibraryExploreOnClicked; 
            btnLibraryExplore.clicked += BtnLibraryExploreOnClicked; 

            btnAllScan = root.Query<Button>("button-all-scan").First();
            btnAllScan.clicked -= BtnAllScanOnClicked; 
            btnAllScan.clicked += BtnAllScanOnClicked; 

        }

        private void OnDisable() {
            if (btnContextScan != null) btnContextScan.clicked -= BtnContextScanOnClicked;
            if (btnContextExplore != null) btnContextExplore.clicked += BtnContextExploreOnClicked;
            if (btnEventScan != null) btnEventScan.clicked += BtnEventScanOnClicked;
            if (btnEventExplore != null) btnEventExplore.clicked += BtnEventExploreOnClicked;
            if (btnLibraryScan != null) btnLibraryScan.clicked += BtnLibraryScanOnClicked;
            if (btnLibraryExplore != null) btnLibraryExplore.clicked += BtnLibraryExploreOnClicked;
            if (btnAllScan != null) btnAllScan.clicked += BtnAllScanOnClicked;
        }

        private void BtnContextScanOnClicked() {
            GameContextSystem.Scan();
        }

        private void BtnContextExploreOnClicked() {
            
        }

        private void BtnEventScanOnClicked() {
            GameEventSystem.Scan();
        }

        private void BtnEventExploreOnClicked() {
            
        }

        private void BtnLibraryScanOnClicked() {
            GameLibrarySystem.Scan();
        }

        private void BtnLibraryExploreOnClicked() {
            
        }

        private void BtnAllScanOnClicked() {
            GameContextSystem.Scan();
            GameEventSystem.Scan();
            GameLibrarySystem.Scan();
        }

    }

}