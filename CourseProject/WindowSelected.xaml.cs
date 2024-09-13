using System.Data.SqlClient;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace CourseProject
{
    public partial class WindowSelected : Window
    {
        private UserViewModel userViewModel;
        private Product _selectedProduct;

        public WindowSelected(UserViewModel userViewModel)
        {
            InitializeComponent();
            this.userViewModel = userViewModel;
            FavoritesViewModel viewModel = new FavoritesViewModel(userViewModel.User.Id);
            DataContext = viewModel;
        }
        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            WindowMainMenu windowMainMenu = new WindowMainMenu(userViewModel);
            windowMainMenu.Show();
            this.Close();
        }
        private void ProductDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedProduct = ProductDataGrid.SelectedItem as Product;
        }
        private async void Button_Click_Delete(object sender, RoutedEventArgs e)
        {
            if (_selectedProduct != null)
            {
                await DeleteProductFromFavoritesAsync(_selectedProduct.Id);
                RefreshFavoritesList();
            }
            else
            {
                errors.Text = "Please select the product to remove!";
                errors.Foreground = Brushes.Red;
            }
        }
        private async Task DeleteProductFromFavoritesAsync(int productId)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    string query = @"DELETE FROM Favorites WHERE ProductsId = @ProductId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add(new SqlParameter("@ProductId", SqlDbType.Int) { Value = productId });
                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                        {
                            errors.Text = "The product has been successfully deleted!";
                            errors.Foreground = Brushes.GreenYellow;
                        }
                        else
                        {
                            errors.Text = "The product could not be deleted from favorites!";
                            errors.Foreground = Brushes.Red;
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                errors.Text = "SQL error when deleting a product from favorites!";
                errors.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {
                errors.Text = "An error occurred when deleting a product from favorites!";
                errors.Foreground = Brushes.Red;
            }
        }
        private void RefreshFavoritesList()
        {
            if (DataContext is FavoritesViewModel viewModel)
            {
                viewModel.LoadFavorites(); 
            }
        }
    }
}
