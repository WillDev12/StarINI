using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using System.Security.Cryptography.X509Certificates;

namespace StarINI
{
    public partial class Form1 : Form
    {

        string result;
        string yourChildNode;
        public bool isPressed=false;
        FolderBrowserDialog fb = new FolderBrowserDialog();
        public Form1()
        {

            InitializeComponent();

        }

        private void Form1_Load(Object sender, EventArgs e) 
        {

            treeView1.BackColor = treeView1.Parent.BackColor;

        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2CircleButton2_Click(object sender, EventArgs e)
        {
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

            result = guna2TextBox1.Text.Insert(0, "[") + "]";
            treeView1.Nodes.Add(result);

        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {

            if (treeView1.SelectedNode == null)
            {

                MessageBox.Show("Please select a parent first.", "GalaxINI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            yourChildNode = guna2TextBox1.Text;

            if (treeView1.SelectedNode.Parent != null)
            {
                if (treeView1.SelectedNode.Parent.Parent != null)
                {

                    MessageBox.Show("You cannot add a variable to a variable.", "GalaxINI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;

                }
                else
                {

                    treeView1.SelectedNode.Parent.Nodes.Add(yourChildNode);

                }

            }
            else
            {

                treeView1.SelectedNode.Nodes.Add(yourChildNode);
                treeView1.ExpandAll();

            }

        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {

            if (treeView1.SelectedNode == null)
            {

                MessageBox.Show("Please select a parent first.", "GalaxINI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            treeView1.SelectedNode.Remove();

        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {

            if (treeView1.SelectedNode == null)
            {

                MessageBox.Show("Please select a parent first.", "GalaxINI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            if (treeView1.SelectedNode.Parent == null)
            {

                MessageBox.Show("Please select a child variable first.", "GalaxINI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }
            else if (treeView1.SelectedNode.Nodes.Count != 0)
            {

                treeView1.SelectedNode.Nodes.Clear();
                yourChildNode = guna2TextBox1.Text.Insert(0, "=");
                treeView1.SelectedNode.Nodes.Add(yourChildNode);
                treeView1.ExpandAll();

            }
            else if (treeView1.SelectedNode.Parent.Parent != null)
            {
                yourChildNode = guna2TextBox1.Text.Insert(0, "=");
                treeView1.SelectedNode.Text = yourChildNode;
            }
            else
            {

                yourChildNode = guna2TextBox1.Text.Insert(0, "=");
                treeView1.SelectedNode.Nodes.Add(yourChildNode);
                treeView1.ExpandAll();

            }

        }


        private void guna2Button6_Click_1(object sender, EventArgs e)
        {

            if (treeView1.SelectedNode == null)
            {

                MessageBox.Show("Please select a parent first.", "GalaxINI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            string yournode;
            yournode = treeView1.SelectedNode.Text.ToString();
            result = guna2TextBox1.Text;

            if (yournode.Contains("="))
            {

                treeView1.SelectedNode.Text = result.Insert(0, "=");

            }
            else if (yournode.Contains("["))
            {

                treeView1.SelectedNode.Text = result.Insert(0, "[") + "]";

            }
            else 
            {

                treeView1.SelectedNode.Text = result;

            }

        }

        private void treeView1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Back)
            {

                if (treeView1.SelectedNode == null && !isPressed)
                { }
                else
                {

                    isPressed = true;
                    treeView1.SelectedNode.Remove();

                }

            }


        }

        private void treeView1_KeyUp(object sender, KeyEventArgs e)
        {

            if (isPressed)
                isPressed= false;

        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {

            if (fb.ShowDialog() == DialogResult.OK) 
            {

                var path = fb.SelectedPath.ToString();
                guna2TextBox2.Text = path;
                Properties.Settings.Default.savePath = path;
            
            }

        }

        private void guna2Button9_Click(object sender, EventArgs e){}

        private void guna2Button9_Click_1(object sender, EventArgs e)
        {

            if (guna2TextBox3.Text == "") 
            {

                MessageBox.Show("Please set a valid name first", "GalaxINI", MessageBoxButtons.OK, MessageBoxIcon.Error);

            } 
            else 
            {

                result = guna2TextBox3.Text + ".ini";

                Properties.Settings.Default.saveName = result;

                if (guna2TextBox3.Text.Contains(".ini") == false)
                {

                    guna2TextBox3.Text += ".ini";

                }

            }

        }

        private void generateFile() {

            var filen = Properties.Settings.Default.saveName;
            var path = Properties.Settings.Default.savePath;

            var fullpath = path + "/" + filen;

            foreach (TreeNode rootNode in treeView1.Nodes)
            {
                if (rootNode.Text == string.Empty || rootNode.Text == "")
                {
                    MessageBox.Show("Please fill all parameters.", "GalaxINI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                BuildTreeString(rootNode);
            }   

            if (guna2TextBox2.Text == "" || guna2TextBox3.Text == "")
            {

                MessageBox.Show("Please set a valid name first", "GalaxINI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            if (result.Contains(filen)) { result = result.Replace(filen, ""); }

            File.WriteAllText(fullpath, result);

            result = "";
            count1 = 0;

        }

        private void guna2Button7_Click(object sender, EventArgs e){generateFile();}

        int count1 = -0;
        private void BuildTreeString(TreeNode rootNode)
        {

            if (rootNode.Text.Contains("=") == true)
            {

                result += rootNode.Text;

            } else {

                if (count1 > 0)
                {

                    if (rootNode.Text.Contains("["))
                    {

                        result += System.Environment.NewLine;
                        result += System.Environment.NewLine;
                        result += rootNode.Text;

                    }
                    else 
                    {

                        result += System.Environment.NewLine;
                        result += rootNode.Text;

                    }

                }
                else
                {

                    result += rootNode.Text;
                    count1++;

                }

            }

            foreach (TreeNode childNode in rootNode.Nodes)
                BuildTreeString(childNode);

        }

    }

}
