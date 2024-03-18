using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using RobotManager.Utility.Debug;

using static RobotManager.Utility.Imports;
using static RobotManager.Utility.StringEx;

namespace RobotManager.Utility;

public static class FormEx {

	//------------------------
	//        Enums
	//------------------------

    public enum DWMWindowAttribute : uint {
		DWMWA_NCRENDERING_ENABLED,
		DWMWA_NCRENDERING_POLICY,
		DWMWA_TRANSITIONS_FORCEDISABLED,
		DWMWA_ALLOW_NCPAINT,
		DWMWA_CAPTION_BUTTON_BOUNDS,
		DWMWA_NONCLIENT_RTL_LAYOUT,
		DWMWA_FORCE_ICONIC_REPRESENTATION,
		DWMWA_FLIP3D_POLICY,
		DWMWA_EXTENDED_FRAME_BOUNDS,
		DWMWA_HAS_ICONIC_BITMAP,
		DWMWA_DISALLOW_PEEK,
		DWMWA_EXCLUDED_FROM_PEEK,
		DWMWA_CLOAK,
		DWMWA_CLOAKED,
		DWMWA_FREEZE_REPRESENTATION,
		DWMWA_PASSIVE_UPDATE_MODE,
		DWMWA_USE_HOSTBACKDROPBRUSH,
		DWMWA_USE_IMMERSIVE_DARK_MODE = 20,
		DWMWA_WINDOW_CORNER_PREFERENCE = 33,
		DWMWA_BORDER_COLOR,
		DWMWA_CAPTION_COLOR,
		DWMWA_TEXT_COLOR,
		DWMWA_VISIBLE_FRAME_BORDER_THICKNESS,
		DWMWA_SYSTEMBACKDROP_TYPE,
		DWMWA_LAST
	}

    //--------------------------
    //  Generic Flash Element
    //--------------------------

    public static void FlashElement(this Button Target, Color FlashColor, int FlashDurationMs) => FlashGeneric(Target, FlashColor, FlashDurationMs);
	public static void FlashElement(this Label Target, Color FlashColor, int FlashDurationMs) => FlashGeneric(Target, FlashColor, FlashDurationMs);
	public static void FlashElement(this TextBox Target, Color FlashColor, int FlashDurationMs) => FlashGeneric(Target, FlashColor, FlashDurationMs);
	public static void FlashElement(this Control Target, Color FlashColor, int FlashDurationMs) => FlashGeneric(Target, FlashColor, FlashDurationMs);

    private static async void FlashGeneric(dynamic Ctrl, Color FlashColor, int FlashDurationMs) {
		if (Ctrl.ForeColor == FlashColor) return;
		Ctrl.ForeColor = FlashColor;
		await Task.Delay(FlashDurationMs);
		Ctrl.ForeColor = Color.Black;
    }


	//-------------------------------
    //       RichTextBox
    //-------------------------------

    public static void AppendText(this RichTextBox Box, string Text, Color Col) {

	   // lock (Busy) {
		    Box.SelectionStart = Box.TextLength;
		    Box.SelectionLength = 0;
		    Box.SelectionColor = Col;
		    Box.AppendText(Text);
		    Box.SelectionColor = Box.ForeColor;
	    //}
    }

    //-----------------------------
    //        Generic
    //-----------------------------
    /// <summary>
    /// Get A List Of All Child Controls Recursively
    /// </summary>
    /// <param name="Base">Control To Search</param>
    /// <returns></returns>
    public static List<dynamic> GetAllChildControls(Control Base) {
	    List<dynamic> list = [Base];
	    //This Should not work...
        foreach (Control C in Base.Controls) {
	        list.AddRange(GetAllChildControls(C));
        }

	    return list;
    }


    /// <summary>
    /// Get A List Of All Child Controls Recursively Given A Specific Control Type
    /// </summary>
    /// <param name="Base">Control To Search</param>
    /// <returns></returns>
    public static List<Control> GetChildControlsOfType<T> (Control Base){
	    List<Control> list = [(dynamic)Base];
	    //THIS IS A SPICEY SPAGHETTI
	    foreach (Control C in Base.Controls) {
		    list.AddRange(GetChildControlsOfType<T>(C).Where(X => X.GetType() == typeof(T)));
	    }

	    return list;
    }


    /// <summary>
    /// Checks wether we are in designer mode. Used as a hack to keep the winforms designer from executing certain stuff.
    /// </summary>
    /// <returns></returns>
    public static bool IsDesigner() => LicenseManager.UsageMode == LicenseUsageMode.Designtime;


    //-------------------------------
    //        ComboBox
    //-------------------------------

    //TODO This thing is currently broken

    /// <summary>
    /// Finds And Selects The Closest Matching String
    /// </summary>
    /// <param name="Target"></param>
    /// <param name="Input"></param>
    public static void SelectClosest(this ComboBox Target, string Input) {
	    List<int> Distances = (from object T in Target.Items select Distance(T.ToString(), Input)).ToList();
	    int Closest = Array.IndexOf(Distances.ToArray(), Distances.Min());
	    Target.SelectedIndex = Closest;
    }

    /// <summary>
    /// Extended Append Text.
    /// Given A string Will Find Specially Formated Delimiters And Apply A Predefined Text Style Based On Said Delimiter
    /// </summary>
    /// <param name="Box">Fast Colored Text box</param>
    /// <param name="Text">Formated Text (Vallid Color Codes Are: &0-&9)</param>

	public static void InvokeC(this Control Obj, MethodInvoker Del) {
		try {
			if (Obj.InvokeRequired) {
				if (Obj.IsDisposed) return;
				Obj.Invoke(Del);
			}
			else Del();
		}
		catch (Exception Ex) {
			Logger.Warn(Ex.Message);
		}
    }

	public static T InvokeC<T>(this Control Obj, Func<T> Function) {
		if (Obj.InvokeRequired) {
			return (T) Obj.Invoke(Function);
		} return Function();
	}

	public static void InvokeC(this Form Obj, MethodInvoker Del) {
		try {
			if (Obj.InvokeRequired) {
				if (Obj.IsDisposed) return;
				Obj.Invoke(Del);
			}
			else Del();
		} 
		catch (Exception Ex) {
			Logger.Warn(Ex.Message);
		}
    }

	public static T InvokeC<T>(this Form Obj, Func<T> Function) {
        if (Obj.InvokeRequired) {
            return (T)Obj.Invoke(Function);
        }
        return Function();
    }

    public static void BeginInvokeC(this Control Obj, MethodInvoker Del) {
	    try {

		    if (Obj.InvokeRequired) {
			    if (Obj.IsDisposed) return;
			    Obj.BeginInvoke(Del);
		    }
		    else Del();
	    }
	    catch (Exception Ex) {
		    Logger.Warn(Ex.Message);
            throw;
	    }
    }

    public static T BeginInvokeC<T>(this Control Obj, Func<T> Function) {
	    try {
		    if (Obj.InvokeRequired) {
			    return (T)Obj.BeginInvoke(Function);
		    }
			return Function();
	    } 
	    catch (Exception Ex) {
	        Logger.Warn(Ex.Message);
	        throw;
        }
    }

    public static void BeginInvokeC(this Form Obj, MethodInvoker Del) {
        if (Obj.InvokeRequired) {
	        if (Obj.IsDisposed) return;
            Obj.BeginInvoke(Del);
        } else Del();
    }

    public static T BeginInvokeC<T>(this Form Obj, Func<T> Function) {
        if (Obj.InvokeRequired) {
            return (T)Obj.BeginInvoke(Function);
        }
        return Function();
    }


    public static void SetDoubleBuffered(Control C, bool DoubleBuff) {
	    try {
		    typeof(Control).InvokeMember("DoubleBuffered",
			    BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, C,
			    new object[] { DoubleBuff });
	    } catch {
		    // ignored
	    }

    }

    //-------------------------
    //       Methods
    //-------------------------

    /// <summary>
    /// Windows DWM API Window Restore
    /// </summary>
    /// <param name="Form"></param>
    public static void Restore(this Form Form) {
        if (Form.WindowState == FormWindowState.Minimized) {
            ShowWindow(Form.Handle, SW_RESTORE);
        }
    }

    /// <summary>
    /// Force The Titlebar To Use The Dark Theme
    /// </summary>
    /// <param name="Form"></param>
    public static void SetTitleBarDarkMode(this Form Form) => DwmSetWindowAttribute(Form.Handle, DWMWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, new[] { 1 }, sizeof(int));


    /// <summary>
    /// Prevent OnDraw Event From Firing
    /// </summary>
    /// <param name="Parent"></param>
    public static void SuspendDrawing(this Control Parent) => SendMessage(Parent.Handle, WM_SETREDRAW, 0, IntPtr.Zero);


    /// <summary>
    /// Resume OnDraw Events
    /// </summary>
    /// <param name="Parent"></param>
    public static void ResumeDrawing(this Control Parent) {
        SendMessage(Parent.Handle, WM_SETREDRAW, 1, IntPtr.Zero);
        Parent.Refresh();
    }

    /// <summary>
    /// Prevent OnPaint From Firing
    /// </summary>
    /// <param name="Parent"></param>
    public static void SuspendPaint(this Control Parent) => SendMessage(Parent.Handle, WM_PAINT, 0, IntPtr.Zero);


    /// <summary>
    /// Resume OnPaint Events
    /// </summary>
    /// <param name="Parent"></param>
    public static void ResumePaint(this Control Parent) {
        SendMessage(Parent.Handle, WM_PAINT, 1, IntPtr.Zero);
        Parent.Refresh();
    }




    private const int WM_SETREDRAW = 0x000B;
    private const int WM_USER = 0x400;
    private const int EM_GETEVENTMASK = WM_USER + 59;
    private const int EM_SETEVENTMASK = WM_USER + 69;

    [DllImport("user32", CharSet = CharSet.Auto)]
    private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);

    private static IntPtr eventMask = IntPtr.Zero;

    public static void StopDrawing(this Control Frm) {
	    //if (drawStopCount == 0) {
		    // Stop redrawing:
		    SendMessage(Frm.Handle, WM_SETREDRAW, 0, IntPtr.Zero);
		    // Stop sending of events:
		    eventMask = SendMessage(Frm.Handle, EM_GETEVENTMASK, 0, IntPtr.Zero);
	    //}
	    //drawStopCount++;
    }

    public static void StartDrawing(this Control Frm) {
	   // drawStopCount--;
	    //if (drawStopCount == 0) {
		    // turn on events
		    SendMessage(Frm.Handle, EM_SETEVENTMASK, 0, eventMask);

		    // turn on redrawing
		    SendMessage(Frm.Handle, WM_SETREDRAW, 1, IntPtr.Zero);

		    Frm.Invalidate();
		    Frm.Refresh();
	    //}
    }

	/// <summary>
	/// Get Assembly Build Time
	/// </summary>
	/// <param name="Asm">Target C# Assembly</param>
	/// <returns>DateTime Of Built Assembly</returns>
	public static DateTime GetBuildTime(Assembly Asm) => GetBuildTime(Asm.Location);

    /// <summary>
    /// Get Assembly Build Time
    /// </summary>
    /// <param name="Path">string Path To Target Assembly</param>
    /// <returns>DateTime Of Built Assembly</returns>
    public static DateTime GetBuildTime(string Path) {
        const int PEHeaderOffset = 60;
        const int LinkerTimestampOffset = 8;
        byte[] Bytes = new byte[2048];

        using (FileStream File = new(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
            File.Read(Bytes, 0, Bytes.Length);
        }

        int Header = BitConverter.ToInt32(Bytes, PEHeaderOffset);
        int Epoch = BitConverter.ToInt32(Bytes, Header + LinkerTimestampOffset);
        DateTime Delta = new(1970, 1, 1, 2, 0, 0, DateTimeKind.Utc);
        return Delta.AddSeconds(Epoch);
    }

    /// <summary>
    /// Get Current Assembly Build Info Version
    /// </summary>
    /// <returns>Assembly Version</returns>
    public static string GetBuildInfo() {
        Version Ver = Assembly.GetEntryAssembly()!.GetName().Version;
        return $" (v{Ver.Major}.{Ver.Minor}.{Ver.Build}.{Ver.Revision})";
    }


    public static bool IsInDesignMode() {
        if (Application.ExecutablePath.IndexOf("devenv.exe", StringComparison.OrdinalIgnoreCase) > -1) {
            return true;
        }
        return false;
    }

}
