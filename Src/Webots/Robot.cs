using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RobotManager.Webots;


public class Robot {

    //------------------------------
    //
    //------------------------------


    public abstract unsafe class Structures {
        public struct RobotData {
            //Robot Position
            public byte XPos;
            public byte YPos;

            //Robot Name
            public fixed byte Name[32];

            //Each Flag weighs a bit
            public byte CarryCapacity;
            public byte RemainingCapacity;


            //2 Arrays Containing Flag ID's Flags are virtual
            public fixed byte CarriedFlags[16];
            public fixed byte OrderedFlags[16];

        }
    }

    public class Data {

    }

    public void OnReceive(object Obj, byte[] RXData) {
   
        List<byte> RXDataBuffer = new();

    }

    //public bool void CMD_Tick() =>

    public void CMDHandler(object sender, byte[] payload) {

    }

    public void CMDHandler_Ack(object sender, byte[] payload) {

    }

}



