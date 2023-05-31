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
    public partial class Bookings : Form
    {
        public Bookings()
        {
            InitializeComponent();
            populate();
            GetRooms();
            GetCustomers();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\ABANS\Documents\HotelDB.mdf;Integrated Security=True;Connect Timeout=30");
        private void populate()
        {
            Con.Open();
            string Query = "select * from BookingsTable";
            SqlDataAdapter sda = new SqlDataAdapter(Query, Con);
            SqlCommandBuilder Builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            BookingDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void GetRooms()
        {
            Con.Open();
            SqlCommand cmd = new SqlCommand("select * from RoomsTable where Rstatus='Available'", Con);
            SqlDataReader rdr;
            rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("Rnum", typeof(int));
            dt.Load(rdr);
            RoomCb.ValueMember = "Rnum";
            RoomCb.DataSource = dt;
            Con.Close();
        }
        int Price = 1;
        private void fetchCost()
        {
            Con.Open();
            string Query = "select Typecost from RoomsTable join TypeTable on Rtype= Typenum where Rnum=" + RoomCb.SelectedValue.ToString() + "";
            SqlCommand cmd = new SqlCommand(Query, Con);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            foreach(DataRow dr in dt.Rows)
            {
                Price = Convert.ToInt32(dr["Typecost"].ToString());
            }
            Con.Close();
        }
        private void GetCustomers()
        {
            Con.Open();
            SqlCommand cmd = new SqlCommand("select * from CustomerTable", Con);
            SqlDataReader rdr;
            rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("Cnum", typeof(int));
            dt.Load(rdr);
            CustomerCb.ValueMember = "Cnum";
            CustomerCb.DataSource = dt;
            Con.Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (CustomerCb.SelectedIndex == -1 || RoomCb.SelectedIndex == -1 || AmountTb.Text == "" || DurationTb.Text=="")
            {
                MessageBox.Show("Missing Information!");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into BookingsTable(Room,Customer,BookDate,Duration,Cost) values(@R,@C,@BD,@D,@Cost)", Con);
                    cmd.Parameters.AddWithValue("@R", RoomCb.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@C", CustomerCb.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@BD", Bdate.Value.Date);
                    cmd.Parameters.AddWithValue("@D", DurationTb.Text);
                    cmd.Parameters.AddWithValue("@D", AmountTb.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Room Booked!");
                    Con.Close();
                    populate();
                    SetBooked();
                    GetRooms();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }

            }
        }

        private void RoomCb_SelectedIndexChanged(object sender, EventArgs e)
        {
            fetchCost();
        }

        private void DurationTb_TextChanged(object sender, EventArgs e)
        {
            if(AmountTb.Text=="")
            {
                AmountTb.Text = "Rs 0";
            }
            else
            {
                int Total = Price * Convert.ToInt32(DurationTb.Text);
                AmountTb.Text = "Rs" + Total;
            }
            
        }
        int Key = 0;
        private void CancelBooking()
        {
            if (Key == 0)
            {
                MessageBox.Show("Select a Booking!");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("delete form BookingsTable where Booknum = @BKey", Con);
                    cmd.Parameters.AddWithValue("@BKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Booking Cancelled!");
                    Con.Close();
                    populate();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }

            }

        }
        private void CancelBtn_Click(object sender, EventArgs e)
        {
            CancelBooking();
            SetAvailable();
            GetRooms();
        }
        private void SetBooked()
        {
            try
            {
                Con.Open();
                SqlCommand cmd = new SqlCommand("update RoomsTable set Rstatus=@RS where Rnum=@RKey", Con);
                cmd.Parameters.AddWithValue("@RS", "Booked");
                cmd.Parameters.AddWithValue("@RKey", RoomCb.SelectedValue.ToString());
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
        private void SetAvailable()
        {
            try
            {
                Con.Open();
                SqlCommand cmd = new SqlCommand("update RoomsTable set Rstatus=@RS where Rnum=@RKey", Con);
                cmd.Parameters.AddWithValue("@RS", "Available");
                cmd.Parameters.AddWithValue("@RKey", RoomCb.Text);
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
        private void BookingDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            RoomCb.Text = BookingDGV.SelectedRows[0].Cells[0].Value.ToString();
            CustomerCb.Text = BookingDGV.SelectedRows[0].Cells[1].Value.ToString();
            Bdate.Text = BookingDGV.SelectedRows[0].Cells[2].Value.ToString();
            DurationTb.Text = BookingDGV.SelectedRows[0].Cells[3].Value.ToString();
            AmountTb.Text = BookingDGV.SelectedRows[0].Cells[4].Value.ToString();
            if (AmountTb.Text == "")
            {
                Key = 0;
            }
            else
            {
                Key = Convert.ToInt32(BookingDGV.SelectedRows[0].Cells[0].Value.ToString());
            }
        }
    }
}
