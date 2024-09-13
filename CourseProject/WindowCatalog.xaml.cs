using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CourseProject
{
    public partial class WindowCatalog : Window
    {
        private CatalogViewModel _catalogViewModel;
        public WindowCatalog(UserViewModel userViewModel)
        {
            InitializeComponent();
            _catalogViewModel = new CatalogViewModel(userViewModel);
            DataContext = _catalogViewModel;
        }

        private void AddToFavorites_Click(object sender, RoutedEventArgs e)
        {
            DataGrid dataGrid = ProductDataGrid;

            if (dataGrid.SelectedItem is Product selectedProduct)
            {
                
                if (_catalogViewModel.UserViewModel != null)
                {
                    AddProductToFavoritesAsync(selectedProduct, _catalogViewModel.UserViewModel.CurrentUserId);
                }
                else
                {
                    errors.Text = "UserViewModel is not set.";
                    errors.Foreground = Brushes.Red;
                }
            }
            else
            {
                errors.Text = "Please select a product first.";
                errors.Foreground = Brushes.Red;
            }
        }
        private async Task<bool> AddProductToFavoritesAsync(Product product, int userId)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string checkQuery = @"SELECT COUNT(*) FROM Favorites WHERE ProductsId = @ProductsId AND UsersId = @UsersId";

                    using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@ProductsId", product.Id);
                        checkCommand.Parameters.AddWithValue("@UsersId", userId);

                        int exists = (int)await checkCommand.ExecuteScalarAsync();

                        if (exists > 0)
                        {
                            errors.Text = "His product is already in favorites..";
                            errors.Foreground = Brushes.Red;
                            return false; 
                        }
                    }

                    string insertQuery = @"INSERT INTO Favorites (ProductsId, UsersId) VALUES (@ProductsId, @UsersId)";

                    using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@ProductsId", product.Id);
                        insertCommand.Parameters.AddWithValue("@UsersId", userId);

                        int rowsAffected = await insertCommand.ExecuteNonQueryAsync();
                        errors.Text = "The product has been added to favorites";
                        errors.Foreground = Brushes.GreenYellow;
                        return rowsAffected > 0; 
                    }
                }
            }

            catch (SqlException sqlEx)
            {
                errors.Text = "SQL error when adding a product to favorites";
                errors.Foreground = Brushes.Red;
                return false;
            }
            catch (Exception ex)
            {
                errors.Text = "Error when adding a product to favorites";
                errors.Foreground = Brushes.Red;
                return false;
            }
        }
        private void CategorySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is CatalogViewModel viewModel)
            {
                viewModel.LoadProducts();
            }
        }
        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            WindowMainMenu windowMainMenu = new WindowMainMenu(_catalogViewModel.UserViewModel);
            windowMainMenu.Show();
            this.Close();   
        }
  
    }
}
