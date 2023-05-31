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
    public partial class Types : Form
    {
        public Types()
        {
            InitializeComponent();
            populate();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\ABANS\Documents\HotelDB.mdf;Integrated Security=True;Connect Timeout=30");
        private void populate()
        {
            Con.Open();
            string Query = "select * from TypeTable";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            TypesDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void InsertCategories()
        {
            if (TypeNameTb.Text == "" || CostTb.Text == "")
            {
                MessageBox.Show("Missing Information!");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into TypeTable(Typename,Typecost) values(@TN,@TC)", Con);
                    cmd.Parameters.AddWithValue("@TN", TypeNameTb.Text);
                    cmd.Parameters.AddWithValue("@TC", CostTb.Text);    
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Category Inserted!!");
                    Con.Close();
                    populate();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }

            }

        }
        private void EditCategories()
        {
            if (TypeNameTb.Text == "" || CostTb.Text == "")
            {
                MessageBox.Show("Missing Information!");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("update TypeTable set Typename=@TN,Typecost=@TC where Typenum=@TKey", Con);
                    cmd.Parameters.AddWithValue("@TN", TypeNameTb.Text);
                    cmd.Parameters.AddWithValue("@TC", CostTb.Text);
                    cmd.Parameters.AddWithValue("@TKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Category Updated!!");
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
            InsertCategories();
        }

        private void TypesDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {
            Rooms obj = new Rooms();
            obj.Show();
            this.Hide();
        }
        int Key = 0;
        private void TypesDGV_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            TypeNameTb.Text = TypesDGV.SelectedRows[0].Cells[0].Value.ToString();
            CostTb.Text = TypesDGV.SelectedRows[0].Cells[1].Value.ToString();
            if (TypeNameTb.Text == "")
            {
                Key = 0;
            }
            else
            {
                Key = Convert.ToInt32(TypesDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            EditCategories();
        }
        private void DeleteCategories()
        {
            if (Key == 0)
            {
                MessageBox.Show("Select a Category!");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("delete form TypeTable where Typenum = @TKey", Con);
                    cmd.Parameters.AddWithValue("@TKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Category Deleted!");
                    Con.Close();
                    populate();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }

            }

        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            DeleteCategories();
        }

        private void Types_Load(object sender, EventArgs e)
        {
           

        }
    }
}
