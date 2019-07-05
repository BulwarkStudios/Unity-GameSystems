using System;
using System.Collections.Generic;
using UnityEngine;

namespace BulwarkStudios.GameSystems.Platform {

    public abstract class BasePlatform<T> : IPlatform where T : IPlatformConfig {

        /// <summary>
        /// Config
        /// </summary>
        protected T config;

        /// <summary>
        /// Achievement unlock constraint?
        /// </summary>
        protected Func<bool> achievementUnlockConstraint;

        /// <summary>
        /// Initialize
        /// </summary>
        protected abstract void Initialize();

        /// <summary>
        /// Get the user name
        /// </summary>
        /// <returns></returns>
        protected abstract string GetUserName();

        /// <summary>
        /// Update
        /// </summary>
        protected abstract void Update();

        /// <summary>
        /// Destroy
        /// </summary>
        protected abstract void OnDestroy();

        /// <summary>
        /// Get the current language
        /// </summary>
        /// <returns></returns>
        protected abstract SystemLanguage GetLanguage();

        /// <summary>
        /// Has a dlc ?
        /// </summary>
        protected abstract bool HasDlc(PlatformDlc dlc);

        /// <summary>
        /// Reset all achievements
        /// </summary>
        protected abstract void ResetAllAchievements();

        /// <summary>
        /// Reset an achievement
        /// </summary>
        /// <param name="achievement"></param>
        protected abstract void ResetAchievement(PlatformAchievement achievement);

        /// <summary>
        /// Unlock an achievment
        /// </summary>
        /// <param name="achievement"></param>
        protected abstract void UnlockAchievement(PlatformAchievement achievement);

        /// <summary>
        /// Refresh an achievement
        /// </summary>
        /// <param name="achievement"></param>
        protected abstract void RefreshAchievement(PlatformAchievement achievement);

        /// <summary>
        /// Analytics enabled state
        /// </summary>
        /// <param name="value"></param>
        protected abstract void AnalyticsEnabled(bool value);

        /// <summary>
        /// Analytics game start
        /// </summary>
        protected abstract void AnalyticsGameStart();

        /// <summary>
        /// Analytics game over
        /// </summary>
        /// <param name="name"></param>
        protected abstract void AnalyticsGameOver(string name);

        /// <summary>
        /// Analytics level start
        /// </summary>
        /// <param name="name"></param>
        /// <param name="eventData"></param>
        protected abstract void AnalyticsLevelStart(string name, IDictionary<string, object> eventData = null);

        /// <summary>
        /// Analytics level up
        /// </summary>
        /// <param name="name"></param>
        /// <param name="eventData"></param>
        protected abstract void AnalyticsLevelUp(string name, IDictionary<string, object> eventData = null);

        /// <summary>
        /// Analytics level complete
        /// </summary>
        /// <param name="name"></param>
        /// <param name="eventData"></param>
        protected abstract void AnalyticsLevelComplete(string name, IDictionary<string, object> eventData = null);

        /// <summary>
        /// Analytics level fail
        /// </summary>
        /// <param name="name"></param>
        /// <param name="eventData"></param>
        protected abstract void AnalyticsLevelFail(string name, IDictionary<string, object> eventData = null);

        /// <summary>
        /// Analytics cutscene skip
        /// </summary>
        /// <param name="name"></param>
        protected abstract void AnalyticsCutsceneSkip(string name);

        /// <summary>
        /// Analytics tutorial skip
        /// </summary>
        /// <param name="name"></param>
        protected abstract void AnalyticsTutorialSkip(string name);

        #region Implementation of IPlatform

        /// <summary>
        /// Initialize the platform
        /// </summary>
        /// <param name="config"></param>
        void IPlatform.Initialize(IPlatformConfig config) {

            // Save the config
            this.config = (T)config;

            // Initialize
            Initialize();

        }

        /// <summary>
        /// Update
        /// </summary>
        void IPlatform.Update() {
            Update();
        }

        /// <summary>
        /// Destroy
        /// </summary>
        void IPlatform.OnDestroy() {
            OnDestroy();
        }

        /// <summary>
        /// Get the language
        /// </summary>
        /// <returns></returns>
        SystemLanguage IPlatform.GetLanguage() {
            return GetLanguage();
        }

        /// <summary>
        /// Get the user name
        /// </summary>
        /// <returns></returns>
        string IPlatform.GetUserName() {
            return GetUserName();
        }

        /// <summary>
        /// Has a dlc ?
        /// </summary>
        /// <param name="dlc"></param>
        bool IPlatform.HasDlc(PlatformDlc dlc) {
            return HasDlc(dlc);
        }

        /// <summary>
        /// Reset all achievements
        /// </summary>
        void IPlatform.ResetAllAchievements() {
            ResetAllAchievements();
        }

        /// <summary>
        /// Set an achievement constraint
        /// </summary>
        /// <param name="constraint"></param>
        void IPlatform.SetAchievementUnlockConstraint(Func<bool> constraint) {
            achievementUnlockConstraint = constraint;
        }

        /// <summary>
        /// Reset an achievement
        /// </summary>
        /// <param name="achievement"></param>
        void IPlatform.ResetAchievement(PlatformAchievement achievement) {
            ResetAchievement(achievement);
        }

        /// <summary>
        /// Unlock an achievment
        /// </summary>
        /// <param name="achievement"></param>
        void IPlatform.UnlockAchievement(PlatformAchievement achievement) {
            UnlockAchievement(achievement);
        }

        /// <summary>
        /// Refresh an achievement
        /// </summary>
        /// <param name="achievement"></param>
        void IPlatform.RefreshAchievement(PlatformAchievement achievement) {
            RefreshAchievement(achievement);
        }

        /// <summary>
        /// Analytics enabled state
        /// </summary>
        /// <param name="value"></param>
        void IPlatform.AnalyticsEnabled(bool value) {
            AnalyticsEnabled(value);
        }

        /// <summary>
        /// Analytics game start
        /// </summary>
        void IPlatform.AnalyticsGameStart() {
            AnalyticsGameStart();
        }

        /// <summary>
        /// Analytics game over
        /// </summary>
        /// <param name="name"></param>
        void IPlatform.AnalyticsGameOver(string name) {
            AnalyticsGameOver(name);
        }

        /// <summary>
        /// Analytics level start
        /// </summary>
        /// <param name="name"></param>
        /// <param name="eventData"></param>
        void IPlatform.AnalyticsLevelStart(string name, IDictionary<string, object> eventData = null) {
            AnalyticsLevelStart(name, eventData);
        }

        /// <summary>
        /// Analytics level up
        /// </summary>
        /// <param name="name"></param>
        /// <param name="eventData"></param>
        void IPlatform.AnalyticsLevelUp(string name, IDictionary<string, object> eventData = null) {
            AnalyticsLevelUp(name, eventData);
        }

        /// <summary>
        /// Analytics level complete
        /// </summary>
        /// <param name="name"></param>
        /// <param name="eventData"></param>
        void IPlatform.AnalyticsLevelComplete(string name, IDictionary<string, object> eventData = null) {
            AnalyticsLevelComplete(name, eventData);
        }

        /// <summary>
        /// Analytics level fail
        /// </summary>
        /// <param name="name"></param>
        /// <param name="eventData"></param>
        void IPlatform.AnalyticsLevelFail(string name, IDictionary<string, object> eventData = null) {
            AnalyticsLevelFail(name, eventData);
        }

        /// <summary>
        /// Analytics cutscene skip
        /// </summary>
        /// <param name="name"></param>
        void IPlatform.AnalyticsCutsceneSkip(string name) {
            AnalyticsCutsceneSkip(name);
        }

        /// <summary>
        /// Analytics tutorial skip
        /// </summary>
        /// <param name="name"></param>
        void IPlatform.AnalyticsTutorialSkip(string name) {
            AnalyticsTutorialSkip(name);
        }

        #endregion



    }

}