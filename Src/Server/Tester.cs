using System;
using System.Net;
using System.Net.Sockets;
using RobotManager.Utility.Debug;

namespace RobotManager.Server {
    public class Tester {

        Socket s = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public Tester(string Addr, int Prt) {
            if (!IPAddress.TryParse(Addr, out IPAddress IP)) {
                Logger.Trace("Test Parse Fail");
                return;
            }
            
            try {
                Logger.Info("Test Send on {IP} {Port}");
                s.Connect(IP, Prt);
            }
            catch (Exception Ex) {
                Logger.Trace($"Test Exception {Ex.Message}");
            }
        }

        public void Send(byte[] OutBuffer) {
            if (!s.Connected) {
                Logger.Error("NOT CONNECTED");
            }
            Logger.Hex(OutBuffer);
            s.Send(OutBuffer);
        }

    }
}
