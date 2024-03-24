using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using RobotManager.Utility;
using RobotManager.Utility.Debug;

namespace RobotManager.Server;

public class RobotServer : IDisposable {

    //--------------------------
    //  Fields
    //--------------------------
    private readonly TcpListener TCPListener;    //Listens For Incoming TCP Connections
    private Socket TCPSocket;                    //The Actual TCP Socket, Set Once a listened connection is accepted

    //Recieve buffer
    private byte[] InBuffer;
    private readonly int BufferSize;

    //--------------
    //  Events
    //--------------

    public event EventHandler<EventArgs> ClientConnected;       //Fired when a listened connection is accepted
    public event EventHandler<EventArgs> ClientDisconnected;      //Fired when a listened connection is closed
    public event EventHandler<byte[]> Recieve;                  //Fired when a payload has been recieved
    public event EventHandler<EventArgs> Sent;                     //Fired when an async send command has completed

    //---------------
    //  Semaphores
    //---------------
    
    //Semaphores are used here to be able to async await for certain conditions
    //They are effectively a boolean here
    //That is either set or not set. Another method can set the semaphore and await it
    //When it's unset here it means the thing we wanted to wait on happened, so we can continue now.
    public readonly SemaphoreSlim SemConnect = new(0, 1);
    public readonly SemaphoreSlim SemDisconnect = new(0, 1);
    public readonly SemaphoreSlim SemRecieve = new (0, 1);
    public readonly SemaphoreSlim SemSent = new(0, 1);

    //----------------------
    // Constructor
    //----------------------
    public RobotServer(IPAddress Address, ushort Port, int BufferSize) {
        //Create Listener
        TCPListener = new(Address, Port);

        Logger.Info($".ctor &1OK Configured for &4Address: {Address} / &5Port:{Port} / BufferSize: {BufferSize}");
        //Set Buffer Size and create buffer to store recieved bytes
        this.BufferSize = BufferSize;
        InBuffer = new byte[BufferSize];
    }

    //----------------------
    //  Init
    //----------------------

    public async void StartListener() {

        if (TCPListener == null) {
            Logger.Trace("&3Listener Is Null");
            return;
        }

        if (TCPListener.Server.IsBound) {
            Logger.Trace("Already Listening, return");
            return;
        }

        //Start Listening
        TCPListener.Start();
        Logger.Info($"Init TCPListener, Local Enpoint: {TCPListener.LocalEndpoint}");
        Logger.Info("Call AcceptSocketAsync");

        //Async wait, Thread Waits Here Until A Client Connection is started without blocking
        TCPSocket = await TCPListener.AcceptSocketAsync();

        //Config Socket, Object is null before accepting
        TCPSocket.SendBufferSize = BufferSize;
        TCPSocket.ReceiveBufferSize = BufferSize;
        TCPSocket.DontFragment = false;
        TCPSocket.ReceiveTimeout = 2000;
        TCPSocket.SendTimeout = 2000;
        TCPSocket.Ttl = byte.MaxValue;

        Logger.Info("Socket Accepted, Communication Established");
        Logger.Info("Call MainLoop");

        //Trigger Event
        ClientConnected?.Invoke(this,EventArgs.Empty);
        SemConnect.CheckRelease();
        AsyncRead();
    }

    //----------------------
    //  Recieve
    //----------------------

    public void StartRead() => AsyncRead();

    private void AsyncRead() {
        Logger.Info("&1Async Read");
        try {
            TCPSocket.BeginReceive(InBuffer, 0, BufferSize, SocketFlags.None, ReadCallback, this);
        }
        catch (SocketException Ex) {
            Logger.Trace($"&3Socket Exception {Ex.Message}\nDisposing...");
            TCPSocket.Disconnect(false);
            TCPSocket.Dispose();
        }
        catch (Exception Ex) {
            Logger.Trace($"&3Exception {Ex.Message}");
            throw;
        }
    }

    private void ReadCallback(IAsyncResult Ar) {
        Logger.Info("&1Async Read Callback");
        try {
            int BytesIn = TCPSocket.EndReceive(Ar);
            if (BytesIn != BufferSize) throw new("Invallid Packet, Buffer != BytesIn");
            Logger.Hex(InBuffer);

            Recieve?.Invoke(this,InBuffer);
            SemRecieve?.CheckRelease();

            //Start the Read Again
            AsyncRead();
            ClearBuffer();
        }

        catch (ObjectDisposedException) {
            Logger.Trace("&3Callback While Disposed, Doing Nothing");
        }
        catch (SocketException Ex) {
            Logger.Trace($"&3Socket Exception {Ex.Message}\nDisposing...");
            TCPSocket.Disconnect(false);
            TCPSocket.Dispose();
        }
        catch (Exception Ex) {
            Logger.Trace($"&Generic Exception {Ex.Message}\nDisposing...");
            TCPSocket.Disconnect(false);
            TCPSocket.Dispose();
        }
    }

    //----------------------
    //  Send
    //----------------------

    public bool Send(byte[] Payload) {
        try {
            int OutBytes = TCPSocket.Send(Payload, 0, BufferSize, SocketFlags.None);
            if (OutBytes != BufferSize) throw new("Did Not Send Full Buffer");
            Logger.Info("&2SendBytes Sync");
            return true;
        }
        catch (ObjectDisposedException) {
            Logger.Trace($"&3Callback While Disposed, Doing Nothing");
            return false;
        }
        catch (SocketException Ex) {
            Logger.Trace($"&3Socket Exception {Ex.Message}\nDisposing...");
            TCPSocket.Disconnect(false);
            TCPSocket.Dispose();
            return false;
        }
        catch (Exception Ex) {
            Logger.Trace($"&Generic Exception {Ex.Message}\nDisposing...");
            TCPSocket.Disconnect(false);
            TCPSocket.Dispose();
            return false;
        }
    }

    public void AsyncSend(byte[] Payload) {
        try {
            TCPSocket.BeginSend(Payload,0,BufferSize,SocketFlags.None, ASendCallback,this);
            Logger.Info("&2SendBytes ASync");
        }
        catch (ObjectDisposedException) {
            Logger.Trace("&3Callback While Disposed, Doing Nothing");
        }
        catch (SocketException Ex) {
            Logger.Trace($"&3Socket Exception {Ex.Message}\nDisposing...");
            TCPListener.Stop();
            TCPSocket.Disconnect(false);
            TCPSocket.Dispose();
        }
        catch (Exception Ex) {
            Logger.Trace($"&Generic Exception {Ex.Message}\nDisposing...");
            TCPListener.Stop();
            TCPSocket.Disconnect(false);
            TCPSocket.Dispose();
        }
    }

    private void ASendCallback(IAsyncResult Ar) {
        try {
            int BytesOut = TCPSocket.EndSend(Ar);
            Logger.Info($"&2Send Callback: {BytesOut} Bytes");
            if (BytesOut != BufferSize) throw new("Outsize does not match bytes sent");
            Sent?.Invoke(this,EventArgs.Empty);
        }
        catch (ObjectDisposedException) {
            Logger.Trace("&3Callback While Disposed, Doing Nothing");
        }
        catch (SocketException Ex) {
            Logger.Trace($"&3Socket Exception {Ex.Message}\nDisposing...");
            TCPListener.Stop();
            TCPSocket.Disconnect(false);
            TCPSocket.Dispose();
        }
        catch (Exception Ex) {
            Logger.Trace($"&Generic Exception {Ex.Message}\nDisposing...");
            TCPListener.Stop();
            TCPSocket.Disconnect(false);
            TCPSocket.Dispose();
        }
    }

    //----------------------
    //  Misc
    //----------------------

    public void ClearBuffer() => Misc.Set(ref InBuffer, (byte)0);

    public bool IsConnected() => TCPSocket.Connected;

    public bool IsBound() => TCPSocket.IsBound;

    public void Close() {
        try {
            TCPSocket.Disconnect(false);
            TCPSocket.Close();
            TCPListener.Stop();
            ClientDisconnected?.Invoke(this, EventArgs.Empty);
            SemDisconnect?.CheckRelease();
        }

        catch (SocketException Ex) {
            Logger.Trace($"&3Socket Exception When Closing {Ex.Message}");
        }
        catch (ObjectDisposedException) {
            Logger.Trace("&3Object is already Closed / Disposed");
        }
        catch (Exception Ex) {
            Logger.Trace($"&3Socket Exception When Closing {Ex.Message}");
        }

    }

    //----------------------
    //  Object Management
    //----------------------

    public void Dispose() {
        TCPSocket?.Dispose();
        SemConnect?.Dispose();
        SemDisconnect?.Dispose();
        SemRecieve?.Dispose();
        SemSent?.Dispose();
    }


    ~RobotServer() {
        Dispose();
    }
}
