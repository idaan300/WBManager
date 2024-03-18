using System;
using System.Diagnostics;
using System.Management;
using System.Threading;
using System.Threading.Tasks;


namespace RobotManager.Utility;

public static class Threading {
	/// <summary>
	/// Wraper For Semahpore Release That Checks If The Given Semaphore Is Locked First.
	/// </summary>
	/// <param name="Sem"></param>
	/// <returns></returns>
	public static int CheckRelease(this SemaphoreSlim Sem) {
		try {
			return Sem?.CurrentCount == 0 ? Sem.Release() : int.MinValue;
		}
		catch {
            return int.MinValue;
		}
	}

    /// <summary>
    /// Asynchonously wait for a semaphore release.
    /// </summary>
    /// <param name="Sem"></param>
    /// <param name="TimeoutDuration"></param>
    /// <returns></returns>
	public static async Task<bool> SemAsyncWait(SemaphoreSlim Sem, int TimeoutDuration) => await Sem.WaitAsync(TimeoutDuration);

    /// <summary>
    /// Kill a process, and all of its children, grandchildren, etc.
    /// truely brutal...
    /// </summary>
    /// <param name="PID">Process ID.</param>
    public static void KillProcessAndChildren(int PID) {
	    // Cannot close 'system idle process'.
	    if (PID == 0) return;

	    ManagementObjectSearcher Searcher = new("Select * From Win32_Process Where ParentProcessID=" + PID);
	    ManagementObjectCollection MgmtCol = Searcher.Get();

	    foreach (ManagementBaseObject Obj in MgmtCol) { 
		    ManagementObject MO = (ManagementObject)Obj;
		    KillProcessAndChildren(Convert.ToInt32(MO["ProcessID"]));
	    }

	    try {
		    Process Proc = Process.GetProcessById(PID);
		    Proc.Kill();
	    } catch (ArgumentException) {
		    // Process already exited.
	    }
    }
}