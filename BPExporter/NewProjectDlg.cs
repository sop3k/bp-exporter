using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BPExporter
{
    public partial class NewProjectDlg : Form
    {
        private Project project = null;
        private bool unique;

        public NewProjectDlg()
        {
            InitializeComponent();
        }

        public NewProjectDlg(Project proj)
        {
            InitializeComponent();
            project = proj;
            unique = proj.UniqueIP;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            project = project ?? new Project();

            project.FilesNo = FileNo.Text;
            project.Name = ProjectName.Text;
            project.Sufix = sufixFileNo.Text;
            project.Prefix = prefixFileNo.Text;
            project.UniqueIP = uniqueIP.Checked;
            project.Product = ProductName.Text;
        }

        public Project GetProject()
        {
            return project;
        }

        private void FileNo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int.Parse(FileNo.Text);
                FileNo.BackColor = Color.LightGreen;
                okBtn.Enabled = true;
            }
            catch (FormatException)
            {
                FileNo.BackColor = Color.LightSalmon;
                okBtn.Enabled = false;
            }

        }

        private void NewProjectDlg_Load(object sender, EventArgs e)
        {
            if (project != null)
            {
                FileNo.Text = project.FilesNo;
                ProjectName.Text = project.Name;
                sufixFileNo.Text = project.Sufix;
                prefixFileNo.Text = project.Prefix;
                uniqueIP.Checked = project.UniqueIP;

                ProjectName.Enabled = false;
                uniqueIP.Enabled = uniqueIP.Checked;
                sufixFileNo.Enabled = false;

            }
            else
            {
                sufixFileNo.Text = String.Format("{0:yy}", DateTime.Now);
            }
        }

        public bool UniqueChanged()
        {
            return project.UniqueIP != unique;
        }

        private void uniqueIP_CheckedChanged(object sender, EventArgs e)
        {
        }
    }
}
