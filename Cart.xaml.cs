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
    public partial class Cart : Window
    {
        private string connectionString;

        private int User;

        private decimal totalPrice;

        public Cart(int IdUser)
        {
            InitializeComponent();

            connectionString = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=20040607";

            User = IdUser;

            LoadCart();
        }

        public void LoadCart()
        {
            string query = $"SELECT \"Product\", \"IdBasketUser\" FROM public.\"BasketUser\" WHERE \"User\" = {User}";
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
                    int productId = (int)row["Product"];
                    int IdBasketUser = (int)row["IdBasketUser"];
                    byte[] imageData = GetProductImageData(productId);
                    string productName = GetProductName(productId);
                    string productDescription = GetProductDescription(productId);
                    decimal productPrice = GetProductPrice(productId);

                    // Создание элементов в XAML и заполнение данными о товаре
                    StackPanel productPanel = new StackPanel { Orientation = Orientation.Horizontal };
                    
                    StackPanel productInfoPanel = new StackPanel();
                    TextBlock nameTextBlock = new TextBlock { Text = productName, FontSize = 16, FontWeight = FontWeights.Bold };
                    TextBlock descriptionTextBlock = new TextBlock { Text = productDescription, TextWrapping = TextWrapping.Wrap };
                    TextBlock priceTextBlock = new TextBlock { Text = $"Product Price: ${productPrice}" };
                    Button RemoveFromCartButton = new Button { Content = "Удалить из корзины", Tag = IdBasketUser };
                    RemoveFromCartButton.Click += RemoveFromCartClick;

                    productInfoPanel.Children.Add(nameTextBlock);
                    productInfoPanel.Children.Add(descriptionTextBlock);
                    productInfoPanel.Children.Add(priceTextBlock);
                    productInfoPanel.Children.Add(RemoveFromCartButton);

                    ProductStackPanel.Children.Add(productPanel);
                    productPanel.Children.Add(productInfoPanel);

                    totalPrice += productPrice;
                }
            }

            TotalPrice.Text = totalPrice.ToString();
        }

        private byte[] GetProductImageData(int productId)
        {
            string query = $"SELECT \"ImageData\" FROM public.\"Images\" WHERE \"IDProduct\" = {productId}";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                NpgsqlCommand command = new NpgsqlCommand(query, connection);
                connection.Open();
                return (byte[])command.ExecuteScalar();
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

        private string GetProductDescription(int productId)
        {
            string query = $"SELECT \"Description\" FROM public.\"Products\" WHERE \"IdProducts\" = {productId}";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                NpgsqlCommand command = new NpgsqlCommand(query, connection);
                connection.Open();
                return (string)command.ExecuteScalar();
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

        private void RemoveFromCartClick(object sender, RoutedEventArgs e)
        {
            Button RemoveFromCartButton = (Button)sender;
            int IdBasketUser = (int)RemoveFromCartButton.Tag;

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                // Создание команды SQL для удаления данных
                string sql = $"DELETE FROM \"BasketUser\" WHERE \"IdBasketUser\" = {IdBasketUser}";

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    // Выполнение команды удаления
                    command.ExecuteNonQuery();                    
                }
            }
            totalPrice = 0;
            LoadCart();
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
