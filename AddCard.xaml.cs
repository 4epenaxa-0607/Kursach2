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
    public partial class AddCard : Window
    {

        private string connectionString;

        private int User;

        private int IdPayment;

        public AddCard(int IdUser)
        {
            InitializeComponent();

            connectionString = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=20040607";

            User = IdUser;

            string query = $"SELECT \"PaymentData\" FROM public.\"Users\" WHERE \"IdUser\" = {User}";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                NpgsqlCommand command = new NpgsqlCommand(query, connection);
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                foreach (DataRow row in dataTable.Rows)
                {
                    IdPayment = (int)row["PaymentData"];
                }
            }
        }

        private void SaveButton(object sender, RoutedEventArgs e)
        {
            string query = $"UPDATE public.\"PaymentDatas\" Set \"CardNumber\" = @value1, \"Validity\" = @value2, \"CVV\" = @value3  WHERE \"IdPaymentData\" = {IdPayment}";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {

                    // Добавляем параметры к команде (значения, которые вы хотите вставить в таблицу)
                    command.Parameters.AddWithValue("@value1", numberCardTextBox.Text);
                    command.Parameters.AddWithValue("@value2", DateCardTextBox.Text);
                    command.Parameters.AddWithValue("@value3", CVCCardTextBox.Text);

                    // Выполняем запрос на вставку данных в таблицу
                    command.ExecuteNonQuery();

                    Profile profileWindow = new Profile(User);


                    profileWindow.LoadUserData();

                    this.Close();

                    MessageBox.Show("Карта изменена!");

                }
            }
        }
    }
}
