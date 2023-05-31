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
    public partial class Customers : Form
    {
        public Customers()
        {
            InitializeComponent();
            populate();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\ABANS\Documents\HotelDB.mdf;Integrated Security=True;Connect Timeout=30");
        private void populate()
        {
            Con.Open();
            string Query = "select * from CustomerTable";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            CustomerDGV.DataSource = ds.Tables[0];
            Con.Close();
        }

        private void EditCustomer()
        {
            if (CnameTb.Text == "" || CphoneTb.Text == "" || CgenderCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information!");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("update CustomerTable set Cname=@CN,Cphone=@CP,Cgender=@CG where Cnum=@CKey", Con);
                    cmd.Parameters.AddWithValue("@CN", CnameTb.Text);
                    cmd.Parameters.AddWithValue("@CP", CphoneTb.Text);
                    cmd.Parameters.AddWithValue("@CG", CgenderCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@CKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer Updated!");
                    Con.Close();
                    populate();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }

            }
        }
        private void DeleteCustomer()
        {
            if (Key == 0)
            {
                MessageBox.Show("Select a customer!");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("delete form CustomerTable where Cnum = @CKey", Con);
                    cmd.Parameters.AddWithValue("@CKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer Deleted!");
                    Con.Close();
                    populate();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }

            }

        }
        private void InsertCustomer()
        {
            if (CnameTb.Text == "" || CphoneTb.Text == "" || CgenderCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information!");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into CustomerTable(Cname,Cphone,Cgender) values(@CN,@CP,@CG)", Con);
                    cmd.Parameters.AddWithValue("@CN", CnameTb.Text);
                    cmd.Parameters.AddWithValue("@CP", CphoneTb.Text);
                    cmd.Parameters.AddWithValue("@CG", CgenderCb.SelectedItem.ToString());
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer Added!");
                    Con.Close();
                    populate();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }

            }

        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            InsertCustomer();
        }
        int Key = 0;
        private void CustomerDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            CnameTb.Text = CustomerDGV.SelectedRows[0].Cells[0].Value.ToString();
            CphoneTb.Text = CustomerDGV.SelectedRows[0].Cells[1].Value.ToString();
            CgenderCb.Text = CustomerDGV.SelectedRows[0].Cells[2].Value.ToString();
            if (CnameTb.Text == "")
            {
                Key = 0;
            }
            else
            {
                Key = Convert.ToInt32(CustomerDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            DeleteCustomer();
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            EditCustomer();
        }
    }
}
