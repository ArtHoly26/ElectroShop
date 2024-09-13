using System.Configuration;
using System.Windows;
using System.Windows.Media;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Windows.Controls;

namespace CourseProject
{
    public partial class WindowRegistration : Window
    {
        public string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        public WindowRegistration()
        {
            InitializeComponent();
            this.DataContext = new UserViewModel();
        }
        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();
        }
        private void Button_Click_Registration(object sender, RoutedEventArgs e)
        {
            UserViewModel viewModel = DataContext as UserViewModel;
            if (viewModel == null)
            {
                return;
            }

            var validationContext = new ValidationContext(viewModel.User, serviceProvider: null, items: null);
            var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            bool isValid = Validator.TryValidateObject(viewModel.User, validationContext, validationResults, validateAllProperties: true);

            if (isValid)
            {
                UserSaveData();
                ResetErrorColors();
            }
            else
            {
                ResetErrorColors();

                foreach (var result in validationResults)
                {
                    if (result.MemberNames.Contains(nameof(User.FirstName)))
                    {
                        userFirstNameError.Text = result.ErrorMessage;
                        userFirstNameError.Foreground = Brushes.Red;
                    }
                    if (result.MemberNames.Contains(nameof(User.LastName)))
                    {
                        userLastNameError.Text = result.ErrorMessage;
                        userLastNameError.Foreground = Brushes.Red;
                    }
                    if (result.MemberNames.Contains(nameof(User.Email)))
                    {
                        userEmailError.Text = result.ErrorMessage;
                        userEmailError.Foreground = Brushes.Red;
                    }
                    if (result.MemberNames.Contains(nameof(User.Country)))
                    {
                        userCountryError.Text = result.ErrorMessage;
                        userCountryError.Foreground = Brushes.Red;
                    }
                    if (result.MemberNames.Contains(nameof(User.Login)))
                    {
                        userLoginError.Text = result.ErrorMessage;
                        userLoginError.Foreground = Brushes.Red;
                    }
                    if (result.MemberNames.Contains(nameof(User.Password)))
                    {
                        userPasswordError.Text = result.ErrorMessage;
                        userPasswordError.Foreground = Brushes.Red;
                    }
                }
            }
        }
        private void userPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;

            UserViewModel viewModel = DataContext as UserViewModel;

            if (viewModel != null)
            {
                viewModel.User.Password = passwordBox.Password;
            }
        }
        private void UserSaveData()
        {
            string firstName = userFirstName.Text;
            string lastName = userLastName.Text;
            string email = userEmail.Text;
            string country = userCountry.Text;
            DateTime selectedDate = datePicker.SelectedDate ?? DateTime.MinValue;
            string login = userLogin.Text;
            string password = userPassword.Password;
            string passwordRepeat = userPasswordRepeat.Password;

            if (string.IsNullOrWhiteSpace(firstName) ||
                string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(login) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(passwordRepeat))
            {
                errorDataRepeat.Text = "All fields must be filled!";
                errorDataRepeat.Foreground = Brushes.Red;
                return;
            }

            if (password != passwordRepeat)
            {
                errorDataRepeat.Text = "Passwords don't match!";
                errorDataRepeat.Foreground = Brushes.Red;
                return;
            }

            string hashedPassword = HashPassword(password);

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Users (FirstName, LastName, Email, Login, Password, DateOfBirth, Country)" +
                                   "VALUES (@FirstName, @LastName, @Email, @Login, @Password, @DateOfBirth, @Country)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", firstName);
                        command.Parameters.AddWithValue("@LastName", lastName);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Login", login);
                        command.Parameters.AddWithValue("@Password", hashedPassword);
                        command.Parameters.AddWithValue("@DateOfBirth", selectedDate);
                        command.Parameters.AddWithValue("@Country", country);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            successfulRegistration.Text = "Registration was successful!";
                            successfulRegistration.Foreground = Brushes.GreenYellow;
                        }
                        else
                        {
                            successfulRegistration.Text = "Registration failed.";
                            successfulRegistration.Foreground = Brushes.Red;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        private void ResetErrorColors()
        {
            userFirstNameError.Text = string.Empty;
            userLastNameError.Text = string.Empty;
            userEmailError.Text = string.Empty;
            userCountryError.Text = string.Empty;
            userLoginError.Text = string.Empty;
            userPasswordError.Text = string.Empty;

            userFirstNameError.Foreground = Brushes.Transparent;
            userLastNameError.Foreground = Brushes.Transparent;
            userEmailError.Foreground = Brushes.Transparent;
            userCountryError.Foreground = Brushes.Transparent;
            userLoginError.Foreground = Brushes.Transparent;
            userPasswordError.Foreground = Brushes.Transparent;
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
