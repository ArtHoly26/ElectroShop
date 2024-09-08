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
            UserViewModel userViewModel = new UserViewModel();

            string loginToCheck = userLoginToCheck.Text;
            string passwordToCheck = userPasswordToCheck.Password;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT ID, Password FROM Users WHERE Login=@Login";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Login", loginToCheck);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int userId = reader.GetInt32(0);
                                string hashedPasswordFromDatabase = reader.GetString(1);

                                // Хешируем введенный пароль
                                string hashedPasswordToCheck;
                                using (SHA256 sha256 = SHA256.Create())
                                {
                                    byte[] hashedBytesToTextBox = sha256.ComputeHash(Encoding.UTF8.GetBytes(passwordToCheck));
                                    hashedPasswordToCheck = BitConverter.ToString(hashedBytesToTextBox).Replace("-", "").ToLower();
                                }

                                // Сравниваем хеши
                                if (hashedPasswordToCheck == hashedPasswordFromDatabase)
                                {
                                    // Закрываем reader перед выполнением новой операции
                                    reader.Close();

                                    // Пароль совпадает, получаем остальные данные пользователя
                                    string query1 = "SELECT ID, FirstName, LastName, Email, Login, Password, DateOfBirth, Country FROM Users WHERE ID=@UserId";

                                    using (SqlCommand command1 = new SqlCommand(query1, connection))
                                    {
                                        command1.Parameters.AddWithValue("@UserId", userId);

                                        using (SqlDataReader reader1 = command1.ExecuteReader())
                                        {
                                            if (reader1.Read())
                                            {
                                                userViewModel = new UserViewModel
                                                {
                                                    User = new User
                                                    {
                                                        Id = reader1.GetInt32(reader1.GetOrdinal("Id")),
                                                        FirstName = reader1["FirstName"].ToString(),
                                                        LastName = reader1["LastName"].ToString(),
                                                        Email = reader1["Email"].ToString(),
                                                        Login = reader1["Login"].ToString(),
                                                        Password = reader1["Password"].ToString(),
                                                        DateOfBirth = Convert.ToDateTime(reader1["DateOfBirth"]),
                                                        Country = reader1["Country"].ToString()
                                                    }
                                                };
                                            }
                                        }
                                    }

                                    // Открываем главное меню
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
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}