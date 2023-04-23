using ZIProjekat;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Client 
{
    public partial class Form1 : Form
    {
        ZIProjekat.Service1 proxy;
        private string file;
        private string directory;
        private DirectoryInfo directoryInfo;
        private int numOfThreads;
        public Form1()
        {
            InitializeComponent();
            proxy = new Service1();
        }

        private void btnChooseFilesEncryptRC6_Click(object sender, EventArgs e)
        {
            
            openFileDialog1.Filter = "Text files (*.txt)|*.txt|(*.bmp)|*.bmp";
            openFileDialog1.ShowDialog();
            if (String.IsNullOrEmpty(openFileDialog1.FileName))
                return;
            rtbFilesToEncryptRC6.Text = openFileDialog1.FileName;
            this.file = rtbFilesToEncryptRC6.Text;
          
        }

        private void btnChooseFolderEncryptRC6_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = "";
            DialogResult result = folderBrowserDialog.ShowDialog();

            if (String.IsNullOrEmpty(folderBrowserDialog.SelectedPath))
                return;

           
            this.directory = folderBrowserDialog.SelectedPath;
            rtbDestinationFolderEncryptRC6.Text = folderBrowserDialog.SelectedPath;
            proxy.SetOutputDirectory(folderBrowserDialog.SelectedPath);
        }

        private void btnEncryptRC6_Click(object sender, EventArgs e)
        {
           if(txtKeyRC6.Text == "")
            {
                MessageBox.Show("You have to add key.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (rbtnRC6CTRON.Checked)
                proxy.SetCTR(true);
            else
                proxy.SetRc6(true);
            
            proxy.SetOutputDirectory(this.directory);
            proxy.EncodingFile(this.file);
           
            if(rbtnRC6CTRON.Checked)
                proxy.SetCTR(false);
            else
                proxy.SetRc6(false);
        }

        private void btnChooseFilesDecryptRC6_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Text files (*.txt)|*.txt|(*.bmp)|*.bmp|(*.rc6)|*.rc6|(*.ctr)|*.ctr";
            openFileDialog1.ShowDialog();
            if (String.IsNullOrEmpty(openFileDialog1.FileName))
                return;
            rtbFilesToDecryptRC6.Text = openFileDialog1.FileName;
            this.file = rtbFilesToDecryptRC6.Text;
        }

        private void btnChooseFolderDecryptRC6_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = "";
            DialogResult result = folderBrowserDialog.ShowDialog();

            if (String.IsNullOrEmpty(folderBrowserDialog.SelectedPath))
                return;

          
            this.directory = folderBrowserDialog.SelectedPath;
            rbtDestinationFolderDecryptRC6.Text = folderBrowserDialog.SelectedPath;
            proxy.SetOutputDirectory(folderBrowserDialog.SelectedPath);
        }

        private void btnDecryptRC6_Click(object sender, EventArgs e)
        {
            if (txtKeyRC6.Text == "")
            {
                MessageBox.Show("You have to add key.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (rbtnRC6CTRON.Checked)
                proxy.SetCTR(true);
            else
                proxy.SetRc6(true);
            
            proxy.SetOutputDirectory(this.directory);
            proxy.DecryptTextFile(this.file);
            
            if (rbtnRC6CTRON.Checked)
                proxy.SetCTR(false);
            else
                proxy.SetRc6(false);
        }

        private void btnChooseFilesEncryptBifid_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Text files (*.txt)|*.txt";
            openFileDialog1.ShowDialog();
            if (String.IsNullOrEmpty(openFileDialog1.FileName))
                return;
            rtbFilesToEncryptBifid.Text = openFileDialog1.FileName;
            this.file = rtbFilesToEncryptBifid.Text;
        }

        private void btnChooseFolderEncryptBifid_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = "";
            DialogResult result = folderBrowserDialog.ShowDialog();

            if (String.IsNullOrEmpty(folderBrowserDialog.SelectedPath))
                return;

            this.directory = folderBrowserDialog.SelectedPath;
            rtbDestinationFolderEncryptBifid.Text = folderBrowserDialog.SelectedPath;
            proxy.SetOutputDirectory(folderBrowserDialog.SelectedPath);
        }

        private void btnEncryptBifid_Click(object sender, EventArgs e)
        {
            
            proxy.SetBifid(true);
            
            proxy.SetOutputDirectory(this.directory);
            proxy.EncodingFile(this.file);
          
            proxy.SetBifid(false);
        }

        private void btnChooseFilesDecryptBifid_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Text files (*.txt)|*.txt|(*.bifid)|*.bifid";
            openFileDialog1.ShowDialog();
            if (String.IsNullOrEmpty(openFileDialog1.FileName))
                return;
            rtbFilesToDecryptBifid.Text = openFileDialog1.FileName;
            this.file = rtbFilesToDecryptBifid.Text;
        }

        private void btnChooseFolderDecryptBifid_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = "";
            DialogResult result = folderBrowserDialog.ShowDialog();

            if (String.IsNullOrEmpty(folderBrowserDialog.SelectedPath))
                return;


            this.directory = folderBrowserDialog.SelectedPath;
            rbtDestinationFolderDecryptBifid.Text = folderBrowserDialog.SelectedPath;
            proxy.SetOutputDirectory(folderBrowserDialog.SelectedPath);
        }

        private void btnDecryptBifid_Click(object sender, EventArgs e)
        {
           
            proxy.SetBifid(true);
          
            proxy.SetOutputDirectory(this.directory);
            proxy.DecryptTextFile(this.file);
            
            proxy.SetBifid(false);

        }

        private void btnChooseFilesEncryptKnapsack_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Text files (*.txt)|*.txt";
            openFileDialog1.ShowDialog();
            if (String.IsNullOrEmpty(openFileDialog1.FileName))
                return;
            rtbFilesToEncryptKnapsack.Text = openFileDialog1.FileName;
            this.file = rtbFilesToEncryptKnapsack.Text;
        }

        private void btnChooseFolderEncryptKnapsack_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = "";
            DialogResult result = folderBrowserDialog.ShowDialog();

            if (String.IsNullOrEmpty(folderBrowserDialog.SelectedPath))
                return;

            this.directory = folderBrowserDialog.SelectedPath;
            rtbDestinationFolderEncryptKnapsack.Text = folderBrowserDialog.SelectedPath;
            proxy.SetOutputDirectory(folderBrowserDialog.SelectedPath);
        }

        private void btnEncryptKnapsack_Click(object sender, EventArgs e)
        {
            
            proxy.SetKnapsack(true);
            
            if (rbtnOn.Checked)
               
                proxy.SetBlockMode(true);
            else
              
                proxy.SetBlockMode(false);
            proxy.SetOutputDirectory(this.directory);
            proxy.EncodingFile(this.file);
            
            proxy.SetKnapsack(false);
        }

        private void btnChooseFilesDecryptKnapsack_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Text files (*.txt)|*.txt|(*.knapsack)|*.knapsack";
            openFileDialog1.ShowDialog();
            if (String.IsNullOrEmpty(openFileDialog1.FileName))
                return;
            rtbFilesToDecryptKnapsack.Text = openFileDialog1.FileName;
            this.file = rtbFilesToDecryptKnapsack.Text;
        }

        private void btnChooseFolderDecryptKnapsack_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = "";
            DialogResult result = folderBrowserDialog.ShowDialog();

            if (String.IsNullOrEmpty(folderBrowserDialog.SelectedPath))
                return;


            this.directory = folderBrowserDialog.SelectedPath;
            rbtDestinationFolderDecryptKnapsack.Text = folderBrowserDialog.SelectedPath;
            proxy.SetOutputDirectory(folderBrowserDialog.SelectedPath);

        }

        private void btnDecryptKnapsack_Click(object sender, EventArgs e)
        {
           
            proxy.SetKnapsack(true);
          
            if (rbtnOn.Checked)
             
                proxy.SetBlockMode(true);
            else
               
                proxy.SetBlockMode(false);
            proxy.SetOutputDirectory(this.directory);
            bool hashed = proxy.DecryptTextFile(this.file);
          
            proxy.SetKnapsack(false);
            if (!hashed)
            {
                
                MessageBox.Show("Hash values were not the same.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
             
            }
            else
            {
                MessageBox.Show("Hash values are the same.","Information",  MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnParallel_Click(object sender, EventArgs e)
        {
            if (txtNumberOfThreads.Text == "")
            {
                MessageBox.Show("You have to add number of threads!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!rbtnRC6CTROFF.Checked)
            {
                MessageBox.Show("Coding mode has to be inactive!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            List<string> lines = new List<string>();
            List<FileInfo> fileInfos = this.directoryInfo.GetFiles().ToList();
            List<FileInfo> onlyTxts = fileInfos.Where(fi => fi.Extension == ".txt").ToList();
           

            proxy.SetOutputDirectory(this.directory);
          
            if (rbtnRC6CTRON.Checked)
                proxy.SetCTR(true);
            else
                proxy.SetRc6(true);

         
            foreach (var file in onlyTxts)
            {
              
                lines.Add(file.FullName);
            }

            proxy.ParallelReading(lines, numOfThreads);
           

            if (rbtnRC6CTRON.Checked)
                proxy.SetCTR(false);
            else
                proxy.SetRc6(false);

            fileInfos.Clear();
            onlyTxts.Clear();
        }

        private void btnChooseFilesParallel_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = "";
            folderBrowserDialog.ShowDialog();

            if (String.IsNullOrEmpty(folderBrowserDialog.SelectedPath))
                return;

            this.directory = folderBrowserDialog.SelectedPath;
            rtbChooseFilesParallel.Text = this.directory;
            this.directoryInfo = new DirectoryInfo(this.directory);
        }

        private void btnKeyRC6_Click(object sender, EventArgs e)
        {
            proxy.SetKey(txtKeyRC6.Text);
        }

       

        private void btnNumberOfThreads_Click(object sender, EventArgs e)
        {
            numOfThreads = int.Parse(txtNumberOfThreads.Text);
        }

        private void btnParallelDecrypt_Click(object sender, EventArgs e)
        {
            if (!rbtnRC6CTROFF.Checked)
            {
                MessageBox.Show("Coding mode has to be inactive!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtNumberOfThreads.Text == "")
            {
                MessageBox.Show("You have to add number of threads!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            List<string> lines = new List<string>();
            List<FileInfo> fileInfos = this.directoryInfo.GetFiles().ToList();
            List<FileInfo> onlyTxts = fileInfos.Where(fi => fi.Extension == ".rc6").ToList();
           

            proxy.SetOutputDirectory(this.directory);
            
            if (rbtnRC6CTRON.Checked)
                proxy.SetCTR(true);
            else
                proxy.SetRc6(true);

           
            foreach (var file in onlyTxts)
            {
               
                lines.Add(file.FullName);
            }

            
            proxy.ParallelWriting(lines, numOfThreads);

            if (rbtnRC6CTRON.Checked)
                proxy.SetCTR(false);
            else
                proxy.SetRc6(false);

            fileInfos.Clear();
            onlyTxts.Clear();
        }

        private void btnParallelCoding_Click(object sender, EventArgs e)
        {
            if (txtNumberOfThreads.Text=="")
            {
                MessageBox.Show("You have to add number of threads!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            List<string> encryptedFileLines = new List<string>();
            List<FileInfo> fileInfos = this.directoryInfo.GetFiles().ToList();
            List<FileInfo> onlyTxts = fileInfos.Where(fi => fi.Extension == ".txt").ToList();

            proxy.SetOutputDirectory(this.directory);

            foreach (var file in onlyTxts)
            {

                encryptedFileLines.Add(file.FullName);
            }
            proxy.ParallelCoding(encryptedFileLines, numOfThreads);

            fileInfos.Clear();
            onlyTxts.Clear();
        }
    }
}
