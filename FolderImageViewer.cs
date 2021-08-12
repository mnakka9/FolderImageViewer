using MimeTypes;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace FolderImageViewer
{
    public partial class FolderImageViewer : Form
    {
        FolderBrowserDialog FolderBrowserDialog;
        public FolderImageViewer()
        {
            InitializeComponent();
            FolderBrowserDialog = new FolderBrowserDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void openFolder_Click(object sender, EventArgs e)
        {
            var result = FolderBrowserDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                LoadDirectory(FolderBrowserDialog.SelectedPath);
            }
        }

        private void LoadDirectory(string Dir)
        {
            DirectoryInfo di = new DirectoryInfo(Dir);
            TreeNode tds = treeView1.Nodes.Add(di.Name);
            tds.Tag = di.FullName;
            tds.StateImageIndex = 0;
            LoadFiles(Dir, tds);
            LoadSubDirectories(Dir, tds);
        }

        private void LoadSubDirectories(string dir, TreeNode td)
        {
            // Get all subdirectories
            string[] subdirectoryEntries = Directory.GetDirectories(dir).OrderBy(s => PadNumbers(s)).ToArray();
            // Loop through them to see if they have any other subdirectories
            foreach (string subdirectory in subdirectoryEntries)
            {

                DirectoryInfo di = new DirectoryInfo(subdirectory);
                TreeNode tds = td.Nodes.Add(di.Name);
                tds.StateImageIndex = 0;
                tds.Tag = di.FullName;
                LoadFiles(subdirectory, tds);
                LoadSubDirectories(subdirectory, tds);
            }
        }

        public static string PadNumbers(string input)
        {
            return Regex.Replace(input, "[0-9]+", match => match.Value.PadLeft(10, '0'));
        }

        private void LoadFiles(string dir, TreeNode td)
        {
            string[] Files = Directory.GetFiles(dir, "*.*").OrderBy(s => PadNumbers(s)).ToArray();

            // Loop through them to see files
            foreach (string file in Files)
            {
                FileInfo fi = new FileInfo(file);
                TreeNode tds = td.Nodes.Add(fi.Name);
                tds.Tag = fi.FullName;
                tds.StateImageIndex = 1;
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var node = this.treeView1.SelectedNode;

            if (node != null)
            {
                var path = node.Tag?.ToString();
                if (File.Exists(path))
                {
                    var file = new FileInfo(path);
                    var isImageFile = MimeTypeMap.GetMimeType(file.Extension)?.StartsWith("image/");

                    if (isImageFile.GetValueOrDefault())
                    {
                        pictureBox1.Image = Image.FromFile(file.FullName);
                    }
                }
            }
        }
    }
}
