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
            labelLastName.Text = Resource1.LastName;
            labelFirstName.Text = Resource1.FirstName;
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
                LastName = textBox1.Text,
                FirstName = textBox2.Text
            };
            users.Add(u);
        }
    }
}
