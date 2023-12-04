using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
    public partial class Registration : Window
    {
        private string connectionString;
        int rowCountBasket;
        int rowCountPaymentDatas;
        int rowCountAddress;

        public Registration()
        {
            InitializeComponent();

            connectionString = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=20040607";
        }

        private void RegistrationButton(object sender, RoutedEventArgs e)
        {
            if (NameTextBox.Text.Length > 0 && SurnameTextBox.Text.Length > 0 && PatronymicTextBox.Text.Length > 0 && PasswordTextBox.Password.Length > 0 && RepeatPasswordTextBox.Password.Length > 0 && LoginTextBox.Text.Length > 0)
            {
                string query = $"SELECT Count(\"IdUser\") FROM public.\"Users\" WHERE \"Name\" = '{NameTextBox.Text}'";


                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        int rowCount = Convert.ToInt32(command.ExecuteScalar());

                        if (rowCount == 0)
                        {
                            if (PasswordTextBox.Password == RepeatPasswordTextBox.Password)
                            {
                                query = $"INSERT INTO public.\"Basket\" DEFAULT VALUES";

                                using (NpgsqlCommand command2 = new NpgsqlCommand(query, connection))
                                { 
                                    command2.ExecuteNonQuery(); 
                                }
                                
                                query = $"INSERT INTO public.\"PaymentDatas\" DEFAULT VALUES";

                                using (NpgsqlCommand command3 = new NpgsqlCommand(query, connection))
                                { 
                                    command3.ExecuteNonQuery(); 
                                }
                                
                                query = $"INSERT INTO public.\"Address\" DEFAULT VALUES";

                                using (NpgsqlCommand command4 = new NpgsqlCommand(query, connection))
                                { 
                                    command4.ExecuteNonQuery(); 
                                }
                                
                                query = $"SELECT Count(\"IdBasket\") FROM public.\"Basket\" ";

                                using (NpgsqlCommand command5 = new NpgsqlCommand(query, connection))
                                {
                                    rowCountBasket = Convert.ToInt32(command5.ExecuteScalar());
                                }
                                
                                query = $"SELECT Count(\"IdPaymentData\") FROM public.\"PaymentDatas\" ";

                                using (NpgsqlCommand command6 = new NpgsqlCommand(query, connection))
                                {
                                    rowCountPaymentDatas = Convert.ToInt32(command6.ExecuteScalar());
                                }
                                
                                query = $"SELECT Count(\"IdAddress\") FROM public.\"Address\"";

                                using (NpgsqlCommand command7 = new NpgsqlCommand(query, connection))
                                {
                                    rowCountAddress = Convert.ToInt32(command7.ExecuteScalar());
                                }

                                query = $"INSERT INTO public.\"Users\"(\"Name\", \"Surname\", \"Patronymic\", \"Login\", \"Password\", \"Basket\", \"PaymentData\", \"Address\" ) VALUES (@value1, @value2, @value3, @value4, @value5, @value6, @value7, @value8)";

                                using (NpgsqlCommand command8 = new NpgsqlCommand(query, connection))
                                {
                                    // Добавляем параметры к команде (значения, которые вы хотите вставить в таблицу)
                                    command8.Parameters.AddWithValue("@value1", NameTextBox.Text);
                                    command8.Parameters.AddWithValue("@value2", SurnameTextBox.Text);
                                    command8.Parameters.AddWithValue("@value3", PatronymicTextBox.Text);
                                    command8.Parameters.AddWithValue("@value4", LoginTextBox.Text);
                                    command8.Parameters.AddWithValue("@value5", PasswordTextBox.Password);
                                    command8.Parameters.AddWithValue("@value6", rowCountBasket + 2);
                                    command8.Parameters.AddWithValue("@value7", rowCountPaymentDatas);
                                    command8.Parameters.AddWithValue("@value8", rowCountAddress);

                                    // Выполняем запрос на вставку данных в таблицу
                                    command8.ExecuteNonQuery();
                                }

                                Message.Text = "Аккаунт создан";
                                Message.Foreground = new SolidColorBrush(Colors.Green);
                                Message.Visibility = Visibility.Visible;
                            }

                            else
                            {
                                Message.Text = "Пароли не совпадают";
                                Message.Visibility = Visibility.Visible;
                            }
                        }

                        else
                        {
                            Message.Text = "Пользователь с таким логином уже есть";
                            Message.Visibility = Visibility.Visible;
                        }
                    }
                }
            }

            else
            {
                Message.Text = "Заполните все поля";
                Message.Visibility = Visibility.Visible;
            }

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Authorization authorizationWindow = new Authorization();


            authorizationWindow.Show();

            this.Close();

           

        }
    }
}