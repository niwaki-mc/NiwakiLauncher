using System;
using System.Threading.Tasks;
using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft;

namespace NiwakiLauncher.Utils;

/// <summary>
/// Helper class for authentication operations.
/// </summary>
public static class AuthentificationHelper
{
    /// <summary>
    /// Authenticates the user based on the specified authentication type.
    /// </summary>
    /// <param name="type">The type of authentication to perform.</param>
    /// <param name="successCallback">Optional. A callback to be executed when authentication is successful. Receives the authenticated session as a parameter.</param>
    /// <param name="errorCallback">Optional. A callback to be executed when an error occurs during authentication. Receives the exception that occurred as a parameter.</param>
    /// <param name="callback">Optional. A callback to be executed after authentication is completed, regardless of success or failure.</param>
    /// <returns>The authenticated session if authentication is successful, or null otherwise.</returns>
    public static async Task<MSession?> Authenticate(AuthentificationType type, Action<MSession>? successCallback,
        Action<Exception>? errorCallback,
        Action? callback)
    {
        switch (type)
        {
            case AuthentificationType.Silent:
                try
                {
                    JELoginHandler loginHandler = JELoginHandlerBuilder.BuildDefault();
                    MSession session = await loginHandler.AuthenticateSilently();
                    ExecuteActionIfNotNull(successCallback, session);
                    return session;
                }
                catch (Exception e)
                {
                    ExecuteActionIfNotNull(errorCallback, e);
                }
                finally
                {
                    ExecuteActionIfNotNull(callback);
                }
                return null;
            case AuthentificationType.Interactive:
                try
                {
                    JELoginHandler loginHandler = JELoginHandlerBuilder.BuildDefault();
                    MSession session = await loginHandler.AuthenticateInteractively();
                    ExecuteActionIfNotNull(successCallback, session);
                    return session;
                }
                catch (Exception e)
                {
                    ExecuteActionIfNotNull(errorCallback, e);
                }
                finally
                {
                    ExecuteActionIfNotNull(callback);
                }
                return null;
            default:
                ExecuteActionIfNotNull(errorCallback, new ArgumentException("Unknown type", nameof(type)));
                ExecuteActionIfNotNull(callback);
                return null;
        }
    }


    /// <summary>
    /// Executes a given action if it is not null.
    /// </summary>
    /// <typeparam name="T">The type of the object passed to the action.</typeparam>
    /// <param name="action">The action to execute.</param>
    /// <param name="object">The value to pass to the action</param>
    private static void ExecuteActionIfNotNull<T>(Action<T>? action, T @object)
    {
        if (action is not null)
        {
            action(@object);
        }
    }

    /// <summary>
    /// Executes a given action if it is not null.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    private static void ExecuteActionIfNotNull(Action? action)
    {
        if (action is not null)
        {
            action();
        }
    }
} 