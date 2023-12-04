using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
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
    public partial class AddAddress : Window
    {
        private string connectionString;

        private int User;

        private int IdAddress;

        public AddAddress(int IdUser)
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
        }

        private void SaveButton(object sender, RoutedEventArgs e)
        {
            string query = $"UPDATE public.\"Address\" Set \"City\" = @value1, \"Street\" = @value2, \"House\" = @value3, \"Apartment\" = @value4, \"Postcode\" = @value5  WHERE \"IdAddress\" = {IdAddress}";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {

                    // Добавляем параметры к команде (значения, которые вы хотите вставить в таблицу)
                    command.Parameters.AddWithValue("@value1", CytiTextBox.Text);
                    command.Parameters.AddWithValue("@value2", StreetTextBox.Text);
                    command.Parameters.AddWithValue("@value3", HouseTextBox.Text);
                    command.Parameters.AddWithValue("@value4", ApartmentTextBox.Text);
                    command.Parameters.AddWithValue("@value5", PostcodeTextBox.Text);

                    // Выполняем запрос на вставку данных в таблицу
                    command.ExecuteNonQuery();

                    Profile profileWindow = new Profile(User);


                    profileWindow.LoadUserData();

                    this.Close();

                    MessageBox.Show("Адрес изменен!");

                }
            }
        }
    }
}
