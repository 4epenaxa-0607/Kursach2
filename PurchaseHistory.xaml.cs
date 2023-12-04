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
    public partial class PurchaseHistory : Window
    {
        private string connectionString;

        private int User;

        public PurchaseHistory(int IdUser)
        {
            InitializeComponent();

            connectionString = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=20040607";

            User = IdUser;

            LoadCart();
        }

        public void LoadCart()
        {
            string query = $"SELECT \"Products\", \"IdPurchases\" FROM public.\"Purchases\" WHERE \"User\" = {User}";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                NpgsqlCommand command = new NpgsqlCommand(query, connection);
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // Очищаем отображение товаров перед добавлением отфильтрованных товаров
                ProductStackPanel.Children.Clear();

                foreach (DataRow row in dataTable.Rows)
                {
                    // Получение данных из строки таблицы
                    int productId = (int)row["Products"];
                    int IdPurchases = (int)row["IdPurchases"];
                    string productName = GetProductName(productId);
                    string productDescription = GetDateOfPurchasen(IdPurchases).ToString("d");
                    decimal productPrice = GetProductPrice(productId);


                    // Создание элементов в XAML и заполнение данными о товаре
                    StackPanel productPanel = new StackPanel { Orientation = Orientation.Horizontal };

                    StackPanel productInfoPanel = new StackPanel();
                    TextBlock nameTextBlock = new TextBlock { Text = productName, FontSize = 16, FontWeight = FontWeights.Bold };
                    TextBlock dateOfPurchaseBlock = new TextBlock { Text = "Дата покупки: " + productDescription, TextWrapping = TextWrapping.Wrap };
                    TextBlock priceTextBlock = new TextBlock { Text = $"Product Price: ${productPrice}" };

                    productInfoPanel.Children.Add(nameTextBlock);
                    productInfoPanel.Children.Add(dateOfPurchaseBlock);
                    productInfoPanel.Children.Add(priceTextBlock);
                    
                    ProductStackPanel.Children.Add(productPanel);
                    productPanel.Children.Add(productInfoPanel);
                }
            }
        }

        private string GetProductName(int productId)
        {
            string query = $"SELECT \"NameProducts\" FROM public.\"Products\" WHERE \"IdProducts\" = {productId}";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                NpgsqlCommand command = new NpgsqlCommand(query, connection);
                connection.Open();
                return (string)command.ExecuteScalar();
            }
        }

        private DateTime GetDateOfPurchasen(int IdPurchases)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                string query = $"SELECT \"Date\" FROM public.\"Purchases\" WHERE \"IdPurchases\" = {IdPurchases} AND \"User\" = {User}"; 
                NpgsqlCommand command = new NpgsqlCommand(query, connection); 
                connection.Open();


                object result = command.ExecuteScalar();
                return Convert.ToDateTime(result);
            }
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

        private void BackButton(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(User);

            mainWindow.Show();

            this.Close();
        }

        private void BuyButton(object sender, RoutedEventArgs e)
        {
            AddressInfo AddressInfoWindow = new AddressInfo(User);

            AddressInfoWindow.Show();
        }
    }
}
