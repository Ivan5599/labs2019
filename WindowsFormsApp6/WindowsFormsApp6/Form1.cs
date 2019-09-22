using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;


namespace WindowsFormsApp6
{
    public partial class Auto_Details : Form
    {
        string connectionString = @"Data Source=DESKTOP-V6R1PEQ;Initial Catalog=DB;Integrated Security=True";
        
        public Auto_Details()
        {
            InitializeComponent();
           checkDB(1, "Пруж", "ХОДОВА", 2, "weapoons","Пружина");
            loadAmountofTrade();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        void checkDB(int id1, string model1, string description1, int id2, string model2, string description2)
        {
            
            string sql1 = $"select * from Details_Informations where ID='{id1}' and Model='{model1}' and Description='{description1}'";
            string sql2 = $"select * from Details_Informations where ID='{id2}' and Model='{model2}' and Description='{description2}'";


            using (SqlConnection table2 = new SqlConnection(connectionString))
            {
                table2.Open();
                SqlCommand command = new SqlCommand(sql1, table2);
                SqlDataReader R = command.ExecuteReader();
                if (R.Read() != false)
                {
                    string insertQuery1 = $"use DB insert into Details_Informations(Model,Description) values(N'{model1}', N'{description1}')";
                    SqlCommand command1 = new SqlCommand(insertQuery1, table2);
                    
                    command1.ExecuteNonQuery();

                }
                R.Close();
                SqlCommand commandn = new SqlCommand(sql2, table2);
                R = commandn.ExecuteReader();
                if (R.Read() != false)
                {
                    string insertQuery2 = $"use DB insert into Details_Informations(Model, Description) values(N'{model2}', N'{description2}')";
                    SqlCommand command2 = new SqlCommand(insertQuery2, table2);
                    
                    command2.ExecuteNonQuery();
                }
                R.Close();
                table2.Close();
            }
        }

        void fillColums(string[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                dataGridView1.Columns.Add(arr[i], arr[i]);
                dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }
        }

        void loadAmountofTrade()
        {
            comboBox1.Items.Clear();

            string query1 = $"select Description from Details_Informations ";

            using (SqlConnection table2 = new SqlConnection(connectionString))
            {
                table2.Open();
                SqlCommand command = new SqlCommand(query1, table2);
                SqlDataReader R = command.ExecuteReader();
                while (R.Read())
                {
                    comboBox1.Items.Add(R[0].ToString());
                }
                table2.Close();
            }

        }

        
        void LoadOrgs(bool id, bool amount)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            string query;
            if (!id && !amount)
            {
                query = "use DB; select * from Details_Knot inner join Details_Informations on Details_Knot.ID_Description = Details_Informations.Description";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand sqlCommand = new SqlCommand(query, connection);
                    SqlDataReader R = sqlCommand.ExecuteReader();

                    fillColums(new string[] { "ID", "Name_Detail", "Seria_Number", "ID_Description", "Manufacture", "Material", "Model", "Description" });

                    while (R.Read())
                    {
                        dataGridView1.Rows.Add(R[0].ToString(), R[1].ToString(), R[2].ToString(), R[3].ToString(), R[4].ToString(), R[5].ToString(),R[6].ToString(),R[7].ToString());
                    }
                    connection.Close();
                }
            }
            else
            {

                query = "use DB select * from Details_Knot inner join Details_Informations on Details_Informations.Description = Details_Knot.ID_Description where ";
                if (id && amount)
                {
                    if (checkID(textBox1.Text))
                        query += $"Details_Knot.ID={Convert.ToInt32(textBox1.Text)} and Details_Informations.Description='{comboBox1.Text}'";
                    else
                    {
                        MessageBox.Show("Невірний ID");
                        return;
                    }
                }
                else if (id)
                    if (checkID(textBox1.Text))
                        query += $"Details_Knot.ID={Convert.ToInt32(textBox1.Text)}";
                    else
                    {
                        MessageBox.Show("Невірний ID");
                        return;
                    }
                else query += $"Details_Informations.Description='{comboBox1.Text}'";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand sqlCommand = new SqlCommand(query, connection);
                        SqlDataReader R = sqlCommand.ExecuteReader();///
                        fillColums(new string[] { "ID", "Name_Detail", "Seria_Number", "ID_Description", "Manufacture", "Material","Model","Description"});

                        while (R.Read())
                        {
                            dataGridView1.Rows.Add(R[0].ToString(), R[1].ToString(), R[2].ToString(), R[3].ToString(), R[4].ToString(), R[5].ToString(), R[6].ToString(), R[7].ToString());
                        }
                        connection.Close();

                    }
                
            }

        }
  
        bool checkExit(string name, string seria, string id_d, string manuf, string mater)
        {
            bool res = false;

            string query = $" SELECT * FROM Details_Knot where Name_Detail='{name}' and Seria_Number='{seria}' and ID_Description='{id_d}' and Manufacture= '{manuf}' and Material='{mater}'";

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

        bool checkID(int id)
        {
            bool res = false;
            string query = $"select * from Details_Knot where ID= '{id}'";

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

        int getAlountID(string description)
        {
            int res;
            string query1 = $"select ID from Details_Informations where Description=N'{description}'";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(query1, conn);
                SqlDataReader R = command.ExecuteReader();
                if (R.Read())
                {
                    res = R.GetInt32(0);
                }
                else
                    res = -1;
                conn.Close();

            }
            return res;
        }
     
        void InsertOrg(string Name, string Seria, string ID_Det, string Man, string Date)
        {
            string query = $"use DB insert into Details_Knot(Name_Detail, Seria_Number, ID_Description, Manufacture, Material) values(N'{Name}', N'{Seria}', N'{ID_Det}', N'{Man}', N'{Date}')";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            MessageBox.Show("Рядок було додано успішно");
        }

        void updateRecord(int Id, string Name, string Seria, string ID_Det, string Man, string Date)
        {


            string query = $"use DB; update Details_Knot set Name_Detail = N'{Name}', Seria_Number = N'{Seria}', ID_Description = N'{ID_Det}',Manufacture = N'{Man}',  Material = N'{Date}' where ID = N'{Id}'";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            MessageBox.Show("Рядок було змінено успішно");
        }

        void deleteRecord(int id)
        {
            string query = $"use DB delete Details_Knot where ID = '{id}'";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            MessageBox.Show("Рядок було видалено успішно");
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //get records
            if (radioButton1.Checked)
                LoadOrgs(checkBox2.Checked, checkBox1.Checked);

            //insert record
            if (radioButton2.Checked)
            {
                if (checkName(textBox2.Text)  && checkDate(textBox4.Text) )
                    if (!checkExit(textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, comboBox1.Text))
                        InsertOrg(textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text,comboBox1.Text);
                    else
                        MessageBox.Show("Даний запис вже існує");
            }

            //update record
            if (radioButton3.Checked)
            {
                if (checkID(textBox1.Text) && checkName(textBox2.Text)  && checkDate(textBox4.Text) )
                    if (checkID(Convert.ToInt32(textBox1.Text)))
                    {
                        if (chekSeria(textBox3.Text))
                            updateRecord(Convert.ToInt32(textBox1.Text), textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, comboBox1.Text);
                    }
                    else
                        MessageBox.Show($"Запис з ID='{textBox1.Text}' не існує", "Помилка");
            }

            //delete record
            if (radioButton4.Checked)
            {
                if (checkID(textBox1.Text))
                {
                    if (checkID(Convert.ToInt32(textBox1.Text)))
                        deleteRecord(Convert.ToInt32(textBox1.Text));
                    else
                        MessageBox.Show($"Запис з ID='{textBox1.Text}' не існує", "Помилка");
                }
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                textBox3.Enabled = false;
                textBox4.Enabled = false;
                textBox5.Enabled = false;
                comboBox1.Enabled = false;
                if (checkBox1.Checked)
                    comboBox1.Enabled = true;
                if (checkBox2.Checked)
                    textBox1.Enabled = true;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                textBox1.Enabled = false;
                textBox2.Enabled = true;
                textBox3.Enabled = true;
                textBox4.Enabled = true;
                textBox5.Enabled = true;
                comboBox1.Enabled = true;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
            {
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                textBox3.Enabled = true;
                textBox4.Enabled = true;
                textBox5.Enabled = true;
                comboBox1.Enabled = true;
            }

        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            comboBox1.Enabled = false;
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked && checkBox1.Checked)
                comboBox1.Enabled = true;
            else if (radioButton1.Checked) comboBox1.Enabled = false;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked && checkBox2.Checked)
                textBox1.Enabled = true;
            else if (radioButton1.Checked) textBox1.Enabled = false;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        bool chekSeria(string str)
        {
            Regex regex = new Regex("^[a-zA-Zа-яА-Я /d]");
            Match M = regex.Match(str);
            if (!M.Success)
            {
                MessageBox.Show("Власник введений не вірно");
                return false;
            }
            return true;
        }

        bool checkName(string det)
        {
            bool chN = det != "";
            if (!chN) MessageBox.Show("Невірно введено назву деталі");
            return chN;
        }

        bool checkDate(string det)
        {
            bool chD = det != "";
            if (!chD) MessageBox.Show("Невірно введеий тип");
            return chD;
        }

        //bool checkManufacture(string price)
        //{
        //    float tmp;
        //    bool res = float.TryParse(price, out tmp);
        //    if (!res) MessageBox.Show("Невірний виробник");
        //    return res;
        //}

        bool checkID(string id)
        {
            bool chi;
            int tmp;
            chi = int.TryParse(id, out tmp);
            if (!chi) MessageBox.Show("Невірний ID");
            return chi;
        }

       

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2();
            form.parent = this;
            form.Show();
            this.Hide();
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void fillByToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.details_KnotTableAdapter.FillBy(this.dBDataSet.Details_Knot);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
