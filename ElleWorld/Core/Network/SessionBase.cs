using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using ElleWorld.Core.Costants.Account;
using ElleWorld.Core.Cryptography;
using ElleWorld.Core.Network.Packets.Server.Authentication;
using ElleWorld.Core.Network.Packets.Server.Net;
using ElleWorld.Core.Network.Packets.Handlers;

namespace ElleWorld.Core.Network
{
    public abstract class SessionBase
    {
        public long Id { get; set; }
        public WoWCrypt Crypt { get; set; }
        public SessionState State { get; set; }

        public Socket client;
        public ConcurrentQueue<Packet> packetQueue;
        public bool[] isTransferInitiated;
        public byte[] dataBuffer = new byte[0x2000];

        public SessionBase(Socket clientSocket)
        {
            client = clientSocket;
            packetQueue = new ConcurrentQueue<Packet>();
            isTransferInitiated = new bool[2]; // ServerInitiated, ClientInitiated
        }

        public void Accept()
        {
            var socketEventargs = new SocketAsyncEventArgs();

            socketEventargs.SetBuffer(dataBuffer, 0, dataBuffer.Length);

            socketEventargs.Completed += OnConnection;
            socketEventargs.UserToken = client;
            socketEventargs.SocketFlags = SocketFlags.None;

            client.ReceiveAsync(socketEventargs);

            if (!isTransferInitiated[0])
            {
                Send(new TransferInitiate());

                isTransferInitiated[0] = true;
            }
        }

        public abstract void OnConnection(object sender, SocketAsyncEventArgs e);
        public abstract void Process(object sender, SocketAsyncEventArgs e);

        public abstract Task ProcessPacket(Packet packet);
        public abstract Task Send(ServerPacket packet);

        public void Encrypt(Packet packet)
        {
            var totalLength = (uint)packet.Header.Size - 2;
            totalLength <<= 13;
            totalLength |= ((uint)packet.Header.Message & 0x1FFF);

            var header = BitConverter.GetBytes(totalLength);

            Crypt.Encrypt(header, 4);

            packet.Data[0] = header[0];
            packet.Data[1] = header[1];
            packet.Data[2] = header[2];
            packet.Data[3] = header[3];
        }

        public void Decrypt(byte[] data)
        {
            Crypt.Decrypt(data, 4);

            var header = BitConverter.ToUInt32(data, 0);
            var size = (ushort)(header >> 13);
            var message = (ushort)(header & 0x1FFF);

            data[0] = (byte)(0xFF & size);
            data[1] = (byte)(0xFF & (size >> 8));
            data[2] = (byte)(0xFF & message);
            data[3] = (byte)(0xFF & (message >> 8));
        }

        public string GetClientInfo()
        {
            var ipEndPoint = client.RemoteEndPoint as IPEndPoint;

            return ipEndPoint != null ? ipEndPoint.Address + ":" + ipEndPoint.Port : "";
        }

        public async Task<ServerPacket> Compress(ServerPacket packet)
        {
            await Send(new ResetCompressionContext());

            packet = NetHandler.CompressPacket(packet.Packet);

            packet.Write();
            packet.Packet.Finish();

            return packet;
        }
    }
}
