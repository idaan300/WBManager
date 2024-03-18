using RobotManager.Utility.Debug;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Net.Sockets;
using System.Threading.Tasks;
using System;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;
using System.Net;               //required
using System.Net.Sockets;       //required
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Text;
using RobotManager.Webots;

namespace RobotManager.Server;


public class Server {

    public List<RobotListener> Instances;
    public static readonly byte[] MagicNum = [0x55,0x32,0x29,0x11]; //if we recieve this

    public class RobotListener {

        public event EventHandler<byte[]> OnRecieve;

        private TcpListener Listener;
        private Socket TCPSocket;
        private bool Run = true;

        private Queue<byte[]> TransmitQueue;


        public RobotListener(IPAddress Address, ushort Port) {
            Listener = new(Address, Port);
            Logger.Info($".ctor &1OK &4Address: {Address} &5Port:{Port}");
        }

        public void Init() {
            Listener.Start();
            Logger.Info($"Init Listener, Local Enpoint: {Listener.LocalEndpoint}");
            Logger.Info($"Call AcceptSocket");

            //Blocking Method, Thread Waits Here Until A Client Connection is started
            TCPSocket = Listener.AcceptSocket();
            Logger.Info($"Socket Accepted, Communication Established");
            Logger.Info($"Call MainLoop");
            MainLoop();
        }

        private void MainLoop() {


            while (Run) {

                byte[] Buffer = new byte[256];

                try {
                    Thread.Sleep(100);
                    TCPSocket.Send(new byte[(byte)WeEntity.Structures.EntityCommands.ECMD_Tick]);
                }
                catch (SocketException Ex) {
                    Logger.Critical("FFS");
                    throw;
                }
                catch (ObjectDisposedException) {
                    Logger.Warn("TCPSend On Disposed Object");
                    Run = false;
                }

                int BytesRecieved = TCPSocket.Receive(Buffer);
                Assertions.Assert(BytesRecieved, Buffer[0]);
                Logger.Hex(Buffer);

                if ()


            }
        }

    }


    //Server stuurt tick een robot stuurt in respose de robot struct terug.

}
