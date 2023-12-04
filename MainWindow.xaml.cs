using System;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Npgsql;

namespace Kursach2
{
    public partial class MainWindow : Window
    {
        private string connectionString;

        private int User;

        public MainWindow(int IdUser)
        {
            InitializeComponent();
            connectionString = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=20040607";

            User = IdUser;

            LoadProductData();

            string query = "SELECT \"Name\" FROM public.\"Category\"";

            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string data = reader.GetString(0);
                            CategoryComboBox.Items.Add(data);
                        }
                    }
                }
            }
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            string filterText = FilterTextBox.Text;
            
            if (filterText != "")
            {
                FilterNameProductData(filterText);
            }

            else
                LoadProductData();
        }

        private void LoadProductData()
        {
            string query = "SELECT \"IdProducts\" FROM public.\"Products\"";
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
                    int productId = (int)row["IdProducts"];
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
                    Button addToCartButton = new Button { Content = "Добавить в корзину", Tag = productId };
                    addToCartButton.Click += AddToCartClick;

                    productInfoPanel.Children.Add(nameTextBlock);
                    productInfoPanel.Children.Add(descriptionTextBlock);
                    productInfoPanel.Children.Add(priceTextBlock);
                    productInfoPanel.Children.Add(addToCartButton);

                    ProductStackPanel.Children.Add(productPanel);
                    productPanel.Children.Add(productInfoPanel);
                }
            }
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

        private void AddToCartClick(object sender, RoutedEventArgs e)
        {
            Button addToCartButton = (Button)sender;
            int productId = (int)addToCartButton.Tag;
            int IdBasket;
            // Логика добавления товара в корзину по идентификатору товара

            string query = $"SELECT \"Basket\" FROM public.\"Users\" WHERE \"IdUser\" = {User}";

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                NpgsqlCommand command = new NpgsqlCommand(query, connection);
                connection.Open();
                IdBasket = Convert.ToInt32(command.ExecuteScalar());
                
                query = $"INSERT INTO \"BasketUser\"(\"Basket\", \"User\", \"Product\") VALUES (@value1, @value2, @value3)";

                using (command = new NpgsqlCommand(query, connection))
                {
                    // Добавляем параметры к команде (значения, которые вы хотите вставить в таблицу)
                    command.Parameters.AddWithValue("@value1", IdBasket);
                    command.Parameters.AddWithValue("@value2", User);
                    command.Parameters.AddWithValue("@value3", productId);

                    // Выполняем запрос на вставку данных в таблицу
                    command.ExecuteNonQuery();
                }
            }           
        }

        private void EditQuantityClick(object sender, RoutedEventArgs e)
        {
            // Логика изменения количества товара в корзине
        }

        private void FilterNameProductData(string filterText)
        {
            string query = $"SELECT \"IdProducts\" FROM public.\"Products\" WHERE \"NameProducts\" = '{filterText}'";
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
                    int productId = (int)row["IdProducts"];
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
                    Button addToCartButton = new Button { Content = "Добавить в корзину", Tag = productId };
                    addToCartButton.Click += AddToCartClick;

                    productInfoPanel.Children.Add(nameTextBlock);
                    productInfoPanel.Children.Add(descriptionTextBlock);
                    productInfoPanel.Children.Add(priceTextBlock);
                    productInfoPanel.Children.Add(addToCartButton);

                    ProductStackPanel.Children.Add(productPanel);
                    productPanel.Children.Add(productInfoPanel);
                }
            }
        }

        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filterText = FilterTextBox.Text;
            if (filterText != "")
            {
                FilterNameProductData(filterText);
            }

            else
                LoadProductData();
        }

        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string CategoryText = CategoryComboBox.SelectedItem.ToString();

            if (CategoryText != "" && CategoryText != "Нет")
            {
                FilterCategoryProductData(CategoryText);
            }

            else
                LoadProductData();
        }

        private void FilterCategoryProductData(string CategoryText)
        {
            string query = $"SELECT \"Products\".\"IdProducts\", \"Category\".\"IdCategory\" FROM \"Category\" INNER JOIN \"Products\" ON \"Category\".\"IdCategory\" = \"Products\".\"Category\" WHERE \"Name\" = '{CategoryText}'";
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
                    int productId = (int)row["IdProducts"];
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
                    Button addToCartButton = new Button { Content = "Добавить в корзину", Tag = productId };
                    addToCartButton.Click += AddToCartClick;

                    productInfoPanel.Children.Add(nameTextBlock);
                    productInfoPanel.Children.Add(descriptionTextBlock);
                    productInfoPanel.Children.Add(priceTextBlock);
                    productInfoPanel.Children.Add(addToCartButton);

                    ProductStackPanel.Children.Add(productPanel);
                    productPanel.Children.Add(productInfoPanel);
                }
            }
        }

        private void MenuButton(object sender, RoutedEventArgs e)
        {
            if (MenuPanel.Visibility == Visibility.Hidden)
                MenuPanel.Visibility = Visibility.Visible;

            else
                MenuPanel.Visibility = Visibility.Hidden;
        }

        private void GoToCart(object sender, RoutedEventArgs e)
        {
            Cart cartWindow = new Cart(User);


            cartWindow.Show();

            this.Close();
        }
        
        private void GoToProfile(object sender, RoutedEventArgs e)
        {
            Profile profileWindow = new Profile(User);


            profileWindow.Show();

            this.Close();
        }
        
        private void ExitFromProfile(object sender, RoutedEventArgs e)
        {
            Authorization authorizationWindow = new Authorization();


            authorizationWindow.Show();

            this.Close();
        }

        private void PurchaseHistoryButton(object sender, RoutedEventArgs e)
        {
            PurchaseHistory purchaseHistoryWindow = new PurchaseHistory(User);

            purchaseHistoryWindow.Show();

            this.Close();
        }
    }
}