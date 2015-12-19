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
    public partial class AddStudent : Form
    {
        public Student data = null;

        public AddStudent()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // если не все данные введены
            if (textBox1.Text.Trim() == "" || textBox2.Text.Trim() == "")
            {
                data = null;// ничего не возвращать
                this.DialogResult = DialogResult.Abort;// отмена результата
            }

            else
            {
                data = new Student();// создать новую буферную переменную

                // занести в нее все данные
                data.Name = textBox1.Text;
                data.Group = textBox2.Text;
                this.DialogResult = DialogResult.OK;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void AddStudent_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
