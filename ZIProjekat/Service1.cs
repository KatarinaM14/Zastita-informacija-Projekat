using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace ZIProjekat
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class Service1 : IService1
    {
        CompositeType data = new CompositeType();

        public void SetKnapsack(bool k)
        {
            data.Knapsack = k;
        }

        public void SetBifid(bool b)
        {
            data.Bifid = b;
        }

        public void SetRc6(bool r)
        {
            data.Rc6 = r;
        }
        public void SetCTR(bool r)
        {
            data.CTR = r;
        }
        public void SetBlockMode(bool b)
        {
            data.BlockMode = b;
        }
        public void SetParallel(bool p)
        {
            data.Parallel = p;
        }
       
        public void SetOutputDirectory(string dir)
        {
            data.OutputDirectory = dir;
        }
        public void SetKey(string k)
        {
            data.Key = k;
        }

        public void EncodingFile(string file)
        {
        
            if (file.EndsWith(".txt"))
                EncryptTextFile(file, data.OutputDirectory);
            else if (file.EndsWith(".bmp"))
                EncryptBmpFile(file, data.OutputDirectory);
        }

        public void EncryptTextFile(string file, string outputDirectory)
        {
           
            if (data.Rc6)
            {

                string text = ReadTextFile(file);
                string outputFileName; 

                if (!data.Parallel)
                    outputFileName = data.OutputDirectory + "\\Encrypted.rc6";
                else
                    outputFileName = file.Replace(".txt", "Encrypted.rc6");

               
                RC6 rc6 = new RC6();
                rc6.GenerateKey(Encoding.UTF8.GetBytes(data.Key));
                byte[] byteText = Encoding.UTF8.GetBytes(text);
               
                byte[] encoded = rc6.EncryptRc6(byteText);

               
                WriteInBinaryFile(outputFileName, encoded);

               
            }
            else if (data.Bifid)
            {
             
                List<string> text = ReadFromFile(file);
                string outputFileName = outputDirectory + "\\Encrypted.bifid";

                List<string> encryptedFileLines = new List<string>();
                Bifid cryptoAlgorithmBifid = new Bifid();
                encryptedFileLines.AddRange(cryptoAlgorithmBifid.EncryptBifid(file, text));

                WriteIntoDestinationFile(encryptedFileLines, outputFileName);


            }
            else if (data.Knapsack)
            {
                string outputFileName = outputDirectory + "\\Encrypted.knapsack";

                List<string> encryptedFileLines = new List<string>();
                Knapsack cryptoAlgorithmKnapsack = new Knapsack();
                cryptoAlgorithmKnapsack.blockMode = data.BlockMode;
                List<string> text = ReadFromFile(file);
                encryptedFileLines.AddRange(cryptoAlgorithmKnapsack.EncryptKnapsack(file, text));

                WriteIntoDestinationFile(encryptedFileLines, outputFileName);

              
            }
            else if (data.CTR)
            {
                
                string text = ReadTextFile(file);
                string outputFileName;
                if (!data.Parallel)
                    outputFileName = data.OutputDirectory + "\\Encrypted.ctr";
                else
                    outputFileName = file.Replace(".txt", "Encrypted.ctr");

                
                
                CTR ctr = new CTR();
               
                byte[] encodedText = ctr.EncodeStream(text, Encoding.UTF8.GetBytes(data.Key));

               
                WriteInBinaryFile(outputFileName, encodedText);

              
            }
          

        }
        public bool DecryptTextFile(string file)
        {
            if (file.Contains(".bmp"))
            {
                DecryptBmpFile(file, data.OutputDirectory);
                
            }
            else if (data.Rc6)
            {
                string outputFileName;
                if (!data.Parallel)
                    outputFileName = data.OutputDirectory + "\\Decrypted.txt";
                else
                    outputFileName = file.Replace("Encrypted.rc6", "Decrypted.txt");

             
                RC6 rc6 = new RC6();
                rc6.GenerateKey(Encoding.UTF8.GetBytes(data.Key));
                byte[] allBytes = ReadBinaryFile(file);
                string decodedText = Encoding.Default.GetString(rc6.DecryptRc6(allBytes));

                WriteInTextFile(outputFileName, decodedText);
               
            }
            else if (data.Bifid)
            {
                string outputFileName = data.OutputDirectory + "\\Decrypted.txt";
                
                List<string> decrytedFileLines;
                Bifid cryptoAlgorithmBifid = new Bifid();
               
                List<string> text = ReadFromFile(file);
                decrytedFileLines = cryptoAlgorithmBifid.DecryptBifid(file, text);

                

                WriteIntoDestinationFile(decrytedFileLines, outputFileName);

              
            }
            else if (data.Knapsack)
            {
                string outputFileName = data.OutputDirectory + "\\Decrypted.txt";
                bool sameHashes;
                List<string> decrytedFileLines;
                Knapsack cryptoAlgorithmKnapsack = new Knapsack();
                cryptoAlgorithmKnapsack.blockMode = data.BlockMode;
            
                List<string> text = ReadFromFile(file);
                decrytedFileLines = cryptoAlgorithmKnapsack.DecryptKnapsack(file, out sameHashes, text);


                WriteIntoDestinationFile(decrytedFileLines, outputFileName);

                return sameHashes;
            }else if(data.CTR)
            {
                string outputFileName;

                if (!data.Parallel)
                    outputFileName = data.OutputDirectory + "\\Decrypted.txt";
                else
                    outputFileName = file.Replace("Encrypted.ctr", "Decrypted.txt");

                byte[] text = ReadBinaryFile(file);

                CTR ctr = new CTR();
              
                string decodedText = ctr.DecodeStream(text, Encoding.UTF8.GetBytes(data.Key));

              
                WriteInTextFile(outputFileName, decodedText);
            }

        
           
            return false;
        }
        public string ReadTextFile(string file)
        {
            
            string text = File.ReadAllText(file);
            return text;
        }
        public byte[] ReadBinaryFile(string file)
        {
            
            long fileLength = new FileInfo(file).Length;
            BinaryReader br = new BinaryReader(File.OpenRead(file));
            byte[] all_bytes = br.ReadBytes((int)fileLength);
            return all_bytes;

        }
        public void WriteInTextFile(string file, string text)
        {
            
            if (!File.Exists(file))
            {
                using (StreamWriter sw = File.CreateText(file))
                {
                    sw.Write(text);
                }
            }
           
        }
        public List<string> ReadFromFile(string file)
        {
            List<string> lines = new List<string>();

            using (StreamReader sr = new StreamReader(new FileStream(file, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite)))
            {
                string line = sr.ReadLine();
                while (!String.IsNullOrEmpty(line))
                {
                    lines.Add(line);
                    line = sr.ReadLine();
                }
            }

            return lines;
        }
        public void WriteIntoDestinationFile(List<string> fileLines, string fileName)
        {
            using (StreamWriter sw = new StreamWriter(new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite), Encoding.Unicode))
            {
                foreach (var item in fileLines)
                {
                    sw.WriteLine(item);
                }
            }
        }
        public void WriteInBinaryFile(string file, byte[] text)
        {
          
            File.WriteAllBytes(file, text);
        }
        
      
        public void EncryptBmpFile(string file, string outputDirectory)
        {
            byte[] encryptBytes = File.ReadAllBytes(file);
            byte[] z = new byte[54];
            Array.Copy(encryptBytes, z, 54);

          

           
            RC6 rc6 = new RC6();
            rc6.GenerateKey(Encoding.UTF8.GetBytes(data.Key));  
            byte[] data1 = rc6.EncryptRc6(encryptBytes.Skip(54).ToArray());
      
            byte[] encodedBytes = new byte[z.Length + data1.Length];
            Array.Copy(z, encodedBytes, z.Length);
            Array.Copy(data1, 0, encodedBytes, z.Length, data1.Length); 

            using (MemoryStream stream = new MemoryStream(encodedBytes))
            {
                Bitmap img = new Bitmap(stream);
                string outputFile = file.Remove(file.Length - 4, 4) + "Encript.bmp"; 
                int endInd = outputFile.LastIndexOf('\\');

                string newFileName = file;
                if (endInd >= 0)
                {
                    newFileName = outputDirectory + "\\" + outputFile.Substring(endInd + 1);
                }
                img.Save(newFileName);
            }

        }
        public void DecryptBmpFile(string file, string outputDirectory)
        {
            byte[] decryptBytes = File.ReadAllBytes(file);
            byte[] z = new byte[54];
            Array.Copy(decryptBytes, z, 54);

          
           
            RC6 rc6 = new RC6();
            rc6.GenerateKey(Encoding.UTF8.GetBytes(data.Key));
            byte[] data1 = rc6.DecryptRc6(decryptBytes.Skip(54).ToArray());

            
            byte[] decodedBytes = new byte[z.Length + data1.Length];
            Array.Copy(z, decodedBytes, z.Length); 
            Array.Copy(data1, 0, decodedBytes, z.Length, data1.Length); 

            using (MemoryStream stream = new MemoryStream(decodedBytes))
            {
                Bitmap img = new Bitmap(stream);
                

                string outputFile = file.Remove(file.Length - 11, 11) + "Decript.bmp"; 
                int endInd = outputFile.LastIndexOf('\\');

                string newFileName = file;
                if (endInd >= 0)
                {
                    newFileName = outputDirectory + "\\" + outputFile.Substring(endInd + 1);
                }

                img.Save(newFileName);
               
            }

        }
        public void ParallelReading(List<string> lines, int numberOfThreads)
        {
            int threads = numberOfThreads;

            SetParallel(true);
            ParallelOptions options = new ParallelOptions();
            options.MaxDegreeOfParallelism = threads;

            Parallel.ForEach(lines, options, line => {

                EncodingFile(line);
         
            });
            SetParallel(false);
        }
       


        public void ParallelWriting(List<string> lines, int numberOfThreads)
        {
            int threads = numberOfThreads;

            SetParallel(true);
            ParallelOptions options = new ParallelOptions();
            options.MaxDegreeOfParallelism = threads;

            Parallel.ForEach(lines, options, line =>
            {
                DecryptTextFile(line);
            });
            SetParallel(false);
        }
        public void ParallelCoding(List<string> files, int numberOfThreads)
        {
           
            int[] weights = { 2, 5, 3 };
            int[] values = { 10, 40, 30 };
            int capacity = files.Count;
            int i, j;

            int[,] dp = new int[files.Count + 1, capacity + 1];

            for (i = 0; i <= files.Count; i++)
            {
                for (j = 0; j <= capacity; j++)
                {
                    if (i == 0 || j == 0)
                        dp[i, j] = 0;
                    else if (weights[i - 1] <= j)
                        dp[i, j] = Math.Max(values[i - 1] + dp[i - 1, j - weights[i - 1]], dp[i - 1, j]);
                    else
                        dp[i, j] = dp[i - 1, j];
                }
            }

            
            List<string> selectedFiles = new List<string>();
            i = files.Count; j = capacity;
            while (i > 0 && j > 0)
            {
                if (dp[i, j] != dp[i - 1, j])
                {
                    selectedFiles.Add(files[i - 1]);
                    j -= weights[i - 1];
                }
                i--;
            }

            ParallelOptions options = new ParallelOptions();
            options.MaxDegreeOfParallelism = numberOfThreads;

            Parallel.ForEach(selectedFiles,options, (file) => {
                byte[] fileBytes = File.ReadAllBytes(file);
                string encodedFile = Convert.ToBase64String(fileBytes);
                File.WriteAllText(file + ".base64", encodedFile);
            });

        }

    }
}
