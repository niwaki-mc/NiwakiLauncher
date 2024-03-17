using System;
using System.ComponentModel;
using CmlLib.Core;
using CmlLib.Core.Downloader;

namespace NiwakiLauncher.Utils;

/// <summary>
/// Utility class for initializing a CMLauncher instance.
/// </summary>
public static class LauncherHelper
{
    /// <summary>
    /// Initializes a CMLauncher instance with the given parameters.
    /// </summary>
    /// <param name="basePath">The base path for the launcher.</param>
    /// <param name="fileChanged">The event handler for file changed events.</param>
    /// <param name="progressChanged">The event handler for progress changed events.</param>
    /// <returns>A CMLauncher instance.</returns>
    public static CMLauncher InitLauncher(string basePath, DownloadFileChangedHandler fileChanged,
        ProgressChangedEventHandler progressChanged)
    {
        CMLauncher launcher = new CMLauncher(basePath);

         launcher.FileChanged += fileChanged;

         launcher.ProgressChanged += progressChanged;

         return launcher;
    }
}