using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Journal
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
               
            // открыть необходимые потоки
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream("journal", FileMode.Open, FileAccess.Read);

            try
            {
                BinaryReader br = new BinaryReader(fs);
                int n = br.ReadInt32();// считаем количество записей в файле
                if (n < 0)// если неверное число - ошибка
                {
                    MessageBox.Show("Файл поврежден!", "Ошибка загрузки", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    fs.Close();
                    return;
                }

                dataGridView1.Rows.Clear();// очистим таблицу Танюха была здесь

                while (n > 0)// загрузим данные
                {
                    Student s = (Student)bf.Deserialize(fs);// десериализуем объект

                    // создать новую строку
                    dataGridView1.Rows.Add();
                    DataGridViewRow nr = dataGridView1.Rows[dataGridView1.Rows.Count - 1];

                    // заполнить ее
                    nr.Cells["Student"].Value = s.Name;
                    nr.Cells["Group"].Value = s.Group;
                    nr.Cells["Test"].Value = s.Test;
                    nr.Cells["Late"].Value = s.Late;
                    for (int i = 1; i <= 18; i++)
                        nr.Cells["Column" + i.ToString()].Value = s.Visit[i-1];
                    n--;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("При чтении файла произошла ошибка. Возможно файл поврежден или имеет неверный формат.", "Ошибка при чтении файла!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            fs.Close();// закроем поток
        }

        private void добавитьСтудентаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddStudent dlg = new AddStudent();// создать новую диалоговую форму
            DialogResult res = dlg.ShowDialog();// вызвать ее и запомнить результат, который она возвратила

            if (res == DialogResult.OK)// если данные были верно введены
            {
                // создать новую строку
                dataGridView1.Rows.Add();
                DataGridViewRow nr = dataGridView1.Rows[dataGridView1.Rows.Count - 1];

                // заполнить ее
                nr.Cells["Student"].Value = dlg.data.Name;
                nr.Cells["Group"].Value = dlg.data.Group;
                nr.Cells["Late"].Value = dlg.data.Late;
            }
            else if (res == DialogResult.Abort)// иначе вывести сообщение об ошибке
            {
                MessageBox.Show("Вы не ввели данные!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void удалитьСтудентаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0 || dataGridView1.SelectedCells[0].OwningColumn.Name == "Student")// если есть выделение
            {
                dataGridView1.Rows.Remove(dataGridView1.CurrentRow);// удалить выделенную строку
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridView1.SelectedCells[0].OwningColumn.Name == "Student")// если столбец с именами
            {
                InfoStudent dlg = new InfoStudent(dataGridView1.CurrentRow.Cells["Student"].Value.ToString(), dataGridView1.CurrentRow.Cells["Group"].Value.ToString(), Convert.ToInt32(dataGridView1.CurrentRow.Cells["Late"].Value));// создать новую диалоговую форму
                DialogResult res = dlg.ShowDialog();// вызвать ее и запомнить результат, который она возвратила
                
                dataGridView1.CurrentRow.Cells["Late"].Value = dlg.newlate;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Сохранить журнал посещений?", "Сохранение", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                if (dataGridView1.Rows.Count == 0) return;
                else// если есть что сохранять
                {
                    dataGridView1.Show();
                    // открываем потоки
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream fs = new FileStream("journal", FileMode.Create, FileAccess.Write);

                    BinaryWriter bw = new BinaryWriter(fs);
                    bw.Write(dataGridView1.Rows.Count);

                    // записываем данные
                    foreach (DataGridViewRow r in dataGridView1.Rows)
                    {
                        Student s = new Student();
                        s.Name = r.Cells["Student"].Value.ToString();
                        s.Group = r.Cells["Group"].Value.ToString();
                        s.Test = Convert.ToInt32(r.Cells["Test"].Value);
                        s.Late = Convert.ToInt32(r.Cells["Late"].Value);
                        for (int i = 1; i <= 18; i++)
                            s.Visit[i - 1] = Convert.ToInt32(r.Cells["Column" + i.ToString()].Value);

                        bf.Serialize(fs, s);// сериализуем его
                    }
                    fs.Close();
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }

    [Serializable]// класс сериализуемый - его можно сохранять как поток байтов
    public class Student// класс Студент
    {
        // внутренние переменные, отвечающие за методы-свойства
        string name = "";// имя студента
        string group = "";// группа
        int[] visit = new int[18];// массив посещений
        int test = 0;// контрольная работа
        int late = 0;// минуты опозданий

        // имя студента - чтение и однократная запись
        public string Name
        {
            set { if (name == "") name = value; }
            get { return name; }
        }

        // группа - чтение и однократная запись
        public string Group
        {
            set { if (group == "") group = value; }
            get { return group; }
        }

        // контрольная работа - полный доступ
        public int Test
        {
            set { test = value; }
            get { return test; } 
        }

        // опоздания - полный доступ
        public int Late
        {
            set { late = value; }
            get { return late; }
        }

        // кол-во посещений - только чтение
        public int[] Visit
        {
            get { return visit; }
        }

        // индексатор
        public int this[int i]
        {
            get
            {
                return visit[i];
            }
            set
            {
                visit[i] = value;
            }
        }
    }
}
