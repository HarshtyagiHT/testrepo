using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Npgsql;

namespace Milestone1
{
    /// <summary>
    /// Interaction logic for BusinessDetail.xaml
    /// </summary>
    public partial class BusinessDetail : Window
    {
        private string bid = "";
        public BusinessDetail(string bid)
        {
            InitializeComponent();
            this.bid = string.Copy(bid);
            laodBusinessdetail();
            laodBusinessNumberState();
            laodBusinessNumberCity();
        }

        private string buildConnectionString()
        {
            return "Host = localhost; Username = postgres; Database = milestone1db; password = tyagiharsh";
        }

        private void laodBusinessdetail()
        {
            using (var connection = new NpgsqlConnection(buildConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT name, state, city FROM business WHERE business_id = '" + this.bid + "';";
                    try
                    {
                        var reader = cmd.ExecuteReader();
                        reader.Read();
                        setBusinessDetails(reader);
                    }
                    catch (NpgsqlException ex)
                    {
                        Console.WriteLine(ex.Message.ToString());
                        System.Windows.MessageBox.Show("SQL Error - " + ex.Message.ToString());
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
        private void laodBusinessNumberState()
        {
            using (var connection = new NpgsqlConnection(buildConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT count(1) FROM business WHERE state = (SELECT state from business WHERE business_id = '" + this.bid + "');";
                    try
                    {
                        var reader = cmd.ExecuteReader();
                        reader.Read();
                        setNumInState(reader);
                    }
                    catch (NpgsqlException ex)
                    {
                        Console.WriteLine(ex.Message.ToString());
                        System.Windows.MessageBox.Show("SQL Error - " + ex.Message.ToString());
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        private void laodBusinessNumberCity()
        {
            using (var connection = new NpgsqlConnection(buildConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT count(1) FROM business WHERE city = (SELECT city from business WHERE business_id = '" + this.bid + "');";
                    try
                    {
                        var reader = cmd.ExecuteReader();
                        reader.Read();
                        setNumInCity(reader);
                    }
                    catch (NpgsqlException ex)
                    {
                        Console.WriteLine(ex.Message.ToString());
                        System.Windows.MessageBox.Show("SQL Error - " + ex.Message.ToString());
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
        private void setBusinessDetails(NpgsqlDataReader R)
        { 
          bname.Text = R.GetString(0);
          state.Text = R.GetString(1);
          city.Text = R.GetString(2);
        }

        void setNumInCity(NpgsqlDataReader R)
        {
            numInCity.Content = R.GetInt16(0).ToString();
        }

        void setNumInState(NpgsqlDataReader R)
        {
            numInState.Content = R.GetInt16(0).ToString();
        }

    }
}
