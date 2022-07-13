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

namespace BootlegSteam
{
    /// <summary>
    /// Interaction logic for MenuMain.xaml
    /// </summary>
    public partial class MenuMain : Window
    {
        /// <summary>
        /// WPF Elements initialization
        /// </summary>
        public MenuMain()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Logic for opening Players window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clkmenuplayer(object sender, RoutedEventArgs e)
        {
            MenuPlayer open = new MenuPlayer();
            this.Visibility = Visibility.Hidden;
            open.Show();
        }

        /// <summary>
        /// Logic for opening Games window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clkmenugame(object sender, RoutedEventArgs e)
        {
            MenuGame open = new MenuGame();
            this.Visibility = Visibility.Hidden;
            open.Show();
        }

        /// <summary>
        /// Logic for opening Devs window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clkmenudev(object sender, RoutedEventArgs e)
        {
            MenuDev open = new MenuDev();
            this.Visibility = Visibility.Hidden;
            open.Show();
        }

        /// <summary>
        /// Logic for closing the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clkclose(object sender, RoutedEventArgs e)
        {
            Close();
            App.Current.Shutdown();
        }

        /// <summary>
        /// Logic for maximizing/windowed the application window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clkmaximize(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
            else if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
        }

        /// <summary>
        /// Logic for minimizing the application window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clkminimize(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Logic allowing dragging the application window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dragwindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
