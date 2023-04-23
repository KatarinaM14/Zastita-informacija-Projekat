using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZIProjekat
{
    class RC6
    {
        
        private const int r = 20; 
        private static uint[] s = new uint[2 * r + 4];  
        private const int w = 32; 
      
        
        public RC6()
        {
           
        }

        private static uint ShiftRight(uint v, int shift)
        {
            return (v >> shift) | (v << (w - shift));
        }
     
        private static uint ShiftLeft(uint v, int shift)
        {
            return (v << shift) | (v >> (w - shift));
        }
        private static uint[] BytesToWord(byte[] bytes)
        {
            uint[] b = new uint[bytes.Length / 4];
            for (int i = 0; i < b.Length; i++)
            {
                b[i] = BitConverter.ToUInt32(bytes, i * 4);
            }
            return b;
        }
      
        public void GenerateKey(byte[] key)
        {
          
            int c = 0;
            int i, j;
            int t = 2 * r + 4;
           
            c = key.Length / 4;
           
            uint[] L = BytesToWord(key);
            
            s[0] = 0xB7E15163;
            for (i = 1; i < t; i++)
                s[i] = s[i - 1] + 0x9E3779B9;
            uint A = 0; 
            uint B = 0;
            i = j = 0;
            int V = 3 * Math.Max(c, t);  
            for (int m = 1; m <= V; m++)
            {
                A = s[i] = ShiftLeft((s[i] + A + B), 3);
                B = L[j] = ShiftLeft((L[j] + A + B), (int)(A + B)); 
                i = (i + 1) % t;
                j = (j + 1) % c;
            }
        }

        
        private static byte[] ArrayBytes(uint[] uints, int Long)
        {
            byte[] arrayBytes = new byte[Long * 4];
            for (int i = 0; i < Long; i++)
            {
                byte[] pom = BitConverter.GetBytes(uints[i]);
                pom.CopyTo(arrayBytes, i * 4);
            }
            return arrayBytes;
        }
        public byte[] EncryptRc6(byte[] byteText)
        {
            uint A, B, C, D;
           
            int i = byteText.Length;
            while (i % 16 != 0)
                i++;
         
            byte[] text = new byte[i];
      
            byteText.CopyTo(text, 0);
            byte[] cipher = new byte[i];
           
            for (i = 0; i < text.Length; i = i + 16)
            {
            
                A = BitConverter.ToUInt32(text, i);
                B = BitConverter.ToUInt32(text, i + 4);
                C = BitConverter.ToUInt32(text, i + 8);
                D = BitConverter.ToUInt32(text, i + 12);
            
                B = B + s[0];
                D = D + s[1];
                for (int j = 1; j <= r; j++)
                {
                    uint t = ShiftLeft((B * (2 * B + 1)), (int)(Math.Log(w, 2)));
                    uint u = ShiftLeft((D * (2 * D + 1)), (int)(Math.Log(w, 2)));
                    A = (ShiftLeft((A ^ t), (int)u)) + s[j * 2];
                    C = (ShiftLeft((C ^ u), (int)t)) + s[j * 2 + 1];
                    uint temp = A;
                    A = B;
                    B = C;
                    C = D;
                    D = temp;
                }
                A = A + s[2 * r + 2];
                C = C + s[2 * r + 3];
             
                uint[] p = new uint[4] { A, B, C, D };
                byte[] block = ArrayBytes(p, 4);
               
                block.CopyTo(cipher, i);
            }
            return cipher;
        }
        public byte[] DecryptRc6(byte[] text)
        {
            uint A, B, C, D;
            int i;
            byte[] plainText = new byte[text.Length];
         
            for (i = 0; i < text.Length; i = i + 16)
            {
           
                A = BitConverter.ToUInt32(text, i);
                B = BitConverter.ToUInt32(text, i + 4);
                C = BitConverter.ToUInt32(text, i + 8);
                D = BitConverter.ToUInt32(text, i + 12);
               
                C = C - s[2 * r + 3];
                A = A - s[2 * r + 2];
                for (int j = r; j >= 1; j--)
                {
                    uint pom = D;
                    D = C;
                    C = B;
                    B = A;
                    A = pom;
                    uint u = ShiftLeft((D * (2 * D + 1)), (int)Math.Log(w, 2));
                    uint t = ShiftLeft((B * (2 * B + 1)), (int)Math.Log(w, 2));
                    C = ShiftRight((C - s[2 * j + 1]), (int)t) ^ u;
                    A = ShiftRight((A - s[2 * j]), (int)u) ^ t;
                }
                D = D - s[1];
                B = B - s[0];
             
                uint[] p = new uint[4] { A, B, C, D };
                byte[] block = ArrayBytes(p, 4);
              
                block.CopyTo(plainText, i);
            }
            return plainText;
        }
       
    }
}
