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

namespace Hotel_Management
{
    public partial class Rooms : Form
    {
        public Rooms()
        {
            InitializeComponent();
            populate();
            GetCategories();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\ABANS\Documents\HotelDB.mdf;Integrated Security=True;Connect Timeout=30");
        private void populate()
        {
            Con.Open();
            string Query = "select * from RoomsTable";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            RoomDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
       
        private void EditRooms()
        {
            if (RnameTb.Text == "" || RtypeCb.SelectedIndex == -1 || StatusCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information!");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("update RoomsTable set Rname=@RN,Rtype=@RT,Rstatus=@RS where Rnum=@RKey", Con);
                    cmd.Parameters.AddWithValue("@RN", RnameTb.Text);
                    cmd.Parameters.AddWithValue("@RT", RtypeCb.SelectedIndex.ToString());
                    cmd.Parameters.AddWithValue("@RS", StatusCb.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@RKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Room Updated!");
                    Con.Close();
                    populate();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }

            }
        }
        private void DeleteRooms()
        {
            if (Key==0)
            {
                MessageBox.Show("Select a Room!");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("delete form RoomTable where Rnum = @RKey", Con);
                    cmd.Parameters.AddWithValue("@RKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Room Deleted!");
                    Con.Close();
                    populate();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }

            }

        }
        private void InsertRooms()
        {
            if(RnameTb.Text =="" || RtypeCb.SelectedIndex == -1 || StatusCb.SelectedIndex == -1)
            {
                MessageBox.Show("Missing Information!");
            }else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into RoomsTable(Rname,Rtype,Rstatus) values(@RN,@RT,@RS)", Con);
                    cmd.Parameters.AddWithValue("@RN", RnameTb.Text);
                    cmd.Parameters.AddWithValue("@RT", RtypeCb.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@RS", "Available");
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Room Added!");
                    Con.Close();
                    populate();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }

            }

        }
        private void GetCategories()
        {
            Con.Open();
            SqlCommand cmd = new SqlCommand("select * from TypeTable", Con);
            SqlDataReader rdr;
            rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("Typenum",typeof(int));
            dt.Load(rdr);
            RtypeCb.ValueMember = "Typenum";
            RtypeCb.DataSource = dt;
            Con.Close();
        }
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            InsertRooms();
        }
        int Key = 0;
        private void RoomDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            RnameTb.Text = RoomDGV.SelectedRows[0].Cells[0].Value.ToString();
            RtypeCb.Text = RoomDGV.SelectedRows[0].Cells[1].Value.ToString();
            StatusCb.Text = RoomDGV.SelectedRows[0].Cells[3].Value.ToString();
            if(RnameTb.Text == "")
            {
                Key = 0;
            }
            else
            {
                Key = Convert.ToInt32(RoomDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            EditRooms();
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            DeleteRooms();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Types obj = new Types();
            obj.Show();
            this.Hide();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            
        }
        private void panel3_MouseClick(object sender, PaintEventArgs e)
        {
            Login obj = new Login();
            obj.Show();
            this.Hide();
        }
    }
}
