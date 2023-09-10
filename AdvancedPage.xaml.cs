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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace test_2
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class AdvancedPage : Page
    {
        public AdvancedPage()
        {
            InitializeComponent();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F && Keyboard.Modifiers == ModifierKeys.Control)
            {
                Fahim.Visibility = System.Windows.Visibility.Visible;
                e.Handled = true;
            }
        }

        private void simButton_Click(object sender, RoutedEventArgs e)
        {
            SimplePage page = new SimplePage();
            this.NavigationService.Navigate(page);
        }

        private void advButton_Click(object sender, RoutedEventArgs e)
        {
            AdvancedPage page = new AdvancedPage();
            this.NavigationService.Navigate(page);
        }

        private void setButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void savButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void loaButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void senButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void fahButton_Click(object sender, RoutedEventArgs e)
        {
            FahimPics page = new FahimPics();
            this.NavigationService.Navigate(page);
        }
    }
}