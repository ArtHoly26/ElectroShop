using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Windows;

namespace CourseProject
{
    public class FavoritesViewModel
    {
        public ObservableCollection<Product> Favorites { get; set; } = new ObservableCollection<Product>();

        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        private int _currentUserId;

        public FavoritesViewModel(int userId)
        {
            _currentUserId = userId;
            LoadFavorites();
        }

        public async void LoadFavorites()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    string query = @"SELECT p.Id, p.Name, p.ImagePath, p.Price, p.Weight, p.Description, p.CategoryId 
                                 FROM Favorites f
                                 JOIN Products p ON f.ProductsId = p.Id
                                 WHERE f.UsersId = @UserId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", _currentUserId);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            Favorites.Clear();

                            while (await reader.ReadAsync())
                            {
                                Product product = new Product
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    ImagePath = reader.GetString(2),
                                    Price = reader.GetDecimal(3),
                                    Weight = reader.GetDouble(4),
                                    Description = reader.GetString(5),
                                    CategoryId = reader.GetInt32(6)
                                };

                                Favorites.Add(product);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки избранного: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
