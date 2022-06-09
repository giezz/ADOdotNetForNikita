using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace ADOdotNetForNikita
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private const string Server = "localhost";
        private const string Port = "5432";
        private const string UserId = "postgres";
        private const string Password = "admin";
        private const string Database = "delivery";

        private string connectionString =
            $"Server={Server};Port={Port};User Id={UserId};Password={Password};Database={Database};";

        private int rowIndex = -1;

        private void Form1_Load(object sender, EventArgs e)
        {
            GetAllDeliveries();
        }

        private void GetAllDeliveries()
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    NpgsqlCommand commandSelect = new NpgsqlCommand(
                        "SELECT * FROM delivery ORDER BY number_of_delivery DESC", connection);
                    DataTable dataTable = new DataTable();
                    dataTable.Load(commandSelect.ExecuteReader());
                    connection.Close();
                    dataGridView1.DataSource = dataTable;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    connection.Close();
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void GetAllDeliveries(string sql)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    NpgsqlCommand commandSelect = new NpgsqlCommand(sql, connection);
                    DataTable dataTable = new DataTable();
                    dataTable.Load(commandSelect.ExecuteReader());
                    connection.Close();
                    dataGridView1.DataSource = dataTable;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    connection.Close();
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sql =
                        $"INSERT INTO delivery VALUES (DEFAULT, {Int32.Parse(priceOfDelivery.Text)}, '{addressOfDelivery.Text}', '{descriptionOfDelivery.Text}')";
                    NpgsqlCommand commandInsert = new NpgsqlCommand(sql, connection);
                    commandInsert.ExecuteNonQuery();
                    connection.Close();
                    GetAllDeliveries();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.StackTrace);
                }
                finally
                {
                    connection.Close();
                }
            }

            priceOfDelivery.Text = null;
            addressOfDelivery.Text = null;
            descriptionOfDelivery.Text = null;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            rowIndex = e.RowIndex;
            priceOfDelivery.Text = dataGridView1.Rows[e.RowIndex].Cells["price_of_delivery"].Value.ToString();
            addressOfDelivery.Text = dataGridView1.Rows[e.RowIndex].Cells["address_of_delivery"].Value.ToString();
            descriptionOfDelivery.Text =
                dataGridView1.Rows[e.RowIndex].Cells["description_of_delivery"].Value.ToString();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sql =
                        $"DELETE FROM delivery WHERE number_of_delivery = {int.Parse(dataGridView1.Rows[rowIndex].Cells["number_of_delivery"].Value.ToString())}";
                    NpgsqlCommand commandDelete = new NpgsqlCommand(sql, connection);
                    commandDelete.ExecuteNonQuery();
                    connection.Close();
                    GetAllDeliveries();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sql = "UPDATE delivery SET " +
                                 $"price_of_delivery = {Int32.Parse(priceOfDelivery.Text)}, " +
                                 $"address_of_delivery = '{addressOfDelivery.Text}', " +
                                 $"description_of_delivery = '{descriptionOfDelivery.Text}' " +
                                 $"WHERE number_of_delivery = {dataGridView1.Rows[rowIndex].Cells["number_of_delivery"].Value}";
                    NpgsqlCommand commandUpdate = new NpgsqlCommand(sql, connection);
                    commandUpdate.ExecuteNonQuery();
                    connection.Close();
                    GetAllDeliveries();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.StackTrace);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            string sql = "SELECT * FROM delivery WHERE ";
            if (!string.IsNullOrEmpty(numberFilter.Text))
                sql += "(CAST(number_of_delivery AS TEXT) LIKE '" + Int32.Parse(numberFilter.Text) + "') AND ";
            else
                sql += "(CAST(number_of_delivery AS TEXT) LIKE '%') AND ";
            if (!string.IsNullOrEmpty(priceFilter.Text))
                sql += "(CAST(price_of_delivery AS TEXT) LIKE '" + Int32.Parse(priceFilter.Text) + "') AND ";
            else
                sql += "(CAST(price_of_delivery AS TEXT) LIKE '%') AND ";
            if (!string.IsNullOrEmpty(addressFilter.Text))
                sql += "(address_of_delivery LIKE '" + addressFilter.Text + "') AND ";
            else
                sql += "(address_of_delivery LIKE '%') AND ";
            if (!string.IsNullOrEmpty(descFilter.Text))
                sql += "(description_of_delivery LIKE '" + descFilter.Text + "')";
            else
                sql += "(description_of_delivery LIKE '%')";

            GetAllDeliveries(sql);

            numberFilter.Text = null;
            priceFilter.Text = null;
            addressFilter.Text = null;
            descFilter.Text = null;
        }
    }
}