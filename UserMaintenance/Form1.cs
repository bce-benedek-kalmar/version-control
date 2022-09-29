using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
    }
}
