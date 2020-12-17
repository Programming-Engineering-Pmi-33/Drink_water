using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Text;

namespace DrinkWater
{
    /// <summary>
    /// Class for logging an exception.
    /// </summary>
    public class ExceptionHandler
    {
        /// <summary>
        /// This function initialize event handler for unhandled exceptions.
        /// </summary>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
        public static void UnhadledExceptionHandler()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(Handler);
        }

        /// <summary>
        /// Handler.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="args">Arguments.</param>
        public static void Handler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            Logger.Log.Error("Handler caught : " + e.Message);
            Logger.Log.Error($"Runtime terminating: {args.IsTerminating}");
            Logger.Log.Error($"Found in {e.StackTrace}");
        }
    }
}
