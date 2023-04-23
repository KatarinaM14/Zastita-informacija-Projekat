using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace ZIProjekat
{
    class Knapsack
    {
        
        private long[] P; 
        private long n; 
        private long m;
        private long[] J; 
        private long im; 

        public bool blockMode;
        private int counter;
        private byte[] nonce;

        private TigerHash tigerHash;

        public Knapsack()
        {
           
            P = new long[16];
            J = new long[16];
            counter = 0;
            nonce = new byte[2];

            tigerHash = new TigerHash();
        }

      

        public List<string> EncryptKnapsack(string filePath, List<string> textForCoding)
        {
            string[] splitedPath = filePath.Split('\\');
            string fileName = splitedPath[splitedPath.Length - 1].Replace(splitedPath[splitedPath.Length - 1], "Key.txt");
            this.GenerateKey(fileName);

          
            List<string> encrypted = new List<string>();


            string line = "";
            counter = 0;
            foreach (var text in textForCoding)
            {
             line += text + "\n";
            string plaintext = "";
            byte[] charsInBytes = Encoding.Unicode.GetBytes(text.ToCharArray());
            BitArray B;
            for (int c = 0; c < charsInBytes.Length - 1; c += 2)
            {
                long C = 0;
                if (!blockMode)
                {
                    int length;
                    if (charsInBytes[c + 1] == (byte)0)
                            length = 8;
                    else
                            length = 16;
                    B = new BitArray(new byte[] { charsInBytes[c], charsInBytes[c + 1] });
                    for (int i = length - 1; i >= 0; i--)
                    {
                            int b;
                            if (B[i])
                            {
                                b = 1;
                            }
                            else
                            {
                                b = 0;
                            }
                       
                       
                        C += J[i] * b;
                    }
                    plaintext += C.ToString() + " ";
                }
                else
                {
                    BitArray nonceBits = new BitArray(nonce);
                    BitArray counterInBits = new BitArray(new byte[] { (byte)counter, (byte)0 });
                    nonceBits.Xor(counterInBits);
                    for (int i = 15; i >= 0; i--)
                    {
                            int b;
                            if(nonceBits[i])
                            {
                                b = 1;
                            }
                            else
                            {
                                b = 0;
                            }
                           
                        C += J[i] * b;
                    }
                    BitArray decodedChar;
                    byte[] CinBytes = BitConverter.GetBytes(C);
                    decodedChar = new BitArray(CinBytes);
                    B = new BitArray(new byte[] { charsInBytes[c], charsInBytes[c + 1], 0, 0, 0, 0, 0, 0 });
                    decodedChar.Xor(B);
                    byte[] encryptedChar = new byte[8];
                    decodedChar.CopyTo(encryptedChar, 0);

                    plaintext += BitConverter.ToInt32(encryptedChar, 0) + " ";

                    counter++;
                }
            }
            encrypted.Add(plaintext);
            }

            byte[] hashedText = tigerHash.ProcessText(line);
            string pom = Encoding.Unicode.GetString(hashedText);
            encrypted.Add(pom);

            return encrypted;
        }

        public List<string> DecryptKnapsack(string file, out bool sameHashes, List<string> plaintextLines)
        {
            sameHashes = false;

            string[] splitedPath = file.Split('\\');
            string fileName = splitedPath[splitedPath.Length - 1];
            this.GetKey(fileName);
           

            long TC;
            string plaintext = "";
            List<string> decrypted = new List<string>();
            string line = "";
            counter = 0;

            for (int j = 0; j < plaintextLines.Count - 1; j++)
            {
                plaintext = "";
                string[] chars = plaintextLines[j].Split(' ');
                foreach (string C in chars)
                {
                    if (C == "")
                        continue;
                    if (!blockMode)
                    {
                        TC = 0;
                        TC = (Int32.Parse(C) * im) % n;
                        if (TC < 0)
                            TC += n;

                        List<bool> f;
                        f = new List<bool>();
                        f = this.Factors(TC);
                        BitArray bits = new BitArray(Enumerable.Reverse(f).ToArray());
                        byte[] b = new byte[2];
                        bits.CopyTo(b, 0);
                        char c = BitConverter.ToChar(b, 0);
                        plaintext += c;
                    }
                    else
                    {
                        long c = 0;
                        BitArray nonceBits = new BitArray(nonce);
                        BitArray counterInBits = new BitArray(new byte[] { (byte)counter, (byte)0 });
                        nonceBits.Xor(counterInBits);
                        for (int i = 15; i >= 0; i--)
                        {
                            int b;
                            if(nonceBits[i])
                            {
                                b = 1;
                            }
                            else
                            {
                                b = 0;
                            }
                            
                           
                            c += J[i] * b;
                        }
                        BitArray encryptedNonce;
                        byte[] encryptedNonceinBytes = BitConverter.GetBytes(c);
                        encryptedNonce = new BitArray(encryptedNonceinBytes);
                        int encryptedChar = Int32.Parse(C);
                        byte[] encryptedCharInBytes = BitConverter.GetBytes(encryptedChar);
                        BitArray B;
                        B = new BitArray(new byte[] { encryptedCharInBytes[0], encryptedCharInBytes[1], 0, 0, 0, 0, 0, 0 });
                        encryptedNonce.Xor(B);
                        byte[] decryptedChar = new byte[8];
                        encryptedNonce.CopyTo(decryptedChar, 0);

                        plaintext += BitConverter.ToChar(decryptedChar, 0);

                        counter++;
                    }
                }
                line += plaintext + "\n";
                decrypted.Add(plaintext);
            }
            byte[] hashedText = tigerHash.ProcessText(line);
            string pom = Encoding.Unicode.GetString(hashedText);
            string hashedTextFromFile = plaintextLines[plaintextLines.Count - 1];
            if (pom == hashedTextFromFile)
                sameHashes = true;

            return decrypted;
        }

        public void GenerateKey(string file)
        {
            int pom = 0;
            Random random = new Random();
            for (int i = 0; i < 16; i++)
            {
                pom += random.Next(pom + 1, pom + 3); 
                P[i] = pom;
            }

            n = random.Next(pom, pom + 3);

            pom = random.Next(1, (int)n / 2); 
            while (this.GCD(n, pom) != 1)
                pom = random.Next(1, (int)n / 2);
            m = pom;

            for (int i = 0; i < 16; i++)
            {
                J[i] = (P[i] * m) % n;
                if (J[i] < 0)
                    J[i] += n;
            }

            im = this.InverseMod(m, n);

            random.NextBytes(nonce);

            if (!Directory.Exists("Keys"))
                Directory.CreateDirectory("Keys");

            string filePath = "Keys\\" + file;

            using (StreamWriter sw = new StreamWriter(new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite)))
            {

                foreach (int i in J)
                {
                    sw.Write(i + " ");
                }
                sw.WriteLine();
                foreach (int i in P)
                {
                    sw.Write(i + " ");
                }
                sw.WriteLine();
                sw.WriteLine(n);
                sw.WriteLine(im);
                sw.Write(String.Concat(nonce.Select(x => x.ToString("X2")).ToArray()));
            }
        }

        public void GetKey(string file)
        {
            if (file.CompareTo("Encrypted.knapsack") == 0)
            {
                file = file.Replace("Encrypted.knapsack", "Key.txt");
            }
            else
            {
                this.GenerateKey(file);
                return;
            }

            string filePath = "Keys\\" + file;

            using (StreamReader sr = new StreamReader(new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite)))
            {
                string readLine = sr.ReadLine();
                int i = 0;
                foreach (var line in readLine.Split(' '))
                {
                    if (line == "")
                        continue;
                    Int64.TryParse(line, out J[i++]);
                }
                readLine = sr.ReadLine();
                i = 0;
                foreach (var line in readLine.Split(' '))
                {
                    if (line == "")
                        continue;
                    Int64.TryParse(line, out P[i++]);
                }
                Int64.TryParse(sr.ReadLine(), out n);
                Int64.TryParse(sr.ReadLine(), out im);
                string hex = sr.ReadLine();
                nonce = Enumerable.Range(0, hex.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(hex.Substring(x, 2), 16)).ToArray();
            }
        }

        private long GCD(long a, long b)
        {
            long pom;

            while (b != 0)
            {
                pom = a;
                a = b;
                b = pom % b;
            }

            return a;
        }

        private long InverseMod(long m, long n)
        {
            long n0 = n;
            long j = 0;
            long i = 1;

            if (n == 1)
                return 0;

            while (m > 1)
            {
                long q = m / n;
                long t = n;
                n = m % n;
                m = t;
                t = j;

                j = i - q * j;
                i = t;
            }

            if (i < 0)
                i += n0;

            return i;
        }

        private List<bool> Factors(long number)
        {
            List<bool> f = new List<bool>();

            for (int i = 15; i >= 0; i--)
            {
                if (number - P[i] >= 0)
                {
                    f.Add(true);
                    number -= P[i];
                }
                else
                    f.Add(false);
            }

            return f;
        }

    }
}
