using System.Configuration;
using System.Windows;
using System.Windows.Media;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace CourseProject
{
    public partial class WindowRegistration : Window
    {
        public string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        public WindowRegistration()
        {
            InitializeComponent();
            UserViewModel userViewModel= new UserViewModel();
            DataContext = userViewModel;
        }
        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();
        }
        private void Button_Click_Registration(object sender, RoutedEventArgs e)
        {
            string firstName = userFirstName.Text;
            string lastName = userLastName.Text;
            string email = userEmail.Text;
            string country = userCountry.Text;
            DateTime selectedDate = datePicker.SelectedDate ?? DateTime.MinValue;
            string login = userLogin.Text;
            string password = userPassword.Password;
            string passwordRepeat = userPasswordRepeat.Password;

            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(login) ||
                string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(passwordRepeat))
            {
                errorData.Text = "Все поля должны быть заполнены!";
                errorData.Foreground = Brushes.Red;
                return;
            }

            if (password != passwordRepeat)
            {
                errorData.Text = "Пароли не совпадают!";
                errorData.Foreground = Brushes.Red;
                return;
            }

            string hashedPassword = HashPassword(password);

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Users (FirstName,LastName,Email,Login,Password,DateOfBirth,Country)" +
                                       "VALUES (@Value1, @Value2, @Value3, @Value4, @Value5, @Value6, @Value7)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Value1", firstName);
                        command.Parameters.AddWithValue("@Value2", lastName);
                        command.Parameters.AddWithValue("@Value3", email);
                        command.Parameters.AddWithValue("@Value4", login);
                        command.Parameters.AddWithValue("@Value5", hashedPassword);
                        command.Parameters.AddWithValue("@Value6", selectedDate);
                        command.Parameters.AddWithValue("@Value7", country);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            errorData.Text = "Регистрация прошла успешно!";
                            errorData.Foreground = Brushes.GreenYellow;
                        }

                        else
                        {
                            errorData.Text = "Ошибка при вводе данных!";
                            errorData.Foreground = Brushes.Red;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var builder = new StringBuilder();
                foreach (var t in bytes)
                {
                    builder.Append(t.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }  
}
