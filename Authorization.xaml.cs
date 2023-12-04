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
    public partial class Authorization : Window
    {
        private string connectionString;

        public Authorization()
        {
            InitializeComponent();
            connectionString = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=20040607";
        }

        private void SingIn(object sender, RoutedEventArgs e)
        {
            

            string query = "SELECT \"IdUser\" FROM public.\"Users\"";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                NpgsqlCommand command = new NpgsqlCommand(query, connection);
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                foreach (DataRow row in dataTable.Rows)
                {
                    int DataId = (int)row["IdUser"];
                    string Login = GetLogin(DataId);
                    string Password = GetPassword(DataId);

                    if (LoginTextBox.Text == Login && PasswordTextBox.Password == Password)
                    {
                        MainWindow mainWindow = new MainWindow(DataId);

                        mainWindow.Show();

                        this.Close();
                    }

                    else
                        Debag.Visibility = Visibility.Visible;
                }
            }
        }

        public string GetLogin(int IdUser)
        {
            string query = $"SELECT \"Login\" FROM public.\"Users\" WHERE \"IdUser\" = {IdUser}";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                NpgsqlCommand command = new NpgsqlCommand(query, connection);
                connection.Open();
                return (string)command.ExecuteScalar();
            }
        }
        
        public string GetPassword(int IdUser)
        {
            string query = $"SELECT \"Password\" FROM public.\"Users\" WHERE \"IdUser\" = {IdUser}";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                NpgsqlCommand command = new NpgsqlCommand(query, connection);
                connection.Open();
                return (string)command.ExecuteScalar();
            }
        }

        private void SingUp(object sender, RoutedEventArgs e)
        {
            Registration registrationWindow = new Registration();


            registrationWindow.Show();

            this.Close();
        }
    }
}
