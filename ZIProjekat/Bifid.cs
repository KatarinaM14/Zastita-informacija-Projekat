using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ZIProjekat
{
    class Bifid
    {
     

        private char[,] keySquare = new char[5, 5];
        private int period = 5;
        private int indexI; 
        private int indexJ;
      

        public Bifid()
        {
           
        }



        private void EncodeStepOne(string plaintext, out string[] val)
        {
            val = new string[2]; 
            bool found = false;

            foreach (var text in plaintext)
            {
                if (text == ' ')
                {
                    val[0] += " ";
                    val[1] += " ";
                    continue;
                }

                if (!Char.IsLetter(text)) 
                    continue;

                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (text == 'j')
                        {
                            val[0] += (indexI + 1).ToString();
                            val[1] += (indexJ + 1).ToString();

                            found = true;
                            break;
                        }
                        else if (keySquare[i, j] == text)
                        {
                            val[0] += (i + 1).ToString();
                            val[1] += (j + 1).ToString();

                            found = true;
                            break;
                        }
                    }
                    if (found)
                    {
                        found = false;
                        break;
                    }
                }
            }
        }

        private void EncodeStepTwo(string[] val)
        {
            string[] trimmedValues = new string[2];
            trimmedValues[0] = val[0].Replace(" ", "");
            trimmedValues[1] = val[1].Replace(" ", "");

            int length = trimmedValues[0].Length;
            int temp = length;
            val[0] = "";
            val[1] = "";
            int i = 0;
            while ((length - period) >= 0)
            {
                val[0] += trimmedValues[0].Substring(period * i, period);
                val[0] += " ";
                val[1] += trimmedValues[1].Substring(period * i, period);
                val[1] += " ";
                i++;
                length -= period;
            }
            val[0] += trimmedValues[0].Substring(temp - length);
            val[1] += trimmedValues[1].Substring(temp - length);
        }

        private void EncodeStepThree(string[] val, out string newVal)
        {
            newVal = "";
            string[] r = val[0].Split(' ');
            string[] k = val[1].Split(' ');

            for (int i = 0; i < r.Length; i++)
                newVal += r[i] + k[i] + " ";
        }

        private void EncodeStepFour(string newVal, out string plaintext)
        {
            plaintext = "";

            for (int i = 0; i < newVal.Length - 2; i += 2)
            {
                if (newVal[i] == ' ')
                {
                    i++;
                }
                plaintext += keySquare[Int32.Parse(newVal[i].ToString()) - 1, Int32.Parse(newVal[i + 1].ToString()) - 1];
            }
        }

        private void DecodeStepOne(string plaintext, out string newVal)
        {
            newVal = "";
            int length = plaintext.Length;
            int pom = length;
            int i = 0;
            while ((length - period) >= 0)
            {
                newVal += plaintext.Substring(period * i, period);
                newVal += " ";
                i++;
                length -= period;
            }
            newVal += plaintext.Substring(pom - length);
        }

        private void DecodeStepTwo(string newVal, out string val)
        {
            val = "";
            bool found = false;

            for (int p = 0; p < newVal.Length; p++)
            {
                if (newVal[p] == ' ')
                {
                    continue;
                }
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (newVal[p] == keySquare[i, j])
                        {
                            val += (i + 1).ToString() + (j + 1).ToString();
                            found = true;
                            break;
                        }
                    }
                    if (found)
                    {
                        found = false;
                        break;
                    }
                }
            }
        }

        private void DecodeStepThree(string val, out string[] values)
        {
            values = new string[2];
            int length = val.Length;
            int pom = length;
            int i = 0;

            while ((length - period * 2) >= 0)
            {
                values[0] += val.Substring(period * i, period);
                values[1] += val.Substring(period * (i + 1), period);
                i += 2;
                length -= period * 2;
            }
            values[0] += val.Substring(pom - length, length / 2);
            values[1] += val.Substring(pom - length / 2, length / 2);
        }

        private void DecodeStepFour(string[] values, out string plaintext)
        {
            plaintext = "";

            for (int i = 0; i < values[0].Length; i++)
            {
                int r = Int32.Parse(values[0][i].ToString()) - 1;
                int k = Int32.Parse(values[1][i].ToString()) - 1;

                plaintext += keySquare[r, k];
            }
        }

        public List<string> EncryptBifid(string file, List<string> plaintextLines)
        {
            string[] splitedPath = file.Split('\\');
            string fileName = splitedPath[splitedPath.Length - 1].Replace(splitedPath[splitedPath.Length - 1], "Key.txt");
            this.GetKey(fileName);

            string[] values;
            string allVal;
            List<string> encryptedLines = new List<string>();
           

            foreach (var item in plaintextLines)
            {
                string pom;     
                this.EncodeStepOne(item.ToLower(), out values);
                this.EncodeStepTwo(values);
                this.EncodeStepThree(values, out allVal);
                this.EncodeStepFour(allVal, out pom);
                encryptedLines.Add(pom);
            }

            return encryptedLines;
        }

        public List<string> DecryptBifid(string file,  List<string> plaintextLines)
        {
           
            string[] splitedPath = file.Split('\\');
            string fileName = splitedPath[splitedPath.Length - 1].Replace(splitedPath[splitedPath.Length - 1], "Key.txt");
            this.GetKey(fileName);

            string temp1;
            string temp2;
            string[] values;
            string plaintext;
            List<string> decryptedLines = new List<string>();

            for (int i = 0; i < plaintextLines.Count ; i++)
            {
                this.DecodeStepOne(plaintextLines[i], out temp1);
                this.DecodeStepTwo(temp1, out temp2);
                this.DecodeStepThree(temp2, out values);
                this.DecodeStepFour(values, out plaintext);
                decryptedLines.Add(plaintext);
            }

            return decryptedLines;
        }

        public void GenerateAndSaveKey(string file)
        {
            if (!Directory.Exists("Bifid-Key"))
                Directory.CreateDirectory("Bifid-Key");

            string filePath = "Bifid-Key\\" + file;

            using (StreamWriter sw = new StreamWriter(new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite)))
            {
                Random rand = new Random();
                List<char> showedLetters = new List<char>();
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        int charNumber = rand.Next(97, 123);
                        char letter = Convert.ToChar(charNumber);
                        if (letter == 'i' && !showedLetters.Contains(letter))
                        {
                            indexI = i;
                            indexJ = j;
                        }
                        if (letter == 'j' || showedLetters.Contains(letter))
                        {
                            j--;
                            continue;
                        }

                        showedLetters.Add(letter);
                        keySquare[i, j] = letter;

                        sw.Write(letter);
                    }

                    sw.WriteLine();
                }
            }
        }

        public void GetKey(string file)
        {
            if (file.Contains("Encrypted"))
                file = file.Replace("Encrypted", "");
            if (file.Contains("Decrypted"))
                file = file.Replace("Decrypted", "");

            string filePath = "Bifid-Key\\" + file;


            if (!File.Exists(filePath))
            {
                this.GenerateAndSaveKey(file);
                return;
            }

            using (StreamReader sr = new StreamReader(new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite)))
            {
                string readLine = sr.ReadLine();
                int i = 0;
                int j = 0;

                while (i < 5)
                {
                    foreach (var item in readLine)
                    {
                        if (item == 'i')
                        {
                            indexI = i;
                            indexJ = j;
                        }
                        keySquare[i, j++] = item;
                    }
                    i++;
                    j = 0;
                    readLine = sr.ReadLine();
                }
            }
        }

    }
}
