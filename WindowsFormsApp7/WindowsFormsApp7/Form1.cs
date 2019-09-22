using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
namespace WindowsFormsApp7
{
    public partial class Form1 : Form
    {
        string connectionString = "server=192.168.198.147; port=3306; database=DB1; user=root; password=123456;charset=utf8";

        public Form1()
        {
            InitializeComponent();

            check_DB_Table(1, "Пруж", "ХОДОВА", 2, "weapoons","Пружина");
            load_list_details();
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }
        void check_DB_Table(int id1, string model1, string description1, int id2, string model2, string description2)
        {

            string sql1 = $"select * from Details_Informations where ID='{id1}' and Model='{model1}' and Description='{description1}'";
            string sql2 = $"select * from Details_Informations where ID='{id2}' and Model='{model2}' and Description='{description2}'";


            using (MySqlConnection table2 = new MySqlConnection(connectionString))
            {
                table2.Open();
                MySqlCommand command = new MySqlCommand(sql1, table2);
                MySqlDataReader R = command.ExecuteReader();
                if (R.Read() != false)
                {
                    string updateQuery = $"use DB1; insert into Details_Informations(Model,Description) values(n'{model1}', n'{description1}')";
                    MySqlCommand command1 = new MySqlCommand(updateQuery, table2);
                   
                    command1.ExecuteNonQuery();

                }
                 R.Close();
                MySqlCommand commandn = new MySqlCommand(sql2, table2);
                R = commandn.ExecuteReader();
                if (R.Read() != false)
                {
                    string updateQuery1 = $"use DB1; insert into Details_Informations(Model, Description) values(n'{model2}', n'{description2}')";
                    MySqlCommand command2 = new MySqlCommand(updateQuery1, table2);
                  
                    command2.ExecuteNonQuery();
                }
                R.Close();
                table2.Close();
            }
        }

        void fillColums_DB(string[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                dataGridView1.Columns.Add(arr[i], arr[i]);
                dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }
        }

        void load_list_details()
        {
            comboBox1.Items.Clear();

            string query1 = $"select Description from Details_Informations ";

            using (MySqlConnection table2 = new MySqlConnection(connectionString))
            {
                table2.Open();
                MySqlCommand command = new MySqlCommand(query1, table2);
                MySqlDataReader R = command.ExecuteReader();
                while (R.Read())
                {
                    comboBox1.Items.Add(R[0].ToString());
                }
                table2.Close();
            }

        }

        //Тут есть глюк
        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
        void loaddetails(bool id, bool amount)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            string query;
            if (!id && !amount)
            {
                query = "use DB1; select * from Details_Knot";// inner join Details_Informations on Details_Informations.Description = Details_Knot.ID_Description";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand sqlCommand = new MySqlCommand(query, connection);
                    MySqlDataReader R = sqlCommand.ExecuteReader();

                    fillColums_DB(new string[] { "ID", "Name_Detail", "Seria_Number", "ID_Description", "Manufacture", "Material" });//, "Model", "Description" });

                    while (R.Read())
                    {
                        dataGridView1.Rows.Add(R[0].ToString(), R[1].ToString(), R[2].ToString(), R[3].ToString(), R[4].ToString(), R[5].ToString());//, R[6].ToString(), R[7].ToString());
                    }
                    connection.Close();
                }
            }
            else
            {

                query = "use DB1; select * from Details_Knot inner join Details_Informations on Details_Informations.Description = Details_Knot.ID_Description where ";
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
                else
                
                    query += $"Details_Informations.Description='{comboBox1.Text}'";

                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();
                        MySqlCommand sqlCommand = new MySqlCommand(query, connection);
                        MySqlDataReader R = sqlCommand.ExecuteReader();///
                        fillColums_DB(new string[] { "ID", "Name_Detail", "Seria_Number", "ID_Description", "Manufacture", "Material", "Model", "Description" });

                        while (R.Read())
                        {
                            dataGridView1.Rows.Add(R[0].ToString(), R[1].ToString(), R[2].ToString(), R[3].ToString(), R[4].ToString(), R[5].ToString(), R[6].ToString(), R[7].ToString());
                        }
                        connection.Close();

                    }
                
            }

        }
        //Пересмотреть, тут есть глюк
        bool check_Exit(string name, string seria, string id_d, string manuf, string mater)
        {
            bool res = false;

            string query = $" SELECT * FROM Details_Knot where Name_Detail='{name}' and Seria_Number='{seria}' and ID_Description='{id_d}' and Manufacture= '{manuf}' and Material='{mater}'";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand sqlCommand = new MySqlCommand(query, connection);
                MySqlDataReader R = sqlCommand.ExecuteReader();
                res = R.Read();
                connection.Close();
            }
            return res;
        }

        bool checkID(int id)
        {
            bool res = false;
            string query = $"select * from Details_Knot where ID= '{id}'";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand sqlCommand = new MySqlCommand(query, connection);
                MySqlDataReader R = sqlCommand.ExecuteReader();
                res = R.Read();
                connection.Close();
            }
            return res;
        }

        int getDescription(string description)
        {
            int res;
            string query1 = $"select ID from Details_Informations where Description=n'{description}'";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand command = new MySqlCommand(query1, conn);
                MySqlDataReader R = command.ExecuteReader();
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
       // Глюк
        void InsertKnot(string Name, string Seria, string ID_Det, string Man, string Mater)
        {
            string query = $"use DB1; insert into Details_Knot(Name_Detail, Seria_Number, ID_Description, Manufacture, Material) values(n'{Name}', n'{Seria}', n'{ID_Det}', n'{Man}', n'{Mater}')";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(query, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            MessageBox.Show("Рядок було додано успішно");
        }

        void updateRecord(int Id, string Name, string Seria, string ID_Det, string Man, string Mat)
        {


            string query = $"use DB1; update Details_Knot set Name_Detail = n'{Name}', Seria_Number = n'{Seria}', ID_Description = n'{ID_Det}',Manufacture = n'{Man}',  Material = n'{Mat}' where ID = n'{Id}'";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(query, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            MessageBox.Show("Рядок було змінено успішно");
        }

        void deleteRecord(int id)
        {
            string query = $"use DB1; delete from Details_Knot where ID = {id}";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(query, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            MessageBox.Show("Рядок було видалено успішно");
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //get records
            if (radioButton1.Checked)

                loaddetails(checkBox2.Checked, checkBox1.Checked);

            //insert record
            if (radioButton2.Checked)
            {
                if (checkName(textBox2.Text) && checkMaterial(textBox4.Text))
                    if (!check_Exit(textBox2.Text, textBox3.Text, comboBox1.Text, textBox4.Text, textBox5.Text ))
                        InsertKnot(textBox2.Text, textBox3.Text,comboBox1.Text, textBox4.Text, textBox5.Text );
                    else
                        MessageBox.Show("Даний запис вже існує");
            }

            //update record
            if (radioButton3.Checked)
            {
                if (checkID(textBox1.Text) && checkName(textBox2.Text) && checkMaterial(textBox4.Text))
                    if (checkID(Convert.ToInt32(textBox1.Text)))
                    {
                        if (chekSeria(textBox3.Text))
                            updateRecord(Convert.ToInt32(textBox1.Text), textBox2.Text, comboBox1.Text,textBox3.Text, textBox4.Text, textBox5.Text );
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
                MessageBox.Show("Серію введено не вірно, почніть з літер");
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

        bool checkMaterial(string det)
        {
            bool chD = det != "";
            if (!chD) MessageBox.Show("Невірно введеий матеріал");
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





        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

     

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                loaddetails(checkBox2.Checked, checkBox1.Checked);

            //insert record
            if (radioButton2.Checked)
            {
                if (checkName(textBox2.Text) && chekSeria(textBox3.Text) && checkMaterial(textBox4.Text) )
                    if (!check_Exit(textBox2.Text, textBox3.Text, comboBox1.Text,textBox4.Text, textBox5.Text))
                        InsertKnot(textBox2.Text, textBox3.Text, comboBox1.Text, textBox4.Text, textBox5.Text);
                    else
                        MessageBox.Show("Даний запис вже існує");
            }

            //update record
            if (radioButton3.Checked)
            {
                if (checkID(textBox1.Text) && checkName(textBox2.Text) && chekSeria(textBox3.Text) && checkMaterial(textBox4.Text) )
                    if (checkID(Convert.ToInt32(textBox1.Text)))
                    {
                        if (chekSeria(textBox3.Text))
                            updateRecord(Convert.ToInt32(textBox1.Text), textBox2.Text, comboBox1.Text,textBox3.Text, textBox4.Text, textBox5.Text );
                    }
                    else
                        MessageBox.Show($"Запис з ID={textBox1.Text} не існує", "Помилка");
            }

            //delete record
            if (radioButton4.Checked)
            {
                if (checkID(textBox1.Text))
                {
                    if (checkID(Convert.ToInt32(textBox1.Text)))
                        deleteRecord(Convert.ToInt32(textBox1.Text));
                    else
                        MessageBox.Show($"Запис з ID={textBox1.Text} не існує", "Помилка");
                }
            }
        }

        private void Form1_VisibleChanged(object sender, EventArgs e)
        {
            load_list_details();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Form2 form = new Form2();
            form.parent = this;
            form.Show();
            this.Hide();
        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (radioButton1.Checked && checkBox1.Checked)
                comboBox1.Enabled = true;
            else if (radioButton1.Checked) comboBox1.Enabled = false;
        }

        private void checkBox2_CheckedChanged_1(object sender, EventArgs e)
        {
            if (radioButton1.Checked && checkBox2.Checked)
                textBox1.Enabled = true;
            else if (radioButton1.Checked) textBox1.Enabled = false;
        }

        private void radioButton1_CheckedChanged_1(object sender, EventArgs e)
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

        private void radioButton2_CheckedChanged_1(object sender, EventArgs e)
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

        private void radioButton3_CheckedChanged_1(object sender, EventArgs e)
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
    }
}

