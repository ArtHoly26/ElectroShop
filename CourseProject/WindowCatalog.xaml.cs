using System;
using System.Collections.Generic;
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
    
    public partial class WindowCatalog : Window
    {
        private UserViewModel userViewModel;
        public WindowCatalog()
        {
            InitializeComponent();
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

            WindowMainMenu windowMainMenu = new WindowMainMenu(userViewModel);
            windowMainMenu.Show();
            this.Close();
        }
    }
}
