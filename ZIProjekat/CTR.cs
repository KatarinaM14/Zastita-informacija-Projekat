using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZIProjekat
{
    class CTR
    {
   
        private bool ctrModeActive;
        private byte[] initialCounter;

        public bool CtrModeActive
        {
            get { return ctrModeActive; }
            set { ctrModeActive = value; }
        }
        public CTR()
        {
            initialCounter = new byte[1];
        }

        private byte[] EncryptWithCRTMode(byte[] data, RC6 rc6)
        {
            
            byte[] r = new byte[data.Length];

            byte[] counter = new byte[1];
            counter[0] = initialCounter[0];
            for (int i = 0; i < data.Length;i++)
            {
                byte[] counterRes = rc6.EncryptRc6(counter);
                r[i] = (byte)(data[i] ^ counterRes[0]);
                counter[0] = (byte)((counter[0] + 1) % 256);
            }

            return r;
        }
        public byte[] EncodeStream(string plainText, byte[] key)
        {
           
            RC6 rc6 = new RC6();
            rc6.GenerateKey(key);
            byte[] plainBytesDef = Encoding.Default.GetBytes(plainText);
            byte[] plainBytes = Encoding.Convert(Encoding.Default, Encoding.ASCII, plainBytesDef);
     
            byte[] r = EncryptWithCRTMode(plainBytes, rc6);

            return r;
        }

        public string DecodeStream(byte[] cypherText, byte[] key)
        {
          
            RC6 rc6 = new RC6();
            rc6.GenerateKey(key);
         
            byte[] r = EncryptWithCRTMode(cypherText, rc6);

            string plainText = Encoding.Default.GetString(r);
            return plainText;
        }
       
        
    }
}


