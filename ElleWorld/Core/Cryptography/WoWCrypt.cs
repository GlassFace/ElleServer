using System;
using System.Security.Cryptography;

namespace ElleWorld.Core.Cryptography
{

    public sealed class SARC4
    {
        byte[] s;
        byte tmp, tmp2;

        public SARC4()
        {
            s = new byte[0x100];
            tmp = 0;
            tmp2 = 0;
        }

        public void PrepareKey(byte[] key)
        {
            for (int i = 0; i < 0x100; i++)
                s[i] = (byte)i;

            for (int i = 0, j = 0; i < 0x100; i++)
            {
                j = (byte)((j + s[i] + key[i % key.Length]) % 0x100);

                var tempS = s[i];

                s[i] = s[j];
                s[j] = tempS;
            }
        }

        public void ProcessBuffer(byte[] data, int length)
        {
            for (int i = 0; i < length; i++)
            {
                tmp = (byte)((tmp + 1) % 0x100);
                tmp2 = (byte)((tmp2 + s[tmp]) % 0x100);

                var sTemp = s[tmp];

                s[tmp] = s[tmp2];
                s[tmp2] = sTemp;

                data[i] = (byte)(s[(s[tmp] + s[tmp2]) % 0x100] ^ data[i]);
            }
        }
    }

    public sealed class WoWCrypt : IDisposable
    {
        public bool IsInitialized { get; set; }

        //3.3.5a
        static readonly byte[] ServerEncryptionKey = { 0xCC, 0x98, 0xAE, 0x04, 0xE8, 0x97, 0xEA, 0xCA, 0x12, 0xDD, 0xC0, 0x93, 0x42, 0x91, 0x53, 0x57 };
        static readonly byte[] ServerDecryptionKey = { 0xC2, 0xB3, 0x72, 0x3C, 0xC6, 0xAE, 0xD9, 0xB5, 0x34, 0x3C, 0x53, 0xEE, 0x2F, 0x43, 0x67, 0xCE };

        SARC4 SARC4Encrypt, SARC4Decrypt;

        public WoWCrypt() { }

        public WoWCrypt(byte[] sessionKey)
        {
            IsInitialized = false;

            if (IsInitialized)
                throw new InvalidOperationException("PacketCrypt already initialized!");

            SARC4Encrypt = new SARC4();
            SARC4Decrypt = new SARC4();

            var decryptSHA1 = new HMACSHA1(ServerDecryptionKey);
            var encryptSHA1 = new HMACSHA1(ServerEncryptionKey);

            SARC4Encrypt.PrepareKey(encryptSHA1.ComputeHash(sessionKey));
            SARC4Decrypt.PrepareKey(decryptSHA1.ComputeHash(sessionKey));

            var PacketEncryptionDummy = new byte[0x400];
            var PacketDecryptionDummy = new byte[0x400];

            SARC4Encrypt.ProcessBuffer(PacketEncryptionDummy, PacketEncryptionDummy.Length);
            SARC4Decrypt.ProcessBuffer(PacketDecryptionDummy, PacketDecryptionDummy.Length);

            IsInitialized = true;
        }

        public void Initialize(byte[] sessionKey, byte[] clientSeed, byte[] serverSeed)
        {
            IsInitialized = false;

            if (IsInitialized)
                throw new InvalidOperationException("PacketCrypt already initialized!");

            SARC4Encrypt = new SARC4();
            SARC4Decrypt = new SARC4();

            var decryptSHA1 = new HMACSHA1(serverSeed);
            var encryptSHA1 = new HMACSHA1(clientSeed);

            SARC4Encrypt.PrepareKey(encryptSHA1.ComputeHash(sessionKey));
            SARC4Decrypt.PrepareKey(decryptSHA1.ComputeHash(sessionKey));

            var PacketEncryptionDummy = new byte[0x400];
            var PacketDecryptionDummy = new byte[0x400];

            SARC4Encrypt.ProcessBuffer(PacketEncryptionDummy, PacketEncryptionDummy.Length);
            SARC4Decrypt.ProcessBuffer(PacketDecryptionDummy, PacketDecryptionDummy.Length);

            IsInitialized = true;
        }

        public void Encrypt(byte[] data, int count)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("PacketCrypt not initialized!");

            SARC4Encrypt.ProcessBuffer(data, count);
        }

        public void Decrypt(byte[] data, int count)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("PacketCrypt not initialized!");

            SARC4Decrypt.ProcessBuffer(data, count);
        }

        public void Dispose()
        {
            IsInitialized = false;
        }
    }
}
