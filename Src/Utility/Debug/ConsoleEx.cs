using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using RobotManager.Forms;
using static RobotManager.Utility.Imports;
using static RobotManager.Utility.FormEx;

namespace RobotManager.Utility.Debug;

public static class ConsoleEx {

    //-------------------------
    //     Debugging Console
    //-------------------------

    private static IntPtr ConHandle;
    private static IntPtr STDHandle;
    private const int STD_OUTPUT_HANDLE = -11;              //Windows stdout Handle ID
    private const int STD_ERROR_HANDLE = -12;               //Windows stderr Handle ID
    private const int SW_HIDE = 0;                          //DWM Hide Window CMD
    private const int SW_SHOW = 5;                          //DWM Show Window CMD
    private const uint GENERIC_READ = 0x80000000;           //File Read Permission Attribute
    private const uint GENERIC_WRITE = 0x40000000;          //File Write Permission Attribute
    private static ConsoleType Typ = ConsoleType.None;

    private enum ConsoleType {
	    None,
	    Allocated,
	    Internal
    };

    // A delegate type to be used as the handler routine
    // for OnCtrlCheck.

    /// <summary>
    /// Override Any Stdout redirections
    /// (Intended to bypass the override done by visual studio)
    /// </summary>
    private static void OverrideRedirection() {
	    IntPtr OutHandleStd = GetStdHandle(STD_OUTPUT_HANDLE);
	    IntPtr RealOutHandleStd = CreateFile(
		    @"CONOUT$",
		    GENERIC_READ | GENERIC_WRITE,
		    FileShare.Write,
		    IntPtr.Zero,
		    FileMode.OpenOrCreate,
		    0,
		    IntPtr.Zero
	    );

	    //If the Current Handle is not the default one Override it
	    if (RealOutHandleStd == OutHandleStd) return;

	    SetStdHandle(STD_OUTPUT_HANDLE, RealOutHandleStd);
	    Console.SetOut(new StreamWriter(Console.OpenStandardOutput(), Console.OutputEncoding) { AutoFlush = true });
    }

    public static void Hide() => ShowWindow(ConHandle, SW_HIDE);
    public static void Show() => ShowWindow(ConHandle, SW_SHOW);
    public static void Show(bool state) {
	    ShowWindow(ConHandle, state ? SW_SHOW : SW_HIDE);
        if(state) SetForegroundWindow(ConHandle);
        //Set MainForm As Foreground Focus Window
        FormMain.Instance?.InvokeC( () => {
	         SetForegroundWindow(FormMain.Instance.Handle);
	         FormMain.Instance?.Focus();
        });
    }

    public static void Toggle() => Show(!IsWindowVisible(ConHandle));

    private static ConsoleEventDelegate Handler;

    /// <summary>
    /// Initialize A Console
    /// </summary>
    /// <param name="Title">Caption Title Of Window</param>
    public static void Init(string Title) {

	    if (AttachConsole(-1)) {
		    Typ = ConsoleType.Internal;
	    }

	    if (AllocConsole()) {
			#if !DEBUG
				ConHandle = GetConsoleWindow();
	            Hide();
			#endif
            Typ = ConsoleType.Allocated;
	    }

        if (Typ != ConsoleType.None) {

	        STDHandle = GetStdHandle(-10);
	        ConHandle = GetConsoleWindow();

            Console.Title = Title;
			Handler += OnCtrlCheck;
			SetConsoleCtrlHandler(Handler, true);
			//If Compiled In Release Hide On Startup
			DeleteMenu(GetSystemMenu(ConHandle, false), SC_CLOSE, MF_BYCOMMAND);
			OverrideRedirection();
		}
    }

    private static bool OnCtrlCheck(CtrlTypes ctrlType) {
        Hide();
        return true;
	}
}
