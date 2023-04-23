using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ZIProjekat
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
      

        [OperationContract]
        void SetOutputDirectory(string dir);

        [OperationContract]
        void SetKey(string k);

        [OperationContract]
        void EncodingFile(string path);

        [OperationContract]
        void EncryptTextFile(string fullFileName, string outputDirectory);

        [OperationContract]
        bool DecryptTextFile(string fullFileName);

        [OperationContract]
        string ReadTextFile(string file);

        [OperationContract]
        byte[] ReadBinaryFile(string file);

        [OperationContract]
        void WriteInTextFile(string file, string text);

        [OperationContract]
        List<string> ReadFromFile(string file);

        [OperationContract]
        void WriteIntoDestinationFile(List<string> fileLines, string fileName);

        [OperationContract]
        void WriteInBinaryFile(string file, byte[] text);

        [OperationContract]
        void EncryptBmpFile(string file, string outputDirectory);

        [OperationContract]
        void DecryptBmpFile(string file, string outputDirectory);

        [OperationContract]
        void ParallelReading(List<string> lines, int numberOfThreads);

        [OperationContract]
        void ParallelWriting(List<string> lines, int numberOfThreads);

        [OperationContract]
        void ParallelCoding(List<string> files, int numberOfThreads);

        [OperationContract]
        void SetKnapsack(bool k);

        [OperationContract]
        void SetBifid(bool b);

        [OperationContract]
        void SetRc6(bool r);

        [OperationContract]
        void SetBlockMode(bool b);
        [OperationContract]
        void SetCTR(bool r);

       
    }


    [DataContract]
    public class CompositeType
    {
       
        private string outputDirectory;
        
        public bool rc6;  
        public bool knapsack; 
        public bool bifid;       
        public string key;
        public bool blockMode;
        public bool ctr;
        public bool parallel;


        [DataMember]
        public bool Rc6
        {
            get { return rc6; }
            set { rc6 = value; }
        }
        [DataMember]
        public bool Knapsack
        {
            get { return knapsack; }
            set { knapsack = value; }
        }
        [DataMember]
        public bool Bifid
        {
            get { return bifid; }
            set { bifid = value; }
        }
        [DataMember]
        public bool BlockMode
        {
            get { return blockMode; }
            set { blockMode = value; }
        }
        [DataMember]
        public string OutputDirectory
        {
            get { return outputDirectory; }
            set { outputDirectory = value; }
        }
        [DataMember]
        public string Key
        {
            get { return key; }
            set { key = value; }
        }
        [DataMember]
        public bool CTR
        {
            get { return ctr; }
            set { ctr = value; }
        }
        [DataMember]
        public bool Parallel
        {
            get { return parallel; }
            set { parallel = value; }
        }
    }
}
