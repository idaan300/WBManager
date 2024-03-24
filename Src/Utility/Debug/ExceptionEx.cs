using System;
using System.Diagnostics;
using System.Security;
using System.Threading;
using System.Windows.Forms;

namespace RobotManager.Utility.Debug;
public static class ExceptionEx {

    public static void Initialize() {

        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        Application.ThreadException += Application_ThreadException;
        Logger.Info("&6Registered Exception Handler");
    }

    private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e) {


        ConsoleEx.Show();
        Logger.Trace($"Unhandled Exception: &3{e.Exception.Message}");

        Logger.PrintRaw($"&3{e.Exception}");

        //Skip if debugging
        if (Debugger.IsAttached)
            return;

        Logger.PrintRaw("&6Press any key to attempt to continue...");
        Console.ReadKey();
        Logger.PrintRaw("&1OK");

    }

    private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
        ConsoleEx.Show();
        Logger.Trace($"Unhandled Exception: &3{e.ExceptionObject}");

        //Skip if debugging
        if (Debugger.IsAttached)
            return;

        Logger.PrintRaw("&6Press any key to attempt to continue...");
        Console.ReadKey();
        Logger.PrintRaw("&1OK");
    }

    public static bool IsCritical(Exception ex) {
        return ex switch {
            StackOverflowException => true,
            OutOfMemoryException => true,
            AccessViolationException => true,
            TypeInitializationException => true,
            ThreadAbortException => true,
            BadImageFormatException => true,
            InvalidProgramException => true,
            SecurityException => true,
            DllNotFoundException => true,
            ThreadStateException => true,
            AppDomainUnloadedException => true,
            var _ => false
        };
    }

}