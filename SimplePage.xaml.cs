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
    public partial class SimplePage : Page
    {

        public SimplePage()
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

        private void SCheckedY(object sender, RoutedEventArgs e)
        {
            txtDebug.Inlines.Add(new Run("Distance\n"));
        }

        private void SCheckN(object sender, RoutedEventArgs e)
        {
            txtDebug.Text = null;
        }

        private void UCheckedY(object sender, RoutedEventArgs e)
        {
            //txtDebug.Text = "Initial Velocity";
            txtDebug.Inlines.Add(new Run("Initial Velocity\n"));
        }

        private void UCheckN(object sender, RoutedEventArgs e)
        {
            txtDebug.Text = null;
        }
        private void VCheckedY(object sender, RoutedEventArgs e)
        {
            txtDebug.Inlines.Add(new Run("Final Velocity\n"));
        }

        private void VCheckN(object sender, RoutedEventArgs e)
        {
            txtDebug.Text = null;
        }
        private void ACheckedY(object sender, RoutedEventArgs e)
        {
            txtDebug.Inlines.Add(new Run("Acceleration\n"));
        }

        private void ACheckN(object sender, RoutedEventArgs e)
        {
            txtDebug.Text = null;
        }
        private void TCheckedY(object sender, RoutedEventArgs e)
        {
            txtDebug.Inlines.Add(new Run("Time\n"));
        }

        private void TCheckN(object sender, RoutedEventArgs e)
        {
            txtDebug.Text = null;
        }

        private void fahButton_Click(object sender, RoutedEventArgs e)
        {
            FahimPics page = new FahimPics();
            this.NavigationService.Navigate(page);
        }
    }
}