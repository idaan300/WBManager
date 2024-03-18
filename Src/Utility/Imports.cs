using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;
using static RobotManager.Utility.Debug.ConsoleEx;
using static RobotManager.Utility.FormEx;


namespace RobotManager.Utility; 

/// <summary>
/// Windows API Imports
/// </summary>
public static class Imports {

    //-------------------------
    //      Enums
    //-------------------------

    // An enumerated type for the control messages
    // sent to the handler routine.
    public enum CtrlTypes {
	    CTRL_C_EVENT = 0,
	    CTRL_BREAK_EVENT,
	    CTRL_CLOSE_EVENT,
	    CTRL_LOGOFF_EVENT = 5,
	    CTRL_SHUTDOWN_EVENT
    }


    [Flags]
    public enum ConsoleModes : uint {
	    ENABLE_PROCESSED_INPUT = 0x0001,
	    ENABLE_LINE_INPUT = 0x0002,
	    ENABLE_ECHO_INPUT = 0x0004,
	    ENABLE_WINDOW_INPUT = 0x0008,
	    ENABLE_MOUSE_INPUT = 0x0010,
	    ENABLE_INSERT_MODE = 0x0020,
	    ENABLE_QUICK_EDIT_MODE = 0x0040,
	    ENABLE_EXTENDED_FLAGS = 0x0080,
	    ENABLE_AUTO_POSITION = 0x0100,

	    ENABLE_PROCESSED_OUTPUT = 0x0001,
	    ENABLE_WRAP_AT_EOL_OUTPUT = 0x0002,
	    ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004,
	    DISABLE_NEWLINE_AUTO_RETURN = 0x0008,
	    ENABLE_LVB_GRID_WORLDWIDE = 0x0010
    }

    //-------------------------
    //      Delegates
    //-------------------------

    public delegate bool ConsoleEventDelegate(CtrlTypes sig);
    public delegate IntPtr KeyboardEventDelegate(int nCode, IntPtr wParam, IntPtr lParam);

    //-------------------------
    //       Consts
    //-------------------------

    public const uint SW_RESTORE = 0x09;
    public const int WM_SETREDRAW = 0x0B;
    public const int WM_PAINT = 0x0F;
    public const int MF_BYCOMMAND = 0x00000000;
    public const int SC_CLOSE = 0xF060;

    //----------------------
    //     P/Invokes
    //---------------------

    //~~~~~~~~~ Kernel32

    /// <summary>
    /// Allocate a ConHost Console
    /// </summary>
    /// <returns>Operation Sucess</returns>
    [DllImport("kernel32.dll")]
    public static extern bool AllocConsole();

    /// <summary>
    /// Allocate a ConHost Console
    /// </summary>
    /// <returns>Operation Sucess</returns>
    [DllImport("kernel32.dll")]
    public static extern bool AttachConsole(int pid);

    /// <summary>
    /// Allocate a ConHost Console
    /// </summary>
    /// <returns>Operation Sucess</returns>
    [DllImport("kernel32.dll")]
    public static extern bool FreeConsole();

    /// <summary>
    /// Get The standard output handle
    /// </summary>
    /// <param name="nStdHandle"></param>
    /// <returns>IntPtr To StdHandle</returns>
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr GetStdHandle(int nStdHandle);

    /// <summary>
    /// Set The standard output handle
    /// </summary>
    /// <param name="nStdHandle"></param>
    /// <param name="hHandle"></param>
    /// <returns>Success state</returns>
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool SetStdHandle(int nStdHandle, IntPtr hHandle);

    /// <summary>
    /// Kernel32 Create File.
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="access"></param>
    /// <param name="share"></param>
    /// <param name="securityAttributes"></param>
    /// <param name="creationDisposition"></param>
    /// <param name="flagsAndAttributes"></param>
    /// <param name="templateFile"></param>
    /// <returns>IntPtr to created file handle</returns>
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr CreateFile(
        [MarshalAs(UnmanagedType.LPTStr)] string filename,
        [MarshalAs(UnmanagedType.U4)] uint access,
        [MarshalAs(UnmanagedType.U4)] FileShare share,
        IntPtr securityAttributes,
        [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
        [MarshalAs(UnmanagedType.U4)] FileAttributes flagsAndAttributes,
        IntPtr templateFile
    );

    /// <summary>
    /// Get Attatched Console Window
    /// </summary>
    /// <returns>IntPtr To Console Reference</returns>
    [DllImport("kernel32.dll")]
    public static extern IntPtr GetConsoleWindow();

    /// <summary>
    /// Show / Hide Window
    /// </summary>
    /// <param name="hWnd"></param>
    /// <param name="nCmdShow"></param>
    /// <returns>Operation Succeded</returns>
    [DllImport("user32.dll")]
    public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    /// <summary>
    /// Is Window Visible
    /// </summary>
    /// <param name="hWnd"></param>
    /// <returns></returns>
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool IsWindowVisible(IntPtr hWnd);

    /// <summary>
    /// Moves The Seleted Window to the foreground
    /// </summary>
    /// <param name="hWnd"></param>
    /// <returns></returns>
    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool SetForegroundWindow(IntPtr hWnd);

    /// <summary>
    /// Closes an open kernel object handle
    /// </summary>
    /// <param name="handle"></param>
    /// <returns></returns>
    [DllImport("Kernel32.dll", SetLastError = true)]
    public static extern bool CloseHandle(IntPtr handle);

    /// <summary>
    /// Kernel Create Event
    /// </summary>
    /// <param name="lpEventAttributes"></param>
    /// <param name="bManualReset"></param>
    /// <param name="bInitialState"></param>
    /// <param name="lpName"></param>
    /// <returns></returns>
    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern IntPtr CreateEvent(IntPtr lpEventAttributes, bool bManualReset, bool bInitialState, string lpName);

    /// <summary>
    /// Maps The Specified DLL File Into The Address Space Of The Calling Process
    /// </summary>
    /// <param name="lpFileName"></param>
    /// <returns></returns>
    [DllImport("kernel32.dll")]
    public static extern IntPtr LoadLibrary(string lpFileName);

    /// <summary>
    /// Frees The Specified DLL File From The Calling Process
    /// </summary>
    /// <param name="hModule"></param>
    /// <returns></returns>
    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    public static extern bool FreeLibrary(IntPtr hModule);

    /// <summary>
    /// Sets The Console Mode
    /// </summary>
    /// <param name="hConsoleHandle"></param>
    /// <param name="dwMode"></param>
    /// <returns></returns>
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint Mode);


    /// <summary>
    /// Gets The Console Mode
    /// </summary>
    /// <param name="hConsoleHandle"></param>
    /// <param name="dwMode"></param>
    /// <returns></returns>
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);


    /// <summary>
    /// Retrieves the calling thread's last-error code value
    /// </summary>
    /// <returns></returns>
    [DllImport("kernel32.dll")]
    public static extern uint GetLastError();

    [DllImport("kernel32.dll")]
    public static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate Delegate, bool Add);

    [DllImport("user32.dll")]
    public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

    [DllImport("user32.dll")]
    public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

    /// <summary>
    /// Win32 GetKeyState
    /// </summary>
    /// <param name="nCode">Virtual KeyCode</param>
    /// <returns></returns>
    [DllImport("user32.dll", SetLastError = true)]
    public static extern short GetKeyState(int nCode);

    /// <summary>
    /// The SetWindowsHookEx function installs an application-defined hook procedure into a hook chain.
    /// You would install a hook procedure to monitor the system for certain types of events. These events are
    /// associated either with a specific thread or with all threads in the same desktop as the calling thread.
    /// </summary>
    /// <param name="idHook">hook type</param>
    /// <param name="lpfn">hook procedure</param>
    /// <param name="hMod">handle to application instance</param>
    /// <param name="dwThreadId">thread identifier</param>
    /// <returns>If the function succeeds, the return value is the handle to the hook procedure.</returns>
    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr SetWindowsHookEx(int idHook, KeyboardEventDelegate lpfn, IntPtr hMod, int dwThreadId);

    /// <summary>
    /// The UnhookWindowsHookEx function removes a hook procedure installed in a hook chain by the SetWindowsHookEx function.
    /// </summary>
    /// <param name="hHook">handle to hook procedure</param>
    /// <returns>If the function succeeds, the return value is true.</returns>
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool UnhookWindowsHookEx(IntPtr hhk);

    /// <summary>
    /// The CallNextHookEx function passes the hook information to the next hook procedure in the current hook chain.
    /// A hook procedure can call this function either before or after processing the hook information.
    /// </summary>
    /// <param name="hHook">handle to current hook</param>
    /// <param name="code">hook code passed to hook procedure</param>
    /// <param name="wParam">value passed to hook procedure</param>
    /// <param name="lParam">value passed to hook procedure</param>
    /// <returns>If the function succeeds, the return value is true.</returns>
    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr CallNextHookEx(IntPtr hHook, int code, IntPtr wParam, IntPtr lParam);

    /// <summary>
    /// Unhide And Show A Given Window
    /// </summary>
    /// <param name="HWnd"></param>
    /// <param name="Msg"></param>
    /// <returns></returns>
    [DllImport("user32.dll")]
    public static extern int ShowWindow(IntPtr HWnd, uint Msg);

    /// <summary>
    /// Is Given Window Minimized
    /// </summary>
    /// <param name="HWnd"></param>
    /// <returns></returns>
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool IsIconic(IntPtr HWnd);

    /// <summary>
    /// Is Given Window Maximized
    /// </summary>
    /// <param name="HWnd"></param>
    /// <returns></returns>
    [DllImport("user32.dll")]
    public static extern bool IsZoomed(IntPtr HWnd);

    /// <summary>
    /// Send DWM Message
    /// </summary>
    /// <param name="hWnd"></param>
    /// <param name="wMsg"></param>
    /// <param name="wParam"></param>
    /// <param name="lParam"></param>
    /// <returns></returns>
    [DllImport("user32.dll")]
    public static extern int SendMessage(IntPtr hWnd, int wMsg, bool wParam, int lParam);

    /// <summary>
    /// Change DWM Window Attributes
    /// </summary>
    /// <param name="Hwnd"></param>
    /// <param name="Attr"></param>
    /// <param name="AttrValue"></param>
    /// <param name="AttrSize"></param>
    /// <returns></returns>
    [DllImport("dwmapi.dll")]
    public static extern int DwmSetWindowAttribute(IntPtr Hwnd, DWMWindowAttribute Attr, int[] AttrValue, int AttrSize);

    /* This can be changed to kernel32.dll / K32GetMappedFileNameW if compatibility with Windows Server 2008 and
     * earlier is not needed, but it is not clear what the gain of doing so is, see the remarks about
     * PSAPI_VERSION at https://msdn.microsoft.com/en-us/library/windows/desktop/ms683195(v=vs.85).aspx */
    [DllImport("Psapi.dll", EntryPoint = "GetMappedFileNameW", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
    public static extern int GetMappedFileName(
	    SafeProcessHandle hProcess,
	    SafeMemoryMappedViewHandle lpv,
	    [Out] StringBuilder lpFilename,
	    int nSize
    );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="lpKeyState"></param>
    /// <returns></returns>
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetKeyboardState(byte[] lpKeyState);

    [DllImport("user32.dll")]
    public static extern short GetAsyncKeyState(int vKey);

    /// <summary>
    /// Locks the set foreground window.
    /// </summary>
    /// <param name="uLockCode">The u lock code.</param>
    /// <returns></returns>
    [DllImport("user32.dll")]
    public static extern bool LockSetForegroundWindow(uint uLockCode);

    /// <summary>
    /// Gets the foreground window.
    /// </summary>
    /// <returns></returns>
    [DllImport("user32.dll")]
    public static extern IntPtr GetForegroundWindow();

}