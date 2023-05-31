using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hotel_Management
{
    public partial class Users : Form
    {
        public Users()
        {
            InitializeComponent();
            populate();
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\ABANS\Documents\HotelDB.mdf;Integrated Security=True;Connect Timeout=30");
        private void populate()
        {
            Con.Open();
            string Query = "select * from UserTable";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            UserDGV.DataSource = ds.Tables[0];
            Con.Close();
        }

        private void EditUsers()
        {
            if (UsernameTb.Text == "" || UphoneTb.Text == "" || PasswordTb.Text == "" || GenderCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information!");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("update UserTable set Uname=@UN,Uphone=@UP,Upassword=@UPW,Ugender=@UG where Unum=@UKey", Con);
                    cmd.Parameters.AddWithValue("@UN", UsernameTb.Text);
                    cmd.Parameters.AddWithValue("@UP", UphoneTb.Text);
                    cmd.Parameters.AddWithValue("@UPW", PasswordTb.Text);
                    cmd.Parameters.AddWithValue("@UG", GenderCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@UKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("User Updated!");
                    Con.Close();
                    populate();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }

            }
        }
        private void DeleteUsers()
        {
            if (Key == 0)
            {
                MessageBox.Show("Select a User!");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("delete form UserTable where Unum = @UKey", Con);
                    cmd.Parameters.AddWithValue("@UKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("User Deleted!");
                    Con.Close();
                    populate();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }

            }

        }
        private void InsertUsers()
        {
            if (UsernameTb.Text == "" || UphoneTb.Text == "" || PasswordTb.Text == "" || GenderCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information!");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into UserTable(Uname,Uphone,Upassword,Ugender) values(@UN,@UP,@UPW,@UG)", Con);
                    cmd.Parameters.AddWithValue("@UN", UsernameTb.Text);
                    cmd.Parameters.AddWithValue("@UP", UphoneTb.Text);
                    cmd.Parameters.AddWithValue("@UPW", PasswordTb.Text);
                    cmd.Parameters.AddWithValue("@UG", GenderCb.SelectedItem.ToString());
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("User Added!");
                    Con.Close();
                    populate();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }

            }

        }
        int Key = 0;
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            InsertUsers();
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            DeleteUsers();
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            EditUsers();
        }

        private void UserDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panel3_MouseClick(object sender, PaintEventArgs e)
        {
            Login obj = new Login();
            obj.Show();
            this.Hide();
        }
        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            
        }
    }
}
