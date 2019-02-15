using System.Diagnostics;
using BulwarkStudios.GameSystems.Configs;
using BulwarkStudios.GameSystems.Logs;
using BulwarkStudios.Tests;

/// <summary>
/// Log wrapper class to not use using
/// </summary>
public static class Log {

    /// <summary>
    /// Log
    /// </summary>
    [Conditional(BuildConstants.DEBUG)]
    public static void Info(object message, LogConfigTest.TAG tag = LogConfigTest.TAG.DEFAULT) {
        GameLogSystem.Info(message, tag.ToString());
    }

    /// <summary>
    /// Warning
    /// </summary>
    [Conditional(BuildConstants.DEBUG)]
    public static void Warning(object message, LogConfigTest.TAG tag = LogConfigTest.TAG.DEFAULT) {
        GameLogSystem.Warning(message, tag.ToString());
    }

    /// <summary>
    /// Error
    /// </summary>
    [Conditional(BuildConstants.DEBUG)]
    public static void Error(object message, LogConfigTest.TAG tag = LogConfigTest.TAG.DEFAULT) {
        GameLogSystem.Error(message, tag.ToString());
    }

    /// <summary>
    /// Exception
    /// </summary>
    [Conditional(BuildConstants.DEBUG)]
    public static void Exception(object message, LogConfigTest.TAG tag = LogConfigTest.TAG.DEFAULT) {
        GameLogSystem.Exception(message, tag.ToString());
    }

}