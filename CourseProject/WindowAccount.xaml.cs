using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;
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

namespace CourseProject
{
    public partial class WindowAccount : Window
    {
        private UserViewModel userViewModel;
        private string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        public WindowAccount(UserViewModel userViewModel)
        {
            InitializeComponent();
            this.userViewModel = userViewModel;
            DataContext = this.userViewModel;
        }

        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            WindowMainMenu windowMainMenu = new WindowMainMenu(userViewModel);
            windowMainMenu.Show();
            this.Close();
        }

        private void Button_Click_ChangedTheData(object sender, RoutedEventArgs e)
        {
            string newFirstName = userFirstName.Text;
            string newLastName = userLastName.Text;
            string newEmail = userEmail.Text;
            string newCountry = userCountry.Text;
            DateTime newDateOfBirth = userViewModel.User.DateOfBirth; // Используем старую дату по умолчанию
            string newLogin = userLogin.Text;

            if (datePicker.SelectedDate.HasValue)
            {
                newDateOfBirth = datePicker.SelectedDate.Value;
            }
            else if (!string.IsNullOrWhiteSpace(userDateofBirth.Text))
            {
                if (DateTime.TryParse(userDateofBirth.Text, out DateTime parsedDate))
                {
                    if (parsedDate >= SqlDateTime.MinValue.Value && parsedDate <= SqlDateTime.MaxValue.Value)
                    {
                        newDateOfBirth = parsedDate;
                    }
                    else
                    {
                        MessageBox.Show("Указана недопустимая дата.");
                        return;
                    }
                }
            }

            // Дополнительно проверяем на допустимый диапазон
            if (newDateOfBirth < SqlDateTime.MinValue.Value || newDateOfBirth > SqlDateTime.MaxValue.Value)
            {
                MessageBox.Show("Дата рождения должна быть в диапазоне от 1/1/1753 до 12/31/9999.");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string updateSql = "UPDATE Users SET FirstName = @FirstName, LastName = @LastName, Email=@Email, Country=@Country, DateOfBirth=@DateOfBirth, Login=@Login WHERE ID = @Id";

                    using (SqlCommand command = new SqlCommand(updateSql, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", newFirstName);
                        command.Parameters.AddWithValue("@LastName", newLastName);
                        command.Parameters.AddWithValue("@Email", newEmail);
                        command.Parameters.AddWithValue("@Country", newCountry);
                        command.Parameters.AddWithValue("@DateOfBirth", newDateOfBirth);
                        command.Parameters.AddWithValue("@Login", newLogin);
                        command.Parameters.AddWithValue("@Id", userViewModel.User.Id);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            errorChangeData.Text = "Данные успешно изменены";
                            errorChangeData.Foreground = Brushes.GreenYellow;
                        }
                        else
                        {
                            errorChangeData.Text = "Ошибка изменения данных";
                            errorChangeData.Foreground = Brushes.Red;
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
    

