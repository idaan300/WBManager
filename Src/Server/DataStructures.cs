using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using RobotManager.Utility.Debug;

namespace RobotMananager;

public unsafe static class DataStructures {

    //public enum SensorDetected : byte {
    //    None,
    //    Up,
    //    Down,
    //    Left,
    //    Right,
    //}


    public enum TCPCommands : byte {
        RB_PROT_INIT = 0xAA,

        RB_PROT_UNDEFINED = 0x00,
        RB_PROT_ROBOTDATA = 0x01,
        RB_PROT_FLAGDATA = 0x02,

        RB_PROT_SETABSPOS = 0x03,

        RB_PROT_ALLYLIST = 0x04,

        RB_PROT_FLAGPICKED_UP = 0x10,

        RB_PROT_MAPDATA = 0x03,
        RB_PROT_EMGSTOP = 0xFF,
        RB_PROT_TICK = 0xBB,
    }

    //Used By The WebotsMap Renderer
    public enum WeTile : byte {
        None = 0x00,
        Wall = 0x01,

        Spawn1 = 0x10,
        Spawn2 = 0x12,
        Spawn3 = 0x13,
        Spawn4 = 0x14,

        Robot1 = 0x20,
        Robot2 = 0x21,
        Robot3 = 0x22,
        Robot4 = 0x23,

        //Flag is a special case
        Flag = 0x30,
    }

    //[Flags]
    //enum TileData : byte {
    //    Empty,
    //    Wall,
    //    Base,       //Robot Spawn Point
    //    Flag,          
    //}

    private enum RobotAction {
        Wait,
        MoveToTarget,
        DoFuckAll,
        Pickup,
        Drop,
    }

    //-------------
    //  MISC
    //-------------

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Vec3f {
        public Double X, Y, Z;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Vec2f {
        public Double X, Y;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Vec2I {
        public Int32 X, Y;
    }

    // //Struct that Contains A Struct, So Marshal.Copy and Marshal.PtrToStruct Become Unreliable
    // //Manually Get The Pinned Ptr To the Struct When Converting
    // [StructLayout(LayoutKind.Sequential, Pack = 1)]
    // public unsafe struct RobotData {
    //     //Byte ;                        //Robot ID
    //     private Vec3f Pos;                      //Current Robot Pos In Webots Each Block is 0.1f in X,Z Axis
    //     private WeTile SpawnPoint;      //TileID of SpawnPoint
    //     private Int32 CarryCapacity;            //Remaining Carry Capacity
    //     private RobotSensor SensN;              //Robot Distance / Colission Sensor. C# Cant Have Fixed Struct Arrays So Just Make 4 Copies,
    //     private RobotSensor SensE;              //Memory WebotsMap Is Still The Same When We Convert The Byte Payload to A Struct
    //     private RobotSensor SensS;
    //     private RobotSensor SensW;
    //
    //     //Init Capacity To 100
    //     public RobotData() {
    //         CarryCapacity = 100;
    //     }
    // }



    // //A Robot Sensor Just Returns The Tiles it sees 1 block ahead
    // [StructLayout(LayoutKind.Sequential, Pack = 1)]
    // public unsafe struct RobotSensor {
    //     public WeTile Type;
    // }

    // //Flag (Target) Struct
    // [StructLayout(LayoutKind.Sequential, Pack = 1)]
    // public unsafe struct FlagData {
    //     public Int32 ID;        //Flag ID
    //     public Vec2I Pos;       //2D Vector Of Flag Pos On WebotsMap Grid
    //     public Byte Weight;     //Flag Weight
    // }


    /// <summary>
    /// WebotsMap Data Struct
    /// </summary>
    ///

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct WebotsMap {

        private fixed Byte MapData[10 * 10];

        public WeTile GetTile(int X, int Y) {
            if (X > 10 || Y > 10 || Y < 0 || X < 0) throw new("Invallid Coordinates");
            int Index = X * 10 + Y;
            return (WeTile)MapData[Index];
        }

        private void SetTile(int X, int Y, WeTile Type) {
            if (X > 10 || Y > 10 || Y < 0 || X < 0) throw new("Invallid Coordinates");
            int Index = Y * 10 + X;
            MapData[Index] = (byte)Type;
        }

        private void MakeWallVert(int StartPosX, int StartPosY, int Height) {
            for (int i = StartPosY; i < StartPosY + Height; i++) {
                //Debug.WriteLine("ADD ENT TO:" + StartPosX + "," + i);
                SetTile(i, StartPosX, WeTile.Wall);
            }
        }

        private void MakeWallHor(int StartPosX, int StartPosY, int Width) {
            for (int i = StartPosX; i <= StartPosX + Width; i++) {
                //Debug.WriteLine("ADD ENT TO:" + StartPosX + "," + i);
                SetTile(i, StartPosY, WeTile.Wall);
            }
        }

        public void CreateMap() {
            for (int i = 0; i < 10; i++) {
                for (int j = 0; j < 10; j++) {
                    SetTile(j, i, WeTile.None);
                }
            }

            //0,0 Pink Spawn: ID 1
            SetTile(0, 0, WeTile.Spawn1);

            //10,0 Yel Spawn: ID 2
            SetTile(9, 0, WeTile.Spawn2);

            //0,0 Pink Spawn: ID 1
            SetTile(0, 9, WeTile.Spawn3);

            //10,0 Yel Spawn: ID 2
            SetTile(9, 9, WeTile.Spawn4);

            //Column 1
            MakeWallHor(1, 1, 2);
            MakeWallHor(1, 3, 2);
            MakeWallHor(1, 6, 2);
            MakeWallHor(1, 8, 2);

            //Column 2
            MakeWallHor(6, 1, 2);
            MakeWallHor(6, 3, 2);
            MakeWallHor(6, 6, 2);
            MakeWallHor(6, 8, 2);

            // fixed (byte* MapDataPtr = MapData) {
            //     byte[] Temp = new byte[10 * 10];
            //     Marshal.Copy((IntPtr)MapDataPtr, Temp, 0, 10 * 10);
            //     Logger.Hex(Temp, 10);
            // }
        }

        //.Ctor
        public WebotsMap() => CreateMap();
    }
}

