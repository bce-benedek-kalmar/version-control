using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UserMaintenance.Entities;

namespace UserMaintenance
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            labelFullName.Text = Resource1.FullName;
            btnAdd.Text = Resource1.Add;
            btnSave.Text = Resource1.Save;

            //listbox1
            listBox1.DataSource = users;
            listBox1.DisplayMember = "FullName";
            listBox1.ValueMember = "ID";
        }

        BindingList<User> users = new BindingList<User>();

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var u = new User()
            {
                FullName = textBox1.Text
            };
            users.Add(u);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = Application.StartupPath;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string output = sfd.FileName;
                using (StreamWriter sw = new StreamWriter(output))
                {
                    foreach (User u in users)
                    {
                        sw.WriteLine(u.ID.ToString() + ";"+ u.FullName);
                    }
                }
            }
        }
    }
}
