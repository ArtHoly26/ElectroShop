using System.Windows;
using System.Windows.Input;


namespace CourseProject
{
    
    public partial class WindowMainMenu : Window
    {
        private UserViewModel userViewModel;
        public WindowMainMenu(UserViewModel userViewModel)
        {
            InitializeComponent();
            this.userViewModel = userViewModel;
            DataContext = this.userViewModel;
        }
        private void Button_Click_Logout(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
        private void Button_Click_Account(object sender, RoutedEventArgs e)
        {
            WindowAccount windowAccount= new WindowAccount(userViewModel);
            windowAccount.Show();
            this.Close();
        }
        private void Button_Click_Catalog(object sender, RoutedEventArgs e)
        {
            WindowCatalog windowCatalog = new WindowCatalog();
            windowCatalog.Show();
            this.Close();
        }
    }
}
