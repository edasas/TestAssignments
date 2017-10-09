using DCSL_Test.Helpers;
using System;
using System.IO;
using System.Windows.Forms;

namespace DCSL_Test
{
    public partial class Form1 : Form
    {
        #region ----------------------------------- Private members ------------------------------------------------
        private string sourcePath { get; set; }
        private string destPath { get; set; }
        #endregion

        public Form1()
        {
            InitializeComponent();
            btnCopy.Enabled = false;
            textBoxSource.ReadOnly = true;
            textBoxDest.ReadOnly = true;
        }

        #region ----------------------------------- Events ---------------------------------------------------------

        private void btnSource_Click(object sender, EventArgs e)
        {
            sourcePath = SelectDirectory();
            SetDirectory(sourcePath, textBoxSource);
        }

        private void btnDestination_Click(object sender, EventArgs e)
        {
            destPath = SelectDirectory();
            SetDirectory(destPath, textBoxDest);
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(sourcePath) || String.IsNullOrEmpty(destPath))
            {
                MessageBox.Show("Please select both source and destination directories!");
            }
            else
            {
                copyResultListBox.Items.Clear();
                FileCopyHelper.CopyDirectoryContentRecursive(sourcePath, destPath, WriteToListBox);
            }
        }

        #endregion

        #region ----------------------------------- Helper methods -------------------------------------------------
        private void SetDirectory(string pathVar, TextBox destBox)
        {
            try
            {
                if (destBox != null)
                {                    
                    destBox.Text = pathVar;
                }

                if (!String.IsNullOrEmpty(sourcePath) && !String.IsNullOrEmpty(destPath))
                {
                    btnCopy.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                WriteToListBox("Cannot select directory: " + ex.Message);
            }
        }

        private string SelectDirectory()
        {            
            string toReturn = string.Empty;
            try
            {
                using (var fbd = new FolderBrowserDialog())
                {
                    DialogResult result = fbd.ShowDialog();

                    if (result == DialogResult.OK && !string.IsNullOrEmpty(fbd.SelectedPath))
                    {
                        toReturn = fbd.SelectedPath;
                    }
                }
            }
            catch (Exception ex)
            {
                WriteToListBox("Cannot select directory: " + ex.Message);
            }
            return toReturn;
        }

        #endregion


        private void WriteToListBox(string message)
        {
            copyResultListBox.Items.Add(DateTime.Now.ToShortTimeString() + " " + message);
        }
    }
}
