using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Journal
{
    public partial class InfoStudent : Form
    {
        public int newlate;

        public InfoStudent()
        {
            InitializeComponent();
        }

        public InfoStudent(string name, string group, int late)
        {
            InitializeComponent();
            label5.Text = name;
            label6.Text = group;
            label7.Text = late + " минуты.";
            newlate = late;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            newlate = newlate + Convert.ToInt32(numericUpDown1.Value);
            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void InfoStudent_Load(object sender, EventArgs e)
        {

        }
    }
}
