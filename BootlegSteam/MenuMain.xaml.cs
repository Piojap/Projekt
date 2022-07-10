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
        public MenuMain()
        {
            InitializeComponent();
        }

        private void clkmenuplayer(object sender, RoutedEventArgs e)
        {
            MenuPlayer open = new MenuPlayer();
            this.Visibility = Visibility.Hidden;
            open.Show();
        }

        private void clkmenugame(object sender, RoutedEventArgs e)
        {
            MenuGame open = new MenuGame();
            this.Visibility = Visibility.Hidden;
            open.Show();
        }

        private void clkmenudev(object sender, RoutedEventArgs e)
        {
            MenuDev open = new MenuDev();
            this.Visibility = Visibility.Hidden;
            open.Show();
        }

        private void clkclose(object sender, RoutedEventArgs e)
        {
            Close();
            App.Current.Shutdown();
        }

        private void clkmaximize(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
            else if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
        }

        private void clkminimize(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void dragwindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
