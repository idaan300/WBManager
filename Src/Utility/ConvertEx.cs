using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using RobotManager.Utility.Debug;

namespace RobotManager.Utility;

public static class ConvertEx {


	//-----------------------
	//     Data Types
	//-----------------------

    //3 Byte Data Type (Int24)
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
	public readonly struct Int24(uint Value) {
		private readonly byte b0 = (byte)(Value & 0xFF);
		private readonly byte b1 = (byte)(Value >> 8);
		private readonly byte b2 = (byte)(Value >> 16);

		public int Value => b0 | (b1 << 8) | (b2 << 16);
	}

	//3 Byte Data Type (Unsigned)
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public readonly struct UInt24(uint Value) {
		private readonly byte b0 = (byte)(Value & 0xFF);
		private readonly byte b1 = (byte)(Value >> 8);
		private readonly byte b2 = (byte)(Value >> 16);
		public uint Value => (uint)(b0 | (b1 << 8) | (b2 << 16));
	}

    //-----------------------
    //     Converters
    //-----------------------

    /// <summary>
    /// Convert Byte[] -> String With default Charset
    /// </summary>
    /// <param name="Bytes"></param>
    /// <returns></returns>
    public static string BytesToString(byte[] Bytes) => BitConverter.ToString(Bytes);

    /// <summary>
    /// Convert Byte[] -> Struct of Type T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Bytes"></param>
    /// <returns></returns>
    public static T BytesToStruct<T>(byte[] Bytes) where T : struct {
		GCHandle handle = GCHandle.Alloc(Bytes, GCHandleType.Pinned);
		T result = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
		handle.Free();
		return result;
	}

    /// <summary>
    /// Managed Byte Array -> Unmanaged Type
    /// (Use this to convert Payload arrays to structs)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Bytes"></param>
    /// <returns></returns>
    public static unsafe T BytesToUnmanaged<T>(byte[] Bytes) where T : unmanaged {
	    try {
		    T Temp;
		    Marshal.Copy(Bytes, 0, (IntPtr)(&Temp), sizeof(T));
		    return Temp;
	    }
	    catch (Exception Ex) {
            Logger.Trace($"Parse Exception on struct: {typeof(T)}.\n{Ex.Message}");
	    }
	    return new();
    }

    /// <summary>
    /// Managed byte Array -> Unmanaged Type With Given Length
    /// (Use this to convert Payload arrays to structs)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Bytes"></param>
    /// <param name="Length"></param>
    /// <returns></returns>
    public static unsafe T BytesToUnmanaged<T>(byte[] Bytes, int Length) where T : unmanaged {
	    T Type;
	    Marshal.Copy(Bytes, 0, (IntPtr)(&Type), Length);
	    return Type;
    }


    /// <summary>
    /// Converts Any Object -> Byte[]
    /// </summary>
    /// <param name="Obj"></param>
    /// <returns></returns>
    public static byte[] ObjectToBytes(object Obj) {
	    int Size = Marshal.SizeOf(Obj);
	    byte[] Buffer = new byte[Size];
	    IntPtr BufferPtr = Marshal.AllocHGlobal(Size);
	    Marshal.StructureToPtr(Obj, BufferPtr, false);
	    Marshal.Copy(BufferPtr, Buffer, 0, Size);
	    Marshal.FreeHGlobal(BufferPtr);
	    return Buffer;

    }

    /// <summary>
    /// Unmanaged Struct/Type -> Managed byte Array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="Type"></param>
    /// <returns></returns>
    public static unsafe byte[] UnmanagedToBytes<T>(in T Type) where T : unmanaged {
	    int Length = sizeof(T);
	    byte[] Buffer = new byte[Length];

	    fixed (T* BufferPtr = &Type) {
		    Marshal.Copy((IntPtr)BufferPtr, Buffer, 0, Length);
	    }
	    return Buffer;
    }


    /// <summary>
    ///Convert From string -> Byte* With an enforcable Max Length
    ///Throws an exception if the provided string is larger than the enforced max length
    /// </summary>
    public static unsafe void StringToBytePtr(in byte* InB, in string InS, int MaxLen) {
	    byte[] StrByte = Encoding.Default.GetBytes(InS);
	    if (StrByte.Length > MaxLen) throw new ArgumentOutOfRangeException();           //If the given string is too big abort

	    //Make Sure The Array is 0-ed out
	    for (int i = 0; i < MaxLen; i++) InB[i] = 0x00;
	    for (int i = 0; i < StrByte.Length; i++) InB[i] = StrByte[i];

    }

    /// <summary>
    ///Convert From byte[]-> Byte* With an enforcable Max Length
    ///Throws an exception if the provided string is larger than the enforced max length
    /// </summary>
    public static unsafe void StringToBytePtr(in byte* InB, in byte[] InS, int MaxLen) {
	    if (InS.Length > MaxLen) throw new ArgumentOutOfRangeException();           //If the given byte[]is too big abort
	    //Make Sure The Array is 0-ed out
	    for (int i = 0; i < MaxLen; i++) InB[i] = 0x00;
	    for (int i = 0; i < InS.Length; i++) InB[i] = InS[i];

    }

    /// <summary>
    /// Byte* -> Managed Byte[]
    /// </summary>
    /// <param name="ptr"></param>
    /// <param name="Size"></param>
    /// <returns></returns>
    public static unsafe byte[] BytePtrToBytes(byte* ptr, int Size) {
	    byte[] Arr = new byte[Size];
	    Marshal.Copy((IntPtr)ptr, Arr, 0, Size);
	    return Arr;
    }

    /// <summary>
    /// Convert a default encoding string to Byte[]
    /// </summary>
    /// <param name="Payload"></param>
    /// <param name="Length"></param>
    /// <returns></returns>
    public static byte[] StringToBytes(string Payload, int Length) {
        byte[] Buffer = new byte[Length];

        Array.Clear(Buffer, 0, Buffer.Length);
        if (!string.IsNullOrWhiteSpace(Payload)) 
            Array.Copy(Encoding.Default.GetBytes(Payload), 0, Buffer, 0, Encoding.Default.GetBytes(Payload).Length);

        return Buffer;
    }

    /// <summary>
    /// Convert a Hex string to Byte[]
    /// (ex. 0x0101 -> Byte[] = {1, 1}
    /// </summary>
    /// <param name="Hex"></param>
    /// <returns></returns>
    public static byte[] HexToBytes(string Hex) {
        int NumberChars = Hex.Length;
    
        byte[] bytes = new byte[NumberChars / 2];
        try {
            for (int i = 0; i < NumberChars; i += 2) 
	            bytes[i / 2] = Convert.ToByte(Hex.Substring(i, 2), 16);
        } 
        catch (Exception) {
            return null;
        }
    
        return bytes;
    }

    /// <summary>
    /// Converts a Byte[] to Int32
    /// </summary>
    /// <param name="Data"></param>
    /// <returns>The Given value as an int, returns Int32.Min if the array is of an invallid size</returns>
    public static int Bytes4ToInt32(byte[] Data) => Data.Length != 4 ? int.MinValue : (Data[3] << 24) | (Data[2] << 16) | (Data[1] << 8) | Data[0];
    
    /// <summary>
    /// Converts a Byte* to Int32
    /// </summary>
    /// <param name="Data"></param>
    /// <param name="Offset"></param>
    /// <returns></returns>
    public static unsafe int BytePtr4ToInt32(byte* Data, int Offset) {
        int Res = Data[Offset + 2] * 65536 + Data[Offset + 1] * 256 + Data[Offset];
        if ((Res & 0x00800000) > 0) Res = (int)((uint)Res | (uint)0xFF000000);
        return Res;
    }

    /// <summary>
    /// Converts a string to an ascii formated byte[]
    /// </summary>
    /// <param name="Str"></param>
    /// <returns></returns>
    public static byte[] StringToASCIIBytes(string Str) {
	    Dictionary<string, byte> HexIndex = new();
	    List<byte> HexRes = [];

        for (int i = 0; i <= 255; i++) HexIndex.Add(i.ToString("X2"), (byte)i);
	    for (int i = Str.Length; i > 2; i -= 2) HexRes.Add(HexIndex[Str.Substring(i - 2, 2)]);

	    return HexRes.ToArray();
    }

    /// <summary>
    /// Convert Unix Time To Local Machine Time
    /// </summary>
    /// <param name="TimeStamp"></param>
    /// <returns></returns>
    public static DateTime UnixTimeToDateTime(long TimeStamp) => DateTimeOffset.FromUnixTimeSeconds(TimeStamp).DateTime.ToLocalTime();

}


