using System;
using System.Collections.Generic;
using BulwarkStudios.GameSystems.Contexts;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BulwarkStudios.GameSystems.Events {

    /// <summary>
    /// Event data
    /// </summary>
    public abstract class GameEventData {

        /// <summary>
        /// Obj listener
        /// </summary>
        public object listener;

        /// <summary>
        /// Game context contrains
        /// </summary>
        [ShowInInspector]
        private List<ScriptableObject> gameContextConstraints;

        /// <summary>
        /// Functions contrains
        /// </summary>
        [ShowInInspector]
        private List<Func<bool>> functionConstraints;

        protected GameEventData(object listener) {
            this.listener = listener;
        }

        /// <summary>
        /// Add a game context constraint
        /// </summary>
        /// <param name="gameContext"></param>
        /// <returns></returns>
        public GameEventData AddGameContextConstraint(IGameContext gameContext) {

            if (gameContext == null) {
                return this;
            }

            if (gameContextConstraints == null) {
                gameContextConstraints = new List<ScriptableObject>();
            }

            gameContextConstraints.Add(gameContext.GetScriptableObject());
            return this;

        }

        /// <summary>
        /// Add a function constraint
        /// </summary>
        /// <param name="function"></param>
        /// <returns></returns>
        public GameEventData AddFunctionConstraint(Func<bool> function) {

            if (function == null) {
                return this;
            }

            if (functionConstraints == null) {
                functionConstraints = new List<Func<bool>>();
            }

            functionConstraints.Add(function);
            return this;

        }

        /// <summary>
        /// Can we trigger the event?
        /// </summary>
        /// <returns></returns>
        public bool CanTriggerEvent() {

            if (!IsGameContextsValid()) {
                return false;
            }

            if (!IsFunctionsValid()) {
                return false;
            }

            return true;

        }

        /// <summary>
        /// Game contexts valid?
        /// </summary>
        /// <returns></returns>
        private bool IsGameContextsValid() {

            if (gameContextConstraints == null) {
                return true;
            }

            if (GameContextSystem.HasContexts(gameContextConstraints)) {
                return true;
            }

            return false;

        }

        /// <summary>
        /// Functions contrains valid?
        /// </summary>
        /// <returns></returns>
        private bool IsFunctionsValid() {

            if (functionConstraints == null) {
                return true;
            }

            foreach (Func<bool> function in functionConstraints) {
                if (!function()) {
                    return false;
                }
            }

            return true;

        }

    }

}