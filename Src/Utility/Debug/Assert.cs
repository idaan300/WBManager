using System.Runtime.CompilerServices;

namespace RobotManager.Utility.Debug;

public static class Assertions {

    public static void Assert(int Value1, int Value2, [CallerFilePath] string Source = "", [CallerMemberName] string Caller = "", [CallerLineNumber] int Line = 0) {

        if (Value1 != Value2)
            throw new($"Assertion Fail {Source}{Line} In {Caller}");
    }

    public static void Assert(bool Value, [CallerFilePath] string Source = "", [CallerMemberName] string Caller = "", [CallerLineNumber] int Line = 0) {
        if (!Value)
            throw new($"Assertion Fail {Source}{Line} In {Caller}");
    }
}