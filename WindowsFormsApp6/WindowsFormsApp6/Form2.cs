using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace WindowsFormsApp6
{
    public partial class Form2 : Form
    {
        string connectionString = @"Data Source=DESKTOP-V6R1PEQ;Initial Catalog=DB;Integrated Security=True";

        public Form2()
        {
            InitializeComponent();
        }
        public Auto_Details parent;

        private void Form2_Load(object sender, EventArgs e)
        {

        }
        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            parent.Show();
        }
        void fillColums(string[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                dataGridView1.Columns.Add(arr[i], arr[i]);
                dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }
        }

        void LoadAmount()
        {
            dataGridView1.Columns.Clear();
            string query = "use DB; select * from Details_Informations";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand sqlCommand = new SqlCommand(query, connection);
                SqlDataReader R = sqlCommand.ExecuteReader();
                fillColums(new string[] { "ID", "Model", "Description" });

                while (R.Read())
                {
                    dataGridView1.Rows.Add(R[0].ToString(), R[1].ToString(), R[2].ToString());
                }
                connection.Close();


            }

        }
        //
        void InsertAm(string money, string description)
        {
            string query = $"use DB; insert into Details_Informations(Model, Description) values(N'{money}', N'{description}')";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            MessageBox.Show("Рядок було додано успішно");
        }
       
        void deleteRecord(int id)
        {
            string query = $"use DB; delete from Details_Informations where ID = N'{id}'";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            MessageBox.Show("Рядок було видалено успішно");
        }

        void updateRecord(int id, string money, string description)
        {
            string query = $"use DB; update Description_Informations set Model = N'{money}', Description = N'{description}' where ID=N'{id}'";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            MessageBox.Show("Рядок було змінено успішно");
        }
       
        bool checkDescription(string org)
        {
            bool res = org != "";
            if (!res) MessageBox.Show("Невірно введено опис");
            return res;
        }

        bool checkID(string id)
        {
            bool res;
            int tmp;
            res = int.TryParse(id, out tmp);
            if (!res) MessageBox.Show("Невірний ID");
            return res;
        }
        bool checkID(int id)
        {
            bool res = false;
            string query = $"select * from Details_Informations where ID={id}";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand sqlCommand = new SqlCommand(query, connection);
                SqlDataReader R = sqlCommand.ExecuteReader();
                res = R.Read();
                connection.Close();
            }
            return res;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            parent.Show();
            this.Close();
        }
        
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

       

        private void radioButton1_CheckedChanged(object sender, EventArgs e)


        {
            if (radioButton1.Checked)
            {
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                textBox3.Enabled = false;
            }

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                textBox1.Enabled = false;
                textBox2.Enabled = true;
                textBox3.Enabled = true;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                textBox3.Enabled = true;
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
            {
                textBox1.Enabled = true;
                textBox2.Enabled = false;
                textBox3.Enabled = false;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                LoadAmount();
            if (radioButton2.Checked)
                if ( checkDescription(textBox3.Text))
                    InsertAm(textBox2.Text, textBox3.Text);

            if (radioButton3.Checked)
                if (checkDescription(textBox3.Text))
                    if (checkID(Convert.ToInt32(textBox1.Text)))
                    {
                        updateRecord(Convert.ToInt32(textBox1.Text), textBox2.Text, textBox3.Text);
                    }
                    else
                        MessageBox.Show($"Запис з ID={textBox1.Text} не існує", "Помилка");

            if (radioButton4.Checked)
                if (checkID(textBox1.Text))
                {
                    if (checkID(Convert.ToInt32(textBox1.Text)))
                        deleteRecord(Convert.ToInt32(textBox1.Text));
                    else
                        MessageBox.Show($"Запис з ID={textBox1.Text} не існує", "Помилка");
                }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}


      

       
        
        
      

        

       

      
       

