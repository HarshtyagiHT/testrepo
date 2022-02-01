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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Npgsql;

namespace Milestone1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class Business
        {
            public string bid { get; set; }
            public string name { get; set; }
            public string state { get; set; }
            public string city { get; set; }
        }
        public MainWindow()
        {
            InitializeComponent();
            addState();
            addColumns2grid();
        }
        private string buildConnectionString()
        {
            return "Host = localhost; Username = postgres; Database = milestone1db; password = tyagiharsh";
        }

        private void addState()
        {
            using (var connection = new NpgsqlConnection(buildConnectionString()))
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT distinct state FROM business ORDER BY state";
                    try
                    {
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                            stateList.Items.Add(reader.GetString(0));
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

        private void addColumns2grid()
        {
            DataGridTextColumn col1 = new DataGridTextColumn();
            col1.Binding = new Binding("name");
            col1.Header = "BusinessName";
            col1.Width = 255;
            businessList.Columns.Add(col1);

            DataGridTextColumn col2 = new DataGridTextColumn();
            col2.Binding = new Binding("state");
            col2.Header = "State";
            col2.Width = 60;
            businessList.Columns.Add(col2);

            DataGridTextColumn col3 = new DataGridTextColumn();
            col3.Binding = new Binding("city");
            col3.Header = "City";
            col3.Width = 150;
            businessList.Columns.Add(col3);

            DataGridTextColumn col4 = new DataGridTextColumn();
            col4.Binding = new Binding("bid");
            col4.Header = "";
            col4.Width = 0;
            businessList.Columns.Add(col4);


        }

        private void stateList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cityList.Items.Clear();
            if (stateList.SelectedIndex > -1)
            {
                using (var connection = new NpgsqlConnection(buildConnectionString()))
                {
                    connection.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandText = "SELECT distinct city FROM business Where state = '" + stateList.SelectedItem.ToString() + "' ORDER BY city";
                        try
                        {
                            var reader = cmd.ExecuteReader();
                            while (reader.Read())
                                cityList.Items.Add(reader.GetString(0));
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
        }

        private void cityList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            businessList.Items.Clear();
            if (cityList.SelectedIndex > -1)
            {
                using (var connection = new NpgsqlConnection(buildConnectionString()))
                {
                    connection.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandText = "SELECT name,state,city,business_id FROM business Where state = '" + stateList.SelectedItem.ToString() + "' AND city = '" + cityList.SelectedItem.ToString() + "'ORDER BY city;";
                        try
                        {
                            var reader = cmd.ExecuteReader();
                            while (reader.Read())
                                businessList.Items.Add(new Business { name = reader.GetString(0), city = reader.GetString(1), state = reader.GetString(2), bid = reader.GetString(3) });
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

        }

        private void businessList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(businessList.SelectedIndex > -1)
            {
                Business B = businessList.Items[businessList.SelectedIndex] as Business;
                if ((B.bid != null) && (B.bid.ToString().CompareTo("") != 0))
                {
                    BusinessDetail businessWindow = new BusinessDetail(B.bid.ToString());
                    businessWindow.Show();
                }


            }

        }
    }
}
