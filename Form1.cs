using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace AppManagement
{
    public partial class Form1 : Form
    {
        OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Admin\OneDrive\Documents\10012884\Database51.accdb");
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (comboBoxId.SelectedItem == null)
            {
                MessageBox.Show("Please select an item from the dropdown");
                return;  
            }

            double compensations = double.Parse(txtBoxWork.Text) * 300;
            txtBoxCompe.Text = compensations.ToString();
            string selecteed = comboBoxId.SelectedItem.ToString();
            DateTime currentDate = DateTime.Now;
            string dateString = currentDate.ToString("MM/dd/yyyy");
            
            MessageBox.Show("Record submite", "good!");
            using (OleDbCommand cmd = con.CreateCommand())
            {
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "Insert into Table1 ([National Id], [First Name], [Other Name], [Gender], [Work Hours], [Compensation], [Date]) Values('" + txtBoxNational.Text + "','" + txtBoxFirst.Text + "','" + txtBoxOther.Text + "','" + selecteed + "','" + txtBoxWork.Text + "','" + compensations + "','" + dateString + "')";
                cmd.Parameters.Add(new OleDbParameter("national", txtBoxNational.Text));
                cmd.Parameters.Add(new OleDbParameter("first", txtBoxFirst.Text));
                cmd.Parameters.Add(new OleDbParameter("other", txtBoxOther.Text));
                cmd.Parameters.Add(new OleDbParameter("selected", selecteed));
                cmd.Parameters.Add(new OleDbParameter("work", txtBoxWork.Text));
                cmd.Parameters.Add(new OleDbParameter("compensations", compensations));
                cmd.Parameters.Add(new OleDbParameter("Date", dateString));
                cmd.ExecuteNonQuery();
                con.Close();
            }
            MessageBox.Show("Record submitted", "Good!");
        }
        private void loaddata()
        {
            string connString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Admin\OneDrive\Documents\10012884\Database51.accdb";
            DataTable dataTableRes = new DataTable();

            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                OleDbCommand cmd = new OleDbCommand("SELECT * FROM Table1", conn);

                conn.Open();

                OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);

                adapter.Fill(dataTableRes);
            }

            dataGridView1.DataSource = dataTableRes;
            dataGridView1.RowsDefaultCellStyle.BackColor = Color.White;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(0, 255, 0);

        }

        private void btnView_Click(object sender, EventArgs e)
        {
            loaddata();
            dataGridView1.Sort(dataGridView1.Columns["National Id"], ListSortDirection.Ascending);


        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string connString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Admin\OneDrive\Documents\10012884\Database51.accdb";
            if (dataGridView1.SelectedCells.Count > 0)
            {

                if (MessageBox.Show(string.Format("Do you want to delete Table1 registration: {0}?", txtBoxNational.Text.ToString()), "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    using (OleDbConnection conn = new OleDbConnection(connString))
                    {
                        using (OleDbCommand cmd = new OleDbCommand("DELETE FROM Table1  WHERE [National Id]=@national", conn))
                        {
                            cmd.CommandType = CommandType.Text;

                            cmd.Parameters.AddWithValue("@national", txtBoxNational.Text);

                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                            MessageBox.Show("record deleted successfully");
                            loaddata();
                        }
                    }


                }
            }

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string connString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Admin\OneDrive\Documents\10012884\Database51.accdb";
            DateTime currentDate = DateTime.Now;
            string dateString = currentDate.ToString("MM/dd/yyyy");
            string selecteed = comboBoxId.SelectedItem.ToString();
            double compensations = double.Parse(txtBoxWork.Text) * 300;
            string compe = compensations.ToString();
            if (dataGridView1.SelectedCells.Count > 0)
            {

                if (MessageBox.Show(string.Format("Do you want to update student registration: {0}?", txtBoxNational.Text.ToString()), "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    using (OleDbConnection conn = new OleDbConnection(connString))
                    {
                        using (OleDbCommand cmd = new OleDbCommand("update Table1 set [National Id]=@national, [First Name]=@first, [Other Name]=@other, [Gender]=@gender, [Work Hours]=@work, [Compensation]=@compensations, [Date]=@date WHERE [National Id] = @national", conn))
                        {

                            cmd.CommandType = CommandType.Text;

                            cmd.Parameters.AddWithValue("@national", txtBoxNational.Text);
                            cmd.Parameters.AddWithValue("@first", txtBoxFirst.Text);
                            cmd.Parameters.AddWithValue("@other", txtBoxOther.Text);
                            cmd.Parameters.AddWithValue("@gender", selecteed);
                            cmd.Parameters.AddWithValue("@work", txtBoxWork.Text);
                            cmd.Parameters.AddWithValue("@compensations", compe);
                            cmd.Parameters.AddWithValue("@date", dateString);
                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                            MessageBox.Show("record Updated successfully");
                            loaddata();

                        }
                    }
                }
            }

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string connString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Admin\OneDrive\Documents\10012884\Database51.accdb";
            DataTable dataTableRes = new DataTable();
            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                OleDbCommand cmd = new OleDbCommand("SELECT * FROM Table1 where regno=@reg", conn);

                conn.Open();
                cmd.Parameters.AddWithValue("@national", txtBoxSearch.Text);
                OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);

                adapter.Fill(dataTableRes);
            }

            dataGridView1.DataSource = dataTableRes;

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dataGridView1.Refresh();
            loaddata();
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                txtBoxNational.Text = row.Cells[0].Value.ToString();
                txtBoxFirst.Text = row.Cells[1].Value.ToString();
                txtBoxOther.Text = row.Cells[2].Value.ToString();
                comboBoxId.Text = row.Cells[3].Value.ToString();
                txtBoxWork.Text = row.Cells[4].Value.ToString();
                txtBoxCompe.Text = row.Cells[4].Value.ToString();

            }


        }
        public void cleartextboxes(Control.ControlCollection ctlcollection)
        {
            foreach (Control txt in ctlcollection)
            {
                if (txt is TextBoxBase)
                {
                    txt.Text = string.Empty;
                }
                else
                {
                    cleartextboxes(txt.Controls);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            cleartextboxes(this.Controls);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            txtBoxNational.Clear();
            txtBoxFirst.Clear();
            txtBoxOther.Clear();            
            txtBoxWork.Clear();
            txtBoxCompe.Clear();
        }

        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            
        }
    }
}

