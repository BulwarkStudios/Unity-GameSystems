using System.Collections.Generic;
using BulwarkStudios.GameSystems.Logs;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace BulwarkStudios.GameSystems.Events {

    [GlobalConfig(GameEventConstants.RESOURCE_GAMESYSTEM_EVENT_LIST_FOLDER)]
    public abstract class GameEvent<T, P1, P2> : GlobalConfig<T>, IGameEvent where T : GameEvent<T, P1, P2>, new() {

        /// <summary>
        /// Deactive the logs?
        /// </summary>
        [SerializeField] private bool deactiveLogs;

        private bool isLoaded = false;

#if UNITY_EDITOR
        /// <summary>
        /// Editor initialization
        /// </summary>
        public static GameEvent<T, P1, P2> EditorInitialize() {
            if (!Instance.isLoaded) {
                Instance.isLoaded = true;
            }
            return Instance;
        }
#endif

        /// <summary>
        /// List of events
        /// </summary>
        [ShowInInspector, ReadOnly]
        private List<GameEventData2<P1, P2>> events = new List<GameEventData2<P1, P2>>();

        /// <summary>
        /// Listen to the event
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="callback"></param>
        public static GameEventData Listen(object listener, System.Action<P1, P2> callback) {

            GameEventData2<P1, P2> data = new GameEventData2<P1, P2>(listener, callback);
            Instance.events.Add(data);
            return data;

        }

        /// <summary>
        /// Unlisten the event
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="callback"></param>
        public static void Unlisten(object listener, System.Action<P1, P2> callback) {

            for (int i = Instance.events.Count - 1; i >= 0; i--) {

                if (Instance.events[i].listener == listener && Instance.events[i].callback == callback) {
                    Instance.events.RemoveAt(i);
                    break;
                }

            }

        }

        /// <summary>
        /// Trigger the event
        /// </summary>
        [Button]
        public static void Trigger(P1 p1, P2 p2) {

            if (!Instance.deactiveLogs) {
                GameLogSystem.Info("Event triggered: " + Instance + " p1: " + p1 + " p2: " + p2,
                    GameEventConstants.LOG_TAG);
            }

            for (int i = Instance.events.Count - 1; i >= 0; i--) {

                if (i < 0 || i >= Instance.events.Count) {
                    continue;
                }

                if (!Instance.events[i].CanTriggerEvent()) {
                    if (!Instance.deactiveLogs) {
                        GameLogSystem.Info(
                            "Event can't be triggered: " + Instance.events[i].listener + " " +
                            Instance.events[i].callback, GameEventConstants.LOG_TAG);
                    }

                    continue;
                }

                Instance.events[i].callback?.Invoke(p1, p2);
            }

        }

        #region Implementation of IGameEvent

        /// <summary>
        /// Load the context
        /// </summary>
        void IGameEvent.Load() {
            if (!Instance.isLoaded) {
                Instance.isLoaded = true;
            }
        }

        /// <summary>
        /// Add a listener
        /// </summary>
        /// <param name="listener"></param>
        void IGameEvent.Listen(object listener) {
            Listen(listener, null);
        }

        /// <summary>
        /// Remove a listener
        /// </summary>
        void IGameEvent.Unlisten(object listener) {
            Unlisten(listener, null);
        }

        /// <summary>
        /// Trigger
        /// </summary>
        void IGameEvent.Trigger() {
            Trigger(default(P1), default(P2));
        }

        #endregion

    }

}
