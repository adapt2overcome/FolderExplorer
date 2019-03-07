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
using System.Security.Permissions;
using System.Security;

namespace DirectoryStructure
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            DriveInfo[] myDrives = DriveInfo.GetDrives();

            foreach (var drive in myDrives)
            {
                TreeNode rootFolder = treeView1.Nodes.Add(drive.Name);

                addSubFoldersAndFiles(rootFolder);
            }
        }


        private void addSubFoldersAndFiles(TreeNode node)
        {
            try
            {   
                AppendFolders(node);
                AppendFiles(node);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        public void AppendFiles(TreeNode nodeToAppendTo)
        {
            List<string> files = new List<string>(Directory.EnumerateFiles(nodeToAppendTo.FullPath));

            foreach(var file in files)
            {
                nodeToAppendTo.Nodes.Add(Path.GetFileName(file));
            }
        }


        public void AppendFolders(TreeNode nodeToAppendTo)
        {
            List<string> dirs = new List<string>(Directory.EnumerateDirectories(nodeToAppendTo.FullPath));
            foreach (var directory in dirs)
            {
                nodeToAppendTo.Nodes.Add(Path.GetFileName(directory));
            }
        }


        public void AppendExplorer(TreeNode nodeToAppendTo)
        {
            int i; 
            for(i = nodeToAppendTo.Nodes.Count - 1; i >= 0; i--)
            {
                FileAttributes attr = File.GetAttributes(nodeToAppendTo.Nodes[i].FullPath);
                if (nodeToAppendTo.Nodes[i].GetNodeCount(true) == 0 &&
                    attr.HasFlag(FileAttributes.Directory))
                addSubFoldersAndFiles(nodeToAppendTo.Nodes[i]);
            }
        }


        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            propertyGrid1.SelectedObject = new DirectoryInfo(e.Node.FullPath); 
        }


        private void treeView1_AfterExpand(object sender, TreeViewEventArgs e)
        {
            AppendExplorer(e.Node);
        }
    }
}
