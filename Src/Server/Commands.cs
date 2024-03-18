using System;
using System.Collections.Generic;
using System.Threading;
using RobotMananager;

//------------------------------------
//
//    Commands.cs
//
//    Put All The TCPComm.
//    Related Command Creation Here
//
//------------------------------------


namespace RobotManager.Server{
    /// <summary>
    /// Fix For Winforms Thinking This is a Designer File
    /// There's a better fix for this but it requires messing with visual studio
    /// and i dont have time for that
    /// Just Copy this whole thing to each file which partials MainWindow
    /// </summary>
    public partial class Dummy { }


    public partial class MainWindow {

        //-----------------------------------------------
        //
        //       FLAG GENERATION AND MANAGEMENT
        //
        //-----------------------------------------------


        /// <summary>
        /// Flag Payload Data Structure
        /// CMDBYTE [0]
        /// PAYLOADSIZE [1]
        /// PAYLOAD [2...]
        /// </summary>
        /// <param name="Flag"></param>

        // public void CMDConstruct_SendFlagData(int Target, List<DataStructures.FlagData> Flag) {
        //
        //     //Create A new Byte List And Init it with the tcp cmd for the flag payload
        //     //And the flag count (size of payload)
        //     List<byte> Payload = new() {
        //         (byte)DataStructures.TCPCommands.RB_PROT_FLAGDATA,
        //         (byte)Flag.Count
        //     };
        //
        //     //Serialize each flag struct into a byte array and append it to the list
        //     foreach (DataStructures.FlagData F in Flag) {
        //         byte[] Temp = Utility.MStrToBytes<DataStructures.FlagData>(F);
        //         Payload.AddRange(Temp);
        //     }
        //
        //     TCPTransmit(Target, Payload.ToArray());
        // }

        /// <summary>
        /// Generate A Flag Struct With Random But Vallid Data
        /// </summary>
        /// <param name="IDToGive"></param>
        /// <returns></returns>
        // private DataStructures.FlagData CreateRandomFlag(int IDToGive) {
        //     Random RNG = new();
        //
        //     int XPos = RNG.Next(11);
        //     int YPos = RNG.Next(11);
        //
        //     while (!CheckifVallidPos(XPos, YPos))
        //         CreateRandomFlag(IDToGive);
        //
        //     DataStructures.FlagData Flag = new();
        //     Flag.ID = IDToGive;
        //     Flag.Pos.X = XPos;
        //     Flag.Pos.Y = YPos;
        //     Flag.Weight = (byte)RNG.Next(30, 100);
        //
        //     Console.WriteLine("[Manager] Created Flag At: (" + Flag.Pos.X + "," + Flag.Pos.Y + ") Of Weight: " + Flag.Weight);
        //
        //     return Flag;
        //
        // }

        /// <summary>
        /// If Chosen RNG Pos Is in A Wall Or Spawn Or Player Return False
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        // private bool CheckifVallidPos(int X, int Y) {
        //     if (DataStructures.WebotsMap.GetTile(X, Y) == DataStructures.WeTile.None)
        //         return true;
        //     return false;
        // }


        /// <summary>
        /// Generate A List Of Flags And Send Them To A Specific Target
        /// </summary>
        /// <param name="Target"></param>
        // private void GenFlags(int Target) {
        //     //ToDo After Sending cmd disable button untill we get the cmd back that we have delivered them all
        //     Flags.Clear();
        //
        //     for (int i = 0; i < Nud_FlagCreateCount.Value; i++) {
        //         Flags.Add(CreateRandomFlag(i));
        //     }
        //
        //     //Send Data Given A Target And A List Of Flag Structs
        //     new Thread(() => CMDConstruct_SendFlagData(Target, Flags));
        // }

        /// <summary>
        /// Generate Flags For All Bots In List And Send Them Imediatly
        /// Used During Initialization
        /// </summary>
        // private void AutoGenFlags() {
        //     for (int i = 0; i < CmBox_RobotTasks.Items.Count; i++) {
        //         GenFlags(i);
        //     }
        // }

        //TODO Put This in the right place and Fill the code
        private void TCPTransmit(int Target, byte[] Payload) {

        }

    }
}
