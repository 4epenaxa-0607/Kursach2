using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace Kursach2
{
    /// <summary>
    /// Логика взаимодействия для Profile.xaml
    /// </summary>
    public partial class Profile : Window
    {
        private string connectionString;

        private int User;

        public Profile(int IdUser)
        {
            InitializeComponent();

            connectionString = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=20040607";

            User = IdUser;

            LoadUserData();
        }

        public void LoadUserData()
        {
            string query = $"SELECT * FROM public.\"Users\" WHERE \"IdUser\" = {User}";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                NpgsqlCommand command = new NpgsqlCommand(query, connection);
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                foreach (DataRow row in dataTable.Rows)
                {
                    NameTextBox.Text = row["Name"].ToString();
                    SurnameTextBox.Text = row["Surname"].ToString();
                    PatronymicTextBox.Text = row["Patronymic"].ToString();
                    NumberPhoneTextBox.Text = row["PhoneNumbe"].ToString();
                }

                query = $"SELECT * FROM public.\"Address\" INNER JOIN \"Users\" ON \"Users\".\"Address\" = \"Address\".\"IdAddress\" WHERE \"Users\".\"IdUser\" = {User}";

                command = new NpgsqlCommand(query, connection);
                adapter = new NpgsqlDataAdapter(command);
                dataTable = new DataTable();
                adapter.Fill(dataTable);

                foreach (DataRow row in dataTable.Rows)
                {
                    AddressTextBox.Text = row["City"].ToString() + ", " + row["Street"].ToString() + ", " + row["House"].ToString() + ", " + row["Apartment"].ToString() + ", " + row["Postcode"].ToString();
                }

                query = $"SELECT \"CardNumber\" FROM public.\"PaymentDatas\" INNER JOIN \"Users\" ON \"Users\".\"PaymentData\" = \"PaymentDatas\".\"IdPaymentData\" WHERE \"Users\".\"IdUser\" = {User}";

                command = new NpgsqlCommand(query, connection);
                adapter = new NpgsqlDataAdapter(command);
                dataTable = new DataTable();
                adapter.Fill(dataTable);

                foreach (DataRow row in dataTable.Rows)
                {
                    numberCardTextBox.Text = row["CardNumber"].ToString();
                }
            }
        }

        private void BackButton(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(User);

            mainWindow.Show();

            this.Close();
        }

        private void ChangeCardDataButton(object sender, RoutedEventArgs e)
        {
            AddCard AddCardWindow = new AddCard(User);

            AddCardWindow.Show();
        }

        private void ChangeAddressButton(object sender, RoutedEventArgs e)
        {
            AddAddress AddAddressWindow = new AddAddress(User);

            AddAddressWindow.Show();
        }
    }
}
