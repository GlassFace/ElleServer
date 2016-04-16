using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using ClientPacket = ElleWorld.Core.Network.Packets.Client.Authentication;
using ServerPacket = ElleWorld.Core.Network.Packets.Server.Authentication;
using ElleWorld.Core.Costants.Account;
using ElleWorld.Core.Costants.Net;
using ElleWorld.Core.Miscellaneous;
using ElleWorld.Database;
using ElleWorld.Core.Network.Packets;

namespace ElleWorld.Core.Network
{
    public class WorldSession : SessionBase, IDisposable
    {
        public Realm Realm { get; set; }
        public Account Account { get; set; }
        //public GameAccount GameAccount { get; set; }
        //public Player Player { get; set; }

        public uint Challenge { get; private set; }
        public byte[] ClientSeed { get; private set; }
        public byte[] ServerSeed { get; private set; }

        public WorldSession(Socket clientSocket) : base(clientSocket) { }

        public override async void OnConnection(object sender, SocketAsyncEventArgs e)
        {
            var recievedBytes = e.BytesTransferred;

            if (!isTransferInitiated[1])
            {
                var clientToServer = "WORLD OF WARCRAFT CONNECTION - CLIENT TO SERVER\0";

                var transferInit = new Packets.Client.Authentication.TransferInitiate { Packet = new Packet(dataBuffer, 2) } as Packets.Client.Authentication.TransferInitiate;

                transferInit.Read();

                if (transferInit.Msg == clientToServer)
                {
                    State = SessionState.Initiated;

                    isTransferInitiated[1] = true;

                    e.Completed -= OnConnection;
                    e.Completed += Process;

                    Log.Message($"Initial packet transfer for Client '{GetClientInfo()}' successfully initialized.");

                    client.ReceiveAsync(e);

                    // Assign server challenge for auth digest calculations
                    Challenge = BitConverter.ToUInt32(new byte[0].GenerateRandomKey(4), 0);
                    ClientSeed = new byte[16].GenerateRandomKey(16);
                    ServerSeed = new byte[16].GenerateRandomKey(16);

                    await Send(new Packets.Server.Authentication.AuthChallenge
                    {
                        Challenge = Challenge,
                        DosChallenge = ClientSeed.Combine(ServerSeed)
                    });
                }
                else
                {
                    Log.Error($"Wrong initial packet transfer data for Client '{GetClientInfo()}'.");

                    Dispose();
                }
            }
            else
                Dispose();
        }

        public override async void Process(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                var recievedBytes = e.BytesTransferred;

                if (recievedBytes != 0)
                {
                    if (Crypt != null && Crypt.IsInitialized)
                    {
                        while (recievedBytes > 0)
                        {
                            Decrypt(dataBuffer);

                            var length = BitConverter.ToUInt16(dataBuffer, 0) + 4;
                            var packetData = new byte[length];

                            Buffer.BlockCopy(dataBuffer, 0, packetData, 0, length);

                            var packet = new Packet(dataBuffer, 4);

                            if (length > recievedBytes)
                                packetQueue.Enqueue(packet);

                            await ProcessPacket(packet);

                            recievedBytes -= length;

                            Buffer.BlockCopy(dataBuffer, length, dataBuffer, 0, recievedBytes);
                        }
                    }
                    else
                    {
                        var packet = new Packet(dataBuffer);

                        await ProcessPacket(packet);
                    }

                    client.ReceiveAsync(e);
                }
            }
            catch (Exception ex)
            {
                Dispose();

                //ExceptionLog.Write(ex);

                Log.Error(ex.Message);
            }
        }

        public override async Task ProcessPacket(Packet packet)
        {
            if (packetQueue.Count > 0)
                packetQueue.TryDequeue(out packet);

            /*if (PacketLog.Initialized)
                PacketLog.Write<ClientMessage>(packet.Header.Message, packet.Data, client.RemoteEndPoint);*/

            await PacketManager.InvokeHandler<ClientMessage>(packet, this);
        }

        public override async Task Send(ServerPacket packet)
        {
            try
            {
                packet.Write();
                packet.Packet.Finish();

                if (packet.Packet.Header != null)
                {
                    if (packet.Packet.Header.Size > 0x100)
                        packet = await Compress(packet);

                    /*if (PacketLog.Initialized)
                        PacketLog.Write<ServerMessage>(packet.Packet.Header.Message, packet.Packet.Data, client.RemoteEndPoint);*/
                }

                if (Crypt != null && Crypt.IsInitialized)
                    Encrypt(packet.Packet);

                var socketEventargs = new SocketAsyncEventArgs();

                socketEventargs.SetBuffer(packet.Packet.Data, 0, packet.Packet.Data.Length);

                if (!client.Connected)
                    return;

                socketEventargs.Completed += SendCompleted;
                socketEventargs.UserToken = packet;
                socketEventargs.RemoteEndPoint = client.RemoteEndPoint;
                socketEventargs.SocketFlags = SocketFlags.None;

                client.SendAsync(socketEventargs);
            }
            catch (Exception ex)
            {
                Dispose();

                //ExceptionLog.Write(ex);

                Log.Error(ex.Message);
            }
        }

        void SendCompleted(object sender, SocketAsyncEventArgs e)
        {
        }

        public void Dispose()
        {
            isTransferInitiated = new bool[2];

            client.Dispose();
        }
    }
}
