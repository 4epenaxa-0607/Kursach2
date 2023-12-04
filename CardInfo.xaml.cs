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
    public partial class CardInfo : Window
    {
        private string connectionString;

        private int User;

        private int IdPayment;

        private int productId;
        private decimal productPrice;

        public CardInfo(int IdUser)
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

            LoadDataCard();
        }

        private void LoadDataCard()
        {
            string query = $"SELECT * FROM public.\"PaymentDatas\" WHERE \"IdPaymentData\" = {IdPayment}";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                NpgsqlCommand command = new NpgsqlCommand(query, connection);
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                foreach (DataRow row in dataTable.Rows)
                {
                    numberCardTextBox.Text = row["CardNumber"].ToString();
                    DateCardTextBox.Text = row["Validity"].ToString();
                    CVCCardTextBox.Text = row["CVV"].ToString();
                }
            }
        }

        private void PayButton(object sender, RoutedEventArgs e)
        {
            if (numberCardTextBox.Text.Length > 0 && DateCardTextBox.Text.Length > 0 && CVCCardTextBox.Text.Length > 0)
            {
                string query = $"SELECT \"Product\" FROM public.\"BasketUser\" WHERE \"User\" = {User}";

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    NpgsqlCommand command = new NpgsqlCommand(query, connection);
                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    foreach (DataRow row in dataTable.Rows)
                    {
                        productId = (int)row["Product"];
                        productPrice = GetProductPrice(productId);
                    }

                    query = $"INSERT INTO public.\"Purchases\"(\"Products\", \"Cost\", \"User\", \"Date\") VALUES (@value1, @value2, @value3, @value4)";
                    connection.Open();
                    using (NpgsqlCommand command1 = new NpgsqlCommand(query, connection))
                    {
                        // Добавляем параметры к команде (значения, которые вы хотите вставить в таблицу)
                        command1.Parameters.AddWithValue("@value1", productId);
                        command1.Parameters.AddWithValue("@value2", productPrice);
                        command1.Parameters.AddWithValue("@value3", User);
                        command1.Parameters.AddWithValue("@value4", DateTime.Parse(DateTime.Now.ToShortDateString()));

                        // Выполняем запрос на вставку данных в таблицу
                        command1.ExecuteNonQuery();
                    }
                }


                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Создание команды SQL для удаления данных
                    string sql = $"DELETE FROM \"BasketUser\" WHERE \"User\" = {User}";

                    using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                    {
                        // Выполнение команды удаления
                        command.ExecuteNonQuery();
                    }
                }

                Cart CartWindow = new Cart(User);

                CartWindow.LoadCart();

                this.Close();
                MessageBox.Show("Успешно");
            }

            else
                BugReport.Visibility = Visibility.Visible;
        }

        private decimal GetProductPrice(int productId)
        {
            string query = $"SELECT \"Cost\" FROM public.\"Products\" WHERE \"IdProducts\" = {productId}";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                NpgsqlCommand command = new NpgsqlCommand(query, connection);
                connection.Open();
                return (decimal)command.ExecuteScalar();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            AddressInfo AddressInfoWindow = new AddressInfo(User);

            AddressInfoWindow.Show();

            this.Close();
        }
    }
}
