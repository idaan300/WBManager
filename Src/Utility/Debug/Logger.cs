using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

using Pastel;

using static RobotManager.Utility.RColors;


namespace RobotManager.Utility.Debug;

public static class Logger {


    //---------------------------
    //      Constants
    //---------------------------

    private static string FilePath = @"./";
    private const string DefaultFileName = @"RobotManager";
    private const string Ext = @"log";
    private static StreamWriter LogStream;
    
    private static readonly string M_Critical = "ERR".Pastel(ConsoleColor.White).PastelBg(Red);
    private static readonly string M_Info = "INF".Pastel(Green);
    private static readonly string M_Warn = "WAR".Pastel(Yellow);
    private static readonly string M_Error = "ERR".Pastel(Red);
    private static readonly string M_Verbose = "DBG".Pastel(Color.CadetBlue);
    private static readonly string M_HEX = "HEX".Pastel(Sage);
    private static readonly Color Col_File = Color.CornflowerBlue;
    private static readonly Color Col_Line = Color.DarkCyan;
    private static readonly Color Col_Method = Color.Khaki;

    private static readonly object FileLock = new();

    //---------------------------
    //      Enums
    //---------------------------

    //Fields
    [Flags]
    public enum LogSeverity : byte {
	    None = 0,
	    Info = 1,
	    Warn = 2,
	    Error = 3,
        Verbose = 4,
        HexDump = 5,
    };

    private static readonly LogSeverity CurrentSev = LogSeverity.HexDump;

    private static readonly Color[] TextColors = [
	    Color.White,
	    Green,
	    Yellow,
	    Red,
	    Blue,
	    Gray,
	    Sage,
	    Color.AntiqueWhite
    ];



    //---------------------------
    //      Public
    //---------------------------



    public static void Trace(dynamic DynMesg) {
        string StackDump = GetStack();
        (string ConsoleM, string _) = Parse((string)DynMesg.ToString());

        string OutputC = $"[{GetTime()} {"STK".Pastel(Color.Magenta)}] {ConsoleM}\n{StackDump}";
        Console.WriteLine(OutputC);

    }


    public static void Hex(byte[] Mesg, [CallerFilePath] string Source = "", [CallerMemberName] string Caller = "", [CallerLineNumber] int Line = 0) => PrintHex(Mesg, Source, Caller, Line);
    public static void Hex(char[] Mesg, [CallerFilePath] string Source = "", [CallerMemberName] string Caller = "", [CallerLineNumber] int Line = 0) => PrintHex(Encoding.ASCII.GetBytes(Mesg), Source, Caller, Line);
    public static void Hex(string Mesg, [CallerFilePath] string Source = "", [CallerMemberName] string Caller = "", [CallerLineNumber] int Line = 0) => PrintHex(Encoding.ASCII.GetBytes(Mesg), Source, Caller, Line);

    public static void Info(dynamic DynMesg, [CallerFilePath] string Source = "", [CallerMemberName] string Caller = "", [CallerLineNumber] int Line = 0) => Print(DynMesg, Source, Caller, Line, M_Info, "INF", LogSeverity.Info);
    public static void Warn(dynamic DynMesg, [CallerFilePath] string Source = "", [CallerMemberName] string Caller = "", [CallerLineNumber] int Line = 0) => Print(DynMesg, Source, Caller, Line, M_Warn, "WRN", LogSeverity.Warn);
    public static void Error(dynamic DynMesg, [CallerFilePath] string Source = "", [CallerMemberName] string Caller = "", [CallerLineNumber] int Line = 0) => Print(DynMesg, Source, Caller, Line, M_Error, "ERR", LogSeverity.Error);

    public static void InitLogger(string LogFilePath) {

        FilePath = LogFilePath;
        lock (FileLock) {
            LogStream = InitalizeFileLogger();
        }
    }

    private static string GetStack(int Level = 100) {

        string Out = "";
        StackFrame[] Stack = new StackTrace().GetFrames();

        if (Stack == null)
            return "No Stack Available".Pastel(Color.Red);
        ;
        int Depth = Math.Min(Stack.Length, Level);

        for (int i = 0; i < Depth; i++) {
            MethodBase Method = Stack[i].GetMethod();
            if (Method?.DeclaringType is null) {
                Out += "\tNo Frame Avaiable\n".Pastel(Color.Red);
                continue;
            }
            Out += $"\t{Method.DeclaringType.FullName.Pastel(Color.DeepPink)}->{Method.Name.Pastel(Color.HotPink) + $"({string.Join<string>(",", Method.GetParameters().Select(X => X.ParameterType.Name))})".Pastel(Color.Aqua)}@{("0x" + Method.MethodHandle.Value.ToString("X")).Pastel(Color.Chartreuse)}\n";
        }

        return Out.Pastel(Color.White);
    }


    //---------------------------
    //      Private
    //---------------------------

    //FIXME Later
    //Currently each call checks severity. A better method would be to detour the methodcall to a stub depending on severity.
    //Its possible but will take way too much effort.
    private static void Print(dynamic DynMsg, string Src, string Method, int Line, string ConMsg, string FMsg, LogSeverity Sev) {
        if (Sev > CurrentSev)
            return;

        (string ConsoleM, string FileM) = Parse((string)(DynMsg.ToString()));
        string File = GetFileNameNoExt(Src);
        Method = Method.Trim('.');
        string OutputC = $"[{GetTime()} {ConMsg}][{$"{File}.cs".Pastel(Col_File)}:{Line.ToString().Pastel(Col_Line)}({Method.Pastel(Col_Method)})] {ConsoleM}".Pastel(Color.White);
        string OutputF = $"[{GetTime()} {FMsg}[{File}.cs:{Line}({Method})] {FileM}";

        Console.WriteLine(OutputC);
        FileLogWrite(OutputF);

    }

    public static void PrintRaw(dynamic DynMsg) {
        (string ConsoleM, string FileM) = Parse((string)(DynMsg.ToString()));
        Console.WriteLine(ConsoleM);
    }

    private static void PrintHex(IReadOnlyList<byte> BMsg, string Src, string Method, int Line) {
        if (CurrentSev != LogSeverity.HexDump)
            return;
        string Msg = HexDump(BMsg);
        string File = GetFileNameNoExt(Src);
        Method = Method.Trim('.');
        string OutputC = $"[{GetTime()} {M_HEX}][{$"{File}.cs".Pastel(Col_File)}:{Line.ToString().Pastel(Col_Line)}({Method.Pastel(Col_Method)})]\n{Msg}".Pastel(Color.White);
        //string OutputF = $"[{GetTime()} HEX][{File}.cs:{Line}({Method})] {"\n" + Msg}";

        Console.WriteLine(OutputC);
        // FileLogWrite(OutputF);
    }

    private static StreamWriter InitalizeFileLogger() {
        string LogFile = $"{FilePath}/{DefaultFileName}.{Ext}";

        if (!File.Exists(Path.GetFullPath($"{FilePath}/{DefaultFileName}.{Ext}"))) {
            File.Create($"{FilePath}/{DefaultFileName}.{Ext}").Close();
        }

        else
            File.WriteAllText(LogFile, string.Empty);

        Stream LogFileStream = File.Open(LogFile, FileMode.Open, FileAccess.Write, FileShare.Read);
        StreamWriter SW = new(LogFileStream);

        if (LogFileStream == Stream.Null)
            throw new("File Logger Init Error: Stream Was Null");
        if (SW == StreamWriter.Null)
            throw new("File Logger Init Error: StreamWriter Was Null");

        SW.WriteLine("Logging Started");
        SW.Flush();

        return SW;

    }

    private static void FileLogWrite(string Message) {

        lock (FileLock) {
            LogStream.WriteLine(Message);
            LogStream.Flush();
        }
    }



    /// <summary>
    /// Convert A Byte Array To A Formated Hexidecimal Representation
    /// </summary>
    /// <param name="Bytes">Byte Array To Convetr</param>
    /// <param name="Name">Short Description Of Data</param>
    /// <param name="BytesPerLine">Show X Bytes / Line</param>
    /// <returns></returns>
    private static string HexDump(IReadOnlyList<byte> Bytes, int BytesPerLine = 24) {
        if (Bytes == null) return "<null>";
        int ByteLen = Bytes.Count;

        char[] HexChars = "0123456789ABCDEF".ToCharArray();

        const int FirstHexColumn = 8        // 8 characters for the address     
                                 + 3;       // 3 spaces

        int FirstCharColumn = FirstHexColumn
            + BytesPerLine * 3              // - 2 digit for the hexadecimal value and 1 space
            + (BytesPerLine - 1) / 8        // - 1 extra space every 8 characters from the 9th
            + 2;                            // 2 spaces 

        int LineLen = FirstCharColumn
            + BytesPerLine                  // - characters to show the ascii value
            + Environment.NewLine.Length;   // Carriage return and line feed (should normally be 2)

        char[] Line = (new string(' ', LineLen - Environment.NewLine.Length) + Environment.NewLine).ToCharArray();
        StringBuilder Result = new("---------------- \n");

        for (int i = 0; i < ByteLen; i += BytesPerLine) {

            Line[0] = HexChars[(i >> 28) & 0xF];
            Line[1] = HexChars[(i >> 24) & 0xF];
            Line[2] = HexChars[(i >> 20) & 0xF];
            Line[3] = HexChars[(i >> 16) & 0xF];
            Line[4] = HexChars[(i >> 12) & 0xF];
            Line[5] = HexChars[(i >> 8) & 0xF];
            Line[6] = HexChars[(i >> 4) & 0xF];
            Line[7] = HexChars[(i >> 0) & 0xF];

            int HexColumn = FirstHexColumn;
            int CharColumn = FirstCharColumn;

            for (int j = 0; j < BytesPerLine; j++) {
                if (j > 0 && (j & 7) == BytesPerLine) HexColumn++;
                if (i + j >= ByteLen) {
                    Line[HexColumn] = ' ';
                    Line[HexColumn + 1] = ' ';
                    Line[CharColumn] = ' ';
                } else {
                    byte b = Bytes[i + j];
                    Line[HexColumn] = HexChars[(b >> 4) & 0xF];
                    Line[HexColumn + 1] = HexChars[b & 0xF];
                    Line[CharColumn] = b < 0x1F ? '.' : (char)b;
                }
                HexColumn += 3;
                CharColumn++;
            }
            Result.Append(Line);
        }
        return Result.ToString();
    }

    private static string GetTime() => DateTime.Now.ToLongTimeString();
    private static string GetFullTime() => $"{DateTime.Now.ToShortDateString()}|{DateTime.Now.ToLongTimeString()}";


    //TODO Horribly Slow, Needs Rewrite
    private static (string,string) Parse(string Text) {
	    Text = Text.Insert(0, "&0");                              //Insert Black Color At the Beginning to Fix Edge case
	    string[] Segments = Regex.Split(Text, @"(&{1}\d{1})");      //Idea copied from a game
	    int LastProcessedIndex = 0;
	    string FormatedText = "";
	    string FormatedPlain = "";

	    //If no Color Directives are given There will be no splitting
	    if (Segments.Length < 1) return (Text.Pastel(TextColors[0]), Text);
	    
	    AppendRetry:

	    try {

		    for (int i = LastProcessedIndex; i < Segments.Length; i++) {
			    LastProcessedIndex++;
			    if (string.IsNullOrEmpty(Segments[i]) || i > Segments.Length) continue;
			    //For Each Segment That Contains a Separated "&n" Set The Fetched Index Color
			    if (Segments[i][0] == '&' && int.TryParse(Segments[i][1].ToString(), out int ColorIndex)) {
                    //Text Is 1 index higher
                    FormatedText += Segments[i + 1].Pastel(TextColors[ColorIndex]);
                    FormatedPlain += Segments[i + 1];
			    }
		    }
	    }

	    //If For Some reason it fails Revert back to a simple white text append for the Specific string Index
	    catch {
		    for (int i = LastProcessedIndex; i < Segments.Length; i++) {
			    if (string.IsNullOrEmpty(Segments[i]) || i > Segments.Length) continue;
			    FormatedText += Segments[i].Pastel(TextColors[0]);
            }
		    goto AppendRetry;
        } 
	    return string.IsNullOrEmpty(FormatedText) ? (Text,Text) : (FormatedText,FormatedPlain);
    }


    private static string GetFileName(string path) {
	    if (path != null) {
		    int length = path.Length;
		    int index = length;
		    while (--index >= 0) {
			    char ch = path[index];
			    if (ch == Path.DirectorySeparatorChar || ch == Path.AltDirectorySeparatorChar || ch == Path.VolumeSeparatorChar)
				    return path.Substring(index + 1, length - index - 1);
		    }
	    }
	    return path;
    }

    private static string GetFileNameNoExt(string path) {
	    path = GetFileName(path);
	    if (path == null)
		    return null;
	    int length;
	    return (length = path.LastIndexOf('.')) == -1 ? path : path.Substring(0, length);
    }

}