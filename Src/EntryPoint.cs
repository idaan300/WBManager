using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

using RobotManager.Forms;
using RobotManager.Properties;
using RobotManager.Utility;
using RobotManager.Utility.Debug;

using static Bluegrams.Application.PortableSettingsProvider;
using static Bluegrams.Application.PortableSettingsProviderBase;


namespace RobotManager;

internal static class EntryPoint {
    private static string CurrentDir => $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}";
    private static string WorkingDir => $@"{CurrentDir}\Data";

    [STAThread]
    private static void Main() {
        ConsoleEx.Init("RobotManager Debug");
        Logger.InitLogger(WorkingDir);
        InitWinformsSettings();
        Logger.Info("Init");
        ExceptionEx.Initialize();

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new FormMain());
    }

    private static void InitWinformsSettings() {
        SettingsFileName = "Settings.xml";
        SettingsDirectory = WorkingDir;
        AllRoaming = true;

        DirectoryInfo Dir = new(WorkingDir);

        try {
            if (!Dir.Exists) {
                Directory.CreateDirectory(WorkingDir);
                Logger.Info(@$"&3Missing Folder, &0Creating: \{Misc.GetRelativePath(@$"{WorkingDir}\{SettingsFileName}", CurrentDir)}");
            }

            else Logger.Info(@$"Settings Folder: \{Misc.GetRelativePath(@$"{WorkingDir}\{SettingsFileName}", CurrentDir)}");

            ApplyProvider(Settings.Default);
            Settings.Default.Save();
        }
        catch {
            Logger.Trace("Could Not Create Settings File");
            MessageBox.Show("FATAL: Could Not Create Settings! The Application Will Close.", "Fatal Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.Exit();
        }
    }

}

