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
    /// Логика взаимодействия для AddressInfo.xaml
    /// </summary>
    public partial class AddressInfo : Window
    {
        private string connectionString;

        private int User;

        private int IdAddress;

        public AddressInfo(int IdUser)
        {
            InitializeComponent();

            connectionString = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=20040607";

            User = IdUser;

            string query = $"SELECT \"Address\" FROM public.\"Users\" WHERE \"IdUser\" = {User}";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                NpgsqlCommand command = new NpgsqlCommand(query, connection);
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                foreach (DataRow row in dataTable.Rows)
                {
                    IdAddress = (int)row["Address"];
                }
            }

            LoadDataAddress();
        }

        private void LoadDataAddress()
        {
            string query = $"SELECT * FROM public.\"Address\" WHERE \"IdAddress\" = {IdAddress}";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                NpgsqlCommand command = new NpgsqlCommand(query, connection);
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                foreach (DataRow row in dataTable.Rows)
                {
                    CityTextBox.Text = row["City"].ToString();
                    StreetTextBox.Text = row["Street"].ToString();
                    HouseTextBox.Text = row["House"].ToString();
                    ApartmentTextBox.Text = row["Apartment"].ToString();
                    PostcodeTextBox.Text = row["Postcode"].ToString();
                }
            }
        }

        private void NextButton(object sender, RoutedEventArgs e)
        {
            if (CityTextBox.Text.Length > 0 && StreetTextBox.Text.Length > 0 && HouseTextBox.Text.Length > 0 && ApartmentTextBox.Text.Length > 0 && PostcodeTextBox.Text.Length > 0)
            {
                CardInfo CardInfoWindow = new CardInfo(User);

                CardInfoWindow.Show();

                this.Close();
            }

            else
                BugReport.Visibility = Visibility.Visible;
        }
    }
}

