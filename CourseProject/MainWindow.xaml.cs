using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Security.Cryptography;

namespace CourseProject
{
    public partial class MainWindow : Window
    {
        public string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Button_Click_Registration(object sender, RoutedEventArgs e)
        {
            WindowRegistration windowRegistration = new WindowRegistration();
            this.Close();
            windowRegistration.Show();
        }
        private void Button_Click_Log_in(object sender, RoutedEventArgs e)
        {
            string loginToCheck = userLoginToCheck.Text;
            string passwordToCheck = userPasswordToCheck.Password;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT ID, Password FROM Users WHERE Login=@Login";
                    int userId = -1;
                    string hashedPasswordFromDatabase = null;

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Login", loginToCheck);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                userId = reader.GetInt32(0);
                                hashedPasswordFromDatabase = reader.GetString(1);
                            }
                        }
                    }

          
                    if (userId != -1 && hashedPasswordFromDatabase != null)
                    {
                        string hashedPasswordToCheck;
                        using (SHA256 sha256 = SHA256.Create())
                        {
                            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(passwordToCheck));
                            hashedPasswordToCheck = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                        }

                        if (hashedPasswordToCheck == hashedPasswordFromDatabase)
                        {
                            string query1 = "SELECT FirstName, LastName, Email, Login, Password, DateOfBirth, Country FROM Users WHERE ID=@UserId";

                            UserViewModel userViewModel = new UserViewModel { CurrentUserId = userId }; 
                            using (SqlCommand command1 = new SqlCommand(query1, connection))
                            {
                                command1.Parameters.AddWithValue("@UserId", userId);

                                using (SqlDataReader reader1 = command1.ExecuteReader())
                                {
                                    if (reader1.Read())
                                    {
                                        userViewModel.User = new User
                                        {
                                            Id = userId,
                                            FirstName = reader1["FirstName"].ToString(),
                                            LastName = reader1["LastName"].ToString(),
                                            Email = reader1["Email"].ToString(),
                                            Login = reader1["Login"].ToString(),
                                            Password = reader1["Password"].ToString(),
                                            DateOfBirth = Convert.ToDateTime(reader1["DateOfBirth"]),
                                            Country = reader1["Country"].ToString()
                                        };
                                    }
                                    else
                                    {
                                        throw new Exception("Ошибка при получении данных пользователя.");
                                    }
                                }
                            }

                            // Передаем userViewModel в основное окно
                            WindowMainMenu windowMainMenu = new WindowMainMenu(userViewModel);
                            windowMainMenu.Show();
                            this.Close();
                        }
                        else
                        {
                            // Ошибка: неправильный пароль
                            errorData.Text = "Неверный логин или пароль!";
                            errorData.Foreground = Brushes.Red;
                        }
                    }
                    else
                    {
                        // Ошибка: пользователь с таким логином не найден
                        errorData.Text = "Неверный логин или пароль!";
                        errorData.Foreground = Brushes.Red;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}