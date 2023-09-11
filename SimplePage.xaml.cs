using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
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
            //string DistVal; PoC
            //DistVal = DistanceTextBox.Text; PoC
            txtDebug.Inlines.Add(new Run("Distance\n")); //+ DistVal)); PoC
            Distance.Visibility = System.Windows.Visibility.Hidden;
            DistanceTextBox.Visibility = System.Windows.Visibility.Visible;
        }

        private void SCheckN(object sender, RoutedEventArgs e)
        {
            
        }

        private void UCheckedY(object sender, RoutedEventArgs e)
        {
            //txtDebug.Text = "Initial Velocity";
            txtDebug.Inlines.Add(new Run("Initial Velocity\n"));
            InitialVelocity.Visibility = System.Windows.Visibility.Hidden;
            InitialVelocityTextBox.Visibility = System.Windows.Visibility.Visible;
        }

        private void UCheckN(object sender, RoutedEventArgs e)
        {

        }
        private void VCheckedY(object sender, RoutedEventArgs e)
        {
            txtDebug.Inlines.Add(new Run("Final Velocity\n"));
            FinalVelocity.Visibility = System.Windows.Visibility.Hidden;
            FinalVelocityTextBox.Visibility = System.Windows.Visibility.Visible;
        }

        private void VCheckN(object sender, RoutedEventArgs e)
        {

        }
        private void ACheckedY(object sender, RoutedEventArgs e)
        {
            txtDebug.Inlines.Add(new Run("Acceleration\n"));
            Acceleration.Visibility = System.Windows.Visibility.Hidden;
            AccelerationTextBox.Visibility = System.Windows.Visibility.Visible;
        }

        private void ACheckN(object sender, RoutedEventArgs e)
        {

        }
        private void TCheckedY(object sender, RoutedEventArgs e)
        {
            txtDebug.Inlines.Add(new Run("Time\n"));
            Time.Visibility = System.Windows.Visibility.Hidden;
            TimeTextBox.Visibility = System.Windows.Visibility.Visible;
        }

        private void TCheckN(object sender, RoutedEventArgs e)
        {
  
        }

        private void fahButton_Click(object sender, RoutedEventArgs e)
        {
            FahimPics page = new FahimPics();
            this.NavigationService.Navigate(page);
        }
        private void calcButton_Click(object sender, RoutedEventArgs e)
        {
            noinputPopup.IsOpen = true;

        }
        private void revarButton_Click(object sender, RoutedEventArgs e)
        {
            txtDebug.Text = null;
            Distance.Visibility = System.Windows.Visibility.Visible;
            Distance.IsChecked = false;
            InitialVelocity.Visibility = System.Windows.Visibility.Visible;
            InitialVelocity.IsChecked = false;
            FinalVelocity.Visibility = System.Windows.Visibility.Visible;
            FinalVelocity.IsChecked = false;
            Acceleration.Visibility = System.Windows.Visibility.Visible;
            Acceleration.IsChecked = false;
            Time.Visibility = System.Windows.Visibility.Visible;
            Time.IsChecked = false;

            DistanceTextBox.Visibility = System.Windows.Visibility.Hidden;
            DistanceTextBox.Text = null;
            InitialVelocityTextBox.Visibility = System.Windows.Visibility.Hidden;
            InitialVelocityTextBox.Text = null;
            FinalVelocityTextBox.Visibility = System.Windows.Visibility.Hidden;
            FinalVelocityTextBox.Text = null;
            AccelerationTextBox.Visibility = System.Windows.Visibility.Hidden;
            AccelerationTextBox.Text = null;
            TimeTextBox.Visibility = System.Windows.Visibility.Hidden;
            TimeTextBox.Text = null;
        }
    }
}