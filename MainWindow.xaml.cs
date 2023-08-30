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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        
         
            Icon = new BitmapImage(new Uri("./Images/logo.png", UriKind.Relative));

            this.SizeToContent = SizeToContent.Height;

        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F && Keyboard.Modifiers == ModifierKeys.Control)
            {
                Fahim.Visibility = System.Windows.Visibility.Visible;
                e.Handled = true;
            }
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void simButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void advButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void setButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void savButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void loaButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void senButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
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
            WindowState = WindowState.Minimized;
        }
    }
}