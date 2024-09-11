using System.Windows;


namespace CourseProject
{
    public partial class WindowSelected : Window
    {
        private UserViewModel userViewModel;
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
    }
}
