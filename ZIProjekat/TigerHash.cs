using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZIProjekat
{
    class TigerHash
    {
        SBoxes sBox;
        ulong h0;
        ulong h1;
        ulong h2;
        List<byte[]> chunksBits;

        public TigerHash()
        {
            sBox = new SBoxes();
            Initialize();
        }

        private void Initialize()
        {
            h0 = 0x0123456789ABCDEF;
            h1 = 0xFEDCBA9876543210;
            h2 = 0xF096A5B4C3D2E187;
        }

        private void PreprocessText(string text)
        {
            chunksBits = new List<byte[]>();
            Initialize();

            byte[] byteText = Encoding.Unicode.GetBytes(text);
            int length = byteText.Length;
            int numOfBits = length * 8;
            byte[] newbyteText = new byte[length + (64 - length % 64)];

            for (int i = 0; i < byteText.Length; i++)
            {
                newbyteText[i] = byteText[i];
            }

            newbyteText[byteText.Length] = 0x80; 

            for (int i = byteText.Length + 1; i < newbyteText.Length - 8; i++)
            {
                newbyteText[i] = 0; 
            }

            byte[] word64 = BitConverter.GetBytes((long)(numOfBits)); 
            for (int i = 0; i < 8; i++)
            {
                newbyteText[newbyteText.Length - 9 + i] = word64[i];
            }

            for (int i = 0; i < newbyteText.Length / 64; i++)
            {
                byte[] chunk = new byte[64];
                Array.Copy(newbyteText, i * 64, chunk, 0, 64);
                chunksBits.Add(chunk);
            }
        }

        public byte[] ProcessText(string text)
        {
            PreprocessText(text);

            foreach (byte[] chunk in chunksBits)
            {
                ulong a = h0;
                ulong b = h1;
                ulong c = h2;

                for (int i = 0; i < 4; i++)
                {
                    ulong[] v = new ulong[8];

                    for (int j = 0; j < 8; j++)
                    {
                        v[j] = BitConverter.ToUInt64(chunk, j * 8);
                        c = c ^ v[j];
                        a = a - (sBox.S0[(byte)c >> 0] ^ sBox.S1[(byte)c >> 16] ^ sBox.S2[(byte)c >> 32] ^ sBox.S3[(byte)c >> 48]);
                        b = b + (sBox.S3[(byte)c >> 8] ^ sBox.S2[(byte)c >> 24] ^ sBox.S1[(byte)c >> 40] ^ sBox.S3[(byte)c >> 56]);
                        b = b * (ulong)(i + 1);

                    }

                    if (i == 0)
                    {
                        v[0] = v[0] - (v[7] ^ 0xA5A5A5A5A5A5A5A5);
                        v[1] = v[1] ^ v[0];
                        v[2] = v[2] + v[1];
                        v[3] = v[3] - (v[2] ^ ((v[1] ^ 0xFFFFFFFFFFFFFFFF) << 19));
                        v[4] = v[4] ^ v[3];
                        v[5] = v[5] + v[4];
                        v[6] = v[6] - (v[5] ^ ((v[4] ^ 0xFFFFFFFFFFFFFFFF) >> 23));
                        v[7] = v[7] ^ v[6];
                    }
                    else if (i == 1)
                    {
                        v[0] = v[0] + v[7];
                        v[1] = v[1] - (v[0] ^ ((v[0] ^ 0xFFFFFFFFFFFFFFFF) << 19));
                        v[2] = v[2] ^ v[1];
                        v[3] = v[3] + v[2];
                        v[4] = v[4] - (v[3] ^ ((v[2] ^ 0xFFFFFFFFFFFFFFFF) >> 23));
                        v[5] = v[5] ^ v[4];
                        v[6] = v[6] + v[5];
                        v[7] = v[7] - (v[6] ^ 0x0123456789ABCDEF);
                    }
                }

                h0 = h0 + a;
                h1 = h1 + b;
                h2 = h2 + c;
            }

            byte[] d = new byte[24];
            Array.Copy(BitConverter.GetBytes(h0), 0, d, 0, 8);
            Array.Copy(BitConverter.GetBytes(h1), 0, d, 8, 8);
            Array.Copy(BitConverter.GetBytes(h2), 0, d, 16, 8);

            return d;
        }
    }
}
